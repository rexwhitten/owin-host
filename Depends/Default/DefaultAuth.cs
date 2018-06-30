using Microsoft.Owin;

namespace apistation.owin.Depends
{
    public class DefaultAuth : IAuth
    {
        public DefaultAuth()
        {
        }

        public bool IsAuthenticated(IOwinRequest request)
        {
            return true;
        }
    }
}