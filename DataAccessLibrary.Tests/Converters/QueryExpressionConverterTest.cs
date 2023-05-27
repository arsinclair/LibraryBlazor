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
    public class QueryExpressionConverterTest
    {
        public QueryExpressionConverterTest(DatabaseCacheFixture fixture)
        {
            CacheManager.SetBypassDatabaseCache(fixture);
        }

        [Fact]
        public void BuildEmptyOffsetClause()
        {
            var queryExpression = new QueryExpression();
            Assert.Equal(string.Empty, QueryExpressionConverter.BuildOffset(queryExpression));
            queryExpression.PageInfo = new PagingInfo();
            Assert.Equal(string.Empty, QueryExpressionConverter.BuildOffset(queryExpression));
        }

        [Fact]
        public void BuildOffsetClause()
        {
            var queryExpression = new QueryExpression();
            int rowsPerPage = 100;
            queryExpression.PageInfo = new PagingInfo()
            {
                Count = rowsPerPage
            };
            Assert.Equal("OFFSET 0 ROWS FETCH NEXT 100 ROWS ONLY", QueryExpressionConverter.BuildOffset(queryExpression));

            queryExpression.PageInfo.PageNumber = 2;
            Assert.Equal("OFFSET 200 ROWS FETCH NEXT 100 ROWS ONLY", QueryExpressionConverter.BuildOffset(queryExpression));
        }

        [Fact]
        public void BuildEmptyOrderByClause()
        {
            var queryExpressionNoOrderByClause = new QueryExpression();
            Assert.Equal(string.Empty, QueryExpressionConverter.BuildOrderByWithOffset(queryExpressionNoOrderByClause));

            var queryExpressionEmprtOrderByClause = new QueryExpression();
            queryExpressionEmprtOrderByClause.Orders.Add(new OrderExpression());
            Assert.Equal(string.Empty, QueryExpressionConverter.BuildOrderByWithOffset(queryExpressionEmprtOrderByClause));
        }

        [Fact]
        public void BuildSingleFieldOrderByClause()
        {
            var queryExpression = new QueryExpression();
            queryExpression.AddOrder("CreatedOn", OrderType.Ascending);
            Assert.Equal("ORDER BY[CreatedOn]ASC", QueryExpressionConverter.BuildOrderByWithOffset(queryExpression));
        }

        [Fact]
        public void BuildOrderByClauseWithFallbackField()
        {
            var queryExpression = new QueryExpression();
            queryExpression.AddOrder("SentOn", "ProvisionalSentOn", OrderType.Descending);
            
            // Single OrderBy
            Assert.Equal("ORDER BY COALESCE([SentOn],[ProvisionalSentOn])DESC", QueryExpressionConverter.BuildOrderByWithOffset(queryExpression));

            // Multiple OrderBy attributes
            queryExpression.AddOrder("CreatedOn", OrderType.Descending);
            Assert.Equal("ORDER BY COALESCE([SentOn],[ProvisionalSentOn])DESC,[CreatedOn]DESC", QueryExpressionConverter.BuildOrderByWithOffset(queryExpression));
        }

        [Fact]
        public void BuildMultiFieldOrderByClause()
        {
            var queryExpression = new QueryExpression()
            {
                Orders = {
                    new OrderExpression("FieldOne", OrderType.Descending),
                    new OrderExpression(), // Empty Order - should be removed
                    new OrderExpression("FieldTwo", OrderType.Ascending),
                    new OrderExpression() {
                        AttributeName = "FieldThree" // Order without OrderType - should default to ASC
                    }
                }
            };
            Assert.Equal("ORDER BY[FieldOne]DESC,[FieldTwo]ASC,[FieldThree]ASC", QueryExpressionConverter.BuildOrderByWithOffset(queryExpression));
        }

        [Fact]
        public void BuildIncorrectCharactersOrderByClause()
        {
            var queryExpression = new QueryExpression();
            queryExpression.Orders.Add(new OrderExpression("Incorrect Field'$", OrderType.Ascending));
            Assert.Throws<ArgumentException>(() => QueryExpressionConverter.BuildOrderByWithOffset(queryExpression));

            queryExpression.Orders.Clear();
            queryExpression.Orders.Add(new OrderExpression("CorrectField", "Incorrect Field'$", OrderType.Ascending));
            Assert.Throws<ArgumentException>(() => QueryExpressionConverter.BuildOrderByWithOffset(queryExpression));
        }

        [Fact]
        public void BuildFallbackAttributeOrderByClauseWithoutPrimaryAttribute()
        {
            var queryExpression = new QueryExpression();
            queryExpression.Orders.Add(new OrderExpression("", "ModifiedBy", OrderType.Ascending));
            Assert.Throws<ArgumentException>(() => QueryExpressionConverter.BuildOrderByWithOffset(queryExpression));
        }

        [Fact]
        public void BuildEmptyWhereClause()
        {
            var queryExpression = new QueryExpression();
            Assert.Equal("", QueryExpressionConverter.BuildWhereClause(queryExpression));
        }

        [Fact]
        public void BuildWhereClause()
        {
            // Just a basic test that something is returned, as all this logic is located in FilterExpressionConverter, and tested separately
            var queryExpression = new QueryExpression("Contact")
            {
                Criteria = {
                    Queries = {
                        new ConditionExpression() {
                            AttributeName = "FirstName",
                            EntityName = "Contact",
                            Operator = ConditionOperator.Equal,
                            Values = { "Michael" }
                        }
                    }
                }
            };
            Assert.Equal("WHERE([FirstName]='Michael')", QueryExpressionConverter.BuildWhereClause(queryExpression));
        }

        [Fact]
        public void BuildEmptyColumns()
        {
            var queryExpression = new QueryExpression();
            Assert.Equal("[Id]", QueryExpressionConverter.BuildColumns(queryExpression));

            queryExpression.ColumnSet.AddColumn("");
            Assert.Equal("[Id]", QueryExpressionConverter.BuildColumns(queryExpression));
        }

        [Fact]
        public void BuildColumnsAllColumns()
        {
            var queryExpression = new QueryExpression();
            queryExpression.ColumnSet.AllColumns = true;
            Assert.Equal("*", QueryExpressionConverter.BuildColumns(queryExpression));

            queryExpression.ColumnSet.AddColumns("TestColumn", "Id", "DateTime");
            Assert.Equal("*", QueryExpressionConverter.BuildColumns(queryExpression));
        }

        [Fact]
        public void BuildColumns()
        {
            var queryExpression = new QueryExpression();
            queryExpression.ColumnSet.AddColumn("FirstName");
            queryExpression.ColumnSet.AddColumns("LastName", "Age", "Gender");
            queryExpression.ColumnSet.AddColumn("AlternativeName2");

            Assert.Equal("[Id],[FirstName],[LastName],[Age],[Gender],[AlternativeName2]", QueryExpressionConverter.BuildColumns(queryExpression));
        }

        [Fact]
        public void BuildColumnsDuplicateColumnsRemoved()
        {
            var queryExpression = new QueryExpression();
            queryExpression.ColumnSet.AddColumns("FirstName", "Age", "Id", "FirstName", "Id", "LastName");

            Assert.Equal("[Id],[FirstName],[Age],[LastName]", QueryExpressionConverter.BuildColumns(queryExpression));
        }

        [Fact]
        public void BuildColumnsIncorrectNameShouldFail()
        {
            var queryExpression = new QueryExpression();
            queryExpression.ColumnSet.AddColumns("Firs tName", "A'ge", "I;d", "FirstName\b\b\b\n", "Id", "LastName");

            Assert.Throws<ArgumentException>(() => QueryExpressionConverter.BuildColumns(queryExpression));
        }

        [Fact]
        public void ConvertToSQLEmptyEntityNameShouldFail()
        {
            var queryExpressionUndefinedEntityName = new QueryExpression();
            var queryExpressionNullEntityName = new QueryExpression(null);
            var queryExpressionEmptyStringEntityName = new QueryExpression("");

            Assert.Throws<ArgumentNullException>(() => QueryExpressionConverter.ConvertToSQL(queryExpressionUndefinedEntityName));
            Assert.Throws<ArgumentNullException>(() => QueryExpressionConverter.ConvertToSQL(queryExpressionNullEntityName));
            Assert.Throws<ArgumentNullException>(() => QueryExpressionConverter.ConvertToSQL(queryExpressionEmptyStringEntityName));

            queryExpressionUndefinedEntityName.EntityName = ""; // set not through constructor
            Assert.Throws<ArgumentNullException>(() => QueryExpressionConverter.ConvertToSQL(queryExpressionUndefinedEntityName));
        }

        [Fact]
        public void ConvertToSQLSimpleQuery()
        {
            var queryExpression = new QueryExpression("Contact");
            queryExpression.ColumnSet.AddColumns("FirstName", "LastName", "Age");

            Assert.Equal("SELECT[Id],[FirstName],[LastName],[Age]FROM[Contacts];", QueryExpressionConverter.ConvertToSQL(queryExpression));
        }

        [Fact]
        public void ConvertToSQLWithWhereClauseQuery()
        {
            var queryExpression = new QueryExpression("Contact");
            queryExpression.ColumnSet.AddColumns("FirstName", "LastName", "Age");
            queryExpression.Criteria.AddQuery(new ConditionExpression("Age", ConditionOperator.NotEqual, 8));

            Assert.Equal("SELECT[Id],[FirstName],[LastName],[Age]FROM[Contacts]WHERE([Age]<>'8');", QueryExpressionConverter.ConvertToSQL(queryExpression));
        }

        [Fact]
        public void ConvertToSQLWithOrderByClauseQuery()
        {
            var queryExpression = new QueryExpression("Contact");
            queryExpression.ColumnSet.AddColumns("FirstName", "LastName", "Age");
            queryExpression.AddOrder("BirthDate", OrderType.Descending);

            Assert.Equal("SELECT[Id],[FirstName],[LastName],[Age]FROM[Contacts]ORDER BY[BirthDate]DESC;", QueryExpressionConverter.ConvertToSQL(queryExpression));
        }

        [Fact]
        public void ConvertToSQLWithOffsetAndEmptyOrderByShouldFail()
        {
            var queryExpression = new QueryExpression("Contact")
            {
                ColumnSet = new ColumnSet(true),
                PageInfo = new PagingInfo()
                {
                    PageNumber = 50,
                    Count = 10
                }
            };

            Assert.Throws<ArgumentException>(() => QueryExpressionConverter.ConvertToSQL(queryExpression));
        }

        [Fact]
        public void ConvertToSQLWithOffsetClauseQuery()
        {
            var queryExpression = new QueryExpression("Contact")
            {
                ColumnSet = new ColumnSet(true),
                Orders =
                {
                    new OrderExpression("CreatedOn", OrderType.Ascending)
                },
                PageInfo = new PagingInfo()
                {
                    PageNumber = 5,
                    Count = 10
                }
            };

            Assert.Equal("SELECT*FROM[Contacts]ORDER BY[CreatedOn]ASC OFFSET 50 ROWS FETCH NEXT 10 ROWS ONLY;", QueryExpressionConverter.ConvertToSQL(queryExpression));
        }

        [Fact]
        public void ConvertToSQLComplexQuery()
        {
            var queryExpression = new QueryExpression
            {
                EntityName = "Contact",
                ColumnSet = new ColumnSet("FirstName", "LastName", "lastname", "Age"),
                Criteria = new FilterExpression(LogicalOperator.And)
                {
                    Queries = {
                        new ConditionExpression("FirstName", ConditionOperator.Like, "Bob"),
                        new ConditionExpression("Age", ConditionOperator.GreaterThan, 8)
                    }
                },
                Orders =
                {
                    new OrderExpression("Age", OrderType.Descending),
                    new OrderExpression("FirstName", OrderType.Ascending)
                },
                PageInfo = new PagingInfo()
                {
                    PageNumber = 5,
                    Count = 10
                }
            };

            Assert.Equal("SELECT[Id],[FirstName],[LastName],[Age]FROM[Contacts]WHERE([FirstName]LIKE'%Bob%'AND[Age]>'8')ORDER BY[Age]DESC,[FirstName]ASC OFFSET 50 ROWS FETCH NEXT 10 ROWS ONLY;", QueryExpressionConverter.ConvertToSQL(queryExpression));
        }
    }
}
