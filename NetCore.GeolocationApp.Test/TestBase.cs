using Microsoft.Extensions.Options;
using NetCore.GeolocationApp.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetCore.GeolocationApp.Test
{
    public class OptionsSettings : IOptions<AppSettings>
    {
        private AppSettings _settings;
        public AppSettings Value
        {
            get
            {
                if(_settings == null)
                {
                    _settings = new AppSettings
                    {
                        PasswordAdmin = "test"
                    };
                }
                return _settings;
            }
        }
    }

    public class TestBase
    {
        protected readonly IOptions<AppSettings> _appSettings = new OptionsSettings();
    }
}
