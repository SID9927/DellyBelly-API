-- Run this script on your SQL Server database to add audit tracking columns

-- 1. Update SiteSettings table
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[SiteSettings]') AND name = 'UpdatedAt')
BEGIN
    ALTER TABLE [SiteSettings] ADD [UpdatedAt] DATETIME2 NOT NULL DEFAULT '2024-01-01';
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[SiteSettings]') AND name = 'UpdatedBy')
BEGIN
    ALTER TABLE [SiteSettings] ADD [UpdatedBy] NVARCHAR(MAX) NULL;
END
GO

-- 2. Update Customers table
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Customers]') AND name = 'UpdatedAt')
BEGIN
    ALTER TABLE [Customers] ADD [UpdatedAt] DATETIME2 NULL;
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Customers]') AND name = 'UpdatedBy')
BEGIN
    ALTER TABLE [Customers] ADD [UpdatedBy] NVARCHAR(MAX) NULL;
END
GO

-- Optional: If you haven't added CreatedAt to Customers yet
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Customers]') AND name = 'CreatedAt')
BEGIN
    ALTER TABLE [Customers] ADD [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE();
END
GO
