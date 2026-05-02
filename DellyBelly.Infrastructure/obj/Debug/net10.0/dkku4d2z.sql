IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [Categories] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(100) NOT NULL,
    [Description] nvarchar(500) NOT NULL,
    [ImageUrl] nvarchar(255) NOT NULL,
    [IsActive] bit NOT NULL DEFAULT CAST(1 AS bit),
    [CreatedAt] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    CONSTRAINT [PK_Categories] PRIMARY KEY ([Id])
);

CREATE TABLE [Products] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(100) NOT NULL,
    [Description] nvarchar(500) NOT NULL,
    [Price] decimal(18,2) NOT NULL,
    [ImageUrl] nvarchar(255) NOT NULL,
    [Stock] int NOT NULL DEFAULT 0,
    [IsAvailable] bit NOT NULL DEFAULT CAST(1 AS bit),
    [CreatedAt] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [CategoryId] int NOT NULL,
    CONSTRAINT [PK_Products] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Products_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [Categories] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_Products_CategoryId] ON [Products] ([CategoryId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251130114037_InitialCreate', N'10.0.0');

COMMIT;
GO

BEGIN TRANSACTION;
CREATE TABLE [Customers] (
    [Id] int NOT NULL IDENTITY,
    [FullName] nvarchar(100) NOT NULL,
    [Email] nvarchar(100) NOT NULL,
    [PhoneNumber] nvarchar(15) NOT NULL,
    [Address] nvarchar(255) NOT NULL,
    [TotalSpent] decimal(18,2) NOT NULL DEFAULT 0.0,
    [CreatedAt] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    CONSTRAINT [PK_Customers] PRIMARY KEY ([Id])
);

CREATE TABLE [Employees] (
    [Id] int NOT NULL IDENTITY,
    [FullName] nvarchar(100) NOT NULL,
    [Email] nvarchar(100) NOT NULL,
    [PasswordHash] nvarchar(max) NOT NULL,
    [Role] nvarchar(50) NOT NULL,
    [CreatedAt] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    CONSTRAINT [PK_Employees] PRIMARY KEY ([Id])
);

CREATE UNIQUE INDEX [IX_Customers_Email] ON [Customers] ([Email]);

CREATE UNIQUE INDEX [IX_Employees_Email] ON [Employees] ([Email]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251130170454_AddCustomer', N'10.0.0');

COMMIT;
GO

BEGIN TRANSACTION;
INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251130170550_AddEmployee', N'10.0.0');

COMMIT;
GO

BEGIN TRANSACTION;
DECLARE @var nvarchar(max);
SELECT @var = QUOTENAME([d].[name])
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Products]') AND [c].[name] = N'ImageUrl');
IF @var IS NOT NULL EXEC(N'ALTER TABLE [Products] DROP CONSTRAINT ' + @var + ';');
ALTER TABLE [Products] DROP COLUMN [ImageUrl];

DECLARE @var1 nvarchar(max);
SELECT @var1 = QUOTENAME([d].[name])
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Categories]') AND [c].[name] = N'ImageUrl');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Categories] DROP CONSTRAINT ' + @var1 + ';');
ALTER TABLE [Categories] DROP COLUMN [ImageUrl];

ALTER TABLE [Categories] ADD [ImageId] int NULL;

CREATE TABLE [Images] (
    [Id] int NOT NULL IDENTITY,
    [FileName] nvarchar(255) NOT NULL,
    [ContentType] nvarchar(50) NOT NULL,
    [Data] varbinary(max) NOT NULL,
    [UploadedAt] datetime2 NOT NULL,
    [ProductId] int NULL,
    CONSTRAINT [PK_Images] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Images_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE CASCADE
);

CREATE UNIQUE INDEX [IX_Categories_ImageId] ON [Categories] ([ImageId]) WHERE [ImageId] IS NOT NULL;

CREATE INDEX [IX_Images_ProductId] ON [Images] ([ProductId]);

ALTER TABLE [Categories] ADD CONSTRAINT [FK_Categories_Images_ImageId] FOREIGN KEY ([ImageId]) REFERENCES [Images] ([Id]) ON DELETE NO ACTION;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251201165747_UpdateImageRelationships', N'10.0.0');

COMMIT;
GO

BEGIN TRANSACTION;
ALTER TABLE [Categories] DROP CONSTRAINT [FK_Categories_Images_ImageId];

DROP INDEX [IX_Categories_ImageId] ON [Categories];

DECLARE @var2 nvarchar(max);
SELECT @var2 = QUOTENAME([d].[name])
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Categories]') AND [c].[name] = N'ImageId');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Categories] DROP CONSTRAINT ' + @var2 + ';');
ALTER TABLE [Categories] DROP COLUMN [ImageId];

ALTER TABLE [Images] ADD [CategoryId] int NULL;

ALTER TABLE [Images] ADD [GalleryId] int NULL;

ALTER TABLE [Images] ADD [Source] nvarchar(50) NOT NULL DEFAULT N'';

CREATE TABLE [Galleries] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(100) NOT NULL,
    [Description] nvarchar(500) NOT NULL,
    [IsActive] bit NOT NULL DEFAULT CAST(1 AS bit),
    [CreatedAt] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    CONSTRAINT [PK_Galleries] PRIMARY KEY ([Id])
);

CREATE UNIQUE INDEX [IX_Images_CategoryId] ON [Images] ([CategoryId]) WHERE [CategoryId] IS NOT NULL;

