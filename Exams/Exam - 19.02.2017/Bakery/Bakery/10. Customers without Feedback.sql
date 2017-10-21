-- With subquery
SELECT CONCAT(FirstName, ' ', LastName) AS [CustomerName],
       PhoneNumber,
	   Gender
FROM Customers
WHERE Id NOT IN (SELECT CustomerId FROM Feedbacks)


-- With outer join
SELECT CONCAT(c.FirstName, ' ', c.LastName) AS [Name], 
       c.PhoneNumber, 
	   c.Gender
FROM Customers AS c
LEFT OUTER JOIN Feedbacks AS f ON c.Id = f.CustomerId
WHERE f.Id IS NULL