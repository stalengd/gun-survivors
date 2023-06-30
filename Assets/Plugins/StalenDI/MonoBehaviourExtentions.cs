using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Stalen.DI
{
    public static class MonoBehaviourExtentions
    {
        public static T Instantiate<T>(this MonoBehaviour monoBehaviour, GameObject original) where T : Component
        {
            return Object.Instantiate(original).GetComponent<T>();
        }
    }
}
