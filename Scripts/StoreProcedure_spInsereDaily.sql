USE [covid]
GO
/****** Object:  StoredProcedure [dbo].[spInsereDaily]    Script Date: 04/05/2020 18:37:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[spInsereDaily]
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

	IF (SELECT COUNT(*) FROM covid..Lat WHERE coordinate = @Lat) < 1
		INSERT INTO covid..Lat(coordinate) VALUES(@Lat)
	SET @LatId = (SELECT id FROM covid..Lat WHERE coordinate = @Lat)

	IF (SELECT COUNT(*) FROM covid..Long WHERE coordinate = @Long) < 1
		INSERT INTO covid..Long(coordinate) VALUES(@Long)
	SET @LongId = (SELECT id FROM covid..Long WHERE coordinate = @Long)

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