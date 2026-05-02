-- Migration: Add OTP and Profile fields to AdminUsers and Customers

-- Update AdminUsers table
ALTER TABLE [AdminUsers]
ADD 
    [IsEmailVerified] BIT NOT NULL DEFAULT 0,
    [OTP] NVARCHAR(MAX) NULL,
    [OTPExpiry] DATETIME2 NULL;
GO

-- Update Customers table
-- Note: PasswordHash is required, but existing rows might cause issues if not given a default.
ALTER TABLE [Customers]
ADD 
    [PasswordHash] NVARCHAR(MAX) NOT NULL DEFAULT '',
    [Gender] NVARCHAR(MAX) NULL,
    [DateOfBirth] DATETIME2 NULL,
    [IsEmailVerified] BIT NOT NULL DEFAULT 0,
    [OTP] NVARCHAR(MAX) NULL,
    [OTPExpiry] DATETIME2 NULL;
GO
