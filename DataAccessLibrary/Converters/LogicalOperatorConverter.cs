using DataAccessLibrary.Query;
using System;

namespace DataAccessLibrary.Converters
{
    public static class LogicalOperatorConverter
    {
        public static string ConvertToSQL(LogicalOperator logicalOperator)
        {
            switch (logicalOperator)
            {
                case LogicalOperator.And: return "AND";
                case LogicalOperator.Or: return "OR";
                default: throw new NotImplementedException();
            }
        }
    }
}
