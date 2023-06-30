using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.Events;

namespace Core.UI
{
    public class UpgradeUI : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text descText;
        [SerializeField] private UnityEvent onClicked;

        public UnityEvent OnClicked => onClicked;


        public void Render(Sprite icon, int level, string title, string desc)
        {
            iconImage.sprite = icon;
            levelText.text = level.ToString();
            titleText.text = title;
            descText.text = desc;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            onClicked.Invoke();
        }
    }
}