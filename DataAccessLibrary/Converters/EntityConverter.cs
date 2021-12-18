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
            Dictionary<string, string> columnDefinitions = getColumnDefinitionsFromDB(entityName, connectionString);
            var output = new List<Entity>();
            while (reader.Read())
            {
                Entity entity = new Entity(entityName);
                var columns = getColumns(reader).ToList();
                for (int i = 0; i < columns.Count; i++)
                {
                    string currentColumn = columns[i];
                    var type = columnDefinitions[currentColumn];
                    object attribute;
                    if (!reader.IsDBNull(i))
                    {
                        switch (type)
                        {
                            case "Guid":
                                {
                                    var parsed = reader.GetGuid(i);
                                    if (currentColumn == "Id")
                                    {
                                        entity.Id = parsed;
                                    }
                                    attribute = parsed;
                                    break;
                                }
                            case "Text": attribute = reader.GetString(i); break;
                            case "Number": attribute = reader.GetInt32(i); break;
                            case "DateTime": attribute = reader.GetDateTime(i); break;
                            case "Boolean": attribute = reader.GetBoolean(i); break;
                            case "TextArea": attribute = reader.GetString(i); break;
                            case "EntityReference":
                                {
                                    var parsedGuid = reader.GetGuid(i);
                                    attribute = getTargetEntityReference(entityName, currentColumn, parsedGuid, connectionString);
                                    break;
                                }
                            case "EntityCollection":
                                {
                                    attribute = getRelatedEntities(entityName, currentColumn, (Guid)entity["Id"], connectionString);
                                    break;
                                }
                            case "Binary": attribute = (byte[])reader.GetValue(i); break;
                            default: throw new NotImplementedException();
                        }
                        entity[currentColumn] = attribute;
                    }
                }
                output.Add(entity);
            }
            return output;
        }

        private static List<Entity> getRelatedEntities(string currentEntityName, string currentColumn, Guid currentEntityId, string connectionString)
        {
            // Returns metadata about N:N entity mapping that makes possible querying related entities.
            string metadataSQL = @"select targetentity.DatabaseTableName as TargetEntityTableName, targetentity.Name as TargetEntityName, junctionentity.DatabaseTableName as JunctionTableName, listview.Columns as Columns
                                    from SysEntityMappings mapping
                                    inner join SysFields sourcefield on mapping.SourceEntityFieldId = sourcefield.Id 
                                    inner join SysEntities sourceentity on sourcefield.ParentEntity = sourceentity.Id 
                                    inner join SysEntities targetentity on mapping.TargetEntityId = targetentity.Id 
                                    inner join SysEntities junctionentity on mapping.JunctionEntityId = junctionentity.Id 
                                    inner join SysListLayouts listview on mapping.TargetListId = listview.Id 
                                    where UPPER(sourcefield.Name) = UPPER(@CurrentColumn) and UPPER(sourceentity.Name) = UPPER(@CurrentEntityName)"; // This must be done only via cache in the future

            using (var connection = new SqlConnection(connectionString))
            {
                var result = connection.QuerySingle<EntityMappingResult>(metadataSQL, new { CurrentEntityName = currentEntityName, CurrentColumn = currentColumn });

                // Returns records related to current entity via an N:N relationship
                string relatedEntitiesSQL = @$"select Id, {result.Columns}
                                                from {result.TargetEntityTableName}
                                                where Id in (select EntityBId from {result.JunctionTableName} where EntityAId = '{currentEntityId.ToString()}')";

                using (var reader = connection.ExecuteReader(relatedEntitiesSQL, CommandBehavior.SequentialAccess))
                {
                    return EntityConverter.Convert(reader, result.TargetEntityName, connectionString);
                }
            }
        }

        private static Dictionary<string, string> getColumnDefinitionsFromDB(string entityName, string connectionString)
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

        private static EntityReference getTargetEntityReference(string entityName, string entityReferenceFrom, Guid targetId, string connectionString)
        {
            if (string.IsNullOrEmpty(entityName))
                throw new ArgumentNullException("entityName");
            if (string.IsNullOrEmpty(entityReferenceFrom))
                throw new ArgumentNullException("entityReferenceFrom");
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException("connectionString");
            if (targetId == Guid.Empty)
                throw new ArgumentNullException("targetId");

            string fieldMappingSQL = @$"SELECT sf_to.DatabaseColumnName AS TargetEntityId, se_to.DatabaseTableName AS TargetTableName, se_to.Name as TargetEntityName, sf_at.DatabaseColumnName AS TargetTextField
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
                return new EntityReference(result.TargetEntityName, targetId)
                {
                    Name = connection.QuerySingle<string>(targetTextSQL)
                };
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

    class EntityMappingResult
    {
        public string TargetEntityTableName { get; set; }
        public string TargetEntityName { get; set; }
        public string JunctionTableName { get; set; }
        public string Columns { get; set; }
    }
}