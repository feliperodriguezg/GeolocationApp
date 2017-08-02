using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore.GeolocationApp.Models
{
    public class ApiResultResponse<T>
    {
        public bool Ok { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
