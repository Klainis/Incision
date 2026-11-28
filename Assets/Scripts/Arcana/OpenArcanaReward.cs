using Incision.Interfaces;

namespace Incision.Arcana
{
    public readonly struct OpenArcanaReward : IEvent
    {
        public string Message => "Arcana chest opened.";
    }
}
