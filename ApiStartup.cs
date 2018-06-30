using Microsoft.Owin;
using Owin;
using System;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(apistation.owin.ApiStartup))]

namespace apistation.owin
{
    using Commands;
    using Depends;
    using LightInject;
    using Middleware;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System.Collections;
    using System.IO;

    public class ApiStartup
    {
        private readonly string _baseUrl;
        internal static IServiceContainer Container = new ServiceContainer();

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
            #region welcome page

            app.UseWelcomePage(new Microsoft.Owin.Diagnostics.WelcomePageOptions()
            {
                Path = new PathString("/")
            });

            #endregion welcome page

            #region Composition of dependecies

            Container.Register<IAuth, DefaultAuth>();
            Container.Register<ILog, DefaultLog>();
            Container.Register<ICache, DefaultCache>();
            Container.Register<IChannel, DefaultChannel>();
            Container.Register<IRouter, DefaultCommandRouter>();

            // owin middleware
            app.Use(typeof(LogMiddleware), Container.GetInstance<ILog>());
            app.Use(typeof(AuthMiddleware), Container.GetInstance<IAuth>());
            app.Use(typeof(EventEmitterMiddleware), Container.GetInstance<IChannel>());

            #endregion Composition of dependecies

            #region handles all api requests

            // find modules (assemblies and load them)

            app.Run(context =>
            {
                var cache = Container.Create<ICache>();
                var channel = Container.Create<IChannel>();
                var router = Container.Create<IRouter>();
                var body = new Hashtable();

                context.Response.StatusCode = 404; // default status code
                context.Response.Headers.Add("Content-Type", new string[] { "application/json" });
                
                // Swagger Model
                var model = JObject.Parse(File.ReadAllText(ApiOptions.SwaggerPath));

                router.Build(model);

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