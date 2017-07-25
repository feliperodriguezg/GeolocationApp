namespace NetCore.GeolocationApp.Repositories
{
    public class GeolocationRepository : IGeolocationRepository
    {
        public GeolocationRepository()
        {

        }

        public virtual CurrentUserPositionResponse GetCurrentUserPosition(string userIdentifier)
        {
            return new CurrentUserPositionResponse();
        }

        public virtual void EnableGeolocation(string userIdentifier)
        {

        }

        public virtual void DisableGeolocation(string userIdentifier)
        {

        }

        public virtual void SetCurrentPosition(string userIdentifier, string latitude, string longitude)
        {

        }

        public virtual void UpdateCurrentTravelMode(string userIdentifier, string mode)
        {

        }
    }
}
