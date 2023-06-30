using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Weapons
{
    public sealed class Weapon : MonoBehaviour
    {
        [SerializeField] private Vector2 rotationRadius = Vector2.zero;
        [SerializeField] private new SpriteRenderer renderer;
        [SerializeField] private Transform shootingPoint;
        [SerializeField] private GameObject bulletPrefab;

        [SerializeField] private UnityEvent onAttack;

        public Transform ShootingPoint => shootingPoint;
        public WeaponData Data { get; private set; }
        public bool IsTargetInSight { get; private set; }
        public UnityEvent OnAttack => onAttack;
        public Vector2 AimDirection { get; private set; }

        public CompositeFloat Damage { get; } = new();
        public CompositeFloat AttackRate { get; } = new();

        private float attackRateCooldown;

        private const string TargetLayerMaskName = "Enemies";
        private const CreatureFraction Fraction = CreatureFraction.Player;
        private int targetLayerMask;
        private MultiTransform.Part weaponTransform;


        private void Start()
        {
            weaponTransform = transform.AsMultiTransform();
            targetLayerMask = LayerMask.GetMask(TargetLayerMaskName);
        }

        private void Update()
        {
            if (attackRateCooldown > 0f)
            {
                attackRateCooldown -= Time.deltaTime;
            }
        }

        private void FixedUpdate()
        {
            IsTargetInSight = RaycastForTarget();
        }


        public void Init(WeaponData weaponData)
        {
            Data = weaponData;
            Damage.SetBaseValue(weaponData.Damage);
            AttackRate.SetBaseValue(weaponData.AttackRate);
        }

        public bool TryAttack()
        {
            if (!CanAttack())
            {
                return false;
            }

            Attack();
            return true;
        }

        public bool CanAttack()
        {
            return attackRateCooldown <= 0f && IsTargetInSight;
        }

        public void AimAt(Vector3 target)
        {
            if (renderer == null) return;

            target.z = 0f;

            Vector2 dir = target - transform.parent.position;
            dir.Normalize();
            AimDirection = dir;

            weaponTransform.LocalRotation = Quaternion.LookRotation(Vector3.forward, dir.Rotate(90f));

            var scale = new Vector3(1f, Mathf.Sign(dir.x), 1f);
            weaponTransform.LocalScale = scale;
            //renderer.flipY = dir.x < 0f;

            weaponTransform.LocalPosition = dir * rotationRadius;

            renderer.sortingOrder = dir.y > 0.3f ? -1 : 1;
        }


        private void Attack()
        {
            attackRateCooldown = 1 / AttackRate.Value;
            onAttack.Invoke();

            SpawnBullet();
        }

        private void SpawnBullet()
        {
            var pos = shootingPoint.position;
            var rot = shootingPoint.rotation;
            var bullet = Instantiate(bulletPrefab, pos, rot).GetComponent<Bullet>();

            bullet.Fraction = Fraction;
            bullet.Damage = Damage.Value;
        }

        private bool RaycastForTarget()
        {
            var r = Physics2D.Raycast(shootingPoint.position, shootingPoint.right, float.PositiveInfinity, targetLayerMask);
            return r.transform != null;
        }
    }
}