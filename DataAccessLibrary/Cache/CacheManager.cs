using Microsoft.Extensions.Configuration;
using System;

namespace DataAccessLibrary.Cache
{
    public static class CacheManager
    {
        private static IConfiguration _configuration;
        private static string _connectionString;
        private static DatabaseCache _databaseCache;
        internal static bool IsInitialized;

        public static void Initialize(IConfiguration configuration)
        {
            if (!IsInitialized)
            {
                _configuration = configuration;
                _connectionString = configuration.GetConnectionString("DefaultConnection");
                IsInitialized = true;
            }
        }

        public static DatabaseCache GetDatabaseCache()
        {
            ThrowIfNotInitialized();
            if (_databaseCache == null) _databaseCache = new DatabaseCache(_connectionString);
            return _databaseCache;
        }

        /// <summary>
        ///     <para>Allows to bypass building the cache from the database, by setting the in-memory DatabaseCache to provided method parameter. Should only be used for unit tests to avoid spinning up the database instance.</para>
        /// </summary>
        internal static void SetBypassDatabaseCache(DatabaseCache databaseCache)
        {
            if (!IsInitialized)
            {
                if (_databaseCache == null) _databaseCache = databaseCache;
                IsInitialized = true;
            }
        }

        internal static void DisposeCache()
        {
            if (IsInitialized)
            {
                _databaseCache = null;
                IsInitialized = false;
            }
        }

        private static void ThrowIfNotInitialized()
        {
            if (!IsInitialized)
            {
                throw new Exception("Cache Manager not initialized");
            }
        }
    }
}
