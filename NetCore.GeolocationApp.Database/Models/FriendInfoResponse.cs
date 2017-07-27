using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.GeolocationApp.Repositories.Models
{
    public class FriendInfoResponse
    {
        public string UserIdentifier { get; set; }
        public string Name { get; set; }
        public string UrlPhoto { get; set; }
        public string FriendOf { get; set; }
    }
}
