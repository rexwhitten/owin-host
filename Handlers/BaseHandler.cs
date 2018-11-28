using apistation.owin.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apistation.owin.Handlers
{
    public class BaseHandler<TEvent> where TEvent : BaseEvent
    {
        private readonly Action<TEvent> _handler;

        public BaseHandler(Action<TEvent> handler)
        {
            _handler = handler;
        }

        public Action<TEvent> Handler { get { return this._handler; } }
    }
}
