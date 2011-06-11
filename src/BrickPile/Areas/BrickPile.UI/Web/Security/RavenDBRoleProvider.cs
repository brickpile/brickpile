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
using System.Linq;
using System.Collections.Specialized;
using System.Web.Security;
using BrickPile.Domain.Models;
using Raven.Client;
using StructureMap;

namespace BrickPile.UI.Web.Security {
	public class RavenDBRoleProvider : RoleProvider {
		private const string ProviderName = "RavenDBRole";
		private IDocumentStore _documentStore;
        /// <summary>
        /// Gets or sets the document store.
        /// </summary>
        /// <value>
        /// The document store.
        /// </value>
		public IDocumentStore DocumentStore
		{
			get
			{
				if (_documentStore == null)
				{
					throw new NullReferenceException("The DocumentStore is not set. Please set the DocumentStore or make sure that the Common Service Locator can find the IDocumentStore and call Initialize on this provider.");
				}
				return this._documentStore;
			}
			set { this._documentStore = value; }
		}
        /// <summary>
        /// Initializes the provider.
        /// </summary>
        /// <param name="name">The friendly name of the provider.</param>
        /// <param name="config">A collection of the name/value pairs representing the provider-specific attributes specified in the configuration for this provider.</param>
        /// <exception cref="T:System.ArgumentNullException">The name of the provider is null.</exception>
        ///   
        /// <exception cref="T:System.ArgumentException">The name of the provider has a length of zero.</exception>
        ///   
        /// <exception cref="T:System.InvalidOperationException">An attempt is made to call <see cref="M:System.Configuration.Provider.ProviderBase.Initialize(System.String,System.Collections.Specialized.NameValueCollection)"/> on a provider after the provider has already been initialized.</exception>
		public override void Initialize(string name, NameValueCollection config)
		{
			// Try to find an IDocumentStore via Common Service Locator. 
			try
			{
				this.DocumentStore = ObjectFactory.GetInstance<IDocumentStore>();
			}
			catch (NullReferenceException) // Swallow Nullreference expection that occurs when there is no current service locator.
			{
			}
			base.Initialize(name, config);
		}
        /// <summary>
        /// Gets or sets the name of the application to store and retrieve role information for.
        /// </summary>
        /// <returns>The name of the application to store and retrieve role information for.</returns>
		public override string ApplicationName { get; set; }
        /// <summary>
        /// Adds the specified user names to the specified roles for the configured applicationName.
        /// </summary>
        /// <param name="usernames">A string array of user names to be added to the specified roles.</param>
        /// <param name="roleNames">A string array of the role names to add the specified user names to.</param>
		public override void AddUsersToRoles(string[] usernames, string[] roleNames)
		{
			if (usernames.Length == 0 || roleNames.Length == 0)
			{
				return;
			}
			using (var session = this.DocumentStore.OpenSession())
			{
				try
				{
					var users = session.Advanced.LuceneQuery<User>().OpenSubclause();
					foreach (var username in usernames)
					{
						users = users.WhereEquals("Username", username, true);
					}
					users = users.CloseSubclause().AndAlso().WhereEquals("ApplicationName", this.ApplicationName, true);

					var usersAsList = users.ToList();
					var roles = session.Advanced.LuceneQuery<Role>().OpenSubclause();
					foreach (var roleName in roleNames)
					{
						roles = roles.WhereEquals("Name", roleName, true);
					}
					roles = roles.CloseSubclause().AndAlso().WhereEquals("ApplicationName", this.ApplicationName);

					var roleIds = roles.Select(r => r.Id).ToList();
					foreach (var roleId in roleIds)
					{
						foreach (var user in usersAsList)
						{
							user.Roles.Add(roleId);
						}
					}
					session.SaveChanges();
				}
				catch (Exception ex)
				{
					// TODO: log exception properly
					Console.WriteLine(ex.ToString());
					throw;
				}
			}
		}
        /// <summary>
        /// Adds a new role to the data source for the configured applicationName.
        /// </summary>
        /// <param name="roleName">The name of the role to create.</param>
		public override void CreateRole(string roleName)
		{
			using (var session = this.DocumentStore.OpenSession())
			{
				try
				{
					var role = new Role(roleName, null);
					role.ApplicationName = this.ApplicationName;

					session.Store(role);
					session.SaveChanges();
				}
				catch (Exception ex)
				{
					// TODO: log exception properly
					Console.WriteLine(ex.ToString());
					throw;
				}
			}
		}
        /// <summary>
        /// Removes a role from the data source for the configured applicationName.
        /// </summary>
        /// <param name="roleName">The name of the role to delete.</param>
        /// <param name="throwOnPopulatedRole">If true, throw an exception if <paramref name="roleName"/> has one or more members and do not delete <paramref name="roleName"/>.</param>
        /// <returns>
        /// true if the role was successfully deleted; otherwise, false.
        /// </returns>
		public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
		{
			using (var session = this.DocumentStore.OpenSession())
			{
				try
				{
					var role = (from r in session.Query<Role>()
							   where r.Name == roleName && r.ApplicationName == this.ApplicationName
							   select r).SingleOrDefault();
					if (role != null)
					{
						// also find users that have this role
						var users = (from u in session.Query<User>()
									where u.Roles.Any(roleId => roleId == role.Id)
									select u).ToList();
						if (users.Any() && throwOnPopulatedRole)
						{
							throw new Exception(String.Format("Role {0} contains members and cannot be deleted.", role.Name));
						}
						foreach (var user in users)
						{
							user.Roles.Remove(role.Id);
						}
						session.Delete(role);
						session.SaveChanges();
						return true;
					}
					return false;
				}
				catch (Exception ex)
				{
					// TODO: log exception properly
					Console.WriteLine(ex.ToString());
					throw;
				}
			}
		}
        /// <summary>
        /// Gets an array of user names in a role where the user name contains the specified user name to match.
        /// </summary>
        /// <param name="roleName">The role to search in.</param>
        /// <param name="usernameToMatch">The user name to search for.</param>
        /// <returns>
        /// A string array containing the names of all the users where the user name matches <paramref name="usernameToMatch"/> and the user is a member of the specified role.
        /// </returns>
		public override string[] FindUsersInRole(string roleName, string usernameToMatch)
		{
			using (var session = this.DocumentStore.OpenSession())
			{
				// Get role first
				var role = (from r in session.Query<Role>()
							where r.Name == roleName && r.ApplicationName == this.ApplicationName
							select r).SingleOrDefault();
				if (role != null)
				{
					// Find users
					var users = from u in session.Query<User>()
								where u.Roles.Contains(role.Id) && u.Username.Contains(usernameToMatch)
								select u.Username;
					return users.ToArray();
				}
				return null;
			}
		}
        /// <summary>
        /// Gets a list of all the roles for the configured applicationName.
        /// </summary>
        /// <returns>
        /// A string array containing the names of all the roles stored in the data source for the configured applicationName.
        /// </returns>
		public override string[] GetAllRoles()
		{
			using (var session = this.DocumentStore.OpenSession())
			{
				var roles = (from r in session.Query<Role>()
							where r.ApplicationName == this.ApplicationName
							select r).ToList();
				return roles.Select(r => r.Name).ToArray();
			}
		}
        /// <summary>
        /// Gets a list of the roles that a specified user is in for the configured applicationName.
        /// </summary>
        /// <param name="username">The user to return a list of roles for.</param>
        /// <returns>
        /// A string array containing the names of all the roles that the specified user is in for the configured applicationName.
        /// </returns>
		public override string[] GetRolesForUser(string username)
		{
			using (var session = this.DocumentStore.OpenSession())
			{
				var user = (from u in session.Query<User>()
							where u.Username == username && u.ApplicationName == this.ApplicationName
							select u).Single();
				if (user.Roles.Any())
				{
					var dbRoles = session.Query<Role>().ToList();
					return dbRoles.Where(r => user.Roles.Contains(r.Id)).Select(r => r.Name).ToArray();
				}
				return new string[0];
			}
		}

