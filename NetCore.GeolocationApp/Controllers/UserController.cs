using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using NetCore.GeolocationApp.WebApiModels;
using NetCore.GeolocationApp.Services;
using NetCore.CacheManager;
using NetCore.GeolocationApp.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NetCore.GeolocationApp.Controllers
{
    [Route("api/[controller]")]
    public class UserController : ApiControllerBase
    {
        private UsersService _services;
        public UsersService UsersService
        {
            get
            {
                if (_services == null)
                    _services = new UsersService(new DataCacheManager(""));
                return _services;
            }
        }

        [Route("Login")]
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var response = StatusCode(HttpStatusCode.OK, ResponseOk<ServiceResponse>());
            try
            {
                var result = UsersService.Authentication(new AuthenticationRequest
                {
                    Username = username,
                    Password = password
                });
                if (result.Status != Enums.ResponseStatusTypes.Ok)
                {
                    response.Value = ResponseError<ServiceResponse>(result, result.Status.Description());
                }
                else
                    response.Value = ResponseOk<ServiceResponse>(result);
            }
            catch (Exception ex)
            {
                response = StatusCode(HttpStatusCode.InternalServerError, ResponseError<ServiceResponse>(null, ex.Message));
            }
            return response;
        }

        [Route("Register")]
        [HttpPost]
        public IActionResult Register(string username, string password, string name, string email)
        {
            var response = StatusCode(HttpStatusCode.OK, ResponseOk<ServiceResponse>());
            try
            {
                var result = UsersService.RegisterNewUser(new RegisterNewUserRequest
                {
                    Username = username,
                    Password = password,
                    Name = name,
                    Email = email
                });
                if (result.Status != Enums.ResponseStatusTypes.Ok)
                {
                    response.Value = ResponseError<ServiceResponse>(result, result.Status.Description());
                }
                else
                    response.Value = ResponseOk<ServiceResponse>(result);
            }
            catch (Exception ex)
            {
                response = StatusCode(HttpStatusCode.InternalServerError, ResponseError<ServiceResponse>(null, ex.Message));
            }
            return response;
        }

        [Route("Info")]
        [HttpPost]
        public IActionResult GetUserAppInformation(string username)
        {
            var response = StatusCode(HttpStatusCode.OK, ResponseOk<ServiceResponse>());
            try
            {
                var result = UsersService.GetUserAppInformation(new UserAppInformationRequest
                {
                    Username = username
                });
                if (result.Status != Enums.ResponseStatusTypes.Ok)
                {
                    response.Value = ResponseError<ServiceResponse>(result, result.Status.Description());
                }
                else
                    response.Value = ResponseOk<ServiceResponse>(result);
            }
            catch (Exception ex)
            {
                response = StatusCode(HttpStatusCode.InternalServerError, ResponseError<ServiceResponse>(null, ex.Message));
            }
            return response;
        }

        [Route("Info")]
        [HttpPut]
        public IActionResult UpdateUserAppInformation(string username, string email, string name)
        {
            var response = StatusCode(HttpStatusCode.OK, ResponseOk<ServiceResponse>());
            try
            {
                var result = UsersService.UpdateUserAppInformation(new UpdateUserAppInformationRequest
                {
                    Username = username,
                    Email = email,
                    Name = name
                });
                if (result.Status != Enums.ResponseStatusTypes.Ok)
                {
                    response.Value = ResponseError<ServiceResponse>(result, result.Status.Description());
                }
                else
                    response.Value = ResponseOk<ServiceResponse>(result);
            }
            catch (Exception ex)
            {
                response = StatusCode(HttpStatusCode.InternalServerError, ResponseError<ServiceResponse>(null, ex.Message));
            }
            return response;
        }
    }
}
