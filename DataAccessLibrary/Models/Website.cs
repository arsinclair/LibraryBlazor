using System;

namespace DataAccessLibrary.Models
{
    public class Website
    {
        public Guid Id { get; set; } 

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }
    }
}