using System.Runtime.CompilerServices;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DataAccessLibrary.Models;
using Dapper;
using System;
using Microsoft.CSharp;

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
                Entity entity = new Entity();
                entity.Fields = new Dictionary<string, Field>();
                entity.EntityName = entityName;
                var columns = getColumns(reader).ToList();
                for (int i = 0; i < columns.Count; i++)
                {
                    var type = columnTypes[columns[i]];
                    Field field = new Field();
                    if (!reader.IsDBNull(i))
                    {
                        switch (type)
                        {
                            case "GUID":
                                var parsed = reader.GetGuid(i);
                                if (columns[i] == "Id")
                                {
                                    field.Value = parsed;
                                    entity.Id = parsed;
                                }
                                else
                                {
                                    field.Value = new EntityReference(string.Empty, parsed); // TODO
                                }
                                break;
                            case "Text":
                                field.Value = reader.GetString(i);
                                break;
                            case "Number":
                                field.Value = reader.GetInt32(i);
                                break;
                            case "DateTime":
                                field.Value = reader.GetDateTime(i);
                                break;
                            default: throw new NotImplementedException();
                        }
                        entity[columns[i]] = field;
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

        private static IEnumerable<string> getColumns(IDataReader reader)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                yield return reader.GetName(i);
            }
        }
    }
}