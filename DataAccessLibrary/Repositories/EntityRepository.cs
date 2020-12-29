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

        public Guid Create(Entity entity)
        {
            if (string.IsNullOrEmpty(entity.LogicalName))
            {
                throw new ArgumentNullException(entity.LogicalName);
            }

            string tableName = GetEntityTableName(entity.LogicalName);

            entity.Id = Guid.NewGuid();
            entity["Id"] = entity.Id;
            entity["CreatedOn"] = DateTime.UtcNow;
            entity["ModifiedOn"] = DateTime.UtcNow;

            string fieldsToInsert = stringifyFieldsForInsert(entity);
            string sql = $"INSERT INTO {tableName} {fieldsToInsert};";

            using (var connection = new SqlConnection(_configuration.GetConnectionString(DefaultConnection)))
            {
                var rowsAffected = connection.Execute(sql);
                return entity.Id;
            }
        }

        private string stringifyFieldsForInsert(Entity entity)
        {
            string fieldNames = string.Empty;
            string fieldValues = string.Empty;

            int i = 0;
            bool last = false;
            foreach (var attr in entity.Attributes)
            {
                i++;
                last = entity.Attributes.Count == i;

                if (!isValidAttr(attr))
                {
                    continue;
                }

                fieldNames += attr.Key;
                fieldValues += stringifyAttribute(attr.Value);

                if (!last)
                {
                    fieldNames += ", ";
                    fieldValues += ", ";
                }
            }
            string output = $"({fieldNames}) VALUES ({fieldValues})";
            return output;
        }

        private string stringifyAttribute(object value)
        {
            string output = string.Empty;
            if (value is Guid && value.ToString() == Guid.Empty.ToString())
            {
                output = "";
            }
            else if (value is string)
            {
                output = value.ToString().Replace("'", "''"); // SQL Escape single quote
            }
            else if (value is EntityReference)
            {
                output = ((EntityReference)value).Id.ToString();
                if (output == Guid.Empty.ToString())
                {
                    output = "";
                }
            }
            else if (value is DateTime)
            {
                output = ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss");
            }
            else if (value == null)
            {
                output = string.Empty;
            }
            else
            {
                output = value.ToString();
            }

            if (output == string.Empty)
            {
                output = "NULL";
            }
            else
            {
                output = "'" + output + "'";
            }
            return output;
        }

        private bool isValidAttr(KeyValuePair<string, object> attr)
        {
            bool isValid = true;

            // This check doesn't make any sense. However we might want to introduce other field checks here.
            //if (attr.Value is DateTime && (DateTime)attr.Value < new DateTime(1900, 1, 1))
            //{
            //    isValid = false;
            //}

            return isValid;
        }

        public int Delete(Entity entity)
        {
            return Delete(entity.Id, entity.LogicalName);
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