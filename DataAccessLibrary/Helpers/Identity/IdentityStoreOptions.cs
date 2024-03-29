﻿using System.Data;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccessLibrary.Helpers.Identity
{
    /// <summary>
    /// Options for configuring Dapper stores.
    /// </summary>
    public class IdentityStoreOptions
    {
        internal IServiceCollection Services;
        /// <summary>
        /// The connection string to use for connecting to the data source.
        /// </summary>
        public string ConnectionString { get; set; }
        /// <summary>
        /// A factory for creating instances of <see cref="IDbConnection"/>.
        /// </summary>
        public IDbConnectionFactory DbConnectionFactory { get; set; }
    }
}
