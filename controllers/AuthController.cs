using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Gharghargasproject_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        // 🔹 Fake database (temporary)
        private static List<User> users = new List<User>();

        // 🔹 OTP storage (temporary)
        private static Dictionary<string, string> otpStore = new Dictionary<string, string>();

        // ===========================
        // ✅ REGISTER (Generate OTP)
        // ===========================
        [HttpPost("register")]
        public IActionResult Register(RegisterDto dto)
        {
            if (users.Any(u => u.Email == dto.Email))
            {
                return BadRequest("User already exists");
            }

            // Generate 6-digit OTP
            var otp = new Random().Next(100000, 999999).ToString();

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Phone = dto.Phone,
                District = dto.District,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                IsVerified = false
            };

            users.Add(user);

            // Save OTP (key = email)
            otpStore[dto.Email] = otp;

            return Ok(new
            {
                message = "OTP sent successfully",
                otp = otp // ⚠️ only for testing (remove in production)
            });
        }

        // ===========================
        // ✅ VERIFY OTP
        // ===========================
        [HttpPost("verify-otp")]
        public IActionResult VerifyOtp(VerifyOtpDto dto)
        {
            if (!otpStore.ContainsKey(dto.Email))
            {
                return BadRequest("OTP not found");
            }

            if (otpStore[dto.Email] != dto.Otp)
            {
                return BadRequest("Invalid OTP");
            }

            var user = users.FirstOrDefault(u =>
                u.Email == dto.Email && u.Phone == dto.Phone);

            if (user == null)
            {
                return BadRequest("User not found");
            }

            user.IsVerified = true;

            // Remove OTP after success
            otpStore.Remove(dto.Email);

            return Ok(new
            {
                message = "Account verified successfully"
            });
        }

        // ===========================
        // ✅ LOGIN (NO OTP)
        // ===========================
        [HttpPost("login")]
        public IActionResult Login(LoginDto dto)
        {
            var user = users.FirstOrDefault(u => u.Email == dto.Email);

            if (user == null)
            {
                return Unauthorized("User not found");
            }

            if (!user.IsVerified)
            {
                return Unauthorized("Please verify OTP first");
            }

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            {
                return Unauthorized("Invalid password");
            }

            return Ok(new
            {
                message = "Login successful"
            });
        }
    }
}