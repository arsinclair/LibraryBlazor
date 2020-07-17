using System;

namespace DataAccessLibrary.Models
{
    public class Conversation
    {
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }

        public string Subject { get; set; }
    }
}