using DataAccessLibrary.Cache;
using DataAccessLibrary.Converters;
using DataAccessLibrary.Models;
using DataAccessLibrary.Query;
using DataAccessLibrary.Tests.SetUp.Fixtures;
using System;
using Xunit;

namespace DataAccessLibrary.Tests
{
    [Collection("DatabaseCache Collection")]
    public class FilterExpressionConverterTest
    {
        public FilterExpressionConverterTest(DatabaseCacheFixture fixture)
        {
            CacheManager.SetBypassDatabaseCache(fixture);
        }

        [Fact]
        public void ConvertToSQLEmptyFilterExpressionTest()
        {
            FilterExpression fe = new FilterExpression();
            Assert.Equal("", FilterExpressionConverter.ConvertToSQL(fe, ""));
            Assert.Equal("", FilterExpressionConverter.ConvertToSQL(fe, "this-text-doesn't-matter", 10));
        }

        [Fact]
        public void ConvertToSQLSingleQueryFilterExpressionTest()
        {
            FilterExpression fe = new FilterExpression();
            fe.AddQuery("Contact", "FullName", ConditionOperator.Equal, "John Gray");

            Assert.Equal(" WHERE  ( FullName = 'John Gray' ) ", FilterExpressionConverter.ConvertToSQL(fe, "Contact"));
        }

        [Fact]
        public void ConvertToSQLMultipleQueriesFilterExpressionTest()
        {
            FilterExpression fe = new FilterExpression();
            fe.AddQuery("Contact", "FullName", ConditionOperator.Equal, "John Gray");
            fe.AddQuery("FullName", ConditionOperator.Like, true, "David Black");

            Assert.Equal(" WHERE  ( FullName = 'John Gray' AND FullName LIKE '%David Black%' ) ", FilterExpressionConverter.ConvertToSQL(fe, "Contact"));

            fe.QueryOperator = LogicalOperator.Or;

            Assert.Equal(" WHERE  ( FullName = 'John Gray' OR FullName LIKE '%David Black%' ) ", FilterExpressionConverter.ConvertToSQL(fe, "Contact"));

            fe.AddQuery("Age", ConditionOperator.GreaterThan, 6);

            Assert.Equal(" WHERE  ( FullName = 'John Gray' OR FullName LIKE '%David Black%' OR Age > '6' ) ", FilterExpressionConverter.ConvertToSQL(fe, "Contact"));
        }

        [Fact]
        public void ConvertToSQLSingleSubQueryFilterExpressionTest()
        {
            FilterExpression fe = new FilterExpression(LogicalOperator.Or);

            FilterExpression subQuery = new FilterExpression(LogicalOperator.And);
            subQuery.AddQuery("FirstName", ConditionOperator.Like, "David");
            subQuery.AddQuery("GenderId", ConditionOperator.Equal, new EntityReference("Gender", Guid.Parse("824b213f-8ec2-4157-b5f1-96b1ebe89d6c"))); // 824b213f-8ec2-4157-b5f1-96b1ebe89d6c - GUID for Male
            fe.AddSubQuery(subQuery);

            Assert.Equal(" WHERE  (  ( FirstName LIKE '%David%' AND GenderId = '824b213f-8ec2-4157-b5f1-96b1ebe89d6c' )  ) ", FilterExpressionConverter.ConvertToSQL(fe, "Contact"));
        }

