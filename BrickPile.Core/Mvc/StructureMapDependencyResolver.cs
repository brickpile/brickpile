using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using StructureMap;

namespace BrickPile.Core.Mvc
{
    /// <summary>
    ///     Defines the methods that simplify service location and dependency resolution.
    /// </summary>
    internal class StructureMapDependencyResolver : IDependencyResolver
    {
        private readonly IContainer container;

        /// <summary>
        ///     Initializes a new instance of the <see cref="StructureMapDependencyResolver" /> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public StructureMapDependencyResolver(IContainer container)
        {
            this.container = container;
        }

        /// <summary>
        ///     Resolves singly registered services that support arbitrary object creation.
        /// </summary>
        /// <param name="serviceType">The type of the requested service or object.</param>
        /// <returns>
        ///     The requested service or object.
        /// </returns>
        public object GetService(Type serviceType)
        {
            if (serviceType == null) return null;
            try
            {
                return serviceType.IsAbstract || serviceType.IsInterface
                    ? this.container.TryGetInstance(serviceType)
                    : this.container.GetInstance(serviceType);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        ///     Resolves multiply registered services.
        /// </summary>
        /// <param name="serviceType">The type of the requested services.</param>
        /// <returns>
        ///     The requested services.
        /// </returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this.container.GetAllInstances<object>().Where(s => s.GetType() == serviceType);
        }
    }
}