CREATE INDEX [IX_Images_GalleryId] ON [Images] ([GalleryId]);

ALTER TABLE [Images] ADD CONSTRAINT [FK_Images_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [Categories] ([Id]) ON DELETE NO ACTION;

ALTER TABLE [Images] ADD CONSTRAINT [FK_Images_Galleries_GalleryId] FOREIGN KEY ([GalleryId]) REFERENCES [Galleries] ([Id]) ON DELETE CASCADE;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251203172006_AddGalleryAndRefactorImages', N'10.0.0');

COMMIT;
GO

BEGIN TRANSACTION;
ALTER TABLE [Products] ADD [IsBestSeller] bit NOT NULL DEFAULT CAST(0 AS bit);

ALTER TABLE [Products] ADD [IsRecommended] bit NOT NULL DEFAULT CAST(0 AS bit);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20251226085050_AddHomeFlags', N'10.0.0');

COMMIT;
GO

BEGIN TRANSACTION;
CREATE TABLE [ApiLogs] (
    [Id] bigint NOT NULL IDENTITY,
    [RequestTime] datetime2 NOT NULL,
    [ResponseTime] datetime2 NOT NULL,
    [DurationMs] bigint NOT NULL,
    [Method] nvarchar(max) NOT NULL,
    [Path] nvarchar(max) NOT NULL,
    [QueryString] nvarchar(max) NULL,
    [RequestBody] nvarchar(max) NULL,
    [ResponseBody] nvarchar(max) NULL,
    [StatusCode] int NOT NULL,
    [IpAddress] nvarchar(max) NULL,
    CONSTRAINT [PK_ApiLogs] PRIMARY KEY ([Id])
);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260110084157_AddApiLogging', N'10.0.0');

COMMIT;
GO

BEGIN TRANSACTION;
ALTER TABLE [ApiLogs] ADD [Duration] nvarchar(max) NULL;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260110091625_AddDurationToApiLog', N'10.0.0');

COMMIT;
GO

BEGIN TRANSACTION;
DECLARE @var3 nvarchar(max);
SELECT @var3 = QUOTENAME([d].[name])
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ApiLogs]') AND [c].[name] = N'DurationMs');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [ApiLogs] DROP CONSTRAINT ' + @var3 + ';');
ALTER TABLE [ApiLogs] DROP COLUMN [DurationMs];

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260110092645_RemoveDurationMs', N'10.0.0');

COMMIT;
GO

BEGIN TRANSACTION;

                UPDATE [ApiLogs] SET
                    [ResponseBody] = CASE WHEN LEN([ResponseBody]) > 4000 THEN LEFT([ResponseBody], 4000) ELSE [ResponseBody] END,
                    [RequestBody]  = CASE WHEN LEN([RequestBody])  > 4000 THEN LEFT([RequestBody],  4000) ELSE [RequestBody]  END,
                    [QueryString]  = CASE WHEN LEN([QueryString])  > 4000 THEN LEFT([QueryString],  4000) ELSE [QueryString]  END,
                    [Duration]     = CASE WHEN LEN([Duration])     > 20   THEN LEFT([Duration],     20)   ELSE [Duration]     END
                WHERE
                    LEN([ResponseBody]) > 4000
                    OR LEN([RequestBody])  > 4000
                    OR LEN([QueryString])  > 4000
                    OR LEN([Duration])     > 20;
            

DECLARE @var4 nvarchar(max);
SELECT @var4 = QUOTENAME([d].[name])
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ApiLogs]') AND [c].[name] = N'ResponseBody');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [ApiLogs] DROP CONSTRAINT ' + @var4 + ';');
ALTER TABLE [ApiLogs] ALTER COLUMN [ResponseBody] nvarchar(4000) NULL;

DECLARE @var5 nvarchar(max);
SELECT @var5 = QUOTENAME([d].[name])
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ApiLogs]') AND [c].[name] = N'RequestBody');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [ApiLogs] DROP CONSTRAINT ' + @var5 + ';');
ALTER TABLE [ApiLogs] ALTER COLUMN [RequestBody] nvarchar(4000) NULL;

DECLARE @var6 nvarchar(max);
SELECT @var6 = QUOTENAME([d].[name])
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ApiLogs]') AND [c].[name] = N'QueryString');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [ApiLogs] DROP CONSTRAINT ' + @var6 + ';');
ALTER TABLE [ApiLogs] ALTER COLUMN [QueryString] nvarchar(4000) NULL;

DECLARE @var7 nvarchar(max);
SELECT @var7 = QUOTENAME([d].[name])
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ApiLogs]') AND [c].[name] = N'Duration');
IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [ApiLogs] DROP CONSTRAINT ' + @var7 + ';');
ALTER TABLE [ApiLogs] ALTER COLUMN [Duration] nvarchar(20) NULL;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260221091306_AddApiLogColumnConstraints', N'10.0.0');

COMMIT;
GO

BEGIN TRANSACTION;
CREATE INDEX [IX_Products_IsAvailable] ON [Products] ([IsAvailable]);

CREATE INDEX [IX_Products_IsBestSeller] ON [Products] ([IsBestSeller]);

CREATE INDEX [IX_Products_IsRecommended] ON [Products] ([IsRecommended]);

CREATE INDEX [IX_Images_Source] ON [Images] ([Source]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260222115719_AddPerformanceIndexes', N'10.0.0');

COMMIT;
GO

BEGIN TRANSACTION;
INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260222121003_AddPerformanceIndexe', N'10.0.0');

COMMIT;
GO

