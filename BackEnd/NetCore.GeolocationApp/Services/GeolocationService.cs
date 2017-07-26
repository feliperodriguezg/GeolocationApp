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
        private GoogleMapsApiService _googleMapsApi;
        private IGeolocationRepository _repository;

        public IGeolocationRepository Repository
        {
            get
            {
                if (_repository == null)
                    _repository = new GeolocationRepository();
                return _repository;
            }
            set
            {
                _repository = value;
            }
        }

        public GeolocationService(string apiKey)
        {
            _googleMapsApi = new GoogleMapsApiService(apiKey);
            _repository = new GeolocationRepository();
        }

        public virtual ServiceResponse SetCurrentPosition(CurrentPositionInfoRequest request)
        {
            ServiceResponse response = new ServiceResponse();
            try
            {
                ValidateRequest(request);
                _repository.SetCurrentPosition(request.UserIdentifier, request.Latitude, request.Longitude);
            }
            catch (Exception ex)
            {
                response.Status = ResponseStatusTypes.UnknowError;
                response.Message = ex.Message;
            }
            return response;
        }

        public virtual void EnableGeolocation(EnableGeolocationRequest request)
        {
            bool enable = request.Enable;
            if (enable)
                _repository.EnableGeolocation(request.UserIdentifier);
            else
                _repository.DisableGeolocation(request.UserIdentifier);
        }

        public virtual DistanceResponse GetCurrentDistance(string userIdentifierOrigin, string userIdentifierDestination)
        {
            DistanceResponse result = new DistanceResponse();
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
            if(!positionDestination.GeolocationEnabled)
            {
                result.Status = Enums.ResponseStatusTypes.UserPositionNotEnable;
                return result;
            }
            result = DistanceTo(positionOrigin, positionDestination);
            return result;
        }

        protected virtual DistanceResponse DistanceTo(GeolocationResponse positionOrigin, GeolocationResponse positionDestination)
        {
            DistanceResponse result = new DistanceResponse();
            string origin = string.Format("{0},{1}", positionOrigin.Latitude, positionOrigin.Longitude);
            string destination = string.Format("{0},{1}", positionDestination.Latitude, positionDestination.Longitude);
            var resultGoogleRequest =_googleMapsApi.DistanceMatrix.GetDistance(origin, destination, positionDestination.TravelMode);
            if(resultGoogleRequest != null)
            {
                result.AddressOrigin = resultGoogleRequest.origin_addresses[0];
                result.AddressDestination = resultGoogleRequest.destination_addresses[0];
                result.Distance = resultGoogleRequest.rows[0].elements[0].distance.text;
                result.Duration = resultGoogleRequest.rows[0].elements[0].duration.text;

                result.DistanceValue = resultGoogleRequest.rows[0].elements[0].distance.value;
                result.DurationValue = resultGoogleRequest.rows[0].elements[0].duration.value;
            }
            return result;
        }

        public virtual GeolocationResponse GetCurrentPositionUser(GeolocationRequest request)
        {
            string userIdentifier = request.UserIdentifier;
            GeolocationResponse currentPosition = GetCurrentPositionFromDB(userIdentifier);
            //Ahorramos una llamada a la API
            currentPosition.Address = string.Empty;
            return currentPosition;
        }

        protected virtual string GetAddress(GeolocationResponse currentPosition)
        {
            string address = string.Empty;
            var result = _googleMapsApi.GeoLocation.Geocoding(currentPosition.Latitude, currentPosition.Longitude);
            if(result.results != null && result.results.Count > 0)
            {
                var queryAddress = result.results.SingleOrDefault(x =>
                {
                    var query = x.types.SingleOrDefault(y => y == "street_address");
                    if(!String.IsNullOrEmpty(query))
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
            response.Latitude = resultDb.Latitude;
            response.Longitude = resultDb.Longitude;
            response.GeolocationEnabled = resultDb.EnableGeoposition;
            return response;
        }

        private void ValidateRequest(CurrentPositionInfoRequest request)
        {
            //Pendiente
        }
    }
}
