using Microsoft.Extensions.Caching.Memory;
using NetCore.GeolocationApp.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetCore.GeolocationApp.Repositories
{
    public class GeolocationMemoryRepository : IGeolocationRepository
    {
        public class UserInfoTest
        {
            public bool EnableGeolocation { get; set; }
            public string CurrentLatitude { get; set; }
            public string CurrentLongitude { get; set; }
            public string UserIdentifier { get; set; }
            public string CurrentTravelMode { get; set; }
        }

        public class FriendInfo
        {
            public string UserIdentifier { get; set; }
            public string Name { get; set; }
            public string UrlPhoto { get; set; }
            public string FriendOf { get; set; }
        }

        public class FollowInfo
        {
            public string UserIdentifier { get; set; }
            public string UserIdentifierFollower { get; set; }
        }

        public GeolocationMemoryRepository()
        {
            if(_memoryDataBase == null)
                _memoryDataBase = InitDatabase();
            if (_friends == null)
                _friends = InitListFriends();
            if (_follows == null)
                _follows = new List<FollowInfo>();
        }

        private static List<UserInfoResponse> _memoryDataBase;
        private static List<FriendInfoResponse> _friends;
        private static List<FollowInfo> _follows;

        private List<UserInfoResponse> InitDatabase()
        {
            return new List<UserInfoResponse>()
            {
                new UserInfoResponse()
                {
                    UserIdentifier = "frodriguez",
                    EnableGeolocation = true
                },
                new UserInfoResponse()
                {
                    UserIdentifier = "usertest",
                    EnableGeolocation = true
                }
            };
        }

        public List<FriendInfoResponse> InitListFriends()
        {
            return new List<FriendInfoResponse>()
            {
                new FriendInfoResponse
                {
                    Name = "Felipe Rodríguez",
                    UrlPhoto = "http://experience.grupandia.com/wp-content/uploads/2012/08/1.jpg",
                    UserIdentifier = "frodriguez",
                    FriendOf = "usertest"
                },
                new FriendInfoResponse
                {
                    Name = "User test",
                    UrlPhoto = "",
                    UserIdentifier = "usertest",
                    FriendOf = "frodriguez"
                }
            };
        }


        public UserInfoResponse GetUserInfo(string userIdentifier)
        {
            UserInfoResponse response = _memoryDataBase.SingleOrDefault(x => x.UserIdentifier == userIdentifier);
            return response;
        }

        public bool DisableGeolocation(string userIdentifier)
        {
            var user = GetUserInfo(userIdentifier);
            if (user != null)
            {
                user.EnableGeolocation = false;
                return true;
            }
            else
                return false;
        }

        public bool EnableGeolocation(string userIdentifier)
        {
            var user = GetUserInfo(userIdentifier);
            if (user != null)
            {
                user.EnableGeolocation = true;
                return true;
            }
            else
                return false;
                
        }

        public CurrentUserPositionResponse GetCurrentUserPosition(string userIdentifier)
        {
            CurrentUserPositionResponse response = new CurrentUserPositionResponse();
            var user = GetUserInfo(userIdentifier);
            if (user != null)
            {
                response.UserIdentifier = userIdentifier;
                response.EnableGeoposition = user.EnableGeolocation;
                response.Latitude = user.CurrentLatitude;
                response.Longitude = user.CurrentLongitude;
            }
            else
                response = null;
            return response;
        }

        public bool SetCurrentPosition(string userIdentifier, string latitude, string longitude)
        {
            var user = GetUserInfo(userIdentifier);
            if (user != null)
            {
                user.CurrentLatitude = latitude;
                user.CurrentLongitude = longitude;
                return true;
            }
            else
                return false;
        }

        public bool UpdateCurrentTravelMode(string userIdentifier, string mode)
        {
            var user = GetUserInfo(userIdentifier);
            if (user != null)
            {
                user.CurrentTravelMode = mode;
                return true;
            }
            else
                return false;
        }

        public List<FriendInfoResponse> GetFriends(string userIdentifier)
        {
            var result = new List<FriendInfoResponse>();
            var user = GetUserInfo(userIdentifier);
            if(user != null)
            {
                var query = _friends.Where(x => x.FriendOf == userIdentifier);
                if (query != null)
                    result = query.ToList();
            }
            return result;
        }

        public bool ExistUser(string userIdentifier)
        {
            return GetUserInfo(userIdentifier) == null ? false : true;
        }

        public bool AllowFollow(string userIdentifierOrigin, string userIdentifierFollower, bool enable)
        {
            var query = _follows.SingleOrDefault(x => x.UserIdentifierFollower == userIdentifierFollower
            && x.UserIdentifier == userIdentifierOrigin);
            try
            {
                if (enable)
                {
                    if (query == null)
                        _follows.Add(new FollowInfo
                        {
                            UserIdentifier = userIdentifierOrigin,
                            UserIdentifierFollower = userIdentifierFollower
                        });
                    return true;
                }
                else
                {
                    if (query != null)
                        _follows.Remove(query);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public bool IsFollowerOf(string userIdentifierOrigin, string userIdentifierFollower)
        {
            var query = _follows.SingleOrDefault(x => x.UserIdentifier == userIdentifierOrigin && x.UserIdentifierFollower == userIdentifierFollower);
            return query != null;
        }
    }
}
