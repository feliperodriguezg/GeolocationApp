using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.GeolocationApp.Repositories.Models
{
    public class UserInfoResponse
    {
        public bool EnableGeolocation { get; set; }
        public string CurrentLatitude { get; set; }
        public string CurrentLongitude { get; set; }
        public string UserIdentifier { get; set; }
        public string CurrentTravelMode { get; set; }
    }
}
