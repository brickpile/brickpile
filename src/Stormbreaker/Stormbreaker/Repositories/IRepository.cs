using System;
using System.Collections.Generic;
using Stormbreaker.Models;

namespace Stormbreaker.Repositories {
    /// <summary>
    /// The IRepository interface 
    /// </summary>
    public interface IRepository {
        T GetByUrlSegment<T>(string urlSegment) where T : IContentItem;
        T Get<T>(string id);
        T Get<T>(Func<T, bool> where);
        void Save(IContentItem item);
        void SaveChanges();
        IEnumerable<IContentItem> GetChildrenFor(IContentItem item);
    }
}