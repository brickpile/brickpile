using System;
using System.Web;
using Raven.Client.Listeners;
using Raven.Json.Linq;

namespace BrickPile.Core.Infrastructure.Listeners
{
    /// <summary>
    ///     Hook for users to provide additional logic on store operations
    /// </summary>
    internal class StoreListener : IDocumentStoreListener
    {
        private readonly Action<string, IPage, RavenJObject> onPagePublish;
        private readonly Action<string, IPage, RavenJObject> onPageSave;
        private readonly Action<string, IPage, RavenJObject> onPageUnpublish;

        /// <summary>
        ///     Initializes a new instance of the <see cref="StoreListener" /> class.
        /// </summary>
        /// <param name="onPagePublish">The on page publish.</param>
        /// <param name="onPageSave">The on page save.</param>
        /// <param name="onPageUnPublish">The on page un publish.</param>
        public StoreListener(Action<string, IPage, RavenJObject> onPagePublish,
            Action<string, IPage, RavenJObject> onPageSave, Action<string, IPage, RavenJObject> onPageUnPublish)
        {
            this.onPagePublish = onPagePublish;
            this.onPageSave = onPageSave;
            this.onPageUnpublish = onPageUnPublish;
        }

        /// <summary>
        ///     Invoked before the store request is sent to the server.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="entityInstance">The entity instance.</param>
        /// <param name="metadata">The metadata.</param>
        /// <param name="original">The original document that was loaded from the server</param>
        /// <returns>
        ///     Whatever the entity instance was modified and requires us re-serialize it.
        ///     Returning true would force re-serialization of the entity, returning false would
        ///     mean that any changes to the entityInstance would be ignored in the current SaveChanges call.
        /// </returns>
        public bool BeforeStore(string key, object entityInstance, RavenJObject metadata, RavenJObject original)
        {
            var entity = entityInstance as IPage;

            if (entity == null)
                return false;

            if (DocumentListenerContext.IsInDocumentListenerContext)
                return false;

            var action = HttpContext.Current.Items["storeAction"] as StoreAction?;

            using (DocumentListenerContext.Enter())
            {
                switch (action)
                {
                    case StoreAction.None:
                        return false;
                    case StoreAction.Save:
                        //_onPageSave(key, entity, metadata);
                        this.onPagePublish(key, entity, metadata);
                        break;
                    case StoreAction.Publish:
                        this.onPagePublish(key, entity, metadata);
                        break;
                    case StoreAction.UnPublish:
                        //_onPageUnpublish(key, entity, metadata);
                        return false;
                    default:
                        return false;
                }
                return true;
            }
        }

        /// <summary>
        ///     Invoked after the store request is sent to the server.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="entityInstance">The entity instance.</param>
        /// <param name="metadata">The metadata.</param>
        public void AfterStore(string key, object entityInstance, RavenJObject metadata) {}
    }
}