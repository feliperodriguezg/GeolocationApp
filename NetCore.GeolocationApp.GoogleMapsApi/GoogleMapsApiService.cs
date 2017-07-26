namespace NetCore.GeolocationApp.GoogleMapsApi
{
    public class GoogleMapsApiService
    {
        private string _apiKey;
        private IGoogleMapsDistanceMatrix _distanceMatrix;
        private IGoogleMapsGeoLocation _geolocation;
        private GoogleMapsApiSettings _settings;
        private string _authenticationToken;

        public GoogleMapsApiService(string apiKey)
        {
            _apiKey = apiKey;
            _settings = new GoogleMapsApiSettings(apiKey);
        }

        public IGoogleMapsDistanceMatrix DistanceMatrix
        {
            get
            {
                if (_distanceMatrix == null)
                    _distanceMatrix = new GoogleMapsDistanceMatrix(_settings);
                return _distanceMatrix;
            }
        }
        public IGoogleMapsGeoLocation GeoLocation
        {
            get
            {
                if (_geolocation == null)
                    _geolocation = new GoogleMapsGeoLocation(_settings);
                return _geolocation;
            }
        }
    }
}
