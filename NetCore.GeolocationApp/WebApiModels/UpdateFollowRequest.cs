namespace NetCore.GeolocationApp.WebApiModels
{
    public class UpdateFollowRequest
    {
        public string UserIdentifierFollower { get; set; }
        public string UserIdentifierFriend { get; set; }
        public bool Allow { get; set; }
    }
}