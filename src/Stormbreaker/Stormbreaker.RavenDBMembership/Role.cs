using System;

namespace Stormbreaker.RavenDBMembership
{
	public class Role
	{
		private string _id;

		public string Id 
		{
			get
			{
				if (String.IsNullOrEmpty(this._id))
				{
					this._id = GenerateId();
				}
				return this._id;
			}
			set { this._id = value; }
		}

		public string ApplicationName { get; set; }
		public string Name { get; set; }
		public string ParentRoleId { get; set; }

		public Role(string name, Role parentRole)
		{
			this.Name = name;
			if (parentRole != null)
			{
				this.ParentRoleId = parentRole.Id;
			}
		}

		private string GenerateId()
		{
			if (!String.IsNullOrEmpty(this.ParentRoleId))
			{
				return this.ParentRoleId + "/" + this.Name;
			}
			else
			{
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
