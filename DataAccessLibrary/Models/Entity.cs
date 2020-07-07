using System.Collections.Generic;
using System;

namespace DataAccessLibrary.Models
{
    public class Entity
    {
        public Entity()
        {
        }

        public Guid Id { get; set; }
        public string EntityName { get; set; }
        public Dictionary<string, Field> Fields { get; set; }
    }
}