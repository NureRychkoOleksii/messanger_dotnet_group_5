using System.Collections.Generic;
using System.Text;
using Core.Models;
using Newtonsoft.Json.Serialization;

namespace Core
{
    public class Role : IdKey
    {
        public int RoleId { get; set; }
        
        public string RoleName;

        public bool ManageRoles = false;

        public bool RenameRoom = false;
    }
}