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
    [CommandOptions("get", "/status")]
    public class StatusGetCommand : IGetCommand
    {
        private ICache _cache;
        private readonly DateTime _start;
        private  DateTime _stop;
        private readonly DateTime _timeIndex;

        public StatusGetCommand(ICache cache)
        {
            this._cache = cache;
            this._start = DateTime.Now;
        }

        public void Dispose()
        {
            this._stop = DateTime.Now;
        }

        public Task<Hashtable> Invoke(IOwinContext context)
        {
            return Task.FromResult<Hashtable>(new Hashtable() { { "status", "up" } });
        }
    }
}
