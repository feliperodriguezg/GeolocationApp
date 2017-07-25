namespace NetCore.GeolocationApp.Services
{
    public class ServiceResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }

        public ServiceResponse()
        {
            Status = "OK";
            Message = string.Empty;
        }
    }
}