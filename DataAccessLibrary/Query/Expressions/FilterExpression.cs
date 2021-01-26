namespace DataAccessLibrary.Query
{
    /// <summary>
    ///   <para>Specifies complex condition and logical filter expressions used for filtering the results of the query.</para>
    /// </summary>
    public sealed class FilterExpression
    {
        private LogicalOperator _filterOperator;
        private string _filterHint;
        private DataCollection<ConditionExpression> _conditions;
        private DataCollection<FilterExpression> _filters;
        private bool _isQuickFindFilter;

        /// <summary>
        ///   <para>Initializes a new instance of the <see cref="T:DataAccessLibrary.Query.FilterExpression" /> class.</para>
        /// </summary>
        public FilterExpression()
        {
        }

        /// <summary>
        ///   <para>Initializes a new instance of the <see cref="T:DataAccessLibrary.Query.FilterExpression" /> class.</para>
        /// </summary>
        /// <param name="filterOperator">
        ///   <para>Type: <see cref="T:DataAccessLibrary.Query.LogicalOperator" />. The filter operator.</para>
        /// </param>
        public FilterExpression(LogicalOperator filterOperator) => this.FilterOperator = filterOperator;

        /// <summary>
        ///   <para>Gets or sets the logical AND/OR filter operator.</para>
        /// </summary>
        /// <returns>
        ///   <para>Type: <see cref="T:DataAccessLibrary.Query.LogicalOperator" />The filter operator.</para>
        /// </returns>
        public LogicalOperator FilterOperator
        {
            get => this._filterOperator;
            set => this._filterOperator = value;
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
        public DataCollection<ConditionExpression> Conditions
        {
            get
            {
                if (this._conditions == null)
                    this._conditions = new DataCollection<ConditionExpression>();
                return this._conditions;
            }
            private set => this._conditions = value;
        }

        /// <summary>
        ///   <para>Gets a collection of condition and logical filter expressions that filter the results of the query.</para>
        /// </summary>
        /// <returns>
        ///   <para>Type: <see cref="T:DataAccessLibrary.DataCollection`1" />&lt;<see cref="T:DataAccessLibrary.Query.FilterExpression" />&gt;The collection of filters.</para>
        /// </returns>
        public DataCollection<FilterExpression> Filters
        {
            get
            {
                if (this._filters == null)
                    this._filters = new DataCollection<FilterExpression>();
                return this._filters;
            }
            private set => this._filters = value;
        }

        /// <summary>
        ///   <para>Gets or sets whether the expression is part of a quick find query.</para>
        /// </summary>
        /// <returns>
        ///   <para>Type: Booleantrue if the filter is part of a quick find query; otherwise, false.</para>
        /// </returns>
        public bool IsQuickFindFilter
        {
            get => this._isQuickFindFilter;
            set => this._isQuickFindFilter = value;
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
        public void AddCondition(
          string attributeName,
          ConditionOperator conditionOperator,
          params object[] values)
        {
            this.Conditions.Add(new ConditionExpression(attributeName, conditionOperator, values));
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
        public void AddCondition(
          string entityName,
          string attributeName,
          ConditionOperator conditionOperator,
          params object[] values)
        {
            this.Conditions.Add(new ConditionExpression(entityName, attributeName, conditionOperator, values));
        }

        /// <param name="attributeName" />
        /// <param name="conditionOperator" />
        /// <param name="compareColumns" />
        /// <param name="values" />
        public void AddCondition(
          string attributeName,
          ConditionOperator conditionOperator,
          bool compareColumns,
          object[] values)
        {
            this.Conditions.Add(new ConditionExpression(attributeName, conditionOperator, compareColumns, values));
        }

        /// <param name="entityName" />
        /// <param name="attributeName" />
        /// <param name="conditionOperator" />
        /// <param name="compareColumns" />
        /// <param name="values" />
        public void AddCondition(
          string entityName,
          string attributeName,
          ConditionOperator conditionOperator,
          bool compareColumns,
          object[] values)
        {
            this.Conditions.Add(new ConditionExpression(entityName, attributeName, conditionOperator, compareColumns, values));
        }

        /// <param name="attributeName" />
        /// <param name="conditionOperator" />
        /// <param name="compareColumns" />
        /// <param name="value" />
        public void AddCondition(
          string attributeName,
          ConditionOperator conditionOperator,
          bool compareColumns,
          object value)
        {
            this.Conditions.Add(new ConditionExpression(attributeName, conditionOperator, (compareColumns ? 1 : 0) != 0, new object[1]
            {
        value
            }));
        }

        /// <param name="entityName" />
        /// <param name="attributeName" />
        /// <param name="conditionOperator" />
        /// <param name="compareColumns" />
        /// <param name="value" />
        public void AddCondition(
          string entityName,
          string attributeName,
          ConditionOperator conditionOperator,
          bool compareColumns,
          object value)
        {
            this.Conditions.Add(new ConditionExpression(entityName, attributeName, conditionOperator, (compareColumns ? 1 : 0) != 0, new object[1]
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
        public void AddCondition(ConditionExpression condition) => this.Conditions.Add(condition);

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
        public FilterExpression AddFilter(LogicalOperator logicalOperator)
        {
            FilterExpression filterExpression = new FilterExpression();
            filterExpression.FilterOperator = logicalOperator;
            this.Filters.Add(filterExpression);
            return filterExpression;
        }

        /// <summary>
        ///   <para>Adds a child filter to the filter expression.</para>
        /// </summary>
        /// <param name="childFilter">
        ///   <para>Type: <see cref="T:DataAccessLibrary.Query.FilterExpression" />. The filter to be added.</para>
        /// </param>
        public void AddFilter(FilterExpression childFilter)
        {
            if (childFilter == null)
                return;
            this.Filters.Add(childFilter);
        }
    }
}
