using apistation.owin.Commands;
using Microsoft.Owin;

namespace apistation.owin.Depends
{
    public interface IRouter
    {
        ICommand Route(IOwinRequest request);
    }
}