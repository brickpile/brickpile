using System;
using System.Linq;
using BrickPile.Domain.Models;

namespace BrickPile.Core.Infrastructure.Common {
    public static class QueryableExtensions {
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