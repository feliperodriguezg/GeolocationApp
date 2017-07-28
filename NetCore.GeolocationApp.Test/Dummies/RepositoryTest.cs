using NetCore.GeolocationApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetCore.GeolocationApp.Repositories.Models;

namespace NetCore.GeolocationApp.Test.Dummies
{
    public class UserInfoTest
    {
        public bool EnableGeolocation { get; set; }
        public string CurrentLatitude { get; set; }
        public string CurrentLongitude { get; set; }
        public string UserIdentifier { get; set; }
        public string CurrentTravelMode { get; set; }
    }

    public class RepositoryTest : IGeolocationRepository
    {
        public const string UserIdentifierTest1 = "frodriguez";
        public const string LatitudeTest1 = "38.3589035";
        public const string LongitudeTest1 = "-0.4810262";
        public const string UserIdentifierTest2 = "usertest";
        public const string LatitudeTest2 = "38.3478913";
        public const string LongitudeTest2 = "-0.4940793";

        private List<UserInfoTest> _usersRepositoryTest;

        public RepositoryTest()
        {
            _usersRepositoryTest = new List<UserInfoTest>();
            _usersRepositoryTest.Add(new UserInfoTest
            {
                UserIdentifier = UserIdentifierTest1,
                EnableGeolocation = true,
                CurrentLatitude = LatitudeTest1,
                CurrentLongitude = LongitudeTest1
            });
            _usersRepositoryTest.Add(new UserInfoTest
            {
                UserIdentifier = UserIdentifierTest2,
                EnableGeolocation = true,
                CurrentLatitude = LatitudeTest2,
                CurrentLongitude = LongitudeTest2
            });
        }

        public void AllowFollow(string userIdentifierOrigin, string userIdentifierFollower, bool enable)
        {
            throw new NotImplementedException();
        }

        public void DisableGeolocation(string userIdentifier)
        {
            _usersRepositoryTest.Single(x => x.UserIdentifier == userIdentifier)
                .EnableGeolocation = false;
        }

        public void EnableGeolocation(string userIdentifier)
        {
            _usersRepositoryTest.Single(x => x.UserIdentifier == userIdentifier)
                .EnableGeolocation = true;
        }

        public bool ExistUser(string userIdentifier)
        {
            throw new NotImplementedException();
        }

        public CurrentUserPositionResponse GetCurrentUserPosition(string userIdentifier)
        {
            CurrentUserPositionResponse response = new CurrentUserPositionResponse();
            var query = _usersRepositoryTest.SingleOrDefault(x => x.UserIdentifier == userIdentifier);
            if(query != null)
            {
                response.EnableGeoposition = query.EnableGeolocation;
                response.UserIdentifier = query.UserIdentifier;
                response.Latitude = query.CurrentLatitude;
                response.Longitude = query.CurrentLongitude;
            }
            return response;
        }

        public List<FriendInfoResponse> GetFriends(string userIdentifier)
        {
            throw new NotImplementedException();
        }

        public bool IsFollowerOf(string userIdentifierOrigin, string userIdentifierFollower)
        {
            throw new NotImplementedException();
        }

        public void SetCurrentPosition(string userIdentifier, string latitude, string longitude)
        {
            var entity = _usersRepositoryTest.Single(x => x.UserIdentifier == userIdentifier);
            entity.CurrentLatitude = latitude;
            entity.CurrentLongitude = longitude;
        }

        public void UpdateCurrentTravelMode(string userIdentifier, string mode)
        {
            var userInfo = _usersRepositoryTest.Single(x => x.UserIdentifier == userIdentifier);
            userInfo.CurrentTravelMode = mode;
        }
    }
}
