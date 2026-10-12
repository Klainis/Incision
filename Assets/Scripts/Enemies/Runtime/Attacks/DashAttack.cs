using System;
using System.Threading.Tasks;
using Incision.Enemies.Components;
using Incision.Enemies.Interfaces;
using Incision.Enemies.Structs;
using Incision.Enemies.Templates;
using Incision.Extensions;
using UnityEngine;

namespace Incision.Enemies.Runtime.Attacks
{
    public class DashAttack : IEnemyAttack
    {
        private readonly string _animationTrigger;
        public float Damage { get; }
        public float HitRange { get; }
        public float OnHitRangeMultiplier { get; }
        public float EffectiveHitRange { get; }
        public float Cooldown { get; }
        public float StartDelay { get; }
        public float EndDelay { get; }
        public float Duration { get; }
        
        public float Speed { get; }

        public DashAttack(DashAttackTemplate template)
        {
            _animationTrigger = template.AnimationTrigger;
            Damage = template.Damage;
            HitRange = template.HitRange;
            OnHitRangeMultiplier = template.OnHitRangeMultiplier;
            Cooldown = template.Cooldown;
            StartDelay = template.StartDelay;
            EndDelay = template.EndDelay;
            Duration = template.Duration;
            Speed = template.Speed;
            
            EffectiveHitRange = HitRange * OnHitRangeMultiplier;
        }

        public bool CanAttack(Transform attackPoint, Transform target)
        {
            if (target == null) return false;
            return Vector3.Distance(attackPoint.position, target.position) <= HitRange;
        }

        public async Task Execute(Enemy enemy, Transform attackPoint, Transform target)
        {
            var animEvents = enemy.AnimationEvents;
            animEvents.OnAttackHit += ApplyDamage;
            
            await Task.Delay(StartDelay.AsMilliseconds());
            
            var view = enemy.View;
            view.TriggerAttack(_animationTrigger);
            
            Vector3 start = enemy.transform.position;
            Vector3 targetPos = start + enemy.transform.forward * HitRange;

            float time = 0f;

            while (time < Duration)
            {
                time += Time.deltaTime;
                float t = time / Duration;
                enemy.transform.position = Vector3.Lerp(start, targetPos, t * Speed);
                await Task.Yield();
            }
            
            await Task.Delay(EndDelay.AsMilliseconds());
            return;
            
            void ApplyDamage()
            {
                animEvents.OnAttackHit -= ApplyDamage;
                
                var dist = Vector3.Distance(attackPoint.position, target.position);
                if (dist > EffectiveHitRange) return;

                if (!target.TryGetComponent<IDamageable>(out var damageable))
                    return;
                
                damageable.TakeDamage(
                    new DamageInfo(
                        Damage, 
                        enemy.gameObject, 
                        target.position
                    )
                );
            }
        }
    }
}