using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace Core.UI
{
    public class InteractionColorTransition : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private float duration = 0.5f;
        [SerializeField] private TransitionTarget[] targets;



        [System.Serializable]
        private struct TransitionTarget
        {
            public Graphic graphic;
            public Color defaultColor;
            public Color hoverColor;
            public Color pressedColor;
        }


        private void OnEnable()
        {
            for (int i = 0; i < targets.Length; i++)
            {
                var target = targets[i];
                target.graphic.color = target.defaultColor;
            }
        }


        public void OnPointerDown(PointerEventData eventData)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                var target = targets[i];
                target.graphic.DOColor(target.pressedColor, duration);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                var target = targets[i];
                target.graphic.DOColor(target.hoverColor, duration);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            for (int i = 0; i < targets.Length; i++)
            {
                var target = targets[i];
                target.graphic.DOColor(target.defaultColor, duration);
            }
        }
    }
}