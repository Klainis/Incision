using System;
using System.Threading.Tasks;
using Incision.Enemies.Components;
using Incision.Enemies.Interfaces;
using Incision.Enemies.Templates;
using Incision.Extensions;
using UnityEngine;

namespace Incision.Enemies.Runtime.Attacks
{
    public abstract class Attack : IEnemyAttack
    {
        public float Damage { get; }
        public float HitRange { get; }
        public float OnHitRangeMultiplier { get; }
        public float Cooldown { get; }
        private float StartDelay { get; }
        private float EndDelay { get; }

        protected Attack(AttackTemplate template)
        {
            Damage = template.Damage;
            HitRange = template.HitRange;
            OnHitRangeMultiplier = template.OnHitRangeMultiplier;
            Cooldown = template.Cooldown;
            StartDelay = template.StartDelay;
            EndDelay = template.EndDelay;
        }
        public virtual bool CanAttack(Transform attackPoint, Transform target)
        { 
            if (!target)
                return false;
            return TargetInRange(attackPoint, target, HitRange);
        }
        protected bool TargetInRange(Transform attackPoint, Transform target, float range)
        {
            return Vector3.Distance(attackPoint.position, target.position) <= range;
        }

        public async Task Execute(Enemy enemy, Transform attackPoint, Transform target)
        {
            if (!AreConditionsMet(enemy, attackPoint, target))
                return;
            
            if (StartDelay > 0)
                await Task.Delay(StartDelay.AsMilliseconds());
            
            await PrepareAttack(enemy, attackPoint, target);
            
            try
            {
                await ExecuteAttack(enemy);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            
            await EndAttack();
            
            if (EndDelay > 0)
                await Task.Delay(EndDelay.AsMilliseconds());
        }

        protected virtual bool AreConditionsMet(Enemy enemy, Transform attackPoint, Transform target)
        {
            return enemy && attackPoint && target;
        }
        
        protected virtual async Task PrepareAttack(Enemy enemy, Transform attackPoint, Transform target) => await Task.Yield();
        protected virtual async Task ExecuteAttack(Enemy enemy) => await Task.Yield();
        protected virtual async Task EndAttack() => await Task.Yield();
    }
}
