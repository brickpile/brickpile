using System.Text.RegularExpressions;
using System.Web;
using System.Web.Routing;

namespace BrickPile.UI.Web.Routing {
    public class SubdomainRouteConstraint : IRouteConstraint {
        private readonly string _subdomainWithDot;
        /// <summary>
        /// Initializes a new instance of the <see cref="SubdomainRouteConstraint"/> class.
        /// </summary>
        /// <param name="subdomainWithDot">The subdomain with dot.</param>
        public SubdomainRouteConstraint(string subdomainWithDot) {
            _subdomainWithDot = subdomainWithDot;
        }
        /// <summary>
        /// Determines whether the URL parameter contains a valid value for this constraint.
        /// </summary>
        /// <param name="httpContext">An object that encapsulates information about the HTTP request.</param>
        /// <param name="route">The object that this constraint belongs to.</param>
        /// <param name="parameterName">The name of the parameter that is being checked.</param>
        /// <param name="values">An object that contains the parameters for the URL.</param>
        /// <param name="routeDirection">An object that indicates whether the constraint check is being performed when an incoming request is being handled or when a URL is being generated.</param>
        /// <returns>
        /// true if the URL parameter contains a valid value; otherwise, false.
        /// </returns>
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection) {
            return new Regex("^https?://" + _subdomainWithDot).IsMatch(httpContext.Request.Url.AbsoluteUri);
        }
    }
}