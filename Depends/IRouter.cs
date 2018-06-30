using apistation.owin.Commands;
using Microsoft.Owin;
using Newtonsoft.Json.Linq;

namespace apistation.owin.Depends
{
    public interface IRouter
    {
        ICommand Route(IOwinRequest request);

        void Build(JObject model);
    }
}