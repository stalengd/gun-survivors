using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Data;

namespace Core.Upgrades
{
    [CreateAssetMenu(menuName = "Data/Upgrade")]
    public class UpgradeType : ScriptableObject
    {
        public string Id => name;
        public string Name => title;
        public string Description => description;
        public Sprite Icon => sprite;
        public IReadOnlyList<float> Values => sheet.Ref.Values;

        [SerializeField] private string title;
        [SerializeField] private string description;
        [SerializeField] private Sprite sprite;
        [SerializeField] private UpgradesSheet.Reference sheet;
    }
}