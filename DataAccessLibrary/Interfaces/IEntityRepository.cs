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

        Task<Entity> Get(string id, string entityName, bool allColumns);
        Task<Entity> Get(string id, string entityName, params string[] columns);

        Task<IEnumerable<Entity>> GetAll(string entityName, bool allColumns);
        Task<IEnumerable<Entity>> GetAll(string entityName, params string[] columns);
    }
}