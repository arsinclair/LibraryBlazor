using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using DataAccessLibrary.Helpers.Identity.Abstract;
using Microsoft.AspNetCore.Identity;

namespace DataAccessLibrary.Helpers.Identity.Stores
{

    /// <summary>
    /// Represents a persistence store for the specified user type, without support for roles.
    /// </summary>
    /// <typeparam name="TUser">The type representing a user.</typeparam>
    /// <typeparam name="TKey">The type of the primary key for a role and user.</typeparam>
    /// <typeparam name="TUserClaim">The type representing a claim.</typeparam>
    /// <typeparam name="TUserLogin">The type representing a user external login.</typeparam>
    /// <typeparam name="TUserToken">The type representing a user token.</typeparam>
    public class UserStore<TUser, TKey, TUserClaim, TUserLogin, TUserToken> :
        UserStoreBase<TUser, TKey, TUserClaim, TUserLogin, TUserToken>,
        IProtectedUserStore<TUser>
        where TUser : IdentityUser<TKey>
        where TKey : IEquatable<TKey>
        where TUserClaim : IdentityUserClaim<TKey>, new()
        where TUserLogin : IdentityUserLogin<TKey>, new()
        where TUserToken : IdentityUserToken<TKey>, new()
    {
        /// <summary>
        /// Constructs a new instance of <see cref="UserStore{TUser, TKey, TUserClaim, TUserLogin, TUserToken}"/>.
        /// </summary>
        /// <param name="usersTable">Abstraction for interacting with AspNetUsers table.</param>
        /// <param name="describer">The <see cref="IdentityErrorDescriber"/>.</param>
        public UserStore(IUsersTable<TUser, TKey> usersTable, IdentityErrorDescriber describer) : base(describer)
        {
            UsersTable = usersTable ?? throw new ArgumentNullException(nameof(usersTable));
        }

        /// <summary>
        /// Abstraction for interacting with AspNetUsers table.
        /// </summary>
        public IUsersTable<TUser, TKey> UsersTable { get; }

        /// <inheritdoc/>
        public override IQueryable<TUser> Users => throw new NotSupportedException();

        /// <inheritdoc/>
        public override Task AddClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override Task AddLoginAsync(TUser user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override async Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.ThrowIfNull(nameof(user));
            var created = await UsersTable.CreateAsync(user);
            return created ? IdentityResult.Success : IdentityResult.Failed(new IdentityError
            {
                Code = string.Empty,
                Description = $"User '{user.UserName}' could not be created."
            });
        }

        /// <inheritdoc/>
        public override async Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.ThrowIfNull(nameof(user));
            var deleted = await UsersTable.DeleteAsync(user.Id);
            return deleted ? IdentityResult.Success : IdentityResult.Failed(new IdentityError
            {
                Code = string.Empty,
                Description = $"User '{user.UserName}' could not be deleted."
            });
        }

        /// <inheritdoc/>
        public override async Task<TUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            var user = await UsersTable.FindByEmailAsync(normalizedEmail);
            return user;
        }

        /// <inheritdoc/>
        public override async Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            var id = ConvertIdFromString(userId);
            var user = await UsersTable.FindByIdAsync(id);
            return user;
        }

        /// <inheritdoc/>
        public override async Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            var user = await UsersTable.FindByNameAsync(normalizedUserName);
            return user;
        }

        /// <inheritdoc/>
        public override Task<IList<Claim>> GetClaimsAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult((IList<Claim>)(new List<Claim>()));
        }

        /// <inheritdoc/>
        public override Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override Task<IList<TUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override Task RemoveClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override Task RemoveLoginAsync(TUser user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override Task ReplaceClaimAsync(TUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public override async Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.ThrowIfNull(nameof(user));
            user.ConcurrencyStamp = Guid.NewGuid().ToString();
            var updated = await UsersTable.UpdateAsync(user);
            return updated ? IdentityResult.Success : IdentityResult.Failed(new IdentityError
            {
                Code = string.Empty,
                Description = $"User '{user.UserName}' could not be updated."
            });
        }

        /// <inheritdoc/>
        protected override Task AddUserTokenAsync(TUserToken token)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        protected override Task<TUserToken> FindTokenAsync(TUser user, string loginProvider, string name, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        protected override async Task<TUser> FindUserAsync(TKey userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            var user = await UsersTable.FindByIdAsync(userId);
            return user;
        }

        /// <inheritdoc/>
        protected override Task<TUserLogin> FindUserLoginAsync(TKey userId, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        protected override Task<TUserLogin> FindUserLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        protected override Task RemoveUserTokenAsync(TUserToken token)
        {
            throw new NotImplementedException();
        }
    }
}
