using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DellyBelly.Shared.Helpers
{
    public class PasswordHelper
    {
        private readonly PasswordHasher<object> _hasher = new PasswordHasher<object>();

        // Hash plain text password before saving
        public string HashPassword(string password)
        {
            return _hasher.HashPassword(null, password);
        }

        // Verify plain text password against stored hash
        public bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            var result = _hasher.VerifyHashedPassword(null, hashedPassword, providedPassword);
            return result == PasswordVerificationResult.Success;
        }

    }
}
