using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Stormbreaker;
using Stormbreaker.Extensions;

namespace Dashboard.Models {
    public class DefaultPageModel {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the selected page model.
        /// </summary>
        /// <value>
        /// The selected page model.
        /// </value>
        [ScaffoldColumn(false)]
        public string SelectedPageModel { get; set; }
        /// <summary>
        /// Gets the available models.
        /// </summary>
        public IEnumerable<SelectListItem> AvailableModels {
            get {
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
                    foreach (var type in assembly.GetTypes()) {
                        if (type.GetCustomAttributes(typeof(PageModelAttribute), true).Length > 0) {
                            yield return new SelectListItem { Text = type.GetAttribute<PageModelAttribute>().Name, Value = type.AssemblyQualifiedName };
                        }
                    }
                }
            }
        }
    }
}