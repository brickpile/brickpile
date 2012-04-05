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
using System.Collections.Generic;
using BrickPile.Domain.Models;

namespace BrickPile.UI {
    /// <summary>
    /// Represents the <see cref="StructureInfo" /> class
    /// </summary>
    [Obsolete("Not used anymore", false)]
    public interface IStructureInfo {
        /// <summary>
        /// <see cref="StructureInfo.RootModel"/>
        /// </summary>
        IPageModel RootModel { get; }
        /// <summary>
        ///   <see cref="StructureInfo.CurrentModel"/>
        /// </summary>
        IPageModel CurrentModel { get; }
        /// <summary>
        /// Gets the parent model.
        /// </summary>
        IPageModel ParentModel { get; }
        /// <summary>
        /// Gets or sets the hierarchy.
        /// </summary>
        /// <value>
        /// The hierarchy.
        /// </value>
        IEnumerable<IHierarchyNode<IPageModel>> Hierarchy { get; set; }
    }
}