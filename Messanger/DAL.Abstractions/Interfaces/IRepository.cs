using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;

namespace DAL.Abstractions.Interfaces
{
    public interface IRepository<T> where T : IdKey
    {
        Task<List<T>> GetAll();
        Task CreateObject(T obj);
        Task UpdateObject(T obj);
        Task DeleteObject(T obj);
    }
}