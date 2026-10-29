using Incision.Enemies.Interfaces;
using Incision.Interfaces;
using UnityEngine;

namespace Incision.Enemies.Components
{
    public class EnemyProjectile : Projectile
    {
        public void Initialize(float damage)
        {
            _damage = damage;
        }

        protected override void OnTargetCollision(Collision collision, IDamageable damageable)
        {
            base.OnTargetCollision(collision, damageable);

            damageable.TakeDamage(new(Damage, gameObject, collision.transform.position, IsCrit));
        }

        protected override void OnOtherCollision()
        {
            base.OnOtherCollision();
            
            DisposeProjectile();
        }
    }
}