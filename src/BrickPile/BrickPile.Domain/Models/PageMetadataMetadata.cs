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
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BrickPile.Domain.Models {
    internal class PageMetadataMetadata {
        /// <summary>
        /// Get/Sets the Name of the PageMetaData
        /// </summary>
        /// <value></value>
        [Display(Name = "Page name", Order = 10, Prompt = "My awesome page")]
        [Required(ErrorMessage = "Name_Required")]
        public virtual string Name { get; set; }
        /// <summary>
        /// Gets or sets the start publish.
        /// </summary>
        /// <value>
        /// The start publish.
        /// </value>
        [Display]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/ddThh:mm:ss}")]
        public virtual DateTime? StartPublish { get; set; }
        /// <summary>
        /// Get/Sets the Slug of the PageMetaData
        /// </summary>
        /// <value></value>
        //[Display(Name = "Slug",Order = 20)]
        //[ScaffoldColumn(false)]
        [HiddenInput(DisplayValue = false)]
        [UIHint("Slug")]
        public virtual string Slug { get; set; }
        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        [HiddenInput(DisplayValue = false)]
        public virtual string Url { get; set; }
        /// <summary>
        /// Gets or sets the sort order.
        /// </summary>
        /// <value>
        /// The sort order.
        /// </value>
        [ScaffoldColumn(false)]
        public virtual int SortOrder { get; set; }
    }
}
