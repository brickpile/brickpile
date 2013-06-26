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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using BrickPile.Domain.Models;
using Raven.Client.Indexes;

namespace BrickPile.Core.Infrastructure.Indexes {
    /// <summary>
    /// 
    /// </summary>
    public class PageModelWithParentsAndChildren : AbstractMultiMapIndexCreationTask<PageModel> {
        /// <summary>
        /// Initializes a new instance of the <see cref="PageModelWithParentsAndChildren"/> class.
        /// </summary>
        public PageModelWithParentsAndChildren() {
            AddMapForAll<PageModel>(pages => from page in pages
                                        select new {
                                            page.Id,
                                            page.Children
                                        });

            Sort(x => x.Metadata.SortOrder, Raven.Abstractions.Indexing.SortOptions.Int);

            TransformResults = (database, pages) => from page in pages
                                                    let ancestors = Recurse(page, c => database.Load<PageModel>(c.Parent.Id))
                                                    select new {
                                                        page.Id,
                                                        page.Children,
                                                        Ancestors =
                                                        (
                                                           from ancestor in ancestors
                                                           select new {
                                                               ancestor.Id,
                                                               ancestor.Children,
                                                           })
                                                    };
        }
        protected new void AddMapForAll<TBase>(Expression<Func<IEnumerable<TBase>, IEnumerable>> expr) {
            // Index the base class.
            AddMap(expr);

            // Index child classes from all assemblies
            var children = AppDomain.CurrentDomain.GetAssemblies().ToList()
                .SelectMany(s => s.GetTypes())
                .Where(p => typeof(TBase).IsAssignableFrom(p));

            var addMapGeneric = GetType().GetMethod("AddMap", BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (var child in children) {
                var genericEnumerable = typeof(IEnumerable<>).MakeGenericType(child);
                var delegateType = typeof(Func<,>).MakeGenericType(genericEnumerable, typeof(IEnumerable));
                var lambdaExpression = Expression.Lambda(delegateType, expr.Body, Expression.Parameter(genericEnumerable, expr.Parameters[0].Name));
                addMapGeneric.MakeGenericMethod(child).Invoke(this, new[] { lambdaExpression });
            }
        }
    }
}
