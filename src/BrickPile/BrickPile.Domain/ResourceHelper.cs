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
using System.Reflection;

namespace BrickPile.Domain {
    /// <summary>
    /// 
    /// </summary>
    public class ResourceHelper {
        /// <summary>
        /// Gets the resource lookup.
        /// </summary>
        /// <param name="resourceType">Type of the resource.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <returns></returns>
        public static string GetResourceLookup(Type resourceType, string resourceName) {
            if ((resourceType != null) && (resourceName != null)) {
                PropertyInfo property = resourceType.GetProperty(resourceName, BindingFlags.Public | BindingFlags.Static);
                if (property == null) {
                    throw new InvalidOperationException(string.Format("Resource Type Does Not Have Property"));
                }
                if (property.PropertyType != typeof(string)) {
                    throw new InvalidOperationException(string.Format("Resource Property is Not String Type"));
                }
                return (string) property.GetValue(null, null);
            }
            return null;
        }
    }
}