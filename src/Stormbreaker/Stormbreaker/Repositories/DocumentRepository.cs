using Raven.Client;
using Stormbreaker.Models;

namespace Stormbreaker.Repositories {
    public class DocumentRepository : IDocumentRepository {

        private readonly IDocumentSession _documentSession;

        public T Load<T>(string id) {
            return _documentSession.Load<T>(id);
        }

        public void Store(IImageModel entity)
        {
            _documentSession.Store(entity);
        }

        public void Delete(IImageModel entity)
        {
            _documentSession.Delete(entity);
        }

        public void SaveChanges() {
            _documentSession.SaveChanges();
        }
        public DocumentRepository(IDocumentSession documentSession) {
            _documentSession = documentSession;
        }
    }
}