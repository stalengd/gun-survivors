using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Stalen.DI
{
    public static class Inject 
    {
        public static T ToNew<T>(GameObject prefab, System.Action<DependencyBuilder<T>> configure)
        {
            var go = Object.Instantiate(prefab);
            var component = go.GetComponent<T>();
            var builder = new DependencyBuilder<T>(component);
            configure(builder);
            return component;
        }

        public static T Here<T>() where T : class
        {
            return Containers.GetService<T>();
        }

        public static void Out<T>(out T result) where T : class
        {
            result = Containers.GetService<T>();
        }


        public struct Lazy<T> where T : class
        {
            public T Value
            {
                get
                {
                    if (value == null)
                        value = Here<T>();

                    return value;
                }
            }
            private T value;
        }
    }
}