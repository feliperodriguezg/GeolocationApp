namespace NetCore.GeolocationApp.WebApiModels
{
    public class FriendsInformationRequest
    {
        public string UserIdentifier { get; set; }
        public DistanceResponse DistanceInfo { get; set; }

        public FriendsInformationRequest()
        {
            DistanceInfo = new DistanceResponse();
        }
    }
}