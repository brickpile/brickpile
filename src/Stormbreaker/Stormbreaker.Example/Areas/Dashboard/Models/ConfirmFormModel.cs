using Stormbreaker.Models;

namespace Stormbreaker.Dashboard.Models {
    public class ConfirmFormModel {
        public string Message { get; set; }
        public string BackAction { get; set; }
        public IPageModel CurrentModel { get; set; }
    }
}