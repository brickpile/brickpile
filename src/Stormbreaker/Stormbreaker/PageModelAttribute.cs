using System;

namespace Stormbreaker {
    /// <summary>
    /// Used to decorate a page model
    /// </summary>
    /// <remarks></remarks>
    /// <example></example>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class PageModelAttribute : Attribute {
        /// <summary>
        /// Get/Sets the Name of the PageModelAttribute
        /// </summary>
        /// <value></value>
        public string Name { get; set; }
        /// <summary>
        /// Get/Sets the Description of the PageModelAttribute
        /// </summary>
        /// <value></value>
        public string Description { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="PageModelAttribute" /> class.
        /// </summary>
        /// <param name="name">The name of the page model</param>
        public PageModelAttribute(string name) : this(name, string.Empty) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="PageModelAttribute" /> class.
        /// </summary>
        /// <param name="name">The name of the page model</param>
        /// <param name="description">The description of the page model</param>
        public PageModelAttribute(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}