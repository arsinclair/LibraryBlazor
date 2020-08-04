using System;

namespace DataAccessLibrary.Models.Metadata
{
    public class SysField
    {
        public Guid Id { get; set; }
        public SysEntity ParentEntity { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string DatabaseColumnName { get; set; }
        public SysFieldType Type { get; set; }
    }
}