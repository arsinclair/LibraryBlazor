using System;
using System.Reflection;
using DataAccessLibrary;
using DataAccessLibrary.Helpers.Identity;
using DataAccessLibrary.Helpers.Identity.Abstract;
using DataAccessLibrary.Helpers.Identity.Stores;
using DataAccessLibrary.Helpers.Identity.Tables;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods on <see cref="IdentityBuilder"/> class.
    /// </summary>
    public static class IdentityBuilderExtensions
    {
        /// <summary>
        /// Adds a Dapper implementation of ASP.NET Core Identity stores.
        /// </summary>
        /// <param name="builder">Helper functions for configuring identity services.</param>
        /// <param name="configureAction">Delegate for configuring options </param>
        /// <returns>The <see cref="IdentityBuilder"/> instance this method extends.</returns>
        public static IdentityBuilder AddDapperStores(this IdentityBuilder builder)
        {
            AddStores(builder.Services, builder.UserType);
            return builder;
        }

        private static void AddStores(IServiceCollection services, Type userType)
        {
            var identityUserType = FindGenericBaseType(userType, typeof(IdentityUser<>));
            if (identityUserType == null)
            {
                throw new InvalidOperationException($"Method {nameof(AddDapperStores)} can only be called with a user that derives from IdentityUser<TKey>.");
            }
            var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var dbConnectionContextOptions = new IdentityStoreOptions
            {
                ConnectionString = configuration.GetConnectionString("DefaultConnection"),
                DbConnectionFactory = new SqlServerDbConnectionFactory()
            };
            var keyType = identityUserType.GenericTypeArguments[0];
            services.TryAddScoped(typeof(IDbConnectionFactory), x => {
                var dbConnectionFactoryInstance = (IDbConnectionFactory)Activator.CreateInstance(dbConnectionContextOptions.DbConnectionFactory.GetType());
                dbConnectionFactoryInstance.ConnectionString = dbConnectionContextOptions.ConnectionString;
                return dbConnectionFactoryInstance;
            });
            Type userStoreType;
            var userClaimType = typeof(IdentityUserClaim<>).MakeGenericType(keyType);
            var userLoginType = typeof(IdentityUserLogin<>).MakeGenericType(keyType);
            var userTokenType = typeof(IdentityUserToken<>).MakeGenericType(keyType);

            services.TryAddScoped(
                typeof(IUsersTable<,>).MakeGenericType(userType, keyType),
                typeof(UsersTable<,>).MakeGenericType(userType, keyType)
            );
            userStoreType = typeof(UserStore<,,,,>).MakeGenericType(userType, keyType, userClaimType, userLoginType, userTokenType);

            services.TryAddScoped(typeof(IUserStore<>).MakeGenericType(userType), userStoreType);
        }

        private static TypeInfo FindGenericBaseType(Type currentType, Type genericBaseType)
        {
            var type = currentType;
            while (type != null)
            {
                var typeInfo = type.GetTypeInfo();
                var genericType = type.IsGenericType ? type.GetGenericTypeDefinition() : null;
                if (genericType != null && genericType == genericBaseType)
                {
                    return typeInfo;
                }
                type = type.BaseType;
            }
            return null;
        }
    }
}
