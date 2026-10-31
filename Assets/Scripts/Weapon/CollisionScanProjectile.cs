using Incision;
using Incision.Enemies.Interfaces;
using UnityEngine;

public class CollisionScanProjectile : CardProjectile
{
    protected override void OnTargetCollision(Collision collision, IDamageable damageable)
    {
        SpawnEffectOnDestroy();
        damageable.TakeDamage(new(Damage, gameObject, collision.gameObject, IsCrit));
    }

    protected override void OnOtherCollision()
    {
        base.OnOtherCollision();

        Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }
}
