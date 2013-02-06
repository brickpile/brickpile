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
using System.ComponentModel.DataAnnotations;

namespace BrickPile.Domain.Models {
    /// <summary>
    /// The base class for page models
    /// </summary>
    /// <remarks>Use IPageModel to create your own base class</remarks>
    /// <example></example>
    [MetadataType(typeof(PageModelMetadata))]
    public sealed class PageModel : IPageModel {

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// Gets the meta data.
        /// </summary>
        public PageMetadata Metadata { get; private set; }

        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        public DocumentReference<IPageModel> Parent { get; set; }

        /// <summary>
        /// Gets or sets the ancestors.
        /// </summary>
        /// <value>
        /// The ancestors.
        /// </value>
        internal IList<PageModel> Ancestors { get; set; }

        /// <summary>
        /// Gets or sets the children.
        /// </summary>
        /// <value>
        /// The children.
        /// </value>
        public List<string> Children { get; private set; }

        /// <summary>
        /// Gets or sets the content reference.
        /// </summary>
        /// <value>
        /// The content reference.
        /// </value>
        [ScaffoldColumn(false)]
        public string ContentReference { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PageModel"/> class.
        /// </summary>
        public PageModel() {
            Metadata = new PageMetadata();
            Children = new List<string>();
        }
    }
}