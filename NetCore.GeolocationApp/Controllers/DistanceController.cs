using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NetCore.GeolocationApp.Helpers;
using NetCore.GeolocationApp.Services;
using NetCore.GeolocationApp.WebApiModels;
using System;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NetCore.GeolocationApp.Controllers
{
    [Route("api/[controller]")]
    public class DistanceController : GelocationControllerBase
    {
        public DistanceController(IOptions<AppSettings> appSettings): base(appSettings) { }

        // GET: api/values
        [HttpPost]
        public IActionResult Post(string userIdentifierOrigin, string userIdentifierDestination)
        {
            var response = StatusCode(HttpStatusCode.OK, ResponseOk<DistanceResponse>());
            try
            {
                var result = GeolocationService.GetCurrentDistance(userIdentifierOrigin, userIdentifierDestination);
                if (result.Status != Enums.ResponseStatusTypes.Ok)
                    response = StatusCode(HttpStatusCode.BadRequest, ResponseError<DistanceResponse>(new DistanceResponse
                    {
                        Status = result.Status
                    }, result.StatusText));
                else
                    response = StatusCode(HttpStatusCode.OK, ResponseOk<DistanceResponse>(result));
            }
            catch (Exception ex)
            {
                response = StatusCode(HttpStatusCode.InternalServerError, ResponseError<DistanceResponse>(null, ex.Message));
            }
            return response;
        }
    }
}
