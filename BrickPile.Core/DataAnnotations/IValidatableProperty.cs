using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BrickPile.Core.DataAnnotations
{
    /// <summary>
    ///     Provides validation for custom properties
    /// </summary>
    public interface IValidatableProperty
    {
        /// <summary>
        ///     Used for validation of custom properties, eg. for implementing the <see cref="RequiredAttribute" /> attribute.
        /// </summary>
        /// <param name="validationContext">The validation context.</param>
        /// <param name="metadata">The metadata.</param>
        /// <returns></returns>
        IEnumerable<ValidationResult> Validate(ValidationContext validationContext, ModelMetadata metadata);
    }
}