namespace NetCore.GeolocationApp.WebApiModels
{
    public class GeolocationResponse
    {
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Address { get; set; }
        public bool GeolocationEnabled { get; set; }
        public string TravelMode { get; set; }
    }
}
