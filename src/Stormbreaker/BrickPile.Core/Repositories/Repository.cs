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
using System.Linq.Expressions;
using Raven.Client;

namespace BrickPile.Core.Repositories {
    public class Repository<T> : IRepository<T> {
        private readonly IDocumentSession _documentSession;
        /// <summary>
        /// Singles the or default.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public T SingleOrDefault<T>(Expression<Func<T, bool>> predicate) {
            return _documentSession.Query<T>().SingleOrDefault(predicate);
        }
        /// <summary>
        /// Loads the specified ids.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ids">The ids.</param>
        /// <returns></returns>
        public T[] Load<T>(string[] ids) {
            return _documentSession.Load<T>(ids);
        }
        /// <summary>
        /// Lists this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> List<T>() {
            return _documentSession.Query<T>();
        }
        /// <summary>
        /// Stores the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Store(T entity) {
            _documentSession.Store(entity);
        }
        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Delete(T entity) {
            _documentSession.Delete(entity);
        }
        /// <summary>
        /// Saves the changes.
        /// </summary>
        public void SaveChanges() {
            _documentSession.SaveChanges();
        }
        /// <summary>
        /// Refreshes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Refresh(T entity) {
            _documentSession.Advanced.Refresh(entity);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Repository&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="documentSession">The document session.</param>
        public Repository(IDocumentSession documentSession) {
            _documentSession = documentSession;
        }
    }
}
