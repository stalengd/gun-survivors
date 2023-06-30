using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stalen.DI
{
    public class MonoService : MonoBehaviour, IService
    {
        protected static ILogger Logger => DependencyInjectionConfig.Logger;

        public string Name => gameObject.name;

        public virtual void ConfigureDependencies() { }
    }
}