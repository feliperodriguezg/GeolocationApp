using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.GeolocationApp.Repositories.Interfaces
{
    public class InternalUserData: UserData
    {
        public string Password { get; set; }
        public bool IsBlocked { get; set; }
        public InternalUserData()
        {

        }
        public InternalUserData(UserData userData)
        {
            this.Username = userData.Username;
            this.Name = userData.Name;
            this.Email = userData.Email;
        }
    }
}
