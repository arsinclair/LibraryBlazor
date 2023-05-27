using System.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using DataAccessLibrary.Converters;
using DataAccessLibrary.Interfaces;
using DataAccessLibrary.Models;
using Microsoft.Extensions.Configuration;
using DataAccessLibrary.Query;
using DataAccessLibrary.Cache;

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

            string tableName = CacheManager.GetDatabaseCache().GetTableName(entity.LogicalName);

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
            else if (value is byte[])
            {
                return "0x" + BitConverter.ToString(value as byte[]).Replace("-", "");
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
                if (value is string)
                {
                    output = "N" + output;
                }
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
            string tableName = CacheManager.GetDatabaseCache().GetTableName(entityName);
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
            string tableName = CacheManager.GetDatabaseCache().GetTableName(entityName);
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

        public int Update(Entity updatedEntity)
        {
            Entity oldEntity = this.GetById(updatedEntity.Id.ToString(), updatedEntity.LogicalName, true);
            if (oldEntity == null)
            {
                throw new Exception("Entity Doesn't exist.");
            }

            List<KeyValuePair<string, object>> updatedAttributes = new List<KeyValuePair<string, object>>();
            foreach (var attributePair in updatedEntity.Attributes)
            {
                string attribute = attributePair.Key;
                if (rectrictedForUpdate(attribute))
                {
                    continue;
                }

                string newValue = stringifyAttribute(attributePair.Value);
                string oldValue = stringifyAttribute(oldEntity[attribute]);

                if (newValue != oldValue)
                {
                    updatedAttributes.Add(attributePair);
                }
            }

            if (updatedAttributes.Count == 0)
            {
                return 0; // No need to update, entities are identical
            }

            updatedAttributes.Add(new KeyValuePair<string, object>("ModifiedOn", DateTime.UtcNow));

            string tableName = CacheManager.GetDatabaseCache().GetTableName(updatedEntity.LogicalName);
            string updatedFieldsSQL = stringifyFieldsForUpdate(updatedAttributes);
            string sql = $"UPDATE {tableName} SET {updatedFieldsSQL} WHERE Id = @Id;";

            using (var connection = new SqlConnection(_configuration.GetConnectionString(DefaultConnection)))
            {
                return connection.Execute(sql, new { Id = updatedEntity.Id.ToString() });
            }
        }

        private string stringifyFieldsForUpdate(List<KeyValuePair<string, object>> updatedAttributes)
        {
            string output = string.Empty;

            int i = 0;
            bool last;
            foreach (var attr in updatedAttributes)
            {
                i++;
                last = updatedAttributes.Count == i;

                if (!isValidAttr(attr))
                {
                    continue;
                }

                output += $"{attr.Key} = {stringifyAttribute(attr.Value)}";

                if (!last)
                {
                    output += ", ";
                }
            }

            return output;
        }

        private bool rectrictedForUpdate(string attribute)
        {
            return new[] { "id", "createdon", "modifiedon" }.Contains(attribute.ToLower());
        }

        #endregion

        private string transformColumns(string[] columns, bool allColumns = false)
        {
            string output;

            if (allColumns || columns.Contains("*"))
            {
                output = "*";
            }
            else
            {
                output = string.Join(", ", columns);

                // Inject must-have columns, e.g. Id
                if (columns.Contains("Id", StringComparer.OrdinalIgnoreCase) == false)
                {
                    output = "Id, " + output;
                }
            }

            return output;
        }

        public IEnumerable<Entity> Get(QueryExpression query)
        {
            string sql = QueryExpressionConverter.ConvertToSQL(query);

            using (var connection = new SqlConnection(_configuration.GetConnectionString(DefaultConnection)))
            {
                using (var reader = connection.ExecuteReader(sql, System.Data.CommandBehavior.SequentialAccess))
                {
                    return EntityConverter.Convert(reader, query.EntityName, _configuration.GetConnectionString(DefaultConnection));
                }
            }
        }

        public Entity GetById(EntityReference entityReference, params string[] columns)
        {
            return GetById(entityReference.Id.ToString(), entityReference.LogicalName, columns);
        }
    }
}