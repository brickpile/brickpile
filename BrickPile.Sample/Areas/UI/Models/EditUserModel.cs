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

namespace BrickPile.UI.Models {
	public class EditUserModel {
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
		public string Username { get; set; }
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
		public string Email { get; set; }
        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        /// <value>
        /// The roles.
        /// </value>
		public string[] Roles { get; set; }
        /// <summary>
        /// Gets or sets the user roles.
        /// </summary>
        /// <value>
        /// The user roles.
        /// </value>
		public string[] UserRoles { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="EditUserModel"/> class.
        /// </summary>
		public EditUserModel() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="EditUserModel"/> class.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="email">The email.</param>
        /// <param name="roles">The roles.</param>
        /// <param name="userRoles">The user roles.</param>
		public EditUserModel(string username, string email, string[] roles, string[] userRoles) {
			this.Username = username;
			this.Email = email;
			this.Roles = roles;
			this.UserRoles = userRoles;
		}
	}
}
