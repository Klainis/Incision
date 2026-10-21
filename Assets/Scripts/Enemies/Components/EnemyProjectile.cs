using Incision.Enemies.Interfaces;
using Incision.Enemies.Structs;
using UnityEngine;

namespace Incision.Enemies.Components
{
    public class EnemyProjectile : Projectile
    {
        private Vector3 _dir;
        private float _speed;
        private GameObject _owner;
        private float _life = 5f;
        
        public void Initialize(float damage)
        {
            _damage = damage;
        }
        
        public void Launch(Vector3 dir, float speed, float damage, GameObject owner)
        {
            _dir = dir; _speed = speed; _damage = damage; _owner = owner;
            Destroy(gameObject, _life);
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