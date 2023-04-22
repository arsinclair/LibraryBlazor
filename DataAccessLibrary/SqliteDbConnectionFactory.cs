using System.Data;
using System.Data.SQLite;

namespace DataAccessLibrary
{
    /// <summary>
    /// Creates a new <see cref="SQLiteConnection"/> instance for connecting to the local SQLite Database file.
    /// </summary>
    public class SqliteDbConnectionFactory : IDbConnectionFactory
    {
        /// <summary>
        /// The connection string to use for connecting to SQLite Database.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <inheritdoc/>
        public IDbConnection Create()
        {
            var sqlConnection = new SQLiteConnection(ConnectionString);
            sqlConnection.Open();
            return sqlConnection;
        }
    }
}
