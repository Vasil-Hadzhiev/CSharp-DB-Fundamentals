SELECT c.FirstName + ' ' + c.LastName AS [Client],
	   DATEDIFF(DAY, j.IssueDate, '04-24-2017') AS [Days going],
	   j.Status
FROM Clients AS c
INNER JOIN Jobs AS j ON j.ClientId = c.ClientId
WHERE j.Status <> 'Finished'
ORDER BY [Days going] DESC,
         j.ClientId