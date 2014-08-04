using System;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using BrickPile.Core.Routing;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace BrickPile.Core.Graph
{
    internal sealed class ContentTypeRegistrationConvention : IRegistrationConvention
    {
        /// <summary>
        ///     The register method
        /// </summary>
        public static readonly MethodInfo RegisterMethod = typeof (ContentTypeRegistrationConvention)
            .GetMethod("Register", BindingFlags.NonPublic | BindingFlags.Static)
            .GetGenericMethodDefinition();

        /// <summary>
        ///     Processes the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="registry">The registry.</param>
        public void Process(Type type, Registry registry)
        {
            if (!typeof (IPage).IsAssignableFrom(type)) return;
            var specificRegisterMethod = RegisterMethod.MakeGenericMethod(new[] {type});
            specificRegisterMethod.Invoke(null, new object[] {registry});
        }

        /// <summary>
        ///     Registers the specified registry.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="registry">The registry.</param>
        private static void Register<T>(Registry registry) where T : IPage
        {
            registry.For<T>().UseSpecial(y => y.ConstructedBy(r => GetCurrentPage<T>()));
        }

        /// <summary>
        ///     Gets the current content.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static T GetCurrentPage<T>() where T : IPage
        {
            var handler = (MvcHandler) HttpContext.Current.Handler;
            if (handler == null)
                return default(T);
            return (T) handler.RequestContext.RouteData.Values[PageRoute.CurrentPageKey];
        }
    }
}