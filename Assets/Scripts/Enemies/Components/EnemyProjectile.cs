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

        public void Launch(Vector3 dir, float speed, float damage, GameObject owner)//Наверное лучше получать через Launch data врага и потом назначать урон из нее? 
        {
            _dir = dir; _speed = speed; _damage = damage; _owner = owner;
            Destroy(gameObject, _life);
        }

        private void Update()
        {
            transform.position += _speed * Time.deltaTime * _dir;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == _owner) return;
            if (other.TryGetComponent<IDamageable>(out var d))
            {
                d.TakeDamage(new DamageInfo
                (   
                    _damage,
                    _owner,
                    transform.position,
                    IsCrit
                ));
            }
            Destroy(gameObject);
        }
    }
}