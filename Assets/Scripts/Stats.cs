using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Core
{
    public sealed class Stats : MonoBehaviour
    {
        [SerializeField] private CreatureFraction fraction;
        [SerializeField] private float maxHealth = 1f;

        public ObservableValue<float> Health { get; private set; }
        public float MaxHealth => maxHealth;
        public CreatureFraction Fraction => fraction;
        public Vector2 LastDamageDirection { get; private set; }

        public PipelineValue<bool> Invincibility { get; private set; } = false;
        public UnityEvent OnDeath { get; private set; } = new();
        public UnityEvent<float> OnDamage { get; private set; } = new();
        public UnityEvent<float, Vector2> OnHit { get; private set; } = new();

        public bool IsAlive { get; private set; } = true;

        public enum MaxHealthMigrationStrategy
        {
            FullHealth,
            KeepHealth,
            KeepPercentage
        }



        private void Awake()
        {
            Health = new(maxHealth, value =>
            {
                var curValue = Health.Value;
                if (value < curValue && Invincibility.GetValue())
                    return curValue;

                return Mathf.Clamp(value, 0f, maxHealth);
            });
            Health.OnChanged.AddListener(HealthChanged);
        }


        public bool CanBeHitted(CreatureFraction fraction)
        {
            return this.fraction != fraction && !Invincibility.GetValue();
        }

        public void OnBulletHit(Bullet bullet)
        {
            bullet.DefaultDestroy();
            LastDamageDirection = bullet.transform.right;
            Health.Value -= bullet.Damage;
            OnHit.Invoke(bullet.Damage, LastDamageDirection);
        }

        public void OnPoisonHit(float damage)
        {
            Health.Value -= damage;
            OnHit.Invoke(damage, Vector2.zero);
        }

        public void OnAreaDamageHit(float damage, Vector2 center)
        {
            var dir = (Vector2)transform.position - center;
            var impactAmount = 1f - dir.magnitude;
            if (impactAmount < 0f) impactAmount = 0f;
            LastDamageDirection = dir.normalized * impactAmount;
            Health.Value -= damage;
            OnHit.Invoke(damage, LastDamageDirection);
        }

        public void SetMaxHealth(float maxHealth, MaxHealthMigrationStrategy migrationStrategy)
        {
            var oldMaxHealth = this.maxHealth;
            this.maxHealth = maxHealth;

            Health.Value = migrationStrategy switch
            {
                MaxHealthMigrationStrategy.FullHealth => MaxHealth,
                MaxHealthMigrationStrategy.KeepHealth => Health,
                MaxHealthMigrationStrategy.KeepPercentage => (Health / oldMaxHealth) * maxHealth,
                _ => Health
            };
        }

        public void Revive(float health)
        {
            IsAlive = true;
            Health.Value = health;
        }


        private void HealthChanged(float oldHealth, float health)
        {
            if (!IsAlive)
                return;

            if (health < oldHealth)
            {
                CreateDamageIndicator(oldHealth - health);
                OnDamage.Invoke(oldHealth - health);
            }

            if (health == 0f)
            {
                IsAlive = false;
                OnDeath.Invoke();
            }
        }

        private void CreateDamageIndicator(float damageAmount)
        {
            //if (damageIndicatorPrefab == null) return;

            //var indicator = Instantiate(damageIndicatorPrefab, transform.position + Vector3.up, Quaternion.identity);
            //indicator.Init(damageAmount);
        }
    }
}