using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using DataAccessLibrary.Interfaces;
using DataAccessLibrary.Models.Metadata;
using Microsoft.Extensions.Configuration;

namespace DataAccessLibrary.Repositories
{
    public class MetadataRepository : IMetadataRepository
    {
        private readonly IConfiguration _configuration;
        private static string DefaultConnection = "DefaultConnection";

        public MetadataRepository(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public List<SysEntity> GetEntities()
        {
            string sql = "SELECT Id, LOWER(Name) as Name, NamePlural, DisplayName, DisplayNamePlural, DatabaseTableName FROM SysEntities;";
            using (var connection = new SqlConnection(_configuration.GetConnectionString(DefaultConnection)))
            {
                return connection.Query<SysEntity>(sql).ToList();
            }
        }

        public List<SysFieldType> GetFieldTypes()
        {
            string sql = "SELECT Id, LOWER(Name) as Name FROM SysFieldTypes;";
            using (var connection = new SqlConnection(_configuration.GetConnectionString(DefaultConnection)))
            {
                return connection.Query<SysFieldType>(sql).ToList();
            }
        }

        public List<SysField> GetFields()
        {
            string sql = @"SELECT F.Id, LOWER(F.Name) as Name, F.DisplayName, F.DatabaseColumnName, F.ParentEntity as ParentEntityId, E.Id, LOWER(E.Name) as Name, E.NamePlural, E.DisplayName, E.DisplayNamePlural, E.DatabaseTableName, F.Type as ParentEntityType, T.Id, T.Name FROM SysFields AS F 
            INNER JOIN SysEntities E ON F.ParentEntity = E.Id
            INNER JOIN SysFieldTypes T ON F.Type = T.Id;";
            using (var connection = new SqlConnection(_configuration.GetConnectionString(DefaultConnection)))
            {
                Func<SysField, SysEntity, SysFieldType, SysField> mapper = (field, parentEntity, fieldType) =>
                {
                    field.ParentEntity = parentEntity;
                    field.Type = fieldType;
                    return field;
                };

                return connection.Query<SysField, SysEntity, SysFieldType, SysField>(sql, mapper, splitOn: "ParentEntityId,ParentEntityType").ToList();
            }
        }

        private Dictionary<Guid, SysFieldMapping> getMappedFields(IEnumerable<Guid> ids)
        {
            if (ids == null || !ids.Any())
            {
                throw new ArgumentNullException("ids");
            }

            IEnumerable<SysFieldMapping> mappings;
            string mappingsSql = $@"SELECT MAP.Id, MAP.SourceField AS SourceField, SOURCEFIELD.Id, MAP.TargetField AS TargetField, TARGETFIELD.Id, MAP.SearchAtField AS SearchAtField, SEARCHATFIELD.Id
            FROM SysFieldMappings MAP
            INNER JOIN SysFields SOURCEFIELD on MAP.SourceField = SOURCEFIELD.Id
            INNER JOIN SysFields TARGETFIELD on MAP.TargetField = TARGETFIELD.Id
            INNER JOIN SysFields SEARCHATFIELD on MAP.SearchAtField = SEARCHATFIELD.Id
            WHERE MAP.SourceField IN ('{string.Join("', '", ids)}');";

            using (var connection = new SqlConnection(_configuration.GetConnectionString(DefaultConnection)))
            {
                Func<SysFieldMapping, SysField, SysField, SysField, SysFieldMapping> mapper = (mapping, sourceField, targetField, searchAtField) =>
                {
                    mapping.SourceField = sourceField;
                    mapping.TargetField = targetField;
                    mapping.SearchAtField = searchAtField;
                    return mapping;
                };

                mappings = connection.Query<SysFieldMapping, SysField, SysField, SysField, SysFieldMapping>(mappingsSql, mapper, splitOn: "SourceField,TargetField,SearchAtField");
            }

            IEnumerable<SysField> fields;
            IEnumerable<Guid> fieldsToSelect = mappings.SelectMany(x => new[] { x.TargetField.Id, x.SearchAtField.Id });
            string sql = $@"SELECT F.Id, F.Name, F.DisplayName, F.DatabaseColumnName, F.ParentEntity as ParentEntityId, E.Id, E.Name, E.NamePlural, E.DisplayName, E.DisplayNamePlural, E.DatabaseTableName, F.Type as ParentEntityType, T.Id, T.Name
            FROM SysFields F
            INNER JOIN SysEntities E ON F.ParentEntity = E.Id
            INNER JOIN SysFieldTypes T ON F.Type = T.Id
            WHERE F.Id IN ('{string.Join("', '", fieldsToSelect)}');";

            using (var connection = new SqlConnection(_configuration.GetConnectionString(DefaultConnection)))
            {
                Func<SysField, SysEntity, SysFieldType, SysField> mapper = (field, parentEntity, fieldType) =>
                {
                    field.ParentEntity = parentEntity;
                    field.Type = fieldType;
                    return field;
                };

                fields = connection.Query<SysField, SysEntity, SysFieldType, SysField>(sql, mapper, splitOn: "ParentEntityId,ParentEntityType");
            }

            foreach (var mapping in mappings)
            {
                mapping.TargetField = fields.Single(x => x.Id == mapping.TargetField.Id);
                mapping.SearchAtField = fields.Single(x => x.Id == mapping.SearchAtField.Id);
            }
            return mappings.ToDictionary(x => x.SourceField.Id);
        }

        private IEnumerable<SysField> populateFieldMappings(IEnumerable<SysField> fields)
        {
            var entityReferences = fields.Where(x => x.Type.Name == "EntityReference");
            if (entityReferences.Any())
            {
                var mappings = getMappedFields(entityReferences.Select(x => x.Id));
                foreach (var field in fields)
                {
                    if (entityReferences.Contains(field) && mappings.ContainsKey(field.Id))
                    {
                        field.ReferenceTarget = mappings[field.Id];
                    }
                }
            }
            return fields;
        }

        public List<SysField> GetFields(string entityName)
        {
            string sql = $@"SELECT F.Id, F.Name, F.DisplayName, F.DatabaseColumnName, F.ParentEntity as ParentEntityId, E.Id, E.Name, E.NamePlural, E.DisplayName, E.DisplayNamePlural, E.DatabaseTableName, F.Type as ParentEntityType, T.Id, T.Name FROM SysFields AS F 
            INNER JOIN SysEntities E ON F.ParentEntity = E.Id
            INNER JOIN SysFieldTypes T ON F.Type = T.Id
            WHERE UPPER(E.Name) = UPPER('{entityName}');";
            using (var connection = new SqlConnection(_configuration.GetConnectionString(DefaultConnection)))
            {
                Func<SysField, SysEntity, SysFieldType, SysField> mapper = (field, parentEntity, fieldType) =>
                {
                    field.ParentEntity = parentEntity;
                    field.Type = fieldType;
                    return field;
                };

                return populateFieldMappings(connection.Query<SysField, SysEntity, SysFieldType, SysField>(sql, mapper, splitOn: "ParentEntityId,ParentEntityType")).ToList();
            }
        }

        public SysListLayout GetDefaultListLayout(string entityName)
        {
            string sql =
            $@"SELECT L.Id, L.Name, L.IsDefault, L.CreatedOn, L.ModifiedOn, L.LayoutXml, L.FetchXml, L.Columns, L.ParentEntity as ParentEntityId, E.Id, E.Name, E.NamePlural, E.DisplayName, E.DisplayNamePlural, E.DatabaseTableName
            FROM SysListLayouts AS L 
            INNER JOIN SysEntities E ON L.ParentEntity = E.Id
            WHERE UPPER(E.Name) = UPPER('{entityName}') AND IsDefault = 1;";
            using (var connection = new SqlConnection(_configuration.GetConnectionString(DefaultConnection)))
            {
                Func<SysListLayout, SysEntity, SysListLayout> mapper = (field, parentEntity) =>
                {
                    field.ParentEntity = parentEntity;
                    return field;
                };

                return connection.Query<SysListLayout, SysEntity, SysListLayout>(sql, mapper, splitOn: "ParentEntityId").FirstOrDefault();
            }
        }

        public List<string> GetEntityCollectionViewColumns(Guid entityCollectionPointerFieldId)
        {
            string sql = $@"SELECT L.Columns 
                        FROM SysListLayouts AS L 
                        INNER JOIN SysEntityMappings entityMapping on entityMapping.TargetListId = L.Id
                        WHERE entityMapping.SourceEntityFieldId = @EntityCollectionPointerFieldId;";
            using (var connection = new SqlConnection(_configuration.GetConnectionString(DefaultConnection)))
            {
                string columns = connection.Query<string>(sql, new { EntityCollectionPointerFieldId = entityCollectionPointerFieldId }).SingleOrDefault();
                return columns.Split(",").ToList();
            }
        }

        public SysViewLayout GetDefaultViewLayout(string entityName)
        {
            string sql =
            $@"SELECT L.Id, L.Name, L.IsDefault, L.CreatedOn, L.ModifiedOn, L.Columns, L.ParentEntity as ParentEntityId, E.Id, E.Name, E.NamePlural, E.DisplayName, E.DisplayNamePlural, E.DatabaseTableName
            FROM SysViewLayouts AS L 
            INNER JOIN SysEntities E ON L.ParentEntity = E.Id
            WHERE UPPER(E.Name) = UPPER('{entityName}') AND IsDefault = 1;";
            using (var connection = new SqlConnection(_configuration.GetConnectionString(DefaultConnection)))
            {
                Func<SysViewLayout, SysEntity, SysViewLayout> mapper = (field, parentEntity) =>
                {
                    field.ParentEntity = parentEntity;
                    return field;
                };

                return connection.Query<SysViewLayout, SysEntity, SysViewLayout>(sql, mapper, splitOn: "ParentEntityId").FirstOrDefault();
            }
        }

        public bool IsProductionEnvironment()
        {
            string sql = "SELECT Value FROM SysSettings WHERE Name = 'Environment';";
            using (var connection = new SqlConnection(_configuration.GetConnectionString(DefaultConnection)))
            {
                return connection.QuerySingleOrDefault<string>(sql) == "PROD";
            }
        }
    }
}