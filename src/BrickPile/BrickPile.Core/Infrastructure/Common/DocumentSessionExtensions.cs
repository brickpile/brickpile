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
using System.Collections.Generic;
using System.Linq;
using BrickPile.Core.Infrastructure.Indexes;
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
        public static IQueryable<T> HierarchyFrom<T>(this IDocumentSession session, Func<PageModel, bool> predicate) where T : IPageModel {

            var page = session.Query<PageModel, PageModelWithParentsAndChildren>()
                .Include(x => x.Ancestors)
                .Include(x => x.Children)
                .Where(predicate)
                .SingleOrDefault();

            if (page == null) {
                return null;
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

            return session.Load<T>(ids).AsQueryable();
        }
    }
}
