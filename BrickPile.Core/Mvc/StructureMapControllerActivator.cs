using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using StructureMap;

namespace BrickPile.Core.Mvc
{
    public class StructureMapControllerActivator : IHttpControllerActivator
    {
        private readonly IContainer _container;

        public StructureMapControllerActivator(IContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");
            _container = container;
        }

        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            try
            {
                var scopedContainer = _container.GetNestedContainer();
                scopedContainer.Inject(typeof(HttpRequestMessage), request);
                request.RegisterForDispose(scopedContainer);
                return (IHttpController)scopedContainer.GetInstance(controllerType);
            }
            catch (System.Exception e)
            {
                // TODO : Logging
                throw e;
            }
        }
    }
}