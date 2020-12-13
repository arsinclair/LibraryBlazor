using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace DataAccessLibrary.Helpers.Identity.Abstract
{
    /// <summary>
    /// Abstraction for interacting with AspNetUsers table.
    /// </summary>
    /// <typeparam name="TUser">The type representing a user.</typeparam>
    /// <typeparam name="TKey">The type of the primary key for a role and user.</typeparam>
    public interface IUsersTable<TUser, TKey>
        where TUser : IdentityUser<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Creates a new user in the store.
        /// </summary>
        /// <param name="user">The user to create in the store.</param>
        Task<bool> CreateAsync(TUser user);

        /// <summary>
        /// Deletes a user from the store.
        /// </summary>
        /// <param name="userId">The id of the user to delete from the store.</param>
        Task<bool> DeleteAsync(TKey userId);

        /// <summary>
        /// Finds the user who has the specified id.
        /// </summary>
        /// <param name="userId">The user id to look for.</param>
        Task<TUser> FindByIdAsync(TKey userId);

        /// <summary>
        /// Finds the user who has the specified username.
        /// </summary>
        /// <param name="normalizedUserName">The username to look for.</param>
        Task<TUser> FindByNameAsync(string normalizedUserName);

        /// <summary>
        /// Finds the user who has the specified email.
        /// </summary>
        /// <param name="normalizedEmail">The email to look for.</param>
        Task<TUser> FindByEmailAsync(string normalizedEmail);

        /// <summary>
        /// Updates a user in the store.
        /// </summary>
        /// <param name="user">The user to update in the store.</param>
        Task<bool> UpdateAsync(TUser user);
    }
}
