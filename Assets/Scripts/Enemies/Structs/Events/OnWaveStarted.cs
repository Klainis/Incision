using Incision.Interfaces;
using Incision.Enemies.Runtime;

namespace Incision.Enemies.Structs.Events
{
    public readonly struct OnWaveStarted : IEvent
    {
        public readonly Wave Wave;
        public string Message { get; }

        public OnWaveStarted(Wave wave)
        {
            Wave = wave;
            Message = $"Wave started with {Wave.Entries.Count} entries";
        }
    }
}
