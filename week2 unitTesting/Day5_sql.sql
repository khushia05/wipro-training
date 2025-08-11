Begin Try
	 Begin transaction;
	 Update Employee Set Salary = salary + 5000 where DeptId= 1;
	 commit
End Try

Begin catch
	 rollback;
	 print 'Error ouccured';
end catch
select * from Employee where DeptId = 1;

CREATE FUNCTION GetYEARofJoining(
    @EmpID INT
)
RETURNS INT
AS
BEGIN 
    DECLARE @year INT;

    SELECT @year = YEAR(DateOfJoining)
    FROM Employee
    WHERE EmpID = @EmpID;

    RETURN @year;
END;
SELECT EmpName, dbo.GetYEARofJoining(101) AS JoiningYear FROM Employee;
select * from Employee;
 


 create function getemployeebyDept(@DeptId int)
 returns Table
 as 
 return
 (
    select EmpId, EmpName, salary  from Employee where DeptId= @DeptId
 )

 select * from dbo.getemployeebyDept(1);

 create procedure PrintEmployeeJoiningYear
 @EmpId int
 as
 begin 
 declare @Year int;
 -- calling funtion inside the fucntion

 set @Year = dbo.GetYEARofJoining(@EmpId);
 print 'joind on  ' + cast(@Year as varchar);
 end;

 execute PrintEmployeeJoiningYear 101;