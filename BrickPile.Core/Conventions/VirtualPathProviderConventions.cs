using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Hosting;
using System.Web.Routing;
using BrickPile.Core.Hosting;

namespace BrickPile.Core.Conventions
{
    /// <summary>
    ///     Represents the registered virtual path providers
    /// </summary>
    public sealed class VirtualPathProviderConventions
    {
        private static IDictionary<string, CommonVirtualPathProvider> registeredProviders;

        /// <summary>
        ///     Gets the registered providers.
        /// </summary>
        /// <value>
        ///     The registered providers.
        /// </value>
        public static IDictionary<string, CommonVirtualPathProvider> RegisteredProviders
        {
            get
            {
                return registeredProviders ??
                       (registeredProviders = new Dictionary<string, CommonVirtualPathProvider>());
            }
        }

        /// <summary>
        ///     Registers the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="provider">The provider.</param>
        public void Register(string name, Func<CommonVirtualPathProvider> provider)
        {
            HostingEnvironment.RegisterVirtualPathProvider(provider());
            RegisteredProviders.Add(name, provider());
            RegisterIgnoreRoute(provider().VirtualPathRoot);
        }

        /// <summary>
        ///     Registers the ignore route.
        /// </summary>
        /// <param name="virtualPathRoot">The virtual path root.</param>
        private static void RegisterIgnoreRoute(string virtualPathRoot)
        {
            RouteTable.Routes.Insert(0, new Route(VirtualPathUtility.MakeRelative("~/", virtualPathRoot) + "{*pathInfo}", new StopRoutingHandler()));
        }
    }
}