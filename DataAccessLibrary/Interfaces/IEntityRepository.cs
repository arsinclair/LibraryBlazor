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

        IEnumerable<Entity> Get(QueryExpression query);
        
        // These GetById methods should be deleted
        Entity GetById(string id, string entityName, bool allColumns);
        Entity GetById(EntityReference entityReference, params string[] allColumns);

        int RetrieveTotalRecordCount(string entityName);
        int RetrieveTotalRecordCount(string entityName, FilterExpression criteria);
    }
}