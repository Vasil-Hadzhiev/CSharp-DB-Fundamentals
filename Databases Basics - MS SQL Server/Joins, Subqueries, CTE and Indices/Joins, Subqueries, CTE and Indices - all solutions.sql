-- 01. Employee Address

SELECT TOP 5 emp.EmployeeID, emp.JobTitle, emp.AddressID, adr.AddressText
FROM Employees AS emp
INNER JOIN Addresses AS adr ON adr.AddressID = emp.AddressID
ORDER BY emp.AddressID

-- 02. Addresses with Towns

SELECT TOP 50 e.FirstName, e.LastName, t.Name, a.AddressText 
FROM Employees AS e
INNER JOIN Addresses AS a ON a.AddressID = e.AddressID
INNER JOIN Towns AS t ON t.TownID = a.TownID
ORDER BY FirstName,
          LastName

-- 03. Sales employee

SELECT e.EmployeeID, e.FirstName, e.LastName, d.Name AS [DepartmentName]
FROM Employees AS e
INNER JOIN Departments AS d ON d.DepartmentID = e.DepartmentID
WHERE d.Name = 'Sales'
ORDER BY EmployeeID

-- 04. Employee departments

SELECT TOP 5 e.EmployeeID, e.FirstName, e.Salary, d.Name AS [DepartmentName]
FROM Employees AS e
INNER JOIN Departments AS d ON d.DepartmentID = e.DepartmentID
WHERE e.Salary > 15000
ORDER BY e.DepartmentID

-- 05. Employees without project

SELECT TOP 3 e.EmployeeID, e.FirstName
FROM Employees AS e
LEFT JOIN EmployeesProjects AS p ON e.EmployeeID = p.EmployeeID
WHERE p.EmployeeID IS NULL
ORDER BY e.EmployeeID

-- 06. Employees hired after

SELECT e.FirstName, e.LastName, e.HireDate, d.Name AS [DeptName]
FROM Employees AS e
INNER JOIN Departments AS d ON d.DepartmentID = e.DepartmentID
WHERE e.HireDate > '1-1-1999' AND d.Name IN('Sales', 'Finance')
ORDER BY e.HireDate 

-- 07. Employees with project

SELECT TOP 5 e.EmployeeID, e.FirstName, p.Name AS [ProjectName]
FROM Employees AS e
LEFT JOIN EmployeesProjects AS ep ON ep.EmployeeID = e.EmployeeID
LEFT JOIN Projects AS p ON p.ProjectID = ep.ProjectID
WHERE p.Name IS NOT NULL AND p.StartDate > '08-13-2002' AND p.EndDate IS NULL
ORDER BY  EmployeeID 

-- 08. Employee 24


SELECT * FROM Employees
SELECT * FROM EmployeesProjects
SELECT * FROM Projects

SELECT e.EmployeeID, e.FirstName, 
  CASE
      WHEN p.StartDate > '01-01-2005' THEN NULL
	  ELSE p.Name
   END AS [ProjectName]
FROM Employees AS e
LEFT JOIN EmployeesProjects AS ep ON ep.EmployeeID = e.EmployeeID
LEFT JOIN Projects AS p ON p.ProjectID = ep.ProjectID
WHERE e.EmployeeID = 24

-- 09. Employee Manager

SELECT e.EmployeeID, e.FirstName, e.ManagerID, e2.FirstName AS [ManagerName]
FROM Employees AS e
INNER JOIN Employees AS e2 ON e2.EmployeeID = e.ManagerID
WHERE e.ManagerID IN(3,7)
ORDER BY EmployeeID

-- 10. Employee summary

SELECT TOP 50
       e.EmployeeID,
       e.FirstName + ' ' + e.LastName AS [EmployeeName],
	   e2.FirstName + ' ' + e2.LastName AS [ManagerName],
	   d.Name AS [DepartmentName]
FROM Employees AS e
INNER JOIN Employees AS e2 ON e2.EmployeeID = e.ManagerID
INNER JOIN Departments AS d ON d.DepartmentID = e.DepartmentID
ORDER BY e.EmployeeID


-- 11. Min average salary

SELECT MIN(m.Salary) AS [MinAverageSalary]
FROM (
SELECT AVG(e.Salary) AS [Salary]
FROM Employees AS e
GROUP BY e.DepartmentID
) AS m

