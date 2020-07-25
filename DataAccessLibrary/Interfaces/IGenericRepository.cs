using System;

namespace DataAccessLibrary.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Guid Create(T entity);
        T GetById(string id, string entityName, params string[] columns);
        int Update(T entity);
        int Delete(string id, string entityName);
    }
}