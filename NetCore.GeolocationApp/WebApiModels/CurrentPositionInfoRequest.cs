namespace NetCore.GeolocationApp.WebApiModels
{
    public class CurrentPositionInfoRequest
    {
        public string UserIdentifier { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}