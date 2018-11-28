using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace apistation.owin.Depends
{
    public class DefaultChannel : IChannel
    {
        private static IDictionary<Type, Collection<Delegate>> _handlers = new Dictionary<Type, Collection<Delegate>>();

        public DefaultChannel()
        {
        }

        public void Emit<TEvent>(TEvent argument)
        {
            if (_handlers.ContainsKey(typeof(TEvent)))
            {
                foreach(var handler in _handlers[typeof(TEvent)])
                {
                    handler.DynamicInvoke(argument);
                }
            }
        }

        public void RegisterHandler<TEvent>(Action<TEvent> handler)
        {
            if (!_handlers.ContainsKey(typeof(TEvent)))
            {
                _handlers.Add(typeof(TEvent), new Collection<Delegate>());
            }

            _handlers[typeof(TEvent)].Add(handler);
        }
    }
}