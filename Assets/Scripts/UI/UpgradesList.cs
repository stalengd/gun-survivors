using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stalen.DI;
using Utils.UI;
using Core.Upgrades;
using System.Linq;

namespace Core.UI
{
    public class UpgradesList : MonoBehaviour
    {
        [SerializeField] private ListHelper<UpgradesListItem> listView;

        private Inject.Lazy<UpgradesState> upgradesService;


        private void Start()
        {
            upgradesService.Value.OnUpgrade.AddListener(Upgraded);
        }

        private void OnDestroy()
        {
            upgradesService.Value.OnUpgrade.RemoveListener(Upgraded);
        }


        private void Upgraded(UpgradesState.Upgrade upgrade)
        {
            listView.SetElements(upgradesService.Value.Upgrades.Where(u => u.Level > 0), InitItem);
        }

        private void InitItem(UpgradesState.Upgrade upgrade, UpgradesListItem item)
        {
            item.Init(upgrade);
        }
    }
}