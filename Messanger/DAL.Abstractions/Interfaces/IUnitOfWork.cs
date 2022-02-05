using System.Threading.Tasks;
using Core;

namespace DAL.Abstractions.Interfaces
{

   public interface IUnitOfWork
{
    // public GenericRepository<User> UserRepository { get; }
    //
    // public GenericRepository<Room> RoomRepository { get; }

    void CreateTransaction();

        void RollBack();

        void Commit();

        Task SaveAsync();

        //Task Dispose(bool disposing);

        void Dispose();
    }
}