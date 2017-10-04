-- 01. Records count

SELECT COUNT(*) FROM WizzardDeposits

-- 02. Longest magic wand

SELECT MAX(MagicWandSize) FROM WizzardDeposits

-- 03. Longest magic wand per deposit groups

SELECT DepositGroup, 
       MAX(MagicWandSize) AS [LongestMagicWand]
  FROM WizzardDeposits
 GROUP BY DepositGroup

-- 04. Smallest deposit group per magic wand size

SELECT TOP 2 DepositGroup 
  FROM WizzardDeposits
 GROUP BY DepositGroup
 ORDER BY AVG(MagicWandSize)

-- 05. Deposits sum

SELECT DepositGroup, 
       SUM(DepositAmount) AS TotalSum
  FROM WizzardDeposits
 GROUP BY DepositGroup

-- 06. Deposits sum for Ollivander family

SELECT DepositGroup, 
       SUM(DepositAmount) AS TotalSum
  FROM WizzardDeposits
 WHERE MagicWandCreator = 'Ollivander family'
 GROUP BY DepositGroup

-- 07. Deposits filter

SELECT DepositGroup, 
       SUM(DepositAmount) AS [TotalSum]
  FROM WizzardDeposits
 WHERE MagicWandCreator = 'Ollivander family'
 GROUP BY DepositGroup
HAVING SUM(DepositAmount) < 150000
 ORDER BY [TotalSum] DESC

-- 08. Deposit charge

SELECT DepositGroup, 
       MagicWandCreator, 
	   MIN(DepositCharge) AS [MinDepositCharge]
  FROM WizzardDeposits
 GROUP BY DepositGroup, 
          MagicWandCreator
 ORDER BY MagicWandCreator,
          DepositGroup

-- 09. Age groups

SELECT w.AgeGroup, COUNT(*) AS [WizardCount]
  FROM (
SELECT 
  CASE
      WHEN Age BETWEEN 0 AND 10 THEN '[0-10]'
      WHEN Age BETWEEN 11 AND 20 THEN '[11-20]'
      WHEN Age BETWEEN 21 AND 30 THEN '[21-30]'
      WHEN Age BETWEEN 31 AND 40 THEN '[31-40]'
      WHEN Age BETWEEN 41 AND 50 THEN '[41-50]'
      WHEN Age BETWEEN 51 AND 60 THEN '[51-60]'
      WHEN Age >= 61 THEN '[61+]'
   END AS [AgeGroup]
  FROM WizzardDeposits
       ) AS w
GROUP BY w.AgeGroup

-- 10. First letter

SELECT DISTINCT LEFT(FirstName, 1) AS [FirstLetter]
  FROM WizzardDeposits
 WHERE DepositGroup = 'Troll Chest'
 ORDER BY FirstLetter

-- 11. Average interest

SELECT * FROM WizzardDeposits

SELECT DepositGroup, 
       IsDepositExpired, 
	   AVG(DepositInterest) AS [AverageInterest]
  FROM WizzardDeposits
 WHERE DepositStartDate > '01-01-1985'
 GROUP BY DepositGroup, 
          IsDepositExpired
 ORDER BY DepositGroup DESC,
          IsDepositExpired

-- 12. Rich wizzard, poor wizzard

-- 13. Departments total salaries

USE SoftUni

SELECT DepartmentID, 
       SUM(Salary) AS[TotalSalary]
  FROM Employees
 GROUP BY DepartmentID
 ORDER BY DepartmentID

-- 14. Employees minimum salaries

SELECT DepartmentID, 
       MIN(Salary) AS [MinimumSalary]
  FROM Employees
 WHERE DepartmentID IN(2, 5, 7) AND
       HireDate > '01-01-2000'
 GROUP BY DepartmentID
 
-- 15. Employees average salaries

SELECT * 
  INTO AverageEmployees
  FROM Employees
 WHERE Salary > 30000

DELETE 
  FROM AverageEmployees
 WHERE ManagerID = 42

UPDATE AverageEmployees
   SET Salary += 5000
 WHERE DepartmentID = 1

SELECT DepartmentID, 
       AVG(Salary) AS [AverageSalary]
  FROM AverageEmployees
 GROUP BY DepartmentID

-- 16. Employees maximum salaries

SELECT DepartmentID, 
       MAX(Salary) AS [MaxSalary]
  FROM Employees
 GROUP BY DepartmentID
HAVING MAX(Salary) NOT BETWEEN 30000 AND 70000

-- 17. Employees count salaries

SELECT COUNT(Salary) AS [Count]
  FROM Employees
 WHERE ManagerID IS NULL

-- 18. Third highest salary




