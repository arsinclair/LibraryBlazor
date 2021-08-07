using DataAccessLibrary.Cache;
using DataAccessLibrary.Converters;
using DataAccessLibrary.Query;
using DataAccessLibrary.Tests.SetUp.Fixtures;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DataAccessLibrary.Tests.Converters
{
    [Collection("DatabaseCache Collection")]
    public class ConditionExpressionConverterTest
    {
        public ConditionExpressionConverterTest(DatabaseCacheFixture fixture)
        {
            CacheManager.SetBypassDatabaseCache(fixture);
        }

        [Fact]
        public void ConvertToSQLTestStringValue()
        {
            var conditionExpression = new ConditionExpression("Name", ConditionOperator.Equal, "testFieldValue");
            conditionExpression.EntityName = "contact";
            string output = ConditionExpressionConverter.ConvertToSQL(conditionExpression);
            Assert.Equal("[testFieldName]=testFieldValue", output);
        }

        [Fact]
        public void ConvertToSQLTestEscapeStringValue()
        {
            var conditionExpression = new ConditionExpression("testFieldName", ConditionOperator.Equal, "testField');[somedata]%percentage_underscore LIKE");
            string output = ConditionExpressionConverter.ConvertToSQL(conditionExpression);
            Assert.Equal("[testFieldName]=testFieldValue", output);
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
            var input = new ConditionExpression("Contact", "IsActive", ConditionOperator.Equal, true);
            Assert.Equal("[IsActive]=1", ConditionExpressionConverter.ConvertToSQL(input));
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
