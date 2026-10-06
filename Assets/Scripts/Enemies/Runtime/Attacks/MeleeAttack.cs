using System.Threading.Tasks;
using Incision.Enemies.Components;
using Incision.Enemies.Interfaces;
using Incision.Enemies.Structs;
using Incision.Enemies.Templates;
using UnityEngine;

namespace Incision.Enemies.Runtime.Attacks
{
    public class MeleeAttack : IEnemyAttack
    {
        private readonly string _animationTrigger;
        public float Damage { get; }
        public float HitRange { get; } 
        public float OnHitRangeMultiplier { get; }
        public float Cooldown { get; }

        public MeleeAttack(MeleeAttackTemplate template)
        {
            _animationTrigger = template.AnimationTrigger;
            Damage = template.Damage;
            HitRange = template.HitRange;
            OnHitRangeMultiplier = template.OnHitRangeMultiplier;
            Cooldown = template.Cooldown;
        }

        public bool CanAttack(Transform attackPoint, Transform target)
        {
            if (target == null) return false;
            return Vector3.Distance(attackPoint.position, target.position) <= HitRange;
        }

        public Task Execute(Enemy enemy, Transform attackPoint, Transform target)
        {
            if (target == null) return Task.CompletedTask;

            var animEvents = enemy.AnimationEvents;
            animEvents.OnAttackHit += ApplyDamage;
            
            var view = enemy.View;
            view?.TriggerAttack(_animationTrigger);
            return Task.CompletedTask;
            
            void ApplyDamage()
            {
                animEvents.OnAttackHit -= ApplyDamage;
                var dist = Vector3.Distance(attackPoint.position, target.position);
                if (dist > HitRange * OnHitRangeMultiplier) return;
                
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