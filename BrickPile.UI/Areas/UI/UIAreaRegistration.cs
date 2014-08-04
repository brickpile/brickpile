using System.Web.Mvc;
using BrickPile.Core.Mvc;
using BrickPile.Core.Routing;
using Raven.Client;
using StructureMap;

namespace BrickPile.UI.Areas.UI {
    public class UIAreaRegistration : AreaRegistration {
        public override string AreaName {
            get { return "UI"; }
        }

        public override void RegisterArea(AreaRegistrationContext context) {

            context.Routes.Add(
                new UiRoute(
                    new VirtualPathResolver(),
                    new RouteResolver(),
                    ObjectFactory.GetInstance<IDocumentStore>,
                    new ControllerMapper()));

            //context.Routes.MapUIRoute("Pages_Default",
            //    "ui/{controller}/{action}/{id}",
            //    new
            //    {
            //        controller = "UI",
            //        action = "Index",
            //        id = UrlParameter.Optional,
            //        area = "UI"
            //    },
            //    new[] {typeof (Controllers.UIController).Namespace});

            context.MapRoute(
                "UI_Default",
                "ui/{controller}/{action}/{id}",
                new
                {
                    controller = "UI",
                    action = "Index",
                    id = UrlParameter.Optional,
                    area = "UI"
                },
                new[] {typeof (Controllers.UIController).Namespace}
                );
        }
    }
}