﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using BrickPile.Core.DataAnnotations;

namespace BrickPile.Core.Mvc
{
    /// <summary>
    ///     Provides a base class for implementing validation logic.
    /// </summary>
    internal class ValidatablePropertyAdapter : ModelValidator
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ValidatablePropertyAdapter" /> class.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        /// <param name="controllerContext">The controller context.</param>
        public ValidatablePropertyAdapter(ModelMetadata metadata, ControllerContext controllerContext)
            : base(metadata, controllerContext) {}

        /// <summary>
        ///     When implemented in a derived class, validates the object.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <returns>
        ///     A list of validation results.
        /// </returns>
        public override IEnumerable<ModelValidationResult> Validate(object container)
        {
            var model = Metadata.Model;
            if (model == null)
            {
                return Enumerable.Empty<ModelValidationResult>();
            }

            var results = new List<ValidationResult>();
            var property = model as IValidatableProperty;
            if (property != null)
            {
                results.AddRange(property.Validate(new ValidationContext(model, null, null), Metadata));
            }

            return this.ConvertResults(results);
        }

        /// <summary>
        ///     Converts the results.
        /// </summary>
        /// <param name="results">The results.</param>
        /// <returns></returns>
        private IEnumerable<ModelValidationResult> ConvertResults(IEnumerable<ValidationResult> results)
        {
            foreach (var result in results)
            {
                if (result != ValidationResult.Success)
                {
                    if (result.MemberNames == null || !result.MemberNames.Any())
                    {
                        yield return new ModelValidationResult {Message = result.ErrorMessage};
                    }
                    else
                    {
                        foreach (var memberName in result.MemberNames)
                        {
                            yield return
                                new ModelValidationResult {Message = result.ErrorMessage, MemberName = memberName};
                        }
                    }
                }
            }
        }
    }
}