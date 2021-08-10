using Dapper;
using DataAccessLibrary.Models.Metadata;
using DataAccessLibrary.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace DataAccessLibrary.Cache
{
    public class DatabaseCache
    {
        private readonly IConfiguration _configuration;
        private MetadataRepository metadataRepository;
        private readonly string _connectionString;
        private bool _IsLocked;

        protected Dictionary<Guid, SysFieldType> _FieldTypes;
        protected Dictionary<Guid, SysEntity> _Entities;
        protected Dictionary<Guid, SysField> _Fields;

        // Format: Entity Name - Table Name (Database)
        protected Dictionary<string, string> _TableNameCache;

        // Format: Entity Name - [Field Name - Column Name (Database)]
        protected Dictionary<string, Dictionary<string, SysField>> _FieldsByEntityName;

        /// <summary>
        ///     <para>Initializes a new instance of the <see cref="T:DataAccessLibrary.Cache.DatabaseCache" /> class. The cache collections in the created instance are empty, as the purpose of this constructor is to be used in unit tests.</para>
        /// </summary>
        internal DatabaseCache()
        {
        }

        public DatabaseCache(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
            metadataRepository = new MetadataRepository(configuration);
            Populate();
        }

        protected void Populate()
        {
            _FieldTypes = GetFieldTypes(metadataRepository);
            _Entities = GetEntities(metadataRepository);
            _Fields = GetFields(metadataRepository);
            _TableNameCache = GetEntityTableNames(_connectionString);
            _FieldsByEntityName = GetAllEntityFields();
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
            foreach (SysEntity entity in _Entities.Values)
            {
                if (entityName.ToLower() == entity.Name.ToLower())
                {
                    foreach (SysField field in _Fields.Values)
                    {
                        if (fieldName.ToLower() == field.Name.ToLower())
                        {
                            return field;
                        }
                    }
                }
            }

            // If not found in the cache, try fetch from DB
            string sql = @"SELECT sf.* 
                           FROM SysFields sf 
                           INNER JOIN SysEntities se ON se.Id = sf.ParentEntity 
                           WHERE se.Name = LOWER(@EntityName) AND sf.Name = LOWER(@FieldName);";

            using (var connection = new SqlConnection(_connectionString))
            {
                SysField sysField = connection.QuerySingleOrDefault<SysField>(sql, new { EntityName = entityName, FieldName = fieldName });
                if (sysField == null)
                {
                    throw new Exception($"A field called '{sysField}' for entity '{entityName}' is not found in SysFields.");
                }
                _Fields.Add(sysField.Id, sysField);
                return sysField;
            }
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

        private Dictionary<Guid, SysEntity> GetEntities(MetadataRepository repository)
        {
            return repository.GetEntities().ToDictionary(x => x.Id, x => x);
        }

        private Dictionary<Guid, SysFieldType> GetFieldTypes(MetadataRepository repository)
        {
            return repository.GetFieldTypes().ToDictionary(x => x.Id, x => x);
        }

        private Dictionary<Guid, SysField> GetFields(MetadataRepository repository)
        {
            return repository.GetFields().ToDictionary(x => x.Id, x => x);
        }

        private Dictionary<string, string> GetEntityTableNames(string connectionString)
        {
            string sql = "SELECT LOWER(Name) as EntityName, LOWER(DatabaseTableName) as TableName FROM SysEntities;";

            using (var connection = new SqlConnection(connectionString))
            {
                return connection.Query(sql).ToDictionary(row => (string)row.EntityName, row => (string)row.TableName, StringComparer.OrdinalIgnoreCase);
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

        protected Dictionary<string, Dictionary<string, SysField>> GetAllEntityFields()
        {
            var output = new Dictionary<string, Dictionary<string, SysField>>();
            foreach (SysField field in _Fields.Values)
            {
                if (output.Keys.Contains(field.ParentEntity.Name))
                {
                    output[field.ParentEntity.Name][field.Name] = field;
                }
                else
                {
                    output.Add(field.ParentEntity.Name, new Dictionary<string, SysField>());
                }
            }
            return output;
        }
        #endregion
    }
}
