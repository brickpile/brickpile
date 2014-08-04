using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrickPile.Core.Extensions
{
    /// <summary>
    ///     Containing extensions for the collection objects.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Filters a collection of pages and removes unpublished pages.
        /// </summary>
        /// <param name="pages">The pages.</param>
        /// <returns></returns>
        public static IEnumerable<IPage> FilterForDisplay(this IEnumerable<IPage> pages)
        {
            var p = pages.ToList();

            for (var i = 0; i < p.Count; i++)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated || (p[i].Metadata.IsPublished)) continue;
                p.RemoveAt(i);
                i--;
            }

            return p.OrderBy(x => x.Metadata.SortOrder);
        }

        /// <summary>
        ///     Converts a <see cref="IEnumerable{T}" /> to a flat data object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
        internal static IEnumerable<T> Flatten<T>(this T source, Func<T, IEnumerable<T>> selector)
        {
            var queue = new Queue<T>();

            queue.Enqueue(source);

            while (queue.Any())
            {
                var item = queue.Dequeue();
                yield return item;
                var children = selector(item);
                if (children == null)
                    continue;

                foreach (var child in children)
                    queue.Enqueue(child);
            }
        }

        /// <summary>
        ///     Converts a <see cref="IEnumerable{IPage}" /> to an hierarchical <see cref="IEnumerable{dynamic}" /> used for
        ///     navigation purpose.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="allItems">All items.</param>
        /// <param name="parentItem">The parent item.</param>
        /// <param name="depth">The depth.</param>
        /// <returns></returns>
        private static IEnumerable<dynamic> CreateHierarchy<TEntity>(IEnumerable<TEntity> allItems, TEntity parentItem,
            int depth) where TEntity : class, IPage
        {
            if (parentItem == null)
                parentItem = allItems.SingleOrDefault(i => i.Parent == null);

            if (parentItem == null)
            {
                yield break;
            }

            var childs =
                allItems.Where(
                    i =>
                        i.Parent != null &&
                        i.Parent.Id.Replace("/draft", "").Equals(parentItem.Id.Replace("/draft", "")));

            if (childs.Any())
            {
                depth++;

                foreach (var item in childs)
                    yield return
                        new
                        {
                            Entity = item,
                            ChildNodes = CreateHierarchy(allItems, item, depth),
                            Depth = depth,
                            Expanded =
                                allItems.Any(
                                    x =>
                                        x.Parent != null &&
                                        x.Parent.Id.Replace("/draft", "").Equals(item.Id.Replace("/draft", "")))
                        };
            }
        }

        /// <summary>
        ///     Converts a <see cref="IEnumerable{IPage}" /> to an hierarchical <see cref="IEnumerable{dynamic}" /> used for
        ///     navigation purpose.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="allItems">All items.</param>
        /// <returns></returns>
        internal static IEnumerable<dynamic> AsHierarchy<TEntity>(this IEnumerable<TEntity> allItems)
            where TEntity : class, IPage
        {
            return CreateHierarchy(allItems, default(TEntity), 0);
        }
    }
}