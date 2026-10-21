using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Incision.Enemies.Interfaces;
using Incision.Enemies.Templates;
using UnityEngine;
using VContainer;

namespace Incision.Enemies.Components
{
    public class EnemyAttacker : MonoBehaviour
    {
        [SerializeField] private Transform attackPoint;
        private readonly List<IEnemyAttack> _attacks = new();

        private Enemy _enemy;
        private float _cooldownTimer;

        private float _minAttackRange = 9999999f;

        public bool IsAttacking { get; private set; }

        [Inject] private IObjectResolver _resolver;

        public void Initialize(Enemy enemy, IReadOnlyList<AttackTemplate> attackTemplates)
        {
            _enemy = enemy;
            
            _attacks.Clear();
            if (attackTemplates == null) return;
            
            foreach (var tpl in attackTemplates)
            {
                if (tpl == null) continue;
                var attack = tpl.CreateRuntime();
                _resolver.Inject(attack);
                _attacks.Add(attack);
                if (_minAttackRange > tpl.HitRange)
                    _minAttackRange = tpl.HitRange;
            }
        }
        
        private void Update()
        {
            if (_cooldownTimer > 0f)
                _cooldownTimer -= Time.deltaTime;
        }

        public bool IsInRange(Transform target)
        {
            if (target == null || _attacks.Count == 0) return false;

            var dist = Vector3.Distance(attackPoint.position, target.position);
            
            return dist < _minAttackRange;
        }

        public async Task TryAttack(Transform target)
        {
            if (_cooldownTimer > 0f || !target) return;

            IsAttacking = true;

            foreach (var attack in _attacks.Where(attack => attack.CanAttack(attackPoint, target)))
            {
                await attack.Execute(_enemy, attackPoint, target);
                IsAttacking = false;
                _cooldownTimer = attack.Cooldown;
                return;
            }
            IsAttacking = false;
        }

        private void OnDrawGizmosSelected()
        {
            if (_attacks == null || _attacks.Count == 0) return;
            Gizmos.color = Color.red;
            var attackWithMinRange = _attacks.OrderBy(a => a.HitRange).First();
            var attackWithMaxRange = _attacks.OrderByDescending(a => a.HitRange).First();
            Gizmos.DrawWireSphere(attackPoint.position, attackWithMinRange.HitRange * attackWithMinRange.OnHitRangeMultiplier);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(attackPoint.position, attackWithMaxRange.HitRange * attackWithMaxRange.OnHitRangeMultiplier);
        }
    }
}