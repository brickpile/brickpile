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

using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using BrickPile.Domain;
using BrickPile.Sample.Controllers;
using BrickPile.UI.Models;

namespace BrickPile.Sample.Models {
    /// <summary>
    /// 
    /// </summary>
    [PageType(Name = "Article", ControllerType = typeof(PageController))]
    public class Page : BaseModel {
        /// <summary>
        /// Gets or sets the nisse.
        /// </summary>
        /// <value>
        /// The nisse.
        /// </value>
        [Display(Name = "Page", Prompt = "Specify page name...")]
        public PageReference PageReference { get; set; }
        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>
        /// The image.
        /// </value>
        [Required(ErrorMessage = "Knark ...")]
        public Image Image { get; set; }
    }
}