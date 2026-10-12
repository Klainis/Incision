using UnityEngine;

namespace Incision.Enemies.Structs
{
    public readonly struct DamageInfo
    {
        public readonly float Amount;
        public readonly GameObject Source;
        public readonly Vector3 Point;
        public readonly bool IsCritical;

        public DamageInfo(float amount, GameObject source, Vector3 point, bool isCrit = false)
        {
            Amount = amount;
            Source = source;
            Point = point;
            IsCritical = isCrit;
        }
    }
}