using apistation.owin.Commands;
using apistation.owin.Depends;
using apistation.owin.Middleware;
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
            app.UseWelcomePage(new Microsoft.Owin.Diagnostics.WelcomePageOptions()
            {
                Path = new PathString("/")
            });

            #region Composition of dependecies

            Container.Register<IAuth, DefaultAuth>();
            Container.Register<ILog, DefaultLog>();
            Container.Register<ICache, DefaultCache>();
            Container.Register<IChannel, DefaultChannel>();
            Container.Register<IRouter, DefaultCommandRouter>();

            // commands
            Container.Register<IGetCommand, DefaultGetCommand>();
            Container.Register<IPostCommand, DefaultPostCommand>();
            Container.Register<IPutCommand, DefaultPutCommand>();
            Container.Register<IDeleteCommand, DefaultDeleteCommand>();

            // commands specialized
            Container.Register<IGetCommand, StatusGetCommand>("default");

            // owin middleware
            app.Use(typeof(LogMiddleware), Container.GetInstance<ILog>());
            app.Use(typeof(AuthMiddleware), Container.GetInstance<IAuth>());
            app.Use(typeof(EventEmitterMiddleware), Container.GetInstance<IChannel>());

            #endregion Composition of dependecies

            #region handles all api requests

            app.Run(context =>
            {
                var cache = Container.Create<ICache>();
                var channel = Container.Create<IChannel>();
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
            });

            #endregion handles all api requests
        }
    }
}