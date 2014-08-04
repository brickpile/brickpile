using System;
using Raven.Client.Listeners;
using Raven.Json.Linq;

namespace BrickPile.Core.Infrastructure.Listeners
{
    /// <summary>
    ///     Hook for users to provide additional logic on delete operations
    /// </summary>
    internal class DeleteListener : IDocumentDeleteListener
    {
        private readonly Action<string, IPage, RavenJObject> onDocumentDelete;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DeleteListener" /> class.
        /// </summary>
        /// <param name="onDocumentDelete">The on document delete.</param>
        public DeleteListener(Action<string, IPage, RavenJObject> onDocumentDelete)
        {
            this.onDocumentDelete = onDocumentDelete;
        }

        /// <summary>
        ///     Invoked before the delete request is sent to the server.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="entityInstance">The entity instance.</param>
        /// <param name="metadata">The metadata.</param>
        public void BeforeDelete(string key, object entityInstance, RavenJObject metadata)
        {
            var entity = entityInstance as IPage;

            if (entity == null)
                return;

            if (DocumentListenerContext.IsInDocumentListenerContext)
                return;

            using (DocumentListenerContext.Enter())
            {
                this.onDocumentDelete(key, entity, metadata);
            }
        }
    }
}