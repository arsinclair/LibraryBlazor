using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccessLibrary.Models;

namespace DataAccessLibrary.DataProviders.Interfaces
{
    public interface IConversationsData
    {
        Task<List<Conversation>> GetConversations(string whereClause, params string[] columns);
        Task InsertConversation(Conversation conversation);
    }
}