using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Stalen.DI
{
    public class DependencyInjectionConfig : ScriptableObject
    {
        public static ILogger Logger { get; set; } 

        private static DependencyInjectionConfig Instance { get; set; }


        private void OnEnable()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("There is should not be 2 or more instances of Dependency Injection Config");

                return;
            }

            Instance = this;
        }
    }
}
