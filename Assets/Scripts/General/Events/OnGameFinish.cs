using Incision.Extensions;
using Incision.Interfaces;
using UnityEngine;

namespace Incision.Events
{
    public readonly struct OnGameFinish : IEvent
    {
        public readonly bool IsInterrupted; 
        public readonly string Cause;

        public string Message { get; }

        public OnGameFinish(bool interrupted, string cause = "")
        {
            IsInterrupted = interrupted;
            Cause = cause;
            Message = $"Game finished, was interrupted: {IsInterrupted}, by {Cause.AsTag(Color.yellow)}";
        }
    }
}
