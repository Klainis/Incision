using Incision;
using Incision.Interfaces;
using UnityEngine;
using VContainer;

public class RicochetProjectile : CardProjectile
{
    [Header("Ricochet")]
    [SerializeField] private int _baseBounces = 1;
    [SerializeField] private float _searchRadius = 25f;
    [SerializeField] private LayerMask _enemyLayers;

    private readonly Collider[] _buffer = new Collider[32];
    private RicochetChain _chain;

    [Inject] private ProjectileFactory _projectileFactory;

    private void OnEnable()
    {
        _chain = new RicochetChain { BouncesLeft = _baseBounces };
    }

    public void SetChain(RicochetChain chain) => _chain = chain;

    protected override void OnTargetCollision(Collision collision, IDamageable damageable)
    {
        if (!_chain.AlreadyHit.Add(damageable))
            return;

        Vector3 hitPoint = collision.GetContact(0).point;

        SpawnEffectOnDestroy();
        damageable.TakeDamage(new(Damage, gameObject, transform.position, IsCrit));

        if (_chain.BouncesLeft <= 0)
        {
            DisposeProjectile();
            return;
        }

        Collider next = FindNearestEnemy(hitPoint);
        if (next == null)
        {
            DisposeProjectile();
            return;
        }

        _chain.BouncesLeft--;
        Vector3 dir = (next.bounds.center - hitPoint).normalized;

        bool handled = _chain.BouncesLeft == 0
                       && _chain.FinalBounce != null
                       && _chain.FinalBounce(hitPoint, dir, collision.collider, _projectileFactory);

        if (!handled)
        {
            CardProjectile spawned = _chain.Spawn(_projectileFactory, CardData, Damage, IsCrit, hitPoint, dir);
            if (spawned is RicochetProjectile bounce)
            {
                bounce.SetChain(_chain);
                foreach (Collider c in bounce.GetComponentsInChildren<Collider>())
                    Physics.IgnoreCollision(c, collision.collider);
            }
        }

        DisposeProjectile();
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
            if (target == null || _chain.AlreadyHit.Contains(target))
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