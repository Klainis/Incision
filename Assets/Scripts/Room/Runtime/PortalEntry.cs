using System;

namespace Incision.Room.Runtime
{
    [Serializable]
    public class PortalEntry
    {
        public Spawnpoint Anchor;
        [NonSerialized] public string TargetSceneName;
    }
}