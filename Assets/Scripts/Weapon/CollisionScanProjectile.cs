using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionScanProjectile : Projectile
{
    protected override void OnTargetCollision(Collision collision, IDamageable damageable)
    {
        damageable.ApplyDamage(Damage);
    }

    protected override void OnOtherCollision()
    {
        Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }
}
