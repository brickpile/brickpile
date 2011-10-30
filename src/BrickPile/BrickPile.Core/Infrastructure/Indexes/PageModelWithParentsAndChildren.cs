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
using Raven.Client.Indexes;

namespace BrickPile.Core.Infrastructure.Indexes {
    public class PageModelWithParentsAndChildren : AbstractIndexCreationTask<Ancestor> {
        /// <summary>
        /// Initializes a new instance of the <see cref="PageModelWithParentsAndChildren"/> class.
        /// </summary>
        public PageModelWithParentsAndChildren() {

            Map = pages => from page in pages
                           select new {
                               page.Id,
                               page.Children,
                               page.Metadata.DisplayInMenu,
                               page.Metadata.IsDeleted,
                               page.Metadata.IsPublished,
                               page.Metadata.Name,
                               page.Metadata.Published
                           };

            TransformResults = (database, pages) => from page in pages
                                                    let ancestors = Recurse(page, c => database.Load<Ancestor>(c.Parent.Id))
                                                    select new
                                                    {
                                                        page.Id,
                                                        page.Children,
                                                        page.Metadata.DisplayInMenu,
                                                        page.Metadata.IsDeleted,
                                                        page.Metadata.IsPublished,
                                                        page.Metadata.Name,
                                                        page.Metadata.Published,
                                                        Ancestors =
                                                        (
                                                           from ancestor in ancestors
                                                           select new
                                                           {
                                                               ancestor.Id,
                                                               ancestor.Children,
                                                               ancestor.Metadata.DisplayInMenu,
                                                               ancestor.Metadata.IsDeleted,
                                                               ancestor.Metadata.IsPublished,
                                                               ancestor.Metadata.Name,
                                                               ancestor.Metadata.Published
                                                           })
                                                    };
        }
    }
}
