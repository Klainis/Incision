using Incision.Interfaces;

namespace Incision.Arcana
{
    public readonly struct ArcanaLoadoutChanged : IEvent
    {
        public string Message => "Arcana loadout changed.";
    }
}
