using System;
using System.Collections.Generic;

namespace Incision.Enemies.Runtime
{
    [Serializable]
    public class Wave
    {
        public int StartDelay = 0;
        public List<SpawnEntry> Entries = new();
    }
}