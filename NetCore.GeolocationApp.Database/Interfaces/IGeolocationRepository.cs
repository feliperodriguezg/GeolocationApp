using NetCore.GeolocationApp.Repositories.Models;
using System.Collections.Generic;

namespace NetCore.GeolocationApp.Repositories
{
    public interface IGeolocationRepository
    {
        bool DisableGeolocation(string userIdentifier);
        bool EnableGeolocation(string userIdentifier);
        CurrentUserPositionResponse GetCurrentUserPosition(string userIdentifier);
        bool SetCurrentPosition(string userIdentifier, string latitude, string longitude);
        bool UpdateCurrentTravelMode(string userIdentifier, string mode);
        List<FriendInfoResponse> GetFriends(string userIdentifier);
        bool ExistUser(string userIdentifier);
        bool AllowFollow(string userIdentifierOrigin, string userIdentifierFollower, bool enable);
        bool IsFollowerOf(string userIdentifierOrigin, string userIdentifierFollower);
        UserInfoResponse GetUserInfo(string userIdentifier);
    }
}