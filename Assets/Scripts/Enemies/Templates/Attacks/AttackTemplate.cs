using Incision.Enemies.Interfaces;
using UnityEngine;

namespace Incision.Enemies.Templates
{
    public abstract class AttackTemplate : ScriptableObject
    {
        [Header("Base")]
        [SerializeField] protected float _damage = 5f;
        [SerializeField] protected float _hitRange = 2f;
        [SerializeField] protected float _onHitRangeMultiplier = 1.15f;
        [SerializeField] protected float _cooldown = 1f;
        [SerializeField] protected string _animationTrigger = "IsAttacking";

        public float Cooldown => _cooldown;
        public float HitRange => _hitRange;
        public float OnHitRangeMultiplier => _onHitRangeMultiplier;
        public float Damage => _damage;
        public string AnimationTrigger => _animationTrigger;

        public abstract IEnemyAttack CreateRuntime();
    }
}


