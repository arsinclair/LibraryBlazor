using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccessLibrary.Models;

namespace DataAccessLibrary.DataProviders.Interfaces
{
    public interface IMessagesData
    {
        Task<List<Message>> GetMessages(string whereClause, params string[] columns);
        Task InsertMessage(Message message);
    }
}