using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Core.Models;
using DAL.Abstractions.Interfaces;

namespace DAL.Services
{
    public class Repository<T> : IRepository<T> where T : IdKey
    {
        private readonly ISerializationWorker _serializationWorker;
        private readonly AppSettings _appSettings;
        private List<T> data;

        public Repository(ISerializationWorker serializationWorker, AppSettings appSettings)
        {
            _serializationWorker = serializationWorker;
            _appSettings = appSettings;
        }

        public async Task<List<T>> GetAll()
        {
            return await _serializationWorker.Deserialize<List<T>>(_appSettings.Directory);
        }

        public async Task CreateObject(T obj)
        {
            data = await GetAll();
            data.Add(obj);
            await _serializationWorker.Serialize<List<T>>(data, _appSettings.Directory);
        }

        public async Task UpdateObject(T obj)
        {
            await DeleteObject(obj);
            await CreateObject(obj);
        }

        public async Task DeleteObject(T obj)
        {
            data = await GetAll();
            var objToRemove = data.FirstOrDefault(s => s.Id == obj.Id);
            data.Remove(objToRemove);
            await _serializationWorker.Serialize<List<T>>(data, _appSettings.Directory);
        }
    }
}