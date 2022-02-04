using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Abstractions.Interfaces;
using Core;
using Core.Models;
using DAL.Abstractions.Interfaces;
using DAL.Services;

namespace BLL.Services
{
    public class ChatService : IChatService
    {
        private readonly IGenericRepository<Chat> _repository;
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        private readonly IRoomService _roomService;
        
        public ChatService(IGenericRepository<Chat> repository, IRoomService roomService)
        {
            _repository = repository;
            _roomService = roomService;
        }
        
        public async Task<bool> CreateChat(Chat chat, Room room)
        {
            if (!(await this.ChatExists(chat, room)))
            {
                _unitOfWork.CreateTransaction();
            
                try
                {
                    await _unitOfWork.ChatRepository.Insert(chat);

                    await _unitOfWork.SaveAsync();
                
                    _unitOfWork.Commit();
                
                }
                catch (Exception e)
                {
                    _unitOfWork.RollBack();
                }
                room.Chats.Add(chat);
                _roomService.UpdateRoom(room);
                return true;
            }
            else
            {
                return false;
            }
        }
        
        public async void DeleteChat(Chat chat)
        {
            _unitOfWork.CreateTransaction();
            
            try
            {
                _unitOfWork.ChatRepository.Delete(chat);

                await _unitOfWork.SaveAsync();
                
                _unitOfWork.Commit();
                
            }
            catch (Exception e)
            {
                _unitOfWork.RollBack();
            }
        }
        
        public async void UpdateChat(Chat chat)
        {
            _unitOfWork.CreateTransaction();
            
            try
            {
                _unitOfWork.ChatRepository.Update(chat);

                await _unitOfWork.SaveAsync();
                
                _unitOfWork.Commit();
                
            }
            catch (Exception e)
            {
                _unitOfWork.RollBack();
            }
        }
        
        public async Task<IEnumerable<Chat>> GetChats(Room room)
        {
            IEnumerable<Chat> users = null;
            
            _unitOfWork.CreateTransaction();
            
            try
            {
                var usersAsync = await _unitOfWork.ChatRepository.Get();

                users = usersAsync.Where(chat => chat.RoomId == room.Id);

                await _unitOfWork.SaveAsync();
                
                _unitOfWork.Commit();
                
            }
            catch (Exception e)
            {
                _unitOfWork.RollBack();
            }

            return users;
        }
        
        public async Task<bool> ChatExists(Chat chat, Room room)
        {
            var roomChats = await this.GetChats(room);
        
            var searchedChat = roomChats.Where(roomChat => roomChat.Name == chat.Name).FirstOrDefault();
        
            if (searchedChat != null)
            {
                return true;
            }
        
            return false;
        }
        
        public async Task<Chat> GetChat(string chatName, Room room)
        {
            var chatsAsync = await this.GetChats(room);
                
            var chats = chatsAsync.Where(chat => chat.Name == chatName)
                .FirstOrDefault();
            return chats;
        }
    }
}