using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using apistation.owin.Commands;
using Microsoft.Owin;

namespace apistation.owin.Depends
{
    public interface IRouter
    {
        ICommand Route(IOwinRequest request);
    }
}
