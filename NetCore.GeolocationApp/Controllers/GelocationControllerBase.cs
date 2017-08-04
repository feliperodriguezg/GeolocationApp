

using Microsoft.Extensions.Options;
using NetCore.GeolocationApp.Helpers;
using NetCore.GeolocationApp.Services;

namespace NetCore.GeolocationApp.Controllers
{
    public class GelocationControllerBase: ApiControllerBase
    {
        protected const string ApiKey = "AIzaSyAwEYAAUxnE9XmNOsIoFJhd-590PdBDZ_4";
        private GeolocationService _services;

        public GelocationControllerBase(IOptions<AppSettings> appSettings): base(appSettings) { }

        public GeolocationService GeolocationService
        {
            get
            {
                if (_services == null)
                    _services = new GeolocationService(ApiKey);
                return _services;
            }
        }
    }
}
