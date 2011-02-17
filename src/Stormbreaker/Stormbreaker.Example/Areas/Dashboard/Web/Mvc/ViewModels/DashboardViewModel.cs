using System;
using System.Collections.Generic;
using Dashboard.Models;
using Stormbreaker;
using Stormbreaker.Models;
using Stormbreaker.Repositories;
using Stormbreaker.Web.UI;

namespace Dashboard.Web.Mvc.ViewModels {

    public class DashboardViewModel : IDashboardViewModel {
        /// <summary>
        /// Get/Sets the StructureInfo of the DashboardViewModel
        /// </summary>
        /// <value></value>
        public virtual IStructureInfo StructureInfo { get; set; }
        /// <summary>
        /// Get/Sets the CurrentModel of the DashboardViewModel
        /// </summary>
        /// <value></value>
        public virtual IPageModel CurrentModel { get; set; }

        public IEnumerable<Type> AvailableModels {
            get {
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    foreach (var type in assembly.GetTypes())
                    {
                        if (type.GetCustomAttributes(typeof(PageModelAttribute), true).Length > 0)
                        {
                            yield return type;
                        }
                    }
                }                
            }
        }

        public NewPageModel NewPageModel {
            get {
                return new NewPageModel();
            }
        }

        public DashboardViewModel(IPageModel model, IPageRepository repository)
        {
            CurrentModel = model;
            this.StructureInfo = new StructureInfo(repository,model);
        }
    }
}