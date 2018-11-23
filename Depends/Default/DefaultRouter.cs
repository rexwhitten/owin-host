using apistation.owin.Commands;
using LightInject;
using Microsoft.Owin;

namespace apistation.owin.Depends
{
    public class DefaultCommandRouter : IRouter
    {
        private readonly ILog _log;

        public DefaultCommandRouter(ILog log)
        {
            _log = log;
        }

        public ICommand Route(IOwinRequest request)
        {
            ICommand cmd = ApiStartup.Container.GetInstance<IGetCommand>("default");

            switch (request.Method.ToLower())
            {
                case "get":
                    cmd = ApiStartup.Container.Create<IGetCommand>();
                    break;

                case "post":
                    cmd = ApiStartup.Container.Create<IPostCommand>();
                    break;

                case "put":
                    cmd = ApiStartup.Container.Create<IPutCommand>();
                    break;

                case "delete":
                    cmd = ApiStartup.Container.Create<IDeleteCommand>();
                    break;

                default:
                    break;
            }

            return cmd;
        }
    }
}