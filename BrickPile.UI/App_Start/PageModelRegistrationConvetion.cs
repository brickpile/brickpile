using System;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using BrickPile.Domain.Models;
using BrickPile.UI.Common;
using BrickPile.UI.Web.ViewModels;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace BrickPile.UI {
    //public class PageTypeRegistrationConvetion : IRegistrationConvention {
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public static readonly MethodInfo RegisterMethod = typeof(PageTypeRegistrationConvetion)
    //        .GetMethod("Register", BindingFlags.NonPublic | BindingFlags.Static)
    //        .GetGenericMethodDefinition();
    //    /// <summary>
    //    /// Processes the specified type.
    //    /// </summary>
    //    /// <param name="type">The type.</param>
    //    /// <param name="registry">The registry.</param>
    //    public void Process(Type type, Registry registry) {
    //        if (typeof(IPageModel).IsAssignableFrom(type)) {
    //            var specificRegisterMethod = RegisterMethod.MakeGenericMethod(new[] { type });
    //            specificRegisterMethod.Invoke(null, new object[] { registry });
    //        }
    //    }
    //    /// <summary>
    //    /// Registers the specified registry.
    //    /// </summary>
    //    /// <typeparam name="T"></typeparam>
    //    /// <param name="registry">The registry.</param>
    //    static private void Register<T>(Registry registry) where T : IPageModel {
    //        registry.For<T>().UseSpecial(y => y.ConstructedBy(r => GetCurrentPage<T>()));
    //    }
    //    /// <summary>
    //    /// Gets the current page model.
    //    /// </summary>
    //    /// <typeparam name="T"></typeparam>
    //    /// <returns></returns>
    //    static private T GetCurrentPage<T>() where T : IPageModel {
    //        var handler = (MvcHandler)HttpContext.Current.Handler;
    //        if (handler == null)
    //            return default(T);
    //        return handler.RequestContext.RouteData.GetCurrentPage<T>();
    //    }
    //}

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
            if (typeof(IContent).IsAssignableFrom(type)) {
                var specificRegisterMethod = RegisterMethod.MakeGenericMethod(new[] { type });
                specificRegisterMethod.Invoke(null, new object[] { registry });
            }
        }
        /// <summary>
        /// Registers the specified registry.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="registry">The registry.</param>
        static private void Register<T>(Registry registry) where T : IContent {
            registry.For<T>().UseSpecial(y => y.ConstructedBy(r => GetCurrentPage<T>()));
        }
        /// <summary>
        /// Gets the current page model.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        static private T GetCurrentPage<T>() where T : IContent {
            var handler = (MvcHandler)HttpContext.Current.Handler;
            if (handler == null)
                return default(T);
            return handler.RequestContext.RouteData.GetCurrentContent<T>();
        }
    }
}