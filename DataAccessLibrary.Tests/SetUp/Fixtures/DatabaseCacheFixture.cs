using DataAccessLibrary.Cache;
using DataAccessLibrary.Models.Metadata;
using System;
using System.Collections.Generic;
using Xunit;

namespace DataAccessLibrary.Tests.SetUp.Fixtures
{
    public class DatabaseCacheFixture : DatabaseCache, IDisposable
    {
        public DatabaseCacheFixture()
        {
            base._FieldTypes = PopulateFieldTypes();
            base._Entities = PopulateEntities();
            base._Fields = PopulateFields();
            base._TableNameCache = PopulateTableNames();
            base._FieldsByEntityName = base.GetAllEntityFields();
        }

        private Dictionary<Guid, SysFieldType> PopulateFieldTypes()
        {
            return new Dictionary<Guid, SysFieldType>()
            {
                { Guid.Parse("006AF2C2-75F7-47C8-9C99-465FFB1C9AE1"), new SysFieldType() { Id = Guid.Parse("006AF2C2-75F7-47C8-9C99-465FFB1C9AE1"), Name = "Text" } },
                { Guid.Parse("28252041-6A08-4CD1-8BDF-4BFE5D3153E2"), new SysFieldType() { Id = Guid.Parse("28252041-6A08-4CD1-8BDF-4BFE5D3153E2"), Name = "TextArea" } },
                { Guid.Parse("692E660D-59D2-4639-A0FA-505CEDEFBF32"), new SysFieldType() { Id = Guid.Parse("692E660D-59D2-4639-A0FA-505CEDEFBF32"), Name = "Number" } },
                { Guid.Parse("F7E381E5-1872-482E-ABA9-6751D671B5DD"), new SysFieldType() { Id = Guid.Parse("F7E381E5-1872-482E-ABA9-6751D671B5DD"), Name = "Guid" } },
                { Guid.Parse("2E027486-B023-4A20-A755-737C8243876C"), new SysFieldType() { Id = Guid.Parse("2E027486-B023-4A20-A755-737C8243876C"), Name = "Boolean" } },
                { Guid.Parse("5F7E56CB-D29C-4B48-97EC-F1F87B365DF4"), new SysFieldType() { Id = Guid.Parse("5F7E56CB-D29C-4B48-97EC-F1F87B365DF4"), Name = "DateTime" } },
                { Guid.Parse("FC4F8D8A-47EB-46D6-B130-F7A76E8F40E5"), new SysFieldType() { Id = Guid.Parse("FC4F8D8A-47EB-46D6-B130-F7A76E8F40E5"), Name = "EntityReference" } }
            };
        }

        private Dictionary<Guid, SysEntity> PopulateEntities()
        {
            return new Dictionary<Guid, SysEntity>()
            {
                { Guid.Parse("A2D89C4D-4DA6-4217-9312-8D6376475A63"), new SysEntity() { Id = Guid.Parse("A2D89C4D-4DA6-4217-9312-8D6376475A63"), Name = "Contact", NamePlural = "Contacts", DisplayName = "Contact", DisplayNamePlural = "Contacts", DatabaseTableName = "Contacts" } },
                { Guid.Parse("4B68859B-260C-4A65-9650-E86058B1A281"), new SysEntity() { Id = Guid.Parse("4B68859B-260C-4A65-9650-E86058B1A281"), Name = "City", NamePlural = "Cities", DisplayName = "City", DisplayNamePlural = "Cities", DatabaseTableName = "Cities" } },
                { Guid.Parse("6CDB2F19-5789-4B5D-83CD-1270449B3624"), new SysEntity() { Id = Guid.Parse("6CDB2F19-5789-4B5D-83CD-1270449B3624"), Name = "Note", NamePlural = "Notes", DisplayName = "Note", DisplayNamePlural = "Notes", DatabaseTableName = "Notes" } }
            };
        }

