using UnityEngine;

namespace Incision.Enemies.Structs
{
    public readonly struct DamageInfo
    {
        public readonly float Amount;
        public readonly GameObject Source;
        public readonly Vector3? Point;

        public DamageInfo(float amount, GameObject source = null, Vector3? point = null)
        {
            Amount = amount;
            Source = source;
            Point = point;
        }
    }
}