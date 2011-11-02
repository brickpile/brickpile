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
    public class PageModelAttribute : Attribute {
        /// <summary>
        /// Get/Sets the Name of the PageModelAttribute
        /// </summary>
        /// <value></value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the type of the controller.
        /// </summary>
        /// <value>
        /// The type of the controller.
        /// </value>
        public Type ControllerType { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="PageModelAttribute"/> class.
        /// </summary>
        public PageModelAttribute() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="PageModelAttribute"/> class.
        /// </summary>
        /// <param name="name">The name of the page model</param>
        /// <param name="controllerType">Type of the controller.</param>
        public PageModelAttribute(string name, Type controllerType)
        {
            Name = name;
            ControllerType = controllerType;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="PageModelAttribute"/> class.
        /// </summary>
        /// <param name="name">The name of the page model</param>
        /// <param name="controllerType">Type of the controller.</param>
        public PageModelAttribute(string name)
        {
            Name = name;
        }

    }
}