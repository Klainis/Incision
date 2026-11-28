using System;
using System.Collections.Generic;
using Incision.Interfaces;

namespace Incision.Arcana
{
    public sealed class ArcanaLoadout
    {
        private const int BaseSlotCount = 3;
        private readonly IEventBus _eventBus;
        private ArcanaSlot[] _slots;

        public ArcanaLoadout(IEventBus eventBus)
        {
            _eventBus = eventBus;
            CreateSlots(0);
        }

        public IReadOnlyList<ArcanaSlot> Slots { get; private set; }

        public void Reset(int unlockedExtraSlots = 0)
        {
            CreateSlots(unlockedExtraSlots);
            _eventBus.Publish(new ArcanaLoadoutChanged());
        }

        public int GetLevel(ArcanaEffect effect) => FindEffect(effect).Level;

        public float GetStrength(ArcanaEffect effect)
        {
            var slot = FindEffect(effect);
            return slot.Definition == null ? 0f : slot.Definition.Effect.GetStrengthAtLevel(slot.Level);
        }

        public float GetDuration(ArcanaEffect effect)
        {
            var slot = FindEffect(effect);
            return slot.Definition == null ? 0f : slot.Definition.Effect.Duration;
        }

        internal bool TryAddOrUpgrade(ArcanaDefinition definition, out int slotIndex)
        {
            slotIndex = Array.FindIndex(_slots, slot => slot.Definition?.Id == definition.Id);
            if (slotIndex >= 0)
            {
                _slots[slotIndex] = new ArcanaSlot(definition, _slots[slotIndex].Level + 1);
                return true;
            }

            slotIndex = Array.FindIndex(_slots, slot => slot.Definition == null);
            if (slotIndex < 0)
                return false;

            _slots[slotIndex] = new ArcanaSlot(definition, 1);
            return true;
        }

        internal bool TryReplace(int index, ArcanaDefinition definition)
        {
            if (index < 0 || index >= _slots.Length || _slots[index].Definition == null)
                return false;

            _slots[index] = new ArcanaSlot(definition, 1);
            return true;
        }

        private ArcanaSlot FindEffect(ArcanaEffect effect)
        {
            foreach (var slot in _slots)
            {
                if (slot.Definition != null && slot.Definition.Effect != null &&
                    slot.Definition.Effect.Kind == effect)
                    return slot;
            }

            return default;
        }

        private void CreateSlots(int unlockedExtraSlots)
        {
            _slots = new ArcanaSlot[BaseSlotCount + Math.Max(0, unlockedExtraSlots)];
            Slots = Array.AsReadOnly(_slots);
        }
    }
}
