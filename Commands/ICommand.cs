using Microsoft.Owin;
using System;
using System.Collections;
using System.Threading.Tasks;

namespace apistation.owin.Commands
{
    public interface ICommand : IDisposable
    {
        Task<Hashtable> Invoke(IOwinContext context);
    }
}