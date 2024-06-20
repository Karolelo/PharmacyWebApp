using System.Runtime.InteropServices.JavaScript;

namespace PrescriptionApp.Models;

public class User : BasicEntity
{
    public string Email { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public string Salt { get; set; }
    public string RefreshToken { get; set; }
    public DateTime? RefreshTokenExp { get; set; }
}