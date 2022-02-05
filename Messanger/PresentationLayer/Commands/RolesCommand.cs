using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;
using BLL.Abstractions.Interfaces;
using Core;
using PL.Abstractions.Interfaces;
using Messanger;

namespace PL.Commands
{
    class RolesCommand : GenericCommand<string>
    {
        private readonly Session _session;
        private readonly IRoomService _roomService;
        private readonly IRoomUsersService _roomUsersService;

        public RolesCommand(Session session, IRoomService roomService, IRoomUsersService roomUsersService)
        {
            _session = session;
            _roomService = roomService;
            _roomUsersService = roomUsersService;
        }

        public async Task ExecuteAsync(string action)
        {
            switch (action.Trim().ToLower())
            {
                case ConsolePages.CreateRolePage:
                    OpenCreateRolePage();
                    break;
                case ConsolePages.DeleteRolePage:
                    OpenDeleteRolePage();
                    break;
                case ConsolePages.ViewRolesPage:
                    OpenViewRolesPage();
                    break;
            }
        }

        public void OpenCreateRolePage()
        {
            if (!_session.IsUserLoggedIn)
            {
                Console.WriteLine("\nError: not logged in\n\n");
                return;
            }

            Role userRole = _roomUsersService.GetUserRole(_session.CurrentUser, _session.CurrentRoom, out int roleId);
            string pageContent = string.Concat(
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

            if (_session.CurrentRoom.Roles[roleId].Permissions["Manage roles"])
            {
                Console.Write("Enter the name of the new role: ");
                string newRoleName = Console.ReadLine().Trim();

                while (String.IsNullOrEmpty(newRoleName))
                {
                    Console.WriteLine("Role name can not be empty.");
                    Console.Write("Enter the name of the new role: ");
                    newRoleName = Console.ReadLine().Trim();
                }

                bool hasCreatedRole = _roomService.CreateRole(newRoleName, _session.CurrentRoom);

                if (hasCreatedRole)
                {
                    Console.WriteLine($"New role {newRoleName} was successfully created!\n");
                }
                else
                {
                    Console.WriteLine($"Role {newRoleName} already exists.");
                }
            }
            else
            {
                Console.WriteLine("You don't have permissions to create roles.");
            }

            Console.WriteLine(pageContent);
        }

        public void OpenDeleteRolePage()
        {
            if (!_session.IsUserLoggedIn)
            {
                Console.WriteLine("\nError: not logged in\n\n");
                return;
            }

            Role userRole = _roomUsersService.GetUserRole(_session.CurrentUser, _session.CurrentRoom, out int roleId);
            string pageContent = string.Concat(
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

            if (_session.CurrentRoom.Roles[roleId].Permissions["Manage roles"])
            {
                Console.Write("Enter the name of the role to delete: ");
                string roleToDelete = Console.ReadLine().Trim();
                bool hasDeletedRole = _roomService.DeleteRole(roleToDelete, _session.CurrentRoom);

                if (hasDeletedRole)
                {
                    Console.WriteLine($"Role {roleToDelete} was successfully deleted!");
                }
                else
                {
                    Console.WriteLine($"No role {roleToDelete} exists.");
                }
            }
            else
            {
                Console.WriteLine("You don't have permissions to delete roles.");
            }

            Console.WriteLine(pageContent);
        }

        public void OpenViewRolesPage()
        {
            if (!_session.IsUserLoggedIn)
            {
                Console.WriteLine("\nError: not logged in\n\n");
                return;
            }

            IEnumerable<Role> roles = _roomService.GetAllRoles(_session.CurrentRoom);
            string pageContent = string.Concat(
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

            Console.WriteLine();
            foreach (Role role in roles)
            {
                Console.WriteLine(role.RoleName);
            }

            Console.WriteLine(pageContent);
        }
    }
}
