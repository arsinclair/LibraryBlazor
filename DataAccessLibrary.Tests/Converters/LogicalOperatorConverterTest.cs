using DataAccessLibrary.Converters;
using DataAccessLibrary.Query;
using Xunit;

namespace DataAccessLibrary.Tests.Converters
{
    public class LogicalOperatorConverterTest
    {
        [Fact]
        public void ConvertToSQLTest()
        {
            Assert.Equal("OR", LogicalOperatorConverter.ConvertToSQL(LogicalOperator.Or));
            Assert.Equal("AND", LogicalOperatorConverter.ConvertToSQL(LogicalOperator.And));
        }
    }
}
