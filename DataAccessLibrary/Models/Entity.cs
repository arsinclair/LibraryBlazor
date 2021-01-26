using System.Collections.Generic;
using System;
using System.Globalization;

namespace DataAccessLibrary.Models
{
    public class Entity
    {
        private string _logicalName;
        private Guid _id;
        private AttributeCollection _attributes;
        private EntityState? _entityState;
        private bool _isReadOnly;

        /// <summary>
        ///   <para>Initializes a new instance of the <see cref="T:DataAccessLibrary.Models.Entity" /> class.</para>
        /// </summary>
        /// <remarks />
        public Entity() : this((string)null)
        {
        }

        /// <summary>
        ///     <para>Initializes a new instance of the <see cref="T:DataAccessLibrary.Models.Entity" /> class setting the entity name.</para>
        /// </summary>
        /// <param name="entityName">
        ///     <para>Type: <see href="https://msdn.microsoft.com/library/system.string.aspx">String</see>. The name of the entity.</para>
        /// </param>
        /// <remarks />
        public Entity(string entityName)
        {
            this._logicalName = entityName;
        }

        /// <summary>
        ///   <para>Initializes a new instance of the <see cref="T:DataAccessLibrary.Models.Entity" /> class.</para>
        /// </summary>
        /// <param name="entityName">
        ///   <para>Specifies the entity name.</para>
        /// </param>
        /// <param name="id">
        ///   <para>Specifies the ID of the record.</para>
        /// </param>
        public Entity(string entityName, Guid id)
        {
            this._logicalName = entityName;
            this._id = id;
        }

        /// <summary>
        ///   <para>Provides an indexer for the attribute values.</para>
        /// </summary>
        /// <param name="attributeName">
        ///   <para>Type: <see href="https://msdn.microsoft.com/library/system.string.aspx">String</see>. The logical name of the attribute.</para>
        /// </param>
        /// <returns>
        ///   <para>Type: <see href="https://msdn.microsoft.com/library/system.object.aspx">Object</see>The attribute specified by the <paramref name="attributeName" /> parameter.</para>
        /// </returns>
        /// <remarks />
        public object this[string attributeName]
        {
            get
            {
                return this.Attributes[attributeName];
            }
            set
            {
                this.Attributes[attributeName] = value;
            }
        }

        /// <summary>
        ///     <para>Gets or sets the ID of the record represented by this entity instance.</para>
        /// </summary>
        /// <returns>
        ///     <para>Type: <see href="https://msdn.microsoft.com/library/system.guid.aspx">Guid</see>The ID of the record (entity instance).</para>
        /// </returns>
        /// <remarks />
        public virtual Guid Id
        {
            get
            {
                return this._id;
            }
            set
            {
                if (this._id != Guid.Empty)
                    this.CheckIsReadOnly(nameof(Id));
                this._id = value;
            }
        }

        /// <summary>
        ///     <para>Gets or sets the logical name of the entity.</para>
        /// </summary>
        /// <returns>
        ///     <para>Type: <see href="https://msdn.microsoft.com/library/system.string.aspx">String</see>the logical name of the entity.</para>
        /// </returns>
        /// <remarks />
        public string LogicalName
        {
            get
            {
                return this._logicalName;
            }
            set
            {
                this.CheckIsReadOnly(nameof(LogicalName));
                this._logicalName = value;
            }
        }

        /// <summary>
        ///     <para>Gets or sets the collection of attributes for the entity.</para>
        /// </summary>
        /// <returns>
        ///     <para>Type: <see cref="T:DataAccessLibrary.AttributeCollection" />
        /// The collection of attributes for the entity.</para>
        /// </returns>
        /// <remarks>
        ///     <para>For performance and security reasons, the attribute collection will typically not contain the complete set of attributes for the entity.</para>
        /// </remarks>
        public AttributeCollection Attributes
        {
            get
            {
                if (this._attributes == null)
                    this._attributes = new AttributeCollection();
                return this._attributes;
            }
            set
            {
                this._attributes = value;
            }
        }

        /// <summary>
        ///   <para>Gets or sets the state of the entity.</para>
        /// </summary>
        /// <returns>
        ///         <para>Type: <see cref="T:DataAccessLibrary.EntityState" />.
        /// The state of the entity.</para>
        ///       </returns>
        /// <remarks>
        ///   <para />
        /// </remarks>
        /// <example>
        ///   <code language="c#">lead1.EntityState = EntityState.Changed;</code>
        /// </example>
        public EntityState? EntityState
        {
            get
            {
                return this._entityState;
            }
            set
            {
                this.CheckIsReadOnly(nameof(EntityState));
                this._entityState = value;
            }
        }

        /// <summary>
        ///   <para>Checks to see if there is a value present for the specified attribute.</para>
        /// </summary>
        /// <param name="attributeName">
        ///   <para>Type: <see href="https://msdn.microsoft.com/library/system.string.aspx">String</see>. The logical name of the attribute.</para>
        /// </param>
        /// <returns>
        ///   <para>Type: <see href="https://msdn.microsoft.com/library/system.boolean.aspx">Boolean</see>true if the <see cref="T:DataAccessLibrary.Entity" /> contains an attribute with the specified name; otherwise, false.</para>
        /// </returns>
        /// <remarks />
        public bool Contains(string attributeName)
        {
            return this.Attributes.Contains(attributeName);
        }

        internal bool IsReadOnly
        {
            get
            {
                return this._isReadOnly;
            }
            set
            {
                this._isReadOnly = value;
            }
        }


        private void CheckIsReadOnly(string propertyName)
        {
            if (this.IsReadOnly)
            {
                throw new InvalidOperationException(string.Format((IFormatProvider)CultureInfo.InvariantCulture, "The entity is read-only and the '{0}' property cannot be modified.", (object)propertyName));
            }
        }

        /// <summary>
        ///     <para>Gets an entity reference for this entity instance.</para>
        /// </summary>
        /// <returns>
        ///     <para>Type: <see cref="T:DataAccessLibrary.EntityReference" />The entity reference for the entity.</para>
        /// </returns>
        /// <remarks />
        public EntityReference ToEntityReference()
        {
            return new EntityReference(this.LogicalName, this.Id);
        }

        /// <summary>
        ///     <para>Gets an entity reference for this entity instance.</para>
        /// </summary>
        /// <param name="name">
        ///   <para>Type: <see href="https://msdn.microsoft.com/library/system.string.aspx">String</see>. The name that represents related record in UI elements.</para>
        /// </param>
        /// <returns>
        ///     <para>Type: <see cref="T:DataAccessLibrary.EntityReference" />The entity reference for the entity.</para>
        /// </returns>
        /// <remarks />
        public EntityReference ToEntityReference(string name)
        {
            return new EntityReference(this.LogicalName, this.Id) { Name = name };
        }
    }
}