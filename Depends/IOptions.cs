using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apistation.owin.Depends
{
    public interface IOptions
    {
        string AppStartup { get; }
        string Url { get; }
        string SwaggerPath { get; }
    }
}
