using Incision.Data;
using Incision.Enemies.Interfaces;
using UnityEngine;

namespace Incision.Enemies.Components
{ 
    public abstract class EnemyMotor : MonoBehaviour
    {
        protected Stats _stats;
        protected Stats Stats => _stats;

        public abstract float CurrentSpeed { get; }
        public abstract bool ReachedDestination { get; }
        public abstract Vector3 Velocity { get; }

        public bool IsMovementLocked { get; protected set; } = false;

        public virtual float NormalizedSpeed
            => Stats is { MoveSpeed: > 0.0001f }
                ? Mathf.Clamp01(CurrentSpeed / Stats.MoveSpeed)
                : 0f;

        public virtual bool IsMoving => CurrentSpeed > 0.05f;

        public abstract void Initialize(Enemy enemy, Stats stats, INavMeshService services);
        public abstract bool MoveTo(Vector3 position);
        public abstract void Stop();
        public abstract bool Warp(Vector3 position);
        public abstract void FaceTarget(Vector3 position);
    }
}