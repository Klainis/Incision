using Incision.Interfaces;

namespace Incision.Structs.Events
{
    public readonly struct Log : ILogEvent
    {
        public string Source { get; }
        public string Message { get; }

        public Log(string source, string log)
        {
            Source = source;
            Message = log;
        }
    }
}
