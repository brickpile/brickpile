using System;
using Raven.Json.Linq;

namespace BrickPile.Core.Infrastructure.Listeners
{
    internal class DeleteListener : Raven.Client.Listeners.IDocumentDeleteListener
    {
        private readonly Action<string, IPage, RavenJObject> _onDocumentDelete;

        /// <summary>
        /// Invoked before the delete request is sent to the server.
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
                _onDocumentDelete(key, entity, metadata);
            }            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteListener"/> class.
        /// </summary>
        /// <param name="onDocumentDelete">The on document delete.</param>
        public DeleteListener(Action<string, IPage, RavenJObject> onDocumentDelete)
        {
            _onDocumentDelete = onDocumentDelete;
        }
    }
}
