using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using BrickPile.Sample.App_Start;
using BrickPile.UI.Web.Hosting;
using FilterConfig = BrickPile.Sample.App_Start.FilterConfig;

namespace BrickPile.Sample {
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication {
        protected void Application_Start() {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

            // When we use a static file handler for the system thumbnails we need to ignore this path
            //RouteTable.Routes.IgnoreRoute("static/{*pathInfo}");
            
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //BrickPile.

            // Register the amazon s3 virtual path provider
            //HostingEnvironment.RegisterVirtualPathProvider(new AmazonS3VirtualPathProvider());
            HostingEnvironment.RegisterVirtualPathProvider(new NativeVirtualPathProvider());
        }
    }
}