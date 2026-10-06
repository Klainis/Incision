using System;
using System.Collections.Generic;
using Incision.Enemies.Interfaces;
using UnityEngine;

namespace Incision
{
    public class EventBus : IEventBus
    {
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
            Debug.Log($"<color=green>[Event Bus]</color>: Event <color=lime>{typeof(T).Name}</color> invoked");
            if (_handlers.TryGetValue(typeof(T), out var d))
                (d as Action<T>)?.Invoke(evt);
        }
    }
}
