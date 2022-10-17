using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace TwiColle
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 設定和服務

            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "Photo",
                routeTemplate: "api/photo/{id}",
                defaults: new { controller = "Photo", id = RouteParameter.Optional }
                );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{name}",
                defaults: new { name = RouteParameter.Optional }
                );
        }
    }
}
