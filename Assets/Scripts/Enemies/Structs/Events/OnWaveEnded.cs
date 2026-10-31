using Incision.Interfaces;
using Incision.Enemies.Runtime;

namespace Incision.Enemies.Structs.Events
{
    public readonly struct OnWaveEnded : IEvent
    {
        public readonly Wave Wave;
        public string Message { get; }

        public OnWaveEnded(Wave wave)
        {
            Wave = wave;
            Message = $"Wave ended";
        }
    }
}
