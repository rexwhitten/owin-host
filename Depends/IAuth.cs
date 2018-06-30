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