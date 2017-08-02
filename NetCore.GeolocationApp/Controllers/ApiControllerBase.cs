using Microsoft.AspNetCore.Mvc;
using NetCore.GeolocationApp.Models;
using NetCore.GeolocationApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace NetCore.GeolocationApp.Controllers
{
    public class ApiControllerBase: Controller
    {
        protected const string ApiKey = "AIzaSyAwEYAAUxnE9XmNOsIoFJhd-590PdBDZ_4";
        private GeolocationService _services;
        public GeolocationService GeolocationService
        {
            get
            {
                if (_services == null)
                    _services = new GeolocationService(ApiKey);
                return _services;
            }
        }

        protected virtual ApiResultResponse<T> ResponseOk<T>()
        {
            return new ApiResultResponse<T>
            {
                Ok = true,
                Message = "",
                Data = default(T)
            };
        }
        protected virtual ApiResultResponse<T> ResponseOk<T>(string message)
        {
            return new ApiResultResponse<T>
            {
                Ok = true,
                Message = message,
                Data = default(T)
            };
        }
        protected virtual ApiResultResponse<T> ResponseOk<T>(T data)
        {
            return new ApiResultResponse<T>
            {
                Ok = true,
                Message = string.Empty,
                Data = data
            };
        }
        protected virtual ApiResultResponse<T> ResponseOk<T>(T data, string message)
        {
            return new ApiResultResponse<T>
            {
                Ok = true,
                Message = message,
                Data = data
            };
        }

        protected virtual ApiResultResponse<T> ResponseError<T>()
        {
            return new ApiResultResponse<T>
            {
                Ok = false,
                Message = "",
                Data = default(T)
            };
        }
        protected virtual ApiResultResponse<T> ResponseError<T>(T data, string message)
        {
            return new ApiResultResponse<T>
            {
                Ok = false,
                Message = message,
                Data = data
            };
        }
        protected virtual ObjectResult StatusCode(HttpStatusCode httpStatusCode, object value)
        {
            return base.StatusCode((int)httpStatusCode, value);
        }
    }
}
