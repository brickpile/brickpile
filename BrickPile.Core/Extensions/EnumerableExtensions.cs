using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrickPile.Core.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<IPage> FilterForDisplay(this IEnumerable<IPage> pages)
        {
            var p = pages.ToList();

            for (var i = 0; i < p.Count; i++)
            {
                if (!HttpContext.Current.User.Identity.IsAuthenticated && (!p[i].Metadata.IsPublished || !p[i].Metadata.DisplayInMenu))
                {
                    p.RemoveAt(i);
                    i--;
                }
            }

            return p.OrderBy(x => x.Metadata.SortOrder);
        }

        public static IEnumerable<T> Flatten<T>(this T source, Func<T, IEnumerable<T>> selector)
        {
            Queue<T> queue = new Queue<T>();
            
            queue.Enqueue(source);

            while (queue.Any())
            {
                T item = queue.Dequeue();
                yield return item;
                var children = selector(item);
                if (children == null)
                    continue;

                foreach (var child in children)
                    queue.Enqueue(child);
            }

        }

        private static IEnumerable<dynamic> CreateHierarchy<TEntity>(IEnumerable<TEntity> allItems, TEntity parentItem, int depth) where TEntity : class, IPage
        {

            if (parentItem == null)                
                parentItem = allItems.SingleOrDefault(i => i.Parent == null);

            if (parentItem == null)
            {
                yield break;
            }

            IEnumerable<TEntity> childs = allItems.Where(i => i.Parent != null && i.Parent.Id.Replace("/draft","").Equals(parentItem.Id.Replace("/draft","")));

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
                            Expanded = allItems.Any(x => x.Parent != null && x.Parent.Id.Replace("/draft","").Equals(item.Id.Replace("/draft","")))
                        };
            }
        }

        public static IEnumerable<dynamic> AsHierarchy<TEntity>(this IEnumerable<TEntity> allItems)
            where TEntity : class, IPage
        {
            return CreateHierarchy(allItems, default(TEntity), 0);
        }
    }
}
