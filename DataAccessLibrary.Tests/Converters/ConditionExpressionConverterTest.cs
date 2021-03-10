using DataAccessLibrary.Converters;
using DataAccessLibrary.Query;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DataAccessLibrary.Tests.Converters
{
    public class ConditionExpressionConverterTest
    {
        [Fact]
        public void ConvertToSQLTestStringValue()
        {
            var conditionExpression = new ConditionExpression("testFieldName", ConditionOperator.Equal, "testFieldValue");
            string output = ConditionExpressionConverter.ConvertToSQL(conditionExpression);
        }

        [Fact]
        public void ConvertToSQLTestEscapeStringValue()
        {
        }

        [Fact]
        public void ConvertToSQLTestDateTimeValue()
        {
        }

        [Fact]
        public void ConvertToSQLTestGuidValue()
        {
        }

        [Fact]
        public void ConvertToSQLTestBooleanValue()
        {
            var input = new ConditionExpression("booleanField", ConditionOperator.Equal, true);
            Assert.Equal("booleanField = 1", ConditionExpressionConverter.ConvertToSQL(input));
        }

        [Fact]
        public void ConvertToSQLTestIntegerValue()
        {
        }

        [Fact]
        public void ConvertToSQLTestNullValue()
        {
        }

        /// Testing
        /// Test all operators
        /// ConditionOperator.Equal: return " = ";
        /// ConditionOperator.NotEqual: return " <> ";
        /// ConditionOperator.GreaterThan: return " > ";
        /// ConditionOperator.LessThan: return " < ";
        /// ConditionOperator.GreaterEqual: return " >= ";
        /// ConditionOperator.LessEqual: return " <= ";
        /// ConditionOperator.Like: return " LIKE ";
        /// ConditionOperator.NotLike: return " NOT LIKE ";
        /// ConditionOperator.NotNull: return " IS NOT NULL ";
        /// ConditionOperator.Null: return " IS NULL ";
        /// Test Escaping
    }
}
