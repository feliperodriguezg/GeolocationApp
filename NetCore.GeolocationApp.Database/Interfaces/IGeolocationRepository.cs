using NetCore.GeolocationApp.Repositories.Models;
using System.Collections.Generic;

namespace NetCore.GeolocationApp.Repositories
{
    public interface IGeolocationRepository
    {
        void DisableGeolocation(string userIdentifier);
        void EnableGeolocation(string userIdentifier);
        CurrentUserPositionResponse GetCurrentUserPosition(string userIdentifier);
        void SetCurrentPosition(string userIdentifier, string latitude, string longitude);
        void UpdateCurrentTravelMode(string userIdentifier, string mode);
        List<FriendInfoResponse> GetFriends(string userIdentifier);
        bool ExistUser(string userIdentifier);
    }
}