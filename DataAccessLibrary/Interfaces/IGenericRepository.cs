using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DataAccessLibrary.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<int> Create(T entity);
        Task<T> Get(string id, string entityName, string columns);
        Task<IEnumerable<T>> GetAll(string entityName, string columns);
        Task<int> Update(T entity);
        Task<int> Delete(string id, string entityName);
    }
}