using System.Collections.Generic;
using System.Text;
using Core.Models;
using Newtonsoft.Json.Serialization;

namespace Core
{
    public class Role
    {
        public string RoleName;

        public Dictionary<string, bool> Permissions = new Dictionary<string, bool>()
        {
            ["Manage roles"] = false,
            ["Rename room"] = false
        };
    }
}