        [Fact]
        public void ConvertToSQLMultipleSubQueriesFilterExpressionTest()
        {
            FilterExpression fe = new FilterExpression(LogicalOperator.Or);

            FilterExpression subQuery1 = new FilterExpression(LogicalOperator.And);
            subQuery1.AddQuery("Contact", "FullName", ConditionOperator.Like, "David Black");
            subQuery1.AddQuery("Contact", "GenderId", ConditionOperator.Equal, new EntityReference("Gender", Guid.Parse("824b213f-8ec2-4157-b5f1-96b1ebe89d6c"))); // 824b213f-8ec2-4157-b5f1-96b1ebe89d6c - GUID for Male
            fe.AddSubQuery(subQuery1);

            FilterExpression subQuery2 = new FilterExpression(LogicalOperator.And);
            subQuery2.AddQuery("Contact", "FirstName", ConditionOperator.Like, "Sarah");
            subQuery2.AddQuery("Contact", "GenderId", ConditionOperator.Equal, new EntityReference("Gender", Guid.Parse("912b9357-c9c8-40d8-8a81-646057a33053"))); // 912b9357-c9c8-40d8-8a81-646057a33053 - GUID for Female
            fe.AddSubQuery(subQuery2);

            Assert.Equal(" WHERE  (  ( FullName LIKE '%David Black%' AND GenderId = '824b213f-8ec2-4157-b5f1-96b1ebe89d6c' )  OR  ( FirstName LIKE '%Sarah%' AND GenderId = '912b9357-c9c8-40d8-8a81-646057a33053' )  ) ", FilterExpressionConverter.ConvertToSQL(fe, "Contact")); // 824B213F-8EC2-4157-B5F1-96B1EBE89D6C - GUID for Male, 912B9357-C9C8-40D8-8A81-646057A33053 - GUID for female
        }

        [Fact]
        public void ConvertToSQLComplexFilterExpressionTest()
        {
            FilterExpression fe = new FilterExpression(LogicalOperator.And);
            fe.AddQuery("CityOfOriginId", ConditionOperator.Equal, new EntityReference("City", Guid.Parse("afbdd272-028b-e811-95b7-08002791e63c"))); // afbdd272-028b-e811-95b7-08002791e63c - Denver
            fe.AddQuery("CountryOfOriginId", ConditionOperator.Equal, new EntityReference("Country", Guid.Parse("c6257bef-848a-e811-8f78-08002791e63c"))); // c6257bef-848a-e811-8f78-08002791e63c - USA
            fe.AddQuery("Age", ConditionOperator.Like, 2);

            FilterExpression subQuery1 = new FilterExpression(LogicalOperator.Or);
            subQuery1.AddQuery("FirstName", ConditionOperator.Like, "Joe");
            subQuery1.AddQuery("FirstName", ConditionOperator.Like, "Mark");
            subQuery1.AddQuery("FirstName", ConditionOperator.Like, "Charley");

            FilterExpression subQuery2 = new FilterExpression(LogicalOperator.And);
            subQuery2.AddQuery("Address", ConditionOperator.Like, "Hanover Sq.");

            FilterExpression subQuery3 = new FilterExpression(LogicalOperator.Or);
            subQuery3.AddQuery("Description", ConditionOperator.Like, "The Biggest Company");

            FilterExpression subSubQuery1 = new FilterExpression(LogicalOperator.Or);
            subSubQuery1.AddQuery("ContactRepresentative", ConditionOperator.Like, "Paul");
            subSubQuery1.AddQuery("ContactRepresentative", ConditionOperator.Like, "Peter");
            subQuery3.AddSubQuery(subSubQuery1);

            fe.AddSubQuery(subQuery1);
            fe.AddSubQuery(subQuery2);
            fe.AddSubQuery(subQuery3);

            Assert.Equal(" WHERE  ( CityOfOriginId = 'afbdd272-028b-e811-95b7-08002791e63c' AND CountryOfOriginId = 'c6257bef-848a-e811-8f78-08002791e63c' AND Age LIKE '%2%' AND  ( FirstName LIKE '%Joe%' OR FirstName LIKE '%Mark%' OR FirstName LIKE '%Charley%' )  AND  ( Address LIKE '%Hanover Sq.%' )  AND  ( Description LIKE '%The Biggest Company%' OR  ( ContactRepresentative LIKE '%Paul%' OR ContactRepresentative LIKE '%Peter%' )  )  ) ", FilterExpressionConverter.ConvertToSQL(fe, "Contact"));
        }
    }
}
