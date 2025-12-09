using System.Net.Http.Headers;
using System.Web.Http;

namespace OrderSystem.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // ルート
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // ★★★ ここが重要 ★★★
            // XML を無効化
            config.Formatters.XmlFormatter.SupportedMediaTypes.Clear();

            // JSON をデフォルトに
            config.Formatters.JsonFormatter.SupportedMediaTypes
                .Add(new MediaTypeHeaderValue("application/json"));
        }
    }
}
