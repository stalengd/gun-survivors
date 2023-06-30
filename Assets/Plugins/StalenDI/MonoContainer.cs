using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Stalen.DI
{
    public class MonoContainer : MonoBehaviour, IService
    {
        [SerializeField] private DefaultScope scope;
        [SerializeField] private ScopeMode scopeMode;
        [SerializeField] private bool preventDublicatesByName = false;

        [Space]
        [SerializeField] private Component[] explicitServiceComponents;
        [SerializeField] private ScriptableObject[] scriptableObjectsServices;

        public string Name => name;
        public MonoContainer Parent
        {
            get => parent;
            set
            {
                parent = value;
                if (parent != null)
                    transform.parent = parent.transform;
                else
                    transform.parent = null;
            }
        }

        public Container Container { get; private set; }

        private MonoContainer parent;
        private MonoContainer copy;

        [System.Serializable]
        public enum DefaultScope
        {
            Singleton,
            Scene
        }

        [System.Serializable]
        public enum ScopeMode
        {
            Destroy,
            Reinit
        }

        public static MonoContainer RootContainer { get; private set; }
        private static List<MonoContainer> Instances { get; set; } = new List<MonoContainer>();


        protected virtual void Awake()
        {
            if (preventDublicatesByName && Instances.Any(c => c.name == name))
            {
                Destroy(gameObject);
                return;
            }

            SceneManager.sceneUnloaded += OnSceneUnloaded;
            SceneManager.sceneLoaded += OnSceneLoaded;
            Instances.Add(this);

            if (RootContainer != null && RootContainer != this)
                Parent = RootContainer;
            DontDestroyOnLoad(gameObject);

            if (scopeMode == ScopeMode.Reinit)
            {
                gameObject.SetActive(false);
                copy = Instantiate(gameObject, transform).GetComponent<MonoContainer>();
                gameObject.SetActive(true);
            }

            Container = new Container(name);

            var childServices = new List<IService>();
            GetComponentsInChildren(false, childServices);
            Container.AddServices(childServices);

            if (explicitServiceComponents != null)
            {
                foreach (var service in explicitServiceComponents)
                {
                    Container.AddService(service);
                }
            }
            if (scriptableObjectsServices != null)
            {
                foreach (var service in scriptableObjectsServices)
                {
                    Container.AddService(service);
                }
            }
        }

        private void OnDestroy()
        {
            if (Container != null)
            {
                DestroyContainer();
            }
        }


        public void DestroyContainer()
        {
            Container.Destroy();
            Container = null;
            Instances.Remove(this);
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Destroy(gameObject);

            if (scopeMode == ScopeMode.Reinit)
                Reinit();
        }

        public void ConfigureDependencies()
        {

        }


        private void OnSceneUnloaded(Scene scene)
        {
            if (scope == DefaultScope.Scene)
            {
                DestroyContainer();
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            foreach (var service in Container.GetServices())
            {
                if (service is IService s)
                {
                    s.ConfigureDependencies();
                }
            }
        }

        private void Reinit()
        {
            copy.gameObject.SetActive(true);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitRootContainer()
        {
            var containerPrefab = ContainersSettings.Instance.RootProjectContainer;
            MonoContainer container;
            
            if (containerPrefab != null)
            {
                container = Instantiate(containerPrefab);
            }
            else
            {
                container = new GameObject("Root Container")
                    .AddComponent<MonoContainer>();
            }

            RootContainer = container;
        }

        //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        //private static void ConfigureDependenciesInit()
        //{
        //    Debug.Log("ConfigureDependencies Init");

        //    foreach (var container in Instances)
        //    {
        //        foreach (var service in container.Container.GetServices())
        //        {
        //            if (service is IService s)
        //            {
        //                s.ConfigureDependencies();
        //            }
        //        }
                
        //    }
        //}
    }
}