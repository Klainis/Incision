using Incision.Enemies.Runtime;

namespace Incision.Enemies.Structs.Events
{
    public readonly struct WaveStarted
    {
        public readonly Wave Wave;

        public WaveStarted(Wave wave)
        {
            Wave = wave;
        }
    }
}
