using System.Collections.Generic;
using System.Linq;
using Stormbreaker.Models;
using Stormbreaker.Web.UI;

namespace Stormbreaker.Extensions {
    /// <summary>
    /// Extension methods for collections implementing IEnumerable<T>.
    /// </summary>
    /// <remarks></remarks>
    /// <example></example>
    public static class EnumerableExtensions {
        /// <summary>
        /// Creates an hierarchical structure of <see cref="IPageModel"/> objects used in navigation scenarios.
        /// </summary>
        /// <param name="allItems">Collection to create the hierarchy from</param>
        /// <param name="rootPage"></param>
        /// <param name="depth"></param>
        /// <returns></returns>
        public static IEnumerable<HierarchyNode<TEntity>> CreateHierarchy<TEntity>(this IEnumerable<TEntity> allItems, TEntity rootPage, int depth) where TEntity : IPageModel
        {
            IEnumerable<TEntity> childs = allItems.Where(x => x.Parent.Id.Equals(rootPage.Id));

            if (childs.Count() > 0)
            {
                depth++;

                foreach (var item in childs)
                    yield return new HierarchyNode<TEntity>
                    {
                        Entity = item,
                        ChildNodes = CreateHierarchy(allItems, item, depth),
                        Depth = depth
                    };
            }
        }
    }
}