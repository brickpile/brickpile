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

        //public T SingleOrDefault<T>(Func<T, bool> predicate) where T : IPageModel
        //{
        //    return _documentSession.Query<T>().SingleOrDefault(predicate);
        //}

        /// <summary>
        /// Gets the children of a page.
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public IPageModel[] GetChildren<T>(IPageModel parent)
        {

            return _documentSession.Query<IPageModel>().Where(d => d.Parent.Id == parent.Id).ToArray();
            //return _documentSession.Advanced.LuceneQuery<IPageModel>()
            //    .Where("Parent.Id:" + parent.Id)
            //    .WaitForNonStaleResults()
            //    .WhereEquals("@metadata.Raven-Document-Revision-Status", "Current").ToArray();
        }
        /// <summary>
        /// Gets a page from RavenDB based on the slug.
        /// </summary>
        /// <param name="slug"></param>
        /// <returns></returns>
        public T GetPageBySlug<T>(string slug)
        {
            return _documentSession.Advanced.LuceneQuery<T>("Document/BySlug")
                .Where("Slug:" + slug)
                .WaitForNonStaleResults()
                //.WhereEquals("@metadata.Raven-Document-Revision-Status", "Current")
                .FirstOrDefault();
        }
        /// <summary>
        /// Loads a specific page with a specific id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Load<T>(string id) {
            //return _documentSession.Advanced.LuceneQuery<T>()
                //.Where("Id:" + id)
                //.WaitForNonStaleResults()
                //.WhereEquals("@metadata.Raven-Document-Revision-Status", "Current")
                //.SingleOrDefault();
            return _documentSession.Load<T>(id);
        }
        /// <summary>
        /// Saves a page to the Raven server.
        /// </summary>
        /// <remarks>Also responsible for generating the slug</remarks>
        /// <param name="entity"></param>
        public void Store(IPageModel entity)
        {
            entity.MetaData.Slug = entity.GenerateSlug();
            _documentSession.Store(entity);
        }
        /// <summary>
        /// Marks a specific page for deletion, the page will be deleted when <see cref="SaveChanges" /> is called. 
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(IPageModel entity)
        {
            _documentSession.Delete(entity);
        }
        /// <summary>
        /// Saves all the changes the Raven server.
        /// </summary>
        public void SaveChanges()
        {
            _documentSession.SaveChanges();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="PageRepository" /> class.
        /// </summary>
        /// <param name="documentSession"></param>
        public PageRepository(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }
    }
}