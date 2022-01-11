using System;
using System.Collections.Generic;
using DataAccessLibrary.Models.Metadata;

namespace DataAccessLibrary.Interfaces
{
    public interface IMetadataRepository
    {
        List<SysEntity> GetEntities();
        List<SysField> GetFields();
        List<SysField> GetFields(string entityName);
        List<SysFieldType> GetFieldTypes();
        SysListLayout GetDefaultListLayout(string entityName);
        List<string> GetEntityCollectionViewColumns(Guid entityCollectionPointerFieldId);
        SysViewLayout GetDefaultViewLayout(string entityName);
        bool IsProductionEnvironment();
    }
}