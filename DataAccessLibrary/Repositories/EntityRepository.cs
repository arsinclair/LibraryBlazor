using System.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using DataAccessLibrary.Converters;
using DataAccessLibrary.Interfaces;
using DataAccessLibrary.Models;
using Microsoft.Extensions.Configuration;

namespace DataAccessLibrary.Repositories
{
    public class EntityRepository : IEntityRepository
    {
        private readonly IConfiguration _configuration;
        private static string DefaultConnection = "DefaultConnection";

        public EntityRepository(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        #region Core Methods

        public int Create(Entity entity)
        {
            throw new NotImplementedException();
        }

        public int Delete(Entity entity)
        {
            return Delete(entity.Id, entity.EntityName);
        }

        public int Delete(Guid id, string entityName)
        {
            string strId = id.ToString();
            return Delete(strId, entityName);
        }

        public int Delete(string id, string entityName)
        {
            string tableName = this.GetEntityTableName(entityName);
            string sql = $"DELETE FROM @TableName WHERE Id = @Id;";

            using (var connection = new SqlConnection(_configuration.GetConnectionString(DefaultConnection)))
            {
                int affectedRows = connection.Execute(sql, new { TableName = tableName, Id = id });
                return affectedRows;
            }
        }

        public Entity GetById(string id, string entityName, bool allColumns)
        {
            return GetById(id, entityName, "*");
        }

        public Entity GetById(string id, string entityName, params string[] columns)
        {
            string tableName = this.GetEntityTableName(entityName);
            string columnsString = transformColumns(columns);

            string sql = $"SELECT {columnsString} FROM {tableName} WHERE Id = @Id;";

            using (var connection = new SqlConnection(_configuration.GetConnectionString(DefaultConnection)))
            {
                using (var reader = connection.ExecuteReader(sql, new { Id = id }))
                {
                    return EntityConverter.Convert(reader, entityName, _configuration.GetConnectionString(DefaultConnection)).SingleOrDefault();
                }
            }
        }

        public int Update(Entity entity)
        {
            throw new NotImplementedException();
        }

        #endregion

        private string GetEntityTableName(string entityName)
        {
            string sql = "SELECT DatabaseTableName FROM SysEntities WHERE UPPER(Name) = UPPER(@EntityName)";

            using (var connection = new SqlConnection(_configuration.GetConnectionString(DefaultConnection)))
            {
                string tableName = connection.QuerySingle<string>(sql, new { EntityName = entityName });
                return tableName;
            }
        }

        private string transformColumns(string[] columns)
        {
            string output = string.Join(", ", columns);

            // Inject must-have columns, e.g. Id
            if (columns.Contains("Id") == false && columns.First() != "*")
            {
                output = "Id, " + output;
            }

            return output;
        }

        public IEnumerable<Entity> Get(string entityName, string whereClause = "", int count = 0, params string[] columns)
        {
            string tableName = this.GetEntityTableName(entityName);
            string columnsString = transformColumns(columns);
            string rowCount = count > 0 ? $"TOP {count}" : string.Empty;
            string whereString = !string.IsNullOrEmpty(whereClause) ? $"WHERE {whereClause}" : string.Empty;
            string sql = $"SELECT {rowCount} {columnsString} FROM {tableName} {whereString};";

            using (var connection = new SqlConnection(_configuration.GetConnectionString(DefaultConnection)))
            {
                using (var reader = connection.ExecuteReader(sql))
                {
                    return EntityConverter.Convert(reader, entityName, _configuration.GetConnectionString(DefaultConnection));
                }
            }
        }

        public Entity GetById(EntityReference entityReference, params string[] columns)
        {
            return GetById(entityReference.Id.ToString(), entityReference.LogicalName, columns);
        }
    }
}