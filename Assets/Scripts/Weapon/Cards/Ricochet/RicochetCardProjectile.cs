using Incision;
using Incision.Interfaces;
using UnityEngine;
using VContainer;

public class RicochetCardProjectile : CardProjectile
{
    private int _baseBounces = 1;
    private float _searchRadius = 25f;
    private LayerMask _enemyLayers;

    private readonly Collider[] _buffer = new Collider[32];
    private RicochetChain _chain;

    [Inject] private ProjectileFactory _projectileFactory;

    public void SetChain(RicochetChain chain) => _chain = chain;

    protected override void OnInitialized()
    {
        SharperCardData sharperCardData = CardData as SharperCardData;

        if (sharperCardData != null)
        {
            _baseBounces = sharperCardData.BaseBounces;
            _searchRadius = sharperCardData.SearchRadius;
            _enemyLayers = sharperCardData.EnemyLayers;

            _chain = new RicochetChain { BouncesLeft = _baseBounces };
        }
    }

    protected override void OnTargetCollision(Collision collision, IDamageable damageable)
    {
        if (!_chain.AlreadyHit.Add(damageable))
            return;

        Vector3 hitPoint = collision.GetContact(0).point;

        SpawnEffectOnDestroy();
        damageable.TakeDamage(new(Damage, gameObject, collision.gameObject, IsCrit));

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

        SpawnProjectile(hitPoint, dir, collision.collider);
        DisposeProjectile();
    }

    protected override void OnOtherCollision(Collision collision)
    {
        ContactPoint contact = collision.GetContact(0);
        Vector3 hitPoint = collision.GetContact(0).point;

        if (_chain.BouncesLeft + 1 <= 0)
        {
            base.OnOtherCollision(collision);
            Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            return;
        }

        //Collider next = FindNearestEnemy(hitPoint);
        Vector3 dir;
        Vector3 incomingDir = collision.relativeVelocity.normalized;
        Vector3 normal = contact.normal.normalized;

        if(Vector3.Dot(incomingDir, normal) > 0f)
            incomingDir = -incomingDir;


        //if (next != null)
        //    dir = (next.bounds.center - hitPoint).normalized;
        //else
        dir = Vector3.Reflect(incomingDir, normal).normalized;

        _chain.BouncesLeft--;

        SpawnProjectile(hitPoint, dir, collision.collider);
        DisposeProjectile();
    }

    private void SpawnProjectile(Vector3 hitPoint, Vector3 dir, Collider collider)
    {
        bool handled = _chain.BouncesLeft == 0
                       && _chain.FinalBounce != null
                       && _chain.FinalBounce(hitPoint, dir, collider, _projectileFactory);

        if (!handled)
        {
            CardProjectile spawned = _chain.Spawn(_projectileFactory, CardData, Damage, IsCrit, hitPoint, dir);
            if (spawned is RicochetCardProjectile bounce)
            {
                bounce.SetChain(_chain);
                foreach (Collider c in bounce.GetComponentsInChildren<Collider>())
                    Physics.IgnoreCollision(c, collider);
            }
        }
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