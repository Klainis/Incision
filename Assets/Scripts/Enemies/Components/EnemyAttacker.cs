using System;
using System.Collections.Generic;
using System.Linq;
using Incision.Enemies.Interfaces;
using Incision.Enemies.Templates;
using UnityEngine;

namespace Incision.Enemies.Components
{
    public class EnemyAttacker : MonoBehaviour
    {
        [SerializeField] private Transform attackPoint;
        private readonly List<IEnemyAttack> _attacks = new();

        private Enemy _enemy;
        private float _cooldownTimer;

        public void Initialize(Enemy enemy, IReadOnlyList<AttackTemplate> attackTemplates)
        {
            _enemy = enemy;
            
            _attacks.Clear();
            if (attackTemplates == null) return;
            
            foreach (var tpl in attackTemplates)
            {
                if (tpl == null) continue;
                _attacks.Add(tpl.CreateRuntime());
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

            float dist = Vector3.Distance(attackPoint.position, target.position);
            for (int i = 0; i < _attacks.Count; i++)
            {
                if (dist <= _attacks[i].Range)
                    return true;
            }
            return false;
        }

        public bool TryAttack(Transform target)
        {
            if (_cooldownTimer > 0f || target == null) return false;

            foreach (var attack in _attacks.Where(attack => attack.CanAttack(attackPoint, target)))
            {
                attack.Execute(_enemy, attackPoint, target);
                _cooldownTimer = attack.Cooldown;
                return true;
            }
            return false;
        }

        private void OnDrawGizmos()
        {
            if (_attacks == null || _attacks.Count == 0) return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, _attacks.Min(a => a.Range));
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(attackPoint.position, _attacks.Max(a => a.Range));
        }
    }
}