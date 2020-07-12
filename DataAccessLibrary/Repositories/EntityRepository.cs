using System.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
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

        public Task<int> Create(Entity entity)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Delete(Entity entity)
        {
            return await Delete(entity.Id, entity.EntityName);
        }

        public async Task<int> Delete(Guid id, string entityName)
        {
            string strId = id.ToString();
            return await Delete(strId, entityName);
        }

        public async Task<int> Delete(string id, string entityName)
        {
            string tableName = await this.GetEntityTableName(entityName);
            string sql = $"DELETE FROM @TableName WHERE Id = @Id;";

            using (var connection = new SqlConnection(_configuration.GetConnectionString(DefaultConnection)))
            {
                int affectedRows = await connection.ExecuteAsync(sql, new { TableName = tableName, Id = id });
                return affectedRows;
            }
        }

        public async Task<Entity> GetById(string id, string entityName, bool allColumns)
        {
            return await GetById(id, entityName, "*");
        }

        public async Task<Entity> GetById(string id, string entityName, params string[] columns)
        {
            string tableName = await this.GetEntityTableName(entityName);
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

        public async Task<IEnumerable<Entity>> GetAll(string entityName, bool allColumns)
        {
            return await GetAll(entityName, "*");
        }

        public async Task<IEnumerable<Entity>> GetAll(string entityName, params string[] columns)
        {
            string columnsString = string.Join(", ", columns);
            return await GetAll(entityName, columnsString);
        }

        public async Task<IEnumerable<Entity>> GetAll(string entityName, string columns)
        {
            string tableName = await this.GetEntityTableName(entityName);
            string sql = $"SELECT {columns} FROM {tableName};";

            using (var connection = new SqlConnection(_configuration.GetConnectionString(DefaultConnection)))
            {
                using (var reader = connection.ExecuteReader(sql))
                {
                    return EntityConverter.Convert(reader, entityName, _configuration.GetConnectionString(DefaultConnection));
                }
            }
        }

        public Task<int> Update(Entity entity)
        {
            throw new NotImplementedException();
        }

        #endregion

        private async Task<string> GetEntityTableName(string entityName)
        {
            string sql = "SELECT DatabaseTableName FROM SysEntities WHERE UPPER(Name) = UPPER(@EntityName)";

            using (var connection = new SqlConnection(_configuration.GetConnectionString(DefaultConnection)))
            {
                string tableName = await connection.QuerySingleAsync<string>(sql, new { EntityName = entityName });
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

        public async Task<IEnumerable<Entity>> Get(string entityName, string whereClause, params string[] columns)
        {
            string tableName = await this.GetEntityTableName(entityName);
            string columnsString = transformColumns(columns);

            string sql = $"SELECT {columnsString} FROM {tableName} WHERE {whereClause};";

            using (var connection = new SqlConnection(_configuration.GetConnectionString(DefaultConnection)))
            {
                using (var reader = connection.ExecuteReader(sql))
                {
                    return EntityConverter.Convert(reader, entityName, _configuration.GetConnectionString(DefaultConnection));
                }
            }
        }

        public async Task<Entity> GetById(EntityReference entityReference, params string[] columns)
        {
            return await GetById(entityReference.Id.ToString(), entityReference.LogicalName, columns);
        }
    }
}