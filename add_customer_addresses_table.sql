-- SQL Command to create the CustomerAddresses table
-- Run this in your database!

CREATE TABLE [CustomerAddresses] (
    [Id] int NOT NULL IDENTITY,
    [CustomerId] int NOT NULL,
    [Type] nvarchar(50) NOT NULL DEFAULT 'Home',
    [AddressText] nvarchar(max) NOT NULL,
    [IsDefault] bit NOT NULL DEFAULT 0,
    [CreatedAt] datetime2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT [PK_CustomerAddresses] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_CustomerAddresses_Customers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [Customers] ([Id]) ON DELETE CASCADE
);
GO

-- Create an index to speed up fetching addresses for a specific customer
CREATE INDEX [IX_CustomerAddresses_CustomerId] ON [CustomerAddresses] ([CustomerId]);
GO
