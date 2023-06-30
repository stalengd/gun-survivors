using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stalen.DI;

namespace Core
{
    public sealed class Enemy : Creature
    {
        [SerializeField] private float movingSpeed = 1f;
        [SerializeField] private float acceleration = 1f;
        [SerializeField] private float impactAffection = 1f;
        [SerializeField] private float experienceReward = 1f;
        
        [Space]
        [SerializeField] private new Rigidbody2D rigidbody;

        public Player Target { get; private set; }
        public EnemiesSpawner Spawner { get; private set; }
        public float MovingSpeed
        {
            get => movingSpeed;
            set => movingSpeed = value;
        }
        public float ExperienceReward => experienceReward;

        private Vector2 velocity;


        protected override void Start()
        {
            base.Start();
            animator.MovingSpeed = movingSpeed;
            stats.OnHit.AddListener(OnHit);
        }

        private void FixedUpdate()
        {
            MoveToTarget();
        }


        public void Init(Player target, EnemiesSpawner spawner)
        {
            Target = target;
            Spawner = spawner;
        }


        private void MoveToTarget()
        {
            Vector2 target = Target.transform.position;
            RotateTo(target);

            var dir = target - rigidbody.position;

            velocity = Vector2.MoveTowards(velocity, dir.normalized * movingSpeed, acceleration * Time.fixedDeltaTime);

            var pos = rigidbody.position + velocity * Time.fixedDeltaTime;
            rigidbody.MovePosition(pos);
        }

        private void OnHit(float damage, Vector2 impact)
        {
            velocity += impact * impactAffection;
        }

        protected override void Death()
        {
            base.Death();

            Spawner.EnemyKilled(this);
        }
    }
}