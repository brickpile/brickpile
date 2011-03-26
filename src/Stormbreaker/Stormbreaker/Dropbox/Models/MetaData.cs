using System;
using System.Collections.Generic;

namespace Stormbreaker.Dropbox.Models {
    public class MetaData {
        /// <summary>
        /// Gets or sets the hash.
        /// </summary>
        /// <value>
        /// The hash.
        /// </value>
        public string Hash { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [thumb_ exists].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [thumb_ exists]; otherwise, <c>false</c>.
        /// </value>
        public bool Thumb_Exists { get; set; }
        /// <summary>
        /// Gets or sets the bytes.
        /// </summary>
        /// <value>
        /// The bytes.
        /// </value>
        public long Bytes { get; set; }
        /// <summary>
        /// Gets or sets the modified.
        /// </summary>
        /// <value>
        /// The modified.
        /// </value>
        public DateTime Modified { get; set; }
        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        public string Path { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [is_ dir].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is_ dir]; otherwise, <c>false</c>.
        /// </value>
        public bool Is_Dir { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [is_ deleted].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is_ deleted]; otherwise, <c>false</c>.
        /// </value>
        public bool Is_Deleted { get; set; }
        /// <summary>
        /// Gets or sets the size.
        /// </summary>
        /// <value>
        /// The size.
        /// </value>
        public string Size { get; set; }
        /// <summary>
        /// Gets or sets the root.
        /// </summary>
        /// <value>
        /// The root.
        /// </value>
        public string Root { get; set; }
        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        /// <value>
        /// The icon.
        /// </value>
        public string Icon { get; set; }
        /// <summary>
        /// Gets or sets the contents.
        /// </summary>
        /// <value>
        /// The contents.
        /// </value>
        public List<MetaData> Contents { get; set; }
        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name {
            get {
                if (Path.LastIndexOf("/") == -1)
                {
                    return string.Empty;
                }
                return string.IsNullOrEmpty(Path) ? "root" : Path.Substring(Path.LastIndexOf("/") + 1);
            }
        }
        /// <summary>
        /// Gets the extension.
        /// </summary>
        public string Extension {
            get {
                if (Path.LastIndexOf(".") == -1)
                {
                    return string.Empty;
                }
                return Is_Dir ? string.Empty : Path.Substring(Path.LastIndexOf("."));
            }
        }
    }

}
