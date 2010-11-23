using System;
using Stormbreaker.Models;

namespace Stormbreaker.Repositories {
    /// <summary>
    /// The IRepository interface 
    /// </summary>
    public interface IRepository {
        /* *******************************************************************
	    * Methods
	    * *******************************************************************/
        T Get<T>(Func<T, bool> where);
        void Update(IContentItem item);
    }
}