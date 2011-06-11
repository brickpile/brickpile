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

namespace BrickPile.Domain.Models {
    /// <summary>
    /// Used as reference between documents in RavenDB
    /// </summary>
    /// <remarks></remarks>
    /// <example></example>
    public class DocumentReference<T> : IDocumentReference where T : IPageModel {
        /// <summary>
        /// Get/Sets the Id of the DocumentReference
        /// </summary>
        /// <value></value>
        public string Id { get; set; }
        /// <summary>
        /// Get/Sets the Slug of the DocumentReference
        /// </summary>
        /// <value></value>
        public string Slug { get; set; }
        /// <summary>
        /// Implicitly converts a page model to a DocumentReference
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static implicit operator DocumentReference<T>(T document) {
            return new DocumentReference<T>
                       {
                           Id = document.Id,
                           Slug = document.Metadata.Slug
                       };
        }
    }
}