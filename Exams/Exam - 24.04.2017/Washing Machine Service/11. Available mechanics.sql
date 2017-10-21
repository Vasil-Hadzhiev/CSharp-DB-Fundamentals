SELECT m.FirstName + ' ' + m.LastName AS [Mechanic]
FROM Mechanics AS m
LEFT JOIN Jobs AS j ON j.MechanicId = m.MechanicId
WHERE m.MechanicId NOT IN
(
	SELECT MechanicId 
	FROM Jobs
	WHERE MechanicId IS NOT NULL AND Status <> 'Finished'
)
GROUP BY m.FirstName, m.LastName, m.MechanicId	
ORDER BY m.MechanicId
