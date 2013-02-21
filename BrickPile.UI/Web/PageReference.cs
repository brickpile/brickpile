/* Copyright (C) 2011-2013 by Marcus Lindblom, Anders Granberg

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE. */

using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BrickPile.Core.DataAnnotations;

namespace BrickPile.UI.Web {
    [DisplayColumn("Name")]
    public class PageReference : IValidatableProperty {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        [ScaffoldColumn(false)]
        public string Id { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString() {
            return Id;
        }
        /// <summary>
        /// Validates the specified validation context.
        /// </summary>
        /// <param name="validationContext">The validation context.</param>
        /// <param name="propertyDescriptor">The property descriptor.</param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext, PropertyDescriptor propertyDescriptor) {
            if (propertyDescriptor != null) {
                var displayAttr = propertyDescriptor.Attributes[typeof(DisplayAttribute)] as DisplayAttribute;
                var requiredAttr = propertyDescriptor.Attributes[typeof(RequiredAttribute)] as RequiredAttribute;
                var displayName = displayAttr != null ? displayAttr.Name : propertyDescriptor.DisplayName;
                if (!IsValidInput()) {
                    yield return new ValidationResult(string.Format("Value of the {0} field couldn´t be recognized as a valid page ", displayName));
                }
                if (requiredAttr != null && !IsValidPage()) {
                    yield return new ValidationResult(requiredAttr.FormatErrorMessage(displayName));
                }                 
            }
            yield return ValidationResult.Success;
        }
        /// <summary>
        /// Checks if the input contains a valid page
        /// </summary>
        /// <returns></returns>
        private bool IsValidPage() {
            return !string.IsNullOrEmpty(Id);
        }
        /// <summary>
        /// Checks if the input is valid; either completely empty or a contains valid page
        /// </summary>
        /// <returns></returns>
        private bool IsValidInput() {
            return string.IsNullOrEmpty(Id + Name) || !string.IsNullOrEmpty(Id);
        }
    }
}
