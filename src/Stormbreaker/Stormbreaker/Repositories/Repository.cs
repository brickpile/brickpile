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

        public TEntity Load<TEntity>(string id)
        {
            return _documentSession.Load<TEntity>(id);
        }
        public IEnumerable<T> GetChildren<T>(IDocument entity) {
            return _documentSession.Advanced.LuceneQuery<T>("DocumentsByStructureInfo").Where("Id:" + entity.Id).ToArray();
        }




        public TEntity[] LoadEntityBySlug<TEntity>(string slug) {
            return _documentSession.Advanced.LuceneQuery<TEntity>("DocumentsBySlug").Include("Children").Where("Slug:" + slug).ToArray();
        }

        public TEntity[] LoadChildren<TEntity>(IDocument entity) {
            return
                _documentSession.Advanced.LuceneQuery<TEntity>("DocumentsByStructureInfo").Where(
                    "Id:" + entity.Id).ToArray();
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