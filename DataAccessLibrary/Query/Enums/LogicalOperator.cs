namespace DataAccessLibrary.Query
{
    /// <summary>
    ///   <para>Contains the possible values for an operator in a <see cref="T:DataAccessLibrary.Query.FilterExpression" />.</para>
    /// </summary>
    public enum LogicalOperator
    {
        /// <summary>
        ///   <para>A logical AND operation is performed. Value = 0.</para>
        /// </summary>
        And,

        /// <summary>
        ///   <para>A logical OR operation is performed. Value = 1.</para>
        /// </summary>
        Or
    }
}