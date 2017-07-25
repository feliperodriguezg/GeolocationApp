using NetCore.GeolocationApp.GoogleMapsApi.Enums;
using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace NetCore.GeolocationApp.GoogleMapsApi
{
    internal class GoogleMapsDistanceMatrix : IGoogleMapsDistanceMatrix
    {
        private const string UrlGeocoding = "/distancematrix";
        private GoogleMapsApiSettings _settings;

        public GoogleMapsDistanceMatrix(GoogleMapsApiSettings settings)
        {
            _settings = settings;
        }

        public RootObjectDistanceMatrix GetDistance(string origin, string destination, string mode)
        {
            using (HttpClient client = new HttpClient())
            {
                string urlRequest = _settings.UrlRootApi + UrlGeocoding + string.Format("/json?units=metric&origins={0}&destinations={1}&key={2}", origin, destination, _settings.ApiKey);
                if (!String.IsNullOrEmpty(mode))
                    urlRequest += "&mode=" + mode;

                var stringTask = client.GetStringAsync(urlRequest);
                string result = stringTask.Result;
                RootObjectDistanceMatrix response = JsonConvert.DeserializeObject<RootObjectDistanceMatrix>(result);
                return response;
            }
        }
    }
}
