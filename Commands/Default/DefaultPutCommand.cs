using apistation.owin.Depends;
using apistation.owin.Models;
using Microsoft.Owin;
using System.Collections;
using System.Threading.Tasks;

namespace apistation.owin.Commands
{
    [CommandOptions("put", "/*")]
    public class DefaultPutCommand : IPutCommand
    {
        private readonly ICache _cache;

        public DefaultPutCommand(ICache cache)
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
                switch (context.Request.ContentType)
                {
                    default: // json handler is the default
                        var input = context.Request.Body.ReadAsString();
                        body.Add("result", _cache.HashSet(context.Request.Path.Value, new EntryModel[1] {
                                            new EntryModel("@body", input.ToString())
                                        }));
                        context.Response.StatusCode = 202;
                        break;
                }
            }

            return Task.FromResult(body);
        }
    }
}