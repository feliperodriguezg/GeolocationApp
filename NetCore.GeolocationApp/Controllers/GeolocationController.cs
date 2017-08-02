using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCore.GeolocationApp.Services;
using NetCore.GeolocationApp.WebApiModels;
using System.Net;
using NetCore.GeolocationApp.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NetCore.GeolocationApp.Controllers
{
    [Route("api/[controller]")]
    public class GeolocationController : ApiControllerBase
    {
        public class RequestCurrentPosition
        {
            public string userIdentifier { get; set; }
            public string latitude { get; set; }
            public string longitude { get; set; }
        }

        #region Get or Set current position for user
        [HttpPost]
        [Route("Current")]
        public IActionResult PostCurrentPosition(string userIdentifier, string latitude, string longitude)
        {
            var response = StatusCode(HttpStatusCode.OK, ResponseOk<ServiceResponse>());
            try
            {
                var result = GeolocationService.SetCurrentPosition(new CurrentPositionInfoRequest
                {
                    UserIdentifier = userIdentifier,
                    Latitude = latitude,
                    Longitude = longitude
                });
                if(result.Status == Enums.ResponseStatusTypes.Ok)
                {
                    var result2 = GeolocationService.GetCurrentPositionUser(new GeolocationRequest
                    {
                        UserIdentifier = userIdentifier
                    });
                    if (result2.Status == Enums.ResponseStatusTypes.Ok)
                        result.Message = string.Format("{0},{1}", result2.Latitude, result2.Longitude);
                    else
                    {
                        result = new ServiceResponse
                        {
                            Status = result2.Status
                        };
                    }
                    response = StatusCode(HttpStatusCode.OK, ResponseOk<ServiceResponse>());
                }
                else
                {
                    response = StatusCode(
                        HttpStatusCode.BadRequest, 
                        ResponseError<ServiceResponse>(new ServiceResponse
                        {
                            Status = result.Status
                        }, 
                        result.StatusText));
                }
            }
            catch (Exception ex)
            {
                response = StatusCode(HttpStatusCode.InternalServerError, ResponseError<ServiceResponse>(null, ex.Message));
            }
            return response;
        }

        [HttpGet]
        [Route("Current/{userIdentifier}")]
        public IActionResult GetCurrentPosition(string userIdentifier)
        {
            var response = StatusCode(HttpStatusCode.OK, ResponseOk<GeolocationResponse>());
            try
            {
                var result = GeolocationService.GetCurrentPositionUser(new GeolocationRequest
                {
                    UserIdentifier = userIdentifier
                });
                if(result.Status == Enums.ResponseStatusTypes.Ok)
                    response.Value = ResponseOk<GeolocationResponse>(result);
                else
                    response = StatusCode(HttpStatusCode.BadRequest, ResponseError<GeolocationResponse>(result, result.StatusText));
            }
            catch (Exception ex)
            {
                response = StatusCode(HttpStatusCode.InternalServerError, ResponseError<GeolocationResponse>(null, ex.Message));
            }
            return response;
        }
        #endregion

        #region View, enable or disable current user position
        [HttpGet]
        [Route("Current/Enable")]
        public IActionResult EnableViewPosition(string userIdentifier)
        {
            var response = StatusCode(HttpStatusCode.OK, ResponseOk<ServiceResponse>());
            try
            {
                var result = GeolocationService.UserHasEnableGeolocation(userIdentifier);
                if (result.Status == Enums.ResponseStatusTypes.Ok)
                    response.Value = ResponseOk<ServiceResponse>(result);
                else
                    response = StatusCode(HttpStatusCode.BadRequest, ResponseError<ServiceResponse>(result, result.StatusText));
            }
            catch (Exception ex)
            {
                response = StatusCode(HttpStatusCode.InternalServerError, ResponseError<ServiceResponse>(null, ex.Message));
            }
            return response;
        }

        [HttpPost]
        [Route("Current/Enable")]
        public IActionResult EnableViewCurrentPosition(string userIdentifier, bool enableViewPosition)
        {
            var response = StatusCode(HttpStatusCode.OK, ResponseOk<ServiceResponse>());
            try
            {
                var result = GeolocationService.EnableGeolocation(new EnableGeolocationRequest
                {
                    UserIdentifier = userIdentifier,
                    Enable = enableViewPosition
                });
                if(result.Status == Enums.ResponseStatusTypes.Ok)
                {
                    response.Value = ResponseOk<ServiceResponse>(new ServiceResponse
                    {
                        Status = result.Status
                    });
                }
                else
                    response = StatusCode(HttpStatusCode.BadRequest, ResponseError<ServiceResponse>(result, result.StatusText));
            }
            catch (Exception ex)
            {
                response = StatusCode(HttpStatusCode.InternalServerError, ResponseError<ServiceResponse>(null, ex.Message));
            }
            return response;
        }
        #endregion
    }
}
