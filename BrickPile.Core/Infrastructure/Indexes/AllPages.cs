using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace BrickPile.Core.Infrastructure.Indexes {
    /// <summary>
    /// 
    /// </summary>
    public class AllPages : AbstractMultiMapIndexCreationTask<IPage> {

        /// <summary>
        /// Initializes a new instance of the <see cref="AllPages"/> class.
        /// </summary>
        public AllPages() {
            AddMapForAll<IPage>(pages => from page in pages
                                         where !page.Id.Contains("/draft")
                                          select new {
                                              page.Id,
                                              Metadata_Url = page.Metadata.Url,
                                              Metadata_SortOrder = page.Metadata.SortOrder,
                                              Metadata_Name = page.Metadata.Name,
                                              Metadata_IsPublished = page.Metadata.IsPublished,
                                              Metadata_IsDeleted = page.Metadata.IsDeleted,
                                              Metadata_Title = page.Metadata.Title,
                                              Metadata_Changed = page.Metadata.Changed,
                                              Metadata_ChangedBy = page.Metadata.ChangedBy,
                                              Metadata_Description = page.Metadata.Description,
                                              Metadata_DisplayInMenu = page.Metadata.DisplayInMenu,
                                              Metadata_Keywords = page.Metadata.Keywords,
                                              Metadata_Published = page.Metadata.Published,
                                              Metadata_Slug = page.Metadata.Slug,
                                              page.Parent,
                                              Parent_Id = page.Parent.Id
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
