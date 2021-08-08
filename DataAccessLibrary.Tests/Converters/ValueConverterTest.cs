using DataAccessLibrary.Converters;
using DataAccessLibrary.Models;
using System;
using Xunit;

namespace DataAccessLibrary.Tests.Converters
{
    public class ValueConverterTest
    {
        [Fact]
        public void ConvertToSQLUnrecognisedFieldTypeShouldFail()
        {
            Assert.Throws<NotImplementedException>(() => ValueConverter.ConvertToSQL("test", "testvalueksjldkj", false));
        }

        [Fact]
        public void ConvertToSQLTextField()
        {
            Assert.Equal("'1234'", ValueConverter.ConvertToSQL(1234, "Text", false));
            Assert.Equal("'%1234%'", ValueConverter.ConvertToSQL(1234, "Text", true));
        }

        [Fact]
        public void ConvertToSQLTextAreaField()
        {
            Assert.Equal("'%test%'", ValueConverter.ConvertToSQL("test", "TextArea", true));
            Assert.Equal("'test'", ValueConverter.ConvertToSQL("test", "TextArea", false));
        }

        [Fact]
        public void ConvertToSQLNumberField()
        {
            Assert.Equal("'42345'", ValueConverter.ConvertToSQL(42345, "Number", false));
        }

        [Fact]
        public void ConvertToSQLGuidField()
        {
            Guid id = Guid.NewGuid();
            Assert.Equal($"'{id}'", ValueConverter.ConvertToSQL(id, "Guid", false));
            Assert.Equal($"'%{id}%'", ValueConverter.ConvertToSQL(id, "Guid", true));
        }

        [Fact]
        public void ConvertToSQLBooleanField()
        {
            Assert.Equal("1", ValueConverter.ConvertToSQL(true, "Boolean", false));
            Assert.Equal("0", ValueConverter.ConvertToSQL(false, "Boolean", true));
            Assert.Throws<FormatException>(() => ValueConverter.ConvertToSQL("test invalid case", "Boolean", false));
            Assert.Throws<ArgumentNullException>(() => ValueConverter.ConvertToSQL(null, "Boolean", false));
        }

        [Fact]
        public void ConvertToSQLDateTimeField()
        {
            DateTime inputDateTime = DateTime.Parse("7/13/2021 10:54:27 PM");
            string UTCDateTimeString = inputDateTime.ToUniversalTime().ToString();
            
            Assert.Equal($"'{UTCDateTimeString}'", ValueConverter.ConvertToSQL(inputDateTime, "DateTime", true));
            Assert.Equal($"'{UTCDateTimeString}'", ValueConverter.ConvertToSQL(inputDateTime, "DateTime", false));

            Assert.Throws<FormatException>(() => ValueConverter.ConvertToSQL("test invalid case", "DateTime", false));
            Assert.Throws<FormatException>(() => ValueConverter.ConvertToSQL(UTCDateTimeString, "DateTime", false));
            Assert.Throws<ArgumentNullException>(() => ValueConverter.ConvertToSQL(null, "DateTime", false));
        }

        [Fact]
        public void ConvertToSQLEntityReferenceField()
        {
            Guid id = Guid.NewGuid();
            EntityReference ef = new EntityReference("contact", id);
            Assert.Equal($"'%{id}%'", ValueConverter.ConvertToSQL(ef, "EntityReference", true));
            Assert.Equal($"'{id}'", ValueConverter.ConvertToSQL(ef, "EntityReference", false));

            EntityReference efNoEntityName = new EntityReference() { Id = id };
            Assert.Equal($"'{id}'", ValueConverter.ConvertToSQL(efNoEntityName, "EntityReference", false));

            Assert.Equal($"'{id}'", ValueConverter.ConvertToSQL(id, "EntityReference", false)); // Guid, instead of EF
            Assert.Equal($"'{id}'", ValueConverter.ConvertToSQL(id.ToString(), "EntityReference", false)); // String instead of EF

            Assert.Throws<FormatException>(() => ValueConverter.ConvertToSQL("test invalid case", "EntityReference", false));
            Assert.Throws<ArgumentNullException>(() => ValueConverter.ConvertToSQL(null, "EntityReference", false));
        }

        [Fact]
        public void EscapeForSQLTest()
        {
            Assert.Equal("'this is an escaped ''piece'' of text'", ValueConverter.EscapeForSQL("this is an escaped 'piece' of text", false));
        }

        [Fact]
        public void EscapeForSQLLikeClauseTest()
        {
            Assert.Equal("'%this is an escaped ''piece'' of text%'", ValueConverter.EscapeForSQL("this is an escaped 'piece' of text", true));
        }

        [Fact]
        public void EscapeForSQLLikeClauseForbiddenCharactersTest()
        {
            Assert.Equal("'%part''ial [[]searc] [%][%] for [%]words[%] betw[_]een%'", ValueConverter.EscapeForSQL("part'ial [searc] %% for %words% betw_een", true));
        }
    }
}
