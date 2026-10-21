using System;
using System.Collections.Generic;
using UnityEngine;

namespace Incision.Enemies.Runtime
{
    [Serializable]
    public class Wave
    {
        [Min(0f)] public int StartDelay = 0;
        public List<SpawnEntry> Entries = new();
    }
}