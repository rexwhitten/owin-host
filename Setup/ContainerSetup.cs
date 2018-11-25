using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using apistation.owin.Commands;
using apistation.owin.Depends;
using LightInject;
using Owin;

namespace apistation.owin.Setup
{
    internal class ContainerSetup
    {
        internal static IServiceContainer Setup(IAppBuilder app)
        {
            var container = new ServiceContainer();

            container.Register<IAuth, DefaultAuth>();
            container.Register<ILog, DefaultLog>();
            container.Register<ICache, DefaultCache>();
            container.Register<IChannel, DefaultChannel>();
            container.Register<IRouter, DefaultCommandRouter>();

            // commands
            container.Register<IGetCommand, DefaultGetCommand>();
            container.Register<IPostCommand, DefaultPostCommand>();
            container.Register<IPutCommand, DefaultPutCommand>();
            container.Register<IDeleteCommand, DefaultDeleteCommand>();

            // commands specialized
            container.Register<IGetCommand, StatusGetCommand>("default");

            return container;
        }
    }
}
