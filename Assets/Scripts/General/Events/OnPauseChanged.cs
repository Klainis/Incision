using Incision.Interfaces;

namespace Incision.Events
{
    public struct OnPauseChanged : IEvent
    {
        public readonly bool IsPaused;

        public string Message { get; }

        public OnPauseChanged(bool isPaused)
        {
            IsPaused = isPaused;
            Message = $"Game is paused: {IsPaused}";
        }
    }
}
