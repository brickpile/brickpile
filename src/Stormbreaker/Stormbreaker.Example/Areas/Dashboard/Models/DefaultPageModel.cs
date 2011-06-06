using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Stormbreaker.Common;

namespace Stormbreaker.Dashboard.Models {
    public class DefaultPageModel {
        [Required]
        [Display(Name = "Page name")]
        public string Name { get; set; }
        [ScaffoldColumn(false)]
        public string SelectedPageModel { get; set; }
        [Display(Name = "Model", Order = 10)]
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