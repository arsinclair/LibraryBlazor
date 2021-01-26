namespace DataAccessLibrary.Query
{
    /// <summary>
    ///   <para>Specifies a number of pages and a number of entity instances per page to return from the query.</para>
    /// </summary>
    public sealed class PagingInfo
    {
        private int _pageNumber;
        private int _count;
        private string _pagingCookie;
        private bool _returnTotalRecordCount;

        /// <summary>
        ///   <para>Gets or sets the page number to return with the query.</para>
        /// </summary>
        /// <returns>
        ///         <para>Type: Int32
        /// The number of pages returned from the query.</para>
        ///       </returns>
        public int PageNumber
        {
            get => this._pageNumber;
            set => this._pageNumber = value;
        }

        /// <summary>
        ///   <para>Gets or sets the number of entity instances returned per page.</para>
        /// </summary>
        /// <returns>
        ///         <para>Type: Int32
        /// The number of entity instances returned per page.</para>
        ///       </returns>
        public int Count
        {
            get => this._count;
            set => this._count = value;
        }

        /// <summary>
        ///   <para>Sets whether the total number of records should be returned from the query.</para>
        /// </summary>
        /// <returns>
        ///   <para>Type: Booleantrue if the <see cref="P:DataAccessLibrary.EntityCollection.TotalRecordCount" /> should be set when the query is executed; otherwise, false.</para>
        /// </returns>
        public bool ReturnTotalRecordCount
        {
            get => this._returnTotalRecordCount;
            set => this._returnTotalRecordCount = value;
        }

        /// <summary>
        ///   <para>Gets or sets the info used to page large result sets.</para>
        /// </summary>
        /// <returns>
        ///         <para>Type: String
        /// The info used to page large result sets.</para>
        ///       </returns>
        public string PagingCookie
        {
            get => this._pagingCookie;
            set => this._pagingCookie = value;
        }
    }
}
