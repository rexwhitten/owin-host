using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace apistation.owin.Depends
{
    public interface IAuth
    {
        /// <summary>
        /// Authenticate Request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        bool IsAuthenticated(IOwinRequest request);
    }
}
