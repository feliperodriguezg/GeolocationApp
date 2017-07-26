using Microsoft.Extensions.Caching.Memory;
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

        public GeolocationMemoryRepository()
        {
            if(_memoryDataBase == null)
                _memoryDataBase = InitDatabase();
        }

        private static List<UserInfoTest> _memoryDataBase;
        private List<UserInfoTest> InitDatabase()
        {
            return new List<UserInfoTest>()
            {
                new UserInfoTest()
                {
                    UserIdentifier = "frodriguez",
                    EnableGeolocation = true
                },
                new UserInfoTest()
                {
                    UserIdentifier = "usertest",
                    EnableGeolocation = true
                }
            };
        }


        private UserInfoTest GetUserInfo(string userIdentifier)
        {
            UserInfoTest response = _memoryDataBase.SingleOrDefault(x => x.UserIdentifier == userIdentifier);
            return response;
        }

        public void DisableGeolocation(string userIdentifier)
        {
            var user = GetUserInfo(userIdentifier);
            if (user != null)
                user.EnableGeolocation = false;
        }

        public void EnableGeolocation(string userIdentifier)
        {
            var user = GetUserInfo(userIdentifier);
            if (user != null)
                user.EnableGeolocation = true;
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

        public void SetCurrentPosition(string userIdentifier, string latitude, string longitude)
        {
            var user = GetUserInfo(userIdentifier);
            if(user != null)
            {
                user.CurrentLatitude = latitude;
                user.CurrentLongitude = longitude;
            }
        }

        public void UpdateCurrentTravelMode(string userIdentifier, string mode)
        {
            var user = GetUserInfo(userIdentifier);
            if(user != null)
            {
                user.CurrentTravelMode = mode;
            }
        }
    }
}
