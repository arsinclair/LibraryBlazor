using System;

namespace DataAccessLibrary.Models
{
    public class Contact
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }

        // Not Mapped
        public bool HiddenInUI { get; set; }
    }
}