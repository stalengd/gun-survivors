using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Utils.UI
{
    [System.Serializable]
    public struct ListHelper<T> where T : Component
    {
        [SerializeField] private RectTransform holder;
        [SerializeField] private T itemPrefab;


        public Transform this[int tabId]
        {
            get
            {
                return holder.GetChild(tabId);
            }
        }

        public int ElementsCount
        {
            get { return holder.childCount; }
        }


        public IEnumerable<T> GetElements()
        {
            return holder.OfType<Transform>().Select(t => t.GetComponent<T>());
        }

        public T CreateElement()
        {
            return Object.Instantiate(itemPrefab.gameObject, holder).GetComponent<T>();
        }

        public void CreateElements<FromT>(IEnumerable<FromT> fromCollection, System.Action<FromT, T> action)
        {
            foreach (var item in fromCollection)
            {
                var controller = CreateElement();
                action(item, controller);
            }
        }

        public void SetElements<FromT>(IEnumerable<FromT> fromCollection, System.Action<FromT, T> action)
        {
            // TODO: More effective way
            Clear();
            CreateElements(fromCollection, action);
        }


        public void Clear()
        {
            foreach (Transform element in holder)
            {
                Object.Destroy(element.gameObject);
            }
        }
    }
}