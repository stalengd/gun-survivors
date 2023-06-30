using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace Stalen.DI
{
    public class ContainersWindow : EditorWindow
    {
        [SerializeField] private TreeViewState treeViewState;

        private ServicesTreeView servicesTreeView;


        [MenuItem("Window/DI Services")]
        public static void ShowWindow()
        {
            GetWindow<ContainersWindow>("DI Services");
        }


        private void OnEnable()
        {
            if (treeViewState == null)
                treeViewState = new TreeViewState();

            servicesTreeView = new ServicesTreeView(treeViewState, Containers.Instances);
        }

        private void OnGUI()
        {
            servicesTreeView.Reload();
            servicesTreeView.OnGUI(new Rect(0, 0, position.width, position.height));
            var allSelection = servicesTreeView.GetSelection();
            if (allSelection.Count == 0) return;

            var selectionId = allSelection[0];
        }


        private class ServicesTreeView : TreeView
        {
            public IEnumerable<Container> Containers { get; set; }
            public IdMap<Container> ContainersMap { get; }
            public IdMap<object> ServicesMap { get; }

            private IdPool idPool;

            public ServicesTreeView(TreeViewState treeViewState, IEnumerable<Container> containers) : base(treeViewState)
            {
                Containers = containers;

                idPool = new IdPool();
                ContainersMap = new IdMap<Container>(idPool);
                ServicesMap = new IdMap<object>(idPool);

                Reload();
            }

            protected override TreeViewItem BuildRoot()
            {
                var root = new TreeViewItem { id = 0, depth = -1, displayName = "Root" };


                foreach (var container in Containers)
                {
                    var containerItem = CreateContainerItem(container);
                    root.AddChild(containerItem);

                    foreach (var service in container.GetServices())
                    {
                        var serviceItem = CreateServiceItem(service);
                        containerItem.AddChild(serviceItem);
                    }
                }

                if (!root.hasChildren)
                    root.AddChild(new TreeViewItem { id = 1, displayName = "--- No containers ---" });

                SetupDepthsFromParentsAndChildren(root);

                return root;
            }


            private ContainerItem CreateContainerItem(Container container)
            {
                var activeTime = System.DateTime.Now - container.ActivationTime;

                return new ContainerItem
                {
                    Container = container,
                    id = ContainersMap.GetId(container),
                    displayName = $"{container?.Name ?? "Null"} [{activeTime:mm'm 'ss's'}]",
                    icon = ContainerIcons.MainIcon
                };
            }

            private ServiceItem CreateServiceItem(object service)
            {
                string name;
                if (service is IService typedService)
                {
                    name = typedService.Name;
                }
                else
                {
                    name = service.GetType().Name;
                }
                name = name.Replace("Service", "");

                return new ServiceItem
                {
                    Service = service,
                    id = ServicesMap.GetId(service),
                    displayName = name
                };
            }


            private class ContainerItem : TreeViewItem
            {
                public Container Container { get; set; }
            }

            private class ServiceItem : TreeViewItem
            {
                public object Service { get; set; }
            }
        }


        private class IdPool
        {
            private int nextId = 1;

            public int DrawId()
            {
                return nextId++;
            }
        }

        private class IdMap<T>
        {
            private Dictionary<int, T> idToItem = new Dictionary<int, T>();
            private Dictionary<T, int> itemToId = new Dictionary<T, int>();
            private IdPool pool;


            public IdMap(IdPool pool)
            {
                this.pool = pool;
            }


            public T GetItem(int id)
            {
                return idToItem[id];
            }

            public bool TryGetItem(int id, out T item)
            {
                return idToItem.TryGetValue(id, out item);
            }

            public int GetId(T item)
            {
                if (itemToId.TryGetValue(item, out int id))
                    return id;

                return RegisterItem(item);
            }


            private int RegisterItem(T item)
            {
                var id = pool.DrawId();

                idToItem.Add(id, item);
                itemToId.Add(item, id);

                return id;
            }
        }
    }
}