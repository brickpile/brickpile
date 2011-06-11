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

using System.ComponentModel.DataAnnotations;

namespace BrickPile.UI.Models {
    /// <summary>
    /// Responsible for some basic settings
    /// </summary>
    public class Settings : ISettings {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        [ScaffoldColumn(false)]
        public string Id { get; set; }
        /// <summary>
        /// Gets or sets the API key.
        /// </summary>
        /// <value>
        /// The API key.
        /// </value>
        public string AwsAccessKey { get; set; }
        /// <summary>
        /// Gets or sets the aws secret access key.
        /// </summary>
        /// <value>
        /// The aws secret access key.
        /// </value>
        public string AwsSecretAccessKey { get; set; }
        /// <summary>
        /// Gets or sets the name of the bucket.
        /// </summary>
        /// <value>
        /// The name of the bucket.
        /// </value>
        public string BucketName { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="Settings"/> class.
        /// </summary>
        public Settings() {
            Id = "brickpile/settings";
        }
    }
}
