/* PERGUNTAS */ 
-- BY LUIS GRITZ / RICARDO BANISKI

--TEMPO DE PROCESSAMENTO
SELECT CAST((MAX(recordedDt))-(MIN(recordedDt)) AS TIME(0)) [TEMPO PROCESSAMENTO] FROM Recorded

--01 - QUAL A POSIÇÃO NO BRASIL EM 30/04/2020?
SELECT D.name [PAÍS], (CONVERT(VARCHAR, E.dateUpdate, 103) + ' ' + CONVERT(VARCHAR, E.dateUpdate, 8)) [ATUALIZAÇÃO], H.amount [TOTAL DE CASOS], I.amount [MORTES], J.amount [RECUPERADOS], K.amount [ATIVOS], L.name [ARQUIVO], (CONVERT(VARCHAR, M.recordedDt, 103) + ' ' + CONVERT(VARCHAR, M.recordedDt, 8)) [PROCESSAMENTO DO ARQUIVO] FROM Daily AS A
	INNER JOIN CountryRegion AS D
		ON D.id = A.CountryRegionId
	INNER JOIN LastUpdate AS E
		ON E.id = A.LastUpdateId
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
WHERE E.dateUpdate BETWEEN '30-04-2020' AND '01-05/2020'
	AND D.name = 'BRAZIL'


--02 - QUAL O NÚMERO DE MORTES ENTRE 15/04 E 30/04 NO BRASIL?
SELECT D.name [PAÍS],(MAX(I.amount) - MIN(I.amount)) [MORTES NO PERÍODO] FROM Daily AS A
	INNER JOIN CountryRegion AS D
		ON D.id = A.CountryRegionId
	INNER JOIN LastUpdate AS E
		ON E.id = A.LastUpdateId
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
WHERE D.name = 'BRAZIL'
	AND E.dateUpdate BETWEEN '15-04-2020' AND '01-05/2020'
GROUP BY D.name


--03 - QUAL O PAÍS COM MENOR NÚMERO DE CASOS?
SELECT TOP 1 D.name [PAÍS], SUM(H.amount) [TOTAL DE CASOS] FROM Daily AS A
	INNER JOIN CountryRegion AS D
		ON D.id = A.CountryRegionId
	INNER JOIN LastUpdate AS E
		ON E.id = A.LastUpdateId
	INNER JOIN Confirmed AS H
		ON H.id = A.ConfirmedId
WHERE E.dateUpdate = (SELECT MAX(dateUpdate) FROM LastUpdate)
GROUP BY D.name
ORDER BY [TOTAL DE CASOS]


-- 04 - QUAL O PERCENTUAL DE AUMENTO DE CASOS NO BRASIL NOS ÚLTIMOS 30 DIAS?
SELECT F.name [PAÍS],
	CONVERT(DECIMAL(10,2),
		(
			CONVERT(DECIMAL(10,2),(SELECT TOP 1 D.amount
			FROM Daily AS A
			INNER JOIN CountryRegion AS B
				ON B.id = A.CountryRegionId
			INNER JOIN LastUpdate AS C
				ON C.id = A.LastUpdateId
			INNER JOIN Confirmed AS D
				ON D.id = A.ConfirmedId
			WHERE CountryRegionId = 14
				AND C.dateUpdate > (SELECT TOP 1 dateUpdate FROM LastUpdate ORDER BY dateUpdate DESC)-30
			ORDER BY D.amount DESC))
			/
			CONVERT(DECIMAL(10,2),(SELECT TOP 1 D.amount 
			FROM Daily AS A
			INNER JOIN CountryRegion AS B
				ON B.id = A.CountryRegionId
			INNER JOIN LastUpdate AS C
				ON C.id = A.LastUpdateId
			INNER JOIN Confirmed AS D
				ON D.id = A.ConfirmedId
			WHERE CountryRegionId = 14
				AND C.dateUpdate > (SELECT TOP 1 dateUpdate FROM LastUpdate ORDER BY dateUpdate DESC)-30
			ORDER BY D.amount))
		)
	) * 100 [% AUMENTO DE CASOS]
