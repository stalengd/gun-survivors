using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using BrunoMikoski.AnimationSequencer;
using Stalen.DI;
using Core.Upgrades;

namespace Core.UI
{
    public class UpgradesPanel : MonoBehaviour
    {
        [SerializeField] private UpgradeUI[] upgrades;
        [SerializeField] private AnimationSequencerController hideAnimation;

        public UnityEvent OnClosed { get; } = new();

        private Inject.Lazy<UpgradesState> upgradesService;
        private List<UpgradesState.Upgrade> currentOptions;


        private void Awake()
        {
            int i = 0;
            foreach (var upgradeButton in upgrades)
            {
                var index = i;
                upgradeButton.OnClicked.AddListener(() =>
                {
                    currentOptions[index].DoUpgrade();
                    Hide();
                });
                i++;
            }
        }

        private void OnEnable()
        {
            currentOptions = upgradesService.Value.DraftUpgrades(upgrades.Length);
            for (int i = 0; i < upgrades.Length; i++)
            {
                var upgrade = currentOptions[i];
                upgrades[i].Render(upgrade.Type.Icon, upgrade.Level + 1, upgrade.Type.Name, upgrade.Type.Description);
            }
        }



        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            hideAnimation.Play(() =>
            {
                gameObject.SetActive(false);
                OnClosed.Invoke();
            });
        }
    }
}
