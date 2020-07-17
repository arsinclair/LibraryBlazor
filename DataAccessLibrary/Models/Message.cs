using System;

namespace DataAccessLibrary.Models
{
    public class Message
    {
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }

        public Guid FromContactId { get; set; }

        public Guid ToContactId { get; set; }

        public DateTime SentOn { get; set; }

        public string Text { get; set; }

        public Guid WebsiteId { get; set; }

        public Guid PlatformId { get; set; }

        public Guid ConversationId { get; set; }

        public string TextFormat { get; set; }
    }
}