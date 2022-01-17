using System;
using System.IO;
using System.Threading.Tasks;
using Core;
using DAL.Abstractions.Interfaces;
using DAL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Messanger
{
    class Program
    {
        static void Main(string[] args)
        {
            User user = new User {Nickname = "Moonler", Password = "1234"};
            User user2 = new User {Nickname = "Xami", Password = "12345"};
            SerializationWorker serializationWorker = new SerializationWorker();
            serializationWorker.Serialization(user2);
            serializationWorker.Serialization(user);
            Console.WriteLine(user.Nickname);
            DeserializationWorker deserializationWorker = new DeserializationWorker();
            // var user5 = deserializationWorker.Deserialize();
            // Console.WriteLine(user5.Result.Nickname);
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDeserializationWorker, DeserializationWorker>();
        }
    }
}