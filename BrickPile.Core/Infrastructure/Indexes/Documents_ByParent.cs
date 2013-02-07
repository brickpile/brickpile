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

using System.Linq;
using BrickPile.Domain.Models;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace BrickPile.Core.Infrastructure.Indexes {
    /// <summary>
    /// Used for registrations of an index in RavenDB
    /// </summary>
    /// <remarks></remarks>
    /// <example></example>
    public class DocumentsByParent :  AbstractIndexCreationTask<IPageModel> {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentsByParent"/> class.
        /// </summary>
        public DocumentsByParent() {

            Map = pages => from page in pages
                           select new
                           {
                               Parent_Id = page.Parent.Id,
                               Metadata_IsDeleted = page.Metadata.IsDeleted,
                               Metadata_SortOrder = page.Metadata.SortOrder
                           };

            Sort(x => x.Metadata.SortOrder, SortOptions.Int);
        }
    }
}