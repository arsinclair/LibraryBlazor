using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using DataAccessLibrary.Helpers.Identity.Abstract;
using Microsoft.AspNetCore.Identity;

namespace DataAccessLibrary.Helpers.Identity.Tables
{
    /// <summary>
    /// The default implementation of <see cref="IUsersTable{TUser, TKey}"/>.
    /// </summary>
    /// <typeparam name="TUser">The type representing a user.</typeparam>
    /// <typeparam name="TKey">The type of the primary key for a role and user.</typeparam>
    public class UsersTable<TUser, TKey> :
        IdentityTable,
        IUsersTable<TUser, TKey>
        where TUser : IdentityUser<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Creates a new instance of <see cref="UsersTable{TUser, TKey}"/>.
        /// </summary>
        /// <param name="dbConnectionFactory">A factory for creating instances of <see cref="IDbConnection"/>.</param>
        public UsersTable(IDbConnectionFactory dbConnectionFactory) : base(dbConnectionFactory) { }

        /// <inheritdoc/>
        public virtual async Task<bool> CreateAsync(TUser user)
        {
            const string sql = "INSERT INTO [dbo].[AspNetUsers] " +
                               "VALUES (@Id, @UserName, @NormalizedUserName, @Email, @NormalizedEmail, @EmailConfirmed, @PasswordHash, @SecurityStamp, @ConcurrencyStamp, " +
                                       "@PhoneNumber, @PhoneNumberConfirmed, @TwoFactorEnabled, @LockoutEnd, @LockoutEnabled, @AccessFailedCount);";
            var rowsInserted = await DbConnection.ExecuteAsync(sql, new
            {
                user.Id,
                user.UserName,
                user.NormalizedUserName,
                user.Email,
                user.NormalizedEmail,
                user.EmailConfirmed,
                user.PasswordHash,
                user.SecurityStamp,
                user.ConcurrencyStamp,
                user.PhoneNumber,
                user.PhoneNumberConfirmed,
                user.TwoFactorEnabled,
                user.LockoutEnd,
                user.LockoutEnabled,
                user.AccessFailedCount
            });
            return rowsInserted == 1;
        }

        /// <inheritdoc/>
        public virtual async Task<bool> DeleteAsync(TKey userId)
        {
            const string sql = "DELETE " +
                               "FROM [dbo].[AspNetUsers] " +
                               "WHERE [Id] = @Id;";
            var rowsDeleted = await DbConnection.ExecuteAsync(sql, new { Id = userId });
            return rowsDeleted == 1;
        }

        /// <inheritdoc/>
        public virtual async Task<TUser> FindByIdAsync(TKey userId)
        {
            const string sql = "SELECT * " +
                               "FROM [dbo].[AspNetUsers] " +
                               "WHERE [Id] = @Id;";
            var user = await DbConnection.QuerySingleOrDefaultAsync<TUser>(sql, new { Id = userId });
            return user;
        }

        /// <inheritdoc/>
        public virtual async Task<TUser> FindByNameAsync(string normalizedUserName)
        {
            const string sql = "SELECT * " +
                               "FROM [dbo].[AspNetUsers] " +
                               "WHERE [NormalizedUserName] = @NormalizedUserName;";
            var user = await DbConnection.QuerySingleOrDefaultAsync<TUser>(sql, new { NormalizedUserName = normalizedUserName });
            return user;
        }

        /// <inheritdoc/>
        public virtual async Task<TUser> FindByEmailAsync(string normalizedEmail)
        {
            const string command = "SELECT * " +
                                   "FROM [dbo].[AspNetUsers] " +
                                   "WHERE [NormalizedEmail] = @NormalizedEmail;";
            var user = await DbConnection.QuerySingleOrDefaultAsync<TUser>(command, new { NormalizedEmail = normalizedEmail });
            return user;
        }

        /// <inheritdoc/>
        public virtual async Task<bool> UpdateAsync(TUser user)
        {
            const string updateUserSql =
                "UPDATE [dbo].[AspNetUsers] " +
                "SET [UserName] = @UserName, [NormalizedUserName] = @NormalizedUserName, [Email] = @Email, [NormalizedEmail] = @NormalizedEmail, [EmailConfirmed] = @EmailConfirmed, " +
                    "[PasswordHash] = @PasswordHash, [SecurityStamp] = @SecurityStamp, [ConcurrencyStamp] = @ConcurrencyStamp, [PhoneNumber] = @PhoneNumber, " +
                    "[PhoneNumberConfirmed] = @PhoneNumberConfirmed, [TwoFactorEnabled] = @TwoFactorEnabled, [LockoutEnd] = @LockoutEnd, [LockoutEnabled] = @LockoutEnabled, " +
                    "[AccessFailedCount] = @AccessFailedCount " +
                "WHERE [Id] = @Id;";
            using (var transaction = DbConnection.BeginTransaction())
            {
                await DbConnection.ExecuteAsync(updateUserSql, new
                {
                    user.UserName,
                    user.NormalizedUserName,
                    user.Email,
                    user.NormalizedEmail,
                    user.EmailConfirmed,
                    user.PasswordHash,
                    user.SecurityStamp,
                    user.ConcurrencyStamp,
                    user.PhoneNumber,
                    user.PhoneNumberConfirmed,
                    user.TwoFactorEnabled,
                    user.LockoutEnd,
                    user.LockoutEnabled,
                    user.AccessFailedCount,
                    user.Id
                }, transaction);

                try
                {
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    return false;
                }
            }
            return true;
        }
    }
}
