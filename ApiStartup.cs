using Microsoft.Owin;
using Owin;
using System;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(apistation.owin.ApiStartup))]

namespace apistation.owin
{
    using Commands;
    using Depends;
    using Depends.Default;
    using Microsoft.Owin.Security.Infrastructure;
    using Microsoft.Owin.Security.OAuth;
    using Middleware;
    using Newtonsoft.Json;
    using System.Collections;

    public class ApiStartup
    {
        private readonly string _baseUrl;
        private static IChannel _globalChannel;

        private static IChannel ApiGlobalChannel
        {
            get { return _globalChannel; }
        }

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
                Path = new PathString("/welcome")
            });

            #endregion welcome page

            #region Composition of dependecies

            ObjectFactory.Register<IAuth, DefaultAuth>();
            ObjectFactory.Register<ILog, DefaultLog>();
            ObjectFactory.Register<ICache, DefaultCache>();
            ObjectFactory.Register<IChannel, DefaultChannel>();
            ObjectFactory.Register<IRouter, DefaultRouter>();
            ObjectFactory.Register<IOAuth, DefaultOAuth>();

            // owin middleware
            app.Use(typeof(LogMiddleware), ObjectFactory.Resolve<ILog>());
            app.Use(typeof(AuthMiddleware), ObjectFactory.Resolve<IAuth>());
            app.Use(typeof(EventEmitterMiddleware), ObjectFactory.Resolve<IChannel>());

            #endregion Composition of dependecies

            #region handles all api requests

            // find modules (assemblies and load them)
            app.Run(context =>
            {
                var cache = ObjectFactory.Resolve<ICache>();
                var channel = ObjectFactory.Resolve<IChannel>();
                var router = ObjectFactory.Resolve<IRouter>();
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

            #region authentication

            IOAuth oauth = ObjectFactory.Resolve<IOAuth>();
            app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
            {
                AuthorizeEndpointPath = new PathString(ApiPathModel.AuthorizePath),
                TokenEndpointPath = new PathString(ApiPathModel.TokenPath),
                ApplicationCanDisplayErrors = true,
                // Authorization server provider which controls the lifecycle of Authorization Server
                Provider = new OAuthAuthorizationServerProvider
                {
                    OnValidateClientRedirectUri = oauth.ValidateClientRedirectUri,
                    OnValidateClientAuthentication = oauth.ValidateClientAuthentication,
                    OnGrantResourceOwnerCredentials = oauth.GrantResourceOwnerCredentials,
                    OnGrantClientCredentials = oauth.GrantClientCredetails
                },
                // Authorization code provider which creates and receives the authorization code.
                AuthorizationCodeProvider = new AuthenticationTokenProvider
                {
                    OnCreate = oauth.CreateAuthenticationCode,
                    OnReceive = oauth.ReceiveAuthenticationCode,
                },

                // Refresh token provider which creates and receives refresh token.
                RefreshTokenProvider = new AuthenticationTokenProvider
                {
                    OnCreate = oauth.CreateRefreshToken,
                    OnReceive = oauth.ReceiveRefreshToken,
                }
            });

            #endregion authentication
        }

        private Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext arg)
        {
            throw new NotImplementedException();
        }
    }
}