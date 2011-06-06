using System.Collections.Generic;
using Stormbreaker.Models;

namespace Stormbreaker.Dashboard.Models {
    public class EditPageReferenceModel {
        public string Message { get; set; }
        public string BackAction { get; set; }
        public IPageModel CurrentModel { get; set; }
        public IEnumerable<IPageModel> PageModels { get; set; }
    }
}