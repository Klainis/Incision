using System;
using Incision.Events;
using Incision.Interfaces;

namespace Incision.Arcana
{
    public sealed class ArcanaCombatModifiers : IDisposable
    {
        private readonly ArcanaLoadout _loadout;
        private readonly IEventBus _events;
        private float _chariotExpiresAt = float.NegativeInfinity;

        public ArcanaCombatModifiers(ArcanaLoadout loadout, IEventBus events)
        {
            _loadout = loadout;
            _events = events;
            _events.Subscribe<OnPlayerDashStarted>(OnDashStarted);
            _events.Subscribe<ArcanaLoadoutChanged>(OnLoadoutChanged);
        }

        public float GetDamageMultiplier(int cardsSpent, int cycleCardCount, bool isLastCharge)
        {
            float cycleProgress = cycleCardCount > 1
                ? Math.Min(1f, (float)cardsSpent / (cycleCardCount - 1))
                : 1f;
            float hermitMultiplier = 1f + _loadout.GetStrength(ArcanaEffect.Hermit) * cycleProgress;
            float moonMultiplier = isLastCharge ? 1f + _loadout.GetStrength(ArcanaEffect.Moon) : 1f;
            return hermitMultiplier * moonMultiplier;
        }

        public float GetAttackSpeedMultiplier(float now) => GetRemainingDuration(ArcanaEffect.Chariot, now) > 0f
            ? 1f + _loadout.GetStrength(ArcanaEffect.Chariot)
            : 1f;

        public float GetRemainingDuration(ArcanaEffect effect, float now) => effect == ArcanaEffect.Chariot
            ? Math.Max(0f, _chariotExpiresAt - now)
            : 0f;

        public void Dispose()
        {
            _events.Unsubscribe<OnPlayerDashStarted>(OnDashStarted);
            _events.Unsubscribe<ArcanaLoadoutChanged>(OnLoadoutChanged);
        }

        private void OnDashStarted(OnPlayerDashStarted evt)
        {
            if (_loadout.GetLevel(ArcanaEffect.Chariot) > 0)
                _chariotExpiresAt = evt.StartedAt + _loadout.GetDuration(ArcanaEffect.Chariot);
        }

        private void OnLoadoutChanged(ArcanaLoadoutChanged evt)
        {
            if (_loadout.GetLevel(ArcanaEffect.Chariot) == 0)
                _chariotExpiresAt = float.NegativeInfinity;
        }
    }
}
