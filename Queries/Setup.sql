Drop Database GTLDB;
Create Database GTLDB;

use GTLDB
--Table: Non-lendable
CREATE TABLE NonLendable(
	ID int Identity(1,1) Primary Key,
	NonLendType varchar(128),
	);

--Table: Subject Area
CREATE TABLE SubjectArea(
	ID int Identity(1,1) Primary Key,
	AreaType varchar(128),
	);

--Table: CoverType
CREATE TABLE CoverType(
	ID int Identity(1,1) Primary Key,
	Cover varchar(128),
	);

--Table: Language
CREATE TABLE LanguageType(
	ID char(2) Primary Key,
	Lang varchar(128),
	);

--Table: Author
CREATE TABLE AuthorName(
	ID int Identity(1,1) Primary Key,
	FName varchar(50),
	Lname varchar(50)
	);

--Table: Book
CREATE TABLE Book (
	ISBN varchar(16) Primary key,
	Title varchar(128),
	Edition int,
	Descript varchar(255),
	Copies int,
	AuthorID int Foreign Key References AuthorName(ID),
	NonLendID int Foreign Key References NonLendable(ID),
	SubjectID int Foreign Key References SubjectArea(ID),
	CoverID int Foreign Key References CoverType(ID),
	LangID char(2) Foreign Key References LanguageType(ID)
	);
	
--Table: Reason
CREATE TABLE AcquireReason(
	ID int Identity(1,1) Primary Key,
	Reason varchar(128),
	);

--Table: AcquireList
CREATE TABLE AcquireList(
	ID int Identity(1,1) Primary Key,
	Resolved int CHECK (Resolved=0 OR Resolved=1),
	ISBN varchar(16) Foreign Key References Book(ISBN),
	ReasonID int Foreign Key References AcquireReason(ID)
	);

--Table: Card
CREATE TABLE Card(
	ID int Identity(1,1) Primary Key,
	AddressID int Identity(1,1) Foreign Key,
	ExpiryDate DATETIME,
	CreateDate DATETIME,
	);

--Table: Loan
CREATE TABLE Loan(
	ID int Identity(1,1) Primary Key,
	LoanDate DATETIME,
	ReturnDate DATETIME,
	ActualReturnDate DATETIME,
	CardID int Foreign Key References Card(ID),
	ISBN varchar(16) Foreign Key References Book(ISBN)
	);

--Table: NoticeType
CREATE TABLE NoticeType(
	ID int Identity(1,1) Primary Key,
	NoticeName varchar(128),
	);

--Table: Notice
CREATE TABLE Notice(
	ID int Identity(1,1) Primary Key,
	Msg varchar(255),
	CardID int Foreign Key References Card(ID),
	);

--Table: Order
CREATE TABLE Orders(
	ID int Identity(1,1) Primary Key,
	OrderDate DATETIME,
	RequiredDate DATETIME,
	CardID int Foreign Key References Card(ID),
	LoanID int Foreign Key References Loan(ID),
	ISBN varchar(16) Foreign Key References Book(ISBN)
	);

--Table: EmployeeType
CREATE TABLE EmployeeType(
	ID int Identity(1,1) Primary Key,
	EmpType varchar(128),
	);

--Table: State
CREATE TABLE State(
	ID int Identity(1,1) Primary Key,
	Statename varchar(128),
	);

--Table: Zip
CREATE TABLE Zip(
	ID varchar(16)  Primary Key,
	ZipName varchar(128),
	);

--Table: Address
CREATE TABLE Addresses(
	ID int Identity(1,1) Primary Key,
	AddressLine1 varchar(255),
	AddressLine2 varchar(255),
	City varchar(255),
	StateID int Foreign Key References State(ID),
	ZipID varchar(16) Foreign Key References Zip(ID),
	);
	   
--Table: Members
CREATE TABLE Members(
	SSN varchar(50) Primary Key,
	FName varchar(128),
	LName varchar(128),
	ActiveMember int CHECK (ActiveMember=0 OR ActiveMember=1),
	Phone varchar(16),
	CampusAddress int Foreign Key References Addresses(ID),
	HomeAddress int Foreign Key References Addresses(ID),
	CardID int Foreign Key References Card(ID),
	EmployeeTypeID int Foreign Key References EmployeeType(ID), 
	);

ALTER TABLE book add InStock int Not NULL Default(30);

