using UnityEngine;

namespace Incision.Enemies.Structs
{
    public readonly struct DamageInfo
    {
        public readonly float Amount;
        public readonly GameObject Source;
        public readonly GameObject Target;
        public readonly bool IsCritical;

        public DamageInfo(float amount, GameObject source, GameObject target, bool isCrit = false)
        {
            Amount = amount;
            Source = source;
            Target = target;
            IsCritical = isCrit;
        }
    }
}