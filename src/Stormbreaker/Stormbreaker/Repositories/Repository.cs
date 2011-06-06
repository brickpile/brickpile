using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Client;

namespace Stormbreaker.Repositories {
    public class Repository<T> : IRepository<T> {
        private readonly IDocumentSession _documentSession;
        /// <summary>
        /// Singles the or default.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public T SingleOrDefault<T>(Func<T, bool> predicate) {
            return _documentSession.Query<T>().SingleOrDefault(predicate);
        }
        /// <summary>
        /// Lists this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> List<T>() {
            return _documentSession.Query<T>();
        }
        /// <summary>
        /// Stores the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Store(T entity) {
            _documentSession.Store(entity);
        }
        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Delete(T entity) {
            _documentSession.Delete(entity);
        }
        /// <summary>
        /// Saves the changes.
        /// </summary>
        public void SaveChanges() {
            _documentSession.SaveChanges();
        }
        /// <summary>
        /// Refreshes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Refresh(T entity) {
            _documentSession.Advanced.Refresh(entity);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Repository&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="documentSession">The document session.</param>
        public Repository(IDocumentSession documentSession) {
            _documentSession = documentSession;
        }
    }
}
