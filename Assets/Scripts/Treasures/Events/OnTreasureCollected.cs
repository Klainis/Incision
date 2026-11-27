using Incision.Interfaces;
using Incision.Treasures;

namespace Incision.Structs.Events
{
    public readonly struct OnGoldCollected : IEvent
    {
        public GoldTreasureTemplate Treasure { get; }
        
        public int Quantity { get; }
        public string Message { get; }

        public OnGoldCollected(GoldTreasureTemplate treasure, int quantity)
        {
            Treasure = treasure;
            Quantity = quantity;
            Message = $"Gold collected: {treasure.name} with {quantity}";
        }
    }
}
