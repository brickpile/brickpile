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
using System.Globalization;
using System.Resources;

namespace BrickPile.UI.Common {
    public static class ResourceHelper {
        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <param name="resourceType">Type of the resource.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <returns></returns>
        public static string GetString(Type resourceType, string resourceName) {
            return new ResourceManager(resourceType.FullName, resourceType.Assembly).GetString(resourceName);
        }
        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <param name="resourceType">Type of the resource.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        public static string GetString(Type resourceType, string resourceName, CultureInfo culture) {
            return new ResourceManager(resourceType.FullName, resourceType.Assembly).GetString(resourceName, culture);
        }
        /// <summary>
        /// Gets the object.
        /// </summary>
        /// <param name="resourceType">Type of the resource.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <returns></returns>
        public static object GetObject(Type resourceType, string resourceName) {
            return new ResourceManager(resourceType.FullName, resourceType.Assembly).GetObject(resourceName);
        }
        /// <summary>
        /// Gets the object.
        /// </summary>
        /// <param name="resourceType">Type of the resource.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        public static object GetObject(Type resourceType, string resourceName, CultureInfo culture) {
            return new ResourceManager(resourceType.FullName, resourceType.Assembly).GetObject(resourceName, culture);
        }
    }

}