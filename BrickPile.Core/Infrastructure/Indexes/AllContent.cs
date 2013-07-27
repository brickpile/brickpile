using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using BrickPile.Domain.Models;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace BrickPile.Core.Infrastructure.Indexes {
    public class AllPages : AbstractMultiMapIndexCreationTask<IPage> {

        /// <summary>
        /// Initializes a new instance of the <see cref="AllPages"/> class.
        /// </summary>
        public AllPages() {

            AddMapForAll<IPage>(assets => from asset in assets
                                          where !asset.Id.Contains("draft")
                                          select new {
                                              asset.Id,
                                              Metadata_Url = asset.Metadata.Url,
                                              Metadata_SortOrder = asset.Metadata.SortOrder,
                                              asset.Parent,
                                              Parent_Id = asset.Parent.Id
                                          });

            Sort(x => x.Metadata.SortOrder, SortOptions.Int);
        }
        /// <summary>
        /// Adds the map for all.
        /// </summary>
        /// <typeparam name="TBase">The type of the base.</typeparam>
        /// <param name="expr">The expr.</param>
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
