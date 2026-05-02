using DellyBelly.Application.Interfaces;
using DellyBelly.Domain.Entities;
using DellyBelly.Infrastructure.Data;
using DellyBelly.Shared.Helpers;
using DellyBellyAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DellyBellyAPI.Controllers
{
    /// <summary>
    /// Handles admin authentication: login, register, forgot/reset password.
    /// All timestamps stored in IST via DateTimeHelper.
    /// </summary>
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;

        public AuthController(ApplicationDbContext db, IEmailService emailService, IConfiguration config)
        {
            _db = db;
            _emailService = emailService;
            _config = config;
        }

        // ── POST /api/auth/login ──────────────────────────────────────────────
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _db.AdminUsers
                .FirstOrDefaultAsync(u => u.Email.ToLower() == dto.Email.ToLower() && u.IsActive);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return Unauthorized(new { message = "Invalid email or password." });

            if (!user.IsEmailVerified)
                return Unauthorized(new { message = "Please verify your email before logging in." });

            // Record login time in IST
            user.LastLoginAt = DateTimeHelper.GetIndianTime();
            await _db.SaveChangesAsync();

            return Ok(BuildResponse(user));
        }

        // ── POST /api/auth/register ───────────────────────────────────────────
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (await _db.AdminUsers.AnyAsync(u => u.Email.ToLower() == dto.Email.ToLower()))
                return Conflict(new { message = "An account with this email already exists." });

            var otpCode = new Random().Next(100000, 999999).ToString();

            var user = new AdminUser
            {
                FullName = dto.FullName.Trim(),
                Email = dto.Email.Trim().ToLower(),
                MobileNumber = dto.MobileNumber.Trim(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = "staff",
                IsActive = true,
                CreatedAt = DateTimeHelper.GetIndianTime(),   // IST
                IsEmailVerified = false,
                OTP = otpCode,
                OTPExpiry = DateTimeHelper.GetIndianTime().AddMinutes(10)
            };

            _db.AdminUsers.Add(user);
            await _db.SaveChangesAsync();

            await _emailService.SendEmailAsync(
                user.Email,
                "Verify Your Delly Belly Account",
                EmailTemplateHelper.GetOtpVerificationTemplate(user.FullName, otpCode),
                "otp_verification"
            );

            return Ok(new { message = "Account created successfully. Please check your email for the verification code.", email = user.Email });
        }

        // ── POST /api/auth/verify-otp ─────────────────────────────────────────
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpDto dto)
        {
            var user = await _db.AdminUsers.FirstOrDefaultAsync(u => u.Email.ToLower() == dto.Email.ToLower() && u.IsActive);
            if (user == null) return NotFound(new { message = "User not found." });

            if (user.IsEmailVerified) return BadRequest(new { message = "Email is already verified. You can sign in." });

            if (user.OTP != dto.OTP || user.OTPExpiry < DateTimeHelper.GetIndianTime())
                return BadRequest(new { message = "Invalid or expired OTP." });

            user.IsEmailVerified = true;
            user.OTP = null;
            user.OTPExpiry = null;
            await _db.SaveChangesAsync();

            return Ok(new { message = "Email verified successfully. You can now sign in." });
        }

        // ── POST /api/auth/resend-otp ─────────────────────────────────────────
        [HttpPost("resend-otp")]
        public async Task<IActionResult> ResendOtp([FromBody] ResendOtpDto dto)
        {
            var user = await _db.AdminUsers.FirstOrDefaultAsync(u => u.Email.ToLower() == dto.Email.ToLower() && u.IsActive);
            if (user == null) return NotFound(new { message = "User not found." });

            if (user.IsEmailVerified) return BadRequest(new { message = "Email is already verified. You can sign in." });

            var otpCode = new Random().Next(100000, 999999).ToString();
            user.OTP = otpCode;
            user.OTPExpiry = DateTimeHelper.GetIndianTime().AddMinutes(10);
            await _db.SaveChangesAsync();

            await _emailService.SendEmailAsync(
                user.Email,
                "Verify Your Delly Belly Account",
                EmailTemplateHelper.GetOtpVerificationTemplate(user.FullName, otpCode),
                "otp_verification"
            );

            return Ok(new { message = "A new verification code has been sent to your email." });
        }

        // ── POST /api/auth/forgot-password ───────────────────────────────────
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            var user = await _db.AdminUsers
                .FirstOrDefaultAsync(u => u.Email.ToLower() == dto.Email.ToLower() && u.IsActive);

            // Always 200 — never reveal whether the email exists
            if (user == null)
                return Ok(new { message = "If an account exists, a reset link has been sent." });

            var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
            user.PasswordResetToken = token;
            // Expiry stored in IST (+15 minutes from now)
            user.PasswordResetTokenExpiry = DateTimeHelper.GetIndianTime().AddMinutes(15);
            await _db.SaveChangesAsync();

            var resetUrl = $"{_config["App:FrontendUrl"]}/admin/reset-password?token={token}";

            await _emailService.SendEmailAsync(
                user.Email,
                "Reset Your Delly Belly Admin Password",
                EmailTemplateHelper.GetPasswordResetTemplate(user.FullName, resetUrl, 15),
                "password_reset"
            );

            return Ok(new { message = "If an account exists, a reset link has been sent." });
        }

        // ── POST /api/auth/reset-password ─────────────────────────────────────
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var nowIst = DateTimeHelper.GetIndianTime();

            var user = await _db.AdminUsers.FirstOrDefaultAsync(u =>
                u.PasswordResetToken == dto.Token &&
                u.PasswordResetTokenExpiry > nowIst);

            if (user == null)
                return BadRequest(new { message = "Reset link is invalid or has expired." });

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            user.PasswordResetToken = null;
            user.PasswordResetTokenExpiry = null;
            await _db.SaveChangesAsync();

            return Ok(new { message = "Password updated successfully. You can now sign in." });
        }

        // ── GET /api/auth/me ──────────────────────────────────────────────────
        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> Me()
        {
            var emailClaim = User.FindFirstValue(ClaimTypes.Email);
            if (emailClaim == null) return Unauthorized();

            var user = await _db.AdminUsers
                .FirstOrDefaultAsync(u => u.Email == emailClaim && u.IsActive);
            if (user == null) return Unauthorized();

            return Ok(BuildResponse(user, includeToken: false));
        }

        // ── Helpers ───────────────────────────────────────────────────────────
        private AuthResponseDto BuildResponse(AdminUser user, bool includeToken = true) => new(
            Token: includeToken ? GenerateJwt(user) : string.Empty,
            Email: user.Email,
            FullName: user.FullName,
            MobileNumber: user.MobileNumber,
            Role: user.Role,
            Avatar: string.Concat(user.FullName.Split(' ').Select(n => n[0]))
        );

        private string GenerateJwt(AdminUser user)
        {
            var secret = _config["Jwt:Secret"]
                ?? throw new InvalidOperationException("JWT Secret not configured.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,   user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role,               user.Role),
                new Claim("name",                        user.FullName),
                new Claim(JwtRegisteredClaimNames.Jti,   Guid.NewGuid().ToString()),
            };

            var expireHours = int.TryParse(_config["Jwt:ExpiresHours"], out var h) ? h : 8;

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(expireHours),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
