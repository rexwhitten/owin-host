using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin;
using apistation.owin.Depends;

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
            return Task.FromResult(body);
            #endregion
        }
    }
}
