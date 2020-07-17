using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccessLibrary.Models;

namespace DataAccessLibrary.Interfaces
{
    public interface IEntityRepository : IGenericRepository<Entity>
    {
        Task<int> Delete(Entity entity);
        Task<int> Delete(Guid id, string entityName);

        Task<IEnumerable<Entity>> Get(string entityName, string whereClause = "", int count = 0, params string[] columns);
        Task<Entity> GetById(string id, string entityName, bool allColumns);
        Task<Entity> GetById(EntityReference entityReference, params string[] allColumns);
    }
}