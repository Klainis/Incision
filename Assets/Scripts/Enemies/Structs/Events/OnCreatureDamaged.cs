
using Incision.Interfaces;
using Incision.Extensions;
using UnityEngine;

namespace Incision.Enemies.Structs.Events
{
    public readonly struct OnCreatureDamaged : IEvent
    {
        public readonly float Damage;
        public readonly GameObject Attacker;
        public readonly Vector3? TargetPoint;
        public string Message { get; }

        public OnCreatureDamaged(DamageInfo info)
        {
            Damage = info.Amount;
            Attacker = info.Source;
            TargetPoint = info.Point;
            var targetName = $"{(TargetPoint is null ? "Unknown" : TargetPoint)}".AsTag(Color.yellow);
            var attackerName = $"{(Attacker is null ? "Unknown" : Attacker.name)}".AsTag(Color.cyan);
            Message = $"{attackerName} attacked {targetName}";
        }
    }
}
