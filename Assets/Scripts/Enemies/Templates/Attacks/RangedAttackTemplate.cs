using Incision.Enemies.Components;
using Incision.Enemies.Interfaces;
using Incision.Enemies.Runtime.Attacks;
using UnityEngine;

namespace Incision.Enemies.Templates
{
    [CreateAssetMenu(menuName = "Enemies/Attacks/Ranged", fileName = "RangedTemplate")]
    public class RangedAttackTemplate : AttackTemplate
    {
        [Header("Ranged specific parameters")]
        [SerializeField] private EnemyProjectile _projectilePrefab;
        [SerializeField, Range(1f, 100f)] private float _projectileSpeed = 14f;
        
        public float ProjectileSpeed => _projectileSpeed;
        public EnemyProjectile ProjectilePrefab => _projectilePrefab;

        public override IEnemyAttack CreateRuntime()
            => new RangedAttack(this);
    }
}