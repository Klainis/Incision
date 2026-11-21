using Incision.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace Incision
{
    public class ShardProjectile : CardProjectile
    {
        [Header("Flight")]
        [SerializeField, Min(0f)] private float _travelDistance = 6f;
        [Tooltip("Сколько осколок лежит после остановки, потом исчезает")]
        [SerializeField, Min(0f)] private float _lingerTime = 2f;

        private readonly HashSet<IDamageable> _alreadyHit = new HashSet<IDamageable>();

        private Collider[] _colliders;
        private Vector3 _launchDirection;
        private Vector3 _lastVelocity;
        private Vector3 _startPosition;
        private float _lingerTimer;
        private bool _isStopped;

        private void Awake()
        {
            _launchDirection = transform.forward;
        }

        protected override void Start()
        {
            base.Start();

            _colliders = GetComponentsInChildren<Collider>();
            _startPosition = transform.position;
        }

        private void FixedUpdate()
        {
            if (_isStopped)
            {
                _lingerTimer -= Time.fixedDeltaTime;
                if (_lingerTimer <= 0f)
                    DisposeProjectile();
                return;
            }

            // скорость до физического шага: после столкновения она уже изменена
            _lastVelocity = Rigidbody.velocity;

            if ((Rigidbody.position - _startPosition).sqrMagnitude >= _travelDistance * _travelDistance)
                StopFlight();
        }

        protected override void OnTargetCollision(Collision collision, IDamageable damageable)
        {
            if (_isStopped)
                return;

            PassThrough(collision);

            if (!_alreadyHit.Add(damageable))
                return;

            SpawnEffectOnDestroy();
            damageable.TakeDamage(new(Damage, gameObject, collision.gameObject, IsCrit));
        }

        protected override void OnOtherCollision(Collision collision)
        {
            base.OnOtherCollision(collision);
            DisposeProjectile();
        }

        private void PassThrough(Collision collision)
        {
            if (collision.collider != null)
            {
                foreach (Collider c in _colliders)
                {
                    if (c != null)
                        Physics.IgnoreCollision(c, collision.collider);
                }
            }

            // если удар случился на самом первом шаге, скорость ещё не записана
            Rigidbody.velocity = _lastVelocity.sqrMagnitude > 0.01f
                ? _lastVelocity
                : _launchDirection * collision.relativeVelocity.magnitude;
        }

        private void StopFlight()
        {
            _isStopped = true;
            _lingerTimer = _lingerTime;

            Rigidbody.velocity = Vector3.zero;
            Rigidbody.constraints = RigidbodyConstraints.FreezeAll;

            foreach (Collider c in _colliders)
            {
                if (c != null)
                    c.enabled = false;
            }
        }
    }
}
