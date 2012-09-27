using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using BrickPile.UI.Models;

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

                    // We can only handle
                    if (attribute.GetType().BaseType == typeof(ValidationAttribute) && property.PropertyType == typeof(PageReference)) {

                        var pageReference = bindingContext.ModelType.GetProperty(property.Name).GetValue(bindingContext.Model, null) as PageReference;

                        Type attrType = attribute.GetType();

                        if (attrType == typeof(RequiredAttribute) && string.IsNullOrEmpty(pageReference.Name)) {

                            var displayAttr = property.Attributes[typeof(DisplayAttribute)] as DisplayAttribute;
                            var displayName = displayAttr != null ? displayAttr.Name : property.DisplayName;

                            bindingContext.ModelState.AddModelError(property.Name, ((RequiredAttribute)attribute).FormatErrorMessage(displayName));

                        }
                    }
                }
            }

            base.OnModelUpdated(controllerContext, bindingContext);
        }
    }
}