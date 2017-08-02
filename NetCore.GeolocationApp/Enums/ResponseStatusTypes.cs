using System.ComponentModel;

namespace NetCore.GeolocationApp.Enums
{
    public enum ResponseStatusTypes
    {
        [Description("Ok")]
        Ok = 100,
        [Description("Google Maps Api no disponible")]
        GelocationServiceNotAvailable = 201,
        [Description("Usuario no encontrado en la base de datos")]
        UserNotFound = 400,
        [Description("Posición del usuario no disponible")]
        UserPositionNotEnable = 404,
        [Description("Posición del usuario no encontrada")]
        UserPositionNotFound = 406,
        [Description("No se ha encontrado amigos relacionados")]
        FriendsNotFound = 500,
        [Description("No permite seguimiento")]
        NotAllowFollow = 600,
        [Description("No se actualizó los datos en el repositorio")]
        UpdateFail = 700,
        [Description("Usuario no tiene habilitada la geolocalización")]
        UserHasNotEnableGeolocation = 800,
        [Description("Error desconocido")]
        UnknowError = 2000
    }
}
