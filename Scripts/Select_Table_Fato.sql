SELECT B.name [CIDADE], C.name [ESTADO], D.name [PAÍS], E.dateUpdate [ATUALIZAÇÃO], F.coordinate [LATITUDE], G.coordinate [LONGITUDE], H.amount [TOTAL], I.amount [MORTES], J.amount [RECUPERADOS], K.amount [ATIVOS], L.name [ARQUIVO], M.recordedDt [GRAVAÇÃO] 
FROM Daily AS A
	INNER JOIN CITY AS B
		ON B.id = A.CityId
	INNER JOIN ProvinceState AS C
		ON C.id = A.ProvinceStateId
	INNER JOIN CountryRegion AS D
		ON D.id = A.CountryRegionId
	INNER JOIN LastUpdate AS E
		ON E.id = A.LastUpdateId
	INNER JOIN Lat AS F
		ON F.id = A.LatId
	INNER JOIN Long AS G
		ON G.id = A.LongId
	INNER JOIN Confirmed AS H
		ON H.id = A.ConfirmedId
	INNER JOIN Deaths AS I
		ON I.id = A.DeathsId
	INNER JOIN Recovered AS J
		ON J.id = A.RecoveredId
	INNER JOIN Active AS K
		ON K.id = A.ActiveId
	INNER JOIN Archive AS L
		ON L.id = A.ArchiveId
	INNER JOIN Recorded AS M
		ON M.id = A.RecordedId
ORDER BY ATUALIZAÇÃO