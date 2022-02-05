using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;
using BLL.Abstractions.Interfaces;
using Core.Models;
using PL.Abstractions.Interfaces;
using Messanger;

namespace PL.Commands
{
    class ChatsCommand : GenericCommand<string>
    {
        private readonly Session _session;
        private readonly IChatService _chatService;

        public ChatsCommand(Session session, IChatService chatService)
        {
            _session = session;
            _chatService = chatService;
        }

        public async Task ExecuteAsync(string action)
        {
            switch (action.Trim().ToLower())
            {
                case ConsolePages.ViewChatsPage:
                    OpenViewChatsPage();
                    break;
                case ConsolePages.CreateChatPage:
                    OpenCreateChatPage();
                    break;
                case ConsolePages.EnterChatPage:
                    OpenEnterChatPage();
                    break;
                case ConsolePages.ExitChatPage:
                    OpenExitChatPage();
                    break;
            }
        }

        public void OpenViewChatsPage()
        {
            string pageContent = string.Concat(
                "\nGo to:\n\n",
                "view users\n",
                "exit room\n",
                "create role\n",
                "delete tole\n",
                "view roles\n",
                "view chats\n",
                "create chat\n",
                "enter chat\n"
            );

            if (!_session.IsUserLoggedIn)
            {
                Console.WriteLine("\nError: not logged in\n\n");
                return;
            }

            if (_session.CurrentRoom != null)
            {
                var chats = _chatService.GetChats(_session.CurrentRoom);

                if (chats.Count() > 0)
                {
                    Console.WriteLine();

                    foreach (Chat chat in chats)
                    {
                        Console.WriteLine(chat.Name);
                    }

                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("\nYou have no chats.");
                }
            }
            else
            {
                Console.WriteLine("You are not in the room right now.");
            }

            Console.WriteLine(pageContent);
        }

        public void OpenCreateChatPage()
        {
            if (!_session.IsUserLoggedIn)
            {
                Console.WriteLine("\nError: not logged in\n\n");
                return;
            }

            if (_session.CurrentRoom != null)
            {
                string pageContent = string.Concat(
                    "\nGo to:\n\n",
                    "view users\n",
                    "exit room\n",
                    "create role\n",
                    "delete tole\n",
                    "view roles\n",
                    "view chats\n",
                    "create chat\n",
                    "enter chat\n"
                );

                Console.Write("Enter the name of the new chat: ");
                string newChatName = Console.ReadLine().Trim();

                while (String.IsNullOrEmpty(newChatName))
                {
                    Console.WriteLine("Chat name can not be empty.");
                    Console.Write("Enter the name of the new chat: ");
                    newChatName = Console.ReadLine().Trim();
                }

                Console.Write("Is the chat private? (yes / no): ");
                string isPrivate = Console.ReadLine().Trim().ToLower();

                while (!isPrivate.Equals("yes") && !isPrivate.Equals("no"))
                {
                    Console.WriteLine("Wrong. Type \"yes\" or \"no\" ");
                    isPrivate = Console.ReadLine().Trim();
                }

                bool isChatPrivate = isPrivate.Equals("yes") ? true : false;

                Chat newChat = new Chat()
                {
                    Name = newChatName,
                    IsPrivate = isChatPrivate,
                    RoomId = _session.CurrentRoom.Id
                };

                bool hasChatCreated = _chatService.CreateChat(newChat, _session.CurrentRoom);

                if (hasChatCreated)
                {
                    Console.WriteLine($"Chat {newChatName} successfully created!");
                }
                else
                {
                    Console.WriteLine($"Chat {newChatName} already exists.");
                }

                Console.WriteLine(pageContent);
            }
            else
            {
                Console.WriteLine("You are not in the room right now.");
            }

        }

        public void OpenEnterChatPage()
        {
            string pageContent;

            if (!_session.IsUserLoggedIn)
            {
                Console.WriteLine("\nError: not logged in\n\n");
                return;
            }

            if (_session.CurrentRoom != null)
            {
                Console.Write("Enter the name of the chat to enter: ");
                string chatName = Console.ReadLine().Trim();

                bool hasEnteredChat = _session.EnterChat(_chatService.GetChat(chatName, _session.CurrentRoom));

                if (hasEnteredChat)
                {
                    Console.WriteLine($"Welcome to the chat {chatName}!");
                    pageContent = string.Concat(
                        "\nGo to:\n\n",
                        "exit chat\n"
                    );
                }
                else
                {
                    Console.WriteLine($"No chat {chatName} exists in the current room.");

                    pageContent = string.Concat(
                        "\nGo to:\n\n",
                        "view users\n",
                        "exit room\n",
                        "create role\n",
                        "delete tole\n",
                        "view roles\n",
                        "view chats\n",
                        "create chat\n",
                        "enter chat\n"
                    );
                }
            }
            else
            {
                Console.WriteLine("You are not in the room right now.");
                pageContent = string.Concat(
                    $"\nHello, {_session.CurrentUser.Nickname}!\n",
                    "\nGo to:\n\n",
                    "view users\n",
                    "exit room\n",
                    "create role\n",
                    "delete tole\n",
                    "view roles\n",
                    "create chat\n",
                    "view chats\n",
                    "enter chat\n"
                );
            }
            Console.WriteLine(pageContent);
        }

        public void OpenExitChatPage()
        {
            if (!_session.IsUserLoggedIn)
            {
                Console.WriteLine("\nError: not logged in\n\n");
                return;
            }

            string pageContent = string.Concat(
                "\nGo to:\n\n",
                "view users\n",
                "exit room\n",
                "create role\n",
                "delete tole\n",
                "view roles\n",
                "view chats\n",
                "create chat\n",
                "enter chat\n"
            );

            bool hasExitedChat = _session.ExitChat();

            if (hasExitedChat)
            {
                Console.WriteLine("Successfully exited!");
            }
            else
            {
                Console.WriteLine("You are not in the chat right now.");
            }

            Console.WriteLine(pageContent);
        }
    }
}
