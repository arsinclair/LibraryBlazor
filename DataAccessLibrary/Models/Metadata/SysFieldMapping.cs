using System;

namespace DataAccessLibrary.Models.Metadata
{
    public class SysFieldMapping
    {
        public Guid Id { get; set; }
        public SysField SourceField { get; set; }
        public SysField TargetField { get; set; }
        public SysField SearchAtField { get; set; }
    }
}