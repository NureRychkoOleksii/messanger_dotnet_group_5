using System.Collections.Generic;
using Core.Models;

namespace Core
{
    public class Role
    {
        public string RoleName;
        
        public Dictionary<string, bool> Permissions;
    }
}