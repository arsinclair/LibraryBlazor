using Dapper;
using DataAccessLibrary.Models.Metadata;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace DataAccessLibrary.Cache
{
    public class DatabaseCache
    {
        private readonly string _connectionString;
        private bool _IsLocked;

        private Dictionary<Guid, SysFieldType> _FieldTypes;
        private Dictionary<Guid, SysEntity> _Entities;
        private Dictionary<Guid, SysField> _Fields;

        // Format: Entity Name - Table Name (Database)
        private Dictionary<string, string> _TableNameCache;

        // Format: Entity Name - [Field Name - Column Name (Database)]
        private Dictionary<string, Dictionary<string, SysField>> _FieldsByEntityName;

        public DatabaseCache(string connectionString)
        {
            _connectionString = connectionString;
            Populate();
        }

        private void Populate()
        {
            _FieldTypes = GetFieldTypes(_connectionString);
            _Entities = GetEntities(_connectionString);
            _Fields = GetFields(_connectionString);
            _TableNameCache = GetEntityTableNames(_connectionString);
            _FieldsByEntityName = GetFieldsByEntityName(_connectionString);
        }

        public void Refresh()
        {
            if (_IsLocked)
            {
                throw new Exception("Database Cache Lock is set"); // In the future in an Async pipeline, something might be getting hold of the cache: we need to wait until the lock is released and only then refresh it
            }
            Populate();
        }

        public string GetTableName(string entityName)
        {
            string output;
            string _entityName = entityName.ToLower();
            if (_TableNameCache.TryGetValue(_entityName, out output))
            {
                return output;
            }
            else
            {
                var tableName = GetEntityTableName(_entityName, _connectionString);
                _TableNameCache.Add(_entityName, tableName);
                return tableName;
            }
        }

        public SysField GetFieldByName(string fieldName, string entityName)
        {

        }

        #region Routines
        private string GetEntityTableName(string entityName, string connectionString)
        {
            string sql = "SELECT LOWER(Name) as EntityName, LOWER(DatabaseTableName) as TableName FROM SysEntities WHERE LOWER(Name) = LOWER(@EntityName);";

            using (var connection = new SqlConnection(connectionString))
            {
                string tableName = connection.QuerySingleOrDefault<string>(sql, new { EntityName = entityName });
                if (string.IsNullOrEmpty(tableName))
                {
                    throw new Exception($"A table called {tableName} is not found in SysEntities.");
                }
                return tableName;
            }
        }

        private Dictionary<Guid, SysEntity> GetEntities(string connectionString)
        {
            string sql = "SELECT Id, LOWER(Name), NamePlural, DisplayName, DisplayNamePlural, DatabaseTableName FROM SysEntities;";
            using (var connection = new SqlConnection(connectionString))
            {
                return connection.Query<SysEntity>(sql).ToDictionary(x => x.Id, x => x);
            }
        }

        private Dictionary<Guid, SysFieldType> GetFieldTypes(string connectionString)
        {
            string sql = "SELECT Id, LOWER(Name) as Name FROM SysFieldTypes;";
            using (var connection = new SqlConnection(connectionString))
            {
                return connection.Query<SysFieldType>(sql).ToDictionary(x => x.Id, x => x);
            }
        }

        private Dictionary<Guid, SysField> GetFields(string connectionString)
        {
            string sql = "select Id, ParentEntity, LOWER(Name) as Name, DisplayName, DatabaseColumnName, Type from SysFields;";

            Func<SysField, SysField> mapper = (field) =>
            {
                // field.ParentEntity = parentEntity;
                return field;
                throw new NotImplementedException();
            };

            using (var connection = new SqlConnection(connectionString))
            {
                return connection.Query<SysField>(sql, mapper).ToDictionary(x => x.Id, x => x);
            }
            throw new NotImplementedException();
        }

        private Dictionary<string, string> GetEntityTableNames(string connectionString)
        {
            string sql = "SELECT LOWER(Name) as EntityName, LOWER(DatabaseTableName) as TableName FROM SysEntities;";

            using (var connection = new SqlConnection(connectionString))
            {
                return connection.Query(sql).ToDictionary(row => (string)row.EntityName, row => (string)row.TableName);
            }
        }

        private string GetDBColumnName(string fieldName, string entityName, string connectionString)
        {
            string sql = @"SELECT sf.DatabaseColumnName
                        FROM SysFields sf
                        INNER JOIN SysEntities se on se.Id = sf.ParentEntity
                        WHERE se.EntityName = LOWER(@EntityName) AND sf.FieldName = LOWER(@FieldName)";

            using (var connection = new SqlConnection(connectionString))
            {
                string columnName = connection.QuerySingleOrDefault<string>(sql, new { EntityName = entityName, FieldName = fieldName });
                if (string.IsNullOrEmpty(columnName))
                {
                    throw new Exception($"A column called {columnName} for entity {entityName} is not found in SysFields.");
                }
                return columnName;
            }
        }

        private Dictionary<string, Dictionary<string, SysField>> GetFieldsByEntityName(string connectionString)
        {
            var output = new Dictionary<string, Dictionary<string, SysField>>();
            foreach (SysField field in _Fields.Values)
            {
                output[field.ParentEntity.Name][field.Name] = field;
            }
            return output;
        }
        #endregion
    }
}
