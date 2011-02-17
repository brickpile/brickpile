using Stormbreaker.Models;

namespace Stormbreaker.Repositories {
    public interface IPageRepository : IRepository<IPageModel> {
        /// <summary>
        /// <see cref="PageRepository.GetChildren{T}<>" />
        /// </summary>
        IPageModel[] GetChildren<T>(IPageModel parent);
        /// <summary>
        /// <see cref="PageRepository.GetPageBySlug{T}<>" />
        /// </summary>
        T GetPageBySlug<T>(string slug);
    }
}