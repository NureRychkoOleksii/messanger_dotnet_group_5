using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Core.Models;
using DAL.Abstractions.Interfaces;
using Microsoft.Extensions.Options;

namespace DAL.Services
{
    public class Repository<T> : IRepository<T> where T : IdKey
    {
        private readonly ISerializationWorker _serializationWorker;
        private readonly AppSettings _appSettings;
        private List<T> data;

        public Repository(ISerializationWorker serializationWorker, IOptions<AppSettings> appSettings)
        {
            _serializationWorker = serializationWorker;
            _appSettings = appSettings?.Value ?? throw new ArgumentNullException(nameof(appSettings));
            data = new List<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Type type)
        {
            var str = this.GetName(type);
            return await _serializationWorker.Deserialize<IEnumerable<T>>(str);
        }

        public async Task CreateObjectAsync(T obj)
        {
            var str = this.GetName(typeof(T));
            data = (await GetAllAsync(typeof(T))).ToList();
            int lastId = data.OrderBy(x => x.Id).Last().Id;
            obj.Id = ++lastId;
            data.Add(obj);
            await _serializationWorker.Serialize<List<T>>(data, str);
        }

        public async Task UpdateObjectAsync(T obj)
        {
            await DeleteObjectAsync(obj);
            await CreateObjectAsync(obj);
        }

        public async Task DeleteObjectAsync(T obj)
        {
            var str = this.GetName(typeof(T));
            data = (await GetAllAsync(typeof(T))).ToList();
            var objToRemove = data.FirstOrDefault(s => s.Id == obj.Id);
            data.Remove(objToRemove);
            await _serializationWorker.Serialize<List<T>>(data, str);
        }

        private string GetName(Type type)
        {
            string x = String.Empty;

            switch (type.Name)
            {
                case "Room":
                    x = _appSettings.RoomsDirectory;
                    break;
                
                case "User":
                    x = _appSettings.UsersDirectory;
                    break;
                
                case "RoomUsers":
                    x = _appSettings.RoomUsersDirectory;
                    break;
                
            }

            return x;
        }
    }
}