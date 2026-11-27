using System;
using UnityEngine;

namespace Incision
{
    public class ProjectileRangeLimiter : MonoBehaviour
    {
        private CardProjectile _projectile;
        private Vector3 _startPosition;
        private float _lingerTimer;
        private float _lingerTime;
        private float _travelDistance;
        private bool _isStopped;

        private Rigidbody _rb;

        public void Initialize(CardProjectile projectile, float maxDistance, float lingerTime)
        {
            _projectile = projectile;
            _rb = projectile.Rigidbody;
            _travelDistance = maxDistance;
            _lingerTime = lingerTime;
            _startPosition = projectile.Rigidbody.position;

            _lingerTimer = _lingerTime;
        }

        private void FixedUpdate()
        {
            if (_projectile == null)
                return;

            if (_isStopped)
            {
                _lingerTimer -= Time.fixedDeltaTime;
                if (_lingerTimer <= 0f)
                    _projectile.DisposeProjectile();
                return;
            }

            if ((_rb.position - _startPosition).sqrMagnitude >= _travelDistance * _travelDistance)
                StopFlight();
        }

        private void StopFlight()
        {
            _isStopped = true;
            _lingerTimer = _lingerTime;

            _rb.velocity = Vector3.zero;
            _rb.constraints = RigidbodyConstraints.FreezeAll;
        }
    }
}