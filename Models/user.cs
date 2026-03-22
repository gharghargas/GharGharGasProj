public class User
{
    public int Id { get; set; }
    public string FullName { get; set; }   // ✅ add
    public string Email { get; set; }
    public string Phone { get; set; }      // ✅ add
    public string District { get; set; }   // ✅ add
    public string Password { get; set; }
    public bool IsVerified { get; set; }   // ✅ for OTP
}