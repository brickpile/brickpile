using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace BrickPile.Core.Mvc
{
    /// <summary>
    ///     Implements the default model metadata provider for ASP.NET MVC.
    /// </summary>
    internal class ExtendedDataAnnotationsModelMetadataProvider : DataAnnotationsModelMetadataProvider
    {
        public const string KeyGroupName = "GroupName";

        /// <summary>
        ///     Gets the metadata for the specified property.
        /// </summary>
        /// <param name="attributes">The attributes.</param>
        /// <param name="containerType">The type of the container.</param>
        /// <param name="modelAccessor">The model accessor.</param>
        /// <param name="modelType">The type of the model.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>
        ///     The metadata for the property.
        /// </returns>
        protected override ModelMetadata CreateMetadata(IEnumerable<Attribute> attributes, Type containerType,
            Func<object> modelAccessor, Type modelType, string propertyName)
        {
            var enumerable = attributes as Attribute[] ?? attributes.ToArray();
            var modelMetadata = base.CreateMetadata(enumerable, containerType, modelAccessor, modelType,
                propertyName);
            var displayAttribute = enumerable.OfType<DisplayAttribute>().FirstOrDefault();

            if (displayAttribute != null)
                modelMetadata.AdditionalValues[KeyGroupName] = displayAttribute.GroupName;

            return modelMetadata;
        }
    }
}