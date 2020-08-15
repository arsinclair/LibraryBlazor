using System.Threading.Tasks;
using System.Collections.Generic;
using DataAccessLibrary.Models;
using DataAccessLibrary.DataProviders.Interfaces;

namespace DataAccessLibrary.DataProviders
{
    public class ContactsData : IContactsData
    {
        private readonly ISqlDataAccess _db;

        public ContactsData(ISqlDataAccess db)
        {
            _db = db;
        }

        public Task<List<Contact>> GetContacts(string whereClause, params string[] columns)
        {
            string _columnsJoined = string.Join(", ", columns);
            string _columns = string.IsNullOrEmpty(_columnsJoined) ? "*" : _columnsJoined;
            string _whereClause = !string.IsNullOrEmpty(whereClause) ? " where " + whereClause : "";
            string sql = $"select {_columns} from dbo.Contacts{_whereClause};";

            return _db.LoadData<Contact, dynamic>(sql, new { });
        }

        public Task InsertPerson(Contact contact)
        {
            string sql = @"insert into dbo.Contacts (Id, FirstName, LastName, Email) values (@Id, @FirstName, @LastName, @Email);";

            return _db.SaveData(sql, contact);
        }
    }
}