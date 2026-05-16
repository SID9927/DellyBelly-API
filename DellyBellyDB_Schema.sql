USE [master]
GO

/****** Object:  Database [DellyBellyDB] ******/
CREATE DATABASE [DellyBellyDB]
GO

ALTER DATABASE [DellyBellyDB] SET COMPATIBILITY_LEVEL = 170
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [DellyBellyDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [DellyBellyDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [DellyBellyDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [DellyBellyDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [DellyBellyDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [DellyBellyDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [DellyBellyDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [DellyBellyDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [DellyBellyDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [DellyBellyDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [DellyBellyDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [DellyBellyDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [DellyBellyDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [DellyBellyDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [DellyBellyDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [DellyBellyDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [DellyBellyDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [DellyBellyDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [DellyBellyDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [DellyBellyDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [DellyBellyDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [DellyBellyDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [DellyBellyDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [DellyBellyDB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [DellyBellyDB] SET  MULTI_USER 
GO
ALTER DATABASE [DellyBellyDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [DellyBellyDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [DellyBellyDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [DellyBellyDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [DellyBellyDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [DellyBellyDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [DellyBellyDB] SET OPTIMIZED_LOCKING = OFF 
GO
ALTER DATABASE [DellyBellyDB] SET QUERY_STORE = ON
GO
ALTER DATABASE [DellyBellyDB] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO

USE [DellyBellyDB]
GO

/****** Object:  Table [dbo].[__EFMigrationsHistory] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[AdminUsers] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AdminUsers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](150) NOT NULL,
	[Email] [nvarchar](200) NOT NULL,
	[MobileNumber] [nvarchar](20) NOT NULL,
	[PasswordHash] [nvarchar](500) NOT NULL,
	[Role] [nvarchar](50) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[PasswordResetToken] [nvarchar](500) NULL,
	[PasswordResetTokenExpiry] [datetime2](7) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[LastLoginAt] [datetime2](7) NULL,
	[IsEmailVerified] [bit] NOT NULL,
	[OTP] [nvarchar](max) NULL,
	[OTPExpiry] [datetime2](7) NULL,
 CONSTRAINT [PK_AdminUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [dbo].[ApiLogs] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApiLogs](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[RequestTime] [datetime2](7) NOT NULL,
	[ResponseTime] [datetime2](7) NOT NULL,
	[Method] [nvarchar](max) NOT NULL,
	[Path] [nvarchar](max) NOT NULL,
	[QueryString] [nvarchar](4000) NULL,
	[RequestBody] [nvarchar](4000) NULL,
	[ResponseBody] [nvarchar](4000) NULL,
	[StatusCode] [int] NOT NULL,
	[IpAddress] [nvarchar](max) NULL,
	[Duration] [nvarchar](20) NULL,
 CONSTRAINT [PK_ApiLogs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Categories] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](500) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[ContactMessages] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContactMessages](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](255) NOT NULL,
	[Email] [nvarchar](255) NOT NULL,
	[PhoneNumber] [nvarchar](50) NULL,
	[Message] [nvarchar](max) NOT NULL,
	[SubmittedAt] [datetime] NOT NULL,
	[IsRead] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [dbo].[CustomerAddresses] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerAddresses](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NOT NULL,
	[Type] [nvarchar](50) NOT NULL,
	[AddressText] [nvarchar](max) NOT NULL,
	[IsDefault] [bit] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_CustomerAddresses] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Customers] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customers](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[PhoneNumber] [nvarchar](15) NOT NULL,
	[Address] [nvarchar](255) NOT NULL,
	[TotalSpent] [decimal](18, 2) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[PasswordHash] [nvarchar](max) NOT NULL,
	[Gender] [nvarchar](max) NULL,
	[DateOfBirth] [datetime2](7) NULL,
	[IsEmailVerified] [bit] NOT NULL,
	[OTP] [nvarchar](max) NULL,
	[OTPExpiry] [datetime2](7) NULL,
	[PasswordResetToken] [nvarchar](max) NULL,
	[PasswordResetTokenExpiry] [datetime] NULL,
	[UpdatedAt] [datetime] NULL,
	[UpdatedBy] [nvarchar](255) NULL,
 CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [dbo].[EmailLogs] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmailLogs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Recipient] [nvarchar](max) NOT NULL,
	[Subject] [nvarchar](max) NOT NULL,
	[Purpose] [nvarchar](max) NOT NULL,
	[SentAt] [datetime2](7) NOT NULL,
	[IsSuccess] [bit] NOT NULL,
 CONSTRAINT [PK_EmailLogs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Employees] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employees](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[PasswordHash] [nvarchar](max) NOT NULL,
	[Role] [nvarchar](50) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Employees] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [dbo].[FeedbackCategories] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FeedbackCategories](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[IsActive] [bit] NULL,
	[CreatedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Feedbacks] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Feedbacks](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[CategoryId] [int] NOT NULL,
	[Subject] [nvarchar](255) NOT NULL,
	[Comment] [nvarchar](max) NOT NULL,
	[Rating] [int] NOT NULL,
	[IsApproved] [bit] NULL,
	[SubmittedAt] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Galleries] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Galleries](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[FileName] [nvarchar](255) NULL,
	[ContentType] [nvarchar](100) NULL,
	[Data] [varbinary](max) NULL,
	[UploadedAt] [datetime2](7) NULL,
 CONSTRAINT [PK_Galleries] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Images] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Images](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FileName] [nvarchar](255) NOT NULL,
	[ContentType] [nvarchar](50) NOT NULL,
	[Data] [varbinary](max) NOT NULL,
	[UploadedAt] [datetime2](7) NOT NULL,
	[ProductId] [int] NULL,
	[CategoryId] [int] NULL,
	[Source] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Images] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Ingredients] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ingredients](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[CategoryId] [int] NOT NULL,
	[IsAllergen] [bit] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Ingredients] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[NewsletterSubscriptions] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NewsletterSubscriptions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](255) NOT NULL,
	[SubscribedAt] [datetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[ProductIngredients] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductIngredients](
	[ProductId] [int] NOT NULL,
	[IngredientId] [int] NOT NULL,
 CONSTRAINT [PK_ProductIngredients] PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC,
	[IngredientId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Products] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](500) NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[Stock] [int] NOT NULL,
	[IsAvailable] [bit] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[CategoryId] [int] NOT NULL,
	[IsBestSeller] [bit] NOT NULL,
	[IsRecommended] [bit] NOT NULL,
 CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[SiteSettings] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SiteSettings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StoreName] [nvarchar](max) NOT NULL,
	[Tagline] [nvarchar](max) NOT NULL,
	[Email] [nvarchar](max) NOT NULL,
	[Phone] [nvarchar](max) NOT NULL,
	[WhatsApp] [nvarchar](max) NOT NULL,
	[Address] [nvarchar](max) NOT NULL,
	[SwiggyUrl] [nvarchar](max) NOT NULL,
	[ZomatoUrl] [nvarchar](max) NOT NULL,
	[MagicPinUrl] [nvarchar](max) NOT NULL,
	[BusinessHoursJson] [nvarchar](max) NOT NULL,
	[UpdatedAt] [datetime2](7) NOT NULL,
	[AnnouncementEnabled] [bit] NOT NULL,
	[AnnouncementText] [nvarchar](max) NULL,
	[AnnouncementVisibility] [nvarchar](50) NOT NULL,
	[UpdatedBy] [nvarchar](255) NULL,
 CONSTRAINT [PK_SiteSettings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Wishlists] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Wishlists](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[AddedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_Wishlists] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING ON
GO

/****** Indexes ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_AdminUsers_Email] ON [dbo].[AdminUsers] ([Email] ASC) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_CustomerAddresses_CustomerId] ON [dbo].[CustomerAddresses] ([CustomerId] ASC) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Customers_Email] ON [dbo].[Customers] ([Email] ASC) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Employees_Email] ON [dbo].[Employees] ([Email] ASC) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Images_CategoryId] ON [dbo].[Images] ([CategoryId] ASC) WHERE ([CategoryId] IS NOT NULL) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Images_ProductId] ON [dbo].[Images] ([ProductId] ASC) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Images_Source] ON [dbo].[Images] ([Source] ASC) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Ingredients_CategoryId_Name] ON [dbo].[Ingredients] ([CategoryId] ASC, [Name] ASC) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_NewsletterSubscriptions_Email] ON [dbo].[NewsletterSubscriptions] ([Email] ASC) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_ProductIngredients_IngredientId] ON [dbo].[ProductIngredients] ([IngredientId] ASC) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Products_CategoryId] ON [dbo].[Products] ([CategoryId] ASC) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Products_IsAvailable] ON [dbo].[Products] ([IsAvailable] ASC) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Products_IsBestSeller] ON [dbo].[Products] ([IsBestSeller] ASC) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Products_IsRecommended] ON [dbo].[Products] ([IsRecommended] ASC) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Wishlists_User_Product] ON [dbo].[Wishlists] ([UserId] ASC, [ProductId] ASC) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Wishlists_UserId] ON [dbo].[Wishlists] ([UserId] ASC) ON [PRIMARY]
GO

/****** Default Constraints ******/
ALTER TABLE [dbo].[AdminUsers] ADD  DEFAULT ('staff') FOR [Role]
GO
ALTER TABLE [dbo].[AdminUsers] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[AdminUsers] ADD  DEFAULT ((0)) FOR [IsEmailVerified]
GO
ALTER TABLE [dbo].[Categories] ADD  DEFAULT (CONVERT([bit],(1))) FOR [IsActive]
GO
ALTER TABLE [dbo].[Categories] ADD  DEFAULT (getutcdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[ContactMessages] ADD  DEFAULT ((0)) FOR [IsRead]
GO
ALTER TABLE [dbo].[CustomerAddresses] ADD  DEFAULT ('Home') FOR [Type]
GO
ALTER TABLE [dbo].[CustomerAddresses] ADD  DEFAULT ((0)) FOR [IsDefault]
GO
ALTER TABLE [dbo].[CustomerAddresses] ADD  DEFAULT (getutcdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Customers] ADD  DEFAULT ((0.0)) FOR [TotalSpent]
GO
ALTER TABLE [dbo].[Customers] ADD  DEFAULT (getutcdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Customers] ADD  DEFAULT ('') FOR [PasswordHash]
GO
ALTER TABLE [dbo].[Customers] ADD  DEFAULT ((0)) FOR [IsEmailVerified]
GO
ALTER TABLE [dbo].[Employees] ADD  DEFAULT (getutcdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[FeedbackCategories] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[FeedbackCategories] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Feedbacks] ADD  DEFAULT ((5)) FOR [Rating]
GO
ALTER TABLE [dbo].[Feedbacks] ADD  DEFAULT ((0)) FOR [IsApproved]
GO
ALTER TABLE [dbo].[Feedbacks] ADD  DEFAULT (getdate()) FOR [SubmittedAt]
GO
ALTER TABLE [dbo].[Galleries] ADD  DEFAULT (CONVERT([bit],(1))) FOR [IsActive]
GO
ALTER TABLE [dbo].[Galleries] ADD  DEFAULT ('image/webp') FOR [ContentType]
GO
ALTER TABLE [dbo].[Galleries] ADD  DEFAULT (getutcdate()) FOR [UploadedAt]
GO
ALTER TABLE [dbo].[Images] ADD  DEFAULT (N'') FOR [Source]
GO
ALTER TABLE [dbo].[Ingredients] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsAllergen]
GO
ALTER TABLE [dbo].[Ingredients] ADD  DEFAULT (CONVERT([bit],(1))) FOR [IsActive]
GO
ALTER TABLE [dbo].[Ingredients] ADD  DEFAULT (getutcdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[NewsletterSubscriptions] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Products] ADD  DEFAULT ((0)) FOR [Stock]
GO
ALTER TABLE [dbo].[Products] ADD  DEFAULT (CONVERT([bit],(1))) FOR [IsAvailable]
GO
ALTER TABLE [dbo].[Products] ADD  DEFAULT (getutcdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Products] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsBestSeller]
GO
ALTER TABLE [dbo].[Products] ADD  DEFAULT (CONVERT([bit],(0))) FOR [IsRecommended]
GO
ALTER TABLE [dbo].[SiteSettings] ADD  DEFAULT (getdate()) FOR [UpdatedAt]
GO
ALTER TABLE [dbo].[SiteSettings] ADD  DEFAULT ((0)) FOR [AnnouncementEnabled]
GO
ALTER TABLE [dbo].[SiteSettings] ADD  DEFAULT ('both') FOR [AnnouncementVisibility]
GO

/****** Foreign Key Constraints ******/
ALTER TABLE [dbo].[CustomerAddresses]  WITH CHECK ADD  CONSTRAINT [FK_CustomerAddresses_Customers_CustomerId] FOREIGN KEY([CustomerId]) REFERENCES [dbo].[Customers] ([Id]) ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Feedbacks]  WITH CHECK ADD  CONSTRAINT [FK_Feedbacks_Categories] FOREIGN KEY([CategoryId]) REFERENCES [dbo].[FeedbackCategories] ([Id]) ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Feedbacks]  WITH CHECK ADD  CONSTRAINT [FK_Feedbacks_Customers] FOREIGN KEY([UserId]) REFERENCES [dbo].[Customers] ([Id]) ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Images]  WITH CHECK ADD  CONSTRAINT [FK_Images_Categories_CategoryId] FOREIGN KEY([CategoryId]) REFERENCES [dbo].[Categories] ([Id])
GO
ALTER TABLE [dbo].[Images]  WITH CHECK ADD  CONSTRAINT [FK_Images_Products_ProductId] FOREIGN KEY([ProductId]) REFERENCES [dbo].[Products] ([Id]) ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Ingredients]  WITH CHECK ADD  CONSTRAINT [FK_Ingredients_Categories_CategoryId] FOREIGN KEY([CategoryId]) REFERENCES [dbo].[Categories] ([Id]) ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProductIngredients]  WITH CHECK ADD  CONSTRAINT [FK_ProductIngredients_Ingredients_IngredientId] FOREIGN KEY([IngredientId]) REFERENCES [dbo].[Ingredients] ([Id])
GO
ALTER TABLE [dbo].[ProductIngredients]  WITH CHECK ADD  CONSTRAINT [FK_ProductIngredients_Products_ProductId] FOREIGN KEY([ProductId]) REFERENCES [dbo].[Products] ([Id]) ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_Categories_CategoryId] FOREIGN KEY([CategoryId]) REFERENCES [dbo].[Categories] ([Id]) ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Wishlists]  WITH CHECK ADD  CONSTRAINT [FK_Wishlists_Customers] FOREIGN KEY([UserId]) REFERENCES [dbo].[Customers] ([Id]) ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Wishlists]  WITH CHECK ADD  CONSTRAINT [FK_Wishlists_Products] FOREIGN KEY([ProductId]) REFERENCES [dbo].[Products] ([Id]) ON DELETE CASCADE
GO

ALTER DATABASE [DellyBellyDB] SET READ_WRITE 
GO
