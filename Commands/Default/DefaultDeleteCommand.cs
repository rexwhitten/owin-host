using apistation.owin.Depends;
using Microsoft.Owin;
using System.Collections;
using System.Threading.Tasks;

namespace apistation.owin.Commands
{
    [CommandOptions("delete", "/*")]
    public class DefaultDeleteCommand : ICommand
    {
        private readonly ICache _cache;

        public DefaultDeleteCommand(ICache cache)
        {
            _cache = cache;
        }

        public void Dispose()
        {
        }

        public Task<Hashtable> Invoke(IOwinContext context)
        {
            var body = new Hashtable();
            if (_cache.HashExists(context.Request.Path.Value, "@body"))
            {
                body.Add("result", _cache.HashDelete(context.Request.Path.Value, "@body"));
                context.Response.StatusCode = 200;
            }
            else
            {
                context.Response.StatusCode = 404;
            }

            return Task.FromResult(body);
        }
    }
}