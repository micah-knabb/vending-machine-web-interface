USE [master];
GO

BEGIN TRY
	DROP DATABASE [VendingMachine];
END TRY
BEGIN CATCH	
END CATCH
GO

CREATE DATABASE [VendingMachine];
GO

USE [VendingMachine]
GO

/****** Object:  User [VendingUser]    Script Date: 12/7/2018 3:24:11 PM ******/
CREATE USER [VendingUser] FOR LOGIN [VendingUser] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [VendingUser]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 12/7/2018 3:24:11 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Noise] [varchar](150) NOT NULL,
 CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Inventory]    Script Date: 12/7/2018 3:24:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Inventory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Qty] [int] NOT NULL,
	[Row] [int] NOT NULL,
	[Col] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
 CONSTRAINT [PK_Inventory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 12/7/2018 3:24:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Image] [varchar](500) NOT NULL,
	[Price] [real] NOT NULL,
	[CategoryId] [int] NOT NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RoleItem]    Script Date: 12/7/2018 3:24:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoleItem](
	[Id] [int] NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_RoleItem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TransactionItem]    Script Date: 12/7/2018 3:24:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TransactionItem](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[VendingTransactionId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[SalePrice] [real] NOT NULL,
 CONSTRAINT [PK_TransactionItem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 12/7/2018 3:24:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[Username] [varchar](50) NOT NULL,
	[Email] [varchar](100) NOT NULL,
	[Hash] [varchar](50) NOT NULL,
	[RoleId] [int] NOT NULL,
	[Salt] [varchar](50) NOT NULL,
 CONSTRAINT [PK_VendUser] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VendingTransaction]    Script Date: 12/7/2018 3:24:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VendingTransaction](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NOT NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK_VendingTransaction] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Inventory] ADD  CONSTRAINT [DF_Inventory_Qty]  DEFAULT ((0)) FOR [Qty]
GO
ALTER TABLE [dbo].[Inventory]  WITH CHECK ADD  CONSTRAINT [FK_Inventory_Product] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Inventory] CHECK CONSTRAINT [FK_Inventory_Product]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_Category] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Category] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_Category]
GO
ALTER TABLE [dbo].[TransactionItem]  WITH CHECK ADD  CONSTRAINT [FK_TransactionItem_Product] FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([Id])
GO
ALTER TABLE [dbo].[TransactionItem] CHECK CONSTRAINT [FK_TransactionItem_Product]
GO
ALTER TABLE [dbo].[TransactionItem]  WITH CHECK ADD  CONSTRAINT [FK_TransactionItem_VendingTransaction] FOREIGN KEY([VendingTransactionId])
REFERENCES [dbo].[VendingTransaction] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TransactionItem] CHECK CONSTRAINT [FK_TransactionItem_VendingTransaction]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Role] FOREIGN KEY([RoleId])
REFERENCES [dbo].[RoleItem] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Role]
GO
ALTER TABLE [dbo].[VendingTransaction]  WITH CHECK ADD  CONSTRAINT [FK_VendingTransaction_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[VendingTransaction] CHECK CONSTRAINT [FK_VendingTransaction_User]
GO