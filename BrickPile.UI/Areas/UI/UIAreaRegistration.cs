using System.Web.Mvc;
using BrickPile.Core.Extensions;

namespace BrickPile.UI.Areas.UI {
    public class UIAreaRegistration : AreaRegistration {
        public override string AreaName {
            get { return "UI"; }
        }

        public override void RegisterArea(AreaRegistrationContext context) {
            context.Routes.MapUIRoute("Pages_Default",
                "ui/{controller}/{action}/{id}",
                new
                {
                    controller = "UI",
                    action = "Index",
                    id = UrlParameter.Optional,
                    area = "UI"
                },
                new[] {typeof (Controllers.UIController).Namespace});

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