namespace GharGharGas.API.Models;

public class User
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string District { get; set; }
    public string PasswordHash { get; set; }
    public bool IsVerified { get; set; } = false;
}