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
using BrickPile.UI.Models;
using BrickPile.UI.Web;

namespace BrickPile.Sample.Models {
    /// <summary>
    /// 
    /// </summary>
    [PageType(Name = "Home page", ControllerType = typeof(HomeController))]
    public class Home : PageModel {
        /// <summary>
        /// Gets or sets the heading.
        /// </summary>
        /// <value>
        /// The heading.
        /// </value>
        //[Required]
        [Display(Order = 100, Prompt = "Enter a descriptive heading")]
        public virtual string Heading { get; set; }
        /// <summary>
        /// Gets or sets the main intro.
        /// </summary>
        /// <value>
        /// The main intro.
        /// </value>
        [DataType(DataType.MultilineText)]
        [Display(Name = "Introduction",Order = 200)]
        public string MainIntro { get; set; }
        /// <summary>
        /// Gets or sets the quote link.
        /// </summary>
        /// <value>
        /// The quote link.
        /// </value>
        [Display(Name = "Get a quote link", Order = 300, Prompt = "Specify page name...")]
        public PageReference QuoteLink { get; set; }
        /// <summary>
        /// Gets or sets the main body.
        /// </summary>
        /// <value>
        /// The main body.
        /// </value>
        [Display(Name = "Why", Order = 400)]
        public HtmlString MainBody { get; set; }
        /// <summary>
        /// Gets or sets the portfolio.
        /// </summary>
        /// <value>
        /// The portfolio.
        /// </value>
        [Display(Order = 500)]
        [DataType(DataType.Html)]
        public HtmlString Portfolio { get; set; }
        /// <summary>
        /// Gets or sets the weblog.
        /// </summary>
        /// <value>
        /// The weblog.
        /// </value>
        [Display(Order = 600)]
        [DataType(DataType.Html)]
        public HtmlString Weblog { get; set; }
        /// <summary>
        /// Gets or sets the contact.
        /// </summary>
        /// <value>
        /// The contact.
        /// </value>
        [Display(Order = 700)]
        [DataType(DataType.Html)]
        public HtmlString Contact { get; set; }
        /// <summary>
        /// Gets or sets the about us.
        /// </summary>
        /// <value>
        /// The about us.
        /// </value>
        [Display(Name = "About us",Order = 800)]
        [DataType(DataType.Html)]
        public HtmlString AboutUs { get; set; }
    }

}