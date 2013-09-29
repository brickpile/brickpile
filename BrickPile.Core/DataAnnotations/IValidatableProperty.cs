using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BrickPile.Core.DataAnnotations {
    public interface IValidatableProperty {
        /// <summary>
        /// Validates the specified validation context.
        /// </summary>
        /// <param name="validationContext">The validation context.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns></returns>
        IEnumerable<ValidationResult> Validate(ValidationContext validationContext, ModelMetadata metadata);
    }
}
