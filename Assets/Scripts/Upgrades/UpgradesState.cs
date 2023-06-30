using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Stalen.DI;

namespace Core.Upgrades
{
    public class UpgradesState : MonoService
    {
        [SerializeField] private UpgradeType[] upgrades;

        public IReadOnlyList<Upgrade> Upgrades => states;
        public UnityEvent<Upgrade> OnUpgrade { get; } = new();

        private List<Upgrade> states = new();
        private bool initialized = false;

        public class Upgrade
        {
            public UpgradesState Manager { get; }
            public UpgradeType Type { get; }
            public int Level { get; private set; }
            public float CurrentValue => Type.Values[Mathf.Clamp(Level, 0, Type.Values.Count)];
            public UnityEvent<Upgrade> OnUpgrade { get; } = new();

            public Upgrade(UpgradesState manager, UpgradeType type, int level)
            {
                Manager = manager;
                Type = type;
                Level = level;
            }

            public void DoUpgrade()
            {
                Level++;
                Manager.UpgradeUpgraded(this);
                OnUpgrade.Invoke(this);
            }
        }


        private void Awake()
        {
            EnsureInitialized();
        }


        public List<Upgrade> DraftUpgrades(int count)
        {
            EnsureInitialized();
            var result = new List<Upgrade>();
            while (result.Count < count)
            {
                var index = Random.Range(0, states.Count);
                var state = states[index];
                if (result.Contains(state)) continue;
                result.Add(state);
            }
            return result;
        }

        public void BindUpgrade(UpgradeType type, UnityAction<Upgrade> callback)
        {
            EnsureInitialized();
            var upgrade = states.Find(x => x.Type == type);
            callback(upgrade);
            upgrade.OnUpgrade.AddListener(callback);
        }

        public void UnbindUpgrade(UpgradeType type, UnityAction<Upgrade> callback)
        {
            EnsureInitialized();
            var upgrade = states.Find(x => x.Type == type);
            upgrade.OnUpgrade.RemoveListener(callback);
        }

        private void UpgradeUpgraded(Upgrade upgrade)
        {
            OnUpgrade.Invoke(upgrade);
        }

        private void EnsureInitialized()
        {
            if (initialized) return;
            initialized = true;
            for (int i = 0; i < upgrades.Length; i++)
            {
                states.Add(new Upgrade(this, upgrades[i], 0));
            }
        }
    }
}