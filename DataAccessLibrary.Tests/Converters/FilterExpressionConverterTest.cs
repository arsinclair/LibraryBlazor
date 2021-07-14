using DataAccessLibrary.Converters;
using DataAccessLibrary.Query;
using Xunit;

namespace DataAccessLibrary.Tests
{
    public class FilterExpressionConverterTest
    {
        [Fact]
        public void ConvertToSQLEmptyFilterExpressionTest()
        {
            FilterExpression fe = new FilterExpression();
            Assert.Equal("", FilterExpressionConverter.ConvertToSQL(fe));
            Assert.Equal("", FilterExpressionConverter.ConvertToSQL(fe, 10));
        }

        [Fact]
        public void ConvertToSQLSingleQueryFilterExpressionTest()
        {
            FilterExpression fe = new FilterExpression();
            fe.AddQuery("contact", "name", ConditionOperator.Equal, "John Gray");

            Assert.Equal("WHERE ( name = 'John Gray' ) ", FilterExpressionConverter.ConvertToSQL(fe));
        }

        [Fact]
        public void ConvertToSQLMultipleQueriesFilterExpressionTest()
        {
            FilterExpression fe = new FilterExpression();
            fe.AddQuery("contact", "name", ConditionOperator.Equal, "John Gray");
            fe.AddQuery("name", ConditionOperator.Like, true, "David Black");

            Assert.Equal("WHERE ( name = 'John Gray' AND name LIKE '%David Black%' ) ", FilterExpressionConverter.ConvertToSQL(fe));

            fe.QueryOperator = LogicalOperator.Or;

            Assert.Equal("WHERE ( name = 'John Gray' OR name LIKE '%David Black%' ) ", FilterExpressionConverter.ConvertToSQL(fe));

            fe.AddQuery("age", ConditionOperator.GreaterThan, 6);

            Assert.Equal("WHERE ( name = 'John Gray' OR name LIKE '%David Black%' OR age > 6 ) ", FilterExpressionConverter.ConvertToSQL(fe));
        }

        [Fact]
        public void ConvertToSQLSingleSubQueryFilterExpressionTest()
        {
            FilterExpression fe = new FilterExpression(LogicalOperator.Or);

            FilterExpression subQuery = new FilterExpression(LogicalOperator.And);
            subQuery.AddQuery("name", ConditionOperator.Like, "David Black");
            subQuery.AddQuery("gender", ConditionOperator.Equal, "male");
            fe.AddSubQuery(subQuery);

            Assert.Equal("WHERE ( ( name LIKE '%David Black%' AND gender = 'male' ) ) ", FilterExpressionConverter.ConvertToSQL(fe));
        }

        [Fact]
        public void ConvertToSQLMultipleSubQueriesFilterExpressionTest()
        {
            FilterExpression fe = new FilterExpression(LogicalOperator.Or);

            FilterExpression subQuery1 = new FilterExpression(LogicalOperator.And);
            subQuery1.AddQuery("name", ConditionOperator.Like, "David Black");
            subQuery1.AddQuery("gender", ConditionOperator.Equal, "male");
            fe.AddSubQuery(subQuery1);

            FilterExpression subQuery2 = new FilterExpression(LogicalOperator.And);
            subQuery2.AddQuery("name", ConditionOperator.Like, "Sarah");
            subQuery2.AddQuery("gender", ConditionOperator.Equal, "female");
            fe.AddSubQuery(subQuery1);

            Assert.Equal("WHERE ( ( name LIKE '%David Black%' AND gender = 'male' ) OR ( name LIKE '%Sarah%' AND gender = 'female' ) ) ", FilterExpressionConverter.ConvertToSQL(fe));
        }

        [Fact]
        public void ConvertToSQLComplexFilterExpressionTest()
        {
            FilterExpression fe = new FilterExpression(LogicalOperator.And);
            fe.AddQuery("city", ConditionOperator.Equal, "New-York");
            fe.AddQuery("country", ConditionOperator.Equal, "USA");
            fe.AddQuery("postalcode", ConditionOperator.Like, 12209);

            FilterExpression subQuery1 = new FilterExpression(LogicalOperator.Or);
            subQuery1.AddQuery("name", ConditionOperator.Like, "Joe");
            subQuery1.AddQuery("name", ConditionOperator.Like, "Mark");
            subQuery1.AddQuery("name", ConditionOperator.Like, "Charley");

            FilterExpression subQuery2 = new FilterExpression(LogicalOperator.And);
            subQuery2.AddQuery("address", ConditionOperator.Like, "Hanover Sq.");

            FilterExpression subQuery3 = new FilterExpression(LogicalOperator.Or);
            subQuery3.AddQuery("companyname", ConditionOperator.Like, "The Biggest Company");

            FilterExpression subSubQuery1 = new FilterExpression(LogicalOperator.Or);
            subSubQuery1.AddQuery("contactname", ConditionOperator.Like, "Paul");
            subSubQuery1.AddQuery("contactname", ConditionOperator.Like, "Peter");
            subQuery3.AddSubQuery(subSubQuery1);

            fe.AddSubQuery(subQuery1);
            fe.AddSubQuery(subQuery2);
            fe.AddSubQuery(subQuery3);

            Assert.Equal("WHERE ( city = 'New-York' AND country = 'USA' AND postalcode LIKE '%12209%' AND ( name LIKE '%Joe%' OR name LIKE '%Mark%' OR name LIKE '%Charley%' ) AND ( address LIKE '%Hanover Sq.%' ) AND ( companyname LIKE 'The Biggest Company' OR ( contactname LIKE '%Paul%' OR contactname LIKE '%Peter%' ) ) ) ", FilterExpressionConverter.ConvertToSQL(fe));
        }
    }
}
