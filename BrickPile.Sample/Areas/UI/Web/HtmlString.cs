using System.Web;

namespace BrickPile.UI.Web {
    /// <summary>
    /// 
    /// </summary>
    public class HtmlString: IHtmlString {
        /// <summary>
        /// Gets or sets the HTML.
        /// </summary>
        /// <value>
        /// The HTML.
        /// </value>
        public string Html { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlString"/> class.
        /// </summary>
        public HtmlString() :this(string.Empty) {}
        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlString"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public HtmlString(string value) { 
            this.Html = value;
        }
        /// <summary>
        /// Returns an HTML-encoded string.
        /// </summary>
        /// <returns>
        /// An HTML-encoded string.
        /// </returns>
        public string ToHtmlString() {
            return this.Html; 
        }
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString() {
            return this.Html; 
        }
    }
}