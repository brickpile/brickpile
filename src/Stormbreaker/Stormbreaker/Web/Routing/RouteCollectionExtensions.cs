using System.Web.Routing;

namespace Stormbreaker.Web.Routing {
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    /// <example></example>
    public static class RouteCollectionExtensions
    {
        /* *******************************************************************
        *  Methods
        * *******************************************************************/
        #region public static RouteCollection RegisterContentRoute(this RouteCollection routes, IPathResolver pathResolver, IVirtualPathResolver virtualPathResolver)
        /// <summary>
        /// Responsible for setting up the content route for Stormbreaker
        /// </summary>
        /// <param name="routes"></param>
        /// <param name="pathResolver"></param>
        /// <param name="virtualPathResolver"></param>
        /// <returns></returns>
        public static RouteCollection RegisterContentRoute(this RouteCollection routes, IPathResolver pathResolver, IVirtualPathResolver virtualPathResolver)
        {
            var contentRoute = new ContentRoute(pathResolver, virtualPathResolver);
            routes.Add("Content", contentRoute);
            return routes;
        }
        #endregion
    }
}