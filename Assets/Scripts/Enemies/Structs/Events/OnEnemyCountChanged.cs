using Incision.Interfaces;
using Incision.Enemies.Components;

namespace Incision
{
    public readonly struct OnEnemyCountChanged : IEvent
    {
        public readonly int Current;
        public readonly int Max;
        public readonly EnemySpawner Spawner;
        public readonly bool WavesFinished;
        public string Message { get; }

        public OnEnemyCountChanged(EnemySpawner spawner, int current, int max, bool wavesFinished)
        {
            Current = current;
            Max = max;
            Spawner = spawner;
            WavesFinished = wavesFinished;
            Message = $"Enemy count changed. Alive: {current}, Max: {max}";
        }

    }
}
