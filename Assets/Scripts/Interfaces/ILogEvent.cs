namespace Incision.Interfaces
{
    public interface ILogEvent : IEvent
    {
        public string Source { get; }
    }
}
