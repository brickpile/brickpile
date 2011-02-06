using System;
using Raven.Database.Data;

namespace Stormbreaker.Raven {
    public static class TriggerContext {

        [ThreadStatic]
        private static bool _currentlyInContext;

        public static bool IsInTriggerContext
        {
            get
            {
                return _currentlyInContext;
            }
        }

        public static IDisposable Enter()
        {
            var old = _currentlyInContext;
            _currentlyInContext = true;
            return new DisposableAction(() => _currentlyInContext = old);
        }
    }
}