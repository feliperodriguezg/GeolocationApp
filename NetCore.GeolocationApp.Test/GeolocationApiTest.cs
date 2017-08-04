using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetCore.GeolocationApp.Controllers;
using NetCore.GeolocationApp.Enums;
using NetCore.GeolocationApp.Models;
using NetCore.GeolocationApp.WebApiModels;
using System.Linq;
using System.Net;
using System;

namespace NetCore.GeolocationApp.Test
{
    [TestClass]
    public class GeolocationApiTest: TestBase
    {
        #region Private methods
        private void SetCurrentPositionOkTest(string userIdentifier, string latitude, string longitude)
        {
            using (GeolocationController geolocationController = new GeolocationController(base._appSettings))
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
            using (DistanceController distanceController = new DistanceController(base._appSettings))
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
            using (GeolocationController controller = new GeolocationController(base._appSettings))
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
            using (GeolocationController controller = new GeolocationController(base._appSettings))
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
            using (FriendsController controller = new FriendsController(base._appSettings))
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

        private void AllowFollowTest(string userIdentifier, string userIdentifierNewFriend, bool follow)
        {
            using (FriendsController controller = new FriendsController(base._appSettings))
            {
                var response = controller.AllowFollow(userIdentifier, userIdentifierNewFriend, follow) as ObjectResult;
                Assert.IsTrue(response.StatusCode == (int)HttpStatusCode.OK);
                Assert.IsTrue(response.Value != null);
                Assert.IsTrue(response.Value is ApiResultResponse<ServiceResponse>);
                var data = response.Value as ApiResultResponse<ServiceResponse>;
                Assert.IsTrue(data.Ok);
                Assert.IsTrue(data.Data.Status == ResponseStatusTypes.Ok);
            }
        }

        private bool HasFriendGeolocationDataTest(string userIdentifier, string userIdentifierFriend)
        {
            bool hasDataLocation = false;
            using (FriendsController controller = new FriendsController(base._appSettings))
            {
                var response = controller.Get(userIdentifier) as ObjectResult;
                Assert.IsTrue(response.StatusCode == (int)HttpStatusCode.OK);
                Assert.IsTrue(response.Value != null);
                Assert.IsTrue(response.Value is ApiResultResponse<FriendInformationResponse>);
                var data = response.Value as ApiResultResponse<FriendInformationResponse>;
                Assert.IsTrue(data.Ok);
                Assert.IsTrue(data.Data.Status == ResponseStatusTypes.Ok);
                var query = data.Data.Friends.SingleOrDefault(x => x.UserIdentifier == userIdentifierFriend);
                hasDataLocation = query != null;
                if(hasDataLocation)
                    hasDataLocation = query.IsEnable && query.DistanceInfo != null;
            }
            return hasDataLocation;
        }

        private bool CanFollowTest(string userIdentifierFollower, string userIdentifierTarget)
        {
            using (FriendsController controller = new FriendsController(base._appSettings))
            {
                var response = controller.CanFollow(userIdentifierFollower, userIdentifierTarget) as ObjectResult;
                Assert.IsTrue(response.StatusCode == (int)HttpStatusCode.OK);
                Assert.IsTrue(response.Value != null);
                Assert.IsTrue(response.Value is ApiResultResponse<ServiceResponse>);
                var data = response.Value as ApiResultResponse<ServiceResponse>;
                Assert.IsTrue(data.Ok);
                Assert.IsTrue(data.Data.Status == ResponseStatusTypes.Ok);
                return (bool)data.Data.Data;
            }
        }
        #endregion

        #region Tests
        [TestMethod]
        public void TestFullProccess()
        {
            #region Constants
            const string userIdentifier1 = "frodriguez";
            const string latitude1 = "38.6819596";
            const string longitude1 = "-0.6012665";

            const string userIdentifier2 = "usertest";
            const string latitude2 = "38.3673834";
            const string longitude2 = "-0.4784841";
            #endregion

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

            #region Comprobar proceso de permitir y no permitir localizar a un amigo
            if (!isFriend1)
                Assert.Fail("user1 no es contacto de user2");

            bool canFollow = CanFollowTest(userIdentifier1, userIdentifier2);
            if(!canFollow)
            {
                /*
                 * En este punto, si obtenemos la lista de amigos del user1, el user2 debe aparecer como amigo
                 * pero no debe contener información de posicionamiento
                 */
                bool hasGeolocationData = HasFriendGeolocationDataTest(userIdentifier1, userIdentifier2);
                if (hasGeolocationData)
                    Assert.Fail("No debería contener datos de geoposicionamiento ni debería aparecer como localización habilitada para el user 2");

                AllowFollowTest(userIdentifier1, userIdentifier2, true);
                canFollow = CanFollowTest(userIdentifier1, userIdentifier2);
                if (!canFollow)
                    Assert.Fail("Error al actualizar los followers del user2");
            }
            else
            {
                AllowFollowTest(userIdentifier1, userIdentifier2, false);
                /*
                * En este punto, si obtenemos la lista de amigos del user1, el user2 debe aparecer como amigo
                * pero no debe contener información de posicionamiento
                */
                bool hasGeolocationData = HasFriendGeolocationDataTest(userIdentifier1, userIdentifier2);
                if (hasGeolocationData)
                    Assert.Fail("No debería contener datos de geoposicionamiento ni debería aparecer como localización habilitada para el user 2");

                canFollow = CanFollowTest(userIdentifier1, userIdentifier2);
                if(canFollow)
                    Assert.Fail("No debería poder seguirle al haber sido deshabilitado anteriormente");
                else
                    AllowFollowTest(userIdentifier1, userIdentifier2, false);
            }
            #endregion
        }
        #endregion
    }
 }
