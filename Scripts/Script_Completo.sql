-- BY LUIS GRITZ / RICARDO BANISKI

USE [master]
GO
/****** Object:  Database [covid]    Script Date: 06/05/2020 12:14:04 ******/
CREATE DATABASE [covid]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'covid', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\covid.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'covid_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\covid_log.ldf' , SIZE = 270336KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [covid] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [covid].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [covid] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [covid] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [covid] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [covid] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [covid] SET ARITHABORT OFF 
GO
ALTER DATABASE [covid] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [covid] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [covid] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [covid] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [covid] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [covid] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [covid] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [covid] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [covid] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [covid] SET  DISABLE_BROKER 
GO
ALTER DATABASE [covid] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [covid] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [covid] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [covid] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [covid] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [covid] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [covid] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [covid] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [covid] SET  MULTI_USER 
GO
ALTER DATABASE [covid] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [covid] SET DB_CHAINING OFF 
GO
ALTER DATABASE [covid] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [covid] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [covid] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [covid] SET QUERY_STORE = OFF
GO
USE [covid]
GO
/****** Object:  Table [dbo].[Active]    Script Date: 06/05/2020 12:14:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Active](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[amount] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Archive]    Script Date: 06/05/2020 12:14:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Archive](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](80) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[City]    Script Date: 06/05/2020 12:14:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[City](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Confirmed]    Script Date: 06/05/2020 12:14:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Confirmed](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[amount] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CountryRegion]    Script Date: 06/05/2020 12:14:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CountryRegion](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Daily]    Script Date: 06/05/2020 12:14:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Daily](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CityId] [int] NOT NULL,
	[ProvinceStateId] [int] NOT NULL,
	[CountryRegionId] [int] NOT NULL,
	[LastUpdateId] [int] NOT NULL,
	[LatId] [int] NOT NULL,
	[LongId] [int] NOT NULL,
	[ConfirmedId] [int] NULL,
	[DeathsId] [int] NULL,
	[RecoveredId] [int] NULL,
	[ActiveId] [int] NULL,
	[ArchiveId] [int] NOT NULL,
	[RecordedId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Deaths]    Script Date: 06/05/2020 12:14:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Deaths](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[amount] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LastUpdate]    Script Date: 06/05/2020 12:14:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LastUpdate](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[dateUpdate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Lat]    Script Date: 06/05/2020 12:14:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Lat](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[coordinate] [decimal](12, 8) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Long]    Script Date: 06/05/2020 12:14:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Long](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[coordinate] [decimal](12, 8) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProvinceState]    Script Date: 06/05/2020 12:14:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProvinceState](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Recorded]    Script Date: 06/05/2020 12:14:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Recorded](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[recordedDt] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Recovered]    Script Date: 06/05/2020 12:14:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Recovered](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[amount] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Daily]  WITH CHECK ADD  CONSTRAINT [FK_Daily_Active] FOREIGN KEY([ActiveId])
REFERENCES [dbo].[Active] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Daily] CHECK CONSTRAINT [FK_Daily_Active]
GO
ALTER TABLE [dbo].[Daily]  WITH CHECK ADD  CONSTRAINT [FK_Daily_Archive] FOREIGN KEY([ArchiveId])
REFERENCES [dbo].[Archive] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Daily] CHECK CONSTRAINT [FK_Daily_Archive]
GO
ALTER TABLE [dbo].[Daily]  WITH CHECK ADD  CONSTRAINT [FK_Daily_City] FOREIGN KEY([CityId])
REFERENCES [dbo].[City] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Daily] CHECK CONSTRAINT [FK_Daily_City]
GO
ALTER TABLE [dbo].[Daily]  WITH CHECK ADD  CONSTRAINT [FK_Daily_Confirmed] FOREIGN KEY([ConfirmedId])
REFERENCES [dbo].[Confirmed] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Daily] CHECK CONSTRAINT [FK_Daily_Confirmed]
GO
ALTER TABLE [dbo].[Daily]  WITH CHECK ADD  CONSTRAINT [FK_Daily_CountryRegion] FOREIGN KEY([CountryRegionId])
REFERENCES [dbo].[CountryRegion] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Daily] CHECK CONSTRAINT [FK_Daily_CountryRegion]
GO
ALTER TABLE [dbo].[Daily]  WITH CHECK ADD  CONSTRAINT [FK_Daily_Deaths] FOREIGN KEY([DeathsId])
REFERENCES [dbo].[Deaths] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Daily] CHECK CONSTRAINT [FK_Daily_Deaths]
GO
ALTER TABLE [dbo].[Daily]  WITH CHECK ADD  CONSTRAINT [FK_Daily_LastUpdate] FOREIGN KEY([LastUpdateId])
REFERENCES [dbo].[LastUpdate] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Daily] CHECK CONSTRAINT [FK_Daily_LastUpdate]
GO
ALTER TABLE [dbo].[Daily]  WITH CHECK ADD  CONSTRAINT [FK_Daily_Lat] FOREIGN KEY([LatId])
REFERENCES [dbo].[Lat] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Daily] CHECK CONSTRAINT [FK_Daily_Lat]
GO
ALTER TABLE [dbo].[Daily]  WITH CHECK ADD  CONSTRAINT [FK_Daily_Long] FOREIGN KEY([LongId])
REFERENCES [dbo].[Long] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Daily] CHECK CONSTRAINT [FK_Daily_Long]
GO
ALTER TABLE [dbo].[Daily]  WITH CHECK ADD  CONSTRAINT [FK_Daily_ProvinceState] FOREIGN KEY([ProvinceStateId])
REFERENCES [dbo].[ProvinceState] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Daily] CHECK CONSTRAINT [FK_Daily_ProvinceState]
GO
ALTER TABLE [dbo].[Daily]  WITH CHECK ADD  CONSTRAINT [FK_Daily_Recorded] FOREIGN KEY([RecordedId])
REFERENCES [dbo].[Recorded] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Daily] CHECK CONSTRAINT [FK_Daily_Recorded]
GO
ALTER TABLE [dbo].[Daily]  WITH CHECK ADD  CONSTRAINT [FK_Daily_Recovered] FOREIGN KEY([RecoveredId])
REFERENCES [dbo].[Recovered] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Daily] CHECK CONSTRAINT [FK_Daily_Recovered]
GO
/****** Object:  StoredProcedure [dbo].[spInsereDaily]    Script Date: 06/05/2020 12:14:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spInsereDaily]
	@City VARCHAR(50) NULL,
	@ProvinceState VARCHAR(50) NULL,
	@CountryRegion VARCHAR(50) NULL,
	@LastUpdate DATETIME NULL,
	@Lat VARCHAR(15) NULL,
	@Long VARCHAR(15) NULL,
	@Confirmed INT NULL,
	@Deaths INT NULL,
	@Recovered INT NULL,
	@Active INT NULL,
	@Archive VARCHAR(80) NULL
AS
BEGIN
DECLARE 
	@Recorded DATETIME = GETDATE(),
	@CityId INT = 0,
	@ProvinceStateId INT = 0,
	@CountryRegionId INT = 0,
	@LastUpdateId INT = 0,
	@ArchiveId INT = 0,
	@LatId INT = 0,
	@LongId INT = 0,
	@ConfirmedId INT = 0,
	@DeathsId INT = 0,
	@RecoveredId INT = 0,
	@ActiveId INT = 0,
	@RecordedId INT = 0

	IF (SELECT COUNT(*) FROM covid..City WHERE [name] = @City) < 1
		INSERT INTO City([name]) VALUES(@City)
	SET @CityId = (SELECT DISTINCT id FROM covid..City WHERE [name] = @City)

	IF (SELECT COUNT(*) FROM covid..ProvinceState WHERE [name] = @ProvinceState) < 1
		INSERT INTO covid..ProvinceState([name]) VALUES(@ProvinceState)
	SET @ProvinceStateId = (SELECT DISTINCT id FROM covid..ProvinceState WHERE [name] = @ProvinceState)

	IF (SELECT COUNT(*) FROM covid..CountryRegion WHERE [name] = @CountryRegion) < 1
		INSERT INTO covid..CountryRegion([name]) VALUES(@CountryRegion)
	SET @CountryRegionId = (SELECT DISTINCT id FROM covid..CountryRegion WHERE [name] = @CountryRegion)

	IF (SELECT COUNT(*) FROM covid..LastUpdate WHERE dateUpdate = @LastUpdate) < 1
		INSERT INTO covid..LastUpdate(dateUpdate) VALUES(@LastUpdate)
	SET @LastUpdateId = (SELECT id FROM covid..LastUpdate WHERE dateUpdate = @LastUpdate)

	IF (SELECT COUNT(*) FROM covid..Lat WHERE coordinate = CONVERT(decimal(12,8), @Lat)) < 1
		INSERT INTO covid..Lat(coordinate) VALUES(CONVERT(decimal(12,8), @Lat))
	SET @LatId = (SELECT id FROM covid..Lat WHERE coordinate = (CONVERT(decimal(12,8), @Lat)))

	IF (SELECT COUNT(*) FROM covid..Long WHERE coordinate = CONVERT(decimal(12,8), @Long)) < 1
		INSERT INTO covid..Long(coordinate) VALUES(CONVERT(decimal(12,8), @Long))
	SET @LongId = (SELECT id FROM covid..Long WHERE coordinate = (CONVERT(decimal(12,8), @Long)))

	IF (SELECT COUNT(*) FROM covid..Confirmed WHERE amount = @Confirmed) < 1
		INSERT INTO covid..Confirmed(amount) VALUES(@Confirmed)
	SET @ConfirmedId = (SELECT id FROM covid..Confirmed WHERE amount = @Confirmed)

	IF (SELECT COUNT(*) FROM covid..Deaths WHERE amount = @Deaths) < 1
		INSERT INTO covid..Deaths(amount) VALUES(@Deaths)
	SET @DeathsId = (SELECT id FROM covid..Deaths WHERE amount = @Deaths)

	IF (SELECT COUNT(*) FROM covid..Recovered WHERE amount = @Recovered) < 1
		INSERT INTO covid..Recovered(amount) VALUES(@Recovered)
	SET @RecoveredId = (SELECT id FROM covid..Recovered WHERE amount = @Recovered)

	IF (SELECT COUNT(*) FROM covid..Active WHERE amount = @Active) < 1
		INSERT INTO covid..Active(amount) VALUES(@Active)
	SET @ActiveId = (SELECT id FROM covid..Active WHERE amount = @Active)

	IF (SELECT COUNT(*) FROM covid..Archive WHERE [name] = @Archive) < 1
		INSERT INTO covid..Archive([name]) VALUES(@Archive)
	SET @ArchiveId = (SELECT DISTINCT id FROM covid..Archive WHERE [name] = @Archive)

	IF (SELECT COUNT(*) FROM covid..Recorded WHERE [recordedDt] = @Recorded) < 1
		INSERT INTO covid..Recorded([recordedDt]) VALUES(@Recorded)
	SET @RecordedId = (SELECT DISTINCT id FROM covid..Recorded WHERE [recordedDt] = @Recorded)
END

BEGIN
	INSERT INTO covid..daily (CityId, ProvinceStateId, CountryRegionId, LastUpdateId, LatId, LongId, ConfirmedId, DeathsId, RecoveredId, ActiveId, ArchiveId, RecordedId) 
	VALUES(@CityId, @ProvinceStateId, @CountryRegionId, @LastUpdateId, @LatId, @LongId, @ConfirmedId, @DeathsId, @RecoveredId, @ActiveId, @ArchiveId, @RecordedId)
END
GO
USE [master]
GO
ALTER DATABASE [covid] SET  READ_WRITE 
GO
