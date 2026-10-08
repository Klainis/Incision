using Incision.Enemies.Interfaces;
using UnityEngine;

public class CollisionScanProjectile : Projectile
{
    protected override void OnTargetCollision(Collision collision, IDamageable damageable)
    {
        damageable.TakeDamage(new(Damage, gameObject, collision.transform.position));
    }

    protected override void OnOtherCollision()
    {
        Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }
}
