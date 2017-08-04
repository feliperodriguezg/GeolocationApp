using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NetCore.GeolocationApp.Helpers;
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
        private readonly AppSettings _appSettings;
        protected AppSettings AppSettings
        {
            get
            {
                return _appSettings;
            }
        }

        public ApiControllerBase(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
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
