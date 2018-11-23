using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OAuth;
using System.Threading.Tasks;

namespace apistation.owin.Depends
{
    public interface IOAuth
    {
        Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext arg);

        Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext arg);

        Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext arg);

        Task GrantClientCredetails(OAuthGrantClientCredentialsContext arg);

        void CreateAuthenticationCode(AuthenticationTokenCreateContext obj);

        void ReceiveAuthenticationCode(AuthenticationTokenReceiveContext obj);

        void CreateRefreshToken(AuthenticationTokenCreateContext obj);

        void ReceiveRefreshToken(AuthenticationTokenReceiveContext obj);
    }
}