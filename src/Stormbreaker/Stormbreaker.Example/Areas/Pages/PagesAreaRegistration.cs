using System;
using System.Web.Mvc;
using Pages.Web.Routing;
using Stormbreaker.Web;
using StructureMap;

namespace Pages {
    public class PagesAreaRegistration : AreaRegistration
    {

        public override string AreaName
        {
            get { return "Pages"; }
        }

        public override void RegisterArea(AreaRegistrationContext context) {
            var dashboardRoute = new PagesRoute(
                ObjectFactory.GetInstance<PathResolver>(),
                ObjectFactory.GetInstance<VirtualPathResolver>(),
                null);
            context.Routes.Add(dashboardRoute);            
        }
    }
}