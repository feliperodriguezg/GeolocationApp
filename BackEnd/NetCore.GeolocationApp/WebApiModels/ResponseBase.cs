using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore.GeolocationApp.WebApiModels
{
    public class ResponseBase
    {
        public string Status { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }

        public ResponseBase()
        {
            Status = "OK";
            Code = "200";
            Message = string.Empty;
        }
    }
}
