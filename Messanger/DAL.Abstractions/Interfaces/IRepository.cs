using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;

namespace DAL.Abstractions.Interfaces
{
    public interface IRepository<T> where T : IdKey
    {
        Task<IEnumerable<T>> GetAllAsync(Type type);
        Task CreateObjectAsync(T obj);
        Task UpdateObjectAsync(T obj);
        Task DeleteObjectAsync(T obj);
    }
}