FROM Daily AS E
	INNER JOIN CountryRegion AS F
		ON F.id = E.CountryRegionId
WHERE F.name = 'brazil'
GROUP BY F.name


-- 05 - QUAL O ÚLTIMO NUMERO DE RECUPERADOS NA AUTRÁLIA HOJE?
SELECT D.name [PAÍS], SUM(J.amount) [RECUPERADOS] FROM Daily AS A
	INNER JOIN CountryRegion AS D
		ON D.id = A.CountryRegionId
	INNER JOIN LastUpdate AS E
		ON E.id = A.LastUpdateId
	INNER JOIN Recovered AS J
		ON J.id = A.RecoveredId
WHERE E.dateUpdate = (SELECT TOP 1 dateUpdate FROM LastUpdate ORDER BY dateUpdate DESC)
	AND d.name = 'Australia'
GROUP BY D.name


-- 6 - QUAIS OS 3 ESTADOS DOS ESTADOS UNIDOS COM MAIOR NÚMERO DE ATIVOS ATUALMENTE
SELECT TOP 3 E.name [PAÍS], B.name [ESTADO], SUM(C.amount)[ATIVOS] FROM Daily AS A
	INNER JOIN ProvinceState AS B
	ON B.id = A.ProvinceStateId
	INNER JOIN Active AS C
	ON C.id = A.ActiveId
	INNER JOIN LastUpdate AS D
	ON D.id = A.LastUpdateId
	INNER JOIN CountryRegion AS E
	ON E.id = A.CountryRegionId
WHERE D.dateUpdate = (SELECT TOP 1 dateUpdate FROM LastUpdate ORDER BY dateUpdate DESC)
	AND E.name = 'UNITED STATES'
GROUP BY E.name, B.name
ORDER BY ATIVOS DESC


--07 QUAL O PAÍS INFECTADO LOCALIZADO ENTRE AS COORDENADAS 60 , 8 ?
SELECT D.name [PAIS], B.coordinate [LATITUDE], C.coordinate [LONGITUDE] FROM Daily AS A
	INNER JOIN Lat AS B
	ON B.id = A.LatId
	INNER JOIN Long AS C
	ON C.id = A.LongId
	INNER JOIN CountryRegion AS D
	ON D.id = A.CountryRegionId
WHERE B.coordinate LIKE '60%' AND C.coordinate LIKE '8%'
GROUP BY D.name, B.coordinate, C.coordinate


--08 QUAIS AS CIDADES DO CANADÁ COM REGISTRO DE CASOS DO COVID-19?
SELECT B.name [PAÍS], C.name [CIDADE]  FROM Daily AS A
	INNER JOIN CountryRegion AS B
	ON B.id = A.CountryRegionId
	INNER JOIN City AS C
	ON C.id = A.CityId
WHERE B.name = 'Canada' and C.name <> ''
GROUP BY B.name, C.name


--09 EM QUE DATA A FRANÇA ULTRAPASSOU A FAIXA DOS 1000 CASOS?
SELECT TOP 1 B.name [PAÍS], D.amount [CASOS], CONVERT(VARCHAR, C.dateUpdate, 103) [DATA] FROM Daily AS A
	INNER JOIN CountryRegion AS B
	ON B.id = A.CountryRegionId
	INNER JOIN LastUpdate AS C
	ON C.id = A.LastUpdateId
	INNER JOIN Confirmed AS D
	ON D.amount = A.ConfirmedId
WHERE B.name = 'France'
	AND D.amount > 1000


--10 QUAL A DATA DA PRIMEIRA MORTE NO BRAZIL
SELECT TOP 1 B.name [PAÍS], D.amount [MORTE], CONVERT(VARCHAR, C.dateUpdate, 103) [DATA] FROM Daily AS A
	INNER JOIN CountryRegion AS B
	ON B.id = A.CountryRegionId
	INNER JOIN LastUpdate AS C
	ON C.id = A.LastUpdateId
	INNER JOIN Deaths AS D
	ON D.amount = A.DeathsId
WHERE B.name = 'BRAZIL'