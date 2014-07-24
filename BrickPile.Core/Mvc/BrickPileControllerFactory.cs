using System.Web.Mvc;
using System.Web.Routing;
using BrickPile.Core.Extensions;
using Raven.Client;
using StructureMap;

namespace BrickPile.Core.Mvc {
    public class BrickPileControllerFactory : DefaultControllerFactory {

        private readonly IDocumentStore _store;
        private static bool _hasConfiguration;

        /// <summary>
        /// Creates the specified controller by using the specified request context.
        /// </summary>
        /// <param name="requestContext">The context of the HTTP request, which includes the HTTP context and route data.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <returns>
        /// The controller.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="requestContext"/> parameter is null.</exception>
        ///   
        /// <exception cref="T:System.ArgumentException">The <paramref name="controllerName"/> parameter is null or empty.</exception>
        public override IController CreateController(RequestContext requestContext, string controllerName) {
            if (HasConfiguration()) return base.CreateController(requestContext, controllerName);
            requestContext.RouteData.DataTokens["area"] = "ui";
            requestContext.RouteData.Values["action"] = "index";
            requestContext.RouteData.Values["controller"] = "setup";
            return base.CreateController(requestContext, "setup");
        }
        /// <summary>
        /// Determines whether this instance has configuration.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance has configuration; otherwise, <c>false</c>.
        /// </returns>
        private bool HasConfiguration() {
            if (!_hasConfiguration) {
                _hasConfiguration = _store.Exists("brickpile/configuration");
            }
            return _hasConfiguration;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="BrickPileControllerFactory"/> class.
        /// </summary>
        public BrickPileControllerFactory() {
            _store = ObjectFactory.GetInstance<IDocumentStore>();
        }
    }
}