using Incision.Interfaces;
using Incision.Room.Runtime;

namespace Incision.Treasures.Events
{
    public struct SpawnTreasure : IEvent
    {
        public readonly RoomNode Node;
        public string Message { get; }
        
        public SpawnTreasure(RoomNode node)
        {
            Node = node;
            Message = $"Spawn treasure in {node.Id}";
        }
    }
}
