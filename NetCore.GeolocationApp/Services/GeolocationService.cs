using System;
using System.Linq;
using NetCore.GeolocationApp.WebApiModels;
using NetCore.GeolocationApp.GoogleMapsApi;
using NetCore.GeolocationApp.Repositories;
using NetCore.GeolocationApp.Enums;

namespace NetCore.GeolocationApp.Services
{
    public class GeolocationService
    {
        #region Members
        private GoogleMapsApiService _googleMapsApi;
        private IGeolocationRepository _repository;
        private string _pathDirectoryCacheInfo;
        #endregion

        #region Properties
        public IGeolocationRepository Repository
        {
            get
            {
                if (_repository == null)
                    _repository = InstanceDefaultRepository();
                return _repository;
            }
            set
            {
                _repository = value;
            }
        }
        #endregion

        #region Constructor
        public GeolocationService(string apiKey)
        {
            _googleMapsApi = new GoogleMapsApiService(apiKey);
            _repository = InstanceDefaultRepository();
        }
        public GeolocationService(string apiKey, string pathDirectoryCacheInfo)
        {
            _googleMapsApi = new GoogleMapsApiService(apiKey);
            _repository = InstanceDefaultRepository();
            _pathDirectoryCacheInfo = pathDirectoryCacheInfo;
        }
        #endregion

        #region Public
        public virtual FriendInformationResponse GetFriends(FriendsInformationRequest request)
        {
            var response = new FriendInformationResponse();
            try
            {
                if(!_repository.ExistUser(request.UserIdentifier))
                {
                    response.Status = ResponseStatusTypes.UserNotFound;
                }
                else
                {
                    var result = _repository.GetFriends(request.UserIdentifier);
                    if (result != null)
                    {
                        foreach (var item in result)
                        {
                            response.Friends.Add(new FriendInformation
                            {
                                UserIdentifier = item.UserIdentifier,
                                IsEnable = _repository.IsFollowerOf(item.UserIdentifier, request.UserIdentifier),
                                Name = item.Name,
                                UrlPhoto = item.UrlPhoto
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatusTypes.UnknowError;
                response.Message = ex.Message;
            }
            return response;
        }

        public virtual ServiceResponse UpdateFollow(UpdateFollowRequest request)
        {
            ServiceResponse response = new ServiceResponse();
            if (!_repository.ExistUser(request.UserIdentifierFriend) || 
                !_repository.ExistUser(request.UserIdentifierFollower))
                response.Status = ResponseStatusTypes.UserNotFound;
            else
            {
                _repository.AllowFollow(
                    request.UserIdentifierFriend, 
                    request.UserIdentifierFollower, 
                    request.Allow);
                response.Status = ResponseStatusTypes.Ok;
            }
            return response;
        }

        public ServiceResponse CanFollow(CanFollowRequest request)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                if(String.IsNullOrEmpty(request.userIdentifierFollower) || String.IsNullOrEmpty(request.userIdentifierTarget))
                    response.Status = ResponseStatusTypes.UserNotFound;
                else
                {
                    bool existUser = _repository.ExistUser(request.userIdentifierFollower) && _repository.ExistUser(request.userIdentifierTarget);
                    if (!existUser)
                        response.Status = ResponseStatusTypes.UserNotFound;
                    else
                        response.Data = _repository.IsFollowerOf(request.userIdentifierTarget, request.userIdentifierFollower);
                }
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatusTypes.UnknowError;
                response.Message = ex.Message;
            }
            return response;
        }

        public virtual ServiceResponse AllowFollow(AllowFollowRequest request)
        {
            ServiceResponse response = new ServiceResponse();
            bool isFollower = _repository.IsFollowerOf(request.UserIdentifierFriend, request.UserIdentifier);
            if (!isFollower)
                response.Status = ResponseStatusTypes.NotAllowFollow;
            return response;
        }

        public virtual ServiceResponse SetCurrentPosition(CurrentPositionInfoRequest request)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                if(String.IsNullOrEmpty(request.UserIdentifier))
                {
                    response.Status = ResponseStatusTypes.UserNotFound;
                }
                else if(String.IsNullOrEmpty(request.Latitude) || String.IsNullOrEmpty(request.Longitude))
                {
                    response.Status = ResponseStatusTypes.UserPositionNotFound;
                }
                else
                {
                    bool result = _repository.SetCurrentPosition(request.UserIdentifier, request.Latitude, request.Longitude);
                    response.Status = (result) ? ResponseStatusTypes.Ok : ResponseStatusTypes.UpdateFail;
                }
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatusTypes.UnknowError;
                response.Message = ex.Message;
            }
            return response;
        }

        public virtual ServiceResponse EnableGeolocation(EnableGeolocationRequest request)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                bool enable = request.Enable;
                bool result = false;
                if (enable)
                {
                    result = _repository.EnableGeolocation(request.UserIdentifier);
                    if (!result)
                        response.Status = ResponseStatusTypes.UpdateFail;
                }
                else
                {
                    result = _repository.DisableGeolocation(request.UserIdentifier);
                    if (!result)
                        response.Status = ResponseStatusTypes.UpdateFail;
                }
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatusTypes.UnknowError;
                response.Message = ex.Message;
            }
            return response;
        }

