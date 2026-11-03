using Incision.Interfaces;

namespace Incision
{
    public readonly struct OnEnemyCountChanged : IEvent
    {
        public readonly int Current;
        public readonly int Max;
        public string Message { get; }

        public OnEnemyCountChanged(int current, int max)
        {
            Current = current;
            Max = max;
            Message = $"Enemy count changed. Alive: {current}, Max: {max}";
        }

    }
}
