using System;
using BrickPile.Domain.Models;

namespace BrickPile.Core {
    /// <summary>
    /// 
    /// </summary>
    public class PageModelEventArgs : EventArgs {
        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>
        /// The model.
        /// </value>
        public IPageModel Model { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="PageModelEventArgs"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public PageModelEventArgs(IPageModel model) {
            this.Model = model;
        }
    }
}