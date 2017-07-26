using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCore.GeolocationApp.Services;
using NetCore.GeolocationApp.WebApiModels;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NetCore.GeolocationApp.Controllers
{
    [Route("api/[controller]")]
    public class GeolocationController : Controller
    {
        private const string ApiKey = "AIzaSyAwEYAAUxnE9XmNOsIoFJhd-590PdBDZ_4";
        private static readonly GeolocationService _services = new GeolocationService(ApiKey);

        #region Get or Set current position for user
        [HttpPost]
        [Route("Current")]
        public ServiceResponse PostCurrentPosition(string userIdentifier, string latitude, string longitude)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                response = _services.SetCurrentPosition(new CurrentPositionInfoRequest
                {
                    UserIdentifier = userIdentifier,
                    Latitude = latitude,
                    Longitude = longitude
                });
                var result = _services.GetCurrentPositionUser(new GeolocationRequest
                {
                    UserIdentifier = userIdentifier
                });
                if (result.Status == Enums.ResponseStatusTypes.Ok)
                    response.Message = string.Format("{0},{1}", result.Latitude, result.Longitude);
            }
            catch (Exception ex)
            {
                response.Status = Enums.ResponseStatusTypes.UnknowError;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpGet]
        [Route("Current/{userIdentifier}")]
        public GeolocationResponse GetCurrentPosition(string userIdentifier)
        {
            GeolocationResponse response = new GeolocationResponse();
            try
            {
                var result = _services.GetCurrentPositionUser(new GeolocationRequest
                {
                    UserIdentifier = userIdentifier
                });
                response.Status = result.Status;
            }
            catch (Exception ex)
            {
                response.Status = Enums.ResponseStatusTypes.UnknowError;
                response.Message = ex.Message;
            }
            return response;
        }
        #endregion

        #region View, enable or disable current user position
        [HttpGet]
        [Route("Current/Enable")]
        public ServiceResponse EnableViewPosition(string userIdentifier)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                var result = _services.GetCurrentPositionUser(new GeolocationRequest
                {
                    UserIdentifier = userIdentifier
                });
                response.Status = (result.GeolocationEnabled) ? Enums.ResponseStatusTypes.Ok: Enums.ResponseStatusTypes.UserPositionNotEnable;
            }
            catch (Exception ex)
            {
                response.Status = Enums.ResponseStatusTypes.UnknowError;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpPost]
        [Route("Current/Enable")]
        public ServiceResponse EnableViewCurrentPosition(string userIdentifier, bool enableViewPosition)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                _services.EnableGeolocation(new EnableGeolocationRequest
                {
                    UserIdentifier = userIdentifier,
                    Enable = enableViewPosition
                });
                response.Status = Enums.ResponseStatusTypes.Ok;
            }
            catch (Exception ex)
            {
                response.Status = Enums.ResponseStatusTypes.UnknowError;
                response.Message = ex.Message;
            }
            return response;
        }
        #endregion
    }
}
