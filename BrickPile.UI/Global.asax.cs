using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using BrickPile.UI.Web.Hosting;
using Raven.Client;
using StructureMap;

namespace BrickPile.Samples {
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication {
        protected void Application_Start() {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            HostingEnvironment.RegisterVirtualPathProvider(new NativeVirtualPathProvider());
            GenericHostingEnvironment.RegisterVirtualPathProvider(new NativeVirtualPathProvider(),"DropboxVPP");
            //var formatter = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            //formatter.SerializerSettings = new JsonSerializerSettings {
            //    Formatting = Formatting.Indented,
            //    ContractResolver = new CamelCasePropertyNamesContractResolver(),
            //    TypeNameHandling = TypeNameHandling.Objects
            //};

        }
    }

    public static class GenericHostingEnvironment {
        private static readonly Dictionary<string, VirtualPathProvider> ProviderMappings = new Dictionary<string, VirtualPathProvider>();
        /// <summary>
        /// Registers the virtual path provider.
        /// </summary>
        /// <param name="virtualPathProvider">The virtual path provider.</param>
        /// <param name="providerName">Name of the provider.</param>
        public static void RegisterVirtualPathProvider(VirtualPathProvider virtualPathProvider, string providerName) {

            ProviderMappings.Add(providerName, virtualPathProvider);
            HostingEnvironment.RegisterVirtualPathProvider(virtualPathProvider);
        }

        /// <summary>
        /// Gets the provider.
        /// </summary>
        /// <param name="providername">The providername.</param>
        /// <returns></returns>
        public static VirtualPathProvider GetProvider(string providername) {
            return (from pair in ProviderMappings where string.Compare(pair.Key, providername, StringComparison.OrdinalIgnoreCase) == 0 select pair.Value).FirstOrDefault();
        }
    }

    public class FileSystemVirtualPathProvider {
        public string Name { get; set; }
        public Type Type { get; set; }
    }
}