namespace GharGharGas.API.Models;

public class Otp
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Email { get; set; }
    public DateTime ExpiryTime { get; set; }
    public bool IsUsed { get; set; } = false;
}