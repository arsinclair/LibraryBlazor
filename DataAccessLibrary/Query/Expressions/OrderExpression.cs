﻿namespace DataAccessLibrary.Query
{
    /// <summary>
    ///   <para>Sets the order in which the entity instances are returned from the query.</para>
    /// </summary>
    public class OrderExpression
    {
        private string _attributeName;
        private string _fallbackAttributeName;
        private OrderType _orderType;
        private string _alias;
        private string _entityName;

        /// <summary>
        ///   <para>Initializes a new instance of the <see cref="T:DataAccessLibrary.Query.OrderExpression" /> class.</para>
        /// </summary>
        public OrderExpression()
        {
        }

        /// <summary>
        ///   <para>Initializes a new instance of the <see cref="T:DataAccessLibrary.Query.OrderExpression" /> class setting the attribute name and the order type.</para>
        /// </summary>
        /// <param name="attributeName">
        ///   <para>Type: String. The name of the attribute.</para>
        /// </param>
        /// <param name="orderType">
        ///   <para>Type: <see cref="T:DataAccessLibrary.Query.OrderType" />. The order, ascending or descending.</para>
        /// </param>
        public OrderExpression(string attributeName, OrderType orderType)
        {
            this._attributeName = attributeName;
            this._orderType = orderType;
        }

        /// <summary>
        ///   <para>Initializes a new instance of the <see cref="T:DataAccessLibrary.Query.OrderExpression" /> class setting the attribute name, the fallback attribute name and the order type.</para>
        /// </summary>
        /// <param name="attributeName">
        ///   <para>Type: String. The name of the attribute.</para>
        /// </param>
        /// <param name="fallbackAttributeName">
        ///   <para>Type: String. The fallback attribute name that will be used if the value of the attributeName is null.</para>
        /// </param>
        /// <param name="orderType">
        ///   <para>Type: <see cref="T:DataAccessLibrary.Query.OrderType" />. The order, ascending or descending.</para>
        /// </param>
        public OrderExpression(string attributeName, string fallbackAttributeName, OrderType orderType)
        {
            this._attributeName = attributeName;
            this._fallbackAttributeName = fallbackAttributeName;
            this._orderType = orderType;
        }

        /// <param name="attributeName" />
        /// <param name="orderType" />
        /// <param name="alias" />
        public OrderExpression(string attributeName, OrderType orderType, string alias)
        {
            this._attributeName = attributeName;
            this._orderType = orderType;
            this._alias = alias;
        }

        public OrderExpression(string attributeName, OrderType orderType, string alias, string entityName)
        {
            this._attributeName = attributeName;
            this._orderType = orderType;
            this._alias = alias;
            this._entityName = entityName;
        }

        /// <summary>
        ///   <para>Gets or sets the name of the attribute in the order expression.</para>
        /// </summary>
        /// <returns>
        ///   <para>Type: String. The name of the attribute in the order expression.</para>
        /// </returns>
        public string AttributeName
        {
            get => this._attributeName;
            set => this._attributeName = value;
        }

        /// <summary>
        ///   <para>Gets or sets the name of the fallback attribute in the order expression.</para>
        ///   <para>If a <see cref="FallbackAttributeName" /> is provided and the database value of the primary attribute provided with <see cref="AttributeName" /> is null, the <see cref="FallbackAttributeName" /> will be used. If this attribute is provided, the ORDER BY clause in the SQL query will have a COALESCE call.</para>
        /// </summary>
        /// <returns>
        ///   <para>Type: String. The name of the fallback attribute in the order expression.</para>
        /// </returns>
        public string FallbackAttributeName
        {
            get => this._fallbackAttributeName;
            set => this._fallbackAttributeName = value;
        }

        /// <summary>
        ///   <para>Gets or sets the order, ascending or descending.</para>
        /// </summary>
        /// <returns>
        ///   <para>Returns <see cref="T:DataAccessLibrary.Query.OrderType" />The order, ascending or descending.</para>
        /// </returns>
        public OrderType OrderType
        {
            get => this._orderType;
            set => this._orderType = value;
        }

        public string Alias
        {
            get => this._alias;
            set => this._alias = value;
        }

        public string EntityName
        {
            get => this._entityName;
            set => this._entityName = value;
        }
    }
}
