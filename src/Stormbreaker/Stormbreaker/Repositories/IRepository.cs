using Stormbreaker.Models;

namespace Stormbreaker.Repositories {
    /// <summary>
    /// Represents a common repository 
    /// </summary>
    public interface IRepository {
        /// <summary>
        /// <see cref="PageRepository.Load{T}" />
        /// </summary>
        T Load<T>(string id);
        /// <summary>
        /// <see cref="PageRepository.Store" />
        /// </summary>
        void Store(IPageModel entity);
        /// <summary>
        /// <see cref="PageRepository.Delete" />
        /// </summary>
        void Delete(IPageModel entity);
        /// <summary>
        /// <see cref="PageRepository.SaveChanges" />
        /// </summary>
        void SaveChanges();
    }
}