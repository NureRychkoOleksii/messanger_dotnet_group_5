using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Channels;
using System.Threading.Tasks;
using Core;
using DAL.Abstractions.Interfaces;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace DAL.Services
{
    public class SerializationWorker : ISerializationWorker
    {
        public async Task Serialization(User user)
        {
            string path = Path.GetFullPath(@"..\..\..\..\DAL\JSON files\Users.json");
            Console.WriteLine(path);
            var file = File.ReadAllTextAsync(path);
            var x = JsonSerializer.Deserialize<List<User>>(file.Result);
            x.Add(user);
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                await JsonSerializer.SerializeAsync(fs, x);
                Console.WriteLine("Data has been saved");
            }
        }
        
        // var list = JsonConvert.DeserializeObject<List<Person>>(myJsonString);
        // list.Add(new Person(1234,"carl2");
        // var convertedJson = JsonConvert.SerializeObject(list, Formatting.Indented);
    }
}