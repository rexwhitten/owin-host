using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;

namespace apistation.owin.Depends.Default
{
    public class DefaultOAuth : IOAuth
    {
        private ICache _cache;

        public DefaultOAuth()
        {
            _cache = new DefaultCache();
        }

        public DefaultOAuth(ICache cache)
        {
            _cache = cache;
        }

        public void CreateAuthenticationCode(AuthenticationTokenCreateContext obj)
        {
            
        }

        public void CreateRefreshToken(AuthenticationTokenCreateContext obj)
        {
            
        }

        public Task GrantClientCredetails(OAuthGrantClientCredentialsContext arg)
        {
            return Task.Delay(10);
        }

        public Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext arg)
        {
            return Task.Delay(10);
        }

        public void ReceiveAuthenticationCode(AuthenticationTokenReceiveContext obj)
        {
            
        }

        public void ReceiveRefreshToken(AuthenticationTokenReceiveContext obj)
        {
            
        }

        public Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext arg)
        {
            return Task.Delay(10);
        }

        public Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext arg)
        {
            return Task.Delay(10);
        }
    }
}
