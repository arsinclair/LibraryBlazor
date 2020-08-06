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
            string sql = "SELECT * FROM SysEntities;";
            using (var connection = new SqlConnection(_configuration.GetConnectionString(DefaultConnection)))
            {
                return connection.Query<SysEntity>(sql).ToList();
            }
        }

        public List<SysFieldType> GetFieldTypes()
        {
            string sql = "SELECT * FROM SysFieldTypes;";
            using (var connection = new SqlConnection(_configuration.GetConnectionString(DefaultConnection)))
            {
                return connection.Query<SysFieldType>(sql).ToList();
            }
        }

        public List<SysField> GetFields()
        {
            string sql = @"SELECT F.Id, F.Name, F.DisplayName, F.DatabaseColumnName, F.ParentEntity as ParentEntityId, E.Id, E.Name, E.NamePlural, E.DisplayName, E.DisplayNamePlural, E.DatabaseTableName, F.Type as ParentEntityType, T.Id, T.Name FROM SysFields AS F 
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

                return connection.Query<SysField, SysEntity, SysFieldType, SysField>(sql, mapper, splitOn: "ParentEntityId,ParentEntityType").ToList();
            }
        }

        public SysLayoutList GetDefaultListLayout(string entityName)
        {
            string sql = 
            $@"SELECT L.Id, L.Name, L.IsDefault, L.CreatedOn, L.ModifiedOn, L.LayoutXml, L.FetchXml, L.Columns, L.ParentEntity as ParentEntityId, E.Id, E.Name, E.NamePlural, E.DisplayName, E.DisplayNamePlural, E.DatabaseTableName
            FROM SysLayoutList AS L 
            INNER JOIN SysEntities E ON L.ParentEntity = E.Id
            WHERE UPPER(E.Name) = UPPER('{entityName}') AND IsDefault = 1;";
            using (var connection = new SqlConnection(_configuration.GetConnectionString(DefaultConnection)))
            {
                Func<SysLayoutList, SysEntity, SysLayoutList> mapper = (field, parentEntity) =>
                {
                    field.ParentEntity = parentEntity;
                    return field;
                };

                return connection.Query<SysLayoutList, SysEntity, SysLayoutList>(sql, mapper, splitOn: "ParentEntityId").FirstOrDefault();
            }
        }
    }
}