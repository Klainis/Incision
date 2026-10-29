using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Incision
{
    [CreateAssetMenu(fileName = "HealthBarDamageData", menuName = "Scriptable Objects/HealthBarDamageData")]
    public class HealthBarDamageTemplate : ScriptableObject
    {
        [SerializeField] private float _delay = 0.6f;
        [SerializeField] private float _damageBarDuration = 6f;

        public float DamageBarDelay => _delay;
        public float DamageBarDuration => _damageBarDuration;
    }
}
