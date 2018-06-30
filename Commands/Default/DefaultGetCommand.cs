using apistation.owin.Depends;
using Microsoft.Owin;
using System.Collections;
using System.Threading.Tasks;

namespace apistation.owin.Commands
{
    [CommandOptions("get", "/*")]
    public class DefaultGetCommand : IGetCommand
    {
        private readonly ICache _cache;

        public DefaultGetCommand(ICache cache)
        {
            _cache = cache;
        }

        public void Dispose()
        {
        }

        public Task<Hashtable> Invoke(IOwinContext context)
        {
            #region http:get

            var body = new Hashtable();
            if (_cache.HashExists(context.Request.Path.Value, "@body"))
            {
                body.Add("results", _cache.HashGet(context.Request.Path.Value, "@body"));
                context.Response.StatusCode = 200;
            }
            else
            {
                var result = _cache.HashGet(context.Request.Path.Value, "@body");
                if (result == null || result.Count == 0)
                {
                    context.Response.StatusCode = 404;
                }
                else
                {
                    body.Add("results", result);
                    context.Response.StatusCode = 200;
                }
            }
            return Task.FromResult(body);

            #endregion http:get
        }
    }
}