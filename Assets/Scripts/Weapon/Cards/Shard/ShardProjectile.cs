using Incision.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace Incision
{
    public class ShardProjectile : CardProjectile
    {
        private readonly HashSet<IDamageable> _alreadyHit = new HashSet<IDamageable>();

        private Collider[] _colliders;
        private Vector3 _launchDirection;
        private Vector3 _lastVelocity;

        private void Awake()
        {
            _launchDirection = transform.forward;
        }

        protected override void Start()
        {
            base.Start();

            _colliders = GetComponentsInChildren<Collider>();
        }

        private void FixedUpdate()
        {
            _lastVelocity = Rigidbody.velocity;
        }

        protected override void OnTargetCollision(Collision collision, IDamageable damageable)
        {
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

            Rigidbody.velocity = _lastVelocity.sqrMagnitude > 0.01f
                ? _lastVelocity
                : _launchDirection * collision.relativeVelocity.magnitude;
        }
    }
}
