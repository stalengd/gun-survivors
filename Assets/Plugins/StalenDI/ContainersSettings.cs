using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Stalen.DI
{
    [CreateAssetMenu(fileName = "ContainersSettings", menuName = "Containers Settings")]
    public class ContainersSettings : ScriptableObject
    {
        [field: SerializeField] 
        public MonoContainer RootProjectContainer { get; set; }


        private static ContainersSettings instance;
        public static ContainersSettings Instance 
        {
            get
            {
                if (instance == null)
                    LoadInstance();

                return instance;
            }
        }

        private static void LoadInstance()
        {
            var assets = Resources.LoadAll<ContainersSettings>("");
            if (!assets.Any())
            {
                Debug.Log("Containers Settings asset not found. Consider create one anywhere in Resources folder.");
                instance = CreateInstance<ContainersSettings>();
                return;
            }
            else if (assets.Length > 1)
            {
                Debug.Log("There are several instances of Containers Settins.");
            }

            instance = assets[0];
        }
    }
}
