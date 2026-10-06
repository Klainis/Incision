using System;
using System.Collections.Generic;
using Incision.Enemies.Interfaces;
using Incision.Enemies.Runtime.States;
using Incision.Enemies.Templates;
using UnityEngine;

namespace Incision.Enemies.Components
{
    public class EnemyBrain : MonoBehaviour
    {
        private Enemy _enemy;
        private Health _health;
        private EnemyAITemplate _profile;
        private EnemyStateBase _current;
        private readonly Dictionary<Type, EnemyStateBase> _states = new();

        public EnemyAITemplate Profile => _profile;

        public void Initialize(Enemy enemy, EnemyAITemplate profile, Health health, ITargetProvider targetProvider)
        {
            _profile = profile;
            _enemy = enemy;
            _health = health;
            _health.OnDied += _ => ChangeState<EnemyDeadState>();

            _states[typeof(EnemyIdleState)] = new EnemyIdleState(this, enemy, targetProvider);
            _states[typeof(EnemyPatrolState)] = new EnemyPatrolState(this, enemy, targetProvider);
            _states[typeof(EnemyChaseState)] = new EnemyChaseState(this, enemy);
            _states[typeof(EnemyAttackState)] = new EnemyAttackState(this, enemy);
            _states[typeof(EnemyDeadState)] = new EnemyDeadState(this, enemy);
            
            ChangeState<EnemyIdleState>();
        }

        private void Update() => _current?.Tick();

        public void ChangeState<T>() where T : EnemyStateBase
        {
            if (!_states.TryGetValue(typeof(T), out var next)) return;
            _current?.Dispose();
            _current = next;
            _current.Start();
            Debug.Log($"<color=red>{_enemy.name}</color> is now at <color=yellow>{typeof(T).Name}</color> state");
        }
        
        public Transform CurrentTarget { get; private set; }
        public void SetTarget(Transform target) => CurrentTarget = target;

        void OnDrawGizmos()
        {
            if (_current == null)
                return;
            
            switch (_current.GetType().Name)
            {
                case nameof(EnemyIdleState):
                    Gizmos.color = Color.blue;
                    Gizmos.DrawWireSphere(_enemy.transform.position, Profile.PatrolRadius);
                    break;
                case nameof(EnemyPatrolState):
                    Gizmos.color = Color.magenta;
                    Gizmos.DrawWireSphere(_enemy.transform.position, _enemy.Stats.DetectionRadius);
                    break;
                case nameof(EnemyChaseState):
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawWireSphere(_enemy.transform.position, _enemy.Stats.DetectionRadius);
                    break;
                case nameof(EnemyAttackState):
                    Gizmos.color = Color.red;
                    Gizmos.DrawWireSphere(_enemy.transform.position, _enemy.Stats.DetectionRadius);
                    break;
            }
        }
    }
}