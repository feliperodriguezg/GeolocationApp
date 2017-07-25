using Newtonsoft.Json;
using System.Net.Http;

namespace NetCore.GeolocationApp.GoogleMapsApi
{
    internal class GoogleMapsGeoLocation : IGoogleMapsGeoLocation
    {
        private const string UrlGeocoding = "/geocode";
        private GoogleMapsApiSettings _settings;

        public GoogleMapsGeoLocation(GoogleMapsApiSettings settings)
        {
            _settings = settings;
        }

        public RootObject Geocoding(string latitude, string longitude)
        {
            return ProccessGeocodingAsync(latitude, longitude);
        }

        public RootObject Geocoding(string address)
        {
            return ProcessAddress(address);
        }

        private RootObject ProccessGeocodingAsync(string latitude, string longitude)
        {
            using (HttpClient client = new HttpClient())
            {
                string urlRequest = _settings.UrlRootApi + UrlGeocoding + string.Format("/json?latlng={0},{1}&key={2}", latitude, longitude, _settings.ApiKey);
                var stringTask = client.GetStringAsync(urlRequest);
                string result = stringTask.Result;
                RootObject response = JsonConvert.DeserializeObject<RootObject>(result);
                return response;
            }
        }

        private RootObject ProcessAddress(string address)
        {
            using (HttpClient client = new HttpClient())
            {
                string urlRequest = _settings.UrlRootApi + UrlGeocoding + string.Format("/json?address={0}&key={1}", address, _settings.ApiKey);
                var stringTask = client.GetStringAsync(urlRequest);
                string result = stringTask.Result;
                RootObject response = JsonConvert.DeserializeObject<RootObject>(result);
                return response;
            }
        }

    }
}
