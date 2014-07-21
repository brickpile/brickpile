using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using BrickPile.Domain;
using BrickPile.UI.Common;
using IPage = BrickPile.Core.IPage;

namespace BrickPile.UI.Areas.UI.Models {
    /// <summary>
    /// 
    /// </summary>
    public class NewModel {
        /// <summary>
        /// Gets or sets the current model.
        /// </summary>
        /// <value>
        /// The current model.
        /// </value>
        public IPage CurrentModel { get; set; }
        /// <summary>
        /// Gets or sets the selected page model.
        /// </summary>
        /// <value>
        /// The selected page model.
        /// </value>
        [ScaffoldColumn(false)]
        [Required(ErrorMessage = "Test Error")]
        public string SelectedPageModel { get; set; }
        /// <summary>
        /// Gets the available models.
        /// </summary>
        [Display(Name = "Select page type", Order = 10)]
        public IEnumerable<SelectListItem> AvailableModels {
            get {
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
                    foreach (var type in assembly.GetTypes()) {
                        if (type.GetCustomAttributes(typeof(ContentTypeAttribute), true).Length > 0) {
                            yield return new SelectListItem { Text = type.GetAttribute<ContentTypeAttribute>().Name ?? type.Name, Value = type.AssemblyQualifiedName };
                        }
                    }
                }
            }
        }
    }
}