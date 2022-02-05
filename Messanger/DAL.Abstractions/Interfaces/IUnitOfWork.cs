using System.Threading.Tasks;

namespace DAL.Abstractions.Interfaces
{

    public interface IUnitOfWork
    {
        void CreateTransaction();

        void RollBack();

        void Commit();

        Task SaveAsync();

        //Task Dispose(bool disposing);

        void Dispose();
    }
}