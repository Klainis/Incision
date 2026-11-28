using Incision.Interfaces;

namespace Incision.Events
{
    public readonly struct OnPlayerDashStarted : IEvent
    {
        public float StartedAt { get; }
        public string Message => "Player dash started.";

        public OnPlayerDashStarted(float startedAt)
        {
            StartedAt = startedAt;
        }
    }
}
