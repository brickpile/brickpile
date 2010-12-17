using System.Web.Routing;
using Stormbreaker.Models;
using Stormbreaker.Web.Routing;

namespace Stormbreaker.Extensions {
    public static class RouteDataExtensions {
        /* *******************************************************************
	    *  Methods 
	    * *******************************************************************/
        public static RouteData ApplyCurrentDocument(this RouteData data, string controllerName, string actionName, IDocument document)
        {
            data.Values[DocumentRoute.ControllerKey] = controllerName;
            data.Values[DocumentRoute.ActionKey] = actionName;
            data.DataTokens[DocumentRoute.DocumentKey] = document;
            return data;
        }
    }
}