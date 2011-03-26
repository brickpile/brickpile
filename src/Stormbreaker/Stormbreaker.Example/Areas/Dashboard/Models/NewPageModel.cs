using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Stormbreaker;
using Stormbreaker.Extensions;
using Stormbreaker.Models;

namespace Dashboard.Models {
    public class NewPageModel : IPageModel {

        [ScaffoldColumn(false)]
        public string Id { get; set; }

        [ScaffoldColumn(false)]
        public IPageMetaData MetaData { get; set; }

        [ScaffoldColumn(false)]
        public string SelectedPageModel { get; set; }

        [ScaffoldColumn(false)]
        public virtual int? SortOrder { get; set; }

        [Display(Name = "Model",Order = 10)]
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

        [ScaffoldColumn(false)]
        public DenormalizedReference<IPageModel> Parent { get; set; }
        [ScaffoldColumn(false)]
        public IList<DenormalizedReference<IPageModel>> Children { get; set; }

        public NewPageModel() {
            MetaData = new PageMetaData();
            Children = new List<DenormalizedReference<IPageModel>>();
        }
    }
}