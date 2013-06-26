using System;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using BrickPile.Domain.Models;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace BrickPile.Core.Graph {
    /// <summary>
    /// 
    /// </summary>
    public class ContentTypeRegistrationConvetion : IRegistrationConvention {
        /// <summary>
        /// 
        /// </summary>
        public static readonly MethodInfo RegisterMethod = typeof(ContentTypeRegistrationConvetion)
            .GetMethod("Register", BindingFlags.NonPublic | BindingFlags.Static)
            .GetGenericMethodDefinition();
        /// <summary>
        /// Processes the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="registry">The registry.</param>
        public void Process(Type type, Registry registry) {
            if (typeof(IPage).IsAssignableFrom(type)) {
                var specificRegisterMethod = RegisterMethod.MakeGenericMethod(new[] { type });
                specificRegisterMethod.Invoke(null, new object[] { registry });
            }
        }
        /// <summary>
        /// Registers the specified registry.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="registry">The registry.</param>
        static private void Register<T>(Registry registry) where T : IPage {
            registry.For<T>().UseSpecial(y => y.ConstructedBy(r => GetCurrentPage<T>()));
        }
        /// <summary>
        /// Gets the current content.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        static private T GetCurrentPage<T>() where T : IPage {
            var handler = (MvcHandler)HttpContext.Current.Handler;
            if (handler == null)
                return default(T);
            return (T)handler.RequestContext.RouteData.Values["currentPage"];
        }
    }
}