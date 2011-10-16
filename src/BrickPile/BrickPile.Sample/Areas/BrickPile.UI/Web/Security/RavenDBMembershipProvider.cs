using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using System.Collections.Specialized;
using BrickPile.Domain.Models;
using Raven.Client;
using StructureMap;

namespace BrickPile.UI.Web.Security {
	public class RavenDBMembershipProvider : MembershipProvider {
		private const string ProviderName = "RavenDBMembership";
		private IDocumentStore _documentStore;
        /// <summary>
        /// Gets or sets the document store.
        /// </summary>
        /// <value>
        /// The document store.
        /// </value>
		public IDocumentStore DocumentStore {
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
        /// The name of the application using the custom membership provider.
        /// </summary>
        /// <returns>The name of the application using the custom membership provider.</returns>
		public override string ApplicationName { get; set; }
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
		public override void Initialize(string name, NameValueCollection config) {
			// Try to find an IDocumentStore via Common Service Locator. 
			try {
                this.DocumentStore = ObjectFactory.GetInstance<IDocumentStore>();
			}
			catch (NullReferenceException) // Swallow Nullreference expection that occurs when there is no current service locator.
			{
			}
			
			base.Initialize(name, config);
		}
        /// <summary>
        /// Processes a request to update the password for a membership user.
        /// </summary>
        /// <param name="username">The user to update the password for.</param>
        /// <param name="oldPassword">The current password for the specified user.</param>
        /// <param name="newPassword">The new password for the specified user.</param>
        /// <returns>
        /// true if the password was updated successfully; otherwise, false.
        /// </returns>
		public override bool ChangePassword(string username, string oldPassword, string newPassword) {
			ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, newPassword, false);
			OnValidatingPassword(args);
			if (args.Cancel)
			{
				throw new MembershipPasswordException("Apparently, the new password doesn't seem to be valid.");
			}
			using (var session = this.DocumentStore.OpenSession())
			{
				var q = from u in session.Query<User>()
						where u.Username == username && u.ApplicationName == this.ApplicationName
						select u;
				var user = q.SingleOrDefault();
				if (user == null || user.PasswordHash != PasswordUtil.HashPassword(oldPassword, user.PasswordSalt))
				{
					throw new MembershipPasswordException("Invalid username or old password.");
				}
				user.PasswordHash = PasswordUtil.HashPassword(newPassword, user.PasswordSalt);
				session.SaveChanges();
			}
			return true;
		}

        /// <summary>
        /// Processes a request to update the password question and answer for a membership user.
        /// </summary>
        /// <param name="username">The user to change the password question and answer for.</param>
        /// <param name="password">The password for the specified user.</param>
        /// <param name="newPasswordQuestion">The new password question for the specified user.</param>
        /// <param name="newPasswordAnswer">The new password answer for the specified user.</param>
        /// <returns>
        /// true if the password question and answer are updated successfully; otherwise, false.
        /// </returns>
		public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer) {
			throw new NotImplementedException();
		}
        /// <summary>
        /// Adds a new membership user to the data source.
        /// </summary>
        /// <param name="username">The user name for the new user.</param>
        /// <param name="password">The password for the new user.</param>
        /// <param name="email">The e-mail address for the new user.</param>
        /// <param name="passwordQuestion">The password question for the new user.</param>
        /// <param name="passwordAnswer">The password answer for the new user</param>
        /// <param name="isApproved">Whether or not the new user is approved to be validated.</param>
        /// <param name="providerUserKey">The unique identifier from the membership data source for the user.</param>
        /// <param name="status">A <see cref="T:System.Web.Security.MembershipCreateStatus"/> enumeration value indicating whether the user was created successfully.</param>
        /// <returns>
        /// A <see cref="T:System.Web.Security.MembershipUser"/> object populated with the information for the newly created user.
        /// </returns>
		public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status) {
			ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, password, true);
			OnValidatingPassword(args);
			if (args.Cancel) {
				status = MembershipCreateStatus.InvalidPassword;
				return null;
			}

			var user = new User {Username = username, PasswordSalt = PasswordUtil.CreateRandomSalt()};
		    user.PasswordHash = PasswordUtil.HashPassword(password, user.PasswordSalt);
			user.Email = email;
			user.ApplicationName = this.ApplicationName;
			user.DateCreated = DateTime.Now;

			using (var session = this.DocumentStore.OpenSession()) {
				try {
					session.Store(user);
					session.SaveChanges();
					status = MembershipCreateStatus.Success;
					return new MembershipUser(ProviderName, username, user.Id, email, null, null, true, false, user.DateCreated,
						new DateTime(1900, 1, 1), new DateTime(1900, 1, 1), DateTime.Now, new DateTime(1900, 1, 1));
				}
				catch (Exception ex) {
					// TODO: log exception properly
					Console.WriteLine(ex.ToString());
					status = MembershipCreateStatus.ProviderError;
				}
			}
			return null;
		}
        /// <summary>
        /// Removes a user from the membership data source.
        /// </summary>
        /// <param name="username">The name of the user to delete.</param>
        /// <param name="deleteAllRelatedData">true to delete data related to the user from the database; false to leave data related to the user in the database.</param>
        /// <returns>
        /// true if the user was successfully deleted; otherwise, false.
        /// </returns>
		public override bool DeleteUser(string username, bool deleteAllRelatedData)
		{
			using (var session = this.DocumentStore.OpenSession())
			{
				try
				{
					var q = from u in session.Query<User>()
							where u.Username == username && u.ApplicationName == this.ApplicationName
							select u;
					var user = q.SingleOrDefault();
					if (user == null)
					{
						throw new NullReferenceException("The user could not be deleted.");
					}
					session.Delete(user);
					session.SaveChanges();
					return true;
				}
				catch (Exception ex)
				{
					// TODO: log exception properly
					Console.WriteLine(ex.ToString());
					return false;
				}
			}
		}
        /// <summary>
        /// Indicates whether the membership provider is configured to allow users to reset their passwords.
        /// </summary>
        /// <returns>true if the membership provider supports password reset; otherwise, false. The default is true.</returns>
		public override bool EnablePasswordReset
		{
			get { return true; }
		}
        /// <summary>
        /// Indicates whether the membership provider is configured to allow users to retrieve their passwords.
        /// </summary>
        /// <returns>true if the membership provider is configured to support password retrieval; otherwise, false. The default is false.</returns>
		public override bool EnablePasswordRetrieval
		{
			get { return false; }
		}
        /// <summary>
        /// Gets a collection of membership users where the e-mail address contains the specified e-mail address to match.
        /// </summary>
        /// <param name="emailToMatch">The e-mail address to search for.</param>
        /// <param name="pageIndex">The index of the page of results to return. <paramref name="pageIndex"/> is zero-based.</param>
        /// <param name="pageSize">The size of the page of results to return.</param>
        /// <param name="totalRecords">The total number of matched users.</param>
        /// <returns>
        /// A <see cref="T:System.Web.Security.MembershipUserCollection"/> collection that contains a page of <paramref name="pageSize"/><see cref="T:System.Web.Security.MembershipUser"/> objects beginning at the page specified by <paramref name="pageIndex"/>.
        /// </returns>
		public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
		{
			return FindUsers(u => u.Email.Contains(emailToMatch), pageIndex, pageSize, out totalRecords);
		}
        /// <summary>
        /// Gets a collection of membership users where the user name contains the specified user name to match.
        /// </summary>
        /// <param name="usernameToMatch">The user name to search for.</param>
        /// <param name="pageIndex">The index of the page of results to return. <paramref name="pageIndex"/> is zero-based.</param>
        /// <param name="pageSize">The size of the page of results to return.</param>
        /// <param name="totalRecords">The total number of matched users.</param>
        /// <returns>
        /// A <see cref="T:System.Web.Security.MembershipUserCollection"/> collection that contains a page of <paramref name="pageSize"/><see cref="T:System.Web.Security.MembershipUser"/> objects beginning at the page specified by <paramref name="pageIndex"/>.
        /// </returns>
		public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
		{
			return FindUsers(u => u.Username.Contains(usernameToMatch), pageIndex, pageSize, out totalRecords);
		}
        /// <summary>
        /// Gets a collection of all the users in the data source in pages of data.
        /// </summary>
        /// <param name="pageIndex">The index of the page of results to return. <paramref name="pageIndex"/> is zero-based.</param>
        /// <param name="pageSize">The size of the page of results to return.</param>
        /// <param name="totalRecords">The total number of matched users.</param>
        /// <returns>
        /// A <see cref="T:System.Web.Security.MembershipUserCollection"/> collection that contains a page of <paramref name="pageSize"/><see cref="T:System.Web.Security.MembershipUser"/> objects beginning at the page specified by <paramref name="pageIndex"/>.
        /// </returns>
		public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
		{
			return FindUsers(null, pageIndex, pageSize, out totalRecords);
		}
        /// <summary>
        /// Gets the number of users currently accessing the application.
        /// </summary>
        /// <returns>
        /// The number of users currently accessing the application.
        /// </returns>
		public override int GetNumberOfUsersOnline()
		{
			throw new NotImplementedException();
		}
        /// <summary>
        /// Gets the password for the specified user name from the data source.
        /// </summary>
        /// <param name="username">The user to retrieve the password for.</param>
        /// <param name="answer">The password answer for the user.</param>
        /// <returns>
        /// The password for the specified user name.
        /// </returns>
		public override string GetPassword(string username, string answer)
		{
			throw new NotImplementedException();
		}
        /// <summary>
        /// Gets information from the data source for a user. Provides an option to update the last-activity date/time stamp for the user.
        /// </summary>
        /// <param name="username">The name of the user to get information for.</param>
        /// <param name="userIsOnline">true to update the last-activity date/time stamp for the user; false to return user information without updating the last-activity date/time stamp for the user.</param>
        /// <returns>
        /// A <see cref="T:System.Web.Security.MembershipUser"/> object populated with the specified user's information from the data source.
        /// </returns>
		public override MembershipUser GetUser(string username, bool userIsOnline)
		{
			using (var session = this.DocumentStore.OpenSession())
			{
				var q = from u in session.Query<User>()
						where u.Username == username && u.ApplicationName == this.ApplicationName
						select u;
				var user = q.SingleOrDefault();
				if (user != null)
				{
					return UserToMembershipUser(user);
				}
				return null;
			}
		}
        /// <summary>
        /// Gets user information from the data source based on the unique identifier for the membership user. Provides an option to update the last-activity date/time stamp for the user.
        /// </summary>
        /// <param name="providerUserKey">The unique identifier for the membership user to get information for.</param>
        /// <param name="userIsOnline">true to update the last-activity date/time stamp for the user; false to return user information without updating the last-activity date/time stamp for the user.</param>
        /// <returns>
        /// A <see cref="T:System.Web.Security.MembershipUser"/> object populated with the specified user's information from the data source.
        /// </returns>
		public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
		{
			using (var session = this.DocumentStore.OpenSession())
			{
				var user = session.Load<User>(providerUserKey.ToString());
				if (user != null)
				{
					return UserToMembershipUser(user);
				}
				return null;
			}
		}
        /// <summary>
        /// Gets the user name associated with the specified e-mail address.
        /// </summary>
        /// <param name="email">The e-mail address to search for.</param>
        /// <returns>
        /// The user name associated with the specified e-mail address. If no match is found, return null.
        /// </returns>
		public override string GetUserNameByEmail(string email)
		{
			using (var session = this.DocumentStore.OpenSession())
			{
				var q = from u in session.Query<User>()
						where u.Email == email && u.ApplicationName == this.ApplicationName
						select u.Username;
				return q.SingleOrDefault();
			}
		}
        /// <summary>
        /// Gets the number of invalid password or password-answer attempts allowed before the membership user is locked out.
        /// </summary>
        /// <returns>The number of invalid password or password-answer attempts allowed before the membership user is locked out.</returns>
		public override int MaxInvalidPasswordAttempts
		{
			get { return 10; }
		}
        /// <summary>
        /// Gets the minimum number of special characters that must be present in a valid password.
        /// </summary>
        /// <returns>The minimum number of special characters that must be present in a valid password.</returns>
		public override int MinRequiredNonAlphanumericCharacters
		{
			get { return 0; }
		}
        /// <summary>
        /// Gets the minimum length required for a password.
        /// </summary>
        /// <returns>The minimum length required for a password. </returns>
		public override int MinRequiredPasswordLength
		{
			get { return 5; }
		}

		public override int PasswordAttemptWindow
		{
			get { return 5; }
		}
        /// <summary>
        /// Gets a value indicating the format for storing passwords in the membership data store.
        /// </summary>
        /// <returns>One of the <see cref="T:System.Web.Security.MembershipPasswordFormat"/> values indicating the format for storing passwords in the data store.</returns>
		public override MembershipPasswordFormat PasswordFormat
		{
			get { return MembershipPasswordFormat.Hashed; }
		}
        /// <summary>
        /// Gets the regular expression used to evaluate a password.
        /// </summary>
        /// <returns>A regular expression used to evaluate a password.</returns>
		public override string PasswordStrengthRegularExpression
		{
			get { return String.Empty; }
		}
        /// <summary>
        /// Gets a value indicating whether the membership provider is configured to require the user to answer a password question for password reset and retrieval.
        /// </summary>
        /// <returns>true if a password answer is required for password reset and retrieval; otherwise, false. The default is true.</returns>
		public override bool RequiresQuestionAndAnswer
		{
			get { return false; }
		}
        /// <summary>
        /// Gets a value indicating whether the membership provider is configured to require a unique e-mail address for each user name.
        /// </summary>
        /// <returns>true if the membership provider requires a unique e-mail address; otherwise, false. The default is true.</returns>
		public override bool RequiresUniqueEmail
		{
			get { return false; }
		}
        /// <summary>
        /// Resets a user's password to a new, automatically generated password.
        /// </summary>
        /// <param name="username">The user to reset the password for.</param>
        /// <param name="answer">The password answer for the specified user.</param>
        /// <returns>
        /// The new password for the specified user.
        /// </returns>
		public override string ResetPassword(string username, string answer)
		{
			using (var session = this.DocumentStore.OpenSession())
			{
				try
				{
					var q = from u in session.Query<User>()
							where u.Username == username && u.ApplicationName == this.ApplicationName
							select u;
					var user = q.SingleOrDefault();
					if (user == null)
					{
						throw new Exception("The user to reset the password for could not be found.");
					}
					var newPassword = Membership.GeneratePassword(8, 2);
					user.PasswordHash = PasswordUtil.HashPassword(newPassword, user.PasswordSalt);
					session.SaveChanges();
					return newPassword;
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
        /// Clears a lock so that the membership user can be validated.
        /// </summary>
        /// <param name="userName">The membership user whose lock status you want to clear.</param>
        /// <returns>
        /// true if the membership user was successfully unlocked; otherwise, false.
        /// </returns>
		public override bool UnlockUser(string userName)
		{
			throw new NotImplementedException();
		}
        /// <summary>
        /// Updates information about a user in the data source.
        /// </summary>
        /// <param name="user">A <see cref="T:System.Web.Security.MembershipUser"/> object that represents the user to update and the updated information for the user.</param>
		public override void UpdateUser(MembershipUser user)
		{
			using (var session = this.DocumentStore.OpenSession())
			{
				try
				{
					var q = from u in session.Query<User>()
							where u.Username == user.UserName && u.ApplicationName == this.ApplicationName
							select u;
					var dbUser = q.SingleOrDefault();
					if (dbUser == null)
					{
						throw new Exception("The user to update could not be found.");
					}
					dbUser.Username = user.UserName;
					dbUser.Email = user.Email;
					dbUser.DateCreated = user.CreationDate;
					dbUser.DateLastLogin = user.LastLoginDate;
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
        /// Verifies that the specified user name and password exist in the data source.
        /// </summary>
        /// <param name="username">The name of the user to validate.</param>
        /// <param name="password">The password for the specified user.</param>
        /// <returns>
        /// true if the specified username and password are valid; otherwise, false.
        /// </returns>
		public override bool ValidateUser(string username, string password)
		{
			using (var session = this.DocumentStore.OpenSession())
			{
				var q = from u in session.Query<User>()
						where u.Username == username && u.ApplicationName == this.ApplicationName
						select u;
				var user = q.SingleOrDefault();
				if (user != null && user.PasswordHash == PasswordUtil.HashPassword(password, user.PasswordSalt))
				{
					user.DateLastLogin = DateTime.Now;
					session.SaveChanges();
					return true;
				}
			}
			return false;
		}
        /// <summary>
        /// Finds the users.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalRecords">The total records.</param>
        /// <returns></returns>
		private MembershipUserCollection FindUsers(Func<User, bool> predicate, int pageIndex, int pageSize, out int totalRecords)
		{
			var membershipUsers = new MembershipUserCollection();
			using (var session = this.DocumentStore.OpenSession())
			{
				var q = from u in session.Query<User>()
							where u.ApplicationName == this.ApplicationName
						select u;
				IEnumerable<User> results;
				if (predicate != null)
				{
					results = q.Where(predicate);
				}
				else
				{
					results = q;
				}
				totalRecords = results.Count();
				var pagedUsers = results.Skip(pageIndex * pageSize).Take(pageSize);
				foreach (var user in pagedUsers)
				{
					membershipUsers.Add(UserToMembershipUser(user));
				}
			}
			return membershipUsers;
		}
        /// <summary>
        /// Users to membership user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
		private MembershipUser UserToMembershipUser(User user)
		{
			return new MembershipUser(ProviderName, user.Username, user.Id, user.Email, null, null, true, false
				, user.DateCreated, user.DateLastLogin.HasValue ? user.DateLastLogin.Value : new DateTime(1900, 1, 1), new DateTime(1900, 1, 1), new DateTime(1900, 1, 1), new DateTime(1900, 1, 1));
		}
	}
}
