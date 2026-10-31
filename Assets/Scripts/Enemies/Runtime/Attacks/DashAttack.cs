using System.Threading.Tasks;
using Incision.Enemies.Components;
using Incision.Enemies.Interfaces;
using Incision.Enemies.Structs;
using Incision.Enemies.Templates;
using UnityEngine;
using UnityEngine.AI;

namespace Incision.Enemies.Runtime.Attacks
{
    public class DashAttack : Attack
    {
        #region Fields
        private readonly string _animationTrigger;
        public float EffectiveHitRange { get; }
        public float Distance { get; }
        public float JumpHeight { get; }
        public float Duration { get; }
        public float Speed { get; }
        #endregion
        
        private Enemy _pendingEnemy;
        private Transform _pendingAttackPoint;
        private Transform _pendingTarget;
        private EnemyAnimEvents _pendingEvents;
        private byte _currentVersion;
        private byte _pendingVersion;

        private Vector3 _direction;

        private Rigidbody _rb;
        private NavMeshAgent _agent;

        private bool _wasAgentOn;
        private bool _rbWasKinematic;
        private bool _rbHadGravity;
        private RigidbodyInterpolation _rbInterpolation;
        private CollisionDetectionMode _rbCollisionMode;

        public DashAttack(DashAttackTemplate template) : base(template)
        {
            _animationTrigger = template.AnimationTrigger;
            Distance = template.Distance;
            JumpHeight = template.JumpHeight;
            Duration = template.Duration;
            Speed = template.Speed;

            EffectiveHitRange = HitRange * OnHitRangeMultiplier;
        }

        protected override bool AreConditionsMet(Enemy enemy, Transform attackPoint, Transform target)
        {
            var prev = base.AreConditionsMet(enemy,attackPoint,target);
            return
                prev &&
                enemy.Agent &&
                enemy.AnimationEvents &&
                enemy.Rigidbody;
        }

        protected override async Task ExecuteAttack(Enemy enemy)
        {
            var linearSpeed = Distance / Duration * Speed;
            var elapsed = 0f;

            while (elapsed < Duration)
            {
                if (_currentVersion != _pendingVersion || !enemy) return;
                if (Time.timeScale <= 0f) { await Task.Yield(); continue; }

                var t  = elapsed / Duration;
                var vy = JumpHeight > 0f
                    ? JumpHeight * Mathf.PI / Duration * Mathf.Cos(t * Mathf.PI)
                    : 0f;

                _rb.velocity = _direction * linearSpeed + Vector3.up * vy;

                await Task.Yield();
                elapsed += Time.deltaTime;
            }

            _rb.velocity = Vector3.zero;
        }

        protected override async Task PrepareAttack(Enemy enemy, Transform attackPoint, Transform target)
        {
            _agent = enemy.Agent;
            _rb = enemy.Rigidbody;
            
            _rbWasKinematic = _rb.isKinematic;
            _rbHadGravity = _rb.useGravity;
            _rbInterpolation = _rb.interpolation;
            _rbCollisionMode = _rb.collisionDetectionMode;
            
            enemy.View?.TriggerAttack(_animationTrigger);

            _wasAgentOn = _agent.enabled;
            _agent.isStopped = true;
            _agent.enabled = false;

            _rb.isKinematic = false;
            _rb.useGravity = false;
            _rb.interpolation = RigidbodyInterpolation.Interpolate;
            _rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            
            _direction = enemy.transform.forward;
            
            _currentVersion = BeginPendingHit(enemy, attackPoint, target, enemy.AnimationEvents);
            await Task.Yield();
        }

        protected override async Task EndAttack()
        {
            if (_rb)
            {
                _rb.velocity = Vector3.zero;
                _rb.isKinematic = _rbWasKinematic;
                _rb.useGravity = _rbHadGravity;
                _rb.interpolation = _rbInterpolation;
                _rb.collisionDetectionMode = _rbCollisionMode;
            }

            if (_agent)
            {
                _agent.enabled = _wasAgentOn;
                if (_agent.isOnNavMesh) _agent.isStopped = !_wasAgentOn;
            }

            if (_currentVersion == _pendingVersion) 
                EndPendingHit();
            
            await Task.Yield();
        }

        private byte BeginPendingHit(Enemy enemy, Transform attackPoint, Transform target, EnemyAnimEvents events)
        {
            EndPendingHit();

            _pendingVersion++;
            _pendingEnemy = enemy;
            _pendingAttackPoint = attackPoint;
            _pendingTarget = target;
            _pendingEvents = events;

            events.OnAttackHit += HandleAttackHit;

            return _pendingVersion;
        }

        private void HandleAttackHit()
        {
            var enemy = _pendingEnemy;
            var attackPoint = _pendingAttackPoint;
            var target = _pendingTarget;

            EndPendingHit();

            if (enemy == null || attackPoint == null || target == null) return;
            if (!TargetInRange(attackPoint, target, EffectiveHitRange)) return;
            if (!target.TryGetComponent<IDamageable>(out var damageable)) return;

            damageable.TakeDamage(new DamageInfo(Damage, enemy.gameObject, target.gameObject));
        }

        private void EndPendingHit()
        {
            if (_pendingEvents != null)
                _pendingEvents.OnAttackHit -= HandleAttackHit;

            _pendingEnemy = null;
            _pendingAttackPoint = null;
            _pendingTarget = null;
            _pendingEvents = null;
        }
    }
}