using System;
using System.Collections.Generic;
using DataAccessLibrary.Models;

namespace DataAccessLibrary.Interfaces
{
    public interface IEntityRepository : IGenericRepository<Entity>
    {
        int Delete(Entity entity);
        int Delete(Guid id, string entityName);

        IEnumerable<Entity> Get(string entityName, string whereClause = "", int count = 0, params string[] columns);
        Entity GetById(string id, string entityName, bool allColumns);
        Entity GetById(EntityReference entityReference, params string[] allColumns);
    }
}