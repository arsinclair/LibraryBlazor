using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccessLibrary.Models;

namespace DataAccessLibrary.DataProviders.Interfaces
{
    public interface IContactsData
    {
        Task<List<Contact>> GetContacts(string whereClause, params string[] columns);
        Task InsertPerson(Contact contact);
    }
}
