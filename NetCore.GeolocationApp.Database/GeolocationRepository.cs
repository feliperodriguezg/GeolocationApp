using System;
using System.Collections.Generic;
using NetCore.GeolocationApp.Repositories.Models;

namespace NetCore.GeolocationApp.Repositories
{
    public class GeolocationRepository : IGeolocationRepository
    {
        public void DisableGeolocation(string userIdentifier)
        {
            throw new NotImplementedException();
        }

        public void EnableGeolocation(string userIdentifier)
        {
            throw new NotImplementedException();
        }

        public bool ExistUser(string userIdentifier)
        {
            throw new NotImplementedException();
        }

        public CurrentUserPositionResponse GetCurrentUserPosition(string userIdentifier)
        {
            throw new NotImplementedException();
        }

        public List<FriendInfoResponse> GetFriends(string userIdentifier)
        {
            throw new NotImplementedException();
        }

        public void SetCurrentPosition(string userIdentifier, string latitude, string longitude)
        {
            throw new NotImplementedException();
        }

        public void UpdateCurrentTravelMode(string userIdentifier, string mode)
        {
            throw new NotImplementedException();
        }
    }
}
