using System.Threading.Tasks;
using Incision.Enemies.Components;
using Incision.Enemies.Interfaces;
using Incision.Enemies.Structs;
using Incision.Enemies.Templates;
using UnityEngine;

namespace Incision.Enemies.Runtime.Attacks
{
    public class MeleeAttack : Attack
    {
        private readonly string _animationTrigger;
        
        public MeleeAttack(MeleeAttackTemplate template) : base(template)
        {
            _animationTrigger = template.AnimationTrigger;
        }

        protected override Task PrepareAttack(Enemy enemy, Transform attackPoint, Transform target)
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