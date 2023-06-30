using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Core
{
    [System.Serializable]
    public class ObservableValue<T>
    {
        [SerializeField] private T value;
        [SerializeField] private UnityEvent<T, T> onChanged = new();

        public T Value
        {
            get => value;
            set
            {
                SetValue(value);
            }
        }
        public UnityEvent<T, T> OnChanged => onChanged;

        private System.Func<T, T> setter;

        public ObservableValue(T value, System.Func<T, T> setter = null)
        {
            Value = value;
            this.setter = setter;
        }

        private void SetValue(T value)
        {
            var oldValue = this.value;
            if (setter != null)
            {
                value = setter(value);
            }

            this.value = value;
            onChanged.Invoke(oldValue, value);
        }



        public static implicit operator T(ObservableValue<T> observableValue)
        {
            return observableValue.Value;
        }
    }
}