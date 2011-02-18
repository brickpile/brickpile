using System.Web.Mvc;
using Dashboard.Web.Routing;
using Stormbreaker.Web;
using StructureMap;

namespace Dashboard {
    public class DashboardAreaRegistration : AreaRegistration {
        /* *******************************************************************
	    * Properties
	    * *******************************************************************/
        public override string AreaName { get { return "Dashboard"; } }
        /* *******************************************************************
	    * Methods
	    * *******************************************************************/
        public override void RegisterArea(AreaRegistrationContext context) {
            
            ViewEngines.Engines.Add(new RazorViewEngine()
                                        {
                                            AreaPartialViewLocationFormats = new[] { "~/Views/{1}/{0}.cshtml" }
                                        });


            var dashboardRoute = new ContentRoute(
                ObjectFactory.GetInstance<PathResolver>(),
                ObjectFactory.GetInstance<VirtualPathResolver>(),
                null);
            context.Routes.Add("Dashboard", dashboardRoute);

            context.MapRoute(
                "Dashboard_default",
                "dashboard/{controller}/{action}/{id}",
                new { controller = "dashboard", action = "index", id = UrlParameter.Optional }
            );
        }
    }
}