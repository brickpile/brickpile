using System.Web.Hosting;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using BrickPile.UI.Web.Hosting;

namespace BrickPile.Samples {
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication {
        protected void Application_Start() {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BrickPile.Core.Hosting.VirtualPathProviderRegistry.RegisterProvider(new NativeVirtualPathProvider(), "Static");
            BrickPile.Core.Hosting.VirtualPathProviderRegistry.RegisterProvider(new NativeVirtualPathProvider(), "Static2");
            //BrickPile.Core.Hosting.VirtualPathProviderRegistry.RegisterProvider(new DropboxVirtualPathProvider(),"Dropbox");
            ////HostingEnvironment.RegisterVirtualPathProvider(new DropboxVirtualPathProvider());
            ////HostingEnvironment.RegisterVirtualPathProvider(new NativeVirtualPathProvider());
            //new DropboxSync().SetupScheduler(new Settings {
            //    DocumentIcon = "~/areas/ui/content/images/document.png",
            //    IntervalInSeconds = 10
            //});
        }
    }
}