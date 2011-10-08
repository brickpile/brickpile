using System.Web.Mvc;
using BrickPile.Domain.Models;
using BrickPile.UI;
using Raven.Client;

namespace BrickPile.Sample.Controllers {
    public abstract class BaseController<T> : Controller where T : IPageModel {
        /// <summary>
        /// Gets or sets the current page.
        /// </summary>
        /// <value>
        /// The current page.
        /// </value>
        public T CurrentModel { get; set; }
        /// <summary>
        /// Gets or sets the document session.
        /// </summary>
        /// <value>
        /// The document session.
        /// </value>
        public IDocumentSession DocumentSession { get; set; }
        /// <summary>
        /// Gets or sets the structure info.
        /// </summary>
        /// <value>
        /// The structure info.
        /// </value>
        public IStructureInfo StructureInfo { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseController&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="currentModel">The current model.</param>
        /// <param name="documentSession">The document session.</param>
        /// <param name="structureInfo">The structure info.</param>
        protected BaseController(IPageModel currentModel, IDocumentSession documentSession, IStructureInfo structureInfo) {
            CurrentModel = (T)currentModel;
            DocumentSession = documentSession;
            StructureInfo = structureInfo;
        }
    }
}
