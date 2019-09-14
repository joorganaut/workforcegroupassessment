using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web.Cors;
using System.Web.Http.WebHost;
using System.Web.Http;
using System.Web.Configuration;
using Microsoft.Owin.Cors;
using System.Threading;
using Microsoft.Owin.Security.OAuth;
using WF.Api.Providers;
using System.Configuration;
using WF.Api;

[assembly: OwinStartup(typeof(WF.api.App_Start.Startup))]
namespace WF.api.App_Start
{
    public class Startup
    {
        string post_timeout = ConfigurationManager.AppSettings["post_timeout"];
        string token_timeout = ConfigurationManager.AppSettings["token_timeout"];
        public void Configuration(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions options = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromSeconds(Convert.ToDouble(token_timeout)),
                Provider = new SimpleAuthorizationServerProvider()
            };
            OAuthAuthorizationServerOptions posting_options = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/posting/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromSeconds(Convert.ToDouble(post_timeout)),
                Provider = new SimpleAuthorizationServerProvider(),
                
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(options);
            app.UseOAuthAuthorizationServer(posting_options);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);
            app.UseCors(CorsOptions.AllowAll);
            app.UseWebApi(config);
        }
    }

}