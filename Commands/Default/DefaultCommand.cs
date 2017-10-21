using Microsoft.Owin;
using System.Collections;
using System.Threading.Tasks;

namespace apistation.owin.Commands
{
    public class DefaultCommand : ICommand
    {
        public DefaultCommand()
        {

        }

        public void Dispose()
        {

        }

        public Task<Hashtable> Invoke(IOwinContext context)
        {
            context.Response.StatusCode = 404;
            return Task.FromResult(new Hashtable());
        }
    }
}