using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Effects;

namespace Core.Weapons
{
    public sealed class WeaponEffects : MonoBehaviour
    {
        [SerializeField] private float shellLifetime = 10f;
        [SerializeField] private GameObject shellPrefab;
        [SerializeField] private AnimationCurve recoilCurve;
        [SerializeField] private ParticleSystem flashParticles;

        private Weapon weapon;
        private MultiTransform.Part weaponTransform;
        private float timeSinceLastAttack = 999f;


        private void Awake()
        {
            weaponTransform = transform.AsMultiTransform();
            weapon = GetComponent<Weapon>();

            weapon.OnAttack.AddListener(Shoot);
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            var recoilAngle = recoilCurve.Evaluate(timeSinceLastAttack);
            if (weapon.AimDirection.x < 0f) recoilAngle *= -1;
            weaponTransform.LocalRotation = Quaternion.Euler(0, 0, recoilAngle);
        }

        private void Shoot()
        {
            timeSinceLastAttack = 0f;
            SpawnShell();
            DoFlash(); 
        }

        private void SpawnShell()
        {
            var go = Instantiate(shellPrefab, transform.position, Quaternion.identity);
            Destroy(go, shellLifetime);
            var shell = go.GetComponent<BouncyShell>();
            shell.SetHorizontalVelocity(-transform.right);
        }

        private void DoFlash()
        {
            //flashParticles.Emit(1);
            flashParticles.Play(true);
        }
    }
}