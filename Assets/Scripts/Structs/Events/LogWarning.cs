using Incision.Interfaces;

namespace Incision.Structs.Events
{
    public readonly struct LogWarning : ILogEvent
    {
        public string Source { get; }
        public string Message { get; }

        public LogWarning(string source, string log)
        {
            Source = source;
            Message = log;
        }
    }
}
