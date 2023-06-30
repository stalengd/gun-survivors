using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core.Upgrades;
using TMPro;

namespace Core.UI
{
    public class UpgradesListItem : MonoBehaviour
    {
        [SerializeField] private Image iconImage;
        [SerializeField] private TMP_Text levelText;


        public void Init(UpgradesState.Upgrade upgrade)
        {
            iconImage.sprite = upgrade.Type.Icon;
            levelText.text = upgrade.Level.ToString();
        }
    }
}