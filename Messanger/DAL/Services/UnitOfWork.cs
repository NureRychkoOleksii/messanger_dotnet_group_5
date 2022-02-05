using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Core;
using Core.Models;
using DAL.DataBase;
using DAL.Abstractions.Interfaces;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace DAL.Services
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private readonly DALContext _context = new DALContext();
    private IDbContextTransaction _transaction;
    private GenericRepository<User> userRepository;
    private GenericRepository<Room> roomRepository;
    private GenericRepository<RoomUsers> roomUsersRepository;
    private GenericRepository<Chat> chatRepository;
    private GenericRepository<UsersInvitation> usersInvitationRepository;
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
            _transaction = (DbContextTransaction)_context.Database.BeginTransaction();
        }

    public GenericRepository<RoomUsers> RoomUsersRepository
    {
        get
        {
            if (this.roomUsersRepository == null)
            {
                this.roomUsersRepository = new GenericRepository<RoomUsers>(_context);
            }

            return this.roomUsersRepository;
        }
    }
    public GenericRepository<Chat> ChatRepository
    {
        get
        {
            if (this.chatRepository == null)
            {
                this.chatRepository = new GenericRepository<Chat>(_context);
            }

            return this.chatRepository;
        }
    }
    
    public GenericRepository<UsersInvitation> UserInvitationRepository
    {
        get
        {
            if (this.usersInvitationRepository == null)
            {
                this.usersInvitationRepository = new GenericRepository<UsersInvitation>(_context);
            }

            return this.usersInvitationRepository;
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
}