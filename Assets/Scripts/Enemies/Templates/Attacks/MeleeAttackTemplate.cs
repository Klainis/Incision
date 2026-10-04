using Incision.Enemies.Interfaces;
using Incision.Enemies.Runtime.Attacks;
using UnityEngine;

namespace Incision.Enemies.Templates
{
    [CreateAssetMenu(menuName = "Enemies/Attacks/Melee", fileName = "MeleeTemplate")]
    public class MeleeAttackTemplate : AttackTemplate
    {
        [SerializeField] private string _animationTrigger = "Attack";

        public override IEnemyAttack CreateRuntime()
            => new MeleeAttack(Damage, _animationTrigger, Cooldown, Range);
    }
}