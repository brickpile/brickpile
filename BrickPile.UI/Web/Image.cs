using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BrickPile.Core.DataAnnotations;
using BrickPile.Domain.Models;

namespace BrickPile.UI.Web {
    public class Image : Asset, IValidatableProperty {
        /// <summary>
        /// Gets or sets the alt text.
        /// </summary>
        /// <value>
        /// The alt text.
        /// </value>
        public string AltText { get;set; }
        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public int? Height { get; set; }
        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public int? Width { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="Image"/> class.
        /// </summary>
        public Image() {
            Id = "assets/images/";
            
        }
        /// <summary>
        /// Validates the specified validation context.
        /// </summary>
        /// <param name="validationContext">The validation context.</param>
        /// <param name="propertyDescriptor">The property descriptor.</param>
        /// <returns></returns>
        public ValidationResult Validate(ValidationContext validationContext, PropertyDescriptor propertyDescriptor) {
            if (propertyDescriptor != null) {
                var displayAttr = propertyDescriptor.Attributes[typeof(DisplayAttribute)] as DisplayAttribute;
                var requiredAttr = propertyDescriptor.Attributes[typeof(RequiredAttribute)] as RequiredAttribute;
                var displayName = displayAttr != null ? displayAttr.Name : propertyDescriptor.DisplayName;
                if (requiredAttr != null && VirtualPath == null) {
                    return new ValidationResult(requiredAttr.FormatErrorMessage(displayName));
                }
            }
            return ValidationResult.Success;            
        }
    }
}