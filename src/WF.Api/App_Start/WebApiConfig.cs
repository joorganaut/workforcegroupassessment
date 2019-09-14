using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WF.Api.Handlers;

namespace WF.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.MessageHandlers.Add(new ValidatorHandler());
            config.MessageHandlers.Add(new LogRequestAndResponseHandler());
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
