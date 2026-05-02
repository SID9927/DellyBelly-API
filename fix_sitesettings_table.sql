-- Run this script on your SQL Server database to ensure ALL SiteSettings columns exist

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SiteSettings')
BEGIN
    CREATE TABLE [SiteSettings] (
        [Id] INT PRIMARY KEY IDENTITY(1,1),
        [StoreName] NVARCHAR(MAX) NULL,
        [Tagline] NVARCHAR(MAX) NULL,
        [Email] NVARCHAR(MAX) NULL,
        [Phone] NVARCHAR(MAX) NULL,
        [WhatsApp] NVARCHAR(MAX) NULL,
        [Address] NVARCHAR(MAX) NULL,
        [SwiggyUrl] NVARCHAR(MAX) NULL,
        [ZomatoUrl] NVARCHAR(MAX) NULL,
        [MagicPinUrl] NVARCHAR(MAX) NULL,
        [BusinessHoursJson] NVARCHAR(MAX) NULL,
        [AnnouncementEnabled] BIT NOT NULL DEFAULT 0,
        [AnnouncementText] NVARCHAR(MAX) NULL,
        [AnnouncementVisibility] NVARCHAR(50) NOT NULL DEFAULT 'both',
        [UpdatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [UpdatedBy] NVARCHAR(MAX) NULL
    );
END
ELSE
BEGIN
    -- Add missing columns one by one
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[SiteSettings]') AND name = 'StoreName')
        ALTER TABLE [SiteSettings] ADD [StoreName] NVARCHAR(MAX) NULL;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[SiteSettings]') AND name = 'Tagline')
        ALTER TABLE [SiteSettings] ADD [Tagline] NVARCHAR(MAX) NULL;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[SiteSettings]') AND name = 'Email')
        ALTER TABLE [SiteSettings] ADD [Email] NVARCHAR(MAX) NULL;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[SiteSettings]') AND name = 'Phone')
        ALTER TABLE [SiteSettings] ADD [Phone] NVARCHAR(MAX) NULL;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[SiteSettings]') AND name = 'WhatsApp')
        ALTER TABLE [SiteSettings] ADD [WhatsApp] NVARCHAR(MAX) NULL;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[SiteSettings]') AND name = 'Address')
        ALTER TABLE [SiteSettings] ADD [Address] NVARCHAR(MAX) NULL;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[SiteSettings]') AND name = 'SwiggyUrl')
        ALTER TABLE [SiteSettings] ADD [SwiggyUrl] NVARCHAR(MAX) NULL;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[SiteSettings]') AND name = 'ZomatoUrl')
        ALTER TABLE [SiteSettings] ADD [ZomatoUrl] NVARCHAR(MAX) NULL;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[SiteSettings]') AND name = 'MagicPinUrl')
        ALTER TABLE [SiteSettings] ADD [MagicPinUrl] NVARCHAR(MAX) NULL;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[SiteSettings]') AND name = 'BusinessHoursJson')
        ALTER TABLE [SiteSettings] ADD [BusinessHoursJson] NVARCHAR(MAX) NULL;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[SiteSettings]') AND name = 'AnnouncementEnabled')
        ALTER TABLE [SiteSettings] ADD [AnnouncementEnabled] BIT NOT NULL DEFAULT 0;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[SiteSettings]') AND name = 'AnnouncementText')
        ALTER TABLE [SiteSettings] ADD [AnnouncementText] NVARCHAR(MAX) NULL;

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[SiteSettings]') AND name = 'AnnouncementVisibility')
        ALTER TABLE [SiteSettings] ADD [AnnouncementVisibility] NVARCHAR(50) NOT NULL DEFAULT 'both';

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[SiteSettings]') AND name = 'UpdatedAt')
        ALTER TABLE [SiteSettings] ADD [UpdatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE();

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[SiteSettings]') AND name = 'UpdatedBy')
        ALTER TABLE [SiteSettings] ADD [UpdatedBy] NVARCHAR(MAX) NULL;
END
GO
