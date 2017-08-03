using NetCore.CacheManager;
using NetCore.GeolocationApp.Repositories.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace NetCore.GeolocationApp.Repositories
{
    public class GeolocationMemoryRepository : IGeolocationRepository
    {
        public GeolocationMemoryRepository(string pathDirectoryCache, int cacheDurationMiliseconds)
        {
            _cacheDurationMiliseconds = cacheDurationMiliseconds;
            if (_cacheManager == null)
                _cacheManager = new DataCacheManager(pathDirectoryCache);
            if (_memoryDataBase == null)
                _memoryDataBase = InitDatabase();
            if (_friends == null)
                _friends = InitListFriends();
            if (_follows == null)
                _follows = new List<FollowInfo>();
        }
        private static IDataCacheManager _cacheManager;
        private static List<UserInfoResponse> _memoryDataBase;
        private static List<FriendInfoResponse> _friends;
        private static List<FollowInfo> _follows;
        private int _cacheDurationMiliseconds;

        private List<UserInfoResponse> InitDatabase()
        {
            const string ListaUsuarios = "lista_usuarios";
            var data = _cacheManager.GetCache<List<UserInfoResponse>>(ListaUsuarios);
            if(data != null)
            {
                return data;
            }
            else
            {
                var list = new List<UserInfoResponse>()
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
                _cacheManager.SaveCache<List<UserInfoResponse>>(ListaUsuarios, list, 1000);
                return list;
            }   
        }

        public List<FriendInfoResponse> InitListFriends()
        {
            const string ListaFriends = "lista_friends";
            var data = _cacheManager.GetCache<List<FriendInfoResponse>>(ListaFriends);
            if (data != null)
            {
                return data;
            }
            else
            {
                var list = new List<FriendInfoResponse>()
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
                _cacheManager.SaveCache<List<FriendInfoResponse>>(ListaFriends, list, _cacheDurationMiliseconds);
                return list;
            }
            
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
            string cacheFileName = "follows";
            var cacheData = _cacheManager.GetCache<List<FollowInfo>>(cacheFileName);
            if(cacheData != null)
            {
                _follows = cacheData;
            }
            var query = _follows.SingleOrDefault(x => x.UserIdentifierFollower == userIdentifierFollower
            && x.UserIdentifier == userIdentifierOrigin);
            try
            {
                bool result = false;
                if (enable)
                {
                    if (query == null)
                        _follows.Add(new FollowInfo
                        {
                            UserIdentifier = userIdentifierOrigin,
                            UserIdentifierFollower = userIdentifierFollower
                        });
                    result = true;
                }
                else
                {
                    if (query != null)
                        _follows.Remove(query);
                    result = true;
                }
                _cacheManager.SaveCache<List<FollowInfo>>(cacheFileName, _follows, _cacheDurationMiliseconds);
                return result;
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
