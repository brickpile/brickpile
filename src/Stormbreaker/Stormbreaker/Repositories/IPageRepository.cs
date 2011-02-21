using Stormbreaker.Models;

namespace Stormbreaker.Repositories {
    public interface IPageRepository : IRepository<IPageModel> {
        /// <summary>
        /// Gets the children.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent">The parent.</param>
        /// <returns></returns>
        IPageModel[] GetChildren<T>(IPageModel parent);
        /// <summary>
        /// Gets the page by slug.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="slug">The slug.</param>
        /// <returns></returns>
        T GetPageBySlug<T>(string slug);
        /// <summary>
        /// Gets all pages.
        /// </summary>
        /// <returns></returns>
        IPageModel[] GetAllPages();
    }
}