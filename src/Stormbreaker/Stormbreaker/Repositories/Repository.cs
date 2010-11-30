using System;
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
    public class Repository : IRepository {
        private readonly IDocumentSession _documentSession;
        /* *******************************************************************
	    * Constructors
	    * *******************************************************************/
        #region public Repository(IDocumentSession documentSession)
        /// <summary>
        /// Initializes a new instance of the <b>Repository</b> class.
        /// </summary>
        /// <param name="documentSession"></param>
        public Repository(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }
        #endregion
        /* *******************************************************************
	    * Methods
	    * *******************************************************************/
        #region public T GetByUrlSegment<T>(string urlSegment)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="urlSegment"></param>
        /// <returns></returns>
        public T GetByUrlSegment<T>(string urlSegment) where T : IContentItem
        {
            return _documentSession.Query<T, Indexes.DocumentsByUrlSegment>().Where(x => x.MetaData.UrlSegment == urlSegment).SingleOrDefault();
        }
        #endregion
        #region public T Get<T>(string id)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Get<T>(string id)
        {
            return _documentSession.Load<T>(id);
        }
        #endregion
        #region public T Get<T>(Func<T, bool> where)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public T Get<T>(Func<T, bool> where)
        {
            return _documentSession.Query<T>().Where(where).SingleOrDefault();
        }
        #endregion
        #region public IEnumerable<IContentItem> GetChildrenFor(IContentItem item)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public IEnumerable<IContentItem> GetChildrenFor(IContentItem item)
        {
            return from items in _documentSession.Query<IContentItem>().Where(x => x.StructureInfo.ParentId == item.Id).ToList() select items;
        }
        #endregion
        #region public void Save(IContentItem item)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void Save(IContentItem item)
        {
            item.MetaData.Created = DateTime.Now;
            item.MetaData.Updated = DateTime.Now;
            _documentSession.Store(item);
        }
        #endregion
        #region public void SaveChanges()
        /// <summary>
        /// 
        /// </summary>
        public void SaveChanges()
        {
            _documentSession.SaveChanges();
        }
        #endregion
    }
}