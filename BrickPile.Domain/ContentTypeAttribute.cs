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