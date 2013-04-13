using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;
using Newtonsoft.Json;

namespace BrickPile.Samples {
    public static class WebApiConfig {
        public static void Register(HttpConfiguration config) {

            config.Formatters.JsonFormatter.SerializerSettings.TypeNameHandling = TypeNameHandling.Objects;

            config.Routes.MapHttpRoute("DefaultApiWithId", "api/{controller}/{id}", new { id = RouteParameter.Optional }, new { id = @"\d+" });
            config.Routes.MapHttpRoute("DefaultApiWithAction", "api/{controller}/{action}");
            config.Routes.MapHttpRoute("DefaultApiGet", "api/{controller}", new { action = "Get" }, new { httpMethod = new HttpMethodConstraint(HttpMethod.Get) });
            config.Routes.MapHttpRoute("DefaultApiPost", "api/{controller}", new { action = "Post" }, new { httpMethod = new HttpMethodConstraint(HttpMethod.Post) });

            //config.Routes.MapHttpRoute(
            //    name: "ActionApi",
            //    routeTemplate: "api/auth/{action}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    //routeTemplate: "api/{controller}/{*id}",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);


        }
    }
}
