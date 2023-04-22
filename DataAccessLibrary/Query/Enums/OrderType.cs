namespace DataAccessLibrary.Query
{
    /// <summary>
    ///   <para>Contains the possible values for the order type in an <see cref="T:DataAccessLibrary.Query.OrderExpression" />.</para>
    /// </summary>
    public enum OrderType
    {
        /// <summary>
        ///   <para>The values of the specified attribute should be sorted in ascending order, from lowest to highest. Value = 0.</para>
        /// </summary>
        Ascending,

        /// <summary>
        ///   <para>The values of the specified attribute should be sorted in descending order, from highest to lowest. Value = 1.</para>
        /// </summary>
        Descending
    }
}
