namespace DataAccessLibrary.Query
{
    /// <summary>
    ///   <para>Specifies complex condition and logical filter expressions used for filtering the results of the query.</para>
    /// </summary>
    public sealed class FilterExpression
    {
        private LogicalOperator _queryOperator;
        private string _filterHint;
        private DataCollection<ConditionExpression> _queries;
        private DataCollection<FilterExpression> _subQueries;

        /// <summary>
        ///   <para>Initializes a new instance of the <see cref="T:DataAccessLibrary.Query.FilterExpression" /> class.</para>
        /// </summary>
        public FilterExpression()
        {
        }

        /// <summary>
        ///   <para>Initializes a new instance of the <see cref="T:DataAccessLibrary.Query.FilterExpression" /> class.</para>
        /// </summary>
        /// <param name="queryOperator">
        ///   <para>Type: <see cref="T:DataAccessLibrary.Query.LogicalOperator" />. The filter operator.</para>
        /// </param>
        public FilterExpression(LogicalOperator queryOperator) => this.QueryOperator = queryOperator;

        /// <summary>
        ///   <para>Gets or sets the logical AND/OR filter operator.</para>
        /// </summary>
        /// <returns>
        ///   <para>Type: <see cref="T:DataAccessLibrary.Query.LogicalOperator" />The filter operator.</para>
        /// </returns>
        public LogicalOperator QueryOperator
        {
            get => this._queryOperator;
            set => this._queryOperator = value;
        }

        public string FilterHint
        {
            get => this._filterHint;
            set => this._filterHint = value;
        }

        /// <summary>
        ///   <para>Gets condition expressions that include attributes, condition operators, and attribute values.</para>
        /// </summary>
        /// <returns>
        ///         <para>Type: <see cref="T:DataAccessLibrary.DataCollection`1" />&lt;<see cref="T:DataAccessLibrary.Query.ConditionExpression" />&gt;
        /// The collection of condition expressions.</para>
        ///       </returns>
        public DataCollection<ConditionExpression> Queries
        {
            get
            {
                if (this._queries == null)
                    this._queries = new DataCollection<ConditionExpression>();
                return this._queries;
            }
            private set => this._queries = value;
        }

        /// <summary>
        ///   <para>Gets a collection of condition expressions that represent the subquery of a query. SubQueries are used to build complex queries that include multiple OR and AND operations.</para>
        /// </summary>
        /// <returns>
        ///   <para>Type: <see cref="T:DataAccessLibrary.DataCollection`1" />&lt;<see cref="T:DataAccessLibrary.Query.FilterExpression" />&gt;The collection of sub-queries.</para>
        /// </returns>
        public DataCollection<FilterExpression> SubQueries
        {
            get
            {
                if (this._subQueries == null)
                    this._subQueries = new DataCollection<FilterExpression>();
                return this._subQueries;
            }
            private set => this._subQueries = value;
        }

        /// <summary>
        ///   <para>Adds a condition to the filter expression setting the attribute name, condition operator, and value array.</para>
        /// </summary>
        /// <param name="attributeName">
        ///   <para>Type: String. The name of the attribute.</para>
        /// </param>
        /// <param name="conditionOperator">
        ///   <para>Type: <see cref="T:DataAccessLibrary.Query.ConditionOperator" />. The enumeration type.</para>
        /// </param>
        /// <param name="values">
        ///   <para>Type: Object[]. The array of values to add.</para>
        /// </param>
        public void AddQuery(
          string attributeName,
          ConditionOperator conditionOperator,
          params object[] values)
        {
            this.Queries.Add(new ConditionExpression(attributeName, conditionOperator, values));
        }

