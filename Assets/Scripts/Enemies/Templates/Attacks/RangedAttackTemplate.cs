using Incision.Enemies.Components;
using Incision.Enemies.Interfaces;
using Incision.Enemies.Runtime.Attacks;
using UnityEngine;

namespace Incision.Enemies.Templates
{
    [CreateAssetMenu(menuName = "General/Enemy/Attacks/Ranged", fileName = "RangedTemplate")]
    public class RangedAttackTemplate : AttackTemplate
    {
        [Header("Object ID")]
        [SerializeField] private string _id;

        [Header("Ranged specific parameters")]
        [SerializeField] private EnemyProjectile _projectilePrefab;
        [SerializeField, Range(1f, 100f)] private float _projectileSpeed = 14f;
        
        public string ID => _id;
        public float ProjectileSpeed => _projectileSpeed;
        public EnemyProjectile ProjectilePrefab => _projectilePrefab;

        public override IEnemyAttack CreateRuntime()
            => new RangedAttack(this);
    }
}