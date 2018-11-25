using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using apistation.owin.Depends;
using apistation.owin.Middleware;
using Microsoft.Owin;
using Owin;
using LightInject;

namespace apistation.owin.Setup
{
    public class OwinSetup
    {
        internal static Task Setup(IAppBuilder app)
        {
            app.UseWelcomePage(new Microsoft.Owin.Diagnostics.WelcomePageOptions()
            {
                Path = new PathString("/")
            });

            // owin middleware
            
            app.Use(typeof(LogMiddleware), ApiStartup.Container.GetInstance<ILog>());
            app.Use(typeof(AuthMiddleware), ApiStartup.Container.GetInstance<IAuth>());
            app.Use(typeof(EventEmitterMiddleware), ApiStartup.Container.GetInstance<IChannel>());

            return Task.Delay(1);
        }
    }
}
