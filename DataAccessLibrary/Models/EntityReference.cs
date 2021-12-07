using System;

namespace DataAccessLibrary.Models
{
    public class EntityReference
    {
        public EntityReference()
        {
        }

        public EntityReference(string logicalName)
        {
            LogicalName = logicalName;
        }

        public EntityReference(string logicalName, Guid id)
        {
            LogicalName = logicalName;
            Id = id;
        }

        /// <summary>
        /// The ID of the record.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The logical name of the entity.
        /// </summary>
        public string LogicalName { get; set; }

        /// <summary>
        /// This property can contain a value or null. This property is not automatically populated unless the EntityReference object has been retrieved from the server. This value is used to provide text to represent the related record in any user interface elements, such as the value in a view column.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Will return the Name of the Target Entity in the Entity Reference. If the Name is not available, will call base ToString implementation.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (string.IsNullOrEmpty(this.Name) == false)
            {
                return this.Name;
            }

            return base.ToString();
        }

        public bool SameAs(EntityReference entityReference)
        {
            if (entityReference == null) return false;
            return entityReference.Id == this.Id && entityReference.LogicalName.Equals(this.LogicalName, StringComparison.OrdinalIgnoreCase);
        }
    }
}