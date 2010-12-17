using Stormbreaker.Models;

namespace Stormbreaker.Extensions {
    public static class DocumentExtensions {
        /* *******************************************************************
	    *  Methods 
	    * *******************************************************************/
        public static string GetControllerName(this IDocument document)
        {
            return document.GetType().GetAttribute<ControlsAttribute>().ControllerName.ToLower();
        }
    }
}