using System;
using System.Collections.Generic;
using Incision.Interfaces;

namespace Incision.Arcana
{
    public sealed class ArcanaRewardController : IDisposable
    {
        private const int OfferCount = 3;
        private readonly ArcanaCatalog _catalog;
        private readonly ArcanaLoadout _loadout;
        private readonly IEventBus _eventBus;

        public ArcanaRewardController(ArcanaCatalog catalog, ArcanaLoadout loadout, IEventBus eventBus)
        {
            _catalog = catalog;
            _loadout = loadout;
            _eventBus = eventBus;
            _eventBus.Subscribe<OpenArcanaReward>(OnOpenReward);
        }

        public IReadOnlyList<ArcanaDefinition> Offers { get; private set; } = Array.Empty<ArcanaDefinition>();
        public ArcanaDefinition Selected { get; private set; }
        public bool IsOpen => Offers.Count > 0;

        public void Select(int index)
        {
            if (Selected != null || index < 0 || index >= Offers.Count)
                return;

            var offer = Offers[index];
            if (_loadout.TryAddOrUpgrade(offer, out _))
            {
                FinishSelection();
                return;
            }

            Selected = offer;
            _eventBus.Publish(new ArcanaRewardChanged());
        }

        public void Replace(int slotIndex)
        {
            if (Selected != null && _loadout.TryReplace(slotIndex, Selected))
                FinishSelection();
        }

        public void Decline()
        {
            if (!IsOpen)
                return;

            Offers = Array.Empty<ArcanaDefinition>();
            Selected = null;
            _eventBus.Publish(new ArcanaRewardChanged());
        }

        public void Dispose() => _eventBus.Unsubscribe<OpenArcanaReward>(OnOpenReward);

        private void OnOpenReward(OpenArcanaReward evt)
        {
            if (IsOpen)
                return;

            Offers = _catalog.DrawOffers(OfferCount);
            if (IsOpen)
                _eventBus.Publish(new ArcanaRewardChanged());
        }

        private void FinishSelection()
        {
            Decline();
            _eventBus.Publish(new ArcanaLoadoutChanged());
        }
    }
}
