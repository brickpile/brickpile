using System.Web.Mvc;
using BrickPile.Domain.Models;
using BrickPile.UI.Common;
using Raven.Client;
using StructureMap;

namespace BrickPile.Samples.Controllers {
    public abstract class BaseController<TContent> : Controller where TContent : IContent {
        /// <summary>
        /// Gets or sets the current page.
        /// </summary>
        /// <value>
        /// The current page.
        /// </value>
        public IPageModel CurrentPage {
            get { return ControllerContext.RouteData.GetCurrentPage<IPageModel>(); }
        }
        /// <summary>
        /// Gets or sets the content of the current.
        /// </summary>
        /// <value>
        /// The content of the current.
        /// </value>
        public TContent CurrentContent {
            get { return ControllerContext.RouteData.GetCurrentContent<TContent>(); }
        }
        /// <summary>
        /// Gets the document session.
        /// </summary>
        public IDocumentSession DocumentSession {
            get { return ObjectFactory.GetInstance<IDocumentSession>(); }
        }
    }
}