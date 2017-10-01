-- 01. Records count

SELECT COUNT(*) FROM WizzardDeposits

-- 02. Longest magic wand

SELECT MAX(MagicWandSize) FROM WizzardDeposits

-- 03. Longest magic wand per deposit groups

SELECT DepositGroup, MAX(MagicWandSize) AS [LongestMagicWand]
  FROM WizzardDeposits
 GROUP BY DepositGroup

-- 04. Smallest deposit group per magic wand size

SELECT TOP 2 DepositGroup 
  FROM WizzardDeposits
 GROUP BY DepositGroup
 ORDER BY AVG(MagicWandSize)

-- 05. Deposits sum

SELECT DepositGroup, SUM(DepositAmount) AS TotalSum
  FROM WizzardDeposits
 GROUP BY DepositGroup

-- 06. Deposits sum for Ollivander family

SELECT DepositGroup, SUM(DepositAmount) AS TotalSum
  FROM WizzardDeposits
 WHERE MagicWandCreator = 'Ollivander family'
 GROUP BY DepositGroup

-- 07. Deposits filter

SELECT DepositGroup, SUM(DepositAmount) AS [TotalSum]
  FROM WizzardDeposits
 WHERE MagicWandCreator = 'Ollivander family'
 GROUP BY DepositGroup
HAVING SUM(DepositAmount) < 150000
 ORDER BY [TotalSum] DESC

-- 08. Deposit charge

SELECT DepositGroup, MagicWandCreator, MIN(DepositCharge) AS [MinDepositCharge]
  FROM WizzardDeposits
 GROUP BY DepositGroup, MagicWandCreator
 ORDER BY MagicWandCreator,
          DepositGroup

-- 09. Age groups

SELECT
 CASE 
     WHEN Age BETWEEN 0 AND 10 THEN '[0-10]'
	 WHEN Age BETWEEN 11 AND 20 THEN '[11-20]'
	 WHEN Age BETWEEN 21 AND 30 THEN '[21-30]'
	 WHEN Age BETWEEN 31 AND 40 THEN '[31-40]'
	 WHEN Age BETWEEN 41 AND 50 THEN '[41-50]'
	 WHEN Age BETWEEN 51 AND 60 THEN '[51-60]'
	 ELSE '[61+]'
  END AS [AgeGroup],
  COUNT([AgeGroup])

