using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Stormbreaker.Common;
using Stormbreaker.Models;

namespace Stormbreaker.Dashboard.Models {
    public class CreateNewModel {
        [Required]
        [Display(Prompt = "My awesome page")]
        public string Name { get; set; }

        [Required]
        [Display(Prompt = "my-awesome-page")]
        public string Slug { get; set; }

        [Required]
        public string Url { get; set; }
        public string BackAction { get; set; }
        public IPageModel CurrentModel { get; set; }

        [ScaffoldColumn(false)]
        public string SelectedPageModel { get; set; }

        [Display(Name = "Type",Order = 10)]
        public IEnumerable<SelectListItem> AvailableModels
        {
            get
            {
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    foreach (var type in assembly.GetTypes())
                    {
                        if (type.GetCustomAttributes(typeof(PageModelAttribute), true).Length > 0)
                        {
                            yield return new SelectListItem { Text = type.GetAttribute<PageModelAttribute>().Name, Value = type.AssemblyQualifiedName };
                        }
                    }
                }
            }
        }
    }
}