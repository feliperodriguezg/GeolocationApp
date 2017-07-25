namespace NetCore.GeolocationApp.GoogleMapsApi
{
    public interface IGoogleMapsGeoLocation
    {
        RootObject Geocoding(string address);
        RootObject Geocoding(string latitude, string longitude);
    }
}