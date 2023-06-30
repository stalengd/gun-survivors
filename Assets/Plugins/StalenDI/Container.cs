using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Stalen.DI
{
    public class Container
    {
        public bool IsActive { get; private set; }
        public virtual string Name { get; } = "<Container>";
        public System.DateTime ActivationTime { get; private set; }

        protected static ILogger Logger => DependencyInjectionConfig.Logger;

        private List<object> Services { get; set; } = new List<object>();



        public Container(string name = null)
        {
            Containers.Instances.Add(this);
            IsActive = true;
            ActivationTime = System.DateTime.Now;
            Name = name ?? Name;
            Logger?.LogFormat(LogType.Log, "Container {0} added", Name);
        }


        public void Destroy()
        {
            CheckForActivity();
            Containers.Instances.Remove(this);
            IsActive = false;
            Logger?.LogFormat(LogType.Log, "Container {0} destroyed", Name);
        }


        public Container AddService<T>(T service) 
        {
            CheckForActivity();
            Services.Add(service);
            Logger?.LogFormat(LogType.Log, "Added service {0}", typeof(T).Name);
            return this;
        }

        public Container AddServices(List<IService> range)
        {
            CheckForActivity();
            Services.AddRange(range);
            foreach (var service in range)
            {
                //service.ConfigureDependencies();
                Logger?.LogFormat(LogType.Log, "Added service {0}", service.GetType().Name);
            }
            return this;
        }


        public T GetService<T>() where T : class
        {
            CheckForActivity();
            foreach (var service in Services)
            {
                if (service is T targetService)
                    return targetService;
            }

            return null;
        }

        public IReadOnlyList<object> GetServices()
        {
            return Services;
        }


        private void CheckForActivity()
        {
            if (IsActive) return;

            throw new System.InvalidOperationException("Container is not active, services can not be added or getted");
        }
    }

    public static class Containers 
    {
        public static List<Container> Instances { get; private set; } = new List<Container>();

        public static T GetService<T>() where T : class
        {
            foreach (var container in Instances)
            {
                var s = container.GetService<T>();
                if (s != null)
                    return s;
            }

            throw new System.Exception($"There are no service of type '{typeof(T).Name}'");
        }
    }
}