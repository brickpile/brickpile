using System.Linq;
using BrickPile.Domain.Models;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace BrickPile.Core.Infrastructure.Indexes {
    /// <summary>
    /// 
    /// </summary>
    public class AllAssets : AbstractMultiMapIndexCreationTask<Asset> {

        /// <summary>
        /// Initializes a new instance of the <see cref="AllAssets"/> class.
        /// </summary>
        public AllAssets() {

            AddMapForAll<Asset>(assets => from asset in assets
                                          select new
                                          {
                                              asset.Id,
                                              asset.Name,
                                              asset.ContentType,
                                              asset.DateUploaded,
                                              asset.VirtualPath,
                                              asset.ContentLength
                                          });

            Indexes.Add(x => x.ContentType, FieldIndexing.Analyzed);

        }

        /// <summary>
        /// Adds the map for all.
        /// </summary>
        /// <typeparam name="TBase">The type of the base.</typeparam>
        /// <param name="expr">The expr.</param>
        //protected new void AddMapForAll<TBase>(Expression<Func<IEnumerable<TBase>, IEnumerable>> expr) {
        //    // Index the base class.
        //    AddMap(expr);

        //    // Index child classes from all assemblies
        //    var children = AppDomain.CurrentDomain.GetAssemblies().ToList()
        //        .SelectMany(s => s.GetTypes())
        //        .Where(p => typeof(TBase).IsAssignableFrom(p));

        //    var addMapGeneric = GetType().GetMethod("AddMap", BindingFlags.Instance | BindingFlags.NonPublic);
        //    foreach (var child in children) {
        //        var genericEnumerable = typeof(IEnumerable<>).MakeGenericType(child);
        //        var delegateType = typeof(Func<,>).MakeGenericType(genericEnumerable, typeof(IEnumerable));
        //        var lambdaExpression = Expression.Lambda(delegateType, expr.Body, Expression.Parameter(genericEnumerable, expr.Parameters[0].Name));
        //        addMapGeneric.MakeGenericMethod(child).Invoke(this, new[] { lambdaExpression });
        //    }
        //}
    }
}