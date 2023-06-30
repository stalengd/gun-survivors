using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utils
{
    public struct LazyComponent<T>
    {
        public T Value
        {
            get
            {
                if (!isCached)
                {
                    cache = GameObject.GetComponent<T>();
                    isCached = true;
                }

                return cache;
            }
        }
        public GameObject GameObject { get; private set; }

        private T cache;
        private bool isCached;

        public LazyComponent(GameObject gameObject) : this()
        {
            GameObject = gameObject;
        }
    }
}
