using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Raven.Client;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;

namespace BrickPile.Core.Identity
{
    public class UserStore<TUser> : 
        IUserStore<TUser>, 
        IUserLoginStore<TUser>, 
        IUserClaimStore<TUser>, 
        IUserRoleStore<TUser>,
        IUserPasswordStore<TUser>, 
        IUserSecurityStampStore<TUser>, 
        IUserEmailStore<TUser>, 
        IUserLockoutStore<TUser, string>,
        IUserTwoFactorStore<TUser, string>, 
        IUserPhoneNumberStore<TUser>
        where TUser : IdentityUser
    {
        private bool _disposed;
        private readonly Func<IAsyncDocumentSession> getSessionFunc;
        private IAsyncDocumentSession _session;

        public UserStore(Func<IAsyncDocumentSession> getSession)
        {
            this.getSessionFunc = getSession;
        }

        public UserStore(IAsyncDocumentSession session)
        {
            this._session = session;
            //TODO fix this
            //this._session.Advanced.DocumentStore.Conventions.FindIdentityProperty = prop => prop.Name == "IdentityUsers/" + "Id";            
        }

        private IAsyncDocumentSession session
        {
            get
            {
                if (_session == null)
                {
                    _session = getSessionFunc();
                    //TODO fix this
                    //this._session.Advanced.DocumentStore.Conventions.FindIdentityProperty = prop => prop.Name == "IdentityUsers/" + "Id";
                }
                return _session;
            }
        }

        public async Task CreateAsync(TUser user)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (string.IsNullOrEmpty(user.Id))
            {
                var conventions = session.Advanced.DocumentStore.Conventions;
                var entityName = conventions.GetClrTypeName(typeof(TUser));
                var separator = conventions.IdentityPartsSeparator;
                var id = $"{entityName}{separator}{user.Email}";
                user.Id = id;
            }

            await session.StoreAsync(user);

            // This model allows us to lookup a user by name in order to get the id
            var userByName = new IdentityUserByUserName(user.Id, user.UserName);
            await session.StoreAsync(userByName, Util.GetIdentityUserByUserNameId(user.UserName));
        }

        public async Task DeleteAsync(TUser user)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var userByName = await session.LoadAsync<IdentityUserByUserName>(Util.GetIdentityUserByUserNameId(user.UserName));
            if (userByName != null)
            {
                session.Delete(userByName);
            }

