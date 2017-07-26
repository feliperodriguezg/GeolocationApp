using System;
using System.Linq;
using NetCore.GeolocationApp.Enums;
using System.Reflection;

namespace NetCore.GeolocationApp.WebApiModels
{
    public static class EnumerationExtension
    {
        public static string Description(this Enum value)
        {
            // get attributes  
            var field = value.GetType().GetField(value.ToString());
            var attributes = field.GetCustomAttributes(false);

            // Description is in a hidden Attribute class called DisplayAttribute
            // Not to be confused with DisplayNameAttribute
            dynamic displayAttribute = null;

            if (attributes.Any())
            {
                displayAttribute = attributes.ElementAt(0);
            }

            // return description
            return displayAttribute?.Description ?? "Description Not Found";
        }
    }

    public class ResponseBase
    {
        private ResponseStatusTypes _status;
        public ResponseStatusTypes Status {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
                Message = value.Description();
                StatusText = value.ToString();
            }
        }
        public string Message { get; set; }
        public string StatusText { get; set; }

        public ResponseBase()
        {
            Status = ResponseStatusTypes.Ok;
        }
    }
}
