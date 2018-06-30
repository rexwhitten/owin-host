using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using apistation.owin.Depends;
using apistation.owin.Models;
using Microsoft.Owin;
using Newtonsoft.Json.Linq;

namespace apistation.owin.Commands.Policy
{
    [CommandOptions("POST","/Policy")]
    public class PolicyPostCommand : IPostCommand
    {
        private ILog _log;
        private ICache _cache;

        public PolicyPostCommand(ILog log, ICache cache)
        {
            _log = log;
            _cache = cache;
        }

        public void Dispose()
        {
        }

        public Task<Hashtable> Invoke(IOwinContext context)
        {
            var body = new Hashtable();
            _log.Log("Creating policy");
            if (!_cache.HashExists(context.Request.Path.Value, "@body"))
            {
                switch (context.Request.ContentType)
                {
                    default:
                        var input = context.Request.Body.ReadAsString();
                        body.Add("result", _cache.HashSet(context.Request.Path.Value, new EntryModel[1] {
                                            new EntryModel("@body", input)
                                        }));
                        _log.Log("Policy created successful");
                        context.Response.StatusCode = 202;
                        break;
                }
            }
            else
            {
                context.Response.StatusCode = 400;
            }

            return Task.FromResult(body);
        }
    }
}
