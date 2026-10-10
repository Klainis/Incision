using Incision.Enemies.Interfaces;
using Incision.Enemies.Runtime;
using UnityEngine;
using UnityEngine.AI;

namespace Incision.Enemies.Components
{
    public class EnemyMotor : MonoBehaviour
    {
        private NavMeshAgent _agent;
        [SerializeField] private float _stoppingDistance = 1f;

        private INavMeshService _navMesh;
        private Stats _stats;

        public bool ReachedDestination => _navMesh != null && _navMesh.ReachedDestination(_agent);

        public void Initialize(NavMeshAgent agent, Stats stats, INavMeshService navMesh)
        {
            _agent = agent;
            _stats = stats;
            _navMesh = navMesh;

            _navMesh.Configure(
                _agent,
                speed: stats.MoveSpeed,
                acceleration: stats.Acceleration,
                angularSpeed: stats.TurnSpeed * 60f,
                stoppingDistance: _stoppingDistance);
        }

        public bool MoveTo(Vector3 position)
            => _navMesh.MoveTo(_agent, position);

        public void Stop()
            => _navMesh.Stop(_agent);

        public bool Warp(Vector3 position)
            => _navMesh.Warp(_agent, position);

        public void FaceTarget(Vector3 worldPosition)
            => _navMesh.FaceTarget(transform, worldPosition, _stats.TurnSpeed);
        
        
    }
}