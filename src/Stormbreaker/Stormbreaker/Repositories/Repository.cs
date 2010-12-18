using System.Collections.Generic;
using System.Linq;
using Raven.Client;
using Stormbreaker.Models;

namespace Stormbreaker.Repositories {
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    /// <example></example>
    public class Repository : IRepository
    {
        private readonly IDocumentSession _documentSession;
        public Repository(IDocumentSession documentSession) {
            _documentSession = documentSession;
        }

        public T Load<T>(string id)
        {
            return _documentSession.Load<T>(id);
        }

        public IDocument[] GetChildren<T>(IDocument parent) {
            return _documentSession.Query<IDocument>().Where(d => d.Parent.Id == parent.Id).ToArray();
        }

        public T LoadDocumentBySlug<T>(string slug) {
            return _documentSession.Advanced.LuceneQuery<T>("Document/BySlug").Where("Slug:" + slug).FirstOrDefault();
        }

        public void Store(IDocument entity) {
            _documentSession.Store(entity);
        }
        public void Delete(IDocument entity) {
            _documentSession.Delete(entity);
        }
        public void SaveChanges() {
            _documentSession.SaveChanges();
        }
    }
}