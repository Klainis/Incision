using Incision.Enemies.Interfaces;
using UnityEngine;

public class CollisionScanProjectile : Projectile
{
    protected override void OnTargetCollision(Collision collision, IDamageable damageable)
    {
        SpawnEffectOnDestroy();
        damageable.TakeDamage(new(Damage, gameObject, collision.transform.position, IsCrit));
    }

    protected override void OnOtherCollision()
    {
        Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }
}
