using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Models;

namespace BLL.Abstractions
{
    public interface IActions
    {
        public string ActionRegisterUser(ISession session, string userName, string password, string email);
    }
}