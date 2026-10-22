using System;
using Incision.Enemies.Templates;
using UnityEngine;

namespace Incision.Enemies.Runtime
{
    [Serializable]
    public class SpawnEntry
    {
        public EnemyTemplate Enemy;
        [Min(0)] public int Count = 1;
        [Min(0)] public int Interval = 500;
    }
}