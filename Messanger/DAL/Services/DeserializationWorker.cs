using System.IO;
using System.Threading.Tasks;
using Core;
using DAL.Abstractions.Interfaces;
using System.Text.Json;

namespace DAL.Services
{
    public class DeserializationWorker : IDeserializationWorker
    {
        public async Task<User> Deserialize()
        {
            string path = Path.GetFullPath(@"..\..\..\..\DAL\JSON files\Users.json");
            var objectJsonFile = File.ReadAllTextAsync(path);
            return JsonSerializer.Deserialize<User>(objectJsonFile.Result);
        }
    }
}