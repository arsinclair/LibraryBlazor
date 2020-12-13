using System.Data;

namespace DataAccessLibrary.Helpers.Identity.Tables
{
    public class IdentityTable : TableBase
    {
        /// <summary>
        /// Creates a new instance of <see cref="IdentityTable"/>.
        /// </summary>
        /// <param name="dbConnectionFactory"></param>
        public IdentityTable(IDbConnectionFactory dbConnectionFactory)
        {
            DbConnection = dbConnectionFactory.Create();
        }

        /// <summary>
        /// The type of the database connection class used to access the store.
        /// </summary>
        protected IDbConnection DbConnection { get; }

        /// <inheritdoc/>
        protected override void OnDispose()
        {
            if (DbConnection != null)
            {
                DbConnection.Dispose();
            }
        }
    }
}
