using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetCore.GeolocationApp.Controllers;
using NetCore.GeolocationApp.Enums;
using NetCore.GeolocationApp.Models;
using NetCore.GeolocationApp.WebApiModels;
using System.Linq;
using System.Net;

namespace NetCore.GeolocationApp.Test
{
    [TestClass]
    public class GeolocationApiTest
    {
        #region Private methods
        private void SetCurrentPositionOkTest(string userIdentifier, string latitude, string longitude)
        {
            using (GeolocationController geolocationController = new GeolocationController())
            {
                var resultCurrentPosition1 = (ObjectResult)geolocationController.PostCurrentPosition(userIdentifier, latitude, longitude);
                Assert.IsTrue(resultCurrentPosition1.StatusCode == (int)HttpStatusCode.OK);
                Assert.IsTrue(resultCurrentPosition1.Value != null);
                Assert.IsTrue(resultCurrentPosition1.Value is ApiResultResponse<ServiceResponse>);
                var responseCurrentPosition1 = (ApiResultResponse<ServiceResponse>)resultCurrentPosition1.Value;
                Assert.IsTrue(responseCurrentPosition1.Data == null);
                Assert.IsTrue(responseCurrentPosition1.Ok);
            }
        }

        private void GetDistanceOkTest(string userIdentifier1, string userIdentifier2, HttpStatusCode httpStatusCode, ResponseStatusTypes dataStatus)
        {
            using (DistanceController distanceController = new DistanceController())
            {
                var resultDistance = (ObjectResult)distanceController.Post(userIdentifier1, userIdentifier2);
                Assert.IsTrue(resultDistance.StatusCode == (int)httpStatusCode);
                Assert.IsTrue(resultDistance.Value is ApiResultResponse<DistanceResponse>);
                var distanceResponse = resultDistance.Value as ApiResultResponse<DistanceResponse>;
                var data = distanceResponse.Data;
                
                Assert.IsTrue((httpStatusCode == HttpStatusCode.OK && distanceResponse.Ok) || (httpStatusCode != HttpStatusCode.OK && !distanceResponse.Ok));
                Assert.IsTrue(data != null);
                Assert.IsTrue(distanceResponse.Data.Status == dataStatus);
                if (httpStatusCode == HttpStatusCode.OK)
                {
                    Assert.IsTrue(data.DurationValue > 0);
                    Assert.IsTrue(data.DistanceValue > 0);
                }
            }
        }

        private bool UserHasEnableGeoloactionTest(string userIdentifier)
        {
            using (GeolocationController controller = new GeolocationController())
            {
                var response = (ObjectResult)controller.EnableViewPosition(userIdentifier);
                Assert.IsTrue(response.StatusCode == (int)HttpStatusCode.OK);
                Assert.IsTrue(response.Value != null);
                Assert.IsTrue(response.Value is ApiResultResponse<ServiceResponse>);
                var data = (ApiResultResponse<ServiceResponse>)response.Value;
                Assert.IsTrue(data.Ok);
                Assert.IsTrue(data.Data != null);
                return data.Data.Status == Enums.ResponseStatusTypes.Ok;
            }
        }

        private void SetEnablePositionTest(string userIdentifier, bool enable)
        {
            using (GeolocationController controller = new GeolocationController())
            {
                var response = (ObjectResult)controller.EnableViewCurrentPosition(userIdentifier, enable);
                Assert.IsTrue(response.StatusCode == (int)HttpStatusCode.OK);
                Assert.IsTrue(response.Value is ApiResultResponse<ServiceResponse>);
                var dataResponse = response.Value as ApiResultResponse<ServiceResponse>;
                Assert.IsTrue(dataResponse.Data != null);
                Assert.IsTrue(dataResponse.Data.Status == Enums.ResponseStatusTypes.Ok);
            }
        }

        private void GetDistanceOk(string userIdentifier1, string userIdentifier2)
        {
            GetDistanceOkTest(userIdentifier1, userIdentifier2, HttpStatusCode.OK, ResponseStatusTypes.Ok);
        }

        private void GetDistance_Error_UserPositionNotEnable(string userIdentifier1, string userIdentifier2)
        {
            GetDistanceOkTest(userIdentifier1, userIdentifier2, HttpStatusCode.BadRequest, ResponseStatusTypes.UserPositionNotEnable);
        }

