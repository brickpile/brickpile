using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using BrickPile.Core.DataAnnotations;

namespace BrickPile.UI.Web.Mvc {
    /// <summary>
    /// 
    /// </summary>
    public class PageModelBinder : DefaultModelBinder {
        /// <summary>
        /// Called when the model is updated.
        /// </summary>
        /// <param name="controllerContext">The context within which the controller operates. The context information includes the controller, HTTP content, request context, and route data.</param>
        /// <param name="bindingContext">The context within which the model is bound. The context includes information such as the model object, model name, model type, property filter, and value provider.</param>
        protected override void OnModelUpdated(ControllerContext controllerContext, ModelBindingContext bindingContext) {
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(bindingContext.ModelType)) {
                var attributes = property.Attributes;
                if (attributes.Count == 0) continue;
                foreach (var attribute in attributes) {
                    if (attribute.GetType().BaseType != typeof (ValidationAttribute)) continue;
                    if (typeof(IValidatableProperty).IsAssignableFrom((property.PropertyType))) {
                        var propertyInfo = bindingContext.ModelType.GetProperty(property.Name).GetValue(bindingContext.Model, null);
                        var result = ((IValidatableProperty)propertyInfo).Validate(new ValidationContext(propertyInfo, null, null), property);
                        if(result != ValidationResult.Success) {
                             bindingContext.ModelState.AddModelError(string.Join(", ", result.MemberNames), result.ErrorMessage);
                        }
                    }
                }
            }

            base.OnModelUpdated(controllerContext, bindingContext);
        }
    }
}