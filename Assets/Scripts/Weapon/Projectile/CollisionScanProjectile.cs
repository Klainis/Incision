using Incision;
using Incision.Interfaces;
using UnityEngine;

public class CollisionScanProjectile : CardProjectile
{
    protected override void OnTargetCollision(Collision collision, IDamageable damageable)
    {
        SpawnEffectOnDestroy();
        damageable.TakeDamage(new(Damage, gameObject, collision.gameObject, IsCrit));
    }

    protected override void OnOtherCollision(Collision collision)
    {
        base.OnOtherCollision(collision);

        Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }
}