        private Dictionary<Guid, SysField> PopulateFields()
        {
            return new Dictionary<Guid, SysField>()
            {
                { Guid.Parse("FCB498B7-5729-4FC5-8438-139BB0838667"), new SysField() { Id = Guid.Parse("FCB498B7-5729-4FC5-8438-139BB0838667"), ParentEntity = base._Entities[Guid.Parse("A2D89C4D-4DA6-4217-9312-8D6376475A63")], Name = "CurrentCityId", DisplayName = "Current City Id", DatabaseColumnName = "CurrentCityId", Type = base._FieldTypes[Guid.Parse("F7E381E5-1872-482E-ABA9-6751D671B5DD")] } },
                { Guid.Parse("21D86D7F-341E-4EC4-BA65-1E8F90D0C56E"), new SysField() { Id = Guid.Parse("21D86D7F-341E-4EC4-BA65-1E8F90D0C56E"), ParentEntity = base._Entities[Guid.Parse("A2D89C4D-4DA6-4217-9312-8D6376475A63")], Name = "FirstName", DisplayName = "First Name", DatabaseColumnName = "FirstName", Type = base._FieldTypes[Guid.Parse("006AF2C2-75F7-47C8-9C99-465FFB1C9AE1")] } },
                { Guid.Parse("AF0D1AD9-B453-4C7E-BCE0-29B49E4E9A94"), new SysField() { Id = Guid.Parse("AF0D1AD9-B453-4C7E-BCE0-29B49E4E9A94"), ParentEntity = base._Entities[Guid.Parse("A2D89C4D-4DA6-4217-9312-8D6376475A63")], Name = "FullName", DisplayName = "Full Name", DatabaseColumnName = "FullName", Type = base._FieldTypes[Guid.Parse("006AF2C2-75F7-47C8-9C99-465FFB1C9AE1")] } },
                { Guid.Parse("7F5E363B-7DB8-4D3C-BF8F-2FDDFDA28D8E"), new SysField() { Id = Guid.Parse("7F5E363B-7DB8-4D3C-BF8F-2FDDFDA28D8E"), ParentEntity = base._Entities[Guid.Parse("A2D89C4D-4DA6-4217-9312-8D6376475A63")], Name = "GenderId", DisplayName = "Gender Id", DatabaseColumnName = "GenderId", Type = base._FieldTypes[Guid.Parse("FC4F8D8A-47EB-46D6-B130-F7A76E8F40E5")] } },
                { Guid.Parse("E8B45525-0115-4F50-ADD6-317781AB732D"), new SysField() { Id = Guid.Parse("E8B45525-0115-4F50-ADD6-317781AB732D"), ParentEntity = base._Entities[Guid.Parse("A2D89C4D-4DA6-4217-9312-8D6376475A63")], Name = "CountryOfOriginId", DisplayName = "Country Of Origin Id", DatabaseColumnName = "CountryOfOriginId", Type = base._FieldTypes[Guid.Parse("FC4F8D8A-47EB-46D6-B130-F7A76E8F40E5")] } },
                { Guid.Parse("DD04C861-91CB-4E63-8EF7-3B4AEEE30FDD"), new SysField() { Id = Guid.Parse("DD04C861-91CB-4E63-8EF7-3B4AEEE30FDD"), ParentEntity = base._Entities[Guid.Parse("A2D89C4D-4DA6-4217-9312-8D6376475A63")], Name = "ModifiedOn", DisplayName = "Modified On", DatabaseColumnName = "ModifiedOn", Type = base._FieldTypes[Guid.Parse("5F7E56CB-D29C-4B48-97EC-F1F87B365DF4")] } },
                { Guid.Parse("63254990-1BF5-4F4B-8233-3CF314379347"), new SysField() { Id = Guid.Parse("63254990-1BF5-4F4B-8233-3CF314379347"), ParentEntity = base._Entities[Guid.Parse("A2D89C4D-4DA6-4217-9312-8D6376475A63")], Name = "Description", DisplayName = "Description", DatabaseColumnName = "Description", Type = base._FieldTypes[Guid.Parse("28252041-6A08-4CD1-8BDF-4BFE5D3153E2")] } },
                { Guid.Parse("7BA55FA2-DA79-4478-B820-8C2AF35ECC83"), new SysField() { Id = Guid.Parse("7BA55FA2-DA79-4478-B820-8C2AF35ECC83"), ParentEntity = base._Entities[Guid.Parse("A2D89C4D-4DA6-4217-9312-8D6376475A63")], Name = "CityOfOriginId", DisplayName = "City Of Origin Id", DatabaseColumnName = "CityOfOriginId", Type = base._FieldTypes[Guid.Parse("FC4F8D8A-47EB-46D6-B130-F7A76E8F40E5")] } },
                { Guid.Parse("21E2EB48-F4DC-419B-9959-8E9AF0BAACD2"), new SysField() { Id = Guid.Parse("21E2EB48-F4DC-419B-9959-8E9AF0BAACD2"), ParentEntity = base._Entities[Guid.Parse("A2D89C4D-4DA6-4217-9312-8D6376475A63")], Name = "Id", DisplayName = "Contact Id", DatabaseColumnName = "Id", Type = base._FieldTypes[Guid.Parse("F7E381E5-1872-482E-ABA9-6751D671B5DD")] } },
                { Guid.Parse("482B2BA3-403E-4826-86EF-8F831A676D7C"), new SysField() { Id = Guid.Parse("482B2BA3-403E-4826-86EF-8F831A676D7C"), ParentEntity = base._Entities[Guid.Parse("A2D89C4D-4DA6-4217-9312-8D6376475A63")], Name = "BirthDate", DisplayName = "Birth Date", DatabaseColumnName = "BirthDate", Type = base._FieldTypes[Guid.Parse("5F7E56CB-D29C-4B48-97EC-F1F87B365DF4")] } },
                { Guid.Parse("3084AEEF-2D1A-4723-8E4E-96D3433469C7"), new SysField() { Id = Guid.Parse("3084AEEF-2D1A-4723-8E4E-96D3433469C7"), ParentEntity = base._Entities[Guid.Parse("A2D89C4D-4DA6-4217-9312-8D6376475A63")], Name = "LastName", DisplayName = "Last Name", DatabaseColumnName = "LastName", Type = base._FieldTypes[Guid.Parse("006AF2C2-75F7-47C8-9C99-465FFB1C9AE1")] } },
                { Guid.Parse("A9D450EE-5BC0-4898-A10A-FDD224ECAFB7"), new SysField() { Id = Guid.Parse("A9D450EE-5BC0-4898-A10A-FDD224ECAFB7"), ParentEntity = base._Entities[Guid.Parse("A2D89C4D-4DA6-4217-9312-8D6376475A63")], Name = "CreatedOn", DisplayName = "Created On", DatabaseColumnName = "CreatedOn", Type = base._FieldTypes[Guid.Parse("5F7E56CB-D29C-4B48-97EC-F1F87B365DF4")] } },
                { Guid.Parse("786A30D6-8345-43E3-8DB7-C09D16F3CC44"), new SysField() { Id = Guid.Parse("786A30D6-8345-43E3-8DB7-C09D16F3CC44"), ParentEntity = base._Entities[Guid.Parse("A2D89C4D-4DA6-4217-9312-8D6376475A63")], Name = "IsActive", DisplayName = "Is Active", DatabaseColumnName = "IsActive", Type = base._FieldTypes[Guid.Parse("2E027486-B023-4A20-A755-737C8243876C")] } },
                { Guid.Parse("CAA8E3BE-AD03-4A8B-8E76-4FF7950CB918"), new SysField() { Id = Guid.Parse("CAA8E3BE-AD03-4A8B-8E76-4FF7950CB918"), ParentEntity = base._Entities[Guid.Parse("A2D89C4D-4DA6-4217-9312-8D6376475A63")], Name = "Age", DisplayName = "Age", DatabaseColumnName = "Age", Type = base._FieldTypes[Guid.Parse("692E660D-59D2-4639-A0FA-505CEDEFBF32")] } },
                { Guid.Parse("E954D437-8E10-4226-8C1F-83F37015FA0F"), new SysField() { Id = Guid.Parse("E954D437-8E10-4226-8C1F-83F37015FA0F"), ParentEntity = base._Entities[Guid.Parse("A2D89C4D-4DA6-4217-9312-8D6376475A63")], Name = "Address", DisplayName = "Address", DatabaseColumnName = "Address", Type = base._FieldTypes[Guid.Parse("28252041-6A08-4CD1-8BDF-4BFE5D3153E2")] } },
                { Guid.Parse("EEAEC8C9-5C2C-4C68-8B1A-E281F426B226"), new SysField() { Id = Guid.Parse("EEAEC8C9-5C2C-4C68-8B1A-E281F426B226"), ParentEntity = base._Entities[Guid.Parse("A2D89C4D-4DA6-4217-9312-8D6376475A63")], Name = "ContactRepresentative", DisplayName = "Contact Representative", DatabaseColumnName = "ContactRepresentative", Type = base._FieldTypes[Guid.Parse("28252041-6A08-4CD1-8BDF-4BFE5D3153E2")] } },

                { Guid.Parse("2FF0183E-6838-4091-8976-20E14D0E144A"), new SysField() { Id = Guid.Parse("2FF0183E-6838-4091-8976-20E14D0E144A"), ParentEntity = base._Entities[Guid.Parse("4B68859B-260C-4A65-9650-E86058B1A281")], Name = "ModifiedOn", DisplayName = "Modified On", DatabaseColumnName = "ModifiedOn", Type = base._FieldTypes[Guid.Parse("5F7E56CB-D29C-4B48-97EC-F1F87B365DF4")] } },
                { Guid.Parse("9649E49B-6230-4627-B7FB-80B3D8D319F5"), new SysField() { Id = Guid.Parse("9649E49B-6230-4627-B7FB-80B3D8D319F5"), ParentEntity = base._Entities[Guid.Parse("4B68859B-260C-4A65-9650-E86058B1A281")], Name = "Description", DisplayName = "Description", DatabaseColumnName = "Description", Type = base._FieldTypes[Guid.Parse("006AF2C2-75F7-47C8-9C99-465FFB1C9AE1")] } },
                { Guid.Parse("BB9FB14E-BE2D-47EF-A4B9-8636C68A9676"), new SysField() { Id = Guid.Parse("BB9FB14E-BE2D-47EF-A4B9-8636C68A9676"), ParentEntity = base._Entities[Guid.Parse("4B68859B-260C-4A65-9650-E86058B1A281")], Name = "Id", DisplayName = "Id", DatabaseColumnName = "Id", Type = base._FieldTypes[Guid.Parse("F7E381E5-1872-482E-ABA9-6751D671B5DD")] } },
                { Guid.Parse("67F81DE6-38A2-412E-B760-AA634AEDEDF4"), new SysField() { Id = Guid.Parse("67F81DE6-38A2-412E-B760-AA634AEDEDF4"), ParentEntity = base._Entities[Guid.Parse("4B68859B-260C-4A65-9650-E86058B1A281")], Name = "CreatedOn", DisplayName = "Created On", DatabaseColumnName = "CreatedOn", Type = base._FieldTypes[Guid.Parse("5F7E56CB-D29C-4B48-97EC-F1F87B365DF4")] } },
                { Guid.Parse("B752BC26-5BAA-4663-A984-EE8982675798"), new SysField() { Id = Guid.Parse("B752BC26-5BAA-4663-A984-EE8982675798"), ParentEntity = base._Entities[Guid.Parse("4B68859B-260C-4A65-9650-E86058B1A281")], Name = "CountryId", DisplayName = "Country Id", DatabaseColumnName = "CountryId", Type = base._FieldTypes[Guid.Parse("F7E381E5-1872-482E-ABA9-6751D671B5DD")] } },

                { Guid.Parse("C303FA3E-5DB2-4D85-94D7-322C33ADCC97"), new SysField() { Id = Guid.Parse("C303FA3E-5DB2-4D85-94D7-322C33ADCC97"), ParentEntity = base._Entities[Guid.Parse("6CDB2F19-5789-4B5D-83CD-1270449B3624")], Name = "Collection", DisplayName = "Collection", DatabaseColumnName = "Collection", Type = base._FieldTypes[Guid.Parse("006AF2C2-75F7-47C8-9C99-465FFB1C9AE1")] } },
                { Guid.Parse("0D3C4187-8DFB-4470-A8BD-34EDA6850776"), new SysField() { Id = Guid.Parse("0D3C4187-8DFB-4470-A8BD-34EDA6850776"), ParentEntity = base._Entities[Guid.Parse("6CDB2F19-5789-4B5D-83CD-1270449B3624")], Name = "Id", DisplayName = "Note Id", DatabaseColumnName = "Id", Type = base._FieldTypes[Guid.Parse("F7E381E5-1872-482E-ABA9-6751D671B5DD")] } },
                { Guid.Parse("9BD0C23C-8BBE-49DC-B504-9AFC7C58BBD3"), new SysField() { Id = Guid.Parse("9BD0C23C-8BBE-49DC-B504-9AFC7C58BBD3"), ParentEntity = base._Entities[Guid.Parse("6CDB2F19-5789-4B5D-83CD-1270449B3624")], Name = "Text", DisplayName = "Text", DatabaseColumnName = "Text", Type = base._FieldTypes[Guid.Parse("28252041-6A08-4CD1-8BDF-4BFE5D3153E2")] } },
                { Guid.Parse("C0D8B524-9CC2-4D88-839B-CCCBDACFE63A"), new SysField() { Id = Guid.Parse("C0D8B524-9CC2-4D88-839B-CCCBDACFE63A"), ParentEntity = base._Entities[Guid.Parse("6CDB2F19-5789-4B5D-83CD-1270449B3624")], Name = "CreatedOn", DisplayName = "Created On", DatabaseColumnName = "CreatedOn", Type = base._FieldTypes[Guid.Parse("5F7E56CB-D29C-4B48-97EC-F1F87B365DF4")] } },
                { Guid.Parse("68612ADA-CC77-4C47-8B75-FD7155C8FB7E"), new SysField() { Id = Guid.Parse("68612ADA-CC77-4C47-8B75-FD7155C8FB7E"), ParentEntity = base._Entities[Guid.Parse("6CDB2F19-5789-4B5D-83CD-1270449B3624")], Name = "ModifiedOn", DisplayName = "Modified On", DatabaseColumnName = "ModifiedOn", Type = base._FieldTypes[Guid.Parse("5F7E56CB-D29C-4B48-97EC-F1F87B365DF4")] } }
            };
        }

        private Dictionary<string, string> PopulateTableNames()
        {
            return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "Contact", "Contacts" },
                { "City", "Cities" },
                { "Note", "Notes" }
            };
        }

        public void Dispose()
        {
            base._Entities = null;
            base._Fields = null;

            CacheManager.DisposeCache();
        }
    }

    [CollectionDefinition("DatabaseCache Collection")]
    public class DatabaseCacheCollection : ICollectionFixture<DatabaseCacheFixture>
    {
        // This class has no code, and is never created. Its purpose is to be the place to apply [CollectionDefinition] and all the ICollectionFixture<> interfaces.
    }
}