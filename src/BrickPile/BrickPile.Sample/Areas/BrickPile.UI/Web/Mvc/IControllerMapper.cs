using System;

namespace BrickPile.UI.Web.Mvc {
    public interface IControllerMapper {
        string GetControllerName(Type type);
        bool ControllerHasAction(string controllerName, string actionName);
    }
}
