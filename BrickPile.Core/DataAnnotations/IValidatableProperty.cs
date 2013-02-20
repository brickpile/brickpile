using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BrickPile.Core.DataAnnotations {
    public interface IValidatableProperty {
        /// <summary>
        /// Validates the specified validation context.
        /// </summary>
        /// <param name="validationContext">The validation context.</param>
        /// <param name="propertyDescriptor">The property descriptor.</param>
        /// <returns></returns>
        IEnumerable<ValidationResult> Validate(ValidationContext validationContext, PropertyDescriptor propertyDescriptor);
    }
}
