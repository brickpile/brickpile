/* Copyright (C) 2011 by Marcus Lindblom

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE. */

using System.Collections.Generic;
using System.Linq;
using Stormbreaker.Models;
using Stormbreaker.Web.UI;

namespace Stormbreaker.Common {
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
        public static IEnumerable<HierarchyNode<TEntity>> CreateHierarchy<TEntity>(this IEnumerable<TEntity> allItems, TEntity rootPage, int depth) where TEntity : IPageModel {
            var childs = allItems.Where(x => x.Parent.Id.Equals(rootPage.Id));
            if (childs.Count() > 0) {
                childs.OrderByDescending(x => x.SortOrder);
                depth++;
                foreach (var item in childs)
                    yield return new HierarchyNode<TEntity>
                    {
                        Entity = item,
                        ChildNodes = CreateHierarchy(allItems, item, depth),
                        Depth = depth,
                        Expanded = allItems.Where(x => x.Parent.Id.Equals(item.Id)).Count() > 0
                    };
            }
        }
    }
}