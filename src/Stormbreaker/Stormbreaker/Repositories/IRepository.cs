using Stormbreaker.Models;

namespace Stormbreaker.Repositories {
    /// <summary>
    /// Represents a repository 
    /// </summary>
    public interface IRepository {
        /// <summary>
        /// <see cref="PageRepository.GetChildren{T}<>" />
        /// </summary>
        IPageModel[] GetChildren<T>(IPageModel entity);
        /// <summary>
        /// <see cref="PageRepository.GetPageBySlug{T}<>" />
        /// </summary>
        T GetPageBySlug<T>(string slug);
        /// <summary>
        /// <see cref="PageRepository.Load{T}<>" />
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