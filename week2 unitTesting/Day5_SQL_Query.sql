Create Database Employees
;
use Employees;
Create table Department(
Deptid int primary key,
DeptName varchar(20),
);

create table Employee(
EmpId int primary key,
EmpName varchar(30),
Salary decimal(10,2),
DeptId int,
ManagerId int,
Dateofjoining date);


Insert into Department values
(2,'Finance'),
(3,'IT');

Insert into Employee Values
(101,'Raj',30000,3,null,'2024-01-15'),
(102,'Rajiv',30000,3,101,'2024-01-15'),
(103,'Rajesh',30000,3,101,'2024-01-15'),
(104,'Rajni',30000,3,102,'2024-01-15'),
(105,'Rani',30000,1,null,'2024-01-15');

update Employee set salary = 33240 where EmpId= 101;
Select * from Employee;
Select * from Department;
-- round function 
-- Positve Values Rounds to decimal places (Round(123.456,2) -> 123.46)
-- Negative Values Rounds to power of 10 ro the left( Round( 12345,-2)-> 12300)
select round(salary,-3) from Employee where EmpId= 101;
Select EmpName, Len(EmpName) As NameLength from Employee;

Select GETDATE() as CurrentDate;


-- joins
--Inner joins: Return only matching rows from both table
--Left joins: Return all the rows from the left table and matched rows from the right table
--Right joins : Return all rows from the right table and matched rows from the left
-- full joins : Return all rows where there is a match in one of the table
-- Self join : a table is joined with itself , often using alieases
-- Cross join : Returns the cartisian product of two (All Possible Combination)


-- Set Operators:
--Union
-- Intersect 
--Minus(not directly supported in Sql Server use Except)
select E.EmpName, D.DeptName
from Employee E Inner Join Department D on E.DeptId = D.Deptid;


insert into Department values (4,'Customer Support');

-- FOR LEFT JOIN
INSERT INTO Employee VALUES(106,'PARTH',30000,5,null,'2024-01-15');
Select E.EmpName, D.DeptName
from Employee E left join Department D ON E.DeptId= D.Deptid;



-- RIGHT JOIN
Select E.EmpName, D.DeptName
from Employee E RIGHT join Department D ON E.DeptId= D.Deptid;


-- FULL OUTER JOIN
select E.EmpName, D.DeptName
from Employee E
full outer join Department D on E.DeptId= D.Deptid;

-- Self join : a table is joinded with its self , of the ujsing aliases.

select E1.EmpName as Employee, E2.EmpName as Manager
from Employee E1  left join Employee E2 on E1.ManagerId= E2.EmpId;

-- Cross join 
Select Empname, DeptName  from Employee cross join Department;

select EmpName from Employee
union
select DeptName from Department;

select Empid , Empname from Employee;
--intersection: Return only the columns 

select DeptId from Employee
intersect 
select Deptid from Department;
select Deptid from Employee;

-- minus
select Deptid from Department
Except
select DeptId from Employee;
 

 

 
 --lets create a stored procedure for getting employee details 
 alter procedure UpdateEmployeeDetails
 @Empid int , @NewSalary decimal(10,2) output
 as 
 begin 
 update Employee set salary = @NewSalary
 where Empid = @Empid;
 select * from Employee;
 end;

 execute UpdateEmployeeDetails  104,8000;
 
alter procedure Checksalry
 @Empid int 
 as 
 begin 
 declare @salary decimal (10,2)
 select @salary = salary from Employee where Empid= @Empid;
 if @salary >55000
 print'high earner';
 else
 print'low Earner';
 end;
 
 execute Checksalry 103;
 