            this.session.Delete(user);
        }

        public Task<TUser> FindByIdAsync(string userId)
        {
            return session.LoadAsync<TUser>(userId);
        }

        public async Task<TUser> FindByNameAsync(string userName)
        {
            var userByName = await session.LoadAsync<IdentityUserByUserName>(Util.GetIdentityUserByUserNameId(userName));
            if (userByName == null)
            {
                return null;
            }

            return await FindByIdAsync(userByName.UserId);
        }

        public Task UpdateAsync(TUser user)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return Task.FromResult(true);
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(this.GetType().Name);
            }
        }

        public void Dispose()
        {
            _disposed = true;
        }

        public async Task AddLoginAsync(TUser user, UserLoginInfo login)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (!user.Logins.Any(x => x.LoginProvider == login.LoginProvider && x.ProviderKey == login.ProviderKey))
            {
                user.Logins.Add(login);

                var userLogin = new IdentityUserLogin
                {
                    Id = Util.GetLoginId(login),
                    UserId = user.Id,
                    Provider = login.LoginProvider,
                    ProviderKey = login.ProviderKey
                };
                await session.StoreAsync(userLogin);
            }
        }

        public async Task<TUser> FindAsync(UserLoginInfo login)
        {
            string loginId = Util.GetLoginId(login);

            var loginDoc = await session.Include<IdentityUserLogin>(x => x.UserId)
                .LoadAsync(loginId);

            if (loginDoc != null)
            {
                return await session.LoadAsync<TUser>(loginDoc.UserId);
            }

            return null;
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return Task.FromResult(user.Logins.ToIList());
        }

        public async Task RemoveLoginAsync(TUser user, UserLoginInfo login)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            string loginId = Util.GetLoginId(login);
            var loginDoc = await session.LoadAsync<IdentityUserLogin>(loginId);
            if (loginDoc != null)
            {
                session.Delete(loginDoc);
            }

            user.Logins.RemoveAll(x => x.LoginProvider == login.LoginProvider && x.ProviderKey == login.ProviderKey);
        }

        public Task AddClaimAsync(TUser user, Claim claim)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (!user.Claims.Any(x => x.ClaimType == claim.Type && x.ClaimValue == claim.Value))
            {
                user.Claims.Add(new IdentityUserClaim
                {
                    ClaimType = claim.Type,
                    ClaimValue = claim.Value
                });
            }
            return Task.FromResult(0);
        }

        public Task<IList<Claim>> GetClaimsAsync(TUser user)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            IList<Claim> result = user.Claims.Select(c => new Claim(c.ClaimType, c.ClaimValue)).ToList();
            return Task.FromResult(result);
        }

        public Task RemoveClaimAsync(TUser user, Claim claim)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.Claims.RemoveAll(x => x.ClaimType == claim.Type && x.ClaimValue == claim.Value);
            return Task.FromResult(0);
        }

        public Task<string> GetPasswordHashAsync(TUser user)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(TUser user)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return Task.FromResult(user.PasswordHash != null);
        }

        public Task SetPasswordHashAsync(TUser user, string passwordHash)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public Task<string> GetSecurityStampAsync(TUser user)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return Task.FromResult(user.SecurityStamp);
        }

        public Task SetSecurityStampAsync(TUser user, string stamp)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.SecurityStamp = stamp;
            return Task.FromResult(0);
        }

        public Task AddToRoleAsync(TUser user, string role)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (!user.Roles.Contains(role, StringComparer.InvariantCultureIgnoreCase))
            {
                user.Roles.Add(role);
            }

            return Task.FromResult(0);
        }

        public Task<IList<string>> GetRolesAsync(TUser user)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return Task.FromResult<IList<string>>(user.Roles);
        }

        public Task<bool> IsInRoleAsync(TUser user, string role)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return Task.FromResult(user.Roles.Contains(role, StringComparer.InvariantCultureIgnoreCase));
        }

        public Task RemoveFromRoleAsync(TUser user, string role)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.Roles.RemoveAll(r => String.Equals(r, role, StringComparison.InvariantCultureIgnoreCase));

            return Task.FromResult(0);
        }

        public Task<TUser> FindByEmailAsync(string email)
        {
            ThrowIfDisposed();
            if (email == null)
            {
                throw new ArgumentNullException(nameof(email));
            }

            return session.Query<TUser>()
                .Where(u => u.Email == email)
                .FirstOrDefaultAsync();
        }

        public Task<string> GetEmailAsync(TUser user)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(TUser user)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return Task.FromResult(user.IsEmailConfirmed);
        }

        public Task SetEmailAsync(TUser user, string email)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.Email = email;

            return Task.FromResult(0);
        }

        public Task SetEmailConfirmedAsync(TUser user, bool confirmed)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.IsEmailConfirmed = confirmed;

            return Task.FromResult(0);
        }

        public Task<int> GetAccessFailedCountAsync(TUser user)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<bool> GetLockoutEnabledAsync(TUser user)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return Task.FromResult(user.LockoutEnabled);
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(TUser user)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return Task.FromResult(user.LockoutEndDate);
        }

        public Task<int> IncrementAccessFailedCountAsync(TUser user)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.AccessFailedCount++;

            return Task.FromResult(user.AccessFailedCount);
        }

        public Task ResetAccessFailedCountAsync(TUser user)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.AccessFailedCount = 0;

            return Task.FromResult(0);
        }

        public Task SetLockoutEnabledAsync(TUser user, bool enabled)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.LockoutEnabled = enabled;

            return Task.FromResult(0);
        }

        public Task SetLockoutEndDateAsync(TUser user, DateTimeOffset lockoutEnd)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.LockoutEndDate = lockoutEnd;

            return Task.FromResult(0);
        }

        public Task<bool> GetTwoFactorEnabledAsync(TUser user)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return Task.FromResult(user.TwoFactorAuthEnabled);
        }

        public Task SetTwoFactorEnabledAsync(TUser user, bool enabled)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.TwoFactorAuthEnabled = enabled;

            return Task.FromResult(0);
        }

        public Task<string> GetPhoneNumberAsync(TUser user)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(TUser user)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return Task.FromResult(user.IsPhoneNumberConfirmed);
        }

        public Task SetPhoneNumberAsync(TUser user, string phoneNumber)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.PhoneNumber = phoneNumber;

            return Task.FromResult(0);
        }

        public Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed)
        {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.IsPhoneNumberConfirmed = confirmed;

            return Task.FromResult(0);
        }
    }
}
