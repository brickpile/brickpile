using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using StructureMap;

namespace BrickPile.Core.Mvc
{
    /// <summary>
    /// Defines the methods that are required for an <see cref="T:System.Web.Http.Dispatcher.IHttpControllerActivator"/>.
    /// </summary>
    internal class StructureMapControllerActivator : IHttpControllerActivator
    {
        private readonly IContainer container;

        /// <summary>
        /// Initializes a new instance of the <see cref="StructureMapControllerActivator"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <exception cref="System.ArgumentNullException">container</exception>
        public StructureMapControllerActivator(IContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");
            this.container = container;
        }

        /// <summary>
        /// Creates an <see cref="T:System.Web.Http.Controllers.IHttpController" /> object.
        /// </summary>
        /// <param name="request">The message request.</param>
        /// <param name="controllerDescriptor">The HTTP controller descriptor.</param>
        /// <param name="controllerType">The type of the controller.</param>
        /// <returns>
        /// An <see cref="T:System.Web.Http.Controllers.IHttpController" /> object.
        /// </returns>
        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            try
            {
                var scopedContainer = this.container.GetNestedContainer();
                scopedContainer.Inject(typeof(HttpRequestMessage), request);
                request.RegisterForDispose(scopedContainer);
                return (IHttpController)scopedContainer.GetInstance(controllerType);
            }
            catch (Exception e)
            {
                // TODO : Logging
                throw e;
            }
        }
    }
}