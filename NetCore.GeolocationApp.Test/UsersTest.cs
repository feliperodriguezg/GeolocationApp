using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetCore.GeolocationApp.Controllers;
using NetCore.GeolocationApp.Models;
using NetCore.GeolocationApp.WebApiModels;
using System;
using System.Net;

namespace NetCore.GeolocationApp.Test
{
    [TestClass]
    public class UsersTest: TestBase
    {
        static string Username = "usernametest001";
        static string Password = "test";
        static string Email = Username + "@email.com";
        static string Name = "Test User " + Username;

        [TestMethod]
        public void RegisterNewUser()
        {
            DeleteUser();

            using (UserController controller = new UserController(base._appSettings))
            {
                var response = (ObjectResult)controller.Register(Username, Password, Name, Email);
                Assert.IsTrue(response.StatusCode == (int)HttpStatusCode.OK);
                Assert.IsNotNull(response.Value);
                Assert.IsInstanceOfType(response.Value, typeof(ApiResultResponse<ServiceResponse>));
                var data = response.Value as ApiResultResponse<ServiceResponse>;
                Assert.IsTrue(data.Ok);
            }
        }
        [TestMethod]
        public void LoginUser()
        {
            using (UserController controller = new UserController(base._appSettings))
            {
                var response = (ObjectResult)controller.Login(Username, Password);
                Assert.IsTrue(response.StatusCode == (int)HttpStatusCode.OK);
                Assert.IsNotNull(response.Value);
                Assert.IsInstanceOfType(response.Value, typeof(ApiResultResponse<ServiceResponse>));
                var data = response.Value as ApiResultResponse<ServiceResponse>;
                Assert.IsTrue(data.Ok);
            }
        }
        [TestMethod]
        public void GetInformationUser()
        {
            using (UserController controller = new UserController(base._appSettings))
            {
                var response = (ObjectResult)controller.GetUserAppInformation(Username);
                Assert.IsTrue(response.StatusCode == (int)HttpStatusCode.OK);
                Assert.IsNotNull(response.Value);
                Assert.IsInstanceOfType(response.Value, typeof(ApiResultResponse<ServiceResponse>));
                var data = response.Value as ApiResultResponse<ServiceResponse>;
                Assert.IsTrue(data.Ok);
            }
        }
        [TestMethod]
        public void UpdateInformationUser()
        {

        }
        [TestMethod]
        public void DeleteUser()
        {
            using (UserController controller = new UserController(base._appSettings))
            {
                var response = (ObjectResult)controller.DeleteUser(Username, base._appSettings.Value.PasswordAdmin);
                Assert.IsTrue(response.StatusCode == (int)HttpStatusCode.OK);
                Assert.IsNotNull(response.Value);
                Assert.IsInstanceOfType(response.Value, typeof(ApiResultResponse<ServiceResponse>));
                var data = response.Value as ApiResultResponse<ServiceResponse>;
                Assert.IsTrue(data.Ok);
            }
        }
        [TestMethod]
        public void BlockUser()
        {

        }
    }
}
