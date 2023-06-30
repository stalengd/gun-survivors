using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Effects;

namespace Core
{
    public abstract class Creature : MonoBehaviour
    {
        [SerializeField] protected new SpriteRenderer renderer;
        [SerializeField] protected Stats stats;
        [SerializeField] protected CreatureAnimator animator;
        [SerializeField] protected new Collider2D collider;

        [Space]
        [SerializeField] private GameObject[] deathPrefabs;
        [SerializeField] private Dissolve dissolve;

        public Stats Stats => stats;

        public Vector2 AimDirection { get; private set; }

        private Utils.Timer stunTimer;
        private bool isStunned = false;


        protected virtual void Start()
        {
            Stats.Health.OnChanged.AddListener(HealthChanged);
            Stats.OnDeath.AddListener(Death);
        }

        protected virtual void Update()
        {
            if (isStunned && stunTimer.Update())
            {
                StunEnd();
                isStunned = false;
            }
        }


        public void Stun(float duration)
        {
            StunStart(duration);
            stunTimer.Restart();
            stunTimer.TimeToTrigger = duration;
            isStunned = true;
        }

        protected virtual void RotateTo(Vector2 targetPosition)
        {
            AimDirection = targetPosition - (Vector2)transform.position;
            renderer.flipX = targetPosition.x < transform.position.x;
        }

        protected virtual void StunStart(float duration) { }
        protected virtual void StunEnd() { }

        protected virtual void OnDamage(float amount) 
        {
            animator.PlayHit();
        }

        protected virtual void Death()
        {
            animator.PlayDeath();
            Destroy(this);
            Destroy(collider);
            Destroy(gameObject, .5f);

            var center = renderer.bounds.center;
            foreach (var prefab in deathPrefabs)
            {
                var pos = center;
                pos.z = prefab.transform.localPosition.z;
                var go = Instantiate(prefab, pos, Quaternion.identity);
                go.transform.right = Stats.LastDamageDirection;
            }
            if (dissolve != null) dissolve.enabled = true;
        }


        private void HealthChanged(float oldHealth, float newHealth)
        {
            if (newHealth < oldHealth)
            {
                OnDamage(oldHealth - newHealth);
            }
        }
    }
}