        /// <summary>
        ///   <para>Adds a condition to the filter expression setting the entity name, attribute name, condition operator, and value array.</para>
        /// </summary>
        /// <param name="entityName">
        ///   <para>Type: String. The name of the entity.</para>
        /// </param>
        /// <param name="attributeName">
        ///   <para>Type: String. The name of the attribute.</para>
        /// </param>
        /// <param name="conditionOperator">
        ///   <para>Type: <see cref="T:DataAccessLibrary.Query.ConditionOperator" />. The enumeration type.</para>
        /// </param>
        /// <param name="values">
        ///   <para>Type: Object[]. The array of values to add.</para>
        /// </param>
        public void AddQuery(
          string entityName,
          string attributeName,
          ConditionOperator conditionOperator,
          params object[] values)
        {
            this.Queries.Add(new ConditionExpression(entityName, attributeName, conditionOperator, values));
        }

        /// <param name="attributeName" />
        /// <param name="conditionOperator" />
        /// <param name="compareColumns" />
        /// <param name="values" />
        public void AddQuery(
          string attributeName,
          ConditionOperator conditionOperator,
          bool compareColumns,
          object[] values)
        {
            this.Queries.Add(new ConditionExpression(attributeName, conditionOperator, compareColumns, values));
        }

        /// <param name="entityName" />
        /// <param name="attributeName" />
        /// <param name="conditionOperator" />
        /// <param name="compareColumns" />
        /// <param name="values" />
        public void AddQuery(
          string entityName,
          string attributeName,
          ConditionOperator conditionOperator,
          bool compareColumns,
          object[] values)
        {
            this.Queries.Add(new ConditionExpression(entityName, attributeName, conditionOperator, compareColumns, values));
        }

        /// <param name="attributeName" />
        /// <param name="conditionOperator" />
        /// <param name="compareColumns" />
        /// <param name="value" />
        public void AddQuery(
          string attributeName,
          ConditionOperator conditionOperator,
          bool compareColumns,
          object value)
        {
            this.Queries.Add(new ConditionExpression(attributeName, conditionOperator, (compareColumns ? 1 : 0) != 0, new object[1]
            {
        value
            }));
        }

        /// <param name="entityName" />
        /// <param name="attributeName" />
        /// <param name="conditionOperator" />
        /// <param name="compareColumns" />
        /// <param name="value" />
        public void AddQuery(
          string entityName,
          string attributeName,
          ConditionOperator conditionOperator,
          bool compareColumns,
          object value)
        {
            this.Queries.Add(new ConditionExpression(entityName, attributeName, conditionOperator, (compareColumns ? 1 : 0) != 0, new object[1]
            {
        value
            }));
        }

        /// <summary>
        ///   <para>Adds a condition to the filter expression setting the condition expression.</para>
        /// </summary>
        /// <param name="condition">
        ///   <para>Type: <see cref="T:DataAccessLibrary.Query.ConditionExpression" />. Specifies the condition expression to be added.</para>
        /// </param>
        public void AddQuery(ConditionExpression condition) => this.Queries.Add(condition);

        /// <summary>
        ///   <para>Adds a child filter to the filter expression setting the logical operator.</para>
        /// </summary>
        /// <param name="logicalOperator">
        ///   <para>Type: <see cref="T:DataAccessLibrary.Query.LogicalOperator" />. The enumeration type.</para>
        /// </param>
        /// <returns>
        ///         <para>Type: <see cref="T:DataAccessLibrary.Query.FilterExpression" />
        /// The new filter expression.</para>
        ///       </returns>
        public FilterExpression AddSubQuery(LogicalOperator logicalOperator)
        {
            FilterExpression filterExpression = new FilterExpression();
            filterExpression.QueryOperator = logicalOperator;
            this.SubQueries.Add(filterExpression);
            return filterExpression;
        }

        /// <summary>
        ///   <para>Adds a child filter to the filter expression.</para>
        /// </summary>
        /// <param name="subQuery">
        ///   <para>Type: <see cref="T:DataAccessLibrary.Query.FilterExpression" />. The filter to be added.</para>
        /// </param>
        public void AddSubQuery(FilterExpression subQuery)
        {
            if (subQuery == null)
                return;
            this.SubQueries.Add(subQuery);
        }
    }
}
