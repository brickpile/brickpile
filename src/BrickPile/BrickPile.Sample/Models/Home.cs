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
using BrickPile.Sample.Controllers;
using BrickPile.UI.Models;

namespace BrickPile.Sample.Models {
    /// <summary>
    /// 
    /// </summary>
    [PageModel(Name = "Home page", ControllerType = typeof(HomeController))]
    public class Home : BaseEditorial {
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
        [Display(Name = "Get a quote link", Order = 300)]
        public ModelReference QuoteLink { get; set; }
        /// <summary>
        /// Gets or sets the main body.
        /// </summary>
        /// <value>
        /// The main body.
        /// </value>
        [Display(Name = "Why", Order = 400)]
        public override string MainBody {
            get { 
                return base.MainBody;
            }
            set {
                base.MainBody = value;
            }
        }
        /// <summary>
        /// Gets or sets the portfolio.
        /// </summary>
        /// <value>
        /// The portfolio.
        /// </value>
        [Display(Order = 500)]
        [DataType(DataType.Html)]
        public string Portfolio { get; set; }
        /// <summary>
        /// Gets or sets the weblog.
        /// </summary>
        /// <value>
        /// The weblog.
        /// </value>
        [Display(Order = 600)]
        [DataType(DataType.Html)]
        public string Weblog { get; set; }
        /// <summary>
        /// Gets or sets the contact.
        /// </summary>
        /// <value>
        /// The contact.
        /// </value>
        [Display(Order = 700)]
        [DataType(DataType.Html)]
        public string Contact { get; set; }
        /// <summary>
        /// Gets or sets the about us.
        /// </summary>
        /// <value>
        /// The about us.
        /// </value>
        [Display(Name = "About us",Order = 800)]
        [DataType(DataType.Html)]
        public string AboutUs { get; set; }
    }

}