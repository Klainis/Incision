using System;
using Incision.Enemies.Templates;

namespace Incision.Enemies.Runtime
{
    [Serializable]
    public class SpawnEntry
    {
        public EnemyTemplate Enemy;
        public int Count = 1;
        public int Interval = 500;
    }
}