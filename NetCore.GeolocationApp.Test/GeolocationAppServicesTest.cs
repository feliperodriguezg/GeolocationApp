using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetCore.GeolocationApp.Repositories;
using NetCore.GeolocationApp.Services;
using NetCore.GeolocationApp.Test.Dummies;
using System;

namespace NetCore.GeolocationApp.Test
{
    [TestClass]
    public class GeolocationAppServicesTest
    {
        public const string ApiKeyGoogleMaps = "AIzaSyAwEYAAUxnE9XmNOsIoFJhd-590PdBDZ_4";

        private void MessageOk(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private GeolocationService InitializeServices()
        {
            var service = new GeolocationService(ApiKeyGoogleMaps);
            service.Repository = new GeolocationMemoryRepository();
            var result1 = service.GetCurrentPositionUser(new WebApiModels.GeolocationRequest
            {
                UserIdentifier = RepositoryTest.UserIdentifierTest1
            });
            var result2 = service.GetCurrentPositionUser(new WebApiModels.GeolocationRequest
            {
                UserIdentifier = RepositoryTest.UserIdentifierTest2
            });
            Assert.IsTrue(result1.GeolocationEnabled);
            Assert.IsTrue(result2.GeolocationEnabled);
            return service;
        }

        [TestMethod]
        public void GetDistance()
        {
            string identifierOrigin = RepositoryTest.UserIdentifierTest1;
            string identifierDestination = RepositoryTest.UserIdentifierTest2;
            var service = InitializeServices();
            var positionUser = service.GetCurrentPositionUser(new WebApiModels.GeolocationRequest
            {
                UserIdentifier = identifierOrigin
            });
            if (String.IsNullOrEmpty(positionUser.Latitude) || String.IsNullOrEmpty(positionUser.Longitude))
            {
                positionUser.Latitude = RepositoryTest.LatitudeTest1;
                positionUser.Longitude = RepositoryTest.LongitudeTest1;
                service.SetCurrentPosition(new WebApiModels.CurrentPositionInfoRequest
                {
                    UserIdentifier = identifierOrigin,
                    Latitude = positionUser.Latitude,
                    Longitude = positionUser.Longitude
                });
                positionUser = service.GetCurrentPositionUser(new WebApiModels.GeolocationRequest
                {
                    UserIdentifier = identifierOrigin
                });
            }
            var positionUser2 = service.GetCurrentPositionUser(new WebApiModels.GeolocationRequest
            {
                UserIdentifier = identifierDestination
            });
            if (String.IsNullOrEmpty(positionUser2.Latitude) || String.IsNullOrEmpty(positionUser2.Longitude))
            {
                positionUser2.Latitude = RepositoryTest.LatitudeTest2;
                positionUser2.Longitude = RepositoryTest.LongitudeTest2;
                service.SetCurrentPosition(new WebApiModels.CurrentPositionInfoRequest
                {
                    UserIdentifier = identifierDestination,
                    Latitude = positionUser2.Latitude,
                    Longitude = positionUser2.Longitude
                });
                positionUser2 = service.GetCurrentPositionUser(new WebApiModels.GeolocationRequest
                {
                    UserIdentifier = identifierDestination
                });
            }
            var result = service.GetCurrentDistance(identifierOrigin, identifierDestination);
            Assert.IsNotNull(result.AddressOrigin);
            MessageOk("Dirección origen: " + result.AddressOrigin);
            Assert.IsNotNull(result.AddressDestination);
            MessageOk("Dirección destino: " + result.AddressDestination);
            Assert.IsNotNull(result.Distance);
            MessageOk("Distancia: " + result.Distance);
            Assert.IsTrue(result.DistanceValue > 0);
            MessageOk("Distancia valor: " + result.DistanceValue);
            Assert.IsNotNull(result.Duration);
            MessageOk("Duración: " + result.Duration);
            Assert.IsTrue(result.DurationValue > 0);
            MessageOk("Duración valor: " + result.DurationValue);
            
        }

        [TestMethod]
        public void UserPositionNotEnable()
        {
            string identifierOrigin = RepositoryTest.UserIdentifierTest1;
            string identifierDestination = RepositoryTest.UserIdentifierTest2;
            var service = InitializeServices();
            var positionUser = service.GetCurrentPositionUser(new WebApiModels.GeolocationRequest
            {
                UserIdentifier = identifierOrigin
            });
            if (String.IsNullOrEmpty(positionUser.Latitude) || String.IsNullOrEmpty(positionUser.Longitude))
            {
                positionUser.Latitude = RepositoryTest.LatitudeTest1;
                positionUser.Longitude = RepositoryTest.LongitudeTest1;
                service.SetCurrentPosition(new WebApiModels.CurrentPositionInfoRequest
                {
                    UserIdentifier = identifierOrigin,
                    Latitude = positionUser.Latitude,
                    Longitude = positionUser.Longitude
                });
                positionUser = service.GetCurrentPositionUser(new WebApiModels.GeolocationRequest
                {
                    UserIdentifier = identifierOrigin
                });
            }
                

            var positionUser2 = service.GetCurrentPositionUser(new WebApiModels.GeolocationRequest
            {
                UserIdentifier = identifierDestination
            });
            if (String.IsNullOrEmpty(positionUser2.Latitude) || String.IsNullOrEmpty(positionUser2.Longitude))
            {
                positionUser2.Latitude = RepositoryTest.LatitudeTest2;
                positionUser2.Longitude = RepositoryTest.LongitudeTest2;
                service.SetCurrentPosition(new WebApiModels.CurrentPositionInfoRequest
                {
                    UserIdentifier = identifierDestination,
                    Latitude = positionUser2.Latitude,
                    Longitude = positionUser2.Longitude
                });
                positionUser2 = service.GetCurrentPositionUser(new WebApiModels.GeolocationRequest
                {
                    UserIdentifier = identifierDestination
                });
            }

            service.EnableGeolocation(new WebApiModels.EnableGeolocationRequest
            {
                Enable = false,
                UserIdentifier = identifierDestination
            });
            var result = service.GetCurrentDistance(identifierOrigin, identifierDestination);
            Assert.IsTrue(result.Status == Enums.ResponseStatusTypes.UserPositionNotEnable);
            Console.WriteLine("result.Status: " + result.Status);

            service.EnableGeolocation(new WebApiModels.EnableGeolocationRequest
            {
                Enable = true,
                UserIdentifier = identifierDestination
            });
            var result2 = service.GetCurrentDistance(identifierOrigin, identifierDestination);
            Assert.IsTrue(result2.Status == Enums.ResponseStatusTypes.Ok);
        }

        [TestMethod]
        public void FollowTest()
        {
            string identifierOrigin = RepositoryTest.UserIdentifierTest1;
            string identifierDestination = RepositoryTest.UserIdentifierTest2;
            var service = InitializeServices();

            var response = service.UpdateFollow(new WebApiModels.UpdateFollowRequest
            {
                Allow = true,
                UserIdentifierFollower = identifierOrigin,
                UserIdentifierFriend = identifierDestination
            });
            Assert.IsTrue(response.Status == Enums.ResponseStatusTypes.Ok);
            var responseAllowFollow = service.AllowFollow(new WebApiModels.AllowFollowRequest
            {
                UserIdentifierFriend = identifierDestination,
                UserIdentifier = identifierOrigin
            });
            Assert.IsTrue(responseAllowFollow.Status == Enums.ResponseStatusTypes.Ok);
            var response2 = service.UpdateFollow(new WebApiModels.UpdateFollowRequest
            {
                Allow = false,
                UserIdentifierFollower = identifierOrigin,
                UserIdentifierFriend = identifierDestination
            });
            Assert.IsTrue(response2.Status == Enums.ResponseStatusTypes.Ok);
            var responseNotAllowFollow = service.AllowFollow(new WebApiModels.AllowFollowRequest
            {
                UserIdentifierFriend = identifierDestination,
                UserIdentifier = identifierOrigin
            });
            Assert.IsTrue(responseNotAllowFollow.Status == Enums.ResponseStatusTypes.NotAllowFollow);
        }
    }
}