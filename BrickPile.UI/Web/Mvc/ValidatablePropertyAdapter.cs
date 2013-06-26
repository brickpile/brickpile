using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using BrickPile.Core.DataAnnotations;
using BrickPile.Domain.Models;

namespace BrickPile.UI.Web.Mvc {
    /// <summary>
    /// 
    /// </summary>
    public class ValidatablePropertyAdapter : ModelValidator {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidatablePropertyAdapter"/> class.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        /// <param name="controllerContext">The controller context.</param>
        public ValidatablePropertyAdapter(ModelMetadata metadata, ControllerContext controllerContext) : base(metadata, controllerContext) { }
        /// <summary>
        /// When implemented in a derived class, validates the object.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <returns>
        /// A list of validation results.
        /// </returns>
        public override IEnumerable<ModelValidationResult> Validate(object container) {

            var contentType = ControllerContext.RouteData.Values["currentPage"] as IPage;

            var model = Metadata.Model;
            if (model == null || contentType == null) {
                return Enumerable.Empty<ModelValidationResult>();
            }

            var results = new List<ValidationResult>();
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(contentType.GetType())) {
                if (!typeof(IValidatableProperty).IsAssignableFrom((property.PropertyType))) continue;
                results.AddRange(((IValidatableProperty)model).Validate(new ValidationContext(model, null, null), property));
            }

            return ConvertResults(results);
        }
        /// <summary>
        /// Converts the results.
        /// </summary>
        /// <param name="results">The results.</param>
        /// <returns></returns>
        private IEnumerable<ModelValidationResult> ConvertResults(IEnumerable<ValidationResult> results) {

            foreach (ValidationResult result in results) {

                if (result != ValidationResult.Success) {

                    if (result.MemberNames == null || !result.MemberNames.Any()) {
                        yield return new ModelValidationResult { Message = result.ErrorMessage };
                    } else {
                        foreach (string memberName in result.MemberNames) {
                            yield return new ModelValidationResult { Message = result.ErrorMessage, MemberName = memberName };
                        }
                    }

                }

            }

        }
    }
}
