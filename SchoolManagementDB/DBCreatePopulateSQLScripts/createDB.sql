CREATE DATABASE SchoolManagement;
go
--create DB

USE SchoolManagement;
CREATE TABLE Subject(
Id INT IDENTITY (1,1),
Code NVARCHAR(255) NOT NULL,
Title NVARCHAR(255) NOT NULL,
CONSTRAINT PK_Subject_Id PRIMARY KEY(Id),
CONSTRAINT UQ_Subject_Code UNIQUE(Code),
CONSTRAINT UQ_Subject_Title UNIQUE(Title))
go

USE SchoolManagement;
CREATE TABLE TaughtModule(
Id INT IDENTITY (1,1),
Code NVARCHAR(255) NOT NULL,
Title NVARCHAR(255) NOT NULL,
CONSTRAINT PK_TaughtModule_Id PRIMARY KEY(Id),
CONSTRAINT UQ_TaughtModule_Code UNIQUE(Code),
CONSTRAINT UQ_TaughtModule_Title UNIQUE(Title))
go



USE SchoolManagement;
CREATE TABLE Lecturer(
Id INT IDENTITY (1,1),
FirstName NVARCHAR(255) NOT NULL,
LastName NVARCHAR(255) NOT NULL,
SchoolEmail NVARCHAR(255) NOT NULL,
CONSTRAINT PK_Lecturer_Id PRIMARY KEY(Id),
CONSTRAINT UQ_Lecturer_SchoolEmail UNIQUE(SchoolEmail),
CONSTRAINT CHK_Lecturer_SchoolEmail CHECK(RIGHT(SchoolEmail,17)='@fac.bootcamp.org'))
go

USE SchoolManagement;
CREATE TABLE Student(
Id INT IDENTITY (1,1),
FirstName NVARCHAR(255) NOT NULL,
LastName NVARCHAR(255) NOT NULL,
SchoolEmail NVARCHAR(255) NOT NULL,
DateOfBirth DATE NOT NULL,
CONSTRAINT PK_Student_Id PRIMARY KEY(Id),
CONSTRAINT CHK_Student_DateOfBirth CHECK(DateOfBirth < (CAST( GETDATE() AS Date ))),
CONSTRAINT CHK_Student_SchoolEmail CHECK(RIGHT(SchoolEmail,18)='@stud.bootcamp.org'))
go


USE SchoolManagement;
CREATE TABLE Course(
Id INT IDENTITY (1,1),
Code NVARCHAR(255)NULL,
TaughtModuleId INT NOT NULL,
SubjectId INT NOT NULL,
Credits INT NOT NULL,
CONSTRAINT PK_Course_Id PRIMARY KEY(Id),
CONSTRAINT FK_Course_TaughtModuleId FOREIGN KEY (TaughtModuleId) REFERENCES TaughtModule(Id),
CONSTRAINT FK_Course_SubjectId FOREIGN KEY (SubjectId) REFERENCES Subject(Id),
CONSTRAINT UQ_Course_TaughtModuleIdSubjectId UNIQUE(TaughtModuleId,SubjectId),
CONSTRAINT CHK_Course_Credits CHECK(Credits>0))
go

CREATE TRIGGER trg_Course_Code
ON Course AFTER INSERT AS
UPDATE Course SET Code='CC.'+m.code+'.'+s.code
FROM Course c
join inserted b on c.id=b.id
join TaughtModule m on c.TaughtModuleId=m.id
join Subject s on c.subjectId=s.Id
go


USE SchoolManagement;
CREATE TABLE Seminar(
Id INT IDENTITY (1,1),
Title NVARCHAR(255) NULL,
CourseId INT NOT NULL,
LecturerId INT NOT NULL,
Term INT NOT NULL,
CONSTRAINT PK_Seminar_Id PRIMARY KEY(Id),
CONSTRAINT FK_Seminar_LecturerId FOREIGN KEY (LecturerId) REFERENCES Lecturer(Id),
CONSTRAINT FK_Seminar_CourseId FOREIGN KEY (CourseId) REFERENCES Course(Id),
CONSTRAINT UQ_Seminar_CourseIdLecturerId UNIQUE(CourseId,Term),
CONSTRAINT CHK_Seminar_Term CHECK(Term =1 or Term=2))
go

