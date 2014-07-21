using System;

namespace BrickPile.Core.Infrastructure.Listeners
{
    internal static class DocumentListenerContext
    {
        [ThreadStatic]
        private static bool _currentlyInContext;

        /// <summary>
        /// Gets a value indicating whether [is in document listener context].
        /// </summary>
        /// <value>
        /// <c>true</c> if [is in document listener context]; otherwise, <c>false</c>.
        /// </value>
        public static bool IsInDocumentListenerContext
        {
            get
            {
                return _currentlyInContext;
            }
        }

        /// <summary>
        /// Enters this instance.
        /// </summary>
        /// <returns></returns>
        public static IDisposable Enter()
        {
            var old = _currentlyInContext;
            _currentlyInContext = true;
            return new Raven.Abstractions.Extensions.DisposableAction(() => _currentlyInContext = old);
        }
    }
}