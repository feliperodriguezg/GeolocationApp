namespace NetCore.GeolocationApp.GoogleMapsApi
{
    public class GoogleMapsApiSettings
    {
        private const string DefaultUrlRootApi = "https://maps.googleapis.com/maps/api";
        public string ApiKey { get; private set; }
        public string UrlRootApi { get; private set; }
        public GoogleMapsApiSettings(string apiKey)
        {
            ApiKey = apiKey;
            UrlRootApi = DefaultUrlRootApi;
        }
    }
}
