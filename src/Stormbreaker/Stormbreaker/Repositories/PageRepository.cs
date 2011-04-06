using System;
using System.Collections.Generic;
using System.Linq;
using Raven.Client;
using Stormbreaker.Extensions;
using Stormbreaker.Models;

namespace Stormbreaker.Repositories {
    /// <summary>
    /// Page repository implementation of <see cref="IPageRepository" /> that provides support for basic operations for objects implementing <see cref="IPageModel" /> interface.
    /// </summary>
    /// <remarks></remarks>
    /// <example></example>
    public class PageRepository : IPageRepository {
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
        /// Childrens the specified parent.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent">The parent.</param>
        /// <returns></returns>
        public IEnumerable<T> Children<T>(T parent) where T : IPageModel {
            return _documentSession.Advanced.LuceneQuery<T>("Documents/ByParent")
                .Where("Id:" + parent.Id)
                .WaitForNonStaleResults()
                .ToArray();
        }

        public T ByUrl<T>(string url) where T : IPageModel {
            return _documentSession.Advanced.LuceneQuery<T>("Document/ByUrl")
                .Where("Url:" + url)
                .WaitForNonStaleResults()
                .FirstOrDefault();
        }
        /// <summary>
        /// Loads a specific page with a specific id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Load<T>(string id) {
            return _documentSession.Load<T>(id);
        }
        /// <summary>
        /// Saves a page to the Raven server.
        /// </summary>
        /// <remarks>Also responsible for generating the slug</remarks>
        /// <param name="entity"></param>
        public void Store(IPageModel entity) {
            _documentSession.Store(entity);
        }
        /// <summary>
        /// Marks a specific page for deletion, the page will be deleted when <see cref="SaveChanges" /> is called. 
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(IPageModel entity) {
            _documentSession.Delete(entity);
        }
        /// <summary>
        /// Saves all the changes the Raven server.
        /// </summary>
        public void SaveChanges() {
            _documentSession.SaveChanges();
        }
        public void Refresh(IPageModel entity) {
            _documentSession.Advanced.Refresh(entity);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="PageRepository" /> class.
        /// </summary>
        /// <param name="documentSession"></param>
        public PageRepository(IDocumentSession documentSession) {
            _documentSession = documentSession;
        }
    }
}