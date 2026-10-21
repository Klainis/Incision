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

        public void Launch(Vector3 dir, float speed, float damage, GameObject owner)
        {
            _dir = dir; _speed = speed; _damage = damage; _owner = owner;
            Destroy(gameObject, _life);
        }

        private void Update()
        {
            transform.position += _speed * Time.deltaTime * _dir;
        }
        
        protected override void OnTargetCollision(Collision collision, IDamageable damageable)
        {
            SpawnEffectOnDestroy();
            damageable.TakeDamage(new DamageInfo
            (
                _damage,
                _owner,
                transform.position,
                IsCrit
            ));
        }

        // private void OnTriggerEnter(Collider other)
        // {
        //     Debug.Log($"Collision with {other.gameObject.name}");
        //     if (other.gameObject == _owner) return;
        //     if (other.TryGetComponent<IDamageable>(out var d))
        //     {
        //         d.TakeDamage(new DamageInfo
        //         (   
        //             _damage,
        //             _owner,
        //             transform.position,
        //             IsCrit
        //         ));
        //     }
        //     Destroy(gameObject);
        // }
    }
}