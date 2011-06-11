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

namespace BrickPile.Domain.Models {
	public class Role {
		private string _id;

		public string Id  {
			get {
				if (String.IsNullOrEmpty(this._id)) {
					this._id = GenerateId();
				}
				return this._id;
			}
			set { this._id = value; }
		}

		public string ApplicationName { get; set; }
		public string Name { get; set; }
		public string ParentRoleId { get; set; }

		public Role(string name, Role parentRole) {
			this.Name = name;
			if (parentRole != null)
			{
				this.ParentRoleId = parentRole.Id;
			}
		}

		private string GenerateId() {
			if (!String.IsNullOrEmpty(this.ParentRoleId)) {
				return this.ParentRoleId + "/" + this.Name;
			}
			else {
				var defaultNameSpace = "raven/authorization/roles/";
				// Also use application name for ID generation so we can have multiple roles with the same name.
				if (!String.IsNullOrEmpty(this.ApplicationName))
				{
					return defaultNameSpace + this.ApplicationName.Replace("/", String.Empty) + "/" + this.Name;
				}
				else
				{
					return defaultNameSpace + this.Name;
				}
			}
		}
	}
}
