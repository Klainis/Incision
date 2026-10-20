using System;
using System.Threading.Tasks;
using Incision.Enemies.Components;
using Incision.Enemies.Interfaces;
using Incision.Enemies.Structs;
using Incision.Enemies.Templates;
using Incision.Extensions;
using UnityEngine;

namespace Incision.Enemies.Runtime.Attacks
{
    public class DashAttack : Attack
    {
        private readonly string _animationTrigger;
        public float EffectiveHitRange { get; }
        public float Distance { get; }
        public float JumpHeight { get; }
        public float StartDelay { get; }
        public float EndDelay { get; }
        public float Duration { get; }
        public float Speed { get; }
        
        private Enemy _pendingEnemy;
        private Transform _pendingAttackPoint;
        private Transform _pendingTarget;
        private EnemyAnimEvents _pendingEvents;
        private int _pendingVersion;

        public DashAttack(DashAttackTemplate template) : base(template)
        {
            _animationTrigger = template.AnimationTrigger;
            Distance = template.Distance;
            JumpHeight = template.JumpHeight;
            StartDelay = template.StartDelay;
            EndDelay = template.EndDelay;
            Duration = template.Duration;
            Speed = template.Speed;

            EffectiveHitRange = HitRange * OnHitRangeMultiplier;
        }

        public override async Task Execute(Enemy enemy, Transform attackPoint, Transform target)
        {
            if (enemy == null || attackPoint == null || target == null) return;
            if (enemy.AnimationEvents == null) return;
            if (!enemy.Rigidbody) return;

            var agent = enemy.Agent;
            var rb = enemy.Rigidbody;
            
            var wasAgentOn = agent != null && agent.enabled;
            var wasKinematic = rb.isKinematic;
            var hadGravity = rb.useGravity;
            var  wasInterp = rb.interpolation;
            var  wasCollision = rb.collisionDetectionMode;
            var  wasConstraints = rb.constraints;
            
            var version = BeginPendingHit(enemy, attackPoint, target, enemy.AnimationEvents);
            try
            {
                if (enemy == null || version != _pendingVersion) return;
                await Task.Delay(StartDelay.AsMilliseconds());

                enemy.View?.TriggerAttack(_animationTrigger);

                if (agent != null && agent.enabled)
                {
                    agent.isStopped = true;
                    agent.enabled = false;
                }

                rb.isKinematic = false;
                rb.useGravity = false;
                rb.interpolation = RigidbodyInterpolation.Interpolate;
                rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                rb.constraints = RigidbodyConstraints.FreezeRotation;

                Vector3 dir = enemy.transform.forward;

                float linearSpeed = Distance / Duration * Speed;
                float elapsed = 0f;

                while (elapsed < Duration)
                {
                    if (version != _pendingVersion || enemy == null) return;
                    if (Time.timeScale <= 0f) { await Task.Yield(); continue; }

                    var t  = elapsed / Duration;
                    var vy = JumpHeight > 0f
                        ? JumpHeight * Mathf.PI / Duration * Mathf.Cos(t * Mathf.PI)
                        : 0f;

                    rb.velocity = dir * linearSpeed + Vector3.up * vy;

                    await Task.Yield();
                    elapsed += Time.deltaTime;
                }

                rb.velocity = Vector3.zero;
                
                await Task.Delay(EndDelay.AsMilliseconds());
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            finally
            {
                if (rb != null)
                {
                    rb.velocity = Vector3.zero;
                    rb.isKinematic = wasKinematic;
                    rb.useGravity = hadGravity;
                    rb.interpolation = wasInterp;
                    rb.collisionDetectionMode = wasCollision;
                    rb.constraints = wasConstraints;
                }

                if (agent != null && wasAgentOn)
                {
                    agent.enabled = true;
                    if (agent.isOnNavMesh) agent.isStopped = false;
                }

                if (version == _pendingVersion) 
                    EndPendingHit();
            }
        }

        private int BeginPendingHit(Enemy enemy, Transform attackPoint, Transform target, EnemyAnimEvents events)
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

            damageable.TakeDamage(new DamageInfo(Damage, enemy.gameObject, target.position));
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