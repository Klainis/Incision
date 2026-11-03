using Incision.Data;
using Incision.Enemies.Enums;
using Incision.Enemies.Interfaces;
using Incision.Enemies.Templates;
using UnityEngine;

namespace Incision.Enemies.Components
{
    public class FlyingEnemyMotor : EnemyMotor
    {
        private Rigidbody _rb;
        private FlyingEnemyAITemplate _settings;
        private Vector3 _desiredDirection;
        private Vector3 _desiredVelocity;
        private Vector3 _destination;
        private bool _hasDestination;
        private float _hoverPhase;
        private float _currentBank;
        private float _targetYaw;
    
        private Vector3 _collisionAvoidance;

        public override float CurrentSpeed => _rb != null ? _rb.velocity.magnitude : 0f;
        public override Vector3 Velocity => _rb != null ? _rb.velocity : Vector3.zero;

        public override bool ReachedDestination
        {
            get
            {
                if (!_hasDestination) return true;
                return (_destination - transform.position).sqrMagnitude
                       <= _settings.StopDistance * _settings.StopDistance;
            }
        }

        public override void Initialize(Enemy enemy, Stats stats, INavMeshService navMesh)
        {
            _rb = enemy.Rigidbody;
            _stats = stats;
            if (enemy.Template.Type == EnemyType.Flying && enemy.Template.AIProfile is FlyingEnemyAITemplate flyingAITemplate)
            {
                _settings = flyingAITemplate;
            }
            SnapToAltitude();
        }

        public override bool MoveTo(Vector3 position)
        {
            _destination = ClampAltitude(position);
            _hasDestination = true;
            return true;
        }

        public override void Stop()
        {
            _hasDestination = false;
            _desiredDirection = Vector3.zero;
            _desiredVelocity = Vector3.zero;
            _rb.velocity = Vector3.zero;
        }

        public override bool Warp(Vector3 position)
        {
            _rb.velocity = Vector3.zero;
            transform.position = ClampAltitude(position);
            return true;
        }

        public override void FaceTarget(Vector3 target)
        {
            var dir = target - transform.position;
            if (dir.sqrMagnitude < 0.0001f) return;
            _targetYaw = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        }

        private void Update()
        {
            if (IsMovementLocked) return;
            UpdateBanking();
        }

        private void FixedUpdate()
        {
            if (IsMovementLocked) return;

            UpdateDesiredDirection();
            ApplyVelocityAndRotation();
        }

        private void UpdateDesiredDirection()
        {
            _desiredDirection = Vector3.zero;

            if (_hasDestination)
            {
                var toTarget = _destination - _rb.position;
                var dist = toTarget.magnitude;

                if (dist <= _settings.StopDistance)
                    _hasDestination = false;
                else
                {
                    var dir = toTarget / dist;
                    float speedFactor = 1f;

                    if (dist < _settings.ArriveSlowdownRadius)
                    {
                        speedFactor = dist / _settings.ArriveSlowdownRadius;
                    }

                    _desiredDirection = dir * speedFactor;
                }
            }

            if (_collisionAvoidance.sqrMagnitude <= 0.0001f) return;

            var blended = _desiredDirection + _collisionAvoidance * _settings.AvoidanceStrength;
            if (blended.sqrMagnitude > 0.0001f)
                _desiredDirection = blended.normalized;
        }

        private void ApplyVelocityAndRotation()
        {
            var targetVelocity = _desiredDirection * Stats.MoveSpeed;

            var accel = _desiredDirection.sqrMagnitude > 0.0001f
                ? _stats.Acceleration
                : -_stats.Acceleration;

            _desiredVelocity = Vector3.MoveTowards(_desiredVelocity, targetVelocity, accel * Time.fixedDeltaTime);

            var finalVelocity = _desiredVelocity;

            if (finalVelocity.sqrMagnitude < 0.01f && !_hasDestination)
            {
                _hoverPhase += Time.fixedDeltaTime * _settings.HoverFrequency * Mathf.PI * 2f;
                finalVelocity.y += Mathf.Sin(_hoverPhase) * _settings.HoverAmplitude;
            }

            var predictedY = _rb.position.y + finalVelocity.y * Time.fixedDeltaTime;
            var clampedY   = ClampAltitudeY(predictedY);
            if (!Mathf.Approximately(predictedY, clampedY))
                finalVelocity.y = (clampedY - _rb.position.y) / Time.fixedDeltaTime;

            finalVelocity.y = Mathf.Clamp(finalVelocity.y, -20f, 20f);

            _rb.velocity = finalVelocity;

            var flatVel = finalVelocity; flatVel.y = 0f;
            float desiredYaw = flatVel.sqrMagnitude > 0.05f
                ? Mathf.Atan2(flatVel.x, flatVel.z) * Mathf.Rad2Deg
                : _targetYaw;

            var yawRot = Quaternion.Euler(0f, desiredYaw, 0f);
            var bankRot = Quaternion.Euler(0f, 0f, _currentBank);
            var targetRot = yawRot * bankRot;

            _rb.MoveRotation(Quaternion.RotateTowards(_rb.rotation, targetRot,
                Stats.TurnSpeed * 60f * Time.fixedDeltaTime));
        }

        private void UpdateBanking()
        {
            var flatVel = _desiredVelocity; 
            flatVel.y = 0f;
            var yawRot = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
            var localVel = Quaternion.Inverse(yawRot) * flatVel;

            var targetBank = Mathf.Clamp(
                -localVel.x / Mathf.Max(0.01f, Stats.MoveSpeed) * _settings.MaxBankAngle,
                -_settings.MaxBankAngle, _settings.MaxBankAngle);

            _currentBank = Mathf.Lerp(_currentBank, targetBank, _settings.BankSmoothing * Time.deltaTime);
        }

        private Vector3 ClampAltitude(Vector3 pos)
            => new (pos.x, ClampAltitudeY(pos.y), pos.z);

        private float ClampAltitudeY(float y)
        {
            var ground = SampleGroundY(_rb.position);
            return Mathf.Clamp(y, ground + _settings.MinAltitude, ground + _settings.MaxAltitude);
        }

        private void SnapToAltitude()
        {
            var p = transform.position;
            p.y = ClampAltitudeY(p.y);
            transform.position = p;
        }

        private float SampleGroundY(Vector3 from)
        {
            return Physics.Raycast(from, Vector3.down, out var hit, 200f,
                _settings.GroundMask, QueryTriggerInteraction.Ignore) 
                ? hit.point.y 
                : 0f;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (_settings == null) return;

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, _settings.AvoidanceRadius);

            Vector3 forward = _desiredVelocity.sqrMagnitude > 0.01f
                ? _desiredVelocity.normalized
                : transform.forward;

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, transform.position + forward * _settings.AvoidanceLookAhead);

            if (_hasDestination)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(_destination, _settings.StopDistance);
            }
        }
#endif
    }
}