        private bool UserIsFriendOf(string userIdentifier1, string identifierFriend)
        {
            bool found = false;
            using (FriendsController controller = new FriendsController())
            {
                var response = controller.Get(userIdentifier1) as ObjectResult;
                Assert.IsTrue(response.StatusCode == (int)HttpStatusCode.OK);
                Assert.IsTrue(response.Value != null);
                Assert.IsTrue(response.Value is ApiResultResponse<FriendInformationResponse>);
                var data = response.Value as ApiResultResponse<FriendInformationResponse>;
                Assert.IsTrue(data.Ok);
                Assert.IsTrue(data.Data.Status == ResponseStatusTypes.Ok);
                found = data.Data.Friends.SingleOrDefault(x => x.UserIdentifier == identifierFriend) != null;
            }
            return found;
        }

        private void SetUserAsFriendTestOk(string userIdentifier, string userIdentifierNewFriend, bool follow)
        {
            using (FriendsController controller = new FriendsController())
            {
                var response = controller.Post(userIdentifier, userIdentifierNewFriend, follow) as ObjectResult;
                Assert.IsTrue(response.StatusCode == (int)HttpStatusCode.OK);
                Assert.IsTrue(response.Value != null);
                Assert.IsTrue(response.Value is ApiResultResponse<ServiceResponse>);
                var data = response.Value as ApiResultResponse<ServiceResponse>;
                Assert.IsTrue(data.Ok);
                Assert.IsTrue(data.Data.Status == ResponseStatusTypes.Ok);
            }
        }
        #endregion

        #region Tests
        [TestMethod]
        public void TestFullProccess()
        {
            const string userIdentifier1 = "frodriguez";
            const string latitude1 = "38.6819596";
            const string longitude1 = "-0.6012665";

            const string userIdentifier2 = "usertest";
            const string latitude2 = "38.3673834";
            const string longitude2 = "-0.4784841";

            #region User1 tiene activado geolocalización (OK)
            Assert.IsTrue(UserHasEnableGeoloactionTest(userIdentifier1));
            #endregion

            #region User2 tiene activado geolocalización (OK)
            Assert.IsTrue(UserHasEnableGeoloactionTest(userIdentifier2));
            #endregion

            #region Establecer posiciones actuales de user1 y user 2 (OK)
            SetCurrentPositionOkTest(userIdentifier1, latitude1, longitude1);
            SetCurrentPositionOkTest(userIdentifier2, latitude2, longitude2);
            #endregion

            #region Obtener distancia entre user1 y user2 (OK)
            GetDistanceOk(userIdentifier1, userIdentifier2);
            #endregion

            #region Deshabilitar geolocalización de user1 (OK)
            SetEnablePositionTest(userIdentifier1, false);
            #endregion

            #region Obtener distance entre user1 y user2 (Error)
            GetDistance_Error_UserPositionNotEnable(userIdentifier1, userIdentifier2);
            #endregion

            #region Habilitar geolocalización de user1 (OK)
            SetEnablePositionTest(userIdentifier1, true);
            #endregion

            #region Volver a obtener distancia entre user1 y user2 (OK)
            GetDistanceOk(userIdentifier1, userIdentifier2);
            #endregion

            #region Deshabilitar geolocalización de user2 (OK)
            SetEnablePositionTest(userIdentifier2, false);
            #endregion

            #region Obtener distance entre user1 y user2 (Error)
            GetDistance_Error_UserPositionNotEnable(userIdentifier1, userIdentifier2);
            #endregion

            #region Habilitar geolocalización de user1 (OK)
            SetEnablePositionTest(userIdentifier2, true);
            #endregion

            #region Volver a obtener distancia entre user1 y user2 (OK)
            GetDistanceOk(userIdentifier1, userIdentifier2);
            #endregion

            #region Comprobar si user1 es contacto de user2
            bool isFriend1 = UserIsFriendOf(userIdentifier1, userIdentifier2);
            #endregion

            #region Añadir a user1 como contacto de user2 si no lo es
            if (!isFriend1)
            {
                SetUserAsFriendTestOk(userIdentifier1, userIdentifier2, true);
            }
            #endregion

            #region comprobar si user2 es contacto de user1
            bool isFriend2 = UserIsFriendOf(userIdentifier2, userIdentifier1);
            #endregion

            #region Añadir a user2 como contacto de user1 si no lo es
            if(!isFriend2)
            {
                SetUserAsFriendTestOk(userIdentifier2, userIdentifier1, true);
            }
            #endregion
        }
        #endregion
    }
 }
