CREATE VIEW v_UserWithCountries AS
SELECT CONCAT(FirstName, ' ', LastName) AS CustomerName,
	   Age, Gender, 
	   co.[Name] AS CountryName
FROM Customers AS c
JOIN Countries AS co ON c.CountryId = co.Id