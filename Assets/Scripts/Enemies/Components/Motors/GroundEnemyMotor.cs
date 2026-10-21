using Incision.Data;
using Incision.Enemies.Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace Incision.Enemies.Components
{
    public class GroundEnemyMotor : EnemyMotor
    {
        private NavMeshAgent _agent;
        [SerializeField] private float _stoppingDistance = 1f;

        private INavMeshService _navMesh;
        public override float CurrentSpeed => _agent != null ? _agent.velocity.magnitude : 0f;
        public override Vector3 Velocity => _agent != null ? _agent.velocity : Vector3.zero;

        public override bool ReachedDestination => _navMesh != null && _navMesh.ReachedDestination(_agent);

        public override void Initialize(Enemy enemy, Stats stats, INavMeshService navMesh)
        {
            _agent = enemy.Agent;
            _stats = stats;
            _navMesh = navMesh;

            _navMesh.Configure(
                _agent,
                speed: stats.MoveSpeed,
                acceleration: stats.Acceleration,
                angularSpeed: stats.TurnSpeed * 60f,
                stoppingDistance: _stoppingDistance);
        }

        public override bool MoveTo(Vector3 position)
            => _navMesh.MoveTo(_agent, position);

        public override void Stop()
            => _navMesh.Stop(_agent);

        public override bool Warp(Vector3 position)
            => _navMesh.Warp(_agent, position);

        public override void FaceTarget(Vector3 worldPosition)
            => _navMesh.FaceTarget(transform, worldPosition, _stats.TurnSpeed);
        
        
    }
}