using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using Stormbreaker.Extensions;
using Stormbreaker.Repositories;
using Stormbreaker.Web.Hosting;
using Stormbreaker.Web.Mvc;
using Stormbreaker.Web.Routing;

[assembly: WebActivator.PreApplicationStartMethod(typeof(Stormbreaker.App_Start.Initializer), "Start")]
namespace Stormbreaker.App_Start {
    public static class Initializer {
        /// <summary>
        /// Initializer for Stormbreaker
        /// </summary>
        public static void Start() {

            //Insure that Structuremap would inject dependecies for any ASP.NET controller created
            var container = Bootstrapper.Initialize();

            // Register the Dropbox virtual path provider
            //var dropboxVirtualPathProvider = new DropboxVirtualPathProvider(container.GetInstance<IDropboxRepository>());
            //HostingEnvironment.RegisterVirtualPathProvider(dropboxVirtualPathProvider);

            DependencyResolver.SetResolver(new StructureMapDependencyResolver(container));
            
            RouteTable.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            RouteTable.Routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });

            // Register the default page route
            RouteTable.Routes.RegisterPageRoute(
                container.GetInstance<IPathResolver>(),
                container.GetInstance<IVirtualPathResolver>());
        }
    }
}