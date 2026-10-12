using Incision.Interfaces;

namespace Incision.Structs.Events
{
    public readonly struct LogError : ILogEvent
    {
        public string Source { get; }
        public string Message { get; }

        public LogError(string source, string log)
        {
            Source = source;
            Message = log;
        }
    }
}