		public override string[] GetUsersInRole(string roleName)
		{
			using (var session = this.DocumentStore.OpenSession())
			{
				var role = (from r in session.Query<Role>()
							where r.Name == roleName && r.ApplicationName == this.ApplicationName
							select r).SingleOrDefault();
				if (role != null)
				{
					var usernames = from u in session.Query<User>()
									where u.Roles.Contains(role.Id)
									select u.Username;
					return usernames.ToArray();
				}
				return null;
			}
		}
        /// <summary>
        /// Gets a value indicating whether the specified user is in the specified role for the configured applicationName.
        /// </summary>
        /// <param name="username">The user name to search for.</param>
        /// <param name="roleName">The role to search in.</param>
        /// <returns>
        /// true if the specified user is in the specified role for the configured applicationName; otherwise, false.
        /// </returns>
		public override bool IsUserInRole(string username, string roleName)
		{
			using (var session = this.DocumentStore.OpenSession())
			{
				var user = session.Query<User>()
					.Where(u => u.Username == username && u.ApplicationName == this.ApplicationName)
					.SingleOrDefault();
				if (user != null)
				{
					var role = (from r in session.Query<Role>()
								where r.Name == roleName && r.ApplicationName == this.ApplicationName
								select r).SingleOrDefault();
					if (role != null)
					{
						return user.Roles.Contains(role.Id);
					}
				}
				return false;
			}
		}
        /// <summary>
        /// Removes the specified user names from the specified roles for the configured applicationName.
        /// </summary>
        /// <param name="usernames">A string array of user names to be removed from the specified roles.</param>
        /// <param name="roleNames">A string array of role names to remove the specified user names from.</param>
		public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
		{
			if (usernames.Length == 0 || roleNames.Length == 0)
			{
				return;
			}
			using (var session = this.DocumentStore.OpenSession())
			{
				try
				{
					var users = session.Advanced.LuceneQuery<User>().OpenSubclause();
					foreach (var username in usernames)
					{
						users = users.WhereEquals("Username", username, true);
					}
					users = users.CloseSubclause().AndAlso().WhereEquals("ApplicationName", this.ApplicationName, true);

					var usersAsList = users.ToList();
					var roles = session.Advanced.LuceneQuery<Role>().OpenSubclause();
					foreach (var roleName in roleNames)
					{
						roles = roles.WhereEquals("Name", roleName, true);
					}
					roles = roles.CloseSubclause().AndAlso().WhereEquals("ApplicationName", this.ApplicationName);

					var roleIds = roles.Select(r => r.Id).ToList();
					foreach (var roleId in roleIds)
					{
						var usersWithRole = usersAsList.Where(u => u.Roles.Contains(roleId));
						foreach (var user in usersWithRole)
						{
							user.Roles.Remove(roleId);
						}
					}
					session.SaveChanges();
				}
				catch (Exception ex)
				{
					// TODO: log exception properly
					Console.WriteLine(ex.ToString());
					throw;
				}
			}
		}
        /// <summary>
        /// Gets a value indicating whether the specified role name already exists in the role data source for the configured applicationName.
        /// </summary>
        /// <param name="roleName">The name of the role to search for in the data source.</param>
        /// <returns>
        /// true if the role name already exists in the data source for the configured applicationName; otherwise, false.
        /// </returns>
		public override bool RoleExists(string roleName)
		{
			using (var session = this.DocumentStore.OpenSession())
			{
				return session.Query<Role>().Any(r => r.Name == roleName);
			}
		}
	}
}
