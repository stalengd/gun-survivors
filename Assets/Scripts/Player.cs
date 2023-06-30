using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stalen.DI;
using Core.Weapons;

namespace Core
{
    public sealed class Player : Creature, IService
    {
        [SerializeField] private float movingSpeed = 1f;
        [SerializeField] private WeaponData weaponData;
        [SerializeField] private Weapon weapon;
        [SerializeField] private new Rigidbody2D rigidbody;
        [SerializeField] private PlayerAim aim;

        public string Name { get; } = "Player";

        private Crosshair crosshair;


        protected override void Start()
        {
            base.Start();

            weapon.Init(weaponData);
        }

        protected override void Update()
        {
            base.Update();

            var aimTarget = aim.AimTarget;
            weapon.AimAt(aimTarget);
            RotateTo(aimTarget);
            crosshair.GunPosition = weapon.ShootingPoint.position;
            crosshair.SetLineActive(weapon.IsTargetInSight);

            weapon.TryAttack();
        }

        private void FixedUpdate()
        {
            Move();
        }


        public void ConfigureDependencies()
        {
            Inject.Out(out crosshair);
        }


        private void Move()
        {
            var move = Vector2.zero;

            if (Input.GetKey(KeyCode.D)) move.x = 1f;
            if (Input.GetKey(KeyCode.A)) move.x = -1f;
            if (Input.GetKey(KeyCode.W)) move.y = 1f;
            if (Input.GetKey(KeyCode.S)) move.y = -1f;

            move = move.normalized;

            var velocity = move * movingSpeed;
            rigidbody.velocity = velocity;

            animator.MovingSpeed = velocity.magnitude;
        }
    }
}