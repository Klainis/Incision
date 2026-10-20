using System.Threading.Tasks;
using Incision.Enemies.Components;
using Incision.Enemies.Interfaces;
using Incision.Enemies.Templates;
using UnityEngine;

namespace Incision.Enemies.Runtime.Attacks
{
    public abstract class Attack : IEnemyAttack
    {
        public float Damage { get; }
        public float HitRange { get; }
        public float OnHitRangeMultiplier { get; }
        public float Cooldown { get; }

        protected Attack(AttackTemplate template)
        {
            Damage = template.Damage;
            HitRange = template.HitRange;
            OnHitRangeMultiplier = template.OnHitRangeMultiplier;
            Cooldown = template.Cooldown;
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
        public virtual Task Execute(Enemy enemy, Transform attackPoint, Transform target) => Task.CompletedTask;
    }
}
