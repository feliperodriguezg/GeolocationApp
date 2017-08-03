using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.GeolocationApp.Repositories.Interfaces
{
    public interface IUsersRepository
    {
        bool Authenticate(string username, string password);
        bool AddUser(UserData data);
    }
}
