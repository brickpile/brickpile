using System;
using System.Web.Mvc;

namespace Stormbreaker.Web.Mvc {
    public class PageModelBinder : DefaultModelBinder {
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            var routeData = controllerContext.RouteData;
            if (!routeData.Values.ContainsKey("document"))
            {
                return null;
            }
            return base.CreateModel(controllerContext, bindingContext, modelType);
        }        
    }
}