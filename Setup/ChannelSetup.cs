using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using apistation.owin.Depends;
using apistation.owin.Events;
using apistation.owin.Handlers;
using LightInject;
using Microsoft.Owin;
using Owin;

namespace apistation.owin.Setup
{
    public class ChannelSetup
    {
        internal static void Setup(IAppBuilder app, IServiceContainer container)
        {
            IChannel channel = container.GetInstance<IChannel>();

            channel.RegisterHandler<IOwinRequest>((r) =>
            {
                Console.WriteLine("Channel Handler 1");
            });

            channel.RegisterHandler<IOwinRequest>((r) =>
            {
                Console.WriteLine("Channel Handler 2");
            });
            channel.RegisterHandler<IOwinRequest>( (r) =>
            {
                Console.WriteLine("Channel Handler 3");
            });
            channel.RegisterHandler<IOwinRequest>( (r) =>
            {
                Console.WriteLine("Channel Handler 4");
            });


            channel.RegisterHandler<NotFoundEvent>(new NotFoundEventHandler().Handler);
        }
    }
}
