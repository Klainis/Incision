using Incision.Enemies.Components;
using Incision.Enemies.Interfaces;
using Incision.Enemies.Runtime.Attacks;
using UnityEngine;

namespace Incision.Enemies.Templates
{
    [CreateAssetMenu(menuName = "Enemies/Attacks/Ranged", fileName = "RangedTemplate")]
    public class RangedAttackTemplate : AttackTemplate
    {
        [SerializeField] private string _animationTrigger = "Shoot";
        [SerializeField] private float _projectileSpeed = 14f;
        [SerializeField] private EnemyProjectile _projectilePrefab;

        public override IEnemyAttack CreateRuntime()
            => new RangedAttack(Damage, _projectileSpeed, _projectilePrefab, _animationTrigger, Cooldown, Range);
    }
}