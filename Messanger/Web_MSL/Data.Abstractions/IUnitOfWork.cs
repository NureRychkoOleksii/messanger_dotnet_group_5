namespace Web_MSL.Data.Abstractions;

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