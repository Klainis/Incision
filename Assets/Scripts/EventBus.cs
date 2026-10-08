using System;
using System.Collections.Generic;
using Incision.Interfaces;
using Incision.Extensions;
using UnityEngine;

namespace Incision
{
    public class EventBus : IEventBus, ITaggable
    {
        public string LOG_TAG { get; } = "[Event Bus]".AsTag(Color.green).Bold();
        
        private readonly Dictionary<Type, Delegate> _handlers = new();

        public void Subscribe<T>(Action<T> handler)
        {
            var t = typeof(T);
            if (_handlers.TryGetValue(t, out var existing))
                _handlers[t] = Delegate.Combine(existing, handler);
            else
                _handlers[t] = handler;
        }

        public void Unsubscribe<T>(Action<T> handler)
        {
            var t = typeof(T);
            if (_handlers.TryGetValue(t, out var existing))
                _handlers[t] = Delegate.Remove(existing, handler);
        }

        public void Publish<T>(T evt)
        {
            switch (evt)
            {
                case ILogEvent levt:
                    Log(levt);
                    return;
                case IEvent eve:
                    Debug.Log($"{LOG_TAG}: {eve.Message}");
                    break;
                default:
                    Debug.LogWarning($"{LOG_TAG}: Event {typeof(T).Name.AsTag(Color.green)} invoked");
                    break;
            }

            if (_handlers.TryGetValue(typeof(T), out var d))
                (d as Action<T>)?.Invoke(evt);
        }

        private void Log(ILogEvent levt)
        {
            Debug.Log($"{levt.Source}: {levt.Message}");
        }
    }
}
