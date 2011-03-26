using System;
using System.Collections.Generic;
using System.Linq;

namespace Stormbreaker.Repositories {
    /// <summary>
    /// Represents a common repository 
    /// </summary>
    public interface IRepository<T> {
        /// <summary>
        /// Singles the or default.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        T SingleOrDefault<T>(Func<T, bool> predicate);
        /// <summary>
        /// Lists this instance.
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> List<T>();
        /// <summary>
        /// <see cref="PageRepository.Store" />
        /// </summary>
        void Store(T entity);
        /// <summary>
        /// <see cref="PageRepository.Delete" />
        /// </summary>
        void Delete(T entity);
        /// <summary>
        /// <see cref="PageRepository.SaveChanges" />
        /// </summary>
        void SaveChanges();
    }
}