using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Stalen.DI;
using Core.UI;
using Core.Upgrades;
using static Core.Upgrades.UpgradesState;

namespace Core.Weapons
{
    public sealed class WeaponUpgrades : MonoBehaviour
    {
        [SerializeField] private UpgradeType damageUpgrade;
        [SerializeField] private UpgradeType attackRateUpgrade;

        private Weapon weapon;
        private Inject.Lazy<UpgradesState> upgradesState;


        private void Awake()
        {
            weapon = GetComponent<Weapon>();

            //upgradesState.Value.BindUpgrade(damageUpgrade, DamageUpgraded);

            var l = new List<(UpgradeType type, CompositeFloat target, CompositeToken token, UnityAction<Upgrade> callback)>()
            {
                (damageUpgrade, weapon.Damage, default, default),
                (attackRateUpgrade, weapon.AttackRate, default, default),
            };

            for (int i = 0; i < l.Count; i++)
            {
                var item = l[i];
                var index = i;
                item.callback = u =>
                {
                    var item = l[index];
                    l[index].target.Multiply(ref item.token, u.CurrentValue);
                    l[index] = item;
                };
                upgradesState.Value.BindUpgrade(item.type, item.callback);
                l[i] = item;
            }
        }

        //private void DamageUpgraded(UpgradesState.Upgrade upgrade)
        //{
        //    weapon.Damage.Multiply(ref damageToken, upgrade.CurrentValue);
        //}
    }

    // Draft
    struct UpgradeCompositeValueListener
    {
        UpgradeType type;
        CompositeFloat target;
        CompositeToken token;
        UpgradesState upgrades;
        UnityAction<Upgrade> bindedCallback;

        public UpgradeCompositeValueListener(UpgradeType type, CompositeFloat target)
        {
            this.type = type;
            this.target = target;
            token = default;
            upgrades = null;
            bindedCallback = null;
        }

        public void BindAdd(UpgradesState upgrades)
        {
            this.upgrades = upgrades;
            bindedCallback = UpgradedAdd;
            upgrades.BindUpgrade(type, bindedCallback);
        }

        public void BindMultiply(UpgradesState upgrades)
        {
            this.upgrades = upgrades;
            bindedCallback = UpgradedMultiply;
            upgrades.BindUpgrade(type, bindedCallback);
        }

        public void Unbind()
        {
            upgrades.UnbindUpgrade(type, bindedCallback);
            target.RemoveModification(token);
        }


        private void UpgradedAdd(Upgrade upgrade)
        {
            target.Add(ref token, upgrade.CurrentValue);
        }

        private void UpgradedMultiply(Upgrade upgrade)
        {
            target.Multiply(ref token, upgrade.CurrentValue);
        }
    }
}