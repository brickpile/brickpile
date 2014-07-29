using System;

namespace BrickPile.Domain {
    /// <summary>
    /// Used to decorate a page model
    /// </summary>
    /// <remarks></remarks>
    /// <example></example>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ContentTypeAttribute : Attribute {
        private string _name;
        /// <summary>
        /// Get/Sets the Name of the PageModelAttribute
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name {
            get {
                return ResourceType == null ? _name : ResourceHelper.GetResourceLookup(ResourceType, _name);
            }
            set { _name = value; }
        }
        /// <summary>
        /// Gets or sets the type of the controller.
        /// </summary>
        /// <value>
        /// The type of the controller.
        /// </value>
        public Type ControllerType { get; set; }
        /// <summary>
        /// Gets or sets the type of the resource.
        /// </summary>
        /// <value>
        /// The type of the resource.
        /// </value>
        public Type ResourceType { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentTypeAttribute"/> class.
        /// </summary>
        public ContentTypeAttribute() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentTypeAttribute"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="controllerType">Type of the controller.</param>
        public ContentTypeAttribute(string name, Type controllerType) {
            Name = name;
            ControllerType = controllerType;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ContentTypeAttribute"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="controllerType">Type of the controller.</param>
        /// <param name="resourceType">Type of the resource.</param>
        public ContentTypeAttribute(string name, Type controllerType, Type resourceType) {
            Name = name;
            ControllerType = controllerType;
            ResourceType = resourceType;
        }
    }
}