using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore.GeolocationApp.WebApiModels
{
    public class FriendInformationResponse: ResponseBase
    {
        public List<FriendInformation> Friends { get; set; }
        public FriendInformationResponse()
        {
            Friends = new List<FriendInformation>();
        }
    }

    public class FriendInformation
    {
        public string UserIdentifier { get; set; }
        public string Name { get; set; }
        public string UrlPhoto { get; set; }
        public bool IsEnable { get; set; }
    }
}
