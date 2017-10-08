-- 01. One-to-one relationship

CREATE TABLE Persons (
	PersonID INT PRIMARY KEY IDENTITY,
	FirstName VARCHAR(50) NOT NULL,
	Salary DECIMAL(10, 2) NOT NULL,
	PassportID INT NOT NULL
)

CREATE TABLE Passports (
	PassportID INT PRIMARY KEY,
	PassportNumber VARCHAR(50) NOT NULL
)

INSERT INTO Persons(FirstName, Salary, PassportID) VALUES 
('Roberto', 43300.00, 102),
('Tom', 56100.00, 103),
('Yana', 60200.00, 101)

INSERT INTO Passports(PassportID, PassportNumber) VALUES
(101, 'N34FG21B'),
(102, 'K65LO4R7'),
(103, 'ZE657QP2')

ALTER TABLE Persons
ADD CONSTRAINT FK_Persons_Passports
FOREIGN KEY (PassportID)
REFERENCES Passports(PassportID)

-- 02. One-to-many relationship

CREATE TABLE Models (
	ModelID INT PRIMARY KEY,
	Name VARCHAR(50) NOT NULL,
	ManufacturerID INT
)

CREATE TABLE Manufacturers (
	ManufacturerID INT PRIMARY KEY IDENTITY,
	Name VARCHAR(50) NOT NULL,
	EstablishedON DATE
)

INSERT INTO Models(ModelID, Name, ManufacturerID) VALUES
(101, 'X1', 1),
(102, 'i6', 1),
(103, 'Model S', 2),
(104, 'Model X', 2),
(105, 'Model 3', 2),
(106, 'Nova', 3)

INSERT INTO Manufacturers(Name, EstablishedOn) VALUES
('BMW', '07/03/1916'),
('Tesla', '01/01/2003'),
('Lada', '01/05/1966')

ALTER TABLE Models
ADD CONSTRAINT FK_Models_Manufacturers
FOREIGN KEY (ManufacturerID)
REFERENCES Manufacturers(ManufacturerID)

-- 03. Many-to-many relationship

CREATE TABLE Students (
	StudentID INT PRIMARY KEY,
	Name VARCHAR(50) NOT NULL
)

CREATE TABLE Exams (
	ExamID INT PRIMARY KEY,
	Name VARCHAR(50)
)

CREATE TABLE StudentsExams (
	StudentID INT,
	ExamID INT,
	CONSTRAINT PK_StudentID_ExamID 
	PRIMARY KEY (StudentID, ExamID),

	CONSTRAINT FK_StudentsExams_Students
	FOREIGN KEY (StudentID) 
	REFERENCES Students(StudentID),

	CONSTRAINT FK_StudentsExams_Exams 
	FOREIGN KEY (ExamID) 
	REFERENCES Exams(ExamID)
)

INSERT INTO Students(StudentID, Name) VALUES
(1, 'Mila'),
(2, 'Toni'),
(3, 'Ron')

INSERT INTO Exams(ExamID, Name) VALUES
(101, 'SpringMVC'),
(102, 'Neo4j'),
(103, 'Oracle 11g')

INSERT INTO StudentsExams(StudentID, ExamID) VALUES
(1, 101),
(1, 102),
(2, 101),
(3, 103),
(2, 102),
(2, 103)

-- 04. Self-referencing

CREATE TABLE Teachers (
	TeacherID INT PRIMARY KEY,
	Name VARCHAR(50) NOT NULL,
	ManagerID INT,
	CONSTRAINT FK_Teacher_Manager 
	FOREIGN KEY (ManagerID) 
	REFERENCES Teachers(TeacherID)
)

INSERT INTO Teachers VALUES
(101, 'John', NULL),
(102, 'Maya', 106),
(103, 'Silvia', 106),
(104, 'Ted', 105),
(105, 'Mark', 101),
(106, 'Greta', 101)

-- 05. Online store database

CREATE DATABASE OnlineStore

USE OnlineStore

CREATE TABLE Cities (
	CityID INT PRIMARY KEY IDENTITY,
	Name VARCHAR(50)
)

CREATE TABLE Customers (
	CustomerID INT PRIMARY KEY IDENTITY,
	Name VARCHAR(50),
	Birthday DATE,
	CityID INT,
	CONSTRAINT FK_Customers_Cities 
	FOREIGN KEY (CityID) 
	REFERENCES Cities(CityID)
)

CREATE TABLE Orders (
	OrderID INT PRIMARY KEY IDENTITY,
	CustomerID INT,
	CONSTRAINT FK_Orders_Customers
	FOREIGN KEY (CustomerID) 
	REFERENCES Customers(CustomerID)
)

CREATE TABLE ItemTypes (
	ItemTypeID INT PRIMARY KEY IDENTITY,
	Name VARCHAR(50)
)

CREATE TABLE Items (
	ItemID INT PRIMARY KEY IDENTITY,
	Name VARCHAR(50),
	ItemTypeID INT,
	CONSTRAINT FK_Items_ItemTypes 
	FOREIGN KEY (ItemTypeID)
	REFERENCES ItemTypes(ItemTypeID)
)

CREATE TABLE OrderItems (
	OrderID INT,
	ItemID INT,

	CONSTRAINT PK_OrderItems
	PRIMARY KEY (OrderID, ItemID),

	CONSTRAINT FK_OrderItems_Orders
	FOREIGN KEY (OrderID)
	REFERENCES Orders(OrderID),

	CONSTRAINT FK_OrderItems_Items
	FOREIGN KEY (ItemID)
	REFERENCES Items(ItemID)
)

-- 06. University database

CREATE DATABASE University

USE University

CREATE TABLE Majors (
	MajorID INT PRIMARY KEY IDENTITY,
	Name VARCHAR(50)
)

CREATE TABLE Students (
	StudentID INT PRIMARY KEY IDENTITY,
	StudentNumber VARCHAR(10),
	StudentName VARCHAR(50),
	MajorID INT,
	CONSTRAINT FK_Students_Majors
	FOREIGN KEY (MajorID)
	REFERENCES Majors(MajorID)
)

CREATE TABLE Subjects (
	SubjectID INT PRIMARY KEY IDENTITY,
	SubjectName VARCHAR(50)
)

CREATE TABLE Agenda (
	StudentID INT,
	SubjectID INT,

	CONSTRAINT PK_Agenda
	PRIMARY KEY (StudentID, SubjectID),

	CONSTRAINT FK_Agenda_Students
	FOREIGN KEY (StudentID)
	REFERENCES Students(StudentID),

	CONSTRAINT FK_Agenda_Subjects
	FOREIGN KEY (SubjectID)
	REFERENCES Subjects(SubjectID)
)

CREATE TABLE Payments (
	PaymentID INT PRIMARY KEY IDENTITY,
	PaymentDate DATE,
	PaymentAmount DECIMAL,
	StudentID INT,
	CONSTRAINT FK_Payments_Students
	FOREIGN KEY (StudentID)
	REFERENCES Students(StudentID)
)

-- 09. Peaks in Rila

USE Geography

SELECT m.MountainRange, p.PeakName, p.Elevation
  FROM Mountains AS m
  JOIN Peaks as p ON p.MountainId = m.Id
 WHERE m.MountainRange = 'Rila'
 ORDER BY p.Elevation DESC