namespace DellyBelly.Domain.Entities
{
    /// <summary>
    /// Represents an admin CMS user account.
    /// Passwords are stored as BCrypt hashes — never plaintext.
    /// </summary>
    public class AdminUser
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        /// <summary>Role: super_admin | admin | manager | staff</summary>
        public string Role { get; set; } = "staff";

        public bool IsActive { get; set; } = true;
        
        /// <summary>OTP verification fields</summary>
        public bool IsEmailVerified { get; set; } = false;
        public string? OTP { get; set; }
        public DateTime? OTPExpiry { get; set; }

        /// <summary>Token stored for password-reset flow (expires in 1 hour)</summary>
        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpiry { get; set; }

        /// <summary>Stored in IST using DateTimeHelper.GetIndianTime()</summary>
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
    }
}
