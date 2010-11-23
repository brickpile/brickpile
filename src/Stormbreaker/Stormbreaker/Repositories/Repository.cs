using System;
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
        #region public void Update(IContentItem item)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void Update(IContentItem item)
        {
            item.MetaData.Updated = DateTime.Now;
            _documentSession.Store(item);
            _documentSession.SaveChanges();
        }
        #endregion
    }
}