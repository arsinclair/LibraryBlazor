using System.Collections.Generic;
using System;

namespace DataAccessLibrary.Models
{
    public class Entity
    {
        public Entity()
        {
        }

        public Field this[string fieldName]
        {
            get
            {
                return this.Fields[fieldName];
            }
            set
            {
                this.Fields[fieldName] = value;
            }
        }

        public Guid Id { get; set; }
        public string EntityName { get; set; }
        public Dictionary<string, Field> Fields { get; set; }

        public EntityReference ToEntityReference()
        {
            return new EntityReference(this.EntityName, this.Id);
        }
        
        public EntityReference ToEntityReference(string objectNameField)
        {
            return new EntityReference()
            {
                Id = this.Id,
                LogicalName = this.EntityName,
                Name = this[objectNameField]?.Value.ToString()
            };
        }
    }
}