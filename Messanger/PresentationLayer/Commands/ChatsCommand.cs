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
        private readonly Actions _actions;

        public ChatsCommand(Session session, Actions actions)
        {
            _session = session;
            _actions = actions;
        }

        public override async Task ExecuteAsync(string action)
        {
            switch (action.Trim().ToLower())
            {
                case ConsolePages.ViewChatsPage:
                    await OpenViewChatsPage();
                    break;
                case ConsolePages.CreateChatPage:
                    await OpenCreateChatPage();
                    break;
                case ConsolePages.EnterChatPage:
                    await OpenEnterChatPage();
                    break;
                case ConsolePages.ExitChatPage:
                    OpenExitChatPage();
                    break;
            }
        }

        private async Task OpenViewChatsPage()
        {
            string pageContent;
            string result = await _actions.ActionViewChats(_session);
            Console.WriteLine($"\n{result}\n");

            if (result != Status.NotInTheRoom && result != Status.UserNotLoggedIn)
            {
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
            else if (result == Status.NotInTheRoom)
            {
                pageContent = string.Concat(
                    $"\nHello, {_session.CurrentUser.Nickname}!\n",
                    "\nGo to:\n\n",
                    "enter room\n",
                    "view rooms\n",
                    "my invitations\n",
                    "create room\n",
                    "logout\n",
                    "exit\n"
                );
            }
            else
            {
                pageContent = string.Concat(
                    "\nGo to:\n\n",
                    "login\n",
                    "register\n",
                    "exit\n"
                );
            }
            Console.WriteLine(pageContent);
        }

        public async Task OpenCreateChatPage()
        {
            string pageContent;
            Console.Write("Enter the name of the new chat: ");
            string newChatName = Console.ReadLine().Trim();

            Console.Write("Is the chat private? (yes or anything else): ");
            string isPrivate = Console.ReadLine().Trim().ToLower();

            string result = await _actions.ActionCreateChat(_session, newChatName, isPrivate, _session.CurrentRoom.Id);
            Console.WriteLine($"\n{result}\n");

            if (result != Status.NotInTheRoom && result != Status.UserNotLoggedIn)
            {
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
            else if (result == Status.NotInTheRoom)
            {
                pageContent = string.Concat(
                    $"\nHello, {_session.CurrentUser.Nickname}!\n",
                    "\nGo to:\n\n",
                    "enter room\n",
                    "view rooms\n",
                    "my invitations\n",
                    "create room\n",
                    "logout\n",
                    "exit\n"
                );
            }
            else
            {
                pageContent = string.Concat(
                    "\nGo to:\n\n",
                    "login\n",
                    "register\n",
                    "exit\n"
                );
            }
            Console.WriteLine(pageContent);

        }

        public async Task OpenEnterChatPage()
        {
            string pageContent;

            Console.Write("Enter the name of the chat to enter: ");
            string chatName = Console.ReadLine().Trim();

            string result = await _actions.ActionEnterChat(_session, chatName);
            Console.WriteLine($"\n{result}\n");

            if (result != Status.NotInTheRoom && result != Status.UserNotLoggedIn)
            {
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
            else if (result == Status.NotInTheRoom)
            {
                pageContent = string.Concat(
                    $"\nHello, {_session.CurrentUser.Nickname}!\n",
                    "\nGo to:\n\n",
                    "enter room\n",
                    "view rooms\n",
                    "my invitations\n",
                    "create room\n",
                    "logout\n",
                    "exit\n"
                );
            }
            else
            {
                pageContent = string.Concat(
                    "\nGo to:\n\n",
                    "login\n",
                    "register\n",
                    "exit\n"
                );
            }
            Console.WriteLine(pageContent);

            // if (_session.CurrentRoom != null)
            // {
            //
            //     bool hasEnteredChat = _session.EnterChat(_chatService.GetChat(chatName, _session.CurrentRoom));
            //
            //     if (hasEnteredChat)
            //     {
            //         Console.WriteLine($"Welcome to the chat {chatName}!");
            //         pageContent = string.Concat(
            //             "\nGo to:\n\n",
            //             "exit chat\n"
            //         );
            //     }
            //     else
            //     {
            //         Console.WriteLine($"No chat {chatName} exists in the current room.");
            //
            //         pageContent = string.Concat(
            //             "\nGo to:\n\n",
            //             "view users\n",
            //             "exit room\n",
            //             "create role\n",
            //             "delete tole\n",
            //             "view roles\n",
            //             "view chats\n",
            //             "create chat\n",
            //             "enter chat\n"
            //         );
            //     }
            // }
            // else
            // {
            //     Console.WriteLine("You are not in the room right now.");
            //     pageContent = string.Concat(
            //         $"\nHello, {_session.CurrentUser.Nickname}!\n",
            //         "\nGo to:\n\n",
            //         "view users\n",
            //         "exit room\n",
            //         "create role\n",
            //         "delete tole\n",
            //         "view roles\n",
            //         "create chat\n",
            //         "view chats\n",
            //         "enter chat\n"
            //     );
            // }
        }

        public void OpenExitChatPage()
        {
            string pageContent;
            string result = _actions.ActionExitChat(_session);
            Console.WriteLine($"\n{result}\n");

            if (result != Status.UserNotLoggedIn)
            {
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
            else
            {
                pageContent = string.Concat(
                    "\nGo to:\n\n",
                    "login\n",
                    "register\n",
                    "exit\n"
                );
            }
            Console.WriteLine(pageContent);
        }
    }
}
