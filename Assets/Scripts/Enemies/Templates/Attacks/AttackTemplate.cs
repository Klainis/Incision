using Incision.Enemies.Interfaces;
using UnityEngine;

namespace Incision.Enemies.Templates
{
    public abstract class AttackTemplate : ScriptableObject
    {
        [SerializeField] protected float _damage = 5f;
        [SerializeField] protected float _range = 2f;
        [SerializeField] protected float _cooldown = 1f;

        public float Cooldown => _cooldown;
        public float Range => _range;
        public float Damage => _damage;

        public abstract IEnemyAttack CreateRuntime();
    }
}


