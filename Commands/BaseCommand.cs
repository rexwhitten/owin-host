using Microsoft.Owin;
using System;
using System.Collections;
using System.Threading.Tasks;

namespace apistation.owin.Commands
{
    public class BaseCommand<TResult> : ICommand
    {
        private readonly Func<IOwinContext, Hashtable> _command;

        public BaseCommand(Func<IOwinContext, Hashtable> command)
        {
            this._command = command;
        }

        public void Dispose()
        {
        }

        public Task<Hashtable> Invoke(IOwinContext context)
        {
            Console.WriteLine(string.Format("Command::Invoke::{0}::{1}", this.GetType(), context.Request.Path));
            return Task.FromResult(_command.Invoke(context));
        }
    }
}