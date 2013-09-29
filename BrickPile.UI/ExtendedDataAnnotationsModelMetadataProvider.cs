using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace BrickPile.UI {
    public class ExtendedDataAnnotationsModelMetadataProvider : DataAnnotationsModelMetadataProvider {
        public const string Key_GroupName = "GroupName";

        //protected override ModelMetadata GetMetadataForProperty(Func<object> modelAccessor, Type containerType, System.ComponentModel.PropertyDescriptor propertyDescriptor) {
        //    ModelMetadata modelMetadata = base.GetMetadataForProperty(modelAccessor, containerType, propertyDescriptor);
        //    var displayAttr = propertyDescriptor.Attributes.OfType<DisplayAttribute>().FirstOrDefault();

        //    if (displayAttr != null)
        //        modelMetadata.AdditionalValues[Key_GroupName] = displayAttr.GroupName;

        //    return modelMetadata;
        //}

        protected override ModelMetadata CreateMetadata(IEnumerable<Attribute> attributes, Type containerType, Func<object> modelAccessor, Type modelType, string propertyName) {

            ModelMetadata modelMetadata = base.CreateMetadata(attributes, containerType, modelAccessor, modelType, propertyName);
            DisplayAttribute displayAttribute = attributes.OfType<DisplayAttribute>().FirstOrDefault();

            if (displayAttribute != null)
                modelMetadata.AdditionalValues[Key_GroupName] = displayAttribute.GroupName;

            return modelMetadata;
        }

    }
}