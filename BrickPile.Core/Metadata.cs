using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BrickPile.Core
{
    /// <summary>
    ///     Represents the <see cref="Metadata" /> of a BrickPile page.
    /// </summary>
    public class Metadata
    {
        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        [Display(Name = "Name", Order = 10, Prompt = "My awesome page"),
         Required(ErrorMessage = "You need to specify a name for the page.")]
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the URL.
        /// </summary>
        /// <value>
        ///     The URL.
        /// </value>
        [HiddenInput(DisplayValue = false)]
        public string Url { get; set; }

        /// <summary>
        ///     Gets or sets the slug.
        /// </summary>
        /// <value>
        ///     The slug.
        /// </value>
        [HiddenInput(DisplayValue = false), UIHint("Slug"), Display(Prompt = "my-awesome-page")]
        public string Slug { get; set; }

        /// <summary>
        ///     Gets or sets the published.
        /// </summary>
        /// <value>
        ///     The published.
        /// </value>
        [Display(Name = "Published", Order = 60)]
        public DateTime? Published { get; set; }

        /// <summary>
        ///     Gets a value indicating whether [is published].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [is published]; otherwise, <c>false</c>.
        /// </value>
        //[Raven.Imports.Newtonsoft.Json.JsonIgnore, ScaffoldColumn(false)]
        //[ScaffoldColumn(false)]
        [HiddenInput(DisplayValue = false)]
        public virtual bool IsPublished { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [display in menu].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [display in menu]; otherwise, <c>false</c>.
        /// </value>
        //[UIHint("Boolean")]
        [Display(Name = "Display in menu", Order = 50)]
        public bool DisplayInMenu { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether [is deleted].
        /// </summary>
        /// <value>
        ///     <c>true</c> if [is deleted]; otherwise, <c>false</c>.
        /// </value>
        [ScaffoldColumn(false)]
        public bool IsDeleted { get; set; }

        /// <summary>
        ///     Gets or sets the sort order.
        /// </summary>
        /// <value>
        ///     The sort order.
        /// </value>
        [ScaffoldColumn(false)]
        public virtual int SortOrder { get; set; }

        /// <summary>
        ///     Gets or sets the title.
        /// </summary>
        /// <value>
        ///     The title.
        /// </value>
        [Display(Name = "Title",
            Description =
                "Sidans titel, ange 10 - 12 ord men inte mer än 70 tecken inkl. blanksteg. Inkludera gärna nyckelord som kan assosieras med innehållet.",
            Order = 20)]
        public virtual string Title { get; set; }

        /// <summary>
        ///     Gets or sets the changed.
        /// </summary>
        /// <value>
        ///     The changed.
        /// </value>
        [ScaffoldColumn(false)]
        public virtual DateTime? Changed { get; set; }

        /// <summary>
        ///     Gets or sets the changed by.
        /// </summary>
        /// <value>
        ///     The changed by.
        /// </value>
        [ScaffoldColumn(false)]
        public virtual string ChangedBy { get; set; }

        /// <summary>
        ///     Gets or sets the description.
        /// </summary>
        /// <value>
        ///     The description.
        /// </value>
        [Display(Name = "Description",
            Description = "Beskrivning av sidan, ange 25 - 30 ord men inte mer än 180 tecken inkl. blanksteg.",
            Order = 40), Editable(false)]
        public virtual string Description { get; set; }

        /// <summary>
        ///     Gets or sets the keywords.
        /// </summary>
        /// <value>
        ///     The keywords.
        /// </value>
        [Display(Name = "Keywords",
            Description =
                "Använd nyckelord och fraser från sidans titel, meta beskrivning, sidans titel och från dom första styckena av synlig innehåll. Överskrid inte 15 - 20 ord om möjligt.",
            Order = 30)]
        public virtual string Keywords { get; set; }
    }
}