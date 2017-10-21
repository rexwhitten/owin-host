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
            // determine the search method 
            // default is to get the resource by path
            // starts-with path search will return all resources starting with the request path
            var body = new Hashtable();
            if (_cache.HashExists(context.Request.Path.Value, "@body"))
            {
                body.Add("@body", _cache.HashGet(context.Request.Path.Value, "@body"));
                context.Response.StatusCode = 200;
            }
            return Task.FromResult(body);

            #endregion http:get
        }
    }
}