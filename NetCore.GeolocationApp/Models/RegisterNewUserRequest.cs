namespace NetCore.GeolocationApp.Models
{
    public class RegisterNewUserRequest
    {
        public string Username { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}