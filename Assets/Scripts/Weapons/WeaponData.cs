using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Weapons
{
    [CreateAssetMenu(menuName = "Data/Weapon")]
    public sealed class WeaponData : ScriptableObject
    {
        [field: SerializeField] public float Damage { get; private set; } = 1f;
        [field: SerializeField] public float AttackRate { get; private set; } = 1f;
    }
}