using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Core;

namespace DAL.Abstractions.Interfaces
{
    public interface ISerializationWorker
    {
        Task Serialize<TEntity>(TEntity obj, string jsonFileName);
        Task<TEntity> Deserialize<TEntity>(string fileName);
    }
}