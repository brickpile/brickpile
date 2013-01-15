using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BrickPile.Core.DataAnnotations;

namespace BrickPile.UI.Models {
    public class Image : IValidatableProperty {
        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public string Url { get; set; }
        /// <summary>
        /// Gets or sets the alt text.
        /// </summary>
        /// <value>
        /// The alt text.
        /// </value>
        public string AltText { get; set; }
        /// <summary>
        /// Gets or sets the virtual URL.
        /// </summary>
        /// <value>
        /// The virtual URL.
        /// </value>
        public string VirtualUrl { get; set; }
        /// <summary>
        /// Validates the specified validation context.
        /// </summary>
        /// <param name="validationContext">The validation context.</param>
        /// <param name="propertyDescriptor">The property descriptor.</param>
        /// <returns></returns>
        public ValidationResult Validate(ValidationContext validationContext, PropertyDescriptor propertyDescriptor) {
            if(propertyDescriptor != null) {
                var displayAttr = propertyDescriptor.Attributes[typeof(DisplayAttribute)] as DisplayAttribute;
                var requiredAttr = propertyDescriptor.Attributes[typeof(RequiredAttribute)] as RequiredAttribute;
                var displayName = displayAttr != null ? displayAttr.Name : propertyDescriptor.DisplayName;
                if (requiredAttr != null && VirtualUrl == null) {
                    return new ValidationResult(requiredAttr.FormatErrorMessage(displayName));
                }
            }
            return ValidationResult.Success;
        }
    }
}