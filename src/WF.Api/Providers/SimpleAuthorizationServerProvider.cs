using WF.Core.Data;
using WF.Service;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace WF.Api.Providers
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        UserSystem u_system = new UserSystem();
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            var methodName = context.Options;
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            var callingIP = context.Request.RemoteIpAddress;
            var user = await u_system.ValidateUser(context.UserName, context.Password);
            if (user.IsAuthenticated)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, "admin"));//remember to uncomment
                identity.AddClaim(new Claim("username", context.UserName));
                identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName.ToUpper()));
                context.Validated(identity);
            }
            else
            {
                context.SetError("Invalid Grant", $"Username and Password incorrect. {user.Error}");
                return;
            }
        }

        //public class AuthContext : IdentityDbContext<User>
        //{
        //    public AuthContext() : base("AuthContext")
        //    {

        //    }
        //}
    }

}