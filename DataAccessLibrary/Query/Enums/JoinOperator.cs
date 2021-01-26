namespace DataAccessLibrary.Query
{
    /// <summary>
    ///   <para>Contains the possible values for a join operator in a <see cref="T:DataAccessLibrary.Query.LinkEntity" />.</para>
    /// </summary>
    public enum JoinOperator
    {
        /// <summary>
        ///   <para>The values in the attributes being joined are compared using a comparison operator. Value = 0.</para>
        /// </summary>
        Inner,

        /// <summary>
        ///   <para>All instances of the entity in the FROM clause are returned if they meet WHERE or HAVING search conditions. Value = 1.</para>
        /// </summary>
        LeftOuter,

        /// <summary>
        ///   <para>Only one value of the two joined attributes is returned if an equal-join operation is performed and the two values are identical. Value = 2.</para>
        /// </summary>
        Natural,

        /// <summary>
        ///   <para>Link-entity is generated as Correlated Subquery. The outer entity uses the “cross apply” operator on the Correlated Subquery. Pick the top 1 row.</para>
        /// </summary>
        MatchFirstRowUsingCrossApply,

        In,

        /// <summary>
        ///   <para>Link-entity is generated as a Correlated Subquery. The outer entity uses “exists” operator on the Correlated Subquery.</para>
        /// </summary>
        Exists,

        /// <summary>
        ///   <para>Link-entity is generated as Subquery. The outer entity uses the “in” operator on the Subquery.</para>
        /// </summary>
        Any,

        NotAny,

        All,

        NotAll
    }
}
