-- 01. Employees with salary above 35000

CREATE PROC usp_GetEmployeesSalaryAbove35000 
AS
BEGIN
	SELECT FirstName, LastName
	FROM Employees
	WHERE Salary > 35000
END

EXEC dbo.usp_GetEmployeesSalaryAbove35000

-- 02. Employees with salary above number

CREATE PROC usp_GetEmployeesSalaryAboveNumber(@Number DECIMAL(18, 4))
AS
BEGIN
	SELECT FirstName, LastName
	FROM Employees
	WHERE Salary >= @Number
END

EXEC dbo.usp_GetEmployeesSalaryAboveNumber 48100

-- 03. Towns name starting with B

CREATE PROC usp_GetTownsStartingWith(@String NVARCHAR(MAX))
AS
BEGIN
	SELECT Name AS [Town]
	FROM Towns
	WHERE Name LIKE CONCAT(@String, '%')
END

EXEC dbo.usp_GetTownsStartingWith 'b'

-- 04. Employees from town 

CREATE PROC usp_GetEmployeesFromTown(@TownName VARCHAR(20))
AS
BEGIN
	SELECT e.FirstName, e.LastName 
	FROM Employees AS e
	INNER JOIN Addresses AS a ON a.AddressID = e.AddressID
	INNER JOIN Towns AS t ON t.TownID = a.TownID
	WHERE t.Name = @TownName
END

EXEC dbo.usp_GetEmployeesFromTown 'Sofia'

-- 05. Salary level function

CREATE FUNCTION ufn_GetSalaryLevel(@Salary MONEY)
RETURNS VARCHAR(10)
AS
BEGIN
	DECLARE @SalaryLevel VARCHAR(10)

	SET @SalaryLevel =
	CASE
		WHEN @Salary < 30000 THEN 'Low'
		WHEN @Salary BETWEEN 30000 AND 50000 THEN 'Average'
		ELSE 'High'
	END

	RETURN @SalaryLevel
END

SELECT Salary, dbo.ufn_GetSalaryLevel(Salary) AS [Salary Level]
FROM Employees
ORDER BY Salary

-- 06. Employees by salary level

CREATE PROC usp_EmployeesBySalaryLevel(@SalaryLevel VARCHAR(10))
AS
BEGIN 
	SELECT FirstName, LastName
	FROM Employees
	WHERE dbo.ufn_GetSalaryLevel(Salary) = @SalaryLevel
END

EXEC dbo.usp_EmployeesBySalaryLevel 'High'

-- 07. Define function

CREATE FUNCTION ufn_IsWordComprised(@SetOfLetters VARCHAR(MAX), @Word VARCHAR(20))
RETURNS BIT
AS
BEGIN
	DECLARE @CurrentIndex INT = 1
	DECLARE @Length INT = LEN(@Word)
	DECLARE @CurrentLetter CHAR

	WHILE(@CurrentIndex <= @Length)
	BEGIN
		SET @CurrentLetter = SUBSTRING(@Word, @CurrentIndex, 1)

		IF(CHARINDEX(@CurrentLetter, @SetOfLetters) > 0)
		SET @CurrentIndex += 1
		ELSE
		RETURN 0
	END
	RETURN 1
END

SELECT FirstName, dbo.ufn_IsWordComprised('asdfghteri', FirstName) AS [Result]
FROM Employees

-- 08. Delete employees and departments

CREATE PROC usp_DeleteEmployeesFromDepartment(@DepartmentId INT)
AS
ALTER TABLE Departments
ALTER COLUMN ManagerID INT NULL

DELETE 
FROM EmployeesProjects
WHERE EmployeeID IN 
(
	SELECT EmployeeID 
	FROM Employees
	WHERE DepartmentID = @DepartmentId
)

UPDATE Employees
SET ManagerID = NULL
WHERE ManagerID IN 
(
	SELECT EmployeeID 
	FROM Employees
	WHERE DepartmentID = @DepartmentId
)


UPDATE Departments
SET ManagerID = NULL
WHERE ManagerID IN 
(
	SELECT EmployeeID 
	FROM Employees
	WHERE DepartmentID = @DepartmentId
)

DELETE 
FROM Employees
WHERE EmployeeID IN 
(
	SELECT EmployeeID 
	FROM Employees
	WHERE DepartmentID = @DepartmentId
)

DELETE 
FROM Departments
WHERE DepartmentID = @DepartmentId

SELECT COUNT(*) AS [Employees Count] 
FROM Employees AS e
JOIN Departments AS d
ON d.DepartmentID = e.DepartmentID
WHERE e.DepartmentID = @DepartmentId

-- 09. Find full name

CREATE PROC usp_GetHoldersFullName
AS
BEGIN
	SELECT FirstName + ' ' + LastName AS [Full Name]
	FROM AccountHolders
END

EXEC dbo.usp_GetHoldersFullName 

-- 10. People with balance higher than

CREATE PROC usp_GetHoldersWithBalanceHigherThan(@Number Money)
AS
BEGIN
	SELECT ah.FirstName AS [First Name], ah.LastName [Last Name]
	FROM AccountHolders AS ah
	INNER JOIN Accounts AS a ON a.AccountHolderId = ah.Id
	GROUP BY ah.FirstName, ah.LastName
	HAVING SUM(a.Balance) > @Number
END

EXEC dbo.usp_GetHoldersWithBalanceHigherThan 10000

-- 11. Future value function

CREATE FUNCTION ufn_CalculateFutureValue(@Sum MONEY, @YearlyRate FLOAT, @Years INT)
RETURNS MONEY
AS
BEGIN
	DECLARE @Value MONEY

	SET @Value = @Sum * POWER(1 + @YearlyRate, @Years)

	RETURN @Value
END

SELECT dbo.ufn_CalculateFutureValue(1000, 0.1, 5) AS [FV]

-- 12. Calculating interest

CREATE PROC usp_CalculateFutureValueForAccount(@AccountId INT, @YearlyRate FLOAT)
AS
BEGIN
	SELECT ah.Id AS [Account Id],
	       ah.FirstName AS [First Name],
		   ah.LastName AS [Last Name],
		   a.Balance AS [Current Balance],
		   dbo.ufn_CalculateFutureValue(a.Balance, @YearlyRate, 5) AS [Balance in 5 years]
	FROM AccountHolders AS ah
    INNER JOIN Accounts AS a ON a.AccountHolderId = ah.Id
	WHERE a.Id = @AccountId
END

EXEC dbo.usp_CalculateFutureValueForAccount 1, 0.1

-- 13. Scalar function: Cash in user games odd rows

USE Diablo

SELECT * FROM Games
SELECT * FROM UsersGames
	
CREATE FUNCTION ufn_CashInUsersGames(@GameName NVARCHAR(MAX))
RETURNS TABLE
AS
RETURN SELECT Sum(Cash) AS [SumCash]
	   FROM
	   (
			SELECT ug.Cash AS [Cash],
			ROW_NUMBER() OVER(ORDER BY ug.Cash DESC) AS [RowNum]
			FROM UsersGames AS ug
			INNER JOIN Games AS g ON g.Id = ug.GameId
			WHERE g.Name = @GameName
	   ) AS CashList
	   WHERE [RowNum] % 2 <> 0
