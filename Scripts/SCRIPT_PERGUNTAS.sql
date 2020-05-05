/* PERGUNTAS 

--01 - QUAL A POSIÇÃO NO BRASIL EM 30/04/2020?
SELECT D.name [PAÍS], E.dateUpdate [ATUALIZAÇÃO], H.amount [TOTAL DE CASOS], I.amount [MORTES], J.amount [RECUPERADOS], K.amount [ATIVOS], L.name [ARQUIVO], M.recordedDt [PROCESSAMENTO] FROM Daily AS A
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
SELECT D.name [PAÍS], /*E.dateUpdate [ATUALIZAÇÃO], H.amount [TOTAL DE CASOS],*/ (MAX(I.amount) - MIN(I.amount)) [MORTES]/*, J.amount [RECUPERADOS], K.amount [ATIVOS], L.name [ARQUIVO], M.recordedDt [PROCESSAMENTO]*/ FROM Daily AS A
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

SELECT TOP 1 D.name [PAÍS], SUM(H.amount) [TOTAL] FROM Daily AS A
	INNER JOIN CountryRegion AS D
		ON D.id = A.CountryRegionId
	INNER JOIN LastUpdate AS E
		ON E.id = A.LastUpdateId
	INNER JOIN Confirmed AS H
		ON H.id = A.ConfirmedId
WHERE E.dateUpdate = (SELECT MAX(dateUpdate) FROM LastUpdate)
GROUP BY D.name
ORDER BY TOTAL

*/
-- 04