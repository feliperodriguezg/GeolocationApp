using NetCore.GeolocationApp.GoogleMapsApi.Enums;

namespace NetCore.GeolocationApp.GoogleMapsApi
{
    public interface IGoogleMapsDistanceMatrix
    {
        RootObjectDistanceMatrix GetDistance(string origin, string destination, string mode);
    }
}