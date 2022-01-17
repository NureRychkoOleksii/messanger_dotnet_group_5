using System.Threading.Tasks;
using Core;

namespace DAL.Abstractions.Interfaces
{
    public interface IDeserializationWorker
    {
        Task<User> Deserialize();
    }
}