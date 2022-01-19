using System;
using System.Collections.Generic;
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

            var lst = new List<User>() {user, user2};
            SerializationWorker serializationWorker = new SerializationWorker();
            serializationWorker.Serialization<List<User>>(lst);
            Console.WriteLine(user.Nickname);
            DeserializationWorker deserializationWorker = new DeserializationWorker();
            var user5 = deserializationWorker.Deserialize<List<User>>(@"..\..\..\..\DAL\JSON files\Users.json");
            user5.Result.ForEach(x => Console.WriteLine(x.Nickname));
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDeserializationWorker, DeserializationWorker>();
        }
    }
}