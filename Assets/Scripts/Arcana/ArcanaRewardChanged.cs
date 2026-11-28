using Incision.Interfaces;

namespace Incision.Arcana
{
    public readonly struct ArcanaRewardChanged : IEvent
    {
        public string Message => "Arcana reward selection changed.";
    }
}
