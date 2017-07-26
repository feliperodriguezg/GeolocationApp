using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetCore.GeolocationApp.Enums;

namespace NetCore.GeolocationApp.WebApiModels
{
    public class ResponseBase
    {
        public ResponseStatusTypes Status { get; set; }
        public string Message { get; set; }

        public ResponseBase()
        {
            Status = ResponseStatusTypes.Ok;
        }
    }
}
