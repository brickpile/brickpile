using System.Web.Mvc;
using System.Web.Routing;
using Stormbreaker.Example.Models;
using Stormbreaker.Models;
using Stormbreaker.Web;
using Stormbreaker.Web.Mvc;
using Stormbreaker.Web.Routing;
using StructureMap;

namespace Stormbreaker.Example
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("favicon.ico");

            routes.RegisterDocumentRoute(
                ObjectFactory.GetInstance<IPathResolver>(),
                ObjectFactory.GetInstance<IVirtualPathResolver>());

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            // Bootstrap structuremap
            Bootstrapper.ConfigureStructureMap(ObjectFactory.Container);
            // Insure that Structuremap would inject dependecies for any ASP.NET controller created
            DependencyResolver.SetResolver(new StructureMapDependencyResolver(ObjectFactory.Container));

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            ModelBinders.Binders.Add(typeof(IPageModel), new PageModelBinder());

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
            
        }
        protected void Application_EndRequest()
        {
            // Make sure to dispose NHibernate session if created on this web request
            ObjectFactory.ReleaseAndDisposeAllHttpScopedObjects();
        }
    }
}