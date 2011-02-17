using Stormbreaker.Models;

namespace Stormbreaker.Repositories {
    /// <summary>
    /// Represents a common repository 
    /// </summary>
    public interface IRepository<T> {
        /// <summary>
        /// <see cref="PageRepository.Load{T}" />
        /// </summary>
        T Load<T>(string id);
        /// <summary>
        /// <see cref="PageRepository.Store" />
        /// </summary>
        void Store(T entity);
        /// <summary>
        /// <see cref="PageRepository.Delete" />
        /// </summary>
        void Delete(T entity);
        /// <summary>
        /// <see cref="PageRepository.SaveChanges" />
        /// </summary>
        void SaveChanges();
    }
}