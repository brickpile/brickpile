using System.Web.Http;
using Newtonsoft.Json;

namespace BrickPile.Samples {
    public static class WebApiConfig {
        public static void Register(HttpConfiguration config) {

            config.Formatters.JsonFormatter.SerializerSettings.TypeNameHandling = TypeNameHandling.Objects;

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{*id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
