using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace apistation.owin.Middleware
{
    using Depends;
    using Microsoft.Owin;
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class EventEmitterMiddleware
    {
        private readonly AppFunc _next;
        private IChannel _channel;

        public EventEmitterMiddleware(AppFunc next, IChannel channel)
        {
            if (next == null)
            {
                throw new ArgumentNullException("next");
            }

            if (channel == null)
            {
                throw new ArgumentNullException("channel");
            }

            _channel = channel;
            _next = next;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            try
            {
                // lets check and apply handlers waitiing
                var context = new OwinContext(environment);
                _channel.Emit(typeof(IOwinContext), context);
            }
            catch (Exception x)
            {
                if (_channel != null)
                {
                    _channel.Emit(x.GetType(), x);
                }
            }
            finally
            {
                await _next(environment); // continue_channel
            }
        }
    }
}