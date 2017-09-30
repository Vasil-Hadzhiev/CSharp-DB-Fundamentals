-- 01. Find names of all employees by first name

USE SoftUni

SELECT FirstName, LastName
  FROM Employees
 WHERE FirstName LIKE 'Sa%'

-- 02. Find names of all employees by last name

SELECT FirstName, LastName
  FROM Employees
 WHERE LastName LIKE '%ei%'

-- 03. Find first names of all employees

SELECT FirstName
  FROM Employees
 WHERE DepartmentID IN(3,10)
   AND (HireDate >= 1995 OR HireDate <= 2005)

-- 04. Find all employees except engineers

SELECT FirstName, LastName
  FROM Employees
 WHERE JobTitle NOT LIKE '%engineer%'

-- 05. Find towns with name length

SELECT Name
  FROM Towns
 WHERE LEN(Name) = 5 OR LEN(NAME) = 6
 ORDER BY Name

-- 06. Find towns starting with 

SELECT TownID, Name
  FROM Towns
 WHERE Name LIKE '[MKBE]%'
 ORDER BY Name

-- 07. Find towns not starting with

SELECT TownID, Name
  FROM Towns
 WHERE Name LIKE '[^RBD]%'
 ORDER BY Name
  
-- 08. Create view employees hired after 2000 year

CREATE VIEW V_EmployeesHiredAfter2000 AS
SELECT FirstName, LastName
  FROM Employees
 WHERE YEAR(HireDate) > 2000

SELECT * FROM V_EmployeesHiredAfter2000

-- 09. Length of last name

SELECT FirstName, LastName
  FROM Employees
 WHERE LEN(LastName) = 5

-- 10. Countries holding 'A' 3 or more times

USE Geography

SELECT CountryName AS [Country Name], IsoCode AS [ISO Code]
  FROM Countries
 WHERE LEN(CountryName) - LEN(REPLACE(CountryName, 'a', '')) >= 3
 ORDER BY IsoCode

-- 11. Mix of peak and river names

SELECT PeakName, RiverName, LOWER(LEFT(PeakName, LEN(PeakName) - 1) + RiverName) AS [Mix]
  FROM Peaks, Rivers
 WHERE RIGHT(PeakName, 1) = LEFT(RiverName, 1)
 ORDER BY Mix
 
-- 12. Games from 2011 and 2012 year

USE Diablo

SELECT TOP 50 Name, FORMAT(Start, 'yyyy-MM-dd') AS [Start]
  FROM Games
 WHERE YEAR(Start) BETWEEN 2011 AND 2012
 ORDER BY Start

-- 13. User email providers

SELECT Username, RIGHT(Email, LEN(Email) - CHARINDEX('@', Email)) AS [Email Provider]
  FROM Users
 ORDER BY [Email Provider],
          Username

-- 14. Get users with IP address like pattern

SELECT Username, IpAddress AS [IP Address]
  FROM Users
 WHERE IpAddress LIKE '___.1%.%.___'
 ORDER BY Username

-- 15. Show all games with duration and part of the day

SELECT Name as Game,
  CASE 
      WHEN DATEPART(HOUR, Start) BETWEEN 0 AND 11 THEN 'Morning'
      WHEN DATEPART(HOUR, Start) BETWEEN 12 AND 17 THEN 'Afternoon'
      WHEN DATEPART(HOUR, Start) BETWEEN 18 AND 23 THEN 'Evening'
  END AS [Part of the Day],
  CASE
      WHEN Duration <= 3 THEN 'Extra Short'
	  WHEN Duration BETWEEN 4 AND 6 THEN 'Short'
	  WHEN Duration > 6 THEN 'Long'
	  ELSE 'Extra Long'
  END AS [Duration]
  FROM Games	
 ORDER By Name,
          [Duration],
		  [Part of the Day]


-- 16. Orders table

SELECT ProductName, Orderdate,
DATEADD(DAY, 3, Orderdate) AS [Pay Due],
DATEADD(MONTH, 1, Orderdate) AS [Delivery Due]
  FROM Orders

-- 17. People table

CREATE TABLE People (
	Id INT PRIMARY KEY IDENTITY,
	Name NVARCHAR(50),
	Birthdate DATETIME
)
	
INSERT INTO People (Name,Birthdate) VALUES
('Victor', '2000-12-07 00:00:00.000'),
('Steven', '1992-09-10 00:00:00.000'),
('Stephen', '1910-09-19 00:00:00.000')

SELECT Name,
       DATEDIFF(YEAR, Birthdate, GETDATE()) AS [Age in Years],
	   DATEDIFF(MONTH, Birthdate, GETDATE()) AS [Age in Months],
	   DATEDIFF(DAY, Birthdate, GETDATE()) AS [Age in Days],
	   DATEDIFF(MINUTE, Birthdate, GETDATE()) AS [Age in Minutes]
  FROM People