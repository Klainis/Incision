using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Incision.Enemies.Components;
using Incision.Enemies.Templates;
using UnityEngine;
using VContainer;
using VContainer.Unity;

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
        
        // TODO: Replace by ProjectileFactory
        [Inject] private IObjectResolver _resolver;
        
        public RangedAttack(RangedAttackTemplate template) : base(template)
        {
            _speed = template.ProjectileSpeed;
            _projectilePrefab = template.ProjectilePrefab;
            _animationTrigger = template.AnimationTrigger;
        }

        public override bool CanAttack(Transform attackPoint, Transform target)
        {
            if (!_projectilePrefab) return false;
            return base.CanAttack(attackPoint, target);
        }

        protected override Task PrepareAttack(Enemy enemy, Transform attackPoint, Transform target)
        {
            enemy.View?.TriggerAttack(_animationTrigger);
            
            origin = attackPoint.position + Vector3.up * 1.2f;
            direction = (target.position - origin).normalized;
            return Task.CompletedTask;
        }

        protected override async Task ExecuteAttack(Enemy enemy)
        {
            var p = _resolver.Instantiate(_projectilePrefab, origin, Quaternion.LookRotation(direction));
            p.Initialize(Damage, enemy.gameObject);
            p.Rigidbody.AddForce(direction * _speed, _forceMode);
            await Task.Yield();
        }
    }
}