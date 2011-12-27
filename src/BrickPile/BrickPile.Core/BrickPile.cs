using System;

namespace BrickPile.Core {
    /// <summary>
    /// 
    /// </summary>
    public sealed class Application {
        private static volatile Application _instance;
        private static readonly object SyncRoot = new Object();
        public event EventHandler<PageModelEventArgs> SavingPage;
        public event EventHandler<PageModelEventArgs> SavedPage;
        public event EventHandler<PageModelEventArgs> DeletingPage;
        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static Application Instance {
            get {
                if (_instance == null)  {
                    lock (SyncRoot) {
                    if (_instance == null)
                        _instance = new Application();
                    }
                }
                return _instance;
            }
        }
        /// <summary>
        /// Raises the <see cref="E:SavingPage"/> event.
        /// </summary>
        /// <param name="e">The <see cref="BrickPile.Core.PageModelEventArgs"/> instance containing the event data.</param>
        public void OnSavingPage(PageModelEventArgs e) {
            if (SavingPage != null) {
                SavingPage(this, e);
            }
        }
        /// <summary>
        /// Raises the <see cref="E:SavedPage"/> event.
        /// </summary>
        /// <param name="e">The <see cref="BrickPile.Core.PageModelEventArgs"/> instance containing the event data.</param>
        public void OnSavedPage(PageModelEventArgs e) {
            if (SavedPage != null) {
                SavedPage(this, e);
            }
        }
        /// <summary>
        /// Raises the <see cref="E:DeletingPage"/> event.
        /// </summary>
        /// <param name="e">The <see cref="BrickPile.Core.PageModelEventArgs"/> instance containing the event data.</param>
        public void OnDeletingPage(PageModelEventArgs e) {
            if (DeletingPage != null) {
                DeletingPage(this, e);
            }
        }
    }
}
