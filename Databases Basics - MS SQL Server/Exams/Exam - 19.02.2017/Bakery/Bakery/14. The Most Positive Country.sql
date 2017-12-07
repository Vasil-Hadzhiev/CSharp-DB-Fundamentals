SELECT TOP(1) WITH TIES
	   c.[Name] AS CountryName, AVG(f.Rate) AS FeedbackRate
FROM Countries AS c
JOIN Customers AS cc ON cc.CountryId = c.Id
JOIN Feedbacks AS f ON f.CustomerId = cc.Id 
GROUP BY c.[Name]
ORDER BY FeedbackRate DESC