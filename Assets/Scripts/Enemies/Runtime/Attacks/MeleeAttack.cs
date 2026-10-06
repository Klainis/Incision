using Incision.Enemies.Components;
using Incision.Enemies.Interfaces;
using Incision.Enemies.Structs;
using UnityEngine;

namespace Incision.Enemies.Runtime.Attacks
{
    public class MeleeAttack : IEnemyAttack
    {
        private readonly string _animationTrigger;

        public float Damage { get; }
        public float Range { get; }
        public float Cooldown { get; }

        public MeleeAttack(float damage, string animationTrigger, float cooldown, float range)
        {
            _animationTrigger = animationTrigger;
            Damage = damage;
            Range = range;
            Cooldown = cooldown;
        }

        public bool CanAttack(Transform attackPoint, Transform target)
        {
            if (target == null) return false;
            return Vector3.Distance(attackPoint.position, target.position) <= Range;
        }

        public void Execute(Enemy enemy, Transform attackPoint, Transform target)
        {
            if (target == null) return;

            var animEvents = enemy.AnimationEvents;
            animEvents.OnAttackHit += ApplyDamage;
            
            var view = enemy.View;
            view?.TriggerAttack(_animationTrigger);
            return;
            
            void ApplyDamage()
            {
                animEvents.OnAttackHit -= ApplyDamage;
                var dist = Vector3.Distance(attackPoint.position, target.position);
                if (dist > Range * 1.15f) return;
                
                if (target.TryGetComponent<IDamageable>(out var damageable))
                {
                    damageable.TakeDamage(new DamageInfo
                    (
                        Damage,
                        enemy.gameObject,
                        target.position
                    ));
                }
                
            }
        }
    }
}