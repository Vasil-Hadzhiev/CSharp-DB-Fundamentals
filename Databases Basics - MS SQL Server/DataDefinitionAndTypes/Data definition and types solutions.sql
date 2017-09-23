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