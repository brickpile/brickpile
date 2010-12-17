using System.Collections.Generic;
using Stormbreaker.Models;

namespace Stormbreaker.Repositories {
    /// <summary>
    /// The IRepository interface 
    /// </summary>
    public interface IRepository {

        IEnumerable<T> GetChildren<T>(IDocument entity);

        T LoadEntityBySlug<T>(string slug);
        T Load<T>(string id);

        void Store(IDocument entity);
        void Delete(IDocument entity);
        void SaveChanges();
    }
}