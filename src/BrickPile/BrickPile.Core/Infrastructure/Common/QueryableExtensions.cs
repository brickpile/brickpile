using System.Linq;
using BrickPile.Domain.Models;

namespace BrickPile.Core.Infrastructure.Common {
    public static class QueryableExtensions {
        public static IQueryable<IPageModel> GetChildren(this IQueryable<IPageModel> query, IPageModel parent) {
            return query.Where(model => model.Parent.Id == parent.Id);
        }
    }
}