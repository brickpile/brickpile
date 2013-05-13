using System.Web.Http.Dependencies;
using StructureMap;

namespace BrickPile.UI.Web.Mvc
{
    public class StructureMapWebApiDependencyResolver : StructureMapScope, IDependencyResolver
    {
        private readonly IContainer _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureMapWebApiDependencyResolver"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public StructureMapWebApiDependencyResolver(IContainer container)
            : base(container)
        {
            this._container = container;
        }

        /// <summary>
        /// Starts a resolution scope.
        /// </summary>
        /// <returns>
        /// The dependency scope.
        /// </returns>
        public IDependencyScope BeginScope()
        {
            var childContainer = this._container.GetNestedContainer();
            return new StructureMapScope(childContainer);
        }
    }
}