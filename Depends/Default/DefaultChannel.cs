using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apistation.owin.Depends
{
    public class DefaultChannel : IChannel
    {
        static IDictionary<Type, Delegate> _handlers = new Dictionary<Type, Delegate>();

        public DefaultChannel()
        {
            
        }

        public void Emit<TEvent>(Type typeSelector, TEvent argument)
        {
            if(_handlers.ContainsKey(typeSelector))
            {
                _handlers[typeSelector].DynamicInvoke(argument);
            }
        }

        public void RegisterHandler<TEvent>(Type typeSelector, Action<TEvent> handler)
        {
            if(!_handlers.ContainsKey(typeSelector))
            {
                _handlers.Add(typeSelector, handler);
            }
            else
            {
                _handlers[typeSelector] = handler;
            }
        }
    }
}
