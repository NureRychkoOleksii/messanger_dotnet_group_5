using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Core;

namespace DAL.Abstractions.Interfaces
{
    public interface ISerializationWorker
    {
        Task Serialization(User user);
    }
}