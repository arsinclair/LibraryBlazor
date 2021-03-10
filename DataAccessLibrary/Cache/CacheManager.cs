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

        private static void ThrowIfNotInitialized()
        {
            if (!IsInitialized)
            {
                throw new Exception("Cache Manager not initialized");
            }
        }
    }
}