        public virtual ServiceResponse UserHasEnableGeolocation(string userIdentifier)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                var data = _repository.GetUserInfo(userIdentifier);
                if(data != null)
                {
                    if (!data.EnableGeolocation)
                        response.Status = ResponseStatusTypes.UserHasNotEnableGeolocation;
                }
                else
                    response.Status = ResponseStatusTypes.UserNotFound;
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatusTypes.UnknowError;
                response.Message = ex.Message;
            }
            return response;
        }

        public virtual DistanceResponse GetCurrentDistance(string userIdentifierOrigin, string userIdentifierDestination)
        {
            var result = new DistanceResponse();
            try
            {
                //Obtener posiciones
                GeolocationResponse positionOrigin = GetCurrentPositionUser(new GeolocationRequest
                {
                    UserIdentifier = userIdentifierOrigin
                });
                GeolocationResponse positionDestination = GetCurrentPositionUser(new GeolocationRequest
                {
                    UserIdentifier = userIdentifierDestination
                });
                //Validar datos
                if (!positionOrigin.GeolocationEnabled)
                {
                    result.Status = Enums.ResponseStatusTypes.UserPositionNotEnable;
                    result.Message = "Origin position disable";
                    return result;
                }
                if (!positionDestination.GeolocationEnabled)
                {
                    result.Status = Enums.ResponseStatusTypes.UserPositionNotEnable;
                    result.Message = "Destination position disable";
                    return result;
                }
                result = DistanceTo(positionOrigin, positionDestination);
            }
            catch (Exception ex)
            {
                result.Status = ResponseStatusTypes.UnknowError;
                result.Message = ex.Message;
            }
            return result;
        }

        public virtual GeolocationResponse GetCurrentPositionUser(GeolocationRequest request)
        {
            var response = new GeolocationResponse();
            try
            {
                string userIdentifier = request.UserIdentifier;
                GeolocationResponse currentPosition = GetCurrentPositionFromDB(userIdentifier);
                if (currentPosition == null)
                    currentPosition.Status = ResponseStatusTypes.UserNotFound;
                else
                {
                    if ((String.IsNullOrEmpty(currentPosition.Latitude)
                        || String.IsNullOrEmpty(currentPosition.Longitude))
                        && currentPosition.Status != ResponseStatusTypes.UserNotFound)
                        currentPosition.Status = ResponseStatusTypes.UserPositionNotFound;
                }
                //Ahorramos una llamada a la API
                currentPosition.Address = string.Empty;
                response = currentPosition;
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatusTypes.UnknowError;
                response.Message = ex.Message;
            }
            return response;
        }
        #endregion

        #region Private
        private IGeolocationRepository InstanceDefaultRepository()
        {
            return new GeolocationMemoryRepository(_pathDirectoryCacheInfo, 10000);
        }

        private bool HasGeoposition(GeolocationResponse geoposition)
        {
            return !String.IsNullOrEmpty(geoposition.Latitude) && !String.IsNullOrEmpty(geoposition.Longitude);
        }

        private void ValidateRequest(CurrentPositionInfoRequest request)
        {
            //Pendiente
        }
        #endregion

        #region Protected
        protected virtual DistanceResponse DistanceTo(GeolocationResponse positionOrigin, GeolocationResponse positionDestination)
        {
            DistanceResponse result = new DistanceResponse();
            if (!HasGeoposition(positionOrigin) || !HasGeoposition(positionDestination))
                result.Status = ResponseStatusTypes.UserPositionNotFound;
            else
            {
                string origin = string.Format("{0},{1}", positionOrigin.Latitude, positionOrigin.Longitude);
                string destination = string.Format("{0},{1}", positionDestination.Latitude, positionDestination.Longitude);
                var resultGoogleRequest = _googleMapsApi.DistanceMatrix.GetDistance(origin, destination, positionDestination.TravelMode);
                if (resultGoogleRequest != null)
                {
                    result.AddressOrigin = resultGoogleRequest.origin_addresses[0];
                    result.AddressDestination = resultGoogleRequest.destination_addresses[0];
                    result.Distance = resultGoogleRequest.rows[0].elements[0].distance.text;
                    result.Duration = resultGoogleRequest.rows[0].elements[0].duration.text;
                    result.DistanceValue = resultGoogleRequest.rows[0].elements[0].distance.value;
                    result.DurationValue = resultGoogleRequest.rows[0].elements[0].duration.value;
                }
            }
            return result;
        }

        protected virtual string GetAddress(GeolocationResponse currentPosition)
        {
            string address = string.Empty;
            var result = _googleMapsApi.GeoLocation.Geocoding(currentPosition.Latitude, currentPosition.Longitude);
            if (result.results != null && result.results.Count > 0)
            {
                var queryAddress = result.results.SingleOrDefault(x =>
                {
                    var query = x.types.SingleOrDefault(y => y == "street_address");
                    if (!String.IsNullOrEmpty(query))
                    {
                        return true;
                    }
                    else
                        return false;
                });
                if (queryAddress != null)
                    address = queryAddress.formatted_address;
            }
            return address;
        }

        protected virtual GeolocationResponse GetCurrentPositionFromDB(string userIdentifier)
        {
            var resultDb = _repository.GetCurrentUserPosition(userIdentifier);
            GeolocationResponse response = new GeolocationResponse();
            if (resultDb != null)
            {
                response.Latitude = resultDb.Latitude;
                response.Longitude = resultDb.Longitude;
                response.GeolocationEnabled = resultDb.EnableGeoposition;
            }
            else
            {
                response.Status = ResponseStatusTypes.UserNotFound;
            }

            return response;
        }
        #endregion
    }
}
