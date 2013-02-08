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
using BrickPile.Domain;
using BrickPile.Domain.Models;
using BrickPile.Sample.Controllers;
using BrickPile.UI.Web.ViewModels;

namespace BrickPile.Sample.Models {
    /// <summary>
    /// 
    /// </summary>
    [ContentType(
        Name = "Article",
        ControllerType = typeof(PageController))]
    public class Page : IContent {

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        [ScaffoldColumn(false)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the heading.
        /// </summary>
        /// <value>
        /// The heading.
        /// </value>
        //[Required]
        [Display(
            Order = 100,
            Prompt = "Enter a descriptive heading")]
        //Required(ErrorMessage = "Great thanks!")]
        public string Heading { get; set; }

        /// <summary>
        /// Gets or sets the main body.
        /// </summary>
        /// <value>
        /// The main body.
        /// </value>
        //[Required(ErrorMessage = "What know then?")]
        [DataType(DataType.Html)]
        public string MainBody { get; set; }

        /// <summary>
        /// Gets or sets the page reference.
        /// </summary>
        /// <value>
        /// The page reference.
        /// </value>
        [Display(
            Name = "Page",
            Prompt = "Specify page name...")]
        public PageReference PageReference { get; set; }

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>
        /// The image.
        /// </value>
        [Display(
            Name = "An image",
            Prompt = "Specify an alternative text for your image",
            Order = 1)]
        //[Required(ErrorMessage = "{0} cannot be empty bitch!")]
        public Image Image { get; set; }
    }
}