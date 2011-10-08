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
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BrickPile.Domain.Models {
    public class PageMetadataMetadata {
        /// <summary>
        /// Get/Sets the Name of the PageMetaData
        /// </summary>
        /// <value></value>
        [Display(Name = "Name", Order = 10, Prompt = "My awesome page")]
        [Required(ErrorMessage = "Name_Required")]
        public virtual string Name { get; set; }
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [Display(Name = "Title", Description = "Sidans titel, ange 10 - 12 ord men inte mer än 70 tecken inkl. blanksteg. Inkludera gärna nyckelord som kan assosieras med innehållet.", Order = 20)]
        public virtual string Title { get; set; }
        /// <summary>
        /// Gets or sets the keywords.
        /// </summary>
        /// <value>
        /// The keywords.
        /// </value>
        [Display(Name = "Keywords", Description = "Använd nyckelord och fraser från sidans titel, meta beskrivning, sidans titel och från dom första styckena av synlig innehåll. Överskrid inte 15 - 20 ord om möjligt.", Order = 30)]
        public virtual string Keywords { get; set; }
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [Display(Name = "Description", Description = "Beskrivning av sidan, ange 25 - 30 ord men inte mer än 180 tecken inkl. blanksteg.", Order = 40)]
        [Editable(false)]
        public virtual string Description { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [display in menu].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [display in menu]; otherwise, <c>false</c>.
        /// </value>
        [Display(Name = "Display in menu", Order = 50)]
        public virtual bool DisplayInMenu { get; set; }
        /// <summary>
        /// Gets or sets the published.
        /// </summary>
        /// <value>
        /// The published.
        /// </value>
        [Display(Name = "Published", Order = 60)]
        public virtual DateTime? Published { get; set; }
        /// <summary>
        /// Gets or sets the changed.
        /// </summary>
        /// <value>
        /// The changed.
        /// </value>
        [ScaffoldColumn(false)]
        public virtual DateTime? Changed { get; set; }
        /// <summary>
        /// Gets or sets the published by.
        /// </summary>
        /// <value>
        /// The published by.
        /// </value>
        [ScaffoldColumn(false)]
        public virtual string ChangedBy { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is published.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is published; otherwise, <c>false</c>.
        /// </value>
        [ScaffoldColumn(false)]
        public virtual bool IsPublished { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is deleted.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is deleted; otherwise, <c>false</c>.
        /// </value>
        [ScaffoldColumn(false)]
        public virtual bool IsDeleted { get; set; }
        /// <summary>
        /// Get/Sets the Slug of the PageMetaData
        /// </summary>
        /// <value></value>
        //[Display(Name = "Slug",Order = 20)]
        //[ScaffoldColumn(false)]
        [HiddenInput(DisplayValue = false)]
        [UIHint("Slug")]
        [Display(Prompt = "my-awesome-page")]
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
