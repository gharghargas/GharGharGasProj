using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GharGharGas.API.Data;
using GharGharGas.API.Models;
using GharGharGas.API.DTOs;
using GharGharGas.API.Services;


// CONTROLLER FOR AUTHENTICATION (REGISTER, VERIFY OTP, LOGIN)
namespace GharGharGas.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly EmailService _emailService;

    public AuthController(AppDbContext context, EmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    // REGISTER
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        if (await _context.Users.AnyAsync(x => x.Email == dto.Email))
            return BadRequest("Email already exists");

        var user = new User
        {
            FullName = dto.FullName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            District = dto.District,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };

        _context.Users.Add(user);

        var otpCode = OtpGenerator.Generate();

        var otp = new Otp
        {
            Email = dto.Email,
            Code = otpCode,
            ExpiryTime = DateTime.UtcNow.AddMinutes(5)
        };

        _context.Otps.Add(otp);

        await _context.SaveChangesAsync();

        await _emailService.SendOtp(dto.Email, otpCode);

        return Ok("OTP sent");
    }

    // VERIFY OTP
    [HttpPost("verify-otp")]
    public async Task<IActionResult> VerifyOtp(VerifyOtpDto dto)
    {
        var otp = await _context.Otps
            .Where(x => x.Email == dto.Email && x.Code == dto.Code && !x.IsUsed)
            .OrderByDescending(x => x.Id)
            .FirstOrDefaultAsync();

        if (otp == null || otp.ExpiryTime < DateTime.UtcNow)
            return BadRequest("Invalid or expired OTP");

        otp.IsUsed = true;

        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);
        user.IsVerified = true;

        await _context.SaveChangesAsync();

        return Ok("Account verified");
    }

    // LOGIN
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == dto.Email);

        if (user == null)
            return Unauthorized("Invalid credentials");

        if (!user.IsVerified)
            return BadRequest("Account not verified");

        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return Unauthorized("Invalid credentials");

        return Ok("Login success");
    }
}