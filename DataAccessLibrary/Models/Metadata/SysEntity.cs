using System;

namespace DataAccessLibrary.Models.Metadata
{
    public class SysEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string NamePlural { get; set; }
        public string DisplayName { get; set; }
        public string DisplayNamePlural { get; set; }
        public string DatabaseTableName { get; set; }
    }
}