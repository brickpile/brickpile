using System.Linq;
using System.Web.Mvc;
using StructureMap;

namespace Stormbreaker.Web.Mvc {
    public class StructureMapDependencyResolver : IDependencyResolver
    {
        private readonly IContainer _container;

        public StructureMapDependencyResolver(IContainer container)
        {
            this._container = container;
        }

        public object GetService(System.Type serviceType)
        {
            try
            {
                return this._container.GetInstance(serviceType);
            }
            catch
            {
                return null;
            }
        }

        public System.Collections.Generic.IEnumerable<object> GetServices(System.Type serviceType)
        {
            return this._container.GetAllInstances<object>()
                .Where(s => s.GetType() == serviceType);
        }        
    }
}