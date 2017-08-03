using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.GeolocationApp.Repositories.Interfaces
{
    public interface IUsersRepository
    {
        bool Authenticate(string username, string password);
        bool RegisterNewUser(string passwordUser, UserData userData);
        bool UpdateUserInformation(object userIdentifier, UserData userData);
        bool DeleteUser(object userIdentifier);
        bool BlockUser(object userIdentifier, bool block);
        UserData GetUserData(object userIdentifier);
    }
}
