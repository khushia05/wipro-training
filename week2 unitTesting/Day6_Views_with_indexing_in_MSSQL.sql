use CollegeDb;

-- Create Students table
CREATE TABLE Students (
    student_id INT PRIMARY KEY,
    student_name VARCHAR(50),
    email VARCHAR(50),
    major VARCHAR(50),
    enrollment_year INT
);

-- Create Courses table
CREATE TABLE Courses (
    course_id INT PRIMARY KEY,
    course_name VARCHAR(50),
    credit_hours INT,
    department VARCHAR(50)
);

-- Create StudentCourses table for enrollment
CREATE TABLE StudentCourses (
    enrollment_id INT PRIMARY KEY,
    student_id INT,
    course_id INT,
    semester VARCHAR(20),
    grade CHAR(2),
    FOREIGN KEY (student_id) REFERENCES Students(student_id),
    FOREIGN KEY (course_id) REFERENCES Courses(course_id)
)


-- Insert sample data
INSERT INTO Students VALUES 
(1, 'John Doe', 'john@example.com', 'Computer Science', 2020),
(2, 'Jane Smith', 'jane@example.com', 'Mathematics', 2021),
(3, 'Mike Johnson', 'mike@example.com', 'Physics', 2020);

INSERT INTO Courses VALUES
(101, 'Database Systems', 3, 'CS'),
(102, 'Calculus II', 4, 'MATH'),
(103, 'Quantum Physics', 4, 'PHYSICS');

INSERT INTO StudentCourses VALUES
(1, 1, 101, 'Fall 2023', 'A'),
(2, 1, 102, 'Spring 2024', 'B'),
(3, 2, 102, 'Fall 2023', 'A'),
(4, 3, 103, 'Spring 2024', 'B+');
drop table student;
drop table enrollment;
drop table course;
drop table Instructor

-- Simple View 
select * from Students,StudentCourses,Courses;
create view cs_student as Select student_id,student_name,email from Students
where major= 'computer science';

select * from cs_student;

-- Complex view ( from multiple table with joins)

create view dbo.StudentEntrollments As 
select s.student_name,c.Course_name,sc.Semester,sc.grade
from dbo.Students s 
inner join dbo.StudentCourses sc on s.student_id = sc.student_id
inner join dbo.Courses c on sc.course_id= c.course_id;

--JOIN and INNER JOIN they are exactly same, so inner is used for clarity.

select * from dbo.StudentEntrollments;
select * from Students,Courses;
select * from Courses;

UPDATE StudentEntrollments 
SET Course_name = 'Data Science' 
WHERE student_name = 'John doe';

-- Query and modify view
select top 2 * from dbo.StudentEntrollments;

select * from dbo.StudentEntrollments where grade='A';

-- updating data through view
begin transaction
update dbo.cs_student
set email='john_doe_.edu'
where student_id= 1;

-- varifying the update operation
select * from dbo.cs_student where student_id = 1;
rollback transaction;

--IRCTC servers, Instead of doing every small changes in my main system, we have views that works as data
-- where we can make local changes and later on when they pemanent they are updated in main db
-- Limitation with views ( V.IMP)

-- Attempting to update a complex view using error handling( will fail)
begin try
    begin transaction
    update dbo.StudentEntrollments
    set grade = 'A+'
    where student_name ='John Doe' and course_name='Data Science'
    commit transaction;
end try
begin catch 
    rollback transaction;
    print ' Error Occured..!!'+ ERroR_MESSAGE();
end catch;


-- Altering a View (MSSQL uses create OR Alter in new version) 
-- for older version, we need to drop and create
IF EXISTS (SELECT * FROM sys.views WHERE name = 'cs_students' and )
DROP VIEW view_name;

ALTER VIEW cs_student AS
Select student_id,student_name,email,enrollment_year from Students
where major= 'computer science';

select * from cs_student;

--list all view in the data base
select name as ViewName, create_date,modify_date
from sys.views
where is_ms_shipped=0
order by name;
