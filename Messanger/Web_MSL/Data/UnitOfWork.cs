using Microsoft.EntityFrameworkCore.Storage;
using Web_MSL.Data.Abstractions;
using Web_MSL.Models;

namespace Web_MSL.Data;

public class UnitOfWork : IDisposable, IUnitOfWork
{
    private readonly WebMSLContext _context = new WebMSLContext();
    private IDbContextTransaction _transaction;
    private GenericRepository<User> userRepository;
    private bool _disposed = false;

    // public UnitOfWork(DALContext context)
    // {
    //     this._context = context;
    // }
    
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

    public void CreateTransaction()
    {
        _transaction = _context.Database.BeginTransaction();
    }

    public void RollBack()
    {
        if (_transaction != null)
        {
            _transaction.Rollback();
            _transaction.Dispose();
        }

        else
        {
            _transaction = _context.Database.BeginTransaction();
            _transaction.Rollback();
            _transaction.Dispose();
        }
        
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