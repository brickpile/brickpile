using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BrickPile.Sample.Models;

namespace BrickPile.Sample.Web.Mvc {
    /// <summary>
    /// 
    /// </summary>
    public class TagsModelBinder : DefaultModelBinder {
        protected override void BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, System.ComponentModel.PropertyDescriptor propertyDescriptor) {
            if (propertyDescriptor.PropertyType == typeof(ICollection<string>)) {
                var form = controllerContext.HttpContext.Request.Form;
                var tagsAsString = form["CurrentModel.Tags"];
                var model = bindingContext.Model as Page;
                model.Tags = string.IsNullOrEmpty(tagsAsString)
                    ? new List<string>()
                    : tagsAsString.Split(',').Select(i => i.Trim()).ToList();
            }
            base.BindProperty(controllerContext, bindingContext, propertyDescriptor);
        }
    }
}