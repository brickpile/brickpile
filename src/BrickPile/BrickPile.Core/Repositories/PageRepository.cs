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
using BrickPile.Domain.Models;
using Raven.Client;

namespace BrickPile.Core.Repositories {
    /// <summary>
    /// Page repository implementation of <see cref="IPageRepository" /> that provides support for basic operations for objects implementing <see cref="IPageModel" /> interface.
    /// </summary>
    /// <remarks></remarks>
    /// <example></example>
    public class PageRepository : Repository<IPageModel>, IPageRepository {
        private readonly IDocumentSession _documentSession;
        /// <summary>
        /// Get all children of a specific page
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <returns></returns>
        public IEnumerable<IPageModel> GetChildren(IPageModel parent) {
            return _documentSession.Advanced.LuceneQuery<IPageModel>("Documents/ByParent")
                .Where("Id:" + parent.Id)
                .WaitForNonStaleResultsAsOfNow()
                .ToArray();
        }
        /// <summary>
        /// Get a page by the url
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public T GetPageByUrl<T>(string url) where T : IPageModel {
            return _documentSession.Advanced.LuceneQuery<T>("Document/ByUrl")
                .Where("Url:" + url)
                .WaitForNonStaleResultsAsOfNow()
                .FirstOrDefault();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="PageRepository" /> class.
        /// </summary>
        /// <param name="documentSession"></param>
        public PageRepository(IDocumentSession documentSession) : base(documentSession) {
            _documentSession = documentSession;
        }
    }
}