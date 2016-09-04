using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin;

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
