using System;
using System.Collections;

namespace DataAccessLibrary.Query
{
    /// <summary>
    ///   <para>Contains a condition expression used to filter the results of the query.</para>
    /// </summary>
    public class ConditionExpression
    {
        private string _attributeName;
        private ConditionOperator _conditionOperator;
        private DataCollection<object> _values;
        private string _entityName;
        private bool _compareColumns;

        /// <summary>
        ///   <para>Initializes a new instance of the <see cref="T:DataAccessLibrary.Query.ConditionExpression" /> class.</para>
        /// </summary>
        public ConditionExpression()
        {
        }

        /// <summary>
        ///   <para>Initializes a new instance of the <see cref="T:DataAccessLibrary.Query.ConditionExpression" /> class setting the attribute name, condition operator and an array of value objects.</para>
        /// </summary>
        /// <param name="attributeName">
        ///   <para>Type: String. The logical name of the attribute in the condition expression.</para>
        /// </param>
        /// <param name="conditionOperator">
        ///   <para>Type: <see cref="T:DataAccessLibrary.Query.ConditionOperator" />. The condition operator.</para>
        /// </param>
        /// <param name="values">
        ///   <para>Type: Object[]. The array of attribute values.</para>
        /// </param>
        public ConditionExpression(
          string attributeName,
          ConditionOperator conditionOperator,
          params object[] values)
          : this(null, attributeName, conditionOperator, values)
        {
        }

        /// <summary>
        ///   <para>Initializes a new instance of the <see cref="T:DataAccessLibrary.Query.ConditionExpression" /> class.</para>
        /// </summary>
        /// <param name="entityName">
        ///   <para>Type: String. The logical name of the entity in the condition expression.</para>
        /// </param>
        /// <param name="attributeName">
        ///   <para>Type: String. The logical name of the attribute in the condition expression.</para>
        /// </param>
        /// <param name="conditionOperator">
        ///   <para>Type: <see cref="T:DataAccessLibrary.Query.ConditionOperator" />. The condition operator.</para>
        /// </param>
        /// <param name="values">
        ///   <para>Type: Object[]. The array of attribute values.</para>
        /// </param>
        public ConditionExpression(
          string entityName,
          string attributeName,
          ConditionOperator conditionOperator,
          params object[] values)
        {
            this._entityName = entityName;
            this._attributeName = attributeName;
            this._conditionOperator = conditionOperator;
            if (values == null)
                return;
            this._values = new DataCollection<object>(values);
        }

        /// <param name="entityName" />
        /// <param name="attributeName" />
        /// <param name="conditionOperator" />
        /// <param name="compareColumns" />
        /// <param name="values" />
        public ConditionExpression(
          string entityName,
          string attributeName,
          ConditionOperator conditionOperator,
          bool compareColumns,
          object[] values)
          : this(entityName, attributeName, conditionOperator, values)
        {
            this._compareColumns = compareColumns;
        }

        /// <param name="entityName" />
        /// <param name="attributeName" />
        /// <param name="conditionOperator" />
        /// <param name="compareColumns" />
        /// <param name="value" />
        public ConditionExpression(
          string entityName,
          string attributeName,
          ConditionOperator conditionOperator,
          bool compareColumns,
          object value) : this(entityName, attributeName, conditionOperator, (compareColumns ? 1 : 0) != 0, new object[1] { value })
        {
        }

        /// <param name="attributeName" />
        /// <param name="conditionOperator" />
        /// <param name="compareColumns" />
        /// <param name="value" />
        public ConditionExpression(
          string attributeName,
          ConditionOperator conditionOperator,
          bool compareColumns,
          object value) : this(null, attributeName, conditionOperator, (compareColumns ? 1 : 0) != 0, new object[1] { value })
        {
        }

        /// <param name="attributeName" />
        /// <param name="conditionOperator" />
        /// <param name="compareColumns" />
        /// <param name="values" />
        public ConditionExpression(
          string attributeName,
          ConditionOperator conditionOperator,
          bool compareColumns,
          object[] values) : this(null, attributeName, conditionOperator, compareColumns, values)
        {
        }

        /// <summary>
        ///   <para>Initializes a new instance of the <see cref="T:DataAccessLibrary.Query.ConditionExpression" /> class setting the attribute name, condition operator and value object.</para>
        /// </summary>
        /// <param name="attributeName">
        ///   <para>Type: String. The logical name of the attribute in the condition expression.</para>
        /// </param>
        /// <param name="conditionOperator">
        ///   <para>Type: <see cref="T:DataAccessLibrary.Query.ConditionOperator" />. The condition operator.</para>
        /// </param>
        /// <param name="value">
        ///   <para>Type: Object. The attribute value.</para>
        /// </param>
        public ConditionExpression(string attributeName, ConditionOperator conditionOperator, object value)
            : this(attributeName, conditionOperator, new object[1] { value })
        {
        }

