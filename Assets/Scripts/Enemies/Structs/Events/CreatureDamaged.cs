
using UnityEngine;

namespace Incision.Enemies.Structs.Events
{
    public readonly struct CreatureDamaged 
    {
        public readonly float Damage;
        public readonly GameObject Attacker;
        public readonly Vector3? TargetPoint;

        public CreatureDamaged(DamageInfo info)
        {
            Damage = info.Amount;
            Attacker = info.Source;
            TargetPoint = info.Point;
        }
    }
}
