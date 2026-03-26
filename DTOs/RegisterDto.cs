// DTO FOR USER REGISTRATION
namespace GharGharGas.API.DTOs;

public class RegisterDto
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string District { get; set; }
    public string Password { get; set; }
}