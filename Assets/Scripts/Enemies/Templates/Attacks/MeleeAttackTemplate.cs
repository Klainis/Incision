using Incision.Enemies.Interfaces;
using Incision.Enemies.Runtime.Attacks;
using UnityEngine;

namespace Incision.Enemies.Templates
{
    [CreateAssetMenu(menuName = "Enemies/Attacks/Melee", fileName = "MeleeTemplate")]
    public class MeleeAttackTemplate : AttackTemplate
    {
        public override IEnemyAttack CreateRuntime()
            => new MeleeAttack(this);
    }
}