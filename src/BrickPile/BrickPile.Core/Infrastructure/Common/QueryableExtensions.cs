using System.Linq;
using BrickPile.Domain.Models;

namespace BrickPile.Core.Infrastructure.Common {
    public static class QueryableExtensions {
        /// <summary>
        /// Gets the children.
        /// </summary>
        /// <param name="queryable">The queryable.</param>
        /// <param name="parent">The parent.</param>
        /// <returns></returns>
        public static IQueryable<IPageModel> GetChildren(this IQueryable<IPageModel> queryable, IPageModel parent) {
            return queryable.Where(model => model.Parent.Id == parent.Id);
        }
        /// <summary>
        /// Wheres the page is published.
        /// </summary>
        /// <param name="queryable">The queryable.</param>
        /// <returns></returns>
        public static IQueryable<IPageModel> WherePageIsPublished(this IQueryable<IPageModel> queryable) {
            return queryable.Where(model => model.Metadata.IsPublished == true);
        }
    }
}