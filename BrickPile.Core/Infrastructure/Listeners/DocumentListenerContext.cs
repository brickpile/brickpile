using System;
using Raven.Client.Util;

namespace BrickPile.Core.Infrastructure.Listeners
{
    /// <summary>
    /// </summary>
    internal static class DocumentListenerContext
    {
        [ThreadStatic] private static bool currentlyInContext;

        /// <summary>
        ///     Gets a value indicating whether [is in document listener context].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [is in document listener context]; otherwise, <c>false</c>.
        /// </value>
        public static bool IsInDocumentListenerContext
        {
            get { return currentlyInContext; }
        }

        /// <summary>
        ///     Enters this instance.
        /// </summary>
        /// <returns></returns>
        public static IDisposable Enter()
        {
            var old = currentlyInContext;
            currentlyInContext = true;
            return new DisposableAction(() => currentlyInContext = old);
        }
    }
}