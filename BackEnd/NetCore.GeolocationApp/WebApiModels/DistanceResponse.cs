namespace NetCore.GeolocationApp.WebApiModels
{
    public class DistanceResponse: ResponseBase
    {
        public DistanceResponse(): base() { }

        public string AddressOrigin { get; set; }
        public string AddressDestination { get; set; }
        public string Distance { get; set; }
        public string Duration { get; set; }

        public decimal DistanceValue { get; set; }
        public decimal DurationValue { get; set; }
    }
}
