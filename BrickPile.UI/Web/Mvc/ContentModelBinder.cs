using System;
using System.Web.Mvc;
using BrickPile.UI.Common;

namespace BrickPile.UI.Web.Mvc {
    /// <summary>
    /// 
    /// </summary>
    public class ContentModelBinder  : DefaultModelBinder {
        /// <summary>
        /// Binds the model by using the specified controller context and binding context.
        /// </summary>
        /// <param name="controllerContext">The context within which the controller operates. The context information includes the controller, HTTP content, request context, and route data.</param>
        /// <param name="bindingContext">The context within which the model is bound. The context includes information such as the model object, model name, model type, property filter, and value provider.</param>
        /// <returns>
        /// The bound object.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="bindingContext "/>parameter is null.</exception>
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext) {
            
            if(string.IsNullOrEmpty(controllerContext.RequestContext.HttpContext.Request.Form["AssemblyQualifiedName"])) {
                return null;
            }

            var item = Activator.CreateInstance(Type.GetType(controllerContext.RequestContext.HttpContext.Request.Form["AssemblyQualifiedName"], true));

            var context = new ModelBindingContext(bindingContext)
            {
                ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => item, item.GetType()),
                ModelName = "ContentModel",
                ModelState = bindingContext.ModelState,
                ValueProvider = bindingContext.ValueProvider
            };

            controllerContext.RouteData.ApplyCurrentContent(item);

            return base.BindModel(controllerContext, context);
        }
    }
}