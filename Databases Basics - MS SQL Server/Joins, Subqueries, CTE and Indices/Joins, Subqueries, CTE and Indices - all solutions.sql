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






