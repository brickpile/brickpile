using System.Collections.Generic;
using System.Web.Routing;

namespace BrickPile.Core
{
    public interface INavigationContext {
        RequestContext RequestContext { get; }
        IPage CurrentPage { get; set; }
        IEnumerable<IPage> CurrentContext { get; set; }
    }
}