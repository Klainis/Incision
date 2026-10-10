using Incision.Interfaces;
using UnityEngine;
using UnityEngine.AI;

namespace Incision.Enemies.Interfaces
{
    public interface INavMeshService : IService
    {
        void Configure(NavMeshAgent agent, float speed, float acceleration, float angularSpeed, float stoppingDistance);

        bool MoveTo(NavMeshAgent agent, Vector3 worldPosition);

        void Stop(NavMeshAgent agent);

        bool Warp(NavMeshAgent agent, Vector3 worldPosition);

        bool ReachedDestination(NavMeshAgent agent);

        void FaceTarget(Transform self, Vector3 worldPosition, float turnSpeed);

        bool SamplePosition(Vector3 source, out Vector3 result, float maxDistance = 2f);
    }
}