using Incision.Interfaces;
using Incision.Extensions;
using UnityEngine;  

namespace Incision.Enemies.Structs.Events
{
    public readonly struct OnCreatureDied : IEvent
    {
        public readonly float Damage;
        public readonly GameObject Attacker;
        public readonly GameObject Target;
        public string Message { get; }

        public OnCreatureDied(DamageInfo info)
        {
            Damage = info.Amount;
            Attacker = info.Source;
            Target = info.Target;

            var targetName = $"{(Target is null  ? "Unknown" : Target.name)}".AsTag(Color.yellow);
            var attackerName = $"{(Attacker is null ? "Unknown" : Attacker.name)}".AsTag(Color.cyan);
            Message = $"Target at {targetName} died by {attackerName}";
        }
    }
}
