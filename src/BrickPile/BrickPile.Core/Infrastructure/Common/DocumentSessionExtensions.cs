using System;
using System.Collections.Generic;
using System.Linq;
using BrickPile.Domain.Models;
using Raven.Client;
using Raven.Client.Linq;

namespace BrickPile.Core.Infrastructure.Common {
    public static class DocumentSessionExtensions {
        /// <summary>
        /// Method for retrieving all ancestors with their direct children from a specific page
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session">The session.</param>
        /// <param name="predicate">The predicate eg. the Id of a document</param>
        /// <returns></returns>
        public static IQueryable<T> HierarchyFrom<T>(this IDocumentSession session, Func<Ancestor, bool> predicate) where T : IPageModel {
            var page = session.Query<Ancestor>("PageModelWithParentsAndChildren")
                .Include(x => x.Ancestors)
                .Include(x => x.Children)
                .Where(predicate)
                .SingleOrDefault();
            if (page == null) {
                return session.Query<T>();
            }
            var ids = new List<string> { page.Id };
            ids.AddRange(page.Children);
            foreach (var ancestor in page.Ancestors.Where(ancestor => ancestor.Children != null)) {
                if (!ids.Contains(ancestor.Id)) {
                    ids.Add(ancestor.Id);
                }
                foreach (var child in ancestor.Children.Where(child => !ids.Contains(child))) {
                    ids.Add(child);
                }
            }

            return session.Query<T>()
                .Customize(x => x.WaitForNonStaleResultsAsOfLastWrite())
                .Where(x => x.Id.In(ids));
        }

    }
}
