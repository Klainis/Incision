using Incision.Enemies.Interfaces;
using UnityEngine;

namespace Incision.Enemies.Components
{
    public class EnemyProjectile : Projectile
    {
        private GameObject _owner;
        public void Initialize(float damage, GameObject owner)
        {
            _damage = damage;
            _owner = owner;
        }

        protected override void OnTargetCollision(Collision collision, IDamageable damageable)
        {
            base.OnTargetCollision(collision, damageable);

            damageable.TakeDamage(new(Damage, _owner, collision.gameObject, IsCrit));
        }

        protected override void OnOtherCollision()
        {
            base.OnOtherCollision();
            
            DisposeProjectile();
        }
    }
}