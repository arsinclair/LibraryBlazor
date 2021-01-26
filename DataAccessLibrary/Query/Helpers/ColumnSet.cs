using System.Collections.Generic;

namespace DataAccessLibrary.Query
{
    /// <summary>
    ///   <para>Specifies the attributes for which non-null values are returned from a query.</para>
    /// </summary>
    public class ColumnSet
    {
        private bool _allColumns;
        private DataCollection<string> _columns;

        /// <summary>
        ///   <para>Initializes a new instance of the <see cref="T:DataAccessLibrary.Query.ColumnSet" /> class.</para>
        /// </summary>
        public ColumnSet()
        {
        }

        /// <summary>
        ///   <para>Initializes a new instance of the <see cref="T:DataAccessLibrary.Query.ColumnSet" /> class setting the <see cref="P:DataAccessLibrary.Query.ColumnSet.AllColumns" /> property.</para>
        /// </summary>
        /// <param name="allColumns">
        ///   <para>Type: Boolean. A <see langword="Boolean" /> that specifies whether to retrieve all attributes of a record.</para>
        /// </param>
        public ColumnSet(bool allColumns)
        {
            this._allColumns = allColumns;
        }

        /// <summary>
        ///   <para>Initializes a new instance of the <see cref="T:DataAccessLibrary.Query.ColumnSet" /> class setting the <see cref="P:DataAccessLibrary.Query.ColumnSet.Columns" /> property.</para>
        /// </summary>
        /// <param name="columns">
        ///   <para>Type: String[]. Specifies an array of <see langword="Strings" /> containing the names of the attributes.</para>
        /// </param>
        public ColumnSet(params string[] columns)
        {
            this._columns = new DataCollection<string>((IList<string>)columns);
        }

        /// <summary>
        ///   <para>Adds the specified attribute to the column set.</para>
        /// </summary>
        /// <param name="columns">
        ///   <para>Type: String[]. Specifies an array of <see langword="Strings" /> containing the names of the attributes.</para>
        /// </param>
        public void AddColumns(params string[] columns)
        {
            foreach (string column in columns)
            {
                this.Columns.Add(column);
            }
        }

        /// <summary>
        ///   <para>Adds the specified attribute to the column set.</para>
        /// </summary>
        /// <param name="column">
        ///   <para>Type: String. Specifies a <see langword="String" /> containing the name of the attribute.</para>
        /// </param>
        public void AddColumn(string column) => this.Columns.Add(column);

        /// <summary>
        ///   <para>Gets or sets whether to retrieve all the attributes of a record.</para>
        /// </summary>
        /// <returns>
        ///   <para>Type: Booleantrue to specify to retrieve all attributes; false to to retrieve only specified attributes.</para>
        /// </returns>
        public bool AllColumns
        {
            get => this._allColumns;
            set => this._allColumns = value;
        }

        /// <summary>
        ///   <para>Gets the collection of <see langword="String" />s containing the names of the attributes to be retrieved from a query.</para>
        /// </summary>
        /// <returns>
        ///   <para>Type: ICollection
        /// The collection of attribute names to return from a query.</para>
        /// </returns>
        public DataCollection<string> Columns
        {
            get
            {
                if (this._columns == null)
                {
                    this._columns = new DataCollection<string>();
                }
                return this._columns;
            }
        }
    }
}
