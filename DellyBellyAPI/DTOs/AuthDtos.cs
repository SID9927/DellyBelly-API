namespace DellyBellyAPI.DTOs
{
    public record LoginDto(string Email, string Password);

    public record RegisterDto(string FullName, string Email, string MobileNumber, string Password);

    public record ForgotPasswordDto(string Email);

    public record ResetPasswordDto(string Token, string NewPassword);

    public record VerifyOtpDto(string Email, string OTP);
    
    public record ResendOtpDto(string Email);

    public record GoogleLoginDto(string Credential);

    public record CheckEmailDto(string Email);
    public record EmailCheckResponseDto(bool Exists);

    public record AuthResponseDto(
        string Token,
        string Email,
        string FullName,
        string MobileNumber,
        string Role,
        string Avatar
    );

    public record CustomerLoginDto(string Email, string Password);
    
    public record CustomerRegisterDto(
        string FirstName, 
        string? LastName, 
        string Mobile, 
        string Password, 
        string Address, 
        string Gender, 
        DateTime DateOfBirth, 
        string Email);

    public record CustomerAuthResponseDto(
        string Token,
        string Email,
        string FullName,
        string MobileNumber,
        string Role,
        string Avatar,
        string Address,
        string? Gender,
        DateTime? DateOfBirth,
        bool HasPassword
    );

    public record UpdateCustomerProfileDto(
        string FullName,
        string MobileNumber,
        string? Gender,
        DateTime? DateOfBirth
    );

    public record ChangePasswordDto(string? OldPassword, string NewPassword);
}
