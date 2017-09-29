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

