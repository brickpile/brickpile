using System;
using System.Web;
using StructureMap;

namespace Stormbreaker.Web {
    class ContainerDisposalModule : IHttpModule {
        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpApplication"/> that provides access to the methods, properties, and events common to all application objects within an ASP.NET application</param>
        public void Init(HttpApplication context) {
            context.EndRequest += Application_EndRequest;
        }
        /// <summary>
        /// Handles the EndRequest event of the Application control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Application_EndRequest(object sender, EventArgs e) {
            // Make sure to dispose RavenDB session if created on this web request
            ObjectFactory.ReleaseAndDisposeAllHttpScopedObjects();
        }
        /// <summary>
        /// Disposes of the resources (other than memory) used by the module that implements <see cref="T:System.Web.IHttpModule"/>.
        /// </summary>
        public void Dispose() { }
    }
}
