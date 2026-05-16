-- =====================================================
-- AddIngredients Migration — Run this on your SQL Server
-- Adds: Ingredients table + ProductIngredients join table
-- =====================================================

-- 1. Create Ingredients table
-- =====================================================
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

-- 2. Create ProductIngredients join table (composite PK)
-- =====================================================
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

-- 3. Default values
-- =====================================================
ALTER TABLE [dbo].[Ingredients] ADD DEFAULT (CONVERT([bit],(0))) FOR [IsAllergen]
GO
ALTER TABLE [dbo].[Ingredients] ADD DEFAULT (CONVERT([bit],(1))) FOR [IsActive]
GO
ALTER TABLE [dbo].[Ingredients] ADD DEFAULT (getutcdate()) FOR [CreatedAt]
GO

-- 4. Indexes
-- =====================================================
SET ANSI_PADDING ON
GO
-- Unique: same ingredient name cannot exist twice in the same category
CREATE UNIQUE NONCLUSTERED INDEX [IX_Ingredients_CategoryId_Name] ON [dbo].[Ingredients]
(
	[CategoryId] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

-- Index on ProductIngredients for fast lookups by product
CREATE NONCLUSTERED INDEX [IX_ProductIngredients_IngredientId] ON [dbo].[ProductIngredients]
(
	[IngredientId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

-- 5. Foreign Keys
-- =====================================================

-- Ingredients → Categories (ON DELETE CASCADE: delete category removes its ingredients)
ALTER TABLE [dbo].[Ingredients] WITH CHECK ADD CONSTRAINT [FK_Ingredients_Categories_CategoryId] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Categories] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Ingredients] CHECK CONSTRAINT [FK_Ingredients_Categories_CategoryId]
GO

-- ProductIngredients → Products (ON DELETE CASCADE: delete product removes its links)
ALTER TABLE [dbo].[ProductIngredients] WITH CHECK ADD CONSTRAINT [FK_ProductIngredients_Products_ProductId] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProductIngredients] CHECK CONSTRAINT [FK_ProductIngredients_Products_ProductId]
GO

-- ProductIngredients → Ingredients (NO ACTION: deleting ingredient won't delete links automatically)
ALTER TABLE [dbo].[ProductIngredients] WITH CHECK ADD CONSTRAINT [FK_ProductIngredients_Ingredients_IngredientId] FOREIGN KEY([IngredientId])
REFERENCES [dbo].[Ingredients] ([Id])
GO
ALTER TABLE [dbo].[ProductIngredients] CHECK CONSTRAINT [FK_ProductIngredients_Ingredients_IngredientId]
GO

-- =====================================================
-- DONE! Tables created:
--   dbo.Ingredients          (Id, Name, CategoryId, IsAllergen, IsActive, CreatedAt)
--   dbo.ProductIngredients   (ProductId, IngredientId)
-- =====================================================
