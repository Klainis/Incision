using Incision.Enemies.Interfaces;
using Incision.Extensions;
using UnityEngine;
using UnityEngine.AI;

namespace Incision.Enemies.Services
{
    public class NavMeshService : INavMeshService
    {
        public string LOG_TAG { get; } = "[NavMeshService]".AsTag(Color.red);
        private int _areaMask;
        private float _sampleDistance;
        
        public void Configure(NavMeshAgent agent, float speed, float acceleration, float angularSpeed, float stoppingDistance)
        {
            Configure(agent, speed, acceleration, angularSpeed, stoppingDistance, NavMesh.AllAreas, 2f);
        }

        public void Configure(NavMeshAgent agent, float speed, float acceleration, float angularSpeed, float stoppingDistance, int areaMask = NavMesh.AllAreas, float sampleDistance = 2f)
        {
            if (agent == null) return;

            agent.speed = speed;
            agent.acceleration = acceleration;
            agent.angularSpeed = angularSpeed;
            agent.stoppingDistance = stoppingDistance;
            agent.updateRotation = false;
            agent.updateUpAxis = true;     
            agent.autoBraking = true;
            agent.autoRepath = true;
            
            _areaMask = areaMask;
            _sampleDistance = sampleDistance;
            
            EnsureOnNavMesh(agent);
        }

        public bool MoveTo(NavMeshAgent agent, Vector3 worldPosition)
        {
            if (!IsUsable(agent)) return false;

            if (!SamplePosition(worldPosition, out var projected, _sampleDistance))
                return false;

            agent.isStopped = false;
            return agent.SetDestination(projected);
        }

        public void Stop(NavMeshAgent agent)
        {
            if (!IsUsable(agent)) return;

            agent.ResetPath();
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
        }

        public bool Warp(NavMeshAgent agent, Vector3 worldPosition)
        {
            if (agent == null || !agent.enabled) return false;

            if (!SamplePosition(worldPosition, out var projected, _sampleDistance * 2f))
                return false;

            return agent.Warp(projected);
        }

        public bool ReachedDestination(NavMeshAgent agent)
        {
            if (!IsUsable(agent)) return true;
            if (agent.pathPending) return false;
            if (agent.remainingDistance > agent.stoppingDistance + 0.05f) return false;
            if (agent.hasPath && agent.velocity.sqrMagnitude > 0.01f) return false;
            return true;
        }

        public void FaceTarget(Transform self, Vector3 worldPosition, float turnSpeed)
        {
            if (self == null) return;

            Vector3 dir = worldPosition - self.position;
            dir.y = 0f;
            if (dir.sqrMagnitude < 0.0001f) return;

            Quaternion target = Quaternion.LookRotation(dir);
            self.rotation = Quaternion.Slerp(self.rotation, target, Time.deltaTime * turnSpeed);
        }

        public bool SamplePosition(Vector3 source, out Vector3 result, float maxDistance = 2f)
        {
            if (NavMesh.SamplePosition(source, out var hit, maxDistance, _areaMask))
            {
                result = hit.position;
                return true;
            }

            result = source;
            return false;
        }

        private static bool IsUsable(NavMeshAgent agent)
            => agent != null && agent.enabled && agent.isOnNavMesh;

        private void EnsureOnNavMesh(NavMeshAgent agent)
        {
            if (agent.isOnNavMesh) return;

            if (NavMesh.SamplePosition(agent.transform.position, out var hit, _sampleDistance * 2f, _areaMask))
            {
                agent.Warp(hit.position);
            }
            else
            {
                Debug.LogWarning($"[NavMeshService] Не удалось поставить агента {agent.name} на NavMesh", agent);
            }
        }
    }
}