using DataAccessLibrary.Cache;
using DataAccessLibrary.Converters;
using DataAccessLibrary.Models;
using DataAccessLibrary.Query;
using DataAccessLibrary.Tests.SetUp.Fixtures;
using System;
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
        public void ConvertToSQLEmptyAttributeNameShouldFail()
        {
            var conditionExpression = new ConditionExpression();
            Assert.Throws<ArgumentNullException>(() => ConditionExpressionConverter.ConvertToSQL(conditionExpression));
            conditionExpression = new ConditionExpression("", ConditionOperator.NotNull);
            Assert.Throws<ArgumentNullException>(() => ConditionExpressionConverter.ConvertToSQL(conditionExpression));
        }

        [Fact]
        public void ConvertToSQLEmptyEntityNameShouldFail()
        {
            var conditionExpression = new ConditionExpression();
            Assert.Throws<ArgumentNullException>(() => ConditionExpressionConverter.ConvertToSQL(conditionExpression));
            conditionExpression = new ConditionExpression("", "Description", ConditionOperator.NotNull);
            Assert.Throws<ArgumentNullException>(() => ConditionExpressionConverter.ConvertToSQL(conditionExpression));
        }

        /// <summary>
        /// In case when value/values are provided, but not needed, e.g. Null/NotNull/Exists/NotExists checks, it should fail.
        /// </summary>
        [Fact]
        public void ConvertToSQLNullCaseUnnecessareValuesProvidedShouldFail()
        {
            var CEWithNull = new ConditionExpression("Contact", "FirstName", ConditionOperator.Null, "Jack");
            var CEWithNotNull = new ConditionExpression("Contact", "LastName", ConditionOperator.NotNull, "Jack");
            Assert.Throws<Exception>(() => ConditionExpressionConverter.ConvertToSQL(CEWithNull));
            Assert.Throws<Exception>(() => ConditionExpressionConverter.ConvertToSQL(CEWithNotNull));
        }

        [Fact]
        public void ConvertToSQLNullCase()
        {
            var CEWithNull = new ConditionExpression("Contact", "FirstName", ConditionOperator.Null);
            var CEWithNotNull = new ConditionExpression("Contact", "LastName", ConditionOperator.NotNull);
            Assert.Equal("[FirstName]IS NULL", ConditionExpressionConverter.ConvertToSQL(CEWithNull));
            Assert.Equal("[LastName]IS NOT NULL", ConditionExpressionConverter.ConvertToSQL(CEWithNotNull));
        }

        [Fact]
        public void ConvertToSQLInCaseNoValuesProvidedShouldFail()
        {
            var CEWithIn = new ConditionExpression("Contact", "FirstName", ConditionOperator.In);
            var CEWithNotIn = new ConditionExpression("Contact", "LastName", ConditionOperator.NotIn, null);
            Assert.Throws<Exception>(() => ConditionExpressionConverter.ConvertToSQL(CEWithIn));
            Assert.Throws<Exception>(() => ConditionExpressionConverter.ConvertToSQL(CEWithNotIn));
        }

        [Fact]
        public void ConvertToSQLInCase()
        {
            var CEWithIn = new ConditionExpression("Contact", "FirstName", ConditionOperator.In, "Andrew", "Peter", "Joshua");
            var CEWithNotIn = new ConditionExpression("Contact", "LastName", ConditionOperator.NotIn, "Mary", "Sofi");
            Assert.Equal("[FirstName]IN('Andrew','Peter','Joshua')", ConditionExpressionConverter.ConvertToSQL(CEWithIn));
            Assert.Equal("[LastName]NOT IN('Mary','Sofi')", ConditionExpressionConverter.ConvertToSQL(CEWithNotIn));
        }

        [Fact]
        public void ConvertToSQLLikeCaseIfNotSingleValueProvidedShouldFail()
        {
            // 0 args
            var CEWithLikeNoValue = new ConditionExpression("Contact", "FirstName", ConditionOperator.Like);
            var CEWithNotLikeNoValue = new ConditionExpression("Contact", "LastName", ConditionOperator.NotLike, null);
            Assert.Throws<Exception>(() => ConditionExpressionConverter.ConvertToSQL(CEWithLikeNoValue));
            Assert.Throws<Exception>(() => ConditionExpressionConverter.ConvertToSQL(CEWithNotLikeNoValue));

            // >1 args
            var CEWithLikeMultipleValues = new ConditionExpression("Contact", "FirstName", ConditionOperator.Like, "Bob", "Dmitry");
            var CEWithNotLikeMultipleValues = new ConditionExpression("Contact", "Address", ConditionOperator.NotLike, "USA", "Canada", "UK");
            Assert.Throws<Exception>(() => ConditionExpressionConverter.ConvertToSQL(CEWithLikeMultipleValues));
            Assert.Throws<Exception>(() => ConditionExpressionConverter.ConvertToSQL(CEWithNotLikeMultipleValues));
        }

        /// <summary>
        /// Some field types are banned from Like/NotLike comparison, such as Boolean, EntityReference, DateTime etc.
        /// </summary>
        [Fact]
        public void ConvertToSQLLikeCaseUsedWithIncorrectFieldTypeShouldFail()
        {
            var CEWithIncorrectFieldType = new ConditionExpression("Contact", "Id", ConditionOperator.Like, "46D6-B130"); // Guid
            Assert.Throws<Exception>(() => ConditionExpressionConverter.ConvertToSQL(CEWithIncorrectFieldType));

            CEWithIncorrectFieldType.AttributeName = "IsActive"; // Bool
            CEWithIncorrectFieldType.Values.Clear();
            CEWithIncorrectFieldType.Values.Add("1");
            Assert.Throws<Exception>(() => ConditionExpressionConverter.ConvertToSQL(CEWithIncorrectFieldType));

            CEWithIncorrectFieldType.AttributeName = "ModifiedOn"; // DateTime
            CEWithIncorrectFieldType.Values.Clear();
            CEWithIncorrectFieldType.Values.Add("2019-03-31");
            Assert.Throws<Exception>(() => ConditionExpressionConverter.ConvertToSQL(CEWithIncorrectFieldType));

            CEWithIncorrectFieldType.AttributeName = "GenderId"; // EntityReference
            CEWithIncorrectFieldType.Values.Clear();
            CEWithIncorrectFieldType.Values.Add(new EntityReference("Gender", Guid.Parse("912B9357-C9C8-40D8-8A81-646057A33053")));
            Assert.Throws<Exception>(() => ConditionExpressionConverter.ConvertToSQL(CEWithIncorrectFieldType));
        }

        [Fact]
        public void ConvertToSQLLikeCase()
        {
            var CEWithTextType = new ConditionExpression("Contact", "FullName", ConditionOperator.Like, "Bob");
            var CEWithTextAreaType = new ConditionExpression("Contact", "Description", ConditionOperator.NotLike, "text'with escaping[test]';");
            var CEWithNumberType = new ConditionExpression("Contact", "Age", ConditionOperator.Like, "8");
            var CEWithEscapedNumberType = new ConditionExpression("Contact", "Age", ConditionOperator.Like, "1'");

            Assert.Equal("[FullName]LIKE'%Bob%'", ConditionExpressionConverter.ConvertToSQL(CEWithTextType));
            Assert.Equal("[Description]NOT LIKE'%text''with escaping[[]test]'';%'", ConditionExpressionConverter.ConvertToSQL(CEWithTextAreaType));
            Assert.Equal("[Age]LIKE'%8%'", ConditionExpressionConverter.ConvertToSQL(CEWithNumberType));
            Assert.Equal("[Age]LIKE'%1''%'", ConditionExpressionConverter.ConvertToSQL(CEWithEscapedNumberType));
        }


        [Fact]
        public void ConvertToSQLOtherCasesNoValueProvidedShouldFail()
        {
            var CEWithMissingValue = new ConditionExpression("Contact", "FullName", ConditionOperator.Equal);
            var CEWithMultipleValue = new ConditionExpression("Contact", "FullName", ConditionOperator.NotEqual, "Bob", "Samantha");
            Assert.Throws<Exception>(() => ConditionExpressionConverter.ConvertToSQL(CEWithMissingValue));
            Assert.Throws<Exception>(() => ConditionExpressionConverter.ConvertToSQL(CEWithMultipleValue));
        }

        [Fact]
        public void ConvertToSQLOtherCases()
        {
            var CEWithDateTime = new ConditionExpression("Contact", "ModifiedOn", ConditionOperator.GreaterThan, DateTime.Parse("2019-08-31").ToUniversalTime());
            var CEWithGuidAsString = new ConditionExpression("Contact", "Id", ConditionOperator.NotEqual, "CE683F2B-0758-E911-80C2-005056010787");
            var CEWithGuid = new ConditionExpression("Contact", "Id", ConditionOperator.NotEqual, Guid.Parse("CE683F2B-0758-E911-80C2-005056010787"));
            var CEWithText = new ConditionExpression("Contact", "FirstName", ConditionOperator.NotNull);
            var CEWithBool = new ConditionExpression("Contact", "IsActive", ConditionOperator.Equal, true);
            var CEWithIntAsString = new ConditionExpression("Contact", "Age", ConditionOperator.LessEqual, "8");
            var CEWithInt = new ConditionExpression("Contact", "Age", ConditionOperator.LessEqual, 8);
            var CEWithEFAsString = new ConditionExpression("Contact", "GenderId", ConditionOperator.In, "824B213F-8EC2-4157-B5F1-96B1EBE89D6C", "912B9357-C9C8-40D8-8A81-646057A33053");
            var CEWithEF = new ConditionExpression("Contact", "GenderId", ConditionOperator.NotIn, new EntityReference("Gender", Guid.Parse("824B213F-8EC2-4157-B5F1-96B1EBE89D6C")), new EntityReference("Gender", Guid.Parse("912B9357-C9C8-40D8-8A81-646057A33053")));

            Assert.Equal("[ModifiedOn]>'8/30/2019 9:00:00 PM'", ConditionExpressionConverter.ConvertToSQL(CEWithDateTime));
            Assert.Equal("[Id]<>'CE683F2B-0758-E911-80C2-005056010787'", ConditionExpressionConverter.ConvertToSQL(CEWithGuidAsString));
            Assert.Equal("[Id]<>'ce683f2b-0758-e911-80c2-005056010787'", ConditionExpressionConverter.ConvertToSQL(CEWithGuid));
            Assert.Equal("[FirstName]IS NOT NULL", ConditionExpressionConverter.ConvertToSQL(CEWithText));
            Assert.Equal("[IsActive]=1", ConditionExpressionConverter.ConvertToSQL(CEWithBool));
            Assert.Equal("[Age]<='8'", ConditionExpressionConverter.ConvertToSQL(CEWithIntAsString));
            Assert.Equal("[Age]<='8'", ConditionExpressionConverter.ConvertToSQL(CEWithInt));
            Assert.Equal("[GenderId]IN('824b213f-8ec2-4157-b5f1-96b1ebe89d6c','912b9357-c9c8-40d8-8a81-646057a33053')", ConditionExpressionConverter.ConvertToSQL(CEWithEFAsString));
            Assert.Equal("[GenderId]NOT IN('824b213f-8ec2-4157-b5f1-96b1ebe89d6c','912b9357-c9c8-40d8-8a81-646057a33053')", ConditionExpressionConverter.ConvertToSQL(CEWithEF));
        }

        [Fact]
        public void GetOperatorAsSQL()
        {
            Assert.Equal("=", ConditionExpressionConverter.GetOperatorAsSQL(ConditionOperator.Equal));
            Assert.Equal("<>", ConditionExpressionConverter.GetOperatorAsSQL(ConditionOperator.NotEqual));
            Assert.Equal(">", ConditionExpressionConverter.GetOperatorAsSQL(ConditionOperator.GreaterThan));
            Assert.Equal("<", ConditionExpressionConverter.GetOperatorAsSQL(ConditionOperator.LessThan));
            Assert.Equal(">=", ConditionExpressionConverter.GetOperatorAsSQL(ConditionOperator.GreaterEqual));
            Assert.Equal("<=", ConditionExpressionConverter.GetOperatorAsSQL(ConditionOperator.LessEqual));
            Assert.Equal("LIKE", ConditionExpressionConverter.GetOperatorAsSQL(ConditionOperator.Like));
            Assert.Equal("NOT LIKE", ConditionExpressionConverter.GetOperatorAsSQL(ConditionOperator.NotLike));
            Assert.Equal("IS NOT NULL", ConditionExpressionConverter.GetOperatorAsSQL(ConditionOperator.NotNull));
            Assert.Equal("IS NULL", ConditionExpressionConverter.GetOperatorAsSQL(ConditionOperator.Null));
            Assert.Equal("IN", ConditionExpressionConverter.GetOperatorAsSQL(ConditionOperator.In));
            Assert.Equal("NOT IN", ConditionExpressionConverter.GetOperatorAsSQL(ConditionOperator.NotIn));
        }
    }
}
