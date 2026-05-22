using DellyBelly.Application.Interfaces;
using DellyBelly.Domain.Entities;
using DellyBelly.Infrastructure.Data;
using DellyBelly.Shared.Helpers;
using DellyBellyAPI.DTOs;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Caching.Memory;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DellyBellyAPI.Controllers
{
    [ApiController]
    [Route("api/customer-auth")]
    public class CustomerAuthController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;
        private readonly IMemoryCache _cache;

        public CustomerAuthController(ApplicationDbContext db, IEmailService emailService, IConfiguration config, IMemoryCache cache)
        {
            _db = db;
            _emailService = emailService;
            _config = config;
            _cache = cache;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] CustomerLoginDto dto)
        {
            var user = await _db.Customers
                .FirstOrDefaultAsync(u => u.Email.ToLower() == dto.Email.ToLower());

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return Unauthorized(new { message = "Invalid email or password." });

            if (!user.IsEmailVerified)
                return Unauthorized(new { message = "Please verify your email before logging in." });

            return Ok(BuildResponse(user));
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginDto dto)
        {
            var clientId = _config["GoogleSettings:ClientId"];
            if (string.IsNullOrEmpty(clientId) || clientId == "YOUR_GOOGLE_CLIENT_ID_HERE")
                return BadRequest(new { message = "Google Login is not fully configured." });

            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings() { Audience = new List<string>() { clientId } };
                var payload = await GoogleJsonWebSignature.ValidateAsync(dto.Credential, settings);

                if (payload == null) return Unauthorized(new { message = "Invalid Google token." });

                var user = await _db.Customers.FirstOrDefaultAsync(u => u.Email.ToLower() == payload.Email.ToLower());
                
                if (user == null)
                {
                    return NotFound(new 
                    { 
                        message = "Account not found", 
                        email = payload.Email, 
                        firstName = payload.GivenName ?? payload.Name ?? "", 
                        lastName = payload.FamilyName ?? "" 
                    });
                }

                // Google has already verified this email — mark it verified if not already
                if (!user.IsEmailVerified)
                {
                    user.IsEmailVerified = true;
                    user.OTP = null;
                    user.OTPExpiry = null;
                    await _db.SaveChangesAsync();
                }

                return Ok(BuildResponse(user));
            }
            catch (InvalidJwtException)
            {
                return Unauthorized(new { message = "Invalid Google token." });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CustomerRegisterDto dto)
        {
            if (await _db.Customers.AnyAsync(u => u.Email.ToLower() == dto.Email.ToLower()))
                return Conflict(new { message = "An account with this email already exists." });

            var otpCode = new Random().Next(100000, 999999).ToString();
            string fullName = dto.FirstName.Trim() + (string.IsNullOrWhiteSpace(dto.LastName) ? "" : " " + dto.LastName.Trim());

            var user = new Customer
            {
                FullName = fullName,
                Email = dto.Email.Trim().ToLower(),
                PhoneNumber = dto.Mobile.Trim(),
                PasswordHash = string.IsNullOrWhiteSpace(dto.Password) ? string.Empty : BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Address = dto.Address?.Trim() ?? "",
                Gender = dto.Gender,
                DateOfBirth = dto.DateOfBirth,
                CreatedAt = DateTimeHelper.GetIndianTime(),
                IsEmailVerified = string.IsNullOrWhiteSpace(dto.Password), // Auto-verify if no password (Google signup)
                OTP = string.IsNullOrWhiteSpace(dto.Password) ? null : otpCode,
                OTPExpiry = string.IsNullOrWhiteSpace(dto.Password) ? null : DateTimeHelper.GetIndianTime().AddMinutes(10)
            };

            _db.Customers.Add(user);
            await _db.SaveChangesAsync();

            if (string.IsNullOrWhiteSpace(dto.Password))
            {
                // Google Signup
                await _emailService.SendEmailAsync(
                    user.Email,
                    "Welcome to Delly Belly!",
                    EmailTemplateHelper.GetWelcomeTemplate(user.FullName),
                    "welcome"
                );
                return Ok(BuildResponse(user));
            }

            await _emailService.SendEmailAsync(
                user.Email,
                "Verify Your Delly Belly Account",
                EmailTemplateHelper.GetOtpVerificationTemplate(user.FullName, otpCode),
                "otp_verification"
            );

            return Ok(new { message = "Account created successfully. Please check your email for the verification code.", email = user.Email });
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpDto dto)
        {
            var user = await _db.Customers.FirstOrDefaultAsync(u => u.Email.ToLower() == dto.Email.ToLower());
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

        [HttpPost("resend-otp")]
        public async Task<IActionResult> ResendOtp([FromBody] ResendOtpDto dto)
        {
            var user = await _db.Customers.FirstOrDefaultAsync(u => u.Email.ToLower() == dto.Email.ToLower());
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

        [HttpPost("check-email")]
        public async Task<IActionResult> CheckEmail([FromBody] CheckEmailDto dto)
        {
            var exists = await _db.Customers.AnyAsync(u => u.Email.ToLower() == dto.Email.ToLower());
            return Ok(new EmailCheckResponseDto(exists));
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            var user = await _db.Customers
                .FirstOrDefaultAsync(u => u.Email.ToLower() == dto.Email.ToLower());

            if (user == null)
                return Ok(new { message = "If an account exists, a reset link has been sent." });

            var token = Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
            user.PasswordResetToken = token;
            user.PasswordResetTokenExpiry = DateTimeHelper.GetIndianTime().AddMinutes(15);
            await _db.SaveChangesAsync();

            var resetUrl = $"{_config["App:FrontendUrl"]}/reset-password?token={token}";

            await _emailService.SendEmailAsync(
                user.Email,
                "Reset Your Delly Belly Password",
                EmailTemplateHelper.GetPasswordResetTemplate(user.FullName, resetUrl, 15, "Customer Portal"),
                "password_reset"
            );

            return Ok(new { message = "If an account exists, a reset link has been sent." });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var nowIst = DateTimeHelper.GetIndianTime();

            var user = await _db.Customers.FirstOrDefaultAsync(u =>
                u.PasswordResetToken == dto.Token &&
                u.PasswordResetTokenExpiry > nowIst);

            if (user == null)
                return BadRequest(new { message = "Reset link is invalid or has expired." });

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            user.PasswordResetToken = null;
            user.PasswordResetTokenExpiry = null;
            user.UpdatedAt = DateTimeHelper.GetIndianTime();
            user.UpdatedBy = user.FullName;
            await _db.SaveChangesAsync();

            await _emailService.SendEmailAsync(
                user.Email,
                "Password Reset Successful — Delly Belly",
                EmailTemplateHelper.GetPasswordResetSuccessTemplate(user.FullName),
                "password_reset_success"
            );

            return Ok(new { message = "Password updated successfully. You can now sign in." });
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> Me()
        {
            var emailClaim = User.FindFirstValue(ClaimTypes.Email);
            if (emailClaim == null) return Unauthorized();

            var user = await _db.Customers
                .FirstOrDefaultAsync(u => u.Email == emailClaim);
            if (user == null) return Unauthorized();

            return Ok(BuildResponse(user, includeToken: false));
        }

        [HttpPut("profile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateCustomerProfileDto dto)
        {
            var emailClaim = User.FindFirstValue(ClaimTypes.Email);
            if (emailClaim == null) return Unauthorized();

            var user = await _db.Customers
                .FirstOrDefaultAsync(u => u.Email == emailClaim);
            if (user == null) return Unauthorized();

            user.FullName = dto.FullName;
            user.PhoneNumber = dto.MobileNumber;
            user.Gender = dto.Gender;
            user.DateOfBirth = dto.DateOfBirth;
            user.UpdatedAt = DateTimeHelper.GetIndianTime();
            user.UpdatedBy = dto.FullName;

            await _db.SaveChangesAsync();

            return Ok(BuildResponse(user, includeToken: false));
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var emailClaim = User.FindFirstValue(ClaimTypes.Email);
            if (emailClaim == null) return Unauthorized();

            var emailKey = emailClaim.ToLower();
            var lockoutKey = $"pw_lock_{emailKey}";

            // Check if user is locked out
            if (_cache.TryGetValue(lockoutKey, out DateTime lockoutExpiry))
            {
                if (DateTime.UtcNow < lockoutExpiry)
                {
                    var timeLeft = lockoutExpiry - DateTime.UtcNow;
                    var hours = Math.Ceiling(timeLeft.TotalHours);
                    return BadRequest(new { message = $"Too many failed attempts. Password update is locked for {hours} hours." });
                }
            }

            var user = await _db.Customers.FirstOrDefaultAsync(u => u.Email == emailClaim);
            if (user == null) return Unauthorized();

            // If user has a password, verify the old one
            if (!string.IsNullOrEmpty(user.PasswordHash))
            {
                if (string.IsNullOrEmpty(dto.OldPassword) || !BCrypt.Net.BCrypt.Verify(dto.OldPassword, user.PasswordHash))
                {
                    var attemptKey = $"pw_attempts_{emailKey}";
                    _cache.TryGetValue(attemptKey, out int attempts);
                    attempts++;

                    if (attempts >= 3)
                    {
                        var expiry = DateTime.UtcNow.AddHours(4);
                        _cache.Set(lockoutKey, expiry, TimeSpan.FromHours(4));
                        _cache.Remove(attemptKey); // Reset attempts since they are now locked out
                        return BadRequest(new { message = "Too many failed attempts. Password update is locked for 4 hours." });
                    }
                    else
                    {
                        _cache.Set(attemptKey, attempts, TimeSpan.FromHours(4));
                        return BadRequest(new { message = $"Incorrect old password. You have {3 - attempts} attempts remaining before being locked out for 4 hours." });
                    }
                }
            }

            // Clear attempts on success
            var attemptKeyToClear = $"pw_attempts_{emailKey}";
            _cache.Remove(attemptKeyToClear);

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            user.UpdatedAt = DateTimeHelper.GetIndianTime();
            user.UpdatedBy = user.FullName;
            await _db.SaveChangesAsync();

            return Ok(new { message = "Password updated successfully." });
        }

        private CustomerAuthResponseDto BuildResponse(Customer user, bool includeToken = true) => new(
            Token: includeToken ? GenerateJwt(user) : string.Empty,
            Email: user.Email ?? "",
            FullName: user.FullName ?? "User",
            MobileNumber: user.PhoneNumber ?? "",
            Role: "customer",
            Avatar: !string.IsNullOrWhiteSpace(user.FullName) ? string.Concat(user.FullName.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(n => n.Length > 0 ? n[0].ToString() : "U")) : "U",
            Address: user.Address ?? "",
            Gender: user.Gender,
            DateOfBirth: user.DateOfBirth,
            HasPassword: !string.IsNullOrEmpty(user.PasswordHash)
        );

        private string GenerateJwt(Customer user)
        {
            var secret = _config["Jwt:Secret"] ?? throw new InvalidOperationException("JWT Secret not configured.");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,   user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role,               "customer"),
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
