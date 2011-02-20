using Stormbreaker.Models;

namespace Stormbreaker.Web.Routing {
    public class PathData : IPathData {
        public string Action { get; set; }
        public string Controller { get; set; }
        public IPageModel CurrentPageModel { get; set; }
    }
}