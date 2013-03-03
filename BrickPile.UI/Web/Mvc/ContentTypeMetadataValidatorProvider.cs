using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BrickPile.Core.DataAnnotations;

namespace BrickPile.UI.Web.Mvc {
    /// <summary>
    /// 
    /// </summary>
    public class ContentTypeMetadataValidatorProvider : DataAnnotationsModelValidatorProvider {
        /// <summary>
        /// Gets a list of validators.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        /// <param name="context">The context.</param>
        /// <param name="attributes">The list of validation attributes.</param>
        /// <returns>
        /// A list of validators.
        /// </returns>
        protected override IEnumerable<ModelValidator> GetValidators(ModelMetadata metadata, ControllerContext context, IEnumerable<Attribute> attributes) {
            List<ModelValidator> validators = base.GetValidators(metadata, context, attributes).ToList();
            if (typeof(IValidatableProperty).IsAssignableFrom(metadata.ModelType)) {
                validators.Add(new ValidatablePropertyAdapter(metadata, context));
            }
            return validators.AsEnumerable();
        }
    }
}