-- 12. Highest peaks in bulgaria

SELECT mc.CountryCode, MountainRange, PeakName, Elevation
FROM MountainsCountries AS mc
INNER JOIN Mountains AS m ON m.Id = mc.MountainId
INNER JOIN Peaks AS p ON p.MountainId = mc.MountainId
WHERE mc.CountryCode = 'BG' AND p.Elevation > 2835
ORDER BY p.Elevation DESC

-- 13. Count mountain ranges

SELECT mc.CountryCode, COUNT(m.MountainRange) AS [MountainRanges]
FROM MountainsCountries AS mc
INNER JOIN Mountains AS m ON m.Id = mc.MountainId
WHERE mc.CountryCode IN('US', 'RU', 'BG')
GROUP BY mc.CountryCode

-- 14. Countries with rivers

SELECT TOP 5 c.CountryName, RiverName
FROM Countries AS c
LEFT JOIN CountriesRivers AS cr ON cr.CountryCode = c.CountryCode
LEFT JOIN Rivers AS r ON r.Id = cr.RiverId
INNER JOIN Continents AS con ON con.ContinentCode = c.ContinentCode
WHERE con.ContinentCode = 
(
SELECT ContinentCode 
FROM Continents
WHERE ContinentName = 'Africa'
)
ORDER BY c.CountryName

-- 15. Continents and currencies

SELECT ContinentCode, CurrencyCode, CurrencyUsage
FROM (
	SELECT ContinentCode, CurrencyCode, CurrencyUsage,
	DENSE_RANK() OVER(PARTITION BY(ContinentCode) 
	ORDER BY CurrencyUsage DESC) AS [Rank]
		FROM (
			SELECT ContinentCode, CurrencyCode, COUNT(CurrencyCode) 
			AS [CurrencyUsage]
			FROM Countries
			GROUP BY CurrencyCode, ContinentCode) 
			AS Currencies) 
			AS RankedCurrencies
WHERE [Rank] = 1 AND CurrencyUsage > 1
ORDER BY ContinentCode

-- 16. Countries without mountains

SELECT COUNT(CountryCode) AS [CountryCode]
FROM Countries
WHERE CountryCode NOT IN (SELECT CountryCode FROM MountainsCountries)

-- 17. Highest peak and longest river by country

SELECT TOP 5 
		c.CountryName,
		MAX(p.Elevation) AS [HighestPeakElevation],
		MAX(r.Length) AS [LongestRiverLength]
FROM Countries AS c
LEFT JOIN MountainsCountries AS mc ON mc.CountryCode = c.CountryCode
LEFT JOIN Peaks AS p ON p.MountainId = mc.MountainId
LEFT JOIN CountriesRivers AS cr ON cr.CountryCode = c.CountryCode
LEFT JOIN Rivers AS r ON r.Id = cr.RiverId
GROUP BY CountryName
ORDER BY MAX(p.Elevation) DESC, 
         MAX(r.Length) DESC, 
		 c.CountryName ASC

-- 18. Highest peak name and elevation by country

SELECT TOP 5
       CountryName, 
       CASE 
	       WHEN PeakName IS NULL THEN ('no highest peak')
		   ELSE PeakName
	   END AS [Highest Peak Name], 
	   CASE 
	       WHEN Elevation IS NULL THEN 0
		   ELSE Elevation
	   END AS [Highest Peak Elevation], 
	   CASE 
	       WHEN MountainRange IS NULL THEN ('no mountain')
		   ELSE MountainRange
	   END AS [Mountain]
FROM (
	SELECT CountryName, PeakName, Elevation, MountainRange,
	DENSE_RANK() OVER(PARTITION BY CountryName ORDER BY Elevation DESC) AS [Rank]
	FROM ( 
		SELECT c.CountryName,
			   p.PeakName,
			   p.Elevation,
			   m.MountainRange
		FROM Countries AS c
		LEFT JOIN MountainsCountries AS mc ON mc.CountryCode = c.CountryCode
		LEFT JOIN Mountains AS m ON m.Id = mc.MountainId
		LEFT JOIN Peaks AS p ON p.MountainId = m.Id) AS TopPeaks) AS RankedPeaks
		WHERE [Rank] = 1
		ORDER BY CountryName,
		         [Highest Peak Name]
