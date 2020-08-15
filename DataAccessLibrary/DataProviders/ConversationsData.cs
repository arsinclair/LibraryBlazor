using System.Threading.Tasks;
using System.Collections.Generic;
using DataAccessLibrary.Models;
using DataAccessLibrary.DataProviders.Interfaces;

namespace DataAccessLibrary.DataProviders
{
    public class ConversationsData : IConversationsData
    {
        private readonly ISqlDataAccess _db;

        public ConversationsData(ISqlDataAccess db)
        {
            _db = db;
        }

        public Task<List<Conversation>> GetConversations(string whereClause, params string[] columns)
        {
            string _columnsJoined = string.Join(", ", columns);
            string _columns = string.IsNullOrEmpty(_columnsJoined) ? "*" : _columnsJoined;
            string _whereClause = !string.IsNullOrEmpty(whereClause) ? " where " + whereClause : "";
            string sql = $"select {_columns} from dbo.Conversations{_whereClause};";

            return _db.LoadData<Conversation, dynamic>(sql, new { });
        }

        public Task InsertConversation(Conversation conversation)
        {
            string sql = @"insert into dbo.Conversations (Id, CreatedOn, ModifiedOn, Subject) values (@Id, @CreatedOn, @ModifiedOn, @Subject);";

            return _db.SaveData(sql, conversation);
        }
    }
}