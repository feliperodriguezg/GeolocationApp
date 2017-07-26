namespace NetCore.GeolocationApp.WebApiModels
{
    public class EnableGeolocationRequest
    {
        public string UserIdentifier { get; set; }
        public bool Enable { get; set; }
    }
}