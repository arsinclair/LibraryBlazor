using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DataAccessLibrary.Models;
using Dapper;
using System;

namespace DataAccessLibrary.Converters
{
    public static class EntityConverter
    {
        public static List<Entity> Convert(IDataReader reader, string entityName, string connectionString)
        {
            Dictionary<string, string> columnTypes = getColumnTypes(entityName, connectionString);
            var output = new List<Entity>();
            while (reader.Read())
            {
                Entity entity = new Entity(entityName);
                var columns = getColumns(reader).ToList();
                for (int i = 0; i < columns.Count; i++)
                {
                    var type = columnTypes[columns[i]];
                    object attribute;
                    if (!reader.IsDBNull(i))
                    {
                        switch (type)
                        {
                            case "Guid":
                                var parsed = reader.GetGuid(i);
                                if (columns[i] == "Id")
                                {
                                    attribute = parsed;
                                    entity.Id = parsed;
                                }
                                else
                                {
                                    string name = getEntityReferenceTargetText(entityName, columns[i], parsed, connectionString);
                                    attribute = new EntityReference(name, parsed);
                                }
                                break;
                            case "Text":
                                attribute = reader.GetString(i);
                                break;
                            case "Number":
                                attribute = reader.GetInt32(i);
                                break;
                            case "DateTime":
                                attribute = reader.GetDateTime(i);
                                break;
                            case "Boolean":
                                attribute = reader.GetBoolean(i);
                                break;
                            case "TextArea":
                                attribute = reader.GetString(i);
                                break;
                            case "EntityReference":
                                {
                                    var parsedGuid = reader.GetGuid(i);
                                    string name = getEntityReferenceTargetText(entityName, columns[i], parsedGuid, connectionString);
                                    attribute = new EntityReference(entityName, parsedGuid) {
                                        Name = name
                                    };
                                    break;
                                }

                            default: throw new NotImplementedException();
                        }
                        entity[columns[i]] = attribute;
                    }
                }
                output.Add(entity);
            }
            return output;
        }

        private static Dictionary<string, string> getColumnTypes(string entityName, string connectionString)
        {
            string sql = $@"select sf.DatabaseColumnName, sft.Name as TypeName
                            from SysFields sf 
                            inner join SysFieldTypes sft on sf.Type = sft.Id 
                            where ParentEntity = (select Id from SysEntities se where UPPER(Name) = UPPER('{entityName}'))";
            Dictionary<string, string> output = new Dictionary<string, string>();
            using (var connection = new SqlConnection(connectionString))
            {
                var result = connection.Query(sql);
                foreach (var item in result)
                {
                    output.Add(item.DatabaseColumnName.ToString(), item.TypeName.ToString());
                }
            }
            return output;
        }

        private static string getEntityReferenceTargetText(string entityName, string entityReferenceFrom, Guid targetId, string connectionString)
        {
            if (string.IsNullOrEmpty(entityName))
                throw new ArgumentNullException("entityName");
            if (string.IsNullOrEmpty(entityReferenceFrom))
                throw new ArgumentNullException("entityReferenceFrom");
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException("connectionString");
            if (targetId == null || targetId == Guid.Empty)
                throw new ArgumentNullException("targetId");

            string fieldMappingSQL = @$"SELECT sf_to.DatabaseColumnName AS TargetEntityId, se_to.DatabaseTableName AS TargetTableName, sf_at.DatabaseColumnName AS TargetTextField
                            FROM SysFieldMappings sfm
                            INNER JOIN SysFields sf_from ON sfm.SourceField = sf_from.Id
                            INNER JOIN SysFields sf_to ON sfm.TargetField = sf_to.Id
                            INNER JOIN SysFields sf_at ON sfm.SearchAtField = sf_at.Id
                            INNER JOIN SysEntities se_from ON sf_from.ParentEntity = se_from.Id
                            INNER JOIN SysEntities se_to ON sf_to.ParentEntity = se_to.Id
                            WHERE se_from.Name = '{entityName}' AND sf_from.Name = '{entityReferenceFrom}';";

            using (var connection = new SqlConnection(connectionString))
            {
                var result = connection.QuerySingleOrDefault(fieldMappingSQL);
                if (result == null)
                {
                    throw new Exception("Entity Field Mappping is not found");
                }

                string targetTextSQL = $@"SELECT {result.TargetTextField}
                                          FROM {result.TargetTableName}
                                          WHERE {result.TargetEntityId} = '{targetId}';";
                return connection.QuerySingle<string>(targetTextSQL);
            }
        }

        private static IEnumerable<string> getColumns(IDataReader reader)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                yield return reader.GetName(i);
            }
        }
    }
}