CREATE TRIGGER trg_Seminar_Title
ON Seminar AFTER INSERT AS
UPDATE Seminar SET Title=(CASE when s.Term=1 then 'Fall ' else 'Spring ' END)+c.Code
FROM Seminar s
join inserted i on s.id=i.id
join Course c on s.CourseId=c.id
go


USE SchoolManagement;
CREATE TABLE Enrollment(
Id INT IDENTITY(1,1),
SeminarId INT NOT NULL,
StudentId INT NOT NULL,
CONSTRAINT PK_Enrollment_Id PRIMARY KEY(Id),
CONSTRAINT UQ_Enrollment_SeminarIdStudentId UNIQUE(SeminarId,StudentId),
CONSTRAINT FK_Enrollment_SeminarId FOREIGN KEY(SeminarId) REFERENCES Seminar(Id),
CONSTRAINT FK_Enrollment_StudentId FOREIGN KEY(StudentId) REFERENCES Student(Id))
go

--populate DB
INSERT INTO Lecturer(firstName,lastName,schoolEmail) VALUES 
('Giorgos','Pasparakis','gp@fac.bootcamp.org'),
('Michalis','Grivas','mg@fac.bootcamp.org'),
('Maria','Kalogiroy','mk@fac.bootcamp.org'),
('Dimitris','Bakas','db@fac.bootcamp.org')

INSERT INTO Student(firstName,lastName,schoolEmail,DateOfBirth) VALUES 
('Giorgos','Petrou','gp@stud.bootcamp.org','1980-11-20'),
('Petros','Dimakis','pg@stud.bootcamp.org','1988-12-25'),
('Lia','Papadaki','lp@stud.bootcamp.org','1992-01-20'),
('Giorgos','Kornilakis','gk@stud.bootcamp.org','1990-11-25'),
('Nikos','Passas','np@stud.bootcamp.org','1989-12-25'),
('Vasiliki','Manioy','vm@stud.bootcamp.org','1998-07-25'),
('Loukas','Ntoskas','lk@stud.bootcamp.org','1998-10-22'),
('Spyros','Trikoypis','st@stud.bootcamp.org','1999-02-02'),
('Sakis','Halias','sh@stud.bootcamp.org','1999-02-02')

INSERT INTO Subject(Code,Title) VALUES 
('CS','C#'),
('JV','Java'),
('PY','Python'),
('SQLS','MS SQLServer'),
('MSQL','MySQL'),
('JS','Javascript'),
('HC','HTML'),
('BEFRCS','ASP.NET'),
('BEFRJV','Spring MVC'),
('JSFRR','React'),
('JSFRA','Angular')

INSERT INTO TaughtModule(Code,Title) VALUES 
('OOP','Object Oriented Programmnng'),
('BWD','Backend Web Development'),
('FWD','Frontend Web Development'),
('DB','Databases'),
('ML','Machine Learning')

INSERT INTO Course(TaughtModuleId,SubjectId,Credits) VALUES
(1,1,30),
(1,2,30),
(5,3,20),
(4,4,25),
(3,11,25),
(2,8,25)
go

INSERT INTO Seminar(CourseId,LecturerId,Term) VALUES
(1,1,1),
(2,1,1),
(3,3,2),
(4,4,2),
(5,2,1),
(6,3,2)
go

INSERT INTO Enrollment(SeminarId,StudentId) VALUES
(1,1),(1,2),(1,3),(1,4),(1,5),(1,6),(2,7),(2,8),(3,1),
(3,4),(3,5),(3,8),(4,1),(4,3),(5,7),(5,8),(6,1),
(6,2),(6,5)
go


--Stored procs to get data

CREATE PROCEDURE usp_getAllStudents
AS
BEGIN
select * from student order by id asc
END
GO

CREATE PROCEDURE usp_getAllSubjects
AS
BEGIN
select * from subject order by id asc
END
GO

CREATE PROCEDURE usp_getAllTaughtModules
AS
BEGIN
select * from taughtModule order by id asc
END
GO

CREATE PROCEDURE usp_getAllLecturers
AS
BEGIN
select * from lecturer order by id asc
END
GO

CREATE PROCEDURE usp_getAllCourses
AS
BEGIN
select c.id,c.code, m.title as module,s.title as subject, c.credits
from course c join taughtModule m on c.taughtModuleId=m.id
join subject s on c.subjectId=s.id 
order by c.id asc
END
GO

CREATE PROCEDURE usp_getAllSeminars
AS
BEGIN
select s.id,s.title,s.CourseId,s.Term,left(l.firstName,1)+'.'+l.lastName as Lecturer
from seminar s join course c on s.courseId=c.Id
join lecturer l on s.lecturerId=l.Id
order by s.id asc
END
GO

CREATE PROCEDURE usp_getAllEnrollments
AS
BEGIN
select s.Id, s.SeminarId,se.Title as Seminar, s.StudentId,Left(st.firstName,1)+'.'+st.LastName as Student
from enrollment s join seminar se on s.SeminarId=se.Id
join student st on s.studentId=st.Id
order by se.term,se.Title asc
END
GO

CREATE PROCEDURE usp_getStudentsPerSeminar
AS
BEGIN
select s.title as Seminar, 
count(ss.SeminarId) as Participation from seminar s 
join Enrollment ss on ss.seminarId = s.id
group by  s.title
order by count(ss.seminarId) desc
END
GO

CREATE PROCEDURE usp_getSeminarsPerStudent
AS
BEGIN
select cast(s.Id as nvarchar(255))+'. '+s.firstName+' '+s.LastName as 'Student Details', count(ss.seminarId) as Courses
from student s left join enrollment ss on ss.StudentId = s.Id
group by cast(s.Id as nvarchar(255))+'. '+s.firstName+' '+s.LastName
order by count(ss.seminarId) desc
END
GO


--stored procs to insert data

CREATE PROCEDURE usp_insertTaughtModule(
@Code NVARCHAR(255),
@Title NVARCHAR(255))
AS
BEGIN
insert into TaughtModule(Code,Title)
values (@Code,@Title);
END
go

CREATE PROCEDURE usp_insertSubject(
@Code NVARCHAR(255),
@Title NVARCHAR(255))
AS
BEGIN
insert into Subject(Code,Title)
values (@Code,@Title);
END
go


CREATE PROCEDURE usp_insertStudent(
@FirstName NVARCHAR(255),
@LastName NVARCHAR(255),
@SchoolEmail NVARCHAR(255),
@DateOfBirth Date)
AS
BEGIN
insert into Student(FirstName,LastName,SchoolEmail,DateOfBirth)
values (@FirstName,@LastName,@SchoolEmail,@DateOfBirth);
END
go

CREATE PROCEDURE usp_insertLecturer(
@FirstName NVARCHAR(255),
@LastName NVARCHAR(255),
@SchoolEmail NVARCHAR(255))
AS
BEGIN
insert into Lecturer(FirstName,LastName,SchoolEmail)
values (@FirstName,@LastName,@SchoolEmail);
END
go

CREATE PROCEDURE usp_insertCourse(
@TaughtModuleId int,
@SubjectId int,
@Credits int)
AS
BEGIN
insert into Course(TaughtModuleId,SubjectId,Credits)
values (@TaughtModuleId,@SubjectId,@Credits);
END
go


CREATE PROCEDURE usp_insertSeminar(
@CourseId int,
@LecturerId int,
@Term int)
AS
BEGIN
insert into Seminar(CourseId,LecturerId,Term)
values (@CourseId,@LecturerId,@Term);
END
go

CREATE PROCEDURE usp_insertEnrollment(
@SeminarId int,
@StudentId int)
AS
BEGIN
insert into Enrollment(SeminarId,StudentId)
values (@SeminarId,@StudentId)
END
go




