using System;
using System.Collections.Generic;
using Stormbreaker.Models;

namespace Stormbreaker.Repositories {
    public interface IPageRepository : IRepository<IPageModel> {
        /// <summary>
        /// Childrens the specified parent.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent">The parent.</param>
        /// <returns></returns>
        IEnumerable<T> Children<T>(T parent) where T : IPageModel;
        /// <summary>
        /// Bies the URL.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        T ByUrl<T>(string url) where T : IPageModel;
    }
}