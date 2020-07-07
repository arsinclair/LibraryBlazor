using System.Threading.Tasks;
using System.Collections.Generic;
using DataAccessLibrary.Models;
using DataAccessLibrary.DataProviders.Interfaces;

namespace DataAccessLibrary.DataProviders
{
    public class MessagesData : IMessagesData
    {
        private readonly ISqlDataAccess _db;

        public MessagesData(ISqlDataAccess db)
        {
            _db = db;
        }

        public Task<List<Message>> GetMessages(string whereClause, params string[] columns)
        {
            string _columnsJoined = string.Join(", ", columns);
            string _columns = string.IsNullOrEmpty(_columnsJoined) ? "*" : _columnsJoined;
            string _whereClause = !string.IsNullOrEmpty(whereClause) ? " where " + whereClause: "";
            string sql = $"select {_columns} from dbo.Messages{_whereClause} order by SentOn ASC;";

            return _db.LoadData<Message, dynamic>(sql, new { });
        }

        public Task InsertMessage(Message message)
        {
            string sql = @"insert into dbo.Messages (MessageId, CreatedOn, ModifiedOn, FromContactId, ToContactId, SentOn, Text, WebsiteId, PlatformId, ConversationId, TextFormat) values (@MessageId, @CreatedOn, @ModifiedOn, @FromContactId, @ToContactId, @SentOn, @Text, @WebsiteId, @PlatformId, @ConversationId, @TextFormat);";

            return _db.SaveData(sql, message);
        }
    }
}