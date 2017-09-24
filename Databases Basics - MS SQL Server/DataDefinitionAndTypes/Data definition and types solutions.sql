-- 01. Create Database

CREATE DATABASE Minions

-- 02. Create Tables

CREATE TABLE Minions (
	Id INT PRIMARY KEY NOT NULL,
	Name NVARCHAR(50) NOT NULL,
	Age INT NOT NULL
)

CREATE TABLE Towns (
	Id INT PRIMARY KEY NOT NULL,
	Name NVARCHAR(50) NOT NULL
)

-- 03. Alter Minions table

ALTER TABLE Minions
ADD TownId INT FOREIGN KEY REFERENCES Towns(Id)

-- 04. Insert records in both tables

INSERT INTO Towns (Id, Name) VALUES
(1, 'Sofia'),
(2, 'Plovdiv'),
(3, 'Varna')

INSERT INTO Minions (Id, Name, Age, TownId) VALUES
(1, 'Kevin', '22', 1),
(2, 'Bob', '15', 3),
(3, 'Steward', NULL, 2)

-- 05. Truncate table Minions

TRUNCATE TABLE Minions

-- 06. Drop All Tables

DROP TABLE Minions
DROP TABLE Towns

-- 07. Create table People

CREATE TABLE People (
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	Name NVARCHAR(200) NOT NULL,
	Picture VARBINARY(max),
	Height FLOAT(2),
	Weight FLOAT(2),
	Gender VARCHAR(1) CHECK (Gender = 'm' OR Gender = 'f') NOT NULL,
	Birthdate DATE NOT NULL,
	Biography NVARCHAR(max)
)

INSERT INTO People (Name, Picture, Height, Weight, Gender, Birthdate, Biography) VALUES
('The Flash', NULL, 1.80, 75, 'm', '07-21-1988', 'I run fast and throw lightning bolts'),
('The Arrow', NULL, 1.85, 85, 'm', '12-13-1984', 'You have failed this city'),
('Black Canary', NULL, NULL, NULL, 'f', '03-04-1985', 'Screaaaaaam!!!'),
('Supergirl', NULL, 1.76, 67.50, 'f', '06-17-1990', 'I am the strongest women in the universe'),
('Michael Scofield', NULL, NULL, NULL, 'm', '10-25-1980', 'I have a very special arsenal of abilities')

-- 08. Create table Users

CREATE TABLE Users (
	Id BIGINT IDENTITY NOT NULL,
	Username VARCHAR(30) UNIQUE NOT NULL,
	Password VARCHAR(26) NOT NULL,
	ProfilePicture VARBINARY,
	LastLoginTime DATETIME,
	IsDeleted BIT
	CONSTRAINT PK_Users PRIMARY KEY (Id)
)

INSERT INTO Users (Username, Password, ProfilePicture, LastLoginTime, IsDeleted) VALUES
('Vaso', 'qwepoi94', NULL, NULL, 0),
('Pesho', 'qazplm94', NULL, NULL, 1),
('Gosho', 'asdfgh', NULL, NULL, 0),
('Roso', '123456', NULL, NULL, 1),
('Stancho', 'ken123', NULL, NULL, NULL)

-- 09. Change primary key

ALTER TABLE Users
DROP CONSTRAINT PK_Users

ALTER TABLE Users
ADD CONSTRAINT PK_Users PRIMARY KEY (Id, Username)

-- 10. Add check constraint

ALTER TABLE Users
ADD CONSTRAINT CK_PasswordLength CHECK (LEN(Password) >= 5)

-- 11. Set default value of a field

ALTER TABLE Users
ADD DEFAULT GETDATE() FOR LastLoginTime

-- 12. Set unique field

ALTER TABLE Users
DROP CONSTRAINT PK_Users

ALTER TABLE Users
ADD CONSTRAINT PK_Users PRIMARY KEY (Id)

ALTER TABLE Users
ADD CONSTRAINT UQ_Username UNIQUE (Username)

ALTER TABLE Users
ADD CONSTRAINT CK_UsernameLength CHECK (LEN(Username) >= 3)

-- 13. Movies database

CREATE DATABASE Movies

CREATE TABLE Directors (
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	DirectorName VARCHAR(50) NOT NULL,
	Notes NVARCHAR(max)
)

CREATE TABLE Genres (
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	GenreName NVARCHAR(50) NOT NULL,
	Notes NVARCHAR(max)
)

CREATE TABLE Categories (
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	CategoryName NVARCHAR(50) NOT NULL,
	Notes NVARCHAR(max)
)

CREATE TABLE Movies (
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	Title NVARCHAR(50) NOT NULL,
	DirectorId INT FOREIGN KEY REFERENCES Directors(Id),
	CopyrightYear DATE,
	Length INT,
	GenreId INT FOREIGN KEY REFERENCES Genres(Id),
	CategoryId INT FOREIGN KEY REFERENCES Categories(Id),
	Rating FLOAT(1),
	Notes NVARCHAR(max)
)

INSERT INTO Directors (DirectorName, Notes) VALUES
('Christopher Nolan', 'Hands down, best director ever'),
('John Carpenter', 'Horror movies professional'),
('Quentin Tarantino', NULL),
('Peter Jackson', NULL),
('Steven Spielberg', NULL)

INSERT INTO Genres (GenreName, Notes) VALUES 
('Fantasy', NULL),
('Comedy', NULL),
('Horror', NULL),
('Action', NULL),
('Drama', NULL)

INSERT INTO Categories (CategoryName, Notes) VALUES
('Adventure', NULL),
('Crime', NULL),
('War', NULL),
('SciFi', NULL),
('History', NULL)

INSERT INTO Movies (Title, DirectorId, CopyrightYear, Length, GenreId, CategoryId, Rating, Notes) VALUES
('The Lord of the Rings', 4, '2002', 214, 1, 1, 8.8, NULL),
('Inception', 1, '2010', 148, 4, 4, 8.8, NULL),
('Pulp Fictioc', 3, '1994', 154, 5, 2, 8.9, NULL),
('Saving private Ryan', 5, '1998', 169, 5, 3, 8.6, NULL),
('Escape from New York', 2, '1981', 99, 4, 4, 7.6, 'Old but gold')

-- 14. Car Rental database

CREATE DATABASE CarRental

CREATE TABLE Categories (
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	CategoryName NVARCHAR(50) NOT NULL,
	DailyRate FLOAT(2) NOT NULL,
	WeeklyRate FLOAT(2) NOT NULL,
	MonthlyRate FLOAT(2) NOT NULL,
	WeekendRate FLOAT(2) NOT NULL
)

INSERT INTO Categories (CategoryName, DailyRate, WeeklyRate, MonthlyRate, WeekendRate) VALUES
('Sports', 8.8, 44, 176, 17.6),
('Economy', 2.5, 12.5, 50, 5),
('Luxury', 6, 30, 120, 12)

CREATE TABLE Cars (
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	PlateNumber VARCHAR(8),
	Manufacturer VARCHAR(20),
	Model VARCHAR(20),
	CarYear DATE,
	CategoryId INT FOREIGN KEY REFERENCES Categories(Id),
	Doors INT NOT NULL,
	Picture VARBINARY(max),
	Condition NVARCHAR(50),
	Available BIT
)

INSERT INTO Cars (PlateNumber, Manufacturer, Model, CarYear, CategoryId, Doors, Picture, Condition, Available) VALUES
('PB123400', 'Volkswagen', 'Polo', '1996', 2, 2, NULL, 'Good', 0),
('CA000000', 'Lamborghini', 'Aventador', '2017', 1, 2, NULL, 'Excellent', 1),
('C5566HA', 'Audi', 'A5', '2012', 3, 4, NULL, NULL, NULL)

CREATE TABLE Employees (
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	FirstName NVARCHAR(50) NOT NULL,
	LastName NVARCHAR(50) NOT NULL,
	Title NVARCHAR(30),
	Notes NVARCHAR(max)
)

INSERT INTO Employees (FirstName, LastName, Title, Notes) VALUES
('Stamat', 'Stamatov', 'CEO', NULL),
('Minka', 'Minkova', 'Secretary', NULL),
('Chocho', 'Chochev', NULL, NULL)

CREATE TABLE Customers (
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	DriverLicenseNumber BIGINT,
	FullName NVARCHAR(50) NOT NULL,
	Address NVARCHAR(50),
	City NVARCHAR(20),
	ZipCode INT,
	Notes NVARCHAR(max)
)

INSERT INTO Customers (DriverLicenseNumber, FullName, Address, City, ZipCode, Notes) VALUES
(123456789, 'Kichka Bodurova', NULL, NULL , NULL, NULL),
(88888888888, 'Asparuh Asparuhov', NULL, 'Plovdiv', 4000, NULL),
(9253455224, 'Kolio Kolev', NULL, 'Sofia', 1000, NULL)

CREATE TABLE RentalOrders (
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	EmployeeId INT FOREIGN KEY REFERENCES Employees(Id),
	CustomerId INT FOREIGN KEY REFERENCES Customers(Id),
	CarId INT FOREIGN KEY REFERENCES Cars(Id),
	TankLevel INT,
	KilometrageStart INT,
	KilometrageEnd INT,
	TotalKilometrage INT,
	StartDate DATE,
	EndDate DATE,
	TotalDays AS DATEDIFF(DAY, StartDate, EndDate),
	RateApplied FLOAT(2),
	TaxRate DECIMAL(5, 2),
	OrderStatus NVARCHAR(50),
	Notes NVARCHAR(max)
)

INSERT INTO RentalOrders(EmployeeId, CustomerId, CarId, TankLevel, KilometrageStart, KilometrageEnd, 
TotalKilometrage, StartDate, EndDate, RateApplied, TaxRate, OrderStatus, Notes) VALUES
(1, 1, 1, 50, 11111, 44444, 33333, '01-01-1996', '12-12-2000', 5.5, 100.50, 'Not ordered', NULL),
(2, 2, 2, 60, 22222, 55555, 33333, '01-01-2017', '12-12-2017', 10, 200.80, 'Ordered', NULL),
(3, 3, 3, 70, 33333, 66666, 33333, '01-01-2012', '12-12-2015', 7.5, 180.55, 'Bought', NULL)

-- 15. Hotel database

CREATE DATABASE Hotel

CREATE TABLE Employees (
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	FirstName NVARCHAR(50) NOT NULL,
	LastName NVARCHAR(50) NOT NULL,
	Title NVARCHAR(30),
	Notes NVARCHAR(max)
)

INSERT INTO Employees (FirstName, LastName, Title, Notes) VALUES
('Stamat', 'Stamatov', 'Manager', 'THE BOSS'),
('Minka', 'Minkova', 'Maid', 'Cleaning duty'),
('Chocho', 'Chochev', NULL, NULL)

CREATE TABLE Customers (
	AccountNumber BIGINT UNIQUE NOT NULL,
	FirstName NVARCHAR(50) NOT NULL,
	LastName NVARCHAR(50) NOT NULL,
	PhoneNumber INT,
	EmergencyName NVARCHAR(50),
	EmergencyNumber INT,
	Notes NVARCHAR(max)
)

INSERT INTO Customers (AccountNumber, FirstName, LastName, 
PhoneNumber, EmergencyName, EmergencyNumber, Notes) VALUES
(123456789, 'Stamatcho', 'Stamatchov', 0893336665, NULL, 101, NULL),
(987654321, 'Minkata', 'Minkovata', 0893336666, NULL, 911, NULL),
(135792468, 'Goshkata', 'Goshkovata', 0893336667, NULL, 101, NULL)

CREATE TABLE RoomStatus (
	RoomStatus NVARCHAR(50) PRIMARY KEY NOT NULL,
	Notes NVARCHAR(max)
)

INSERT INTO RoomStatus (RoomStatus, Notes) VALUES
('Available', NULL),
('Cleaning', NULL),
('Occupied', NULL)

CREATE TABLE RoomTypes (
	RoomType NVARCHAR(50) PRIMARY KEY NOT NULL,
	Notes NVARCHAR(max)
)

INSERT INTO RoomTypes (RoomType, Notes) VALUES
('Room for 2', NULL),
('Apartment', NULL),
('Room for 2 + child', NULL)

CREATE TABLE BedTypes (
	BedType NVARCHAR(50) PRIMARY KEY NOT NULL,
	Notes NVARCHAR(max)
)

INSERT INTO BedTypes (BedType, Notes) VALUES
('Duo bed', NULL),
('Big bed', NULL),
('Single bed', NULL)

CREATE TABLE Rooms (
	RoomNumber INT PRIMARY KEY IDENTITY NOT NULL,
	RoomType NVARCHAR(50) FOREIGN KEY REFERENCES RoomTypes(RoomType),
	BedType NVARCHAR(50) FOREIGN KEY REFERENCES BedTypes(BedType),
	Rate FLOAT(2),
	RoomStatus NVARCHAR(50) FOREIGN KEY REFERENCES RoomStatus(RoomStatus),
	Notes NVARCHAR(MAX)
)

INSERT INTO Rooms (RoomType, BedType, Rate, RoomStatus, Notes) VALUES
('Apartment', 'Big bed', 9.5, 'Available', NULL),
('Room for 2', 'Duo bed', 8.5, 'Cleaning', NULL),
('Room for 2 + child', 'Big bed', 9.5, 'Occupied', NULL)

CREATE TABLE Payments (
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	EmployeeId INT FOREIGN KEY REFERENCES Employees(Id),
	PaymentDate DATE,
	AccountNumber BIGINT FOREIGN KEY REFERENCES Customers(AccountNumber),
	FirstDateOccupied DATE,
	LastDateOccupied DATE,
	TotalDays AS DATEDIFF(DAY, FirstDateOccupied, LastDateOccupied),
	AmountCharged DECIMAL(5, 2),
	TaxRate FLOAT(2),
	TaxAmount FLOAT(2),
	PaymentTotal DECIMAL(6, 2) NOT NULL,
	Notes NVARCHAR(max)
)

INSERT INTO Payments (EmployeeId, PaymentDate, AccountNumber, PaymentTotal) VALUES
(1, GETDATE(), 123456789, 1050.50),
(2, GETDATE(), 987654321, 750),
(3, GETDATE(), 135792468, 880.75)

CREATE TABLE Occupancies (
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	EmployeeId INT FOREIGN KEY REFERENCES Employees(Id),
	DateOccupied DATE,
	AccountNumber BIGINT FOREIGN KEY REFERENCES Customers(AccountNumber),
	RoomNumber INT,
	RateApplied FLOAT(2),
	PhoneCharge BIT,
	Notes NVARCHAR(max)
)

INSERT INTO Occupancies (EmployeeId, DateOccupied, AccountNumber, RoomNumber, RateApplied, PhoneCharge) VALUES
(1, GETDATE(), 123456789, 55, 5.5, 1),
(2, GETDATE(), 987654321, 66, 6.5, 0),
(3, GETDATE(), 135792468, 33, 7.5, 1)


-- 16. Create SoftUni database

CREATE DATABASE SoftUni

CREATE TABLE Towns (
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	Name NVARCHAR(50)
)

CREATE TABLE Addresses (
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	AddressesText NVARCHAR(100),
	TownId INT FOREIGN KEY REFERENCES Towns(Id)
)

CREATE TABLE Departments (
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	Name NVARCHAR(50)
)

CREATE TABLE Employees (
	Id INT PRIMARY KEY IDENTITY NOT NULL,
	FirstName NVARCHAR(50) NOT NULL,
	MiddleName NVARCHAR(50) NOT NULL,
	LastName NVARCHAR(50) NOT NULL,
	JobTitle NVARCHAR(50),
	DepartmentId INT FOREIGN KEY REFERENCES Departments(Id),
	HireDate DATE,
	Salary DECIMAL(10, 2),
	AddressId INT FOREIGN KEY REFERENCES Addresses(Id)
)

-- 17. Backup database

BACKUP DATABASE Softuni
	TO DISK = 'E:\PROGRAMMING\Software University\SoftUni\C#\softuni-backup.bak'

DROP DATABASE SoftUni

RESTORE DATABASE SoftUni
	FROM DISK = 'E:\PROGRAMMING\Software University\SoftUni\C#\softuni-backup.bak'

-- 18. Basic insert

INSERT INTO Towns (Name) VALUES
('Sofia'),
('Plovdiv'),
('Varna'),
('Burgas')

INSERT INTO Departments (Name) VALUES
('Engineering'), 
('Sales'), 
('Marketing'),
('Software Development'),
('Quality Assurance')

INSERT INTO Employees (FirstName, MiddleName, LastName, JobTitle, DepartmentId, HireDate, Salary) VALUES
('Ivan', 'Ivanov', 'Ivanov', '.NET Developer', 4, '02/01/2013',	3500.00),
('Petar', 'Petrov', 'Petrov', 'Senior Engineering', 1, '03/02/2004', 4000.00),
('Maria', 'Petrova', 'Ivanova', 'Intern', 5, '08/28/2016', 525.25),
('Georgi', 'Teziev', 'Ivanov', 'CEO', 2, '12/09/2007', 3000.00),
('Peter', 'Pan', 'Pan', 'Intern', 3, '08/28/2016', 599.88)

-- 19. Basic select all fields

SELECT * FROM Towns

SELECT * FROM Departments

SELECT * FROM Employees

-- 20. Basic select all fields and order them

SELECT * FROM Towns ORDER BY Name

SELECT * FROM Departments ORDER BY Name

SELECT * FROM Employees ORDER BY Salary DESC 

-- 21. Basic select some fields

SELECT Name FROM Towns ORDER BY Name

SELECT Name FROM Departments ORDER BY Name

SELECT FirstName, LastName, JobTitle, Salary FROM Employees ORDER BY Salary DESC 

-- 22. Increase employees salary

UPDATE Employees
SET Salary += Salary * 0.1

SELECT Salary FROM Employees

-- 23. Decrease tax rate

USE Hotel

UPDATE Payments
SET TaxRate -= TaxRate * 0.03

SELECT TaxRate FROM Payments

-- 24. Delete all records

TRUNCATE TABLE Occupancies