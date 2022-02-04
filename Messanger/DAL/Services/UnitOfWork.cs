using System;
using System.Threading.Tasks;
using Core;
using DAL.DataBase;
using System.Data.Entity;
using DAL.Abstractions.Interfaces;

namespace DAL.Services;

public class UnitOfWork : IDisposable, IUnitOfWork
{
    private readonly DALContext _context;
    private DbContextTransaction _transaction;
    private GenericRepository<User> userRepository;
    private GenericRepository<Room> roomRepository;
    private bool _disposed = false;

    public UnitOfWork(DALContext context)
    {
        this._context = context;
    }
    
    public GenericRepository<User> UserRepository
    {
        get
        {
            if (this.userRepository == null)
            {
                this.userRepository = new GenericRepository<User>(_context);
            }

            return this.userRepository;
        }
    }

    public GenericRepository<Room> RoomRepository
    {
        get
        {
            if (this.roomRepository == null)
            {
                this.roomRepository = new GenericRepository<Room>(_context);
            }

            return this.roomRepository;
        }
    }

    public void CreateTransaction()
    {
        _transaction = (DbContextTransaction) _context.Database.BeginTransaction();
    }

    public void RollBack()
    {
        _transaction.Rollback();
        _transaction.Dispose();
        
    }

    public void Commit()
    {
        _transaction.Commit();
    }
    
    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    private async Task Dispose(bool disposing)
    {
        if (!this._disposed)
        {
            if (disposing)
            {
                await _context.DisposeAsync();
            }
        }

        this._disposed = true;
    }
    
    public async void Dispose()
    {
        await this.Dispose(true);
        GC.SuppressFinalize(this);
    }
}