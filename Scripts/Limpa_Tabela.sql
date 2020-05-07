USE [covid]
GO

DELETE FROM [dbo].[City]
GO
DBCC CHECKIDENT('[City]', RESEED, 0)

DELETE FROM [dbo].[ProvinceState]
GO
DBCC CHECKIDENT('[ProvinceState]', RESEED, 0)

DELETE FROM [dbo].[CountryRegion]
GO
DBCC CHECKIDENT('[CountryRegion]', RESEED, 0)

DELETE FROM [dbo].[LastUpdate]
GO
DBCC CHECKIDENT('[LastUpdate]', RESEED, 0)

DELETE FROM [dbo].[Lat]
GO
DBCC CHECKIDENT('[Lat]', RESEED, 0)

DELETE FROM [dbo].[Long]
GO
DBCC CHECKIDENT('[Long]', RESEED, 0)

DELETE FROM [dbo].[Confirmed]
GO
DBCC CHECKIDENT('[Confirmed]', RESEED, 0)

DELETE FROM [dbo].[Deaths]
GO
DBCC CHECKIDENT('[Deaths]', RESEED, 0)

DELETE FROM [dbo].[Recovered]
GO
DBCC CHECKIDENT('[Recovered]', RESEED, 0)

DELETE FROM [dbo].[Active]
GO
DBCC CHECKIDENT('[Active]', RESEED, 0)

DELETE FROM [dbo].[Recorded]
GO
DBCC CHECKIDENT('[Recorded]', RESEED, 0)

DELETE FROM [dbo].[Archive]
GO
DBCC CHECKIDENT('[Archive]', RESEED, 0)

DELETE FROM [dbo].[Daily]
GO
DBCC CHECKIDENT('[Daily]', RESEED, 0)