using Microsoft.AspNetCore.Mvc;
using NetCore.GeolocationApp.Services;
using NetCore.GeolocationApp.WebApiModels;
using System;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NetCore.GeolocationApp.Controllers
{
    [Route("api/[controller]")]
    public class DistanceController : Controller
    {
        private const string ApiKey = "AIzaSyAwEYAAUxnE9XmNOsIoFJhd-590PdBDZ_4";
        private static readonly GeolocationService _services = new GeolocationService(ApiKey);

        // GET: api/values
        [HttpPost]
        public DistanceResponse Post(string userIdentifierOrigin, string userIdentifierDestination)
        {
            DistanceResponse response = new DistanceResponse();
            try
            {
                response = _services.GetCurrentDistance(userIdentifierOrigin, userIdentifierDestination);
            }
            catch (Exception ex)
            {
                response.Status = Enums.ResponseStatusTypes.UnknowError;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
