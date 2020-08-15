using System;

namespace DataAccessLibrary.Models.Metadata
{
    public class SysViewLayout
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public SysEntity ParentEntity { get; set; }
        public string Columns { get; set; }
    }
}