-- 01. Create table logs

CREATE TABLE Logs 
(
	LogId INT PRIMARY KEY IDENTITY,
	AccountId INT,
	OldSum MONEY,
	NewSum MONEY
)

CREATE TRIGGER tr_AccountBalanceChange ON Accounts 
FOR UPDATE
AS
BEGIN
  DECLARE @AccountId INT = (SELECT Id FROM inserted);
  DECLARE @OldBalance MONEY = (SELECT Balance FROM deleted);
  DECLARE @NewBalance MONEY = (SELECT Balance FROM inserted);

  IF(@newBalance <> @oldBalance)
  INSERT INTO Logs VALUES 
  (@AccountId, @OldBalance, @NewBalance);
END

-- 02. Create table emails

CREATE TABLE NotificationEmails
(
	Id INT PRIMARY KEY IDENTITY,
	Recipient INT,
	Subject NVARCHAR(MAX),
	Body NVARCHAR(MAX)
)

CREATE TRIGGER tr_NewEmailRegister ON Logs 
FOR INSERT
AS
BEGIN

	DECLARE @Recipient int = (SELECT AccountId FROM inserted);
	DECLARE @OldBalance money = (SELECT OldSum FROM inserted);
	DECLARE @NewBalance money = (SELECT NewSum FROM inserted);
	DECLARE @Subject varchar(200) = CONCAT('Balance change for account: ', @recipient);
	DECLARE @Body varchar(200) = CONCAT('On ', GETDATE(), ' your balance was changed from ', @oldBalance, ' to ', @newBalance, '.');  

	INSERT INTO NotificationEmails (Recipient, Subject, Body) 
	VALUES (@recipient, @subject, @body)
END

-- 03. Deposit money

CREATE PROC usp_DepositMoney (@AccountId INT, @MoneyAmount DECIMAL(10, 4))
AS
BEGIN	
	IF(@MoneyAmount >= 0)
	BEGIN
		UPDATE Accounts
		SET Balance += @MoneyAmount
		WHERE Id = @AccountId
	END
END

SELECT * FROM Accounts
WHERE Id = 1

EXEC dbo.usp_DepositMoney 1, 10.5
	
-- 04. Withdraw money

CREATE PROC usp_WithdrawMoney(@AccountId INT, @MoneyAmount DECIMAL(10, 4))
AS
BEGIN
	IF(@MoneyAmount >= 0)
	BEGIN
		UPDATE Accounts
		SET Balance -= @MoneyAmount
		WHERE Id = @AccountId
	END
END

SELECT * FROM Accounts
WHERE Id = 1

EXEC dbo.usp_WithdrawMoney 1, 10.5

-- 05. Money transfer

CREATE PROC usp_TransferMoney(@SenderId INT, @ReceiverId INT, @Amount MONEY)
AS
BEGIN
BEGIN TRANSACTION
	EXEC usp_WithdrawMoney @SenderId, @Amount
	EXEC usp_DepositMoney @ReceiverId, @Amount

	DECLARE @SenderBalance MONEY = 
	(
	SELECT Balance 
	FROM Accounts
	WHERE Id = @SenderId
	)

	IF @SenderBalance < 0
	BEGIN
		ROLLBACK
		RETURN
	END
COMMIT
END

EXEC usp_TransferMoney 1, 2, 100

SELECT * FROM Accounts
WHERE Id  = 1

SELECT* FROM Accounts
WHERE Id = 2

-- 07. Massive shopping

DECLARE @UserId INT = (SELECT Id FROM Users WHERE Username = 'Stamat')
DECLARE @GameId INT = (SELECT Id FROM Games WHERE Name = 'Safflower')
DECLARE @UserGameId INT = (SELECT Id FROM UsersGames WHERE UserId = @UserId AND GameId = @GameId)
DECLARE @ItemsPrice MONEY
DECLARE @UserBalance MONEY

BEGIN TRANSACTION
	SET @ItemsPrice = (SELECT SUM(Price) FROM Items WHERE MinLevel IN(11, 12))
	SET @UserBalance = (SELECT Cash FROM UsersGames WHERE Id = @UserGameId)

	IF(@UserBalance >= @ItemsPrice)
	BEGIN
		INSERT INTO UserGameItems
		SELECT i.Id, @userGameID FROM Items AS i
		WHERE i.Id IN (SELECT Id FROM Items WHERE MinLevel IN(11, 12))

		UPDATE UsersGames
		SET Cash -= @itemsPrice
		WHERE Id = @UserGameId
		COMMIT
	END
	ELSE
	BEGIN
		ROLLBACK
	END

SET @UserBalance = (SELECT Cash FROM UsersGames WHERE Id = @UserGameId)

BEGIN TRANSACTION
	SET @ItemsPrice = (SELECT SUM(Price) FROM Items WHERE MinLevel IN(19, 20, 21))
	SET @UserBalance = (SELECT Cash FROM UsersGames WHERE Id = @UserGameId)

	IF(@UserBalance >= @ItemsPrice)
	BEGIN
		INSERT INTO UserGameItems
		SELECT i.Id, @userGameID FROM Items AS i
		WHERE i.Id IN (SELECT Id FROM Items WHERE MinLevel IN(19, 20, 21))

		UPDATE UsersGames
		SET Cash -= @itemsPrice
		WHERE Id = @UserGameId
		COMMIT
	END
	ELSE
	BEGIN
		ROLLBACK
	END
	
SELECT Name AS [Item Name]
FROM Items
WHERE Id IN (SELECT ItemId FROM UserGameItems WHERE UserGameId = @UserGameId)
ORDER BY [Item Name]

-- 08. Employees with three projects

USE SoftUni

CREATE PROC usp_AssignProject(@EmployeeId INT, @ProjectID INT)
AS
BEGIN
BEGIN TRANSACTION
	DECLARE @ProjectsCount INT = 
	(
		SELECT COUNT(*) 
		FROM EmployeesProjects 
		WHERE EmployeeID = @EmployeeId
	)

	INSERT INTO EmployeesProjects VALUES
	(@EmployeeId, @ProjectID)

	IF(@ProjectsCount >= 3)
	BEGIN
		RAISERROR('The employee has too many projects!', 16, 1)
		ROLLBACK
	END

	COMMIT
END

-- 09. Delete employees	

CREATE TABLE Deleted_Employees
(
	EmployeeId INT PRIMARY KEY IDENTITY,
	FirstName VARCHAR(50),
	LastName VARCHAR(50),
	MiddleName VARCHAR(50),
	JobTitle VARCHAR(50),
	DepartmentId INT,
	Salary MONEY
)

CREATE TRIGGER tr_DeleteEmployee ON Employees 
AFTER DELETE
AS
INSERT INTO Deleted_Employees(FirstName, LastName, MiddleName, JobTitle, DepartmentId, Salary)
SELECT d.FirstName, d.LastName, d.MiddleName, d.JobTitle, d.DepartmentID, d.Salary 
FROM deleted AS d