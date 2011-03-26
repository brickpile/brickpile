using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using StructureMap;

namespace Stormbreaker.Web.Mvc {
    public class StructureMapDependencyResolver : IDependencyResolver {
        private readonly IContainer _container;
        /// <summary>
        /// Resolves singly registered services that support arbitrary object creation.
        /// </summary>
        /// <param name="serviceType">The type of the requested service or object.</param>
        /// <returns>
        /// The requested service or object.
        /// </returns>
        public object GetService(Type serviceType) {
            if (serviceType == null) return null;
            try {
                return serviceType.IsAbstract || serviceType.IsInterface
                         ? _container.TryGetInstance(serviceType)
                         : _container.GetInstance(serviceType);
            } catch {

                return null;
            }
        }
        /// <summary>
        /// Resolves multiply registered services.
        /// </summary>
        /// <param name="serviceType">The type of the requested services.</param>
        /// <returns>
        /// The requested services.
        /// </returns>
        public IEnumerable<object> GetServices(Type serviceType) {
            return _container.GetAllInstances<object>().Where(s => s.GetType() == serviceType);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="StructureMapDependencyResolver"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public StructureMapDependencyResolver(IContainer container) {
            _container = container;
        }
    }
}