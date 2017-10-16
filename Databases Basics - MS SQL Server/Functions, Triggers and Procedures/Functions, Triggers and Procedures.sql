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

CREATE PROC usp_GetEmployeesSalaryAboveNumber @Number DECIMAL(18, 4)
AS
BEGIN
	SELECT FirstName, LastName
	FROM Employees
	WHERE Salary >= @Number
END

EXEC dbo.usp_GetEmployeesSalaryAboveNumber 48100

-- 03. Towns name starting with B

CREATE PROC usp_GetTownsStartingWith @String VARCHAR(1)
AS
BEGIN
	SELECT Name AS [Town]
	FROM Towns
	WHERE Name LIKE CONCAT(@String, '%')
END

EXEC dbo.usp_GetTownsStartingWith 'b'

-- 04. Employees from town 

CREATE PROC usp_GetEmployeesFromTown @TownName VARCHAR(20)
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

CREATE PROC usp_EmployeesBySalaryLevel @SalaryLevel VARCHAR(10)
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


