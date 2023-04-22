namespace DataAccessLibrary.Query
{
    public sealed class QueryExpression
    {
        public static readonly QueryExpression Empty = new QueryExpression();
        private ColumnSet _columnSet;
        private string _entityName;
        private bool _distinct;
        private PagingInfo _pageInfo;
        private DataCollection<LinkEntity> _linkEntities;
        private FilterExpression _criteria;
        private DataCollection<OrderExpression> _orders;
        private int? _topCount;

        /// <summary>
        ///   <para>Initializes a new instance of the <see cref="T:DataAccessLibrary.Query.QueryExpression" /> class.</para>
        /// </summary>
        public QueryExpression() : this(null)
        {
        }

        /// <summary>
        ///   <para>Initializes a new instance of the <see cref="T:DataAccessLibrary.Query.QueryExpression" /> class setting the entity name.</para>
        /// </summary>
        /// <param name="entityName">
        ///   <para>Type: String. The name of the entity.</para>
        /// </param>
        public QueryExpression(string entityName)
        {
            this._entityName = entityName;
            this._criteria = new FilterExpression();
            this._pageInfo = new PagingInfo();
            this._columnSet = new ColumnSet();
        }

        /// <summary>
        ///   <para>Gets or sets whether the results of the query contain duplicate entity instances.</para>
        /// </summary>
        /// <returns>
        ///   <para>Type: Boolean. true if the results of the query contain duplicate entity instances; otherwise, false.</para>
        /// </returns>
        public bool Distinct
        {
            get => this._distinct;
            set => this._distinct = value;
        }

        /// <summary>
        ///   <para>Gets or sets the number of pages and the number of entity instances per page returned from the query.</para>
        /// </summary>
        /// <returns>
        ///   <para>Type: <see cref="T:DataAccessLibrary.Query.PagingInfo" />The number of pages and the number of entity instances per page returned from the query.</para>
        ///   <para>A query can contain either <see cref="P:DataAccessLibrary.Query.QueryExpression.PageInfo" /> or <see cref="P:DataAccessLibrary.Query.QueryExpression.TopCount" /> property values. If both are specified, an error will be thrown.</para>
        /// </returns>

        public PagingInfo PageInfo
        {
            get => this._pageInfo;
            set => this._pageInfo = value;
        }

        /// <summary>
        ///   <para>Gets a collection of the links between multiple entity types.</para>
        /// </summary>
        /// <returns>
        ///   <para>Type: <see cref="T:DataAccessLibrary.DataCollection`1" />&lt;<see cref="T:DataAccessLibrary.Query.LinkEntity" />&gt;The collection of links between entities for the query.</para>
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
        ///   <para>Gets or sets the complex condition and logical filter expressions that filter the results of the query.</para>
        /// </summary>
        /// <returns>
        ///   <para>Type: <see cref="T:DataAccessLibrary.Query.FilterExpression" />The query condition and filter criteria.</para>
        /// </returns>

        public FilterExpression Criteria
        {
            get => this._criteria;
            set => this._criteria = value;
        }

        /// <summary>
        ///   <para>Gets the order in which the entity instances are returned from the query.</para>
        /// </summary>
        /// <returns>
        ///   <para>Type: <see cref="T:DataAccessLibrary.DataCollection`1" />&lt;<see cref="T:DataAccessLibrary.Query.OrderExpression" />&gt;The order expression that defines the order in which the entity instances are returned from the query.</para>
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

        /// <summary>
        ///   <para>Gets or sets the logical name of the entity.</para>
        /// </summary>
        /// <returns>
        ///         <para>Type: String
        /// The logical name of the primary entity for the query.</para>
        ///       </returns>

        public string EntityName
        {
            get => this._entityName;
            set => this._entityName = value;
        }

        /// <summary>
        ///   <para>Gets or sets the columns to include.</para>
        /// </summary>
        /// <returns>
        ///   <para>Type: <see cref="T:DataAccessLibrary.Query.ColumnSet" />The set of columns to return in the query results.</para>
        /// </returns>

        public ColumnSet ColumnSet
        {
            get => this._columnSet;
            set => this._columnSet = value;
        }

        /// <summary>
        ///   <para>Gets or sets the number of rows to be returned.</para>
        /// </summary>
        /// <returns>
        ///   <para>Type: Int32The number of rows to be returned.</para>
        /// </returns>
        public int? TopCount
        {
            get => this._topCount;
            set => this._topCount = value;
        }

        /// <summary>
        ///   <para>Adds the specified order expression to the query expression.</para>
        /// </summary>
        /// <param name="attributeName">
        ///   <para>Type: String. The name of the attribute.</para>
        /// </param>
        /// <param name="orderType">
        ///   <para>Type: <see cref="T:DataAccessLibrary.Query.OrderType" />. The order type.</para>
        /// </param>
        public void AddOrder(string attributeName, OrderType orderType) => this.Orders.Add(new OrderExpression(attributeName, orderType));

        public void AddOrder(string attributeName, OrderType orderType, string alias, string entityName)
        {
            this.Orders.Add(new OrderExpression(attributeName, orderType, alias, entityName));
        }

        /// <summary>
        ///   <para>Adds the specified link to the query expression setting the entity name to link to, the attribute name to link from and the attribute name to link to.</para>
        /// </summary>
        /// <param name="linkToEntityName">
        ///   <para>Type: String. The name of entity to link from.</para>
        /// </param>
        /// <param name="linkFromAttributeName">
        ///   <para>Type: String. The name of the attribute to link from.</para>
        /// </param>
        /// <param name="linkToAttributeName">
        ///   <para>Type: String. The name of the attribute to link to.</para>
        /// </param>
        /// <returns>
        ///   <para>Type: <see cref="T:DataAccessLibrary.Query.LinkEntity" />The link entity instance created and added to the query expression.</para>
        /// </returns>
        public LinkEntity AddLink(string linkToEntityName, string linkFromAttributeName, string linkToAttributeName)
        {
            return this.AddLink(linkToEntityName, linkFromAttributeName, linkToAttributeName, JoinOperator.Inner);
        }

        /// <summary>
        ///   <para>Adds the specified link to the query expression setting the entity name to link to, the attribute name to link from and the attribute name to link to.</para>
        /// </summary>
        /// <param name="linkToEntityName">
        ///   <para>Type: String. The name of entity to link from.</para>
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
        ///   <para>Type: <see cref="T:DataAccessLibrary.Query.LinkEntity" />The link entity instance created and added to the query expression.</para>
        /// </returns>
        public LinkEntity AddLink(string linkToEntityName, string linkFromAttributeName, string linkToAttributeName, JoinOperator joinOperator)
        {
            LinkEntity linkEntity = new LinkEntity(this.EntityName, linkToEntityName, linkFromAttributeName, linkToAttributeName, joinOperator);
            this.LinkEntities.Add(linkEntity);
            return linkEntity;
        }
    }
}
