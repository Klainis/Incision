using System.Threading.Tasks;
using Incision.Enemies.Components;
using Incision.Enemies.Interfaces;
using Incision.Enemies.Templates;
using UnityEngine;

namespace Incision.Enemies.Runtime.Attacks
{
    public class RangedAttack : IEnemyAttack
    {
        private readonly float _speed;
        private readonly EnemyProjectile _projectilePrefab;
        private readonly string _animationTrigger;

        public float Damage { get; }
        public float HitRange { get; }
        public float OnHitRangeMultiplier { get; }
        public float Cooldown { get; }

        public RangedAttack(RangedAttackTemplate template)
        {
            _speed = template.ProjectileSpeed;
            _projectilePrefab = template.ProjectilePrefab;
            _animationTrigger = template.AnimationTrigger;
            Damage = template.Damage;
            HitRange = template.HitRange;
            OnHitRangeMultiplier = template.OnHitRangeMultiplier;
            Cooldown = template.Cooldown;
        }

        public bool CanAttack(Transform attackPoint, Transform target)
        {
            if (target == null || _projectilePrefab == null) return false;
            return Vector3.Distance(attackPoint.position, target.position) <= HitRange;
        }

        public Task Execute(Enemy enemy, Transform attackPoint, Transform target)
        {
            enemy.View?.TriggerAttack(_animationTrigger);
            
            Vector3 origin = attackPoint.position + Vector3.up * 1.2f;
            Vector3 dir = (target.position - origin).normalized;

            var p = Object.Instantiate(_projectilePrefab, origin, Quaternion.LookRotation(dir));
            p.Launch(dir, _speed, Damage, enemy.gameObject);
            return Task.CompletedTask;
        }
    }
}