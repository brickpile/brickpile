using BrickPile.Domain.Models;
using Raven.Client.Listeners;
using Raven.Json.Linq;

namespace BrickPile.Core.Infrastructure.Listeners {
    /// <summary>
    /// 
    /// </summary>
    internal class DeleteListener : IDocumentDeleteListener {
        /// <summary>
        /// Invoked before the delete request is sent to the server.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="entityInstance">The entity instance.</param>
        /// <param name="metadata">The metadata.</param>
        public void BeforeDelete(string key, object entityInstance, RavenJObject metadata) {
            if (entityInstance is IPageModel) {
                var model = entityInstance as IPageModel;
                Application.Instance.OnDeletingPage(new PageModelEventArgs(model));
            }
        }
    }
}