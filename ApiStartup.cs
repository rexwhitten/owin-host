using apistation.owin.Commands;
using apistation.owin.Depends;
using apistation.owin.Middleware;
using apistation.owin.Setup;
using LightInject;
using Microsoft.Owin;
using Newtonsoft.Json;
using Owin;
using System;
using System.Collections;

[assembly: OwinStartup(typeof(apistation.owin.ApiStartup))]

namespace apistation.owin
{
    public class ApiStartup
    {
        private readonly string _baseUrl;

        public static IServiceContainer Container = new ServiceContainer();
        
        #region Constructors

        /// <summary>
        /// api startup ctor
        /// </summary>
        /// <param name="baseUrl"></param>
        public ApiStartup(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        #endregion Constructors

        public void Configuration(IAppBuilder app)
        {
            Container = ContainerSetup.Setup(app);
            OwinSetup.Setup(app);
            ChannelSetup.Setup(app, Container);

            #region handles all api requests
            // CQRS EXECUTION
            app.Run((context =>
            {
                var router = Container.Create<IRouter>();
                var body = new Hashtable();

                context.Response.StatusCode = 404; // default status code
                context.Response.Headers.Add("Content-Type", new string[] { "application/json" });

                try
                {
                    // CQRS
                    ICommand command = router.Route(context.Request);
                    body = command.Invoke(context).Result;
                }
                catch (Exception error)
                {
                    context.Response.StatusCode = 500;
                    body.Add("error", error.ToHashtable());
                }

                return context.Response.WriteAsync(JsonConvert.SerializeObject(body));
            }));

            #endregion handles all api requests
        }
    }
}