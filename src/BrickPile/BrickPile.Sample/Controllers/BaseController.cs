using System.Collections.Generic;
using System.Web.Mvc;
using BrickPile.Core.Infrastructure.Common;
using BrickPile.Domain.Models;
using BrickPile.UI;
using BrickPile.UI.Common;
using Raven.Client;

namespace BrickPile.Sample.Controllers {
    public abstract class BaseController<T> : Controller where T : IPageModel {
        /// <summary>
        /// Gets the current model.
        /// </summary>
        public T CurrentModel { get; private set; }
        /// <summary>
        /// Gets the document session.
        /// </summary>
        public IDocumentSession DocumentSession { get; private set; }
        /// <summary>
        /// Gets the hierarchy
        /// </summary>
        public virtual IEnumerable<IHierarchyNode<IPageModel>> Hierarchy {
            get {
                return DocumentSession.LoadFrom<IPageModel>(x => x.Id == CurrentModel.Id)
                    .WherePageIsPublished()
                    .WherePageIsVisibleInMenu()
                    .AsHierarchy();
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="documentSession">The document session.</param>
        protected BaseController(IPageModel model, IDocumentSession documentSession) {
            CurrentModel = (T)model;
            DocumentSession = documentSession;
        }
    }
}
