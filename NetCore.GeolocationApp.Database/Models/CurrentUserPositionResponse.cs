namespace NetCore.GeolocationApp.Repositories
{
    public class CurrentUserPositionResponse
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string UserIdentifier { get; set; }
        public bool EnableGeoposition { get; set; }
    }
}