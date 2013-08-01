using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
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
        /// <param name="metadata">The metadata.</param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext, ModelMetadata metadata) {
            if (!metadata.IsRequired || VirtualPath != null) yield break;
            var customTypeDescriptor = new AssociatedMetadataTypeTypeDescriptionProvider(metadata.ContainerType).GetTypeDescriptor(metadata.ContainerType);
            if (customTypeDescriptor == null) yield break;
            var descriptor = customTypeDescriptor.GetProperties().Find(metadata.PropertyName, true);
            var req = (new List<Attribute>(descriptor.Attributes.OfType<Attribute>())).OfType<RequiredAttribute>().FirstOrDefault();
            if (req != null) yield return new ValidationResult(req.FormatErrorMessage(metadata.DisplayName));
        }
    }
}