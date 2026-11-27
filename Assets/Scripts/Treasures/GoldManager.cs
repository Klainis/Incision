using System;
using Incision.Interfaces;
using Incision.Structs.Events;

namespace Incision
{
    public sealed class GoldManager : IDisposable
    {
        private readonly IEventBus _eventBus;

        public int Amount { get; private set; }

        public GoldManager(IEventBus eventBus)
        {
            _eventBus = eventBus;
            _eventBus.Subscribe<OnGoldCollected>(OnTreasureCollected);
        }

        public void Reset() => Amount = 0;

        public void Dispose() => _eventBus.Unsubscribe<OnGoldCollected>(OnTreasureCollected);

        private void OnTreasureCollected(OnGoldCollected evt) => Amount += evt.Quantity;
    }
}
