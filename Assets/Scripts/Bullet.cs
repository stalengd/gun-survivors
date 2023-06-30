using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Utils;

namespace Core
{
    public sealed class Bullet : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private new Rigidbody2D rigidbody;

        public CreatureFraction Fraction { get; set; }
        public float Damage { get; set; } = 1f;


        private void Start()
        {
            rigidbody.velocity = transform.right * speed;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Stats>(out var hitTarget) && 
                hitTarget.CanBeHitted(Fraction))
            {
                hitTarget.OnBulletHit(this);
            }
        }


        public void DefaultDestroy()
        {
            Destroy(gameObject);
        }
    }
}