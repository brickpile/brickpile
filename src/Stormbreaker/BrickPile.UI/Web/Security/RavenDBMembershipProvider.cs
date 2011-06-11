using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using System.Collections.Specialized;
using BrickPile.Domain.Models;
using Raven.Client;
using StructureMap;

namespace BrickPile.UI.Web.Security
{
	public class RavenDBMembershipProvider : MembershipProvider
	{
		private const string ProviderName = "RavenDBMembership";
		private IDocumentStore _documentStore;
		
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

		public override string ApplicationName
		{
			get; set;
		}

		public override void Initialize(string name, NameValueCollection config)
		{
			// Try to find an IDocumentStore via Common Service Locator. 
			try {
                this.DocumentStore = ObjectFactory.GetInstance<IDocumentStore>();
			}
			catch (NullReferenceException) // Swallow Nullreference expection that occurs when there is no current service locator.
			{
			}
			
			base.Initialize(name, config);
		}

		public override bool ChangePassword(string username, string oldPassword, string newPassword)
		{
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

		public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
		{
			throw new NotImplementedException();
		}

		public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
		{
			ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, password, true);
			OnValidatingPassword(args);
			if (args.Cancel)
			{
				status = MembershipCreateStatus.InvalidPassword;
				return null;
			}

			var user = new User {Username = username, PasswordSalt = PasswordUtil.CreateRandomSalt()};
		    user.PasswordHash = PasswordUtil.HashPassword(password, user.PasswordSalt);
			user.Email = email;
			user.ApplicationName = this.ApplicationName;
			user.DateCreated = DateTime.Now;

			using (var session = this.DocumentStore.OpenSession())
			{
				try
				{
					session.Store(user);
					session.SaveChanges();
					status = MembershipCreateStatus.Success;
					return new MembershipUser(ProviderName, username, user.Id, email, null, null, true, false, user.DateCreated,
						new DateTime(1900, 1, 1), new DateTime(1900, 1, 1), DateTime.Now, new DateTime(1900, 1, 1));
				}
				catch (Exception ex)
				{
					// TODO: log exception properly
					Console.WriteLine(ex.ToString());
					status = MembershipCreateStatus.ProviderError;
				}
			}
			return null;
		}

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

		public override bool EnablePasswordReset
		{
			get { return true; }
		}

		public override bool EnablePasswordRetrieval
		{
			get { return false; }
		}

		public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
		{
			return FindUsers(u => u.Email.Contains(emailToMatch), pageIndex, pageSize, out totalRecords);
		}

		public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
		{
			return FindUsers(u => u.Username.Contains(usernameToMatch), pageIndex, pageSize, out totalRecords);
		}

		public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
		{
			return FindUsers(null, pageIndex, pageSize, out totalRecords);
		}

		public override int GetNumberOfUsersOnline()
		{
			throw new NotImplementedException();
		}

		public override string GetPassword(string username, string answer)
		{
			throw new NotImplementedException();
		}

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

		public override int MaxInvalidPasswordAttempts
		{
			get { return 10; }
		}

		public override int MinRequiredNonAlphanumericCharacters
		{
			get { return 0; }
		}

		public override int MinRequiredPasswordLength
		{
			get { return 5; }
		}

		public override int PasswordAttemptWindow
		{
			get { return 5; }
		}

		public override MembershipPasswordFormat PasswordFormat
		{
			get { return MembershipPasswordFormat.Hashed; }
		}

		public override string PasswordStrengthRegularExpression
		{
			get { return String.Empty; }
		}

		public override bool RequiresQuestionAndAnswer
		{
			get { return false; }
		}

		public override bool RequiresUniqueEmail
		{
			get { return false; }
		}

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

		public override bool UnlockUser(string userName)
		{
			throw new NotImplementedException();
		}

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

		private MembershipUser UserToMembershipUser(User user)
		{
			return new MembershipUser(ProviderName, user.Username, user.Id, user.Email, null, null, true, false
				, user.DateCreated, user.DateLastLogin.HasValue ? user.DateLastLogin.Value : new DateTime(1900, 1, 1), new DateTime(1900, 1, 1), new DateTime(1900, 1, 1), new DateTime(1900, 1, 1));
		}
	}
}
