using Incision.Enemies.Components;
using Incision.Enemies.Interfaces;
using UnityEngine;

namespace Incision.Enemies.Runtime.Attacks
{
    public class RangedAttack : IEnemyAttack
    {
        private readonly float _speed;
        private readonly EnemyProjectile _projectilePrefab;
        private readonly string _animationTrigger;

        public float Damage { get; }
        public float Range { get; }
        public float Cooldown { get; }

        public RangedAttack(float damage, float speed, EnemyProjectile prefab, string anim, float cooldown, float range)
        {
            _speed = speed;
            _projectilePrefab = prefab;
            _animationTrigger = anim;
            Damage = damage;
            Range = range;
            Cooldown = cooldown;
        }

        public bool CanAttack(Transform attackPoint, Transform target)
        {
            if (target == null || _projectilePrefab == null) return false;
            return Vector3.Distance(attackPoint.position, target.position) <= Range;
        }

        public void Execute(Enemy enemy, Transform attackPoint, Transform target)
        {
            enemy.View?.TriggerAttack(_animationTrigger);
            
            Vector3 origin = attackPoint.position + Vector3.up * 1.2f;
            Vector3 dir = (target.position - origin).normalized;

            var p = Object.Instantiate(_projectilePrefab, origin, Quaternion.LookRotation(dir));
            p.Launch(dir, _speed, Damage, enemy.gameObject);
        }
    }
}