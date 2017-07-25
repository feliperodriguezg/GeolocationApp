using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetCore.GeolocationApp.Services;
using NetCore.GeolocationApp.Test.Dummies;

namespace NetCore.GeolocationApp.Test
{
    [TestClass]
    public class GeolocationAppServicesTest
    {
        public const string ApiKeyGoogleMaps = "AIzaSyAwEYAAUxnE9XmNOsIoFJhd-590PdBDZ_4";

        private GeolocationService InitializeServices()
        {
            var service = new GeolocationService(ApiKeyGoogleMaps);
            service.Repository = new RepositoryTest();
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
            var positionUser2 = service.GetCurrentPositionUser(new WebApiModels.GeolocationRequest
            {
                UserIdentifier = identifierDestination
            });
            var result = service.GetCurrentDistance(identifierOrigin, identifierDestination);
            Assert.IsNotNull(result.AddressOrigin);
            Assert.IsNotNull(result.AddressDestination);
            Assert.IsNotNull(result.Distance);
            Assert.IsNotNull(result.Duration);
            Assert.IsTrue(result.DurationValue > 0);
            Assert.IsTrue(result.DistanceValue > 0);
        }
    }
}