using System;
using System.Collections.Generic;
using DataAccessLibrary.Models;
using DataAccessLibrary.Query;

namespace DataAccessLibrary.Interfaces
{
    public interface IEntityRepository : IGenericRepository<Entity>
    {
        int Delete(Entity entity);
        int Delete(Guid id, string entityName);

        // This GET should be deleted
        IEnumerable<Entity> Get(string entityName, string whereClause = "", int count = 0, params string[] columns);
        IEnumerable<Entity> Get(QueryExpression query);
        
        // These GetById methods should be deleted
        Entity GetById(string id, string entityName, bool allColumns);
        Entity GetById(EntityReference entityReference, params string[] allColumns);
    }
}