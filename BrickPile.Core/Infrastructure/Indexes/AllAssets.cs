using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using BrickPile.Domain.Models;
using Raven.Client.Documents.Indexes;

namespace BrickPile.Core.Infrastructure.Indexes
{
    /// <summary>
    ///     Represents the multi map index that makes it possible to search across all assets
    /// </summary>
    public class AllAssets : AbstractMultiMapIndexCreationTask<Asset>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AllAssets" /> class.
        /// </summary>
        public AllAssets()
        {
            this.AddMapForAll<Asset>(assets => from asset in assets
                select new
                {
                    asset.Id,
                    asset.Name,
                    asset.ContentType,
                    asset.DateUploaded,
                    asset.VirtualPath,
                    asset.ContentLength
                });

            Indexes.Add(x => x.ContentType, FieldIndexing.Search);
        }

        /// <summary>
        ///     Adds the map for all.
        /// </summary>
        /// <typeparam name="TBase">The type of the base.</typeparam>
        /// <param name="expr">The expr.</param>
        protected new void AddMapForAll<TBase>(Expression<Func<IEnumerable<TBase>, IEnumerable>> expr)
        {
            // Index the base class.
            if (typeof(TBase).GetTypeInfo().IsAbstract == false &&
                typeof(TBase).GetTypeInfo().IsInterface == false)
                AddMap(expr);

            // Index child classes.
            var children = AppDomain.CurrentDomain.GetAssemblies().ToList().SelectMany(s => s.GetTypes()).Where(p => typeof(TBase).IsAssignableFrom(p));
            var addMapGeneric = GetType().GetMethod("AddMap", BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (var child in children)
            {
                if (child.GetTypeInfo().IsGenericTypeDefinition ||
                    child.GetTypeInfo().IsAbstract ||
                    child.GetTypeInfo().IsInterface)
                    continue;

                var genericEnumerable = typeof(IEnumerable<>).MakeGenericType(child);
                var delegateType = typeof(Func<,>).MakeGenericType(genericEnumerable, typeof(IEnumerable));
                var lambdaExpression = Expression.Lambda(delegateType, expr.Body, Expression.Parameter(genericEnumerable, expr.Parameters[0].Name));
                addMapGeneric.MakeGenericMethod(child).Invoke(this, new[] { lambdaExpression });
            }
        }
    }
}