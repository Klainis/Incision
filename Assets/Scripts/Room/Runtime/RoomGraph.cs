using System.Collections.Generic;

namespace Incision.Room.Runtime
{
    public class RoomGraph
    {
        public Dictionary<string, RoomNode> Nodes = new();
        public string StartNodeId;

        public List<List<RoomNode>> Layers = new(); // For Debug
    }
}