        /// <summary>
        ///   <para>Initializes a new instance of the <see cref="T:DataAccessLibrary.Query.ConditionExpression" /> class.</para>
        /// </summary>
        /// <param name="entityName">
        ///   <para>Type: String. The logical name of the entity in the condition expression.</para>
        /// </param>
        /// <param name="attributeName">
        ///   <para>Type: String. The logical name of the attribute in the condition expression.</para>
        /// </param>
        /// <param name="conditionOperator">
        ///   <para>Type: <see cref="T:DataAccessLibrary.Query.ConditionOperator" />. The condition operator.</para>
        /// </param>
        /// <param name="value">
        ///   <para>Type: Object. The attribute value.</para>
        /// </param>
        public ConditionExpression(
          string entityName,
          string attributeName,
          ConditionOperator conditionOperator,
          object value) : this(entityName, attributeName, conditionOperator, new object[1] { value })
        {
        }

        /// <summary>
        ///   <para>Initializes a new instance of the <see cref="T:DataAccessLibrary.Query.ConditionExpression" /> class setting the attribute name and condition operator.</para>
        /// </summary>
        /// <param name="attributeName">
        ///   <para>Type: String. The logical name of the attribute in the condition expression.</para>
        /// </param>
        /// <param name="conditionOperator">
        ///   <para>Type: <see cref="T:DataAccessLibrary.Query.ConditionOperator" />. The condition operator.</para>
        /// </param>
        public ConditionExpression(string attributeName, ConditionOperator conditionOperator)
          : this(null, attributeName, conditionOperator, Array.Empty<object>())
        {
        }

        /// <summary>
        ///   <para>Initializes a new instance of the <see cref="T:DataAccessLibrary.Query.ConditionExpression" /> class.</para>
        /// </summary>
        /// <param name="entityName">
        ///   <para>Type: String. The logical name of the entity in the condition expression.</para>
        /// </param>
        /// <param name="attributeName">
        ///   <para>Type: String. The logical name of the attribute in the condition expression.</para>
        /// </param>
        /// <param name="conditionOperator">
        ///   <para>Type: <see cref="T:DataAccessLibrary.Query.ConditionOperator" />. The condition operator.</para>
        /// </param>
        public ConditionExpression(
          string entityName,
          string attributeName,
          ConditionOperator conditionOperator)
          : this(entityName, attributeName, conditionOperator, Array.Empty<object>())
        {
        }

        /// <summary>
        ///   <para>Initializes a new instance of the <see cref="T:DataAccessLibrary.Query.ConditionExpression" /> class setting the attribute name, condition operator and a collection of values.</para>
        /// </summary>
        /// <param name="attributeName">
        ///   <para>Type: String. The logical name of the attribute in the condition expression.</para>
        /// </param>
        /// <param name="conditionOperator">
        ///   <para>Type: <see cref="T:DataAccessLibrary.Query.ConditionOperator" />. The condition operator.</para>
        /// </param>
        /// <param name="values">
        ///   <para>Type: ICollection. The collection of attribute values.</para>
        /// </param>
        public ConditionExpression(string attributeName, ConditionOperator conditionOperator, ICollection values)
        {
            this._attributeName = attributeName;
            this._conditionOperator = conditionOperator;
            if (values == null)
                return;
            this._values = new DataCollection<object>();
            foreach (object obj in values)
                this._values.Add(obj);
        }

        /// <summary>
        ///   <para>Gets or sets the entity name for the condition.</para>
        /// </summary>
        /// <returns>
        ///   <para>Type: StringThe name of the entity.</para>
        /// </returns>
        public string EntityName
        {
            get => this._entityName;
            set => this._entityName = value;
        }

        public bool CompareColumns
        {
            get => this._compareColumns;
            set => this._compareColumns = value;
        }

        /// <summary>
        ///   <para>Gets or sets the logical name of the attribute in the condition expression.</para>
        /// </summary>
        /// <returns>
        ///         <para>Type: String
        /// The logical name of the attribute in the condition expression.</para>
        ///       </returns>
        public string AttributeName
        {
            get => this._attributeName;
            set => this._attributeName = value;
        }

        /// <summary>
        ///   <para>Gets or sets the condition operator.</para>
        /// </summary>
        /// <returns>
        ///         <para>Type: <see cref="T:DataAccessLibrary.Query.ConditionOperator" />
        /// The condition operator.</para>
        ///       </returns>
        public ConditionOperator Operator
        {
            get => this._conditionOperator;
            set => this._conditionOperator = value;
        }

        /// <summary>
        ///   <para>Gets or sets the values for the attribute.</para>
        /// </summary>
        /// <returns>
        ///   <para>Type: <see cref="T:Microsoft.Xrm.Sdk.DataCollection`1" />&lt;Object&gt;The attribute values.</para>
        /// </returns>
        public DataCollection<object> Values
        {
            get
            {
                if (this._values == null)
                    this._values = new DataCollection<object>();
                return this._values;
            }
            private set => this._values = value;
        }
    }
}
