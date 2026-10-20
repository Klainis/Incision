using System.Threading.Tasks;
using Incision.Enemies.Components;
using Incision.Enemies.Interfaces;
using Incision.Enemies.Templates;
using UnityEngine;

namespace Incision.Enemies.Runtime.Attacks
{
    public class RangedAttack : Attack
    {
        private readonly float _speed;
        private readonly EnemyProjectile _projectilePrefab;
        private readonly string _animationTrigger;

        public RangedAttack(RangedAttackTemplate template) : base(template)
        {
            _speed = template.ProjectileSpeed;
            _projectilePrefab = template.ProjectilePrefab;
            _animationTrigger = template.AnimationTrigger;
        }

        public override bool CanAttack(Transform attackPoint, Transform target)
        {
            if (_projectilePrefab == null) return false;
            return base.CanAttack(attackPoint, target);
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