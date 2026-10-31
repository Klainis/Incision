using Incision;
using Incision.Enemies.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RicochetProjectile : CardProjectile
{
    [Header("Ricochet")]
    [SerializeField] private int _baseBounces = 1;
    [SerializeField] private float _searchRadius = 25f;
    [SerializeField] private LayerMask _enemyLayers;

    private readonly Collider[] _buffer = new Collider[32];
    private readonly HashSet<IDamageable> _alreadyHit = new HashSet<IDamageable>();

    private Collider[] _ownColliders;
    private int _bouncesLeft;
    private Func<Vector3, Vector3, Collider, bool> _finalBounceHandler;

    private void Awake()
    {
        _bouncesLeft = _baseBounces;
    }

    public void ConfigureRicochet(int bounces, Func<Vector3, Vector3, Collider, bool> finalBounceHandler) // For card ability
    {
        _bouncesLeft = bounces;
        _finalBounceHandler = finalBounceHandler;
    }

    protected override void OnTargetCollision(Collision collision, IDamageable damageable)
    {
        if (!_alreadyHit.Add(damageable))
            return;

        SpawnEffectOnDestroy();
        damageable.TakeDamage(new(Damage, gameObject, collision.gameObject, IsCrit));

        if (_bouncesLeft <= 0)
        {
            DisposeProjectile();
            return;
        }

        Vector3 from = transform.position;
        Collider next = FindNearestEnemy(from);
        if (next == null)
        {
            DisposeProjectile();
            return;
        }

        _bouncesLeft--;
        Vector3 dir = (next.bounds.center - from).normalized;

        if (_bouncesLeft == 0 && _finalBounceHandler != null
            && _finalBounceHandler(from, dir, collision.collider))
        {
            DisposeProjectile();
            return;
        }

        _ownColliders ??= GetComponentsInChildren<Collider>();
        foreach (Collider c in _ownColliders)
            Physics.IgnoreCollision(c, collision.collider);

        Rigidbody.velocity = Vector3.zero;
        transform.rotation = Quaternion.LookRotation(dir);
        Rigidbody.AddForce(dir * CardData.ProjectileSpeed, ForceMode.Impulse);
    }

    protected override void OnOtherCollision()
    {
        base.OnOtherCollision();

        Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }

    private Collider FindNearestEnemy(Vector3 from)
    {
        int count = Physics.OverlapSphereNonAlloc(from, _searchRadius, _buffer, _enemyLayers);
        Collider best = null;
        float bestSqr = float.MaxValue;

        for (int i = 0; i < count; i++)
        {
            Collider c = _buffer[i];

            IDamageable target = c.GetComponentInParent<IDamageable>();
            if (target == null || _alreadyHit.Contains(target))
                continue;

            float sqr = (c.bounds.center - from).sqrMagnitude;
            if (sqr < bestSqr)
            {
                bestSqr = sqr;
                best = c;
            }
        }

        return best;
    }
}