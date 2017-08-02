using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCore.GeolocationApp.WebApiModels;
using NetCore.GeolocationApp.Services;
using System.Net;
using NetCore.GeolocationApp.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NetCore.GeolocationApp.Controllers
{
    [Route("api/[controller]")]
    public class FriendsController : ApiControllerBase
    {


        // GET: api/values
        [HttpGet]
        [Route("{userIdentifier}")]
        public IActionResult Get(string userIdentifier)
        {
            var response = StatusCode(HttpStatusCode.OK, ResponseOk<FriendInformationResponse>());
            try
            {
                var result = GeolocationService.GetFriends(new FriendsInformationRequest
                {
                    UserIdentifier = userIdentifier
                });
                if (result.Status == Enums.ResponseStatusTypes.Ok)
                {
                    var friends = result.Friends;
                    foreach (FriendInformation friend in friends)
                    {
                        if (friend.IsEnable)
                        {
                            friend.DistanceInfo = GeolocationService.GetCurrentDistance(userIdentifier, friend.UserIdentifier);
                        }
                    }
                    response.Value = ResponseOk<FriendInformationResponse>(result);
                }
                else
                    response = StatusCode(HttpStatusCode.BadRequest, ResponseError<FriendInformationResponse>(result, result.StatusText));
            }
            catch (Exception ex)
            {
                response = StatusCode(HttpStatusCode.InternalServerError, ResponseError<bool>(false, ex.Message));
            }
            return response;
        }

        [HttpPost]
        [Route("AllowFollow")]
        public IActionResult Post(string userIdentifier, string userIdentifierFriend, bool follow)
        {
            var response = StatusCode(HttpStatusCode.OK, ResponseOk<ServiceResponse>(new ServiceResponse()));
            try
            {
                var result = GeolocationService.UpdateFollow(new UpdateFollowRequest
                {
                    Allow = follow,
                    UserIdentifierFriend = userIdentifierFriend,
                    UserIdentifierFollower = userIdentifier
                });
                if (result.Status != Enums.ResponseStatusTypes.Ok)
                    response = StatusCode(HttpStatusCode.NotModified, ResponseOk<ServiceResponse>(result));
                else
                    response.Value = ResponseOk<ServiceResponse>(result);
            }
            catch(Exception ex)
            {
                response = StatusCode(HttpStatusCode.InternalServerError, ResponseError<bool>(false, ex.Message));
            }
            return response;
        }
    }
}
