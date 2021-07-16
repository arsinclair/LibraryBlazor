namespace DataAccessLibrary.Query
{
    /// <summary>
    ///   <para>Describes the type of comparison for two values (or expressions) in a condition expression.</para>
    /// </summary>
    public enum ConditionOperator
    {
        /// <summary>
        ///   <para>The values are compared for equality.</para>
        /// </summary>
        Equal,

        /// <summary>
        ///   <para>The two values are not equal.</para>
        /// </summary>
        NotEqual,

        /// <summary>
        ///   <para>The value is greater than the compared value.</para>
        /// </summary>
        GreaterThan,

        /// <summary>
        ///   <para>The value is less than the compared value.</para>
        /// </summary>
        LessThan,

        /// <summary>
        ///   <para>The value is greater than or equal to the compared value.</para>
        /// </summary>
        GreaterEqual,

        /// <summary>
        ///   <para>The value is less than or equal to the compared value.</para>
        /// </summary>
        LessEqual,

        /// <summary>
        ///   <para>The character string is matched to the specified pattern.</para>
        /// </summary>
        Like,

        /// <summary>
        ///   <para>The character string does not match the specified pattern.</para>
        /// </summary>
        NotLike,

        /// <summary>
        ///   <para>TheThe value exists in a list of values.</para>
        /// </summary>
        In,

        /// <summary>
        ///   <para>The given value is not matched to a value in a subquery or a list.</para>
        /// </summary>
        NotIn,

        ///// <summary>
        /////   <para>The value is between two values.</para>
        ///// </summary>
        //Between,

        ///// <summary>
        /////   <para>The value is not between two values.</para>
        ///// </summary>
        //NotBetween,

        /// <summary>
        ///   <para>The value is null.</para>
        /// </summary>
        Null,

        /// <summary>
        ///   <para>The value is not null.</para>
        /// </summary>
        NotNull,

        ///// <summary>
        /////   <para>The value equals yesterday’s date.</para>
        ///// </summary>
        //Yesterday,

        ///// <summary>
        /////   <para>The value equals today’s date.</para>
        ///// </summary>
        //Today,

        ///// <summary>
        /////   <para>The value equals tomorrow’s date.</para>
        ///// </summary>
        //Tomorrow,

        ///// <summary>
        /////   <para>The value is within the last seven days including today.</para>
        ///// </summary>
        //Last7Days,

        ///// <summary>
        /////   <para>The value is within the next seven days.</para>
        ///// </summary>
        //Next7Days,

        ///// <summary>
        /////   <para>The value is within the previous week including Sunday through Saturday.</para>
        ///// </summary>
        //LastWeek,

        ///// <summary>
        /////   <para>The value is within the current week.</para>
        ///// </summary>
        //ThisWeek,

        ///// <summary>
        /////   <para>The value is within the next week.</para>
        ///// </summary>
        //NextWeek,

        ///// <summary>
        /////   <para>The value is within the last month including first day of the last month and last day of the last month.</para>
        ///// </summary>
        //LastMonth,

        ///// <summary>
        /////   <para>The value is within the current month.</para>
        ///// </summary>
        //ThisMonth,

        ///// <summary>
        /////   <para>The value is within the next month.</para>
        ///// </summary>
        //NextMonth,

        ///// <summary>
        /////   <para>The value is on a specified date.</para>
        ///// </summary>
        //On,

        ///// <summary>
        /////   <para>The value is on or before a specified date.</para>
        ///// </summary>
        //OnOrBefore,

        ///// <summary>
        /////   <para>The value is on or after a specified date.</para>
        ///// </summary>
        //OnOrAfter,

        ///// <summary>
        /////   <para>The value is within the previous year.</para>
        ///// </summary>
        //LastYear,

        ///// <summary>
        /////   <para>The value is within the current year.</para>
        ///// </summary>
        //ThisYear,

        ///// <summary>
        /////   <para>The value is within the next year.</para>
        ///// </summary>
        //NextYear,

        ///// <summary>
        /////   <para>The value is within the last X hours.</para>
        ///// </summary>
        //LastXHours,

        ///// <summary>
        /////   <para>The value is within the next X (specified value) hours.</para>
        ///// </summary>
        //NextXHours,

        ///// <summary>
        /////   <para>The value is within last X days.</para>
        ///// </summary>
        //LastXDays,

        ///// <summary>
        /////   <para>The value is within the next X (specified value) days.</para>
        ///// </summary>
        //NextXDays,

        ///// <summary>
        /////   <para>The value is within the last X (specified value) weeks.</para>
        ///// </summary>
        //LastXWeeks,

        ///// <summary>
        /////   <para>The value is within the next X weeks.</para>
        ///// </summary>
        //NextXWeeks,

        ///// <summary>
        /////   <para>The value is within the last X (specified value) months.</para>
        ///// </summary>
        //LastXMonths,

        ///// <summary>
        /////   <para>The value is within the next X (specified value) months.</para>
        ///// </summary>
        //NextXMonths,

        ///// <summary>
        /////   <para>The value is within the last X years.</para>
        ///// </summary>
        //LastXYears,

        ///// <summary>
        /////   <para>The value is within the next X years.</para>
        ///// </summary>
        //NextXYears,

        ///// <summary>
        /////   <para>The string contains another string.</para>
        /////   <para>You must use the Contains operator for only those attributes that are enabled for full-text indexing. Otherwise, you will receive a generic SQL error message while retrieving data.</para>
        ///// </summary>
        //Contains,

        ///// <summary>
        /////   <para>The string does not contain another string.</para>
        ///// </summary>
        //DoesNotContain,

        ///// <summary>
        /////   <para>The value is older than the specified number of months.</para>
        ///// </summary>
        //OlderThanXMonths,

        ///// <summary>
        /////   <para>The string occurs at the beginning of another string.</para>
        ///// </summary>
        //BeginsWith,

        ///// <summary>
        /////   <para>The string does not begin with another string.</para>
        ///// </summary>
        //DoesNotBeginWith,

        ///// <summary>
        /////   <para>The string ends with another string.</para>
        ///// </summary>
        //EndsWith,

        ///// <summary>
        /////   <para>The string does not end with another string.</para>
        ///// </summary>
        //DoesNotEndWith,

        ///// <summary>
        /////   <para />
        ///// </summary>
        //OlderThanXYears,

        ///// <summary>
        /////   <para />
        ///// </summary>
        //OlderThanXWeeks,

        ///// <summary>
        /////   <para />
        ///// </summary>
        //OlderThanXDays,

        ///// <summary>
        /////   <para />
        ///// </summary>
        //OlderThanXHours,

        ///// <summary>
        /////   <para />
        ///// </summary>
        //OlderThanXMinutes,

        //ContainValues,

        //DoesNotContainValues
    }
}
