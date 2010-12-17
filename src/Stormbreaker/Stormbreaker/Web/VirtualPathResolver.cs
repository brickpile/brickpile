using System.Text;
using System.Web.Routing;
using Stormbreaker.Models;
using Stormbreaker.Repositories;
using Stormbreaker.Web.Routing;
using StructureMap;

namespace Stormbreaker.Web {
    public class VirtualPathResolver : IVirtualPathResolver {
        private string _action;
        /* *******************************************************************
	    *  Methods 
	    * *******************************************************************/
        public string ResolveVirtualPath(IDocument document, RouteValueDictionary routeValueDictionary) {
            var url = new StringBuilder(250);
            url.Append(document.Slug);
            var repository = ObjectFactory.GetInstance<IRepository>();
            var parent = document;
            while(parent.Parent != null) {
                parent = repository.Load<IDocument>(parent.Parent.Id);
                url.Insert(0, string.Format("{0}/", parent.Slug));
            }

            if (routeValueDictionary.ContainsKey(DocumentRoute.ActionKey))
            {
                _action = routeValueDictionary[DocumentRoute.ActionKey] as string;
                if (_action != null && !_action.Equals(DocumentRoute.DefaultAction))
                {
                    return string.Format("{0}/{1}/", url, _action);
                }
            }
            return string.Format("{0}/", url);
        }
    }
}