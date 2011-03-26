using Stormbreaker.Models;
using Stormbreaker.Web.UI;

namespace Stormbreaker.Web.Routing {
    public class PathData : IPathData {
        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>
        /// The action.
        /// </value>
        public string Action { get; set; }
        /// <summary>
        /// Gets or sets the controller.
        /// </summary>
        /// <value>
        /// The controller.
        /// </value>
        public string Controller { get; set; }
        /// <summary>
        /// Gets or sets the current page model.
        /// </summary>
        /// <value>
        /// The current page model.
        /// </value>
        public IPageModel CurrentPageModel { get; set; }
    }
}