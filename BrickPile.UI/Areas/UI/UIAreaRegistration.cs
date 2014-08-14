using System.Web.Mvc;

namespace BrickPile.UI.Areas.UI {
    public class UIAreaRegistration : AreaRegistration {
        public override string AreaName {
            get { return "UI"; }
        }

        public override void RegisterArea(AreaRegistrationContext context) {
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