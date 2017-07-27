using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCore.GeolocationApp.WebApiModels;
using NetCore.GeolocationApp.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NetCore.GeolocationApp.Controllers
{
    [Route("api/[controller]")]
    public class FriendsController : Controller
    {
        private const string ApiKey = "AIzaSyAwEYAAUxnE9XmNOsIoFJhd-590PdBDZ_4";
        private static readonly GeolocationService _services = new GeolocationService(ApiKey);

        // GET: api/values
        [HttpGet]
        [Route("{userIdentifier}")]
        public FriendInformationResponse Get(string userIdentifier)
        {
            var response = new FriendInformationResponse();
            try
            {
                response = _services.GetFriends(new FriendsInformationRequest
                {
                    UserIdentifier = userIdentifier
                });
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
