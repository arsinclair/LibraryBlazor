namespace DataAccessLibrary.Query
{
    /// <summary>
    ///   <para>Specifies the links between multiple entity types used in creating complex queries.</para>
    /// </summary>
    public sealed class LinkEntity
    {
        private string _linkFromEntityName;
        private string _linkFromAttributeName;
        private string _linkToEntityName;
        private string _linkToAttributeName;
        private JoinOperator _joinOperator;
        private FilterExpression _linkCriteria;
        private string _entityAlias;
        private ColumnSet _columns;
        private DataCollection<LinkEntity> _linkEntities;
        private DataCollection<OrderExpression> _orders;

        /// <summary>
        ///   <para>Initializes a new instance of the <see cref="T:DataAccessLibrary.Query.LinkEntity" /> class.</para>
        /// </summary>
        public LinkEntity() : this(null, null, null, null, JoinOperator.Inner)
        {
        }

        /// <summary>
        ///   <para>Initializes a new instance of the <see cref="T:DataAccessLibrary.Query.LinkEntity" /> class setting the required properties.</para>
        /// </summary>
        /// <param name="linkFromEntityName">
        ///   <para>Type: String. The logical name of the entity to link from.</para>
        /// </param>
        /// <param name="linkToEntityName">
        ///   <para>Type: String. The logical name of the entity to link to.</para>
        /// </param>
        /// <param name="linkFromAttributeName">
        ///   <para>Type: String. The name of the attribute to link from.</para>
        /// </param>
        /// <param name="linkToAttributeName">
        ///   <para>Type: String. The name of the attribute to link to.</para>
        /// </param>
        /// <param name="joinOperator">
        ///   <para>Type: <see cref="T:DataAccessLibrary.Query.JoinOperator" />. The join operator.</para>
        /// </param>
        public LinkEntity(string linkFromEntityName, string linkToEntityName, string linkFromAttributeName, string linkToAttributeName, JoinOperator joinOperator)
        {
            this._linkFromEntityName = linkFromEntityName;
            this._linkToEntityName = linkToEntityName;
            this._linkFromAttributeName = linkFromAttributeName;
            this._linkToAttributeName = linkToAttributeName;
            this._joinOperator = joinOperator;
            this._columns = new ColumnSet();
            this._linkCriteria = new FilterExpression();
        }

        /// <summary>
        ///   <para>Gets or sets the logical name of the attribute of the entity that you are linking from.</para>
        /// </summary>
        /// <returns>
        ///         <para>Type: String
        /// The logical name of the attribute of the entity that you are linking from.</para>
        ///       </returns>
        public string LinkFromAttributeName
        {
            get => this._linkFromAttributeName;
            set => this._linkFromAttributeName = value;
        }

        /// <summary>
        ///   <para>Gets or sets the logical name of the entity that you are linking from.</para>
        /// </summary>
        /// <returns>
        ///         <para>Type: String
        /// The logical name of the entity that you are linking from.</para>
        ///       </returns>
        public string LinkFromEntityName
        {
            get => this._linkFromEntityName;
            set => this._linkFromEntityName = value;
        }

        /// <summary>
        ///   <para>Gets or sets the logical name of the entity that you are linking to.</para>
        /// </summary>
        /// <returns>
        ///         <para>Type: String
        /// The logical name of the entity that you are linking to.</para>
        ///       </returns>
        public string LinkToEntityName
        {
            get => this._linkToEntityName;
            set => this._linkToEntityName = value;
        }

        /// <summary>
        ///   <para>Gets or sets the logical name of the attribute of the entity that you are linking to.</para>
        /// </summary>
        /// <returns>
        ///         <para>Type: String
        /// The logical name of the attribute of the entity that you are linking to.</para>
        ///       </returns>
        public string LinkToAttributeName
        {
            get => this._linkToAttributeName;
            set => this._linkToAttributeName = value;
        }

        /// <summary>
        ///   <para>Gets or sets the join operator.</para>
        /// </summary>
        /// <returns>
        ///   <para>Type: <see cref="T:DataAccessLibrary.Query.JoinOperator" />The join operator.</para>
        /// </returns>
        public JoinOperator JoinOperator
        {
            get => this._joinOperator;
            set => this._joinOperator = value;
        }

        /// <summary>
        ///   <para>Gets or sets the complex condition and logical filter expressions that filter the results of the query.</para>
        /// </summary>
        /// <returns>
        ///   <para>Type: <see cref="T:DataAccessLibrary.Query.FilterExpression" />The complex condition and logical filter expressions that filter the results of the query.</para>
        /// </returns>
        public FilterExpression LinkCriteria
        {
            get => this._linkCriteria;
            set => this._linkCriteria = value;
        }

        /// <summary>
        ///   <para>Gets the links between multiple entity types.</para>
        /// </summary>
        /// <returns>
        ///   <para>Type: <see cref="T:DataAccessLibrary.DataCollection`1" />&lt;<see cref="T:DataAccessLibrary.Query.LinkEntity" />&gt;The collection of links between entities.</para>
        /// </returns>
        public DataCollection<LinkEntity> LinkEntities
        {
            get
            {
                if (this._linkEntities == null)
                    this._linkEntities = new DataCollection<LinkEntity>();
                return this._linkEntities;
            }
            private set => this._linkEntities = value;
        }

        /// <summary>
        ///   <para>Gets or sets the column set.</para>
        /// </summary>
        /// <returns>
        ///   <para>Type: <see cref="T:DataAccessLibrary.Query.ColumnSet" />The set of columns (fields) to be returned for the query.</para>
        /// </returns>
        public ColumnSet Columns
        {
            get
            {
                if (this._columns == null)
                    this._columns = new ColumnSet();
                return this._columns;
            }
            set => this._columns = value;
        }

        /// <summary>
        ///   <para>Gets or sets the alias for the entity.</para>
        /// </summary>
        /// <returns>
        ///         <para>Type: String
        /// The alias for the entity.</para>
        ///       </returns>
        public string EntityAlias
        {
            get => this._entityAlias;
            set => this._entityAlias = value;
        }

        /// <summary>
        ///   <para>Adds a link, setting the link to entity name, the link from attribute name and the link to attribute name.</para>
        /// </summary>
        /// <param name="linkToEntityName">
        ///   <para>Type: String. The name of the entity to link to.</para>
        /// </param>
        /// <param name="linkFromAttributeName">
        ///   <para>Type: String. The name of the attribute to link from.</para>
        /// </param>
        /// <param name="linkToAttributeName">
        ///   <para>Type: String. The name of the attribute to link to.</para>
        /// </param>
        /// <returns>
        ///   <para>Type: <see cref="T:DataAccessLibrary.Query.LinkEntity" />The link entity that was created.</para>
        /// </returns>
        public LinkEntity AddLink(
          string linkToEntityName,
          string linkFromAttributeName,
          string linkToAttributeName)
        {
            return this.AddLink(linkToEntityName, linkFromAttributeName, linkToAttributeName, JoinOperator.Inner);
        }

        /// <summary>
        ///   <para>Adds a link setting the link to entity name, the link from attribute name, the link to attribute name, and the join operator.</para>
        /// </summary>
        /// <param name="linkToEntityName">
        ///   <para>Type: String. The name of the entity to link to.</para>
        /// </param>
        /// <param name="linkFromAttributeName">
        ///   <para>Type: String. The name of the attribute to link from.</para>
        /// </param>
        /// <param name="linkToAttributeName">
        ///   <para>Type: String. The name of the attribute to link to.</para>
        /// </param>
        /// <param name="joinOperator">
        ///   <para>Type: <see cref="T:DataAccessLibrary.Query.JoinOperator" />. The join operator.</para>
        /// </param>
        /// <returns>
        ///   <para>Type: <see cref="T:DataAccessLibrary.Query.LinkEntity" />The link entity that was created.</para>
        /// </returns>
        public LinkEntity AddLink(
          string linkToEntityName,
          string linkFromAttributeName,
          string linkToAttributeName,
          JoinOperator joinOperator)
        {
            LinkEntity linkEntity = new LinkEntity(this._linkFromEntityName, linkToEntityName, linkFromAttributeName, linkToAttributeName, joinOperator);
            this.LinkEntities.Add(linkEntity);
            return linkEntity;
        }

        /// <summary>
        ///   <para />
        /// </summary>
        /// <returns>
        ///   <para />
        /// </returns>
        public DataCollection<OrderExpression> Orders
        {
            get
            {
                if (this._orders == null)
                    this._orders = new DataCollection<OrderExpression>();
                return this._orders;
            }
            private set => this._orders = value;
        }
    }
}
