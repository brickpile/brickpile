using System.Web;
using BrickPile.Domain.Models;
using Raven.Client.Listeners;
using Raven.Json.Linq;

namespace BrickPile.Core.Infrastructure.Listeners {
    public class StoreListener : IDocumentStoreListener {
        /// <summary>
        /// Invoked before the store request is sent to the server.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="entityInstance">The entity instance.</param>
        /// <param name="metadata">The metadata.</param>
        /// <param name="original">The original document that was loaded from the server</param>
        /// <returns>
        /// Whatever the entity instance was modified and requires us re-serialize it.
        /// Returning true would force re-serialization of the entity, returning false would
        /// mean that any changes to the entityInstance would be ignored in the current SaveChanges call.
        /// </returns>
        public bool BeforeStore(string key, object entityInstance, RavenJObject metadata, RavenJObject original) {
            var entity = entityInstance as IPageModel;

            if (entity == null)
                return false;

            if (entity.Parent != null) {
                entity.Metadata.Url = entity.Metadata.Slug;
                entity.Metadata.Url = entity.Metadata.Url.Insert(0, VirtualPathUtility.AppendTrailingSlash(entity.Parent.Url ?? ""));
            }

            return true;
        }

        /// <summary>
        /// Invoked after the store request is sent to the server.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="entityInstance">The entity instance.</param>
        /// <param name="metadata">The metadata.</param>
        public void AfterStore(string key, object entityInstance, RavenJObject metadata) { }
    }
}