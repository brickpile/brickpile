using Stormbreaker.Models;

namespace Stormbreaker.Web.Routing {
    /// <summary>
    /// The IPathData interface 
    /// </summary>
    public interface IPathData {
        /* *******************************************************************
	    * Properties
	    * *******************************************************************/
        string Action { get; set; }
        string Controller { get; set; }
        IContentItem CurrentItem { get; set; }
    }
}