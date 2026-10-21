using System.Threading.Tasks;
using Incision.Enemies.Components;
using Incision.Enemies.Templates;
using UnityEngine;

namespace Incision.Enemies.Runtime.Attacks
{
    public class RangedAttack : Attack
    {
        [Header("Force Settings")]
        [SerializeField] private ForceMode _forceMode = ForceMode.Impulse;

        private readonly float _speed;
        private readonly EnemyProjectile _projectilePrefab;
        private readonly string _animationTrigger;

        private Vector3 direction;
        private Vector3 origin;
        
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

        protected override async Task PrepareAttack(Enemy enemy, Transform attackPoint, Transform target)
        {
            enemy.View?.TriggerAttack(_animationTrigger);
            
            origin = attackPoint.position + Vector3.up * 1.2f;
            direction = (target.position - origin).normalized;

            await Task.Yield();
        }

        protected override async Task ExecuteAttack(Enemy enemy)
        {
            var p = Object.Instantiate(_projectilePrefab, origin, Quaternion.LookRotation(direction));
            p.Initialize(Damage);
            p.Rigidbody.AddForce(direction * _speed, _forceMode);
            await Task.Yield();
        }
    }
}