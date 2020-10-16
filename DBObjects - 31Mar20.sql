USE [RigelonicHR]
GO
/****** Object:  StoredProcedure [dbo].[GeneratePayroll]    Script Date: 3/31/2020 5:49:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[GeneratePayroll]
@PayrollCycleID int,
@IsFinal int = 0,
@SingleUserID int = null

as
set nocount on

--declare @PayrollCycleID int
--select @PayrollCycleID = 1

declare @StartDate datetime, @EndDate datetime
select @StartDate = CAST(month as varchar(5)) + '/01/' + CAST(year as varchar(5)) from PayrollCycle (nolock) where id = @PayrollCycleID
select @EndDate = DATEADD(DD, -1, DATEADD(MM, 1, @StartDate))
--select @StartDate,@EndDate

declare @tempDate datetime
select @tempDate = @StartDate
declare @tempDays table(id int identity(1,1), Date datetime)
while(@tempDate <= @EndDate)
begin
	insert into @tempDays
	select @tempDate

	select @tempDate = DATEADD(dd, 1, @tempDate)
end


declare @tempUsers table(id int identity(1,1), UserID int, FullName varchar(100), SalaryTypeID int, Salary decimal(9,2))
insert into @tempUsers
select ID, FirstName, SalaryTypeID,Salary from [User] where IsActive = 1 and ID = ISNULL(@SingleUserID,ID) and SalaryTypeID in (1,3) and isnull(Salary,0) > 0

--select * from @tempUsers

declare @tempPayrollDetail table(id int identity(1,1), UserID int, PayrollPolicyID int, Amount Decimal(22,15), IsDeduction bit)
declare @userID int, @salaryTypeID int, @Salary decimal(9,2), @UnitSalary decimal(22,15)

declare @index int, @count int
select @index = 1, @count = COUNT(1) from @tempUsers

while(@index <= @count)
begin

	select @userID = UserID, @salaryTypeID = SalaryTypeID, @Salary = Salary from @tempUsers where id = @index

	declare @tempAttendanceStatus table(id int identity(1,1), UserID int, Date datetime, IsQuarterDay bit, IsHalfDay bit, IsFullDay bit, OverTimeMinutes int)
	insert into @tempAttendanceStatus
	select a.UserID,a.Date,s.IsQuarterDay,s.IsHalfDay,s.IsFullDay,s.OverTimeMinutes
	FROM AttendanceStatus s (NOLOCK)
	inner join Attendance a (nolock) on a.ID = s.AttendanceID
	where a.Date between @StartDate and @EndDate
	and a.UserId = @userID
	and a.IsActive = 1
	and s.IsActive = 1

	declare @PayrollID int
		
	--Monthly Salary
	if(@salaryTypeID = 1)
	begin
		--print 'Monthly - ' + cast(@userID as varchar)
		
		set @UnitSalary = @Salary / (DATEDIFF(DD, @StartDate, @EndDate) + CAST(1 as decimal(22,15)))

		--Quarter Day
		insert into @tempPayrollDetail
		select @userID, p.id, @UnitSalary * p.value, v.IsDeduction
		from PayrollPolicy p (nolock)
		inner join @tempAttendanceStatus a on a.Date between p.EffectiveDate and ISNULL(p.RetiredDate,a.Date)
		inner join PayrollVariable v (nolock) on v.ID = p.PayrollVariableID
		where p.payrollvariableid = 1
		and a.IsQuarterDay = 1

		--Half Day
		insert into @tempPayrollDetail
		select @userID, p.id, @UnitSalary * p.value, v.IsDeduction
		from PayrollPolicy p (nolock)
		inner join @tempAttendanceStatus a on a.Date between p.EffectiveDate and ISNULL(p.RetiredDate,a.Date)
		inner join PayrollVariable v (nolock) on v.ID = p.PayrollVariableID
		where p.payrollvariableid = 2
		and a.IsHalfDay = 1

		--Full Day
		insert into @tempPayrollDetail
		select @userID, p.id, @UnitSalary * p.value, v.IsDeduction
		from PayrollPolicy p (nolock)
		inner join @tempAttendanceStatus a on a.Date between p.EffectiveDate and ISNULL(p.RetiredDate,a.Date)
		inner join PayrollVariable v (nolock) on v.ID = p.PayrollVariableID
		where p.payrollvariableid = 3
		and a.IsFullDay = 1

		--Absent
		insert into @tempPayrollDetail
		select @userID, p.id, @UnitSalary * p.value, v.IsDeduction
		from PayrollPolicy p (nolock)
		inner join @tempDays a on a.Date between p.EffectiveDate and ISNULL(p.RetiredDate,a.Date)
		inner join PayrollVariable v (nolock) on v.ID = p.PayrollVariableID
		where p.payrollvariableid = 4
		and a.Date not in (select date from @tempAttendanceStatus)

		--Overtime
		insert into @tempPayrollDetail
		select @userID, p.id, @UnitSalary * (p.value/100.00) * (a.OverTimeMinutes/60.00), v.IsDeduction
		from PayrollPolicy p (nolock)
		inner join @tempAttendanceStatus a on a.Date between p.EffectiveDate and ISNULL(p.RetiredDate,a.Date)
		inner join PayrollVariable v (nolock) on v.ID = p.PayrollVariableID
		where p.payrollvariableid = 8
		and a.OverTimeMinutes > 0

		--Payroll Entry
		if(@IsFinal = 0)
		begin
			if exists(select top 1 1 from Payroll(nolock) where PayrollCycleID = @PayrollCycleID and UserID = @userID and IsActive = 0)
			begin
				select top 1 @PayrollID = ID from Payroll(nolock) where PayrollCycleID = @PayrollCycleID and UserID = @userID and IsActive = 0 order by ID desc
				
				update Payroll 
				set Salary = @Salary,
				Addition = isnull((select SUM(Amount) from @tempPayrollDetail where IsDeduction = 0),0),
				Deduction = isnull((select SUM(Amount) from @tempPayrollDetail where IsDeduction = 1),0),
				UpdateDate = GETDATE()
				where ID = @PayrollID
				
				update Payroll set NetSalary = Salary + Addition - Deduction where ID = @PayrollID
				
				delete from PayrollDetail where PayrollID = @PayrollID
				
				insert into PayrollDetail(PayrollID, PayrollPolicyID, Amount)
				select @PayrollID, PayrollPolicyID, Amount
				from @tempPayrollDetail
			end
			else
			begin
				insert into Payroll(PayrollCycleID, UserID, Salary, Addition, Deduction, IsActive)
				select @PayrollCycleID, @userID, @Salary,
				isnull((select SUM(Amount) from @tempPayrollDetail where IsDeduction = 0),0),
				isnull((select SUM(Amount) from @tempPayrollDetail where IsDeduction = 1),0),
				0
				
				SELECT @PayrollID = @@IDENTITY

				update Payroll set NetSalary = Salary + Addition - Deduction where ID = @PayrollID
				
				insert into PayrollDetail(PayrollID, PayrollPolicyID, Amount)
				select @PayrollID, PayrollPolicyID, Amount
				from @tempPayrollDetail
			end
		end
		else
		begin
			insert into Payroll(PayrollCycleID, UserID, Salary, Addition, Deduction)
			select @PayrollCycleID, @userID, @Salary,
			isnull((select SUM(Amount) from @tempPayrollDetail where IsDeduction = 0),0),
			isnull((select SUM(Amount) from @tempPayrollDetail where IsDeduction = 1),0)
			
			SELECT @PayrollID = @@IDENTITY

			update Payroll set NetSalary = Salary + Addition - Deduction where ID = @PayrollID
			
			insert into PayrollDetail(PayrollID, PayrollPolicyID, Amount)
			select @PayrollID, PayrollPolicyID, Amount
			from @tempPayrollDetail
		end
	end

	--Daily Salary
	if(@salaryTypeID = 3)
	begin
		--print 'Daily - ' + cast(@userID as varchar)

		--Quarter Day
		insert into @tempPayrollDetail
		select @userID, p.id, @Salary * p.value, v.IsDeduction
		from PayrollPolicy p (nolock)
		inner join @tempAttendanceStatus a on a.Date between p.EffectiveDate and ISNULL(p.RetiredDate,a.Date)
		inner join PayrollVariable v (nolock) on v.ID = p.PayrollVariableID
		where p.payrollvariableid = 1
		and a.IsQuarterDay = 1

		--Half Day
		insert into @tempPayrollDetail
		select @userID, p.id, @Salary * p.value, v.IsDeduction
		from PayrollPolicy p (nolock)
		inner join @tempAttendanceStatus a on a.Date between p.EffectiveDate and ISNULL(p.RetiredDate,a.Date)
		inner join PayrollVariable v (nolock) on v.ID = p.PayrollVariableID
		where p.payrollvariableid = 2
		and a.IsHalfDay = 1

		--Full Day
		insert into @tempPayrollDetail
		select @userID, p.id, @Salary * p.value, v.IsDeduction
		from PayrollPolicy p (nolock)
		inner join @tempAttendanceStatus a on a.Date between p.EffectiveDate and ISNULL(p.RetiredDate,a.Date)
		inner join PayrollVariable v (nolock) on v.ID = p.PayrollVariableID
		where p.payrollvariableid = 3
		and a.IsFullDay = 1

		--Absent
		insert into @tempPayrollDetail
		select @userID, p.id, @Salary * p.value, v.IsDeduction
		from PayrollPolicy p (nolock)
		inner join @tempDays a on a.Date between p.EffectiveDate and ISNULL(p.RetiredDate,a.Date)
		inner join PayrollVariable v (nolock) on v.ID = p.PayrollVariableID
		where p.payrollvariableid = 4
		and a.Date not in (select date from @tempAttendanceStatus)

		--Overtime
		insert into @tempPayrollDetail
		select @userID, p.id, @Salary * (p.value / 100.00) * (a.OverTimeMinutes/60.00), v.IsDeduction
		from PayrollPolicy p (nolock)
		inner join @tempAttendanceStatus a on a.Date between p.EffectiveDate and ISNULL(p.RetiredDate,a.Date)
		inner join PayrollVariable v (nolock) on v.ID = p.PayrollVariableID
		where p.payrollvariableid = 8
		and a.OverTimeMinutes > 0

		--Payroll Entry
		if(@IsFinal = 0)
		begin
			if exists(select top 1 1 from Payroll(nolock) where PayrollCycleID = @PayrollCycleID and UserID = @userID and IsActive = 0)
			begin
				select top 1 @PayrollID = ID from Payroll(nolock) where PayrollCycleID = @PayrollCycleID and UserID = @userID and IsActive = 0 order by ID desc
				
				update Payroll 
				set Salary = @Salary * (DATEDIFF(DD, @StartDate, @EndDate) + 1),
				Addition = isnull((select SUM(Amount) from @tempPayrollDetail where IsDeduction = 0),0),
				Deduction = isnull((select SUM(Amount) from @tempPayrollDetail where IsDeduction = 1),0),
				UpdateDate = GETDATE()
				where ID = @PayrollID
				
				update Payroll set NetSalary = Salary + Addition - Deduction where ID = @PayrollID
				
				delete from PayrollDetail where PayrollID = @PayrollID
				
				insert into PayrollDetail(PayrollID, PayrollPolicyID, Amount)
				select @PayrollID, PayrollPolicyID, Amount
				from @tempPayrollDetail
			end
			else
			begin
				insert into Payroll(PayrollCycleID, UserID, Salary, Addition, Deduction, IsActive)
				select @PayrollCycleID, @userID, @Salary * (DATEDIFF(DD, @StartDate, @EndDate) + 1),
				isnull((select SUM(Amount) from @tempPayrollDetail where IsDeduction = 0),0),
				isnull((select SUM(Amount) from @tempPayrollDetail where IsDeduction = 1),0),
				0
				
				SELECT @PayrollID = @@IDENTITY

				update Payroll set NetSalary = Salary + Addition - Deduction where ID = @PayrollID
				
				insert into PayrollDetail(PayrollID, PayrollPolicyID, Amount)
				select @PayrollID, PayrollPolicyID, Amount
				from @tempPayrollDetail
			end
		end
		else
		begin
			insert into Payroll(PayrollCycleID, UserID, Salary, Addition, Deduction)
			select @PayrollCycleID, @userID, @Salary * (DATEDIFF(DD, @StartDate, @EndDate) + 1),
			isnull((select SUM(Amount) from @tempPayrollDetail where IsDeduction = 0),0),
			isnull((select SUM(Amount) from @tempPayrollDetail where IsDeduction = 1),0)
			
			SELECT @PayrollID = @@IDENTITY

			update Payroll set NetSalary = Salary + Addition - Deduction where ID = @PayrollID
			
			insert into PayrollDetail(PayrollID, PayrollPolicyID, Amount)
			select @PayrollID, PayrollPolicyID, Amount
			from @tempPayrollDetail
		end
	end

	delete from @tempAttendanceStatus
	delete from @tempPayrollDetail

	set @index = @index + 1
end
set nocount off

GO
/****** Object:  StoredProcedure [dbo].[insert_manualAttendance]    Script Date: 3/31/2020 5:49:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[insert_manualAttendance]
@uid int,
@dt datetime,
@offday bit=0
as
begin
	declare @atid int
	set @atid=0
	if Not exists (select top 1 1 from Attendance where UserID=@uid and [Date]=@dt)
	begin
		insert into Attendance(UserID,[Date]) Values(@uid,@dt)
		set @atid=SCOPE_IDENTITY()
		
		insert into AttendanceDetail(AttendanceID,AttendanceTypeID,StartDate,EndDate) Values(@atid, 1, dateadd(hour,9,@dt), DATEADD(hour,16,@dt))

		insert into AttendanceStatus(AttendanceID,IsShiftOffDay,IsHoliday,IsLeaveDay,IsQuarterDay,IsHalfDay,IsFullDay,IsLate,IsEarly,LateMinutes,EarlyMinutes,WorkingMinutes,TotalMinutes,OverTimeMinutes)	
		values(@atid,@offday,0,0,0,0,0,0,0,0,0,7,7,0)
	end
end

GO
/****** Object:  StoredProcedure [dbo].[InsertPreAttendance]    Script Date: 3/31/2020 5:49:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[InsertPreAttendance]  
@AttendanceDate AS DATETIME = null  
AS  
BEGIN  
 DECLARE @DateStart DATETIME=null, @DateEnd DATETIME=null    
 IF @AttendanceDate IS NULL    
 BEGIN    
  SELECT @DateStart=CAST(Value AS DATETIME) FROM Configuration WHERE [Name]='PreAttendance'    
  SET @DateStart=DATEADD(hour,1,@DateStart)    
  SET @DateEnd=GETDATE()    
  SET @DateEnd=convert(varchar,@DateEnd,101)+ ' '+ CAST(DATEPART(hour,@DateEnd) AS VARCHAR(5))+':00:00.000'    
    
  WHILE @DateStart<=@DateEnd    
  BEGIN    
   EXEC [InsertPreAttendance] @DateStart    
   --PRINT 'EXEC [InsertPreAttendance] '''+ CAST(@DateStart AS VARCHAR(100))+''''    
   SET @DateStart=DATEADD(hour,1,@DateStart)    
  END    
  RETURN    
 END    
    
 -- START Attendance Table Data Generation    
 DECLARE @tblAttendance AS TABLE(ID INT IDENTITY,UserID INT, ShiftID INT, StartHour varchar(10), EndHour varchar(10),Diff INT)    
 INSERT INTO @tblAttendance (UserID, ShiftID,StartHour,EndHour,Diff)    
 SELECT u.ID,us.ShiftID,s.StartHour,s.EndHour,    
 DATEDIFF(hour,CAST(CONVERT(varchar,@AttendanceDate,101) + ' ' +s.StartHour AS DATETIME),@AttendanceDate)    
 FROM [User] u    
 INNER JOIN UserShift us ON u.ID=us.UserID    
 INNER JOIN Shift s ON us.ShiftID=s.ID    
 WHERE CAST(@AttendanceDate AS DATE) BETWEEN us.EffectiveDate AND ISNULL(us.RetiredDate,CAST(@AttendanceDate AS DATE))    
 AND DATEDIFF(hour,CAST(CONVERT(varchar,@AttendanceDate,101) + ' ' +s.StartHour AS DATETIME),@AttendanceDate)=-1    
 AND u.IsActive=1 AND s.IsActive=1    
 AND NOT EXISTS (SELECT TOP 1 1 FROM Attendance a WHERE a.UserID=u.ID AND a.IsActive=1 AND a.Date=CAST(@AttendanceDate AS DATE))    
 -- END Attendance Table Data Generation    
    
 -- START Shift Off day Data Generation    
 DECLARE @tblShiftOffDay TABLE(ID INT IDENTITY, ShiftID INT, OffDayOfWeek INT)    
 INSERT INTO @tblShiftOffDay (ShiftID,OffDayOfWeek )    
 SELECT sod.ShiftID,sod.OffDayOfWeek    
 FROM Shift s INNER JOIN ShiftOffDay sod ON s.ID=sod.ShiftID    
 WHERE s.IsActive=1    
 AND CAST(@AttendanceDate AS DATE) BETWEEN sod.EffectiveDate AND ISNULL(sod.RetiredDate,CAST(@AttendanceDate AS DATE))    
 -- END Shif Off day Data Generation    
    
 -- START Generated Data Selection FOR Testing    
 ----SELECT * from @tblAttendance    
 ----SELECT * from @tblShiftOffDay    
 -- STOP Generated Data Selection FOR Testing    
 --RETURN    
 DECLARE @index INT=0, @count INT=0, @UserID INT=0,@ShiftID INT=0, @AttendanceID INT=0    
 SELECT @count=COUNT(1) FROM @tblAttendance    
    
 WHILE(@index<@count)    
 BEGIN    
  SELECT @UserID=UserID,@ShiftID=ShiftID FROM @tblAttendance WHERE ID=@index    
    
  IF @UserID !=0    
  BEGIN    
  --Previous Day time out missing
   IF EXISTS(SELECT TOP 1 1 
   FROM Attendance a (NOLOCK) 
   INNER JOIN AttendanceDetail ad (NOLOCK) ON a.ID = ad.AttendanceID
   WHERE a.UserID = @UserID
   AND a.IsActive = 1 and ad.IsActive = 1
   AND ad.AttendanceTypeID = 1
   AND StartDate IS NOT NULL AND EndDate IS NULL)
   BEGIN
	UPDATE s
	SET s.IsQuarterDay = 0,s.IsHalfDay = 0,s.IsFullDay = 1,s.IsEarly = 1,s.EarlyMinutes = 0,s.WorkingMinutes = 0,s.TotalMinutes = 0,s.UpdateDate=GETDATE(),s.UserIP='JOB'
	FROM AttendanceStatus s
	INNER JOIN Attendance a on a.ID = s.AttendanceID
	INNER JOIN AttendanceDetail ad on ad.AttendanceID = a.ID
	WHERE a.UserID = @UserID
	AND a.IsActive = 1 AND ad.IsActive = 1 AND s.IsActive = 1
	AND ad.AttendanceTypeID = 1
	AND ad.StartDate IS NOT NULL AND ad.EndDate IS NULL
	
	UPDATE ad
	SET ad.EndDate = ad.StartDate,ad.UpdateDate=GETDATE(),ad.UserIP='JOB'
	FROM AttendanceDetail ad
	INNER JOIN Attendance a on a.ID = ad.AttendanceID
	WHERE a.UserID = @UserID
	AND a.IsActive = 1 AND ad.IsActive = 1
	AND ad.AttendanceTypeID = 1
	AND ad.StartDate IS NOT NULL AND ad.EndDate IS NULL
	
   END
   
   INSERT INTO Attendance (UserID,Date,IsActive,CreationDate,UserIP)    
   VALUES (@UserID,CAST(@AttendanceDate AS Date),1,GETDATE(),'JOB')    
   SET @AttendanceID=SCOPE_IDENTITY()    
   IF(@AttendanceID IS NOT NULL AND @AttendanceID >0)    
   BEGIN    
    --IF Off day    
    IF EXISTS(    
    SELECT TOP 1 1    
    FROM @tblShiftOffDay sod    
    WHERE sod.ShiftID=@ShiftID AND DATEPART(dw,@AttendanceDate)=sod.OffDayOfWeek)    
    AND    
    NOT EXISTS (    
    SELECT TOP 1 1    
    FROM AttendanceStatus ats    
    WHERE ats.AttendanceID=@AttendanceID AND ats.IsActive=1)    
    BEGIN    
     INSERT INTO AttendanceStatus(AttendanceID,IsShiftOffDay,IsActive,CreationDate,UserIP)    
     VALUES(@AttendanceID,1,1,GETDATE(),'JOB')    
    END    
    IF EXISTS(SELECT TOP 1 1  
    FROM Holiday (NOLOCK)  
    WHERE CAST([Date] AS DATE) = CAST(@AttendanceDate AS DATE) AND IsActive = 1)  
    BEGIN  
      IF EXISTS(SELECT TOP 1 1 FROM AttendanceStatus (NOLOCK) WHERE AttendanceID=@AttendanceID AND IsActive=1)  
      BEGIN  
        UPDATE AttendanceStatus SET IsHoliday = 1 WHERE AttendanceID=@AttendanceID AND IsActive=1  
      END  
      ELSE  
      BEGIN  
        INSERT INTO AttendanceStatus(AttendanceID,IsHoliday,IsActive,CreationDate,UserIP)    
        VALUES(@AttendanceID,1,1,GETDATE(),'JOB')    
      END  
    END  
   END    
  END    
  SET @index=@index+1    
 END    
 UPDATE Configuration SET Value=CAST(@AttendanceDate AS VARCHAR(100)) WHERE [Name]='PreAttendance'    
END

GO
/****** Object:  StoredProcedure [dbo].[SELECT_Attendance]    Script Date: 3/31/2020 5:49:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*    
EXEC SELECT_Attendance @StartDate='12/07/2016', @EndDate='12/07/2016',@UserId=null,@BranchId=1    
*/    
CREATE PROC [dbo].[SELECT_Attendance]    
@StartDate DATETIME,    
@EndDate DATETIME,    
@UserId INT=NULL,    
@BranchID INT=NULL    
AS    
BEGIN    
    
SELECT a.Id AS AttendanceId, ISNULL(a.[Date],@StartDate) AS [Date], u.ID AS UserId, u.FirstName, u.UserTypeID,u.Designation,u.ImagePath, u.AccountNumber, u.SalaryTypeID,st.Name AS SalaryTypeName,    
d.ID AS DepartmentId,ud.EffectiveDate AS DEffectiveDate, ud.RetiredDate AS DRetiredDate,d.Name AS DepartmentName,bd.BranchID, b.Name AS BranchName,    
s.ID AS ShiftId, us.ShiftId,us.EffectiveDate AS SEffectiveDate, us.RetiredDate AS SRetiredDate, s.Name AS ShiftName,    
ats.Id AS AttendanceStatusId, ats.IsShiftOffDay, ats.IsHoliday, ats.IsLeaveDay, ats.IsQuarterDay, ats.IsHalfDay, ats.IsFullDay, ats.IsLate,    
ats.IsEarly,ats.LateMinutes,ats.EarlyMinutes, ats.WorkingMinutes, ats.TotalMinutes, ats.OverTimeMinutes, ats.Reason,ats.IsApproved,ats.Remarks, ats.BreakType,   
XADTI.Id AS XADTIAttendanceDetailId,XADTI.AttendanceId AS XADTIAttendanceId, XADTI.StartDate AS XADTIStartDate, XADTI.EndDate AS XADTIEndDate,    
XADTO.Id AS XADTOAttendanceDetailId,XADTO.AttendanceId AS XADTOAttendanceId, XADTO.StartDate AS XADTOStartDate, XADTO.EndDate AS XADTOEndDate    
FROM Department d    
INNER JOIN UserDepartment ud ON ud.DepartmentId=d.Id    
INNER JOIN [User] u ON ud.UserId=u.Id    
LEFT JOIN SalaryType st ON u.SalaryTypeID=st.ID    
INNER JOIN BranchDepartment bd ON d.ID=bd.DepartmentID    
INNER JOIN Branch b ON bd.BranchID=b.ID    
INNER JOIN UserShift us ON us.UserId=u.ID    
INNER JOIN Shift s ON us.ShiftId=s.Id    
LEFT JOIN Attendance a ON a.UserID=u.ID AND a.[Date] BETWEEN @StartDate AND @EndDate AND a.IsActive=1    
LEFT JOIN AttendanceStatus ats ON a.Id=ats.AttendanceId AND ats.IsActive=1    
OUTER APPLY ( SELECT TOP 1 adTI.* FROM AttendanceDetail adTI WHERE a.Id=adTI.AttendanceId AND adTI.AttendanceTypeId=1 AND adTI.IsActive=1 ORDER BY adTI.StartDate) AS XADTI    
OUTER APPLY ( SELECT TOP 1 adTO.* FROM AttendanceDetail adTO WHERE a.Id=adTO.AttendanceId AND adTO.AttendanceTypeId=1 AND adTO.IsActive=1 ORDER BY adTO.EndDate DESC) AS XADTO    
WHERE us.RetiredDate IS NULL AND ud.RetiredDate IS NULL    
AND d.IsActive=1 AND b.IsActive=1    
AND (@UserId is null or u.id=@Userid)    
AND (@BranchID is null or b.id=@BranchID)    
    
ORDER BY a.[Date]    
END

GO
/****** Object:  StoredProcedure [dbo].[SELECT_AttendanceSummary]    Script Date: 3/31/2020 5:49:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
EXEC SELECT_AttendanceSummary @StartDate='11/01/2016', @EndDate='11/01/2016'
EXEC SELECT_AttendanceSummary @StartDate='11/04/2016', @EndDate='11/04/2016'
*/
CREATE PROC [dbo].[SELECT_AttendanceSummary]
@StartDate DATETIME,
@EndDate DATETIME,
@BranchID INT=NULL,
@DepartmentName VARCHAR(300)=NULL,
@SalaryTypeId INT=NULL,
@ShiftID INT=NULL
AS
BEGIN

SELECT 
bd.BranchID, b.Name AS BranchName, bd.DepartmentID, d.Name AS DepartmentName,
COUNT(1) AS TOTAL, COUNT(ats.Id) AS Present, 
COUNT(CASE WHEN ats.IsShiftOffDay IS NULL OR ats.IsShiftOffDay=0 THEN NULL ELSE 1 END) AS OffDay,
COUNT(CASE WHEN ats.IsLate IS NULL OR ats.IsLate=0 THEN NULL ELSE 1 END) AS Late,
COUNT(CASE WHEN ats.IsEarly IS NULL OR ats.IsEarly=0 THEN NULL ELSE 1 END) AS Early,
COUNT(CASE WHEN ats.OverTimeMinutes IS NULL OR ats.OverTimeMinutes<=0 THEN NULL ELSE 1 END) AS OverTime,
COUNT(CASE WHEN ats.IsFullDay IS NULL OR ats.IsFullDay=0 THEN NULL ELSE 1 END) AS FullDay


/*
a.Id AS AttendanceId, a.[Date], u.ID AS UserId, u.FirstName, u.UserTypeID,u.Designation, u.AccountNumber,
d.ID AS DepartmentId,ud.EffectiveDate AS DEffectiveDate, ud.RetiredDate AS DRetiredDate,
s.ID AS ShiftId, us.ShiftId,us.EffectiveDate AS SEffectiveDate, us.RetiredDate AS SRetiredDate, s.Name AS ShiftName,
ats.Id AS AttendanceStatusId, ats.IsShiftOffDay, ats.IsHoliday, ats.IsLeaveDay, ats.IsQuarterDay, ats.IsHalfDay, ats.IsFullDay, ats.IsLate,
ats.IsEarly,ats.LateMinutes,ats.EarlyMinutes, ats.WorkingMinutes, ats.TotalMinutes, ats.OverTimeMinutes, ats.Reason,ats.IsApproved,ats.Remarks,
XADTI.Id AS XADTIAttendanceDetailId,XADTI.AttendanceId AS XADTIAttendanceId, XADTI.StartDate AS XADTIStartDate, XADTI.EndDate AS XADTIEndDate,
XADTO.Id AS XADTOAttendanceDetailId,XADTO.AttendanceId AS XADTOAttendanceId, XADTO.StartDate AS XADTOStartDate, XADTO.EndDate AS XADTOEndDate
*/
FROM Department d
INNER JOIN UserDepartment ud ON ud.DepartmentId=d.Id 
INNER JOIN [User] u ON ud.UserId=u.Id
INNER JOIN BranchDepartment bd ON d.ID=bd.DepartmentID 
INNER JOIN Branch b ON bd.BranchID=b.ID 
INNER JOIN UserShift us ON us.UserId=u.ID 
INNER JOIN Shift s ON us.ShiftId=s.Id 
LEFT JOIN Attendance a ON a.UserID=u.ID AND a.[Date] BETWEEN @StartDate AND @EndDate AND a.IsActive=1
LEFT JOIN AttendanceStatus ats ON a.Id=ats.AttendanceId AND ats.IsActive=1
OUTER APPLY ( SELECT TOP 1 adTI.* FROM AttendanceDetail adTI WHERE a.Id=adTI.AttendanceId AND adTI.AttendanceTypeId=1 AND adTI.IsActive=1 ORDER BY adTI.StartDate) AS XADTI
OUTER APPLY ( SELECT TOP 1 adTO.* FROM AttendanceDetail adTO WHERE a.Id=adTO.AttendanceId AND adTO.AttendanceTypeId=1 AND adTO.IsActive=1 ORDER BY adTO.EndDate DESC) AS XADTO
WHERE (@DepartmentName IS NULL OR d.Name=@DepartmentName)
AND (@BranchID IS NULL OR bd.BranchID =@BranchID)
AND (@ShiftID IS NULL OR us.ShiftID =@ShiftID)
AND (@SalaryTypeId IS NULL OR u.SalaryTypeId =@SalaryTypeId)
AND us.RetiredDate IS NULL  AND ud.RetiredDate IS NULL
AND d.IsActive=1 AND b.IsActive=1
GROUP BY bd.BranchID, b.Name , bd.DepartmentID, d.Name 
ORDER BY BranchName,DepartmentName 
END


/*
SELECT 
bd.BranchID, b.Name AS BranchName, bd.DepartmentID, d.Name AS DepartmentName,
a.Id AS AttendanceId, a.[Date], u.ID AS UserId, u.FirstName, u.UserTypeID,u.Designation, u.AccountNumber,
d.ID AS DepartmentId,ud.EffectiveDate AS DEffectiveDate, ud.RetiredDate AS DRetiredDate,
s.ID AS ShiftId, us.ShiftId,us.EffectiveDate AS SEffectiveDate, us.RetiredDate AS SRetiredDate, s.Name AS ShiftName,
ats.Id AS AttendanceStatusId, ats.IsShiftOffDay, ats.IsHoliday, ats.IsLeaveDay, ats.IsQuarterDay, ats.IsHalfDay, ats.IsFullDay, ats.IsLate,
ats.IsEarly,ats.LateMinutes,ats.EarlyMinutes, ats.WorkingMinutes, ats.TotalMinutes, ats.OverTimeMinutes, ats.Reason,ats.IsApproved,ats.Remarks,
XADTI.Id AS XADTIAttendanceDetailId,XADTI.AttendanceId AS XADTIAttendanceId, XADTI.StartDate AS XADTIStartDate, XADTI.EndDate AS XADTIEndDate,
XADTO.Id AS XADTOAttendanceDetailId,XADTO.AttendanceId AS XADTOAttendanceId, XADTO.StartDate AS XADTOStartDate, XADTO.EndDate AS XADTOEndDate

FROM Department d
INNER JOIN UserDepartment ud ON ud.DepartmentId=d.Id 
INNER JOIN [User] u ON ud.UserId=u.Id
INNER JOIN BranchDepartment bd ON d.ID=bd.DepartmentID 
INNER JOIN Branch b ON bd.BranchID=b.ID 
INNER JOIN UserShift us ON us.UserId=u.ID 
INNER JOIN Shift s ON us.ShiftId=s.Id 
LEFT JOIN Attendance a ON a.UserID=u.ID AND a.[Date] BETWEEN '11/04/2016' AND '11/04/2016' AND a.IsActive=1
LEFT JOIN AttendanceStatus ats ON a.Id=ats.AttendanceId AND ats.IsActive=1
OUTER APPLY ( SELECT TOP 1 adTI.* FROM AttendanceDetail adTI WHERE a.Id=adTI.AttendanceId AND adTI.AttendanceTypeId=1 AND adTI.IsActive=1 ORDER BY adTI.StartDate) AS XADTI
OUTER APPLY ( SELECT TOP 1 adTO.* FROM AttendanceDetail adTO WHERE a.Id=adTO.AttendanceId AND adTO.AttendanceTypeId=1 AND adTO.IsActive=1 ORDER BY adTO.EndDate DESC) AS XADTO
WHERE us.RetiredDate IS NULL  AND ud.RetiredDate IS NULL
AND d.IsActive=1 AND b.IsActive=1
ORDER BY BranchName,DepartmentName 
*/

GO
/****** Object:  StoredProcedure [dbo].[SELECT_MonthlyAttendanceSummary]    Script Date: 3/31/2020 5:49:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
SELECT_MonthlyAttendanceSummary '11/01/2016','11/30/2016'

*/
CREATE PROC [dbo].[SELECT_MonthlyAttendanceSummary]
@StartDate DATETIME,  
@EndDate DATETIME,
@UserID INT=NULL,
@UserName VARCHAR(300)=NULL,  
@BranchID INT=NULL,  
@DepartmentName VARCHAR(300)=NULL,  
@SalaryTypeId INT=NULL,  
@ShiftID INT=NULL
AS
BEGIN

SELECT   
ud.UserId, dbo.GetName(u.FirstName,u.MiddleName,u.LastName) AS UserName,u.ImagePath, u.Designation, bd.DepartmentID,d.Name AS DepartmentName,u.SalaryTypeId, st.Name AS SalaryType,
bd.BranchID, b.Name AS BranchName, us.ShiftId, s.Name AS ShiftName,  
COUNT(1) AS TOTAL, COUNT(ats.Id) AS Present,   
COUNT(CASE WHEN ats.IsShiftOffDay IS NULL OR ats.IsShiftOffDay=0 THEN NULL ELSE 1 END) AS OffDay,  
COUNT(CASE WHEN ats.IsLate IS NULL OR ats.IsLate=0 THEN NULL ELSE 1 END) AS Late,  
COUNT(CASE WHEN ats.IsEarly IS NULL OR ats.IsEarly=0 THEN NULL ELSE 1 END) AS Early,  
COUNT(CASE WHEN ats.OverTimeMinutes IS NULL OR ats.OverTimeMinutes<=0 THEN NULL ELSE 1 END) AS OverTime,  
COUNT(CASE WHEN ats.IsFullDay IS NULL OR ats.IsFullDay=0 THEN NULL ELSE 1 END) AS FullDay  
FROM Department d  
INNER JOIN UserDepartment ud ON ud.DepartmentId=d.Id   
INNER JOIN [User] u ON ud.UserId=u.Id  
INNER JOIN BranchDepartment bd ON d.ID=bd.DepartmentID   
INNER JOIN Branch b ON bd.BranchID=b.ID   
INNER JOIN UserShift us ON us.UserId=u.ID   
INNER JOIN Shift s ON us.ShiftId=s.Id   
LEFT JOIN SalaryType st ON st.Id=u.SalaryTypeId
LEFT JOIN Attendance a ON a.UserID=u.ID AND a.[Date] BETWEEN @StartDate AND @EndDate AND a.IsActive=1  
LEFT JOIN AttendanceStatus ats ON a.Id=ats.AttendanceId AND ats.IsActive=1  
OUTER APPLY ( SELECT TOP 1 adTI.* FROM AttendanceDetail adTI WHERE a.Id=adTI.AttendanceId AND adTI.AttendanceTypeId=1 AND adTI.IsActive=1 ORDER BY adTI.StartDate) AS XADTI  
OUTER APPLY ( SELECT TOP 1 adTO.* FROM AttendanceDetail adTO WHERE a.Id=adTO.AttendanceId AND adTO.AttendanceTypeId=1 AND adTO.IsActive=1 ORDER BY adTO.EndDate DESC) AS XADTO  
WHERE (@UserID IS NULL OR u.ID=@UserID)  
AND (@UserName IS NULL OR dbo.GetName(u.FirstName,u.MiddleName,u.LastName) LIKE '%'+@UserName+'%')  
AND (@DepartmentName IS NULL OR d.Name=@DepartmentName)  
AND (@BranchID IS NULL OR bd.BranchID =@BranchID)  
AND (@ShiftID IS NULL OR us.ShiftID =@ShiftID)  
AND (@SalaryTypeId IS NULL OR u.SalaryTypeId =@SalaryTypeId)  
AND us.RetiredDate IS NULL  AND ud.RetiredDate IS NULL  
AND d.IsActive=1 AND b.IsActive=1  
GROUP BY ud.UserId, dbo.GetName(u.FirstName,u.MiddleName,u.LastName),u.ImagePath, u.Designation, bd.DepartmentID,d.Name,u.SalaryTypeId, st.Name ,bd.BranchID, b.Name , us.ShiftId, s.Name  
ORDER BY 1,2   
END

GO
/****** Object:  Table [dbo].[Attendance]    Script Date: 3/31/2020 5:49:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Attendance](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[Date] [datetime] NULL,
	[IsActive] [bit] NULL,
	[CreationDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateBy] [int] NULL,
	[UserIP] [varchar](20) NULL,
	[DateTimeOut] [datetime] NULL,
	[DateTimeIn] [datetime] NULL,
 CONSTRAINT [PK_Attendance] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AttendanceDetail]    Script Date: 3/31/2020 5:49:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AttendanceDetail](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[AttendanceID] [int] NULL,
	[AttendanceTypeID] [int] NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[IsActive] [bit] NULL,
	[CreationDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateBy] [int] NULL,
	[UserIP] [varchar](20) NULL,
 CONSTRAINT [PK_AttendanceDetail] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AttendancePolicy]    Script Date: 3/31/2020 5:49:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AttendancePolicy](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ShiftID] [int] NULL,
	[AttendanceVariableID] [int] NULL,
	[Hours] [decimal](18, 2) NULL,
	[Description] [varchar](2000) NULL,
	[EffectiveDate] [datetime] NULL,
	[RetiredDate] [datetime] NULL,
	[CreationDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateBy] [int] NULL,
	[UserIP] [varchar](20) NULL,
 CONSTRAINT [PK_AttendancePolicy] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AttendanceStatus]    Script Date: 3/31/2020 5:49:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AttendanceStatus](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[AttendanceID] [int] NULL,
	[IsShiftOffDay] [bit] NULL,
	[IsHoliday] [bit] NULL,
	[IsLeaveDay] [bit] NULL,
	[IsQuarterDay] [bit] NULL,
	[IsHalfDay] [bit] NULL,
	[IsFullDay] [bit] NULL,
	[Reason] [varchar](2000) NULL,
	[IsApproved] [bit] NULL,
	[Remarks] [varchar](2000) NULL,
	[ActionBy] [int] NULL,
	[ActionDate] [datetime] NULL,
	[IsActive] [bit] NULL,
	[CreationDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateBy] [int] NULL,
	[UserIP] [varchar](20) NULL,
	[IsLate] [bit] NULL,
	[IsEarly] [bit] NULL,
	[LateMinutes] [int] NULL,
	[EarlyMinutes] [int] NULL,
	[WorkingMinutes] [int] NULL,
	[TotalMinutes] [int] NULL,
	[OverTimeMinutes] [int] NULL,
	[BreakType] [varchar](50) NULL,
 CONSTRAINT [PK_AttendanceStatus] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AttendanceType]    Script Date: 3/31/2020 5:49:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AttendanceType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
	[IsActive] [bit] NULL,
	[CreationDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateBy] [int] NULL,
	[UserIP] [varchar](20) NULL,
 CONSTRAINT [PK_AttendanceType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AttendanceVariable]    Script Date: 3/31/2020 5:49:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AttendanceVariable](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
	[Description] [varchar](1000) NULL,
	[IsActive] [bit] NULL,
	[CreationDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateBy] [int] NULL,
	[UserIP] [varchar](20) NULL,
 CONSTRAINT [PK_AttendanceVariable] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Branch]    Script Date: 3/31/2020 5:49:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Branch](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](500) NULL,
	[AddressLine] [nvarchar](1000) NULL,
	[CityID] [int] NULL,
	[StateID] [int] NULL,
	[CountryID] [int] NULL,
	[ZipCode] [nvarchar](20) NULL,
	[PhoneNumber] [nvarchar](50) NULL,
	[GUID] [varchar](500) NULL,
	[CreatedDate] [datetime] NULL,
	[UpdationDate] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[UserIP] [nvarchar](50) NULL,
	[IsActive] [bit] NULL,
 CONSTRAINT [PK__Branch__3214EC2773BA3083] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BranchDepartment]    Script Date: 3/31/2020 5:49:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BranchDepartment](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BranchID] [int] NULL,
	[DepartmentID] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[UserIP] [nvarchar](50) NULL,
	[IsActive] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[BranchShift]    Script Date: 3/31/2020 5:49:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BranchShift](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BranchID] [int] NULL,
	[ShiftID] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedDate] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[UserIP] [nvarchar](50) NULL,
	[IsActive] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[City]    Script Date: 3/31/2020 5:49:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[City](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
	[StateID] [int] NULL,
	[IsActive] [bit] NULL,
	[CreationDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateBy] [int] NULL,
	[UserIP] [varchar](20) NULL,
 CONSTRAINT [PK_City] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Configuration]    Script Date: 3/31/2020 5:49:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Configuration](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[Type] [varchar](50) NULL,
	[Value] [varchar](50) NULL,
	[IsActive] [bit] NULL,
	[CreationDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateBy] [int] NULL,
	[UserIP] [varchar](20) NULL,
 CONSTRAINT [PK_Configuration] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ContactType]    Script Date: 3/31/2020 5:49:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ContactType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[IsActive] [bit] NULL,
	[CreationDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateBy] [int] NULL,
	[UserIP] [varchar](20) NULL,
 CONSTRAINT [PK_ContactType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Country]    Script Date: 3/31/2020 5:49:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Country](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
	[IsActive] [bit] NULL,
	[CreationDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateBy] [int] NULL,
	[UserIP] [varchar](20) NULL,
 CONSTRAINT [PK_Country] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Department]    Script Date: 3/31/2020 5:49:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Department](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](200) NULL,
	[Description] [varchar](500) NULL,
	[IsActive] [bit] NULL,
	[CreationDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateBy] [int] NULL,
	[UserIP] [varchar](20) NULL,
 CONSTRAINT [PK_Department] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Device]    Script Date: 3/31/2020 5:49:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Device](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[MachineID] [int] NULL,
	[DeviceID] [int] NULL,
	[ConnectionTypeID] [int] NULL,
	[IPAddress] [varchar](50) NULL,
	[PortNumber] [int] NULL,
	[Password] [varchar](50) NULL,
	[ComPort] [int] NULL,
	[Baudrate] [bigint] NULL,
	[Status] [varchar](50) NULL,
	[StatusDescription] [varchar](1000) NULL,
	[BranchID] [int] NULL,
	[IsActive] [bit] NULL,
	[CreationDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateBy] [int] NULL,
	[UserIP] [varchar](20) NULL,
	[DeviceModalID] [int] NULL,
 CONSTRAINT [PK_Device] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DeviceModal]    Script Date: 3/31/2020 5:49:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DeviceModal](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DeviceModal] [varchar](100) NULL,
	[CreatedDate] [datetime] NULL,
	[UpdationDate] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[UserIP] [nvarchar](50) NULL,
	[IsActive] [bit] NULL,
 CONSTRAINT [PK_DeviceModal] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Gender]    Script Date: 3/31/2020 5:49:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Gender](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](10) NULL,
	[IsActive] [bit] NULL,
	[CreationDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateBy] [int] NULL,
	[UserIP] [varchar](20) NULL,
 CONSTRAINT [PK_Gender] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Holiday]    Script Date: 3/31/2020 5:49:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Holiday](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
	[Date] [datetime] NULL,
	[Detail] [varchar](200) NULL,
	[IsActive] [bit] NULL,
	[CreationDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateBy] [int] NULL,
	[UserIP] [varchar](20) NULL,
 CONSTRAINT [PK_Holiday] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Leave]    Script Date: 3/31/2020 5:49:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Leave](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[Date] [datetime] NULL,
	[Reason] [varchar](500) NULL,
	[LeaveTypeID] [int] NULL,
	[IsActive] [bit] NULL,
	[UpdateDate] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[UserIP] [varchar](20) NULL,
	[CreationDate] [datetime] NULL,
 CONSTRAINT [PK_Leave] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LeaveType]    Script Date: 3/31/2020 5:49:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LeaveType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
	[YearlyLeaves] [int] NULL,
	[PriorDays] [int] NULL,
	[MaxDays] [int] NULL,
	[IsActive] [bit] NULL,
	[CreationDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateBy] [int] NULL,
	[UserIP] [varchar](20) NULL,
 CONSTRAINT [PK_LeaveType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[OG_LeaveType]    Script Date: 3/31/2020 5:49:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[OG_LeaveType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[IsActive] [bit] NULL,
	[CreationDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[UserIP] [varchar](20) NULL,
 CONSTRAINT [PK_OG_LeaveType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Payroll]    Script Date: 3/31/2020 5:49:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Payroll](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PayrollCycleID] [int] NULL,
	[UserID] [int] NULL,
	[Salary] [decimal](9, 2) NULL,
	[Addition] [decimal](9, 2) NULL,
	[Deduction] [decimal](9, 2) NULL,
	[NetSalary] [decimal](9, 2) NULL,
	[IsActive] [bit] NULL,
	[CreationDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateBy] [int] NULL,
	[UserIP] [varchar](20) NULL,
 CONSTRAINT [PK_Payroll] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayrollCycle]    Script Date: 3/31/2020 5:49:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PayrollCycle](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
	[Month] [int] NULL,
	[Year] [int] NULL,
	[IsActive] [bit] NULL,
	[CreationDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateBy] [int] NULL,
	[UserIP] [varchar](20) NULL,
 CONSTRAINT [PK_PayrollCycle] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayrollDetail]    Script Date: 3/31/2020 5:49:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PayrollDetail](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PayrollID] [int] NULL,
	[PayrollPolicyID] [int] NULL,
	[Amount] [decimal](9, 2) NULL,
	[IsActive] [bit] NULL,
	[CreationDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateBy] [int] NULL,
	[UserIP] [varchar](20) NULL,
 CONSTRAINT [PK_PayrollDetail] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayrollPolicy]    Script Date: 3/31/2020 5:49:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PayrollPolicy](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PayrollVariableID] [int] NULL,
	[IsUnit] [bit] NULL,
	[IsPercentage] [bit] NULL,
	[Value] [decimal](9, 2) NULL,
	[Description] [varchar](1000) NULL,
	[EffectiveDate] [datetime] NULL,
	[RetiredDate] [datetime] NULL,
	[CreationDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateBy] [int] NULL,
	[UserIP] [varchar](20) NULL,
 CONSTRAINT [PK_PayrollPolicy] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayrollVariable]    Script Date: 3/31/2020 5:49:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PayrollVariable](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
	[IsDeduction] [bit] NULL,
	[IsActive] [bit] NULL,
	[CreationDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateBy] [int] NULL,
	[UserIP] [varchar](20) NULL,
 CONSTRAINT [PK_PayrollVariable] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Religion]    Script Date: 3/31/2020 5:49:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Religion](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
	[IsActive] [bit] NULL,
	[CreationDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateBy] [int] NULL,
	[UserIP] [varchar](20) NULL,
 CONSTRAINT [PK_Religion] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SalaryType]    Script Date: 3/31/2020 5:49:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SalaryType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
	[IsActive] [bit] NULL,
	[CreationDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateBy] [int] NULL,
	[UserIP] [varchar](20) NULL,
 CONSTRAINT [PK_SalaryType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Shift]    Script Date: 3/31/2020 5:49:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Shift](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](200) NULL,
	[Description] [varchar](1000) NULL,
	[StartHour] [varchar](20) NULL,
	[EndHour] [varchar](20) NULL,
	[IsActive] [bit] NULL,
	[CreationDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateBy] [int] NULL,
	[UserIP] [varchar](20) NULL,
	[BreakHour] [decimal](18, 2) NULL,
 CONSTRAINT [PK_Shift] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ShiftOffDay]    Script Date: 3/31/2020 5:49:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ShiftOffDay](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ShiftID] [int] NULL,
	[OffDayOfWeek] [int] NULL,
	[EffectiveDate] [datetime] NULL,
	[RetiredDate] [datetime] NULL,
	[CreationDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateBy] [int] NULL,
	[UserIP] [varchar](20) NULL,
 CONSTRAINT [PK_ShiftOffDay] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[State]    Script Date: 3/31/2020 5:49:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[State](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
	[CountryID] [int] NULL,
	[IsActive] [bit] NULL,
	[CreationDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateBy] [int] NULL,
	[UserIP] [varchar](20) NULL,
 CONSTRAINT [PK_State] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tmp]    Script Date: 3/31/2020 5:49:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tmp](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[empid] [int] NULL,
	[n] [varchar](500) NULL,
	[fn] [varchar](500) NULL,
	[d] [varchar](100) NULL,
	[a] [varchar](100) NULL,
	[dept] [varchar](500) NULL,
	[did] [int] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[User]    Script Date: 3/31/2020 5:49:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[User](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](200) NULL,
	[MiddleName] [varchar](200) NULL,
	[LastName] [varchar](200) NULL,
	[UserTypeID] [int] NULL,
	[GenderID] [int] NULL,
	[DateOfBirth] [datetime] NULL,
	[NICNo] [varchar](50) NULL,
	[ReligionID] [int] NULL,
	[Address1] [varchar](2000) NULL,
	[Address2] [varchar](2000) NULL,
	[ZipCode] [varchar](20) NULL,
	[CountryID] [int] NULL,
	[CityID] [int] NULL,
	[StateID] [int] NULL,
	[AcadmicQualification] [varchar](1000) NULL,
	[Designation] [varchar](200) NULL,
	[Salary] [decimal](9, 2) NULL,
	[LoginID] [varchar](50) NULL,
	[Password] [varchar](50) NULL,
	[ImagePath] [varchar](1000) NULL,
	[IsActive] [bit] NULL,
	[CreationDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateBy] [int] NULL,
	[UserIP] [varchar](20) NULL,
	[SalaryTypeID] [int] NULL,
	[FatherName] [varchar](200) NULL,
	[AccountNumber] [varchar](50) NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserContact]    Script Date: 3/31/2020 5:49:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserContact](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[ContactTypeID] [int] NULL,
	[Detail] [varchar](200) NULL,
	[IsActive] [bit] NULL,
	[CreationDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateBy] [int] NULL,
	[UserIP] [varchar](20) NULL,
 CONSTRAINT [PK_UserContact] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserDepartment]    Script Date: 3/31/2020 5:49:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserDepartment](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[DepartmentID] [int] NULL,
	[EffectiveDate] [datetime] NULL,
	[RetiredDate] [datetime] NULL,
	[CreationDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateBy] [int] NULL,
	[UserIP] [varchar](20) NULL,
 CONSTRAINT [PK_UserDepartment] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserShift]    Script Date: 3/31/2020 5:49:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserShift](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[ShiftID] [int] NULL,
	[EffectiveDate] [datetime] NULL,
	[RetiredDate] [datetime] NULL,
	[CreationDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateBy] [int] NULL,
	[UserIP] [varchar](20) NULL,
 CONSTRAINT [PK_UserShift] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserType]    Script Date: 3/31/2020 5:49:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
	[IsActive] [bit] NULL,
	[CreationDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateBy] [int] NULL,
	[UserIP] [varchar](20) NULL,
 CONSTRAINT [PK_UserType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[Attendance] ON 

GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (34, 3, CAST(0x0000AB3700000000 AS DateTime), 1, CAST(0x0000AB8200FDB437 AS DateTime), NULL, 1, N'', CAST(0x0000AB370156C600 AS DateTime), CAST(0x0000AB3700A81740 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (35, 2, CAST(0x0000AB3700000000 AS DateTime), 1, CAST(0x0000AB8200FE1E55 AS DateTime), NULL, 1, N'', CAST(0x0000AB37016D4BA0 AS DateTime), CAST(0x0000AB3700A62B10 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (36, 4, CAST(0x0000AB3700000000 AS DateTime), 1, CAST(0x0000AB8200FE753B AS DateTime), NULL, 1, N'', CAST(0x0000AB370172C9E0 AS DateTime), CAST(0x0000AB3700E6B680 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (37, 2, CAST(0x0000AB3800000000 AS DateTime), 1, CAST(0x0000AB8200FECDB7 AS DateTime), NULL, 1, N'', CAST(0x0000AB380142B930 AS DateTime), CAST(0x0000AB3800A62B10 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (38, 3, CAST(0x0000AB3800000000 AS DateTime), 1, CAST(0x0000AB8200FEE071 AS DateTime), NULL, 1, N'', CAST(0x0000AB38013EE0D0 AS DateTime), CAST(0x0000AB3800A78AA0 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (39, 4, CAST(0x0000AB3800000000 AS DateTime), 1, CAST(0x0000AB8200FF04F0 AS DateTime), NULL, 1, N'', CAST(0x0000AB3800000000 AS DateTime), CAST(0x0000AB3800000000 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (40, 2, CAST(0x0000AB3A00000000 AS DateTime), 1, CAST(0x0000AB8200FFF49A AS DateTime), NULL, 1, N'', CAST(0x0000AB3A013E9A80 AS DateTime), CAST(0x0000AB3A00AA0370 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (41, 3, CAST(0x0000AB3A00000000 AS DateTime), 1, CAST(0x0000AB8201000228 AS DateTime), NULL, 1, N'', CAST(0x0000AB3A013E9A80 AS DateTime), CAST(0x0000AB3A00AA0370 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (42, 4, CAST(0x0000AB3A00000000 AS DateTime), 1, CAST(0x0000AB8201002A15 AS DateTime), NULL, 1, N'', CAST(0x0000AB3A01457850 AS DateTime), CAST(0x0000AB3A00BD83A0 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (43, 3, CAST(0x0000AB3B00000000 AS DateTime), 1, CAST(0x0000AB8201005810 AS DateTime), NULL, 1, N'', CAST(0x0000AB3B013E9A80 AS DateTime), CAST(0x0000AB3B00A7D0F0 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (44, 2, CAST(0x0000AB3B00000000 AS DateTime), 1, CAST(0x0000AB8201006511 AS DateTime), NULL, 1, N'', CAST(0x0000AB3B013E9A80 AS DateTime), CAST(0x0000AB3B00ABEFA0 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (45, 4, CAST(0x0000AB3B00000000 AS DateTime), 1, CAST(0x0000AB82010081E5 AS DateTime), NULL, 1, N'', CAST(0x0000AB3B015B7150 AS DateTime), CAST(0x0000AB3B00E556F0 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (46, 2, CAST(0x0000AB3C00000000 AS DateTime), 1, CAST(0x0000AB820100B773 AS DateTime), NULL, 1, N'', CAST(0x0000AB3C01772EE0 AS DateTime), CAST(0x0000AB3C00A62B10 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (47, 4, CAST(0x0000AB3C00000000 AS DateTime), 1, CAST(0x0000AB820100D1AF AS DateTime), NULL, 1, N'', CAST(0x0000AB3C0172C9E0 AS DateTime), CAST(0x0000AB3C00BAC480 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (48, 3, CAST(0x0000AB3C00000000 AS DateTime), 1, CAST(0x0000AB820100D9D3 AS DateTime), NULL, 1, N'', CAST(0x0000AB3C0172C9E0 AS DateTime), CAST(0x0000AB3C00BAC480 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (49, 3, CAST(0x0000AB3D00000000 AS DateTime), 1, CAST(0x0000AB820100FE88 AS DateTime), NULL, 1, N'', CAST(0x0000AB3D015E3070 AS DateTime), CAST(0x0000AB3D00A8EA30 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (50, 2, CAST(0x0000AB3D00000000 AS DateTime), 1, CAST(0x0000AB820101153F AS DateTime), NULL, 1, N'', CAST(0x0000AB3D01556670 AS DateTime), CAST(0x0000AB3D00AFC800 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (51, 4, CAST(0x0000AB3D00000000 AS DateTime), 1, CAST(0x0000AB82010132F0 AS DateTime), NULL, 1, N'', CAST(0x0000AB3D016A8C80 AS DateTime), CAST(0x0000AB3D00F5D1B0 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (52, 2, CAST(0x0000AB3E00000000 AS DateTime), 1, CAST(0x0000AB8201015F9D AS DateTime), NULL, 1, N'', CAST(0x0000AB3E0161C280 AS DateTime), CAST(0x0000AB3E00A85D90 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (53, 3, CAST(0x0000AB3E00000000 AS DateTime), 1, CAST(0x0000AB8201017683 AS DateTime), NULL, 1, N'', CAST(0x0000AB3E015DA3D0 AS DateTime), CAST(0x0000AB3E00A976D0 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (54, 4, CAST(0x0000AB3E00000000 AS DateTime), 1, CAST(0x0000AB8201018E86 AS DateTime), NULL, 1, N'', CAST(0x0000AB3E017B0740 AS DateTime), CAST(0x0000AB3E00000000 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (55, 2, CAST(0x0000AB3F00000000 AS DateTime), 1, CAST(0x0000AB820101BB03 AS DateTime), NULL, 1, N'', CAST(0x0000AB3F0156C600 AS DateTime), CAST(0x0000AB3F00A62B10 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (56, 4, CAST(0x0000AB3F00000000 AS DateTime), 1, CAST(0x0000AB820101DA29 AS DateTime), NULL, 1, N'', CAST(0x0000AB3F015E3070 AS DateTime), CAST(0x0000AB3F00FCAF80 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (57, 3, CAST(0x0000AB4100000000 AS DateTime), 1, CAST(0x0000AB820101F8F1 AS DateTime), NULL, 1, N'', CAST(0x0000AB41014F1540 AS DateTime), CAST(0x0000AB4100A4CB80 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (58, 2, CAST(0x0000AB4100000000 AS DateTime), 1, CAST(0x0000AB8201020AA4 AS DateTime), NULL, 1, N'', CAST(0x0000AB41014F1540 AS DateTime), CAST(0x0000AB4100B28720 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (59, 3, CAST(0x0000AB4200000000 AS DateTime), 1, CAST(0x0000AB8201022814 AS DateTime), NULL, 1, N'', CAST(0x0000AB42016EAB30 AS DateTime), CAST(0x0000AB4200A8EA30 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (60, 2, CAST(0x0000AB4200000000 AS DateTime), 1, CAST(0x0000AB8201023BCC AS DateTime), NULL, 1, N'', CAST(0x0000AB42014DB5B0 AS DateTime), CAST(0x0000AB4200ABA950 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (61, 4, CAST(0x0000AB4200000000 AS DateTime), 1, CAST(0x0000AB8201024CB6 AS DateTime), NULL, 1, N'', CAST(0x0000AB42014DB5B0 AS DateTime), CAST(0x0000AB4200AD08E0 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (62, 2, CAST(0x0000AB4300000000 AS DateTime), 1, CAST(0x0000AB820102AEFC AS DateTime), NULL, 1, N'', CAST(0x0000AB43014418C0 AS DateTime), CAST(0x0000AB4300A62B10 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (63, 3, CAST(0x0000AB4300000000 AS DateTime), 1, CAST(0x0000AB820102C41D AS DateTime), NULL, 1, N'', CAST(0x0000AB43013F2720 AS DateTime), CAST(0x0000AB4300AE6870 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (64, 4, CAST(0x0000AB4300000000 AS DateTime), 1, CAST(0x0000AB820102D979 AS DateTime), NULL, 1, N'', CAST(0x0000AB43016EAB30 AS DateTime), CAST(0x0000AB4300D63BC0 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (65, 2, CAST(0x0000AB4400000000 AS DateTime), 1, CAST(0x0000AB820103A8D6 AS DateTime), NULL, 1, N'', CAST(0x0000AB4401391C40 AS DateTime), CAST(0x0000AB4400986F70 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (66, 3, CAST(0x0000AB4400000000 AS DateTime), 1, CAST(0x0000AB820103D95F AS DateTime), NULL, 1, N'', CAST(0x0000AB4401391C40 AS DateTime), CAST(0x0000AB440099CF00 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (67, 4, CAST(0x0000AB4400000000 AS DateTime), 1, CAST(0x0000AB820103F436 AS DateTime), NULL, 1, N'', CAST(0x0000AB440150BB20 AS DateTime), CAST(0x0000AB4400B964F0 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (68, 3, CAST(0x0000AB4500000000 AS DateTime), 1, CAST(0x0000AB8201041DF0 AS DateTime), NULL, 1, N'', CAST(0x0000AB45014CE2C0 AS DateTime), CAST(0x0000AB4500994260 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (69, 4, CAST(0x0000AB4500000000 AS DateTime), 1, CAST(0x0000AB820104A3A9 AS DateTime), NULL, 1, N'', CAST(0x0000AB45015333F0 AS DateTime), CAST(0x0000AB4500B3E6B0 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (70, 3, CAST(0x0000AB4800000000 AS DateTime), 1, CAST(0x0000AB820104E614 AS DateTime), NULL, 1, N'', CAST(0x0000AB48013E9A80 AS DateTime), CAST(0x0000AB4800970FE0 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (71, 2, CAST(0x0000AB4800000000 AS DateTime), 1, CAST(0x0000AB820104F9B0 AS DateTime), NULL, 1, N'', CAST(0x0000AB48013E9A80 AS DateTime), CAST(0x0000AB4800A06680 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (72, 4, CAST(0x0000AB4800000000 AS DateTime), 1, CAST(0x0000AB8201050D15 AS DateTime), NULL, 1, N'', CAST(0x0000AB48013E9A80 AS DateTime), CAST(0x0000AB4800000000 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (73, 3, CAST(0x0000AB4900000000 AS DateTime), 1, CAST(0x0000AB820105659B AS DateTime), NULL, 1, N'', CAST(0x0000AB490130DEE0 AS DateTime), CAST(0x0000AB490099CF00 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (74, 2, CAST(0x0000AB4900000000 AS DateTime), 1, CAST(0x0000AB8201057C69 AS DateTime), NULL, 1, N'', CAST(0x0000AB490130DEE0 AS DateTime), CAST(0x0000AB4900A0ACD0 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (75, 4, CAST(0x0000AB4900000000 AS DateTime), 1, CAST(0x0000AB8201059835 AS DateTime), NULL, 1, N'', CAST(0x0000AB490130DEE0 AS DateTime), CAST(0x0000AB4900B80560 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (76, 3, CAST(0x0000AB4A00000000 AS DateTime), 1, CAST(0x0000AB820105C1AD AS DateTime), NULL, 1, N'', CAST(0x0000AB4A0146D7E0 AS DateTime), CAST(0x0000AB4A009B74E0 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (77, 2, CAST(0x0000AB4A00000000 AS DateTime), 1, CAST(0x0000AB820105CCD7 AS DateTime), NULL, 1, N'', CAST(0x0000AB4A0146D7E0 AS DateTime), CAST(0x0000AB4A009FD9E0 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (78, 4, CAST(0x0000AB4A00000000 AS DateTime), 1, CAST(0x0000AB820105D7C2 AS DateTime), NULL, 1, N'', CAST(0x0000AB4A0146D7E0 AS DateTime), CAST(0x0000AB4A00A20C60 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (79, 3, CAST(0x0000AB4B00000000 AS DateTime), 1, CAST(0x0000AB820105F46B AS DateTime), NULL, 1, N'', CAST(0x0000AB4B014159A0 AS DateTime), CAST(0x0000AB4B0099CF00 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (80, 2, CAST(0x0000AB4C00000000 AS DateTime), 1, CAST(0x0000AB820106127E AS DateTime), NULL, 1, N'', CAST(0x0000AB4C0151D460 AS DateTime), CAST(0x0000AB4C0099CF00 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (81, 3, CAST(0x0000AB4C00000000 AS DateTime), 1, CAST(0x0000AB82010621DA AS DateTime), NULL, 1, N'', CAST(0x0000AB4C0151D460 AS DateTime), CAST(0x0000AB4C00986F70 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (82, 4, CAST(0x0000AB4C00000000 AS DateTime), 1, CAST(0x0000AB8201062F16 AS DateTime), NULL, 1, N'', CAST(0x0000AB4C0151D460 AS DateTime), CAST(0x0000AB4C00A20C60 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (83, 2, CAST(0x0000AB4F00000000 AS DateTime), 1, CAST(0x0000AB8201065A19 AS DateTime), NULL, 1, N'', CAST(0x0000AB4F01624F20 AS DateTime), CAST(0x0000AB4F00986F70 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (84, 3, CAST(0x0000AB4F00000000 AS DateTime), 1, CAST(0x0000AB82010666F1 AS DateTime), NULL, 1, N'', CAST(0x0000AB4F01624F20 AS DateTime), CAST(0x0000AB4F009C8E20 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (85, 4, CAST(0x0000AB4F00000000 AS DateTime), 1, CAST(0x0000AB8201066ED9 AS DateTime), NULL, 1, N'', CAST(0x0000AB4F01624F20 AS DateTime), CAST(0x0000AB4F00A36BF0 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (86, 2, CAST(0x0000AB5000000000 AS DateTime), 1, CAST(0x0000AB820106AB23 AS DateTime), NULL, 1, N'', CAST(0x0000AB50014C5620 AS DateTime), CAST(0x0000AB50009DA760 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (87, 3, CAST(0x0000AB5000000000 AS DateTime), 1, CAST(0x0000AB820106B728 AS DateTime), NULL, 1, N'', CAST(0x0000AB50015752A0 AS DateTime), CAST(0x0000AB5000970FE0 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (88, 4, CAST(0x0000AB5000000000 AS DateTime), 1, CAST(0x0000AB820106C806 AS DateTime), NULL, 1, N'', CAST(0x0000AB50015752A0 AS DateTime), CAST(0x0000AB5000EC34C0 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (89, 3, CAST(0x0000AB5100000000 AS DateTime), 1, CAST(0x0000AB820106F6A5 AS DateTime), NULL, 1, N'', CAST(0x0000AB51013BDB60 AS DateTime), CAST(0x0000AB5100994260 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (90, 4, CAST(0x0000AB5100000000 AS DateTime), 1, CAST(0x0000AB8201070D0D AS DateTime), NULL, 1, N'', CAST(0x0000AB51013BDB60 AS DateTime), CAST(0x0000AB5100AFC800 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (91, 2, CAST(0x0000AB5200000000 AS DateTime), 1, CAST(0x0000AB8201073AF4 AS DateTime), NULL, 1, N'', CAST(0x0000AB52014F1540 AS DateTime), CAST(0x0000AB5200A8EA30 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (92, 3, CAST(0x0000AB5200000000 AS DateTime), 1, CAST(0x0000AB82010760C6 AS DateTime), NULL, 1, N'', CAST(0x0000AB5201391C40 AS DateTime), CAST(0x0000AB5200A62B10 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (93, 4, CAST(0x0000AB5200000000 AS DateTime), 1, CAST(0x0000AB8201077D0A AS DateTime), NULL, 1, N'', CAST(0x0000AB52015333F0 AS DateTime), CAST(0x0000AB5200C042C0 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (94, 2, CAST(0x0000AB5300000000 AS DateTime), 1, CAST(0x0000AB820107BA87 AS DateTime), NULL, 1, N'', CAST(0x0000AB53014AF690 AS DateTime), CAST(0x0000AB5300A4CB80 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (95, 3, CAST(0x0000AB5300000000 AS DateTime), 1, CAST(0x0000AB820107D1D1 AS DateTime), NULL, 1, N'', CAST(0x0000AB53014418C0 AS DateTime), CAST(0x0000AB5300A81740 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (96, 4, CAST(0x0000AB5300000000 AS DateTime), 1, CAST(0x0000AB820107E04C AS DateTime), NULL, 1, N'', CAST(0x0000AB53014418C0 AS DateTime), CAST(0x0000AB5300A81740 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (97, 3, CAST(0x0000AB5600000000 AS DateTime), 1, CAST(0x0000AB8201081277 AS DateTime), NULL, 1, N'', CAST(0x0000AB56014F1540 AS DateTime), CAST(0x0000AB5600A4CB80 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (98, 2, CAST(0x0000AB5600000000 AS DateTime), 1, CAST(0x0000AB82010824D1 AS DateTime), NULL, 1, N'', CAST(0x0000AB5601601CA0 AS DateTime), CAST(0x0000AB5600A8EA30 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (99, 4, CAST(0x0000AB5600000000 AS DateTime), 1, CAST(0x0000AB8201085E9F AS DateTime), NULL, 1, N'', CAST(0x0000AB5601549380 AS DateTime), CAST(0x0000AB5600C042C0 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (100, 3, CAST(0x0000AB5700000000 AS DateTime), 1, CAST(0x0000AB820108E72B AS DateTime), NULL, 1, N'', CAST(0x0000AB57013A7BD0 AS DateTime), CAST(0x0000AB5700A4CB80 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (101, 2, CAST(0x0000AB5700000000 AS DateTime), 1, CAST(0x0000AB820108F3ED AS DateTime), NULL, 1, N'', CAST(0x0000AB57013A7BD0 AS DateTime), CAST(0x0000AB5700AA49C0 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (102, 3, CAST(0x0000AB5800000000 AS DateTime), 1, CAST(0x0000AB8201094F37 AS DateTime), NULL, 1, N'', CAST(0x0000AB580163F500 AS DateTime), CAST(0x0000AB5800A4CB80 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (103, 2, CAST(0x0000AB5800000000 AS DateTime), 1, CAST(0x0000AB8201096260 AS DateTime), NULL, 1, N'', CAST(0x0000AB580163F500 AS DateTime), CAST(0x0000AB5800C042C0 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (104, 4, CAST(0x0000AB5800000000 AS DateTime), 1, CAST(0x0000AB8201097136 AS DateTime), NULL, 1, N'', CAST(0x0000AB580163F500 AS DateTime), CAST(0x0000AB5800DE7920 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (105, 2, CAST(0x0000AB5900000000 AS DateTime), 1, CAST(0x0000AB8201099280 AS DateTime), NULL, 1, N'', CAST(0x0000AB5901457850 AS DateTime), CAST(0x0000AB5900A4CB80 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (106, 2, CAST(0x0000AB5A00000000 AS DateTime), 1, CAST(0x0000AB820109DE3E AS DateTime), NULL, 1, N'', CAST(0x0000AB5A0145BEA0 AS DateTime), CAST(0x0000AB5A00A4CB80 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (107, 4, CAST(0x0000AB5A00000000 AS DateTime), 1, CAST(0x0000AB82010A08B9 AS DateTime), NULL, 1, N'', CAST(0x0000AB5A015E3070 AS DateTime), CAST(0x0000AB5A00B80560 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (108, 2, CAST(0x0000AB5D00000000 AS DateTime), 1, CAST(0x0000AB82010A3A38 AS DateTime), NULL, 1, N'', CAST(0x0000AB5D01391C40 AS DateTime), CAST(0x0000AB5D00A4CB80 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (109, 4, CAST(0x0000AB5D00000000 AS DateTime), 1, CAST(0x0000AB82010A494F AS DateTime), NULL, 1, N'', CAST(0x0000AB5D013E9A80 AS DateTime), CAST(0x0000AB5D00AE6870 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (110, 2, CAST(0x0000AB5E00000000 AS DateTime), 1, CAST(0x0000AB82010A6A65 AS DateTime), NULL, 1, N'', CAST(0x0000AB5E0167CD60 AS DateTime), CAST(0x0000AB5E00A4CB80 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (111, 4, CAST(0x0000AB5E00000000 AS DateTime), 1, CAST(0x0000AB82010A7DED AS DateTime), NULL, 1, N'', CAST(0x0000AB5E017B0740 AS DateTime), CAST(0x0000AB5E00F05370 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (112, 2, CAST(0x0000AB5F00000000 AS DateTime), 1, CAST(0x0000AB82010AC7F6 AS DateTime), NULL, 1, N'', CAST(0x0000AB5F013D3AF0 AS DateTime), CAST(0x0000AB5F00A78AA0 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (113, 4, CAST(0x0000AB5F00000000 AS DateTime), 1, CAST(0x0000AB82010AD685 AS DateTime), NULL, 1, N'', CAST(0x0000AB5F013D3AF0 AS DateTime), CAST(0x0000AB5F00D8FAE0 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (114, 4, CAST(0x0000AB6000000000 AS DateTime), 1, CAST(0x0000AB82010AF481 AS DateTime), NULL, 1, N'', CAST(0x0000AB60013D3AF0 AS DateTime), CAST(0x0000AB6000A0ACD0 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (115, 4, CAST(0x0000AB6100000000 AS DateTime), 1, CAST(0x0000AB82010B6DBF AS DateTime), NULL, 1, N'', CAST(0x0000AB61014159A0 AS DateTime), CAST(0x0000AB6100A43EE0 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (116, 2, CAST(0x0000AB6100000000 AS DateTime), 1, CAST(0x0000AB82010B9448 AS DateTime), NULL, 1, N'', CAST(0x0000AB61014AF690 AS DateTime), CAST(0x0000AB6100A78AA0 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (117, 2, CAST(0x0000AB6400000000 AS DateTime), 1, CAST(0x0000AB82010E0272 AS DateTime), NULL, 1, N'', CAST(0x0000AB64014F1540 AS DateTime), CAST(0x0000AB6400A8EA30 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (118, 4, CAST(0x0000AB6400000000 AS DateTime), 1, CAST(0x0000AB82010E1405 AS DateTime), NULL, 1, N'', CAST(0x0000AB64012A0110 AS DateTime), CAST(0x0000AB6400A62B10 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (119, 2, CAST(0x0000AB6500000000 AS DateTime), 1, CAST(0x0000AB82010E8479 AS DateTime), NULL, 1, N'', CAST(0x0000AB65013FFA10 AS DateTime), CAST(0x0000AB6500A8EA30 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (120, 4, CAST(0x0000AB6600000000 AS DateTime), 1, CAST(0x0000AB82010EB0AD AS DateTime), NULL, 1, N'', CAST(0x0000AB66014345D0 AS DateTime), CAST(0x0000AB6600A36BF0 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (121, 2, CAST(0x0000AB6600000000 AS DateTime), 1, CAST(0x0000AB82010EC47D AS DateTime), NULL, 1, N'', CAST(0x0000AB66015B7150 AS DateTime), CAST(0x0000AB6600A8EA30 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (122, 3, CAST(0x0000AB6700000000 AS DateTime), 1, CAST(0x0000AB82010F1356 AS DateTime), NULL, 1, N'', CAST(0x0000AB6701457850 AS DateTime), CAST(0x0000AB6700A78AA0 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (123, 3, CAST(0x0000AB6800000000 AS DateTime), 1, CAST(0x0000AB8201106D3B AS DateTime), NULL, 1, N'', CAST(0x0000AB68014418C0 AS DateTime), CAST(0x0000AB6800A62B10 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (124, 2, CAST(0x0000AB6800000000 AS DateTime), 1, CAST(0x0000AB8201108352 AS DateTime), NULL, 1, N'', CAST(0x0000AB68014418C0 AS DateTime), CAST(0x0000AB6800A8EA30 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (125, 4, CAST(0x0000AB6800000000 AS DateTime), 1, CAST(0x0000AB8201108C5B AS DateTime), NULL, 1, N'', CAST(0x0000AB68014418C0 AS DateTime), CAST(0x0000AB6800AA49C0 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (126, 3, CAST(0x0000AB6B00000000 AS DateTime), 1, CAST(0x0000AB820110B9D6 AS DateTime), NULL, 1, N'', CAST(0x0000AB6B0146D7E0 AS DateTime), CAST(0x0000AB6B00A4CB80 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (127, 4, CAST(0x0000AB6B00000000 AS DateTime), 1, CAST(0x0000AB820110C802 AS DateTime), NULL, 1, N'', CAST(0x0000AB6B013FFA10 AS DateTime), CAST(0x0000AB6B00A4CB80 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (128, 2, CAST(0x0000AB6B00000000 AS DateTime), 1, CAST(0x0000AB820110D402 AS DateTime), NULL, 1, N'', CAST(0x0000AB6B014D2910 AS DateTime), CAST(0x0000AB6B00A4CB80 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (129, 3, CAST(0x0000AB6C00000000 AS DateTime), 1, CAST(0x0000AB820110E9CB AS DateTime), NULL, 1, N'', CAST(0x0000AB6C014F1540 AS DateTime), CAST(0x0000AB6C00A4CB80 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (130, 2, CAST(0x0000AB6C00000000 AS DateTime), 1, CAST(0x0000AB820110F90C AS DateTime), NULL, 1, N'', CAST(0x0000AB6C0151D460 AS DateTime), CAST(0x0000AB6C00A8EA30 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (131, 3, CAST(0x0000AB6D00000000 AS DateTime), 1, CAST(0x0000AB8201112217 AS DateTime), NULL, 1, N'', CAST(0x0000AB6D01483770 AS DateTime), CAST(0x0000AB6D00A78AA0 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (132, 2, CAST(0x0000AB6D00000000 AS DateTime), 1, CAST(0x0000AB8201112E89 AS DateTime), NULL, 1, N'', CAST(0x0000AB6D01499700 AS DateTime), CAST(0x0000AB6D00A78AA0 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (133, 4, CAST(0x0000AB6D00000000 AS DateTime), 1, CAST(0x0000AB820111414F AS DateTime), NULL, 1, N'', CAST(0x0000AB6D014159A0 AS DateTime), CAST(0x0000AB6D00AB1CB0 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (134, 2, CAST(0x0000AB6E00000000 AS DateTime), 1, CAST(0x0000AB8201115CE6 AS DateTime), NULL, 1, N'', CAST(0x0000AB6E013B0870 AS DateTime), CAST(0x0000AB6E00A59E70 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (135, 3, CAST(0x0000AB6E00000000 AS DateTime), 1, CAST(0x0000AB8201116B51 AS DateTime), NULL, 1, N'', CAST(0x0000AB6E013F6D70 AS DateTime), CAST(0x0000AB6E00A6B7B0 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (136, 4, CAST(0x0000AB6E00000000 AS DateTime), 1, CAST(0x0000AB82011181B8 AS DateTime), NULL, 1, N'', CAST(0x0000AB6E013F6D70 AS DateTime), CAST(0x0000AB6E00EFC6D0 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (137, 3, CAST(0x0000AB6F00000000 AS DateTime), 1, CAST(0x0000AB820111A279 AS DateTime), NULL, 1, N'', CAST(0x0000AB6F014DB5B0 AS DateTime), CAST(0x0000AB6F00A62B10 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (138, 2, CAST(0x0000AB6F00000000 AS DateTime), 1, CAST(0x0000AB820111CDE2 AS DateTime), NULL, 1, N'', CAST(0x0000AB6F01499700 AS DateTime), CAST(0x0000AB6F00A78AA0 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (139, 4, CAST(0x0000AB6F00000000 AS DateTime), 1, CAST(0x0000AB820111E087 AS DateTime), NULL, 1, N'', CAST(0x0000AB6F01549380 AS DateTime), CAST(0x0000AB6F00BD3D50 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (140, 3, CAST(0x0000AB7000000000 AS DateTime), 1, CAST(0x0000AB82011204FA AS DateTime), NULL, 1, N'', CAST(0x0000AB700107AC00 AS DateTime), CAST(0x0000AB7000A0ACD0 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (141, 2, CAST(0x0000AB7000000000 AS DateTime), 1, CAST(0x0000AB8201124DC5 AS DateTime), NULL, 1, N'', CAST(0x0000AB70013E9A80 AS DateTime), CAST(0x0000AB7000AFC800 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (142, 4, CAST(0x0000AB7000000000 AS DateTime), 1, CAST(0x0000AB82011281E2 AS DateTime), NULL, 1, N'', CAST(0x0000AB700176E890 AS DateTime), CAST(0x0000AB7000F890D0 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (143, 3, CAST(0x0000AB7200000000 AS DateTime), 1, CAST(0x0000AB8201140F51 AS DateTime), NULL, 1, N'', CAST(0x0000AB72013E9A80 AS DateTime), CAST(0x0000AB7200A20C60 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (144, 2, CAST(0x0000AB7200000000 AS DateTime), 1, CAST(0x0000AB8201148EA0 AS DateTime), NULL, 1, N'', CAST(0x0000AB7201064C70 AS DateTime), CAST(0x0000AB7200A62B10 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (145, 3, CAST(0x0000AB7300000000 AS DateTime), 1, CAST(0x0000AB820114B673 AS DateTime), NULL, 1, N'', CAST(0x0000AB73013E9A80 AS DateTime), CAST(0x0000AB7300A4CB80 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (146, 2, CAST(0x0000AB7300000000 AS DateTime), 1, CAST(0x0000AB82011508B2 AS DateTime), NULL, 1, N'', CAST(0x0000AB73013E9A80 AS DateTime), CAST(0x0000AB7300ABA950 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (147, 3, CAST(0x0000AB7400000000 AS DateTime), 1, CAST(0x0000AB8201153066 AS DateTime), NULL, 1, N'', CAST(0x0000AB74015333F0 AS DateTime), CAST(0x0000AB7400A36BF0 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (148, 2, CAST(0x0000AB7400000000 AS DateTime), 1, CAST(0x0000AB8201154569 AS DateTime), NULL, 1, N'', CAST(0x0000AB74015E3070 AS DateTime), CAST(0x0000AB7400AAD660 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (149, 4, CAST(0x0000AB7400000000 AS DateTime), 1, CAST(0x0000AB8201155C80 AS DateTime), NULL, 1, N'', CAST(0x0000AB7401624F20 AS DateTime), CAST(0x0000AB7400D21D10 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (150, 3, CAST(0x0000AB7600000000 AS DateTime), 1, CAST(0x0000AB82011642B3 AS DateTime), NULL, 1, N'', CAST(0x0000AB76014AF690 AS DateTime), CAST(0x0000AB7600A20C60 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (151, 2, CAST(0x0000AB7600000000 AS DateTime), 1, CAST(0x0000AB8201165584 AS DateTime), NULL, 1, N'', CAST(0x0000AB76014159A0 AS DateTime), CAST(0x0000AB7600A8EA30 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (152, 3, CAST(0x0000AB7900000000 AS DateTime), 1, CAST(0x0000AB82011693B6 AS DateTime), NULL, 1, N'', CAST(0x0000AB79014AF690 AS DateTime), CAST(0x0000AB7900A36BF0 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (153, 4, CAST(0x0000AB7900000000 AS DateTime), 1, CAST(0x0000AB820116AC18 AS DateTime), NULL, 1, N'', CAST(0x0000AB79016062F0 AS DateTime), CAST(0x0000AB7900D8FAE0 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (154, 3, CAST(0x0000AB7A00000000 AS DateTime), 1, CAST(0x0000AB820116D5C9 AS DateTime), NULL, 1, N'', CAST(0x0000AB7A0139EF30 AS DateTime), CAST(0x0000AB7A00A62B10 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (155, 2, CAST(0x0000AB7A00000000 AS DateTime), 1, CAST(0x0000AB820116EDA7 AS DateTime), NULL, 1, N'', CAST(0x0000AB7A0139EF30 AS DateTime), CAST(0x0000AB7A00A4CB80 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (156, 3, CAST(0x0000AB7B00000000 AS DateTime), 1, CAST(0x0000AB8201170EBF AS DateTime), NULL, 1, N'', CAST(0x0000AB7B01419FF0 AS DateTime), CAST(0x0000AB7B00A4CB80 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (157, 2, CAST(0x0000AB7B00000000 AS DateTime), 1, CAST(0x0000AB8201171A77 AS DateTime), NULL, 1, N'', CAST(0x0000AB7B01419FF0 AS DateTime), CAST(0x0000AB7B00AA49C0 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (158, 2, CAST(0x0000AB7C00000000 AS DateTime), 1, CAST(0x0000AB82011771D0 AS DateTime), NULL, 1, N'', CAST(0x0000AB7C013A7BD0 AS DateTime), CAST(0x0000AB7C00A78AA0 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (159, 3, CAST(0x0000AB7C00000000 AS DateTime), 1, CAST(0x0000AB8201178034 AS DateTime), NULL, 1, N'', CAST(0x0000AB7C013A7BD0 AS DateTime), CAST(0x0000AB7C00A78AA0 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (160, 4, CAST(0x0000AB7C00000000 AS DateTime), 1, CAST(0x0000AB8201179C4A AS DateTime), NULL, 1, N'', CAST(0x0000AB7C01549380 AS DateTime), CAST(0x0000AB7C00C60750 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (161, 3, CAST(0x0000AB7D00000000 AS DateTime), 1, CAST(0x0000AB820117C105 AS DateTime), NULL, 1, N'', CAST(0x0000AB7D016BEC10 AS DateTime), CAST(0x0000AB7D00A62B10 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (162, 2, CAST(0x0000AB7D00000000 AS DateTime), 1, CAST(0x0000AB820117CF5D AS DateTime), NULL, 1, N'', CAST(0x0000AB7D016D4BA0 AS DateTime), CAST(0x0000AB7D00A78AA0 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (163, 4, CAST(0x0000AB7D00000000 AS DateTime), 1, CAST(0x0000AB820117E165 AS DateTime), NULL, 1, N'', CAST(0x0000AB7D016D4BA0 AS DateTime), CAST(0x0000AB7D00B28720 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (164, 2, CAST(0x0000AB7E00000000 AS DateTime), 1, CAST(0x0000AB8201180D58 AS DateTime), NULL, 1, N'', CAST(0x0000AB7E011F9130 AS DateTime), CAST(0x0000AB7E00A8EA30 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (165, 4, CAST(0x0000AB7E00000000 AS DateTime), 1, CAST(0x0000AB82011826FE AS DateTime), NULL, 1, N'', CAST(0x0000AB7E012CC030 AS DateTime), CAST(0x0000AB7E00ED9450 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (166, 2, CAST(0x0000AB7F00000000 AS DateTime), 1, CAST(0x0000AB8201185B81 AS DateTime), NULL, 1, N'', CAST(0x0000AB7F014345D0 AS DateTime), CAST(0x0000AB7F00A4CB80 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (167, 3, CAST(0x0000AB7F00000000 AS DateTime), 1, CAST(0x0000AB8201186484 AS DateTime), NULL, 1, N'', CAST(0x0000AB7F014345D0 AS DateTime), CAST(0x0000AB7F00A62B10 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (168, 2, CAST(0x0000AB8000000000 AS DateTime), 1, CAST(0x0000AB820118BDCF AS DateTime), NULL, 1, N'', CAST(0x0000AB80014345D0 AS DateTime), CAST(0x0000AB8000A62B10 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (169, 3, CAST(0x0000AB8000000000 AS DateTime), 1, CAST(0x0000AB820118C188 AS DateTime), NULL, 1, N'', CAST(0x0000AB80014345D0 AS DateTime), CAST(0x0000AB8000A62B10 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (170, 3, CAST(0x0000AB8100000000 AS DateTime), 1, CAST(0x0000AB820118EA7C AS DateTime), NULL, 1, N'', CAST(0x0000AB81014159A0 AS DateTime), CAST(0x0000AB8100A85D90 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (171, 2, CAST(0x0000AB8100000000 AS DateTime), 1, CAST(0x0000AB820118F5B1 AS DateTime), NULL, 1, N'', CAST(0x0000AB81014159A0 AS DateTime), CAST(0x0000AB8100ADDBD0 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (172, 4, CAST(0x0000AB8100000000 AS DateTime), 1, CAST(0x0000AB8201190694 AS DateTime), NULL, 1, N'', CAST(0x0000AB81014159A0 AS DateTime), CAST(0x0000AB8100ACC290 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (173, 2, CAST(0x0000AB8900000000 AS DateTime), 1, CAST(0x0000AB8900BEF879 AS DateTime), NULL, 1, N'', CAST(0x0000AB890151D460 AS DateTime), CAST(0x0000AB89009C8E20 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (174, 3, CAST(0x0000AB8900000000 AS DateTime), 1, CAST(0x0000AB8900CB410C AS DateTime), NULL, 1, N'', CAST(0x0000AB8900000000 AS DateTime), CAST(0x0000AB8900000000 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (175, 4, CAST(0x0000AB8900000000 AS DateTime), 1, CAST(0x0000AB89010A25B8 AS DateTime), NULL, 1, N'', CAST(0x0000AB8901499700 AS DateTime), CAST(0x0000AB8900A4CB80 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (176, 5, CAST(0x0000AB8900000000 AS DateTime), 1, CAST(0x0000AB890123AE93 AS DateTime), NULL, 1, N'', CAST(0x0000AB89011826C0 AS DateTime), CAST(0x0000AB8900A4CB80 AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (177, 16, CAST(0x0000AB8900000000 AS DateTime), 1, CAST(0x0000AB890126D0F8 AS DateTime), NULL, 1, NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[Attendance] OFF
GO
SET IDENTITY_INSERT [dbo].[AttendanceDetail] ON 

GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, 34, NULL, CAST(0x0000AB3700A81740 AS DateTime), CAST(0x0000AB370156C600 AS DateTime), 1, CAST(0x0000AB8200FDB44C AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, 35, NULL, CAST(0x0000AB3700A62B10 AS DateTime), CAST(0x0000AB37016D4BA0 AS DateTime), 1, CAST(0x0000AB8200FE1E65 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (3, 36, NULL, CAST(0x0000AB3700E6B680 AS DateTime), CAST(0x0000AB370172C9E0 AS DateTime), 1, CAST(0x0000AB8200FE7544 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (4, 37, NULL, CAST(0x0000AB3800A62B10 AS DateTime), CAST(0x0000AB380142B930 AS DateTime), 1, CAST(0x0000AB8200FECDC0 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (5, 38, NULL, CAST(0x0000AB3800A78AA0 AS DateTime), CAST(0x0000AB38013EE0D0 AS DateTime), 1, CAST(0x0000AB8200FEE078 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (6, 39, NULL, CAST(0x0000AB3800000000 AS DateTime), CAST(0x0000AB3800000000 AS DateTime), 1, CAST(0x0000AB8200FF0501 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (7, 40, NULL, CAST(0x0000AB3A00AA0370 AS DateTime), CAST(0x0000AB3A013E9A80 AS DateTime), 1, CAST(0x0000AB8200FFF4A0 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (8, 41, NULL, CAST(0x0000AB3A00AA0370 AS DateTime), CAST(0x0000AB3A013E9A80 AS DateTime), 1, CAST(0x0000AB8201000230 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (9, 42, NULL, CAST(0x0000AB3A00BD83A0 AS DateTime), CAST(0x0000AB3A01457850 AS DateTime), 1, CAST(0x0000AB8201002A1E AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (10, 43, NULL, CAST(0x0000AB3B00A7D0F0 AS DateTime), CAST(0x0000AB3B013E9A80 AS DateTime), 1, CAST(0x0000AB8201005815 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (11, 44, NULL, CAST(0x0000AB3B00ABEFA0 AS DateTime), CAST(0x0000AB3B013E9A80 AS DateTime), 1, CAST(0x0000AB8201006518 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (12, 45, NULL, CAST(0x0000AB3B00E556F0 AS DateTime), CAST(0x0000AB3B015B7150 AS DateTime), 1, CAST(0x0000AB82010081EB AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (13, 46, NULL, CAST(0x0000AB3C00A62B10 AS DateTime), CAST(0x0000AB3C01772EE0 AS DateTime), 1, CAST(0x0000AB820100B778 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (14, 47, NULL, CAST(0x0000AB3C00BAC480 AS DateTime), CAST(0x0000AB3C0172C9E0 AS DateTime), 1, CAST(0x0000AB820100D1B6 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (15, 48, NULL, CAST(0x0000AB3C00BAC480 AS DateTime), CAST(0x0000AB3C0172C9E0 AS DateTime), 1, CAST(0x0000AB820100D9DA AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (16, 49, NULL, CAST(0x0000AB3D00A8EA30 AS DateTime), CAST(0x0000AB3D015E3070 AS DateTime), 1, CAST(0x0000AB820100FE91 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (17, 50, NULL, CAST(0x0000AB3D00AFC800 AS DateTime), CAST(0x0000AB3D01556670 AS DateTime), 1, CAST(0x0000AB8201011545 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (18, 51, NULL, CAST(0x0000AB3D00F5D1B0 AS DateTime), CAST(0x0000AB3D016A8C80 AS DateTime), 1, CAST(0x0000AB82010132F7 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (19, 52, NULL, CAST(0x0000AB3E00A85D90 AS DateTime), CAST(0x0000AB3E0161C280 AS DateTime), 1, CAST(0x0000AB8201015FA4 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (20, 53, NULL, CAST(0x0000AB3E00A976D0 AS DateTime), CAST(0x0000AB3E015DA3D0 AS DateTime), 1, CAST(0x0000AB820101768B AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (21, 54, NULL, CAST(0x0000AB3E00000000 AS DateTime), CAST(0x0000AB3E017B0740 AS DateTime), 1, CAST(0x0000AB8201018E8C AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (22, 55, NULL, CAST(0x0000AB3F00A62B10 AS DateTime), CAST(0x0000AB3F0156C600 AS DateTime), 1, CAST(0x0000AB820101BB11 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (23, 56, NULL, CAST(0x0000AB3F00FCAF80 AS DateTime), CAST(0x0000AB3F015E3070 AS DateTime), 1, CAST(0x0000AB820101DA30 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (24, 57, NULL, CAST(0x0000AB4100A4CB80 AS DateTime), CAST(0x0000AB41014F1540 AS DateTime), 1, CAST(0x0000AB820101F8F9 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (25, 58, NULL, CAST(0x0000AB4100B28720 AS DateTime), CAST(0x0000AB41014F1540 AS DateTime), 1, CAST(0x0000AB8201020AA9 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (26, 59, NULL, CAST(0x0000AB4200A8EA30 AS DateTime), CAST(0x0000AB42016EAB30 AS DateTime), 1, CAST(0x0000AB820102281C AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (27, 60, NULL, CAST(0x0000AB4200ABA950 AS DateTime), CAST(0x0000AB42014DB5B0 AS DateTime), 1, CAST(0x0000AB8201023BD3 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (28, 61, NULL, CAST(0x0000AB4200AD08E0 AS DateTime), CAST(0x0000AB42014DB5B0 AS DateTime), 1, CAST(0x0000AB8201024CC9 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (29, 62, NULL, CAST(0x0000AB4300A62B10 AS DateTime), CAST(0x0000AB43014418C0 AS DateTime), 1, CAST(0x0000AB820102AF03 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (30, 63, NULL, CAST(0x0000AB4300AE6870 AS DateTime), CAST(0x0000AB43013F2720 AS DateTime), 1, CAST(0x0000AB820102C426 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (31, 64, NULL, CAST(0x0000AB4300D63BC0 AS DateTime), CAST(0x0000AB43016EAB30 AS DateTime), 1, CAST(0x0000AB820102D97E AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (32, 65, NULL, CAST(0x0000AB4400986F70 AS DateTime), CAST(0x0000AB4401391C40 AS DateTime), 1, CAST(0x0000AB820103A8DE AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (33, 66, NULL, CAST(0x0000AB440099CF00 AS DateTime), CAST(0x0000AB4401391C40 AS DateTime), 1, CAST(0x0000AB820103D963 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (34, 67, NULL, CAST(0x0000AB4400B964F0 AS DateTime), CAST(0x0000AB440150BB20 AS DateTime), 1, CAST(0x0000AB820103F43C AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (35, 68, NULL, CAST(0x0000AB4500994260 AS DateTime), CAST(0x0000AB45014CE2C0 AS DateTime), 1, CAST(0x0000AB8201041DF6 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (36, 69, NULL, CAST(0x0000AB4500B3E6B0 AS DateTime), CAST(0x0000AB45015333F0 AS DateTime), 1, CAST(0x0000AB820104A3B2 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (37, 70, NULL, CAST(0x0000AB4800970FE0 AS DateTime), CAST(0x0000AB48013E9A80 AS DateTime), 1, CAST(0x0000AB820104E61A AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (38, 71, NULL, CAST(0x0000AB4800A06680 AS DateTime), CAST(0x0000AB48013E9A80 AS DateTime), 1, CAST(0x0000AB820104F9BD AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (39, 72, NULL, CAST(0x0000AB4800000000 AS DateTime), CAST(0x0000AB48013E9A80 AS DateTime), 1, CAST(0x0000AB8201050D25 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (40, 73, NULL, CAST(0x0000AB490099CF00 AS DateTime), CAST(0x0000AB490130DEE0 AS DateTime), 1, CAST(0x0000AB82010565A2 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (41, 74, NULL, CAST(0x0000AB4900A0ACD0 AS DateTime), CAST(0x0000AB490130DEE0 AS DateTime), 1, CAST(0x0000AB8201057C75 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (42, 75, NULL, CAST(0x0000AB4900B80560 AS DateTime), CAST(0x0000AB490130DEE0 AS DateTime), 1, CAST(0x0000AB8201059840 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (43, 76, NULL, CAST(0x0000AB4A009B74E0 AS DateTime), CAST(0x0000AB4A0146D7E0 AS DateTime), 1, CAST(0x0000AB820105C1B1 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (44, 77, NULL, CAST(0x0000AB4A009FD9E0 AS DateTime), CAST(0x0000AB4A0146D7E0 AS DateTime), 1, CAST(0x0000AB820105CCDD AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (45, 78, NULL, CAST(0x0000AB4A00A20C60 AS DateTime), CAST(0x0000AB4A0146D7E0 AS DateTime), 1, CAST(0x0000AB820105D7C7 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (46, 79, NULL, CAST(0x0000AB4B0099CF00 AS DateTime), CAST(0x0000AB4B014159A0 AS DateTime), 1, CAST(0x0000AB820105F46F AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (47, 80, NULL, CAST(0x0000AB4C0099CF00 AS DateTime), CAST(0x0000AB4C0151D460 AS DateTime), 1, CAST(0x0000AB8201061284 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (48, 81, NULL, CAST(0x0000AB4C00986F70 AS DateTime), CAST(0x0000AB4C0151D460 AS DateTime), 1, CAST(0x0000AB82010621DF AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (49, 82, NULL, CAST(0x0000AB4C00A20C60 AS DateTime), CAST(0x0000AB4C0151D460 AS DateTime), 1, CAST(0x0000AB8201062F1D AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (50, 83, NULL, CAST(0x0000AB4F00986F70 AS DateTime), CAST(0x0000AB4F01624F20 AS DateTime), 1, CAST(0x0000AB8201065A23 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (51, 84, NULL, CAST(0x0000AB4F009C8E20 AS DateTime), CAST(0x0000AB4F01624F20 AS DateTime), 1, CAST(0x0000AB82010666FA AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (52, 85, NULL, CAST(0x0000AB4F00A36BF0 AS DateTime), CAST(0x0000AB4F01624F20 AS DateTime), 1, CAST(0x0000AB8201066EE0 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (53, 86, NULL, CAST(0x0000AB50009DA760 AS DateTime), CAST(0x0000AB50014C5620 AS DateTime), 1, CAST(0x0000AB820106AB37 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (54, 87, NULL, CAST(0x0000AB5000970FE0 AS DateTime), CAST(0x0000AB50015752A0 AS DateTime), 1, CAST(0x0000AB820106B72D AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (55, 88, NULL, CAST(0x0000AB5000EC34C0 AS DateTime), CAST(0x0000AB50015752A0 AS DateTime), 1, CAST(0x0000AB820106C80C AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (56, 89, NULL, CAST(0x0000AB5100994260 AS DateTime), CAST(0x0000AB51013BDB60 AS DateTime), 1, CAST(0x0000AB820106F6AD AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (57, 90, NULL, CAST(0x0000AB5100AFC800 AS DateTime), CAST(0x0000AB51013BDB60 AS DateTime), 1, CAST(0x0000AB8201070D11 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (58, 90, NULL, CAST(0x0000AB5100B28720 AS DateTime), CAST(0x0000AB51013BDB60 AS DateTime), 1, CAST(0x0000AB8201071439 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (59, 91, NULL, CAST(0x0000AB5200A8EA30 AS DateTime), CAST(0x0000AB52014F1540 AS DateTime), 1, CAST(0x0000AB8201073B0A AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (60, 92, NULL, CAST(0x0000AB5200A62B10 AS DateTime), CAST(0x0000AB5201391C40 AS DateTime), 1, CAST(0x0000AB82010760D9 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (61, 93, NULL, CAST(0x0000AB5200C042C0 AS DateTime), CAST(0x0000AB52015333F0 AS DateTime), 1, CAST(0x0000AB8201077D0F AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (62, 94, NULL, CAST(0x0000AB5300A4CB80 AS DateTime), CAST(0x0000AB53014AF690 AS DateTime), 1, CAST(0x0000AB820107BA90 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (63, 95, NULL, CAST(0x0000AB5300A81740 AS DateTime), CAST(0x0000AB53014418C0 AS DateTime), 1, CAST(0x0000AB820107D1D9 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (64, 96, NULL, CAST(0x0000AB5300A81740 AS DateTime), CAST(0x0000AB53014418C0 AS DateTime), 1, CAST(0x0000AB820107E051 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (65, 97, NULL, CAST(0x0000AB5600A4CB80 AS DateTime), CAST(0x0000AB56014F1540 AS DateTime), 1, CAST(0x0000AB8201081285 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (66, 98, NULL, CAST(0x0000AB5600A8EA30 AS DateTime), CAST(0x0000AB5601601CA0 AS DateTime), 1, CAST(0x0000AB82010824D6 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (67, 99, NULL, CAST(0x0000AB5600C042C0 AS DateTime), CAST(0x0000AB5601549380 AS DateTime), 1, CAST(0x0000AB8201085EA4 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (68, 100, NULL, CAST(0x0000AB5700A4CB80 AS DateTime), CAST(0x0000AB57013A7BD0 AS DateTime), 1, CAST(0x0000AB820108E731 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (69, 101, NULL, CAST(0x0000AB5700AA49C0 AS DateTime), CAST(0x0000AB57013A7BD0 AS DateTime), 1, CAST(0x0000AB820108F3F3 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (70, 102, NULL, CAST(0x0000AB5800A4CB80 AS DateTime), CAST(0x0000AB580163F500 AS DateTime), 1, CAST(0x0000AB8201094F40 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (71, 103, NULL, CAST(0x0000AB5800C042C0 AS DateTime), CAST(0x0000AB580163F500 AS DateTime), 1, CAST(0x0000AB8201096264 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (72, 104, NULL, CAST(0x0000AB5800DE7920 AS DateTime), CAST(0x0000AB580163F500 AS DateTime), 1, CAST(0x0000AB820109713B AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (73, 105, NULL, CAST(0x0000AB5900A4CB80 AS DateTime), CAST(0x0000AB5901457850 AS DateTime), 1, CAST(0x0000AB8201099287 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (74, 106, NULL, CAST(0x0000AB5A00A4CB80 AS DateTime), CAST(0x0000AB5A0145BEA0 AS DateTime), 1, CAST(0x0000AB820109DE45 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (75, 107, NULL, CAST(0x0000AB5A00B80560 AS DateTime), CAST(0x0000AB5A015E3070 AS DateTime), 1, CAST(0x0000AB82010A08C0 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (76, 108, NULL, CAST(0x0000AB5D00A4CB80 AS DateTime), CAST(0x0000AB5D01391C40 AS DateTime), 1, CAST(0x0000AB82010A3A3E AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (77, 109, NULL, CAST(0x0000AB5D00AE6870 AS DateTime), CAST(0x0000AB5D013E9A80 AS DateTime), 1, CAST(0x0000AB82010A4954 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (78, 110, NULL, CAST(0x0000AB5E00A4CB80 AS DateTime), CAST(0x0000AB5E0167CD60 AS DateTime), 1, CAST(0x0000AB82010A6A69 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (79, 111, NULL, CAST(0x0000AB5E00F05370 AS DateTime), CAST(0x0000AB5E017B0740 AS DateTime), 1, CAST(0x0000AB82010A7DFA AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (80, 112, NULL, CAST(0x0000AB5F00A78AA0 AS DateTime), CAST(0x0000AB5F013D3AF0 AS DateTime), 1, CAST(0x0000AB82010AC802 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (81, 113, NULL, CAST(0x0000AB5F00D8FAE0 AS DateTime), CAST(0x0000AB5F013D3AF0 AS DateTime), 1, CAST(0x0000AB82010AD68A AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (82, 114, NULL, CAST(0x0000AB6000A0ACD0 AS DateTime), CAST(0x0000AB60013A7BD0 AS DateTime), 1, CAST(0x0000AB82010AF488 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (83, 114, NULL, CAST(0x0000AB6000A78AA0 AS DateTime), CAST(0x0000AB60013D3AF0 AS DateTime), 1, CAST(0x0000AB82010B061C AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (84, 115, NULL, CAST(0x0000AB6100A43EE0 AS DateTime), CAST(0x0000AB61014159A0 AS DateTime), 1, CAST(0x0000AB82010B6DC6 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (85, 116, NULL, CAST(0x0000AB6100A78AA0 AS DateTime), CAST(0x0000AB61014AF690 AS DateTime), 1, CAST(0x0000AB82010B9451 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (86, 117, NULL, CAST(0x0000AB6400A8EA30 AS DateTime), CAST(0x0000AB64014F1540 AS DateTime), 1, CAST(0x0000AB82010E0286 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (87, 118, NULL, CAST(0x0000AB6400A62B10 AS DateTime), CAST(0x0000AB64012A0110 AS DateTime), 1, CAST(0x0000AB82010E140B AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (88, 119, NULL, CAST(0x0000AB6500A8EA30 AS DateTime), CAST(0x0000AB65013FFA10 AS DateTime), 1, CAST(0x0000AB82010E8485 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (89, 120, NULL, CAST(0x0000AB6600A36BF0 AS DateTime), CAST(0x0000AB66014345D0 AS DateTime), 1, CAST(0x0000AB82010EB0B2 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (90, 121, NULL, CAST(0x0000AB6600A8EA30 AS DateTime), CAST(0x0000AB66015B7150 AS DateTime), 1, CAST(0x0000AB82010EC484 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (91, 122, NULL, CAST(0x0000AB6700A78AA0 AS DateTime), CAST(0x0000AB6701457850 AS DateTime), 1, CAST(0x0000AB82010F137C AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (92, 123, NULL, CAST(0x0000AB6800A62B10 AS DateTime), CAST(0x0000AB68014418C0 AS DateTime), 1, CAST(0x0000AB8201106D42 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (93, 124, NULL, CAST(0x0000AB6800A8EA30 AS DateTime), CAST(0x0000AB68014418C0 AS DateTime), 1, CAST(0x0000AB8201108356 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (94, 125, NULL, CAST(0x0000AB6800AA49C0 AS DateTime), CAST(0x0000AB68014418C0 AS DateTime), 1, CAST(0x0000AB8201108C61 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (95, 126, NULL, CAST(0x0000AB6B00A4CB80 AS DateTime), CAST(0x0000AB6B0146D7E0 AS DateTime), 1, CAST(0x0000AB820110B9E1 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (96, 127, NULL, CAST(0x0000AB6B00A4CB80 AS DateTime), CAST(0x0000AB6B013FFA10 AS DateTime), 1, CAST(0x0000AB820110C806 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (97, 128, NULL, CAST(0x0000AB6B00A4CB80 AS DateTime), CAST(0x0000AB6B014D2910 AS DateTime), 1, CAST(0x0000AB820110D40A AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (98, 129, NULL, CAST(0x0000AB6C00A4CB80 AS DateTime), CAST(0x0000AB6C014F1540 AS DateTime), 1, CAST(0x0000AB820110E9D8 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (99, 130, NULL, CAST(0x0000AB6C00A8EA30 AS DateTime), CAST(0x0000AB6C0151D460 AS DateTime), 1, CAST(0x0000AB820110F911 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (100, 131, NULL, CAST(0x0000AB6D00A78AA0 AS DateTime), CAST(0x0000AB6D01483770 AS DateTime), 1, CAST(0x0000AB820111221C AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (101, 132, NULL, CAST(0x0000AB6D00A78AA0 AS DateTime), CAST(0x0000AB6D01499700 AS DateTime), 1, CAST(0x0000AB8201112E95 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (102, 133, NULL, CAST(0x0000AB6D00AB1CB0 AS DateTime), CAST(0x0000AB6D014159A0 AS DateTime), 1, CAST(0x0000AB8201114154 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (103, 134, NULL, CAST(0x0000AB6E00A59E70 AS DateTime), CAST(0x0000AB6E013B0870 AS DateTime), 1, CAST(0x0000AB8201115CEE AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (104, 135, NULL, CAST(0x0000AB6E00A6B7B0 AS DateTime), CAST(0x0000AB6E013F6D70 AS DateTime), 1, CAST(0x0000AB8201116B58 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (105, 136, NULL, CAST(0x0000AB6E00EFC6D0 AS DateTime), CAST(0x0000AB6E013F6D70 AS DateTime), 1, CAST(0x0000AB82011181BE AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (106, 137, NULL, CAST(0x0000AB6F00A62B10 AS DateTime), CAST(0x0000AB6F014DB5B0 AS DateTime), 1, CAST(0x0000AB820111A27E AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (107, 138, NULL, CAST(0x0000AB6F00A78AA0 AS DateTime), CAST(0x0000AB6F01499700 AS DateTime), 1, CAST(0x0000AB820111CDE8 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (108, 139, NULL, CAST(0x0000AB6F00BD3D50 AS DateTime), CAST(0x0000AB6F01549380 AS DateTime), 1, CAST(0x0000AB820111E091 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (109, 140, NULL, CAST(0x0000AB7000A0ACD0 AS DateTime), CAST(0x0000AB700107AC00 AS DateTime), 1, CAST(0x0000AB82011204FF AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (110, 141, NULL, CAST(0x0000AB7000AFC800 AS DateTime), CAST(0x0000AB70013E9A80 AS DateTime), 1, CAST(0x0000AB8201124DCD AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (111, 142, NULL, CAST(0x0000AB7000F890D0 AS DateTime), CAST(0x0000AB700176E890 AS DateTime), 1, CAST(0x0000AB82011281E8 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (112, 142, NULL, CAST(0x0000AB7000F890D0 AS DateTime), CAST(0x0000AB700176E890 AS DateTime), 1, CAST(0x0000AB820112CF23 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (113, 142, NULL, CAST(0x0000AB7000F890D0 AS DateTime), CAST(0x0000AB700176E890 AS DateTime), 1, CAST(0x0000AB820113BC87 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (114, 143, NULL, CAST(0x0000AB7200A20C60 AS DateTime), CAST(0x0000AB72013E9A80 AS DateTime), 1, CAST(0x0000AB8201140F5D AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (115, 144, NULL, CAST(0x0000AB7200A62B10 AS DateTime), CAST(0x0000AB7201064C70 AS DateTime), 1, CAST(0x0000AB8201148EA8 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (116, 145, NULL, CAST(0x0000AB7300A4CB80 AS DateTime), CAST(0x0000AB73013E9A80 AS DateTime), 1, CAST(0x0000AB820114B678 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (117, 146, NULL, CAST(0x0000AB7300ABA950 AS DateTime), CAST(0x0000AB73013E9A80 AS DateTime), 1, CAST(0x0000AB82011508B9 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (118, 147, NULL, CAST(0x0000AB7400A36BF0 AS DateTime), CAST(0x0000AB74012F7F50 AS DateTime), 1, CAST(0x0000AB820115306B AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (119, 148, NULL, CAST(0x0000AB7400AAD660 AS DateTime), CAST(0x0000AB7401373010 AS DateTime), 1, CAST(0x0000AB820115456E AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (120, 149, NULL, CAST(0x0000AB7400D21D10 AS DateTime), CAST(0x0000AB740151D460 AS DateTime), 1, CAST(0x0000AB8201155C85 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (121, 147, NULL, CAST(0x0000AB7400A78AA0 AS DateTime), CAST(0x0000AB74015333F0 AS DateTime), 1, CAST(0x0000AB8201159EBB AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (122, 148, NULL, CAST(0x0000AB7400B54640 AS DateTime), CAST(0x0000AB74015E3070 AS DateTime), 1, CAST(0x0000AB820115C488 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (123, 149, NULL, CAST(0x0000AB7400F73140 AS DateTime), CAST(0x0000AB7401624F20 AS DateTime), 1, CAST(0x0000AB820115DB4C AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (124, 150, NULL, CAST(0x0000AB7600A20C60 AS DateTime), CAST(0x0000AB76014AF690 AS DateTime), 1, CAST(0x0000AB82011642BC AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (125, 151, NULL, CAST(0x0000AB7600A8EA30 AS DateTime), CAST(0x0000AB76014159A0 AS DateTime), 1, CAST(0x0000AB820116558C AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (126, 152, NULL, CAST(0x0000AB7900A36BF0 AS DateTime), CAST(0x0000AB79014AF690 AS DateTime), 1, CAST(0x0000AB82011693BD AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (127, 153, NULL, CAST(0x0000AB7900D8FAE0 AS DateTime), CAST(0x0000AB79016062F0 AS DateTime), 1, CAST(0x0000AB820116AC1F AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (128, 154, NULL, CAST(0x0000AB7A00A62B10 AS DateTime), CAST(0x0000AB7A0139EF30 AS DateTime), 1, CAST(0x0000AB820116D5D0 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (129, 155, NULL, CAST(0x0000AB7A00A4CB80 AS DateTime), CAST(0x0000AB7A0139EF30 AS DateTime), 1, CAST(0x0000AB820116EDAD AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (130, 156, NULL, CAST(0x0000AB7B00A4CB80 AS DateTime), CAST(0x0000AB7B01419FF0 AS DateTime), 1, CAST(0x0000AB8201170EC6 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (131, 157, NULL, CAST(0x0000AB7B00AA49C0 AS DateTime), CAST(0x0000AB7B01419FF0 AS DateTime), 1, CAST(0x0000AB8201171A7C AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (132, 158, NULL, CAST(0x0000AB7C00A78AA0 AS DateTime), CAST(0x0000AB7C013A7BD0 AS DateTime), 1, CAST(0x0000AB82011771D6 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (133, 159, NULL, CAST(0x0000AB7C00A78AA0 AS DateTime), CAST(0x0000AB7C013A7BD0 AS DateTime), 1, CAST(0x0000AB820117803D AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (134, 160, NULL, CAST(0x0000AB7C00C60750 AS DateTime), CAST(0x0000AB7C01549380 AS DateTime), 1, CAST(0x0000AB8201179C4F AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (135, 161, NULL, CAST(0x0000AB7D00A62B10 AS DateTime), CAST(0x0000AB7D016BEC10 AS DateTime), 1, CAST(0x0000AB820117C110 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (136, 162, NULL, CAST(0x0000AB7D00A78AA0 AS DateTime), CAST(0x0000AB7D016D4BA0 AS DateTime), 1, CAST(0x0000AB820117CF61 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (137, 163, NULL, CAST(0x0000AB7D00B28720 AS DateTime), CAST(0x0000AB7D016D4BA0 AS DateTime), 1, CAST(0x0000AB820117E16A AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (138, 164, NULL, CAST(0x0000AB7E00B28720 AS DateTime), CAST(0x0000AB7E011F9130 AS DateTime), 1, CAST(0x0000AB8201180D60 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (139, 165, NULL, CAST(0x0000AB7E00ED9450 AS DateTime), CAST(0x0000AB7E012CC030 AS DateTime), 1, CAST(0x0000AB8201182707 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (140, 166, NULL, CAST(0x0000AB7F00A4CB80 AS DateTime), CAST(0x0000AB7F014345D0 AS DateTime), 1, CAST(0x0000AB8201185B85 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (141, 167, NULL, CAST(0x0000AB7F00A62B10 AS DateTime), CAST(0x0000AB7F014345D0 AS DateTime), 1, CAST(0x0000AB820118648B AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (142, 168, NULL, CAST(0x0000AB8000A62B10 AS DateTime), CAST(0x0000AB80014345D0 AS DateTime), 1, CAST(0x0000AB820118BDD4 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (143, 169, NULL, CAST(0x0000AB8000A62B10 AS DateTime), CAST(0x0000AB80014345D0 AS DateTime), 1, CAST(0x0000AB820118C18F AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (144, 170, NULL, CAST(0x0000AB8100A85D90 AS DateTime), CAST(0x0000AB81014159A0 AS DateTime), 1, CAST(0x0000AB820118EA81 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (145, 171, NULL, CAST(0x0000AB8100ADDBD0 AS DateTime), CAST(0x0000AB81014159A0 AS DateTime), 1, CAST(0x0000AB820118F5B6 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (146, 172, NULL, CAST(0x0000AB8100ACC290 AS DateTime), CAST(0x0000AB81014159A0 AS DateTime), 1, CAST(0x0000AB820119069D AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (147, 165, NULL, CAST(0x0000AB7E00ED9450 AS DateTime), CAST(0x0000AB7E00ED9450 AS DateTime), 1, CAST(0x0000AB820137A9F2 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (148, 164, NULL, CAST(0x0000AB7E00A8EA30 AS DateTime), CAST(0x0000AB7E00A8EA30 AS DateTime), 1, CAST(0x0000AB820137E583 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (149, 164, NULL, CAST(0x0000AB7E00A8EA30 AS DateTime), CAST(0x0000AB7E00A8EA30 AS DateTime), 1, CAST(0x0000AB82013861C5 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (150, 164, NULL, CAST(0x0000AB7E00A8EA30 AS DateTime), CAST(0x0000AB7E00A8EA30 AS DateTime), 1, CAST(0x0000AB820139D91F AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (151, 164, NULL, CAST(0x0000AB7E00A8EA30 AS DateTime), CAST(0x0000AB7E00A8EA30 AS DateTime), 1, CAST(0x0000AB82013B5190 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (152, 164, NULL, CAST(0x0000AB7E00A8EA30 AS DateTime), CAST(0x0000AB7E00A8EA30 AS DateTime), 1, CAST(0x0000AB82013BD9C4 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (153, 164, NULL, CAST(0x0000AB7E00A8EA30 AS DateTime), CAST(0x0000AB7E00A8EA30 AS DateTime), 1, CAST(0x0000AB82013CBAC9 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (154, 164, NULL, CAST(0x0000AB7E00A8EA30 AS DateTime), CAST(0x0000AB7E00A8EA30 AS DateTime), 1, CAST(0x0000AB82013CDD00 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (155, 164, NULL, CAST(0x0000AB7E00A8EA30 AS DateTime), CAST(0x0000AB7E00A8EA30 AS DateTime), 1, CAST(0x0000AB82013D1D98 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (156, 164, NULL, CAST(0x0000AB7E00A8EA30 AS DateTime), CAST(0x0000AB7E00A8EA30 AS DateTime), 1, CAST(0x0000AB82013D3F8E AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (157, 164, NULL, CAST(0x0000AB7E00A8EA30 AS DateTime), CAST(0x0000AB7E00A8EA30 AS DateTime), 1, CAST(0x0000AB82013DBE49 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (158, 173, NULL, CAST(0x0000AB8900A4CB80 AS DateTime), CAST(0x0000AB8900D63BC0 AS DateTime), 1, CAST(0x0000AB8900BEF884 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (159, 173, NULL, CAST(0x0000AB8900F73140 AS DateTime), CAST(0x0000AB8901499700 AS DateTime), 1, CAST(0x0000AB8900BF33FA AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (160, 174, NULL, CAST(0x0000AB8900000000 AS DateTime), CAST(0x0000AB8900000000 AS DateTime), 1, CAST(0x0000AB8900CB4281 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (161, 175, NULL, CAST(0x0000AB8900A4CB80 AS DateTime), CAST(0x0000AB8900E6B680 AS DateTime), 1, CAST(0x0000AB89010A2716 AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (162, 175, NULL, CAST(0x0000AB890107AC00 AS DateTime), CAST(0x0000AB8901499700 AS DateTime), 1, CAST(0x0000AB89010A7FCE AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (163, 176, NULL, CAST(0x0000AB8900A4CB80 AS DateTime), CAST(0x0000AB89011826C0 AS DateTime), 1, CAST(0x0000AB890123E30F AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (164, 173, NULL, CAST(0x0000AB89009C8E20 AS DateTime), CAST(0x0000AB890151D460 AS DateTime), 1, CAST(0x0000AB8901269D51 AS DateTime), NULL, 1, NULL)
GO
SET IDENTITY_INSERT [dbo].[AttendanceDetail] OFF
GO
SET IDENTITY_INSERT [dbo].[AttendancePolicy] ON 

GO
INSERT [dbo].[AttendancePolicy] ([ID], [ShiftID], [AttendanceVariableID], [Hours], [Description], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, 2, 7, CAST(0.50 AS Decimal(18, 2)), N'consider left early, if left half hour before shift end time', CAST(0x0000AB5400000000 AS DateTime), NULL, CAST(0x0000AB8200B7FA0C AS DateTime), CAST(0x0000AB8200D443D8 AS DateTime), 1, N'::1')
GO
INSERT [dbo].[AttendancePolicy] ([ID], [ShiftID], [AttendanceVariableID], [Hours], [Description], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, 2, 8, CAST(0.50 AS Decimal(18, 2)), N'consider came late, if came half hour after shift start time', CAST(0x0000AB5400000000 AS DateTime), NULL, CAST(0x0000AB8200B7FA22 AS DateTime), CAST(0x0000AB8200D443D8 AS DateTime), 1, N'::1')
GO
INSERT [dbo].[AttendancePolicy] ([ID], [ShiftID], [AttendanceVariableID], [Hours], [Description], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (3, 3, 7, CAST(0.50 AS Decimal(18, 2)), NULL, CAST(0x0000AB5400000000 AS DateTime), NULL, CAST(0x0000AB8200B80EC7 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[AttendancePolicy] ([ID], [ShiftID], [AttendanceVariableID], [Hours], [Description], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (4, 3, 8, CAST(0.50 AS Decimal(18, 2)), NULL, CAST(0x0000AB5400000000 AS DateTime), NULL, CAST(0x0000AB8200B80ED2 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[AttendancePolicy] ([ID], [ShiftID], [AttendanceVariableID], [Hours], [Description], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (5, 4, 1, CAST(4.00 AS Decimal(18, 2)), NULL, CAST(0x0000AB5400000000 AS DateTime), NULL, CAST(0x0000AB8200B82CCE AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[AttendancePolicy] ([ID], [ShiftID], [AttendanceVariableID], [Hours], [Description], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (6, 4, 2, CAST(2.00 AS Decimal(18, 2)), NULL, CAST(0x0000AB5400000000 AS DateTime), NULL, CAST(0x0000AB8200B82CD9 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[AttendancePolicy] ([ID], [ShiftID], [AttendanceVariableID], [Hours], [Description], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (7, 4, 7, CAST(1.00 AS Decimal(18, 2)), NULL, CAST(0x0000AB5400000000 AS DateTime), NULL, CAST(0x0000AB8200B82CEA AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[AttendancePolicy] ([ID], [ShiftID], [AttendanceVariableID], [Hours], [Description], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (8, 4, 8, CAST(1.00 AS Decimal(18, 2)), NULL, CAST(0x0000AB5400000000 AS DateTime), NULL, CAST(0x0000AB8200B82D04 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[AttendancePolicy] ([ID], [ShiftID], [AttendanceVariableID], [Hours], [Description], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (9, 5, 7, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0x0000AB5400000000 AS DateTime), NULL, CAST(0x0000AB8200B83DB4 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[AttendancePolicy] ([ID], [ShiftID], [AttendanceVariableID], [Hours], [Description], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (10, 5, 8, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0x0000AB5400000000 AS DateTime), NULL, CAST(0x0000AB8200B83DB9 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[AttendancePolicy] ([ID], [ShiftID], [AttendanceVariableID], [Hours], [Description], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (11, 6, 7, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0x0000AB5400000000 AS DateTime), NULL, CAST(0x0000AB8200B85055 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[AttendancePolicy] ([ID], [ShiftID], [AttendanceVariableID], [Hours], [Description], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (12, 6, 8, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0x0000AB5400000000 AS DateTime), NULL, CAST(0x0000AB8200B85059 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[AttendancePolicy] ([ID], [ShiftID], [AttendanceVariableID], [Hours], [Description], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (13, 7, 1, CAST(4.00 AS Decimal(18, 2)), NULL, CAST(0x0000AB8900000000 AS DateTime), NULL, CAST(0x0000AB8900BCF527 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[AttendancePolicy] ([ID], [ShiftID], [AttendanceVariableID], [Hours], [Description], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (14, 7, 2, CAST(2.00 AS Decimal(18, 2)), NULL, CAST(0x0000AB8900000000 AS DateTime), NULL, CAST(0x0000AB8900BCF530 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[AttendancePolicy] ([ID], [ShiftID], [AttendanceVariableID], [Hours], [Description], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (15, 7, 7, CAST(0.50 AS Decimal(18, 2)), NULL, CAST(0x0000AB8900000000 AS DateTime), NULL, CAST(0x0000AB8900BCF535 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[AttendancePolicy] ([ID], [ShiftID], [AttendanceVariableID], [Hours], [Description], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (16, 7, 8, CAST(0.50 AS Decimal(18, 2)), NULL, CAST(0x0000AB8900000000 AS DateTime), NULL, CAST(0x0000AB8900BCF537 AS DateTime), NULL, 1, N'::1')
GO
SET IDENTITY_INSERT [dbo].[AttendancePolicy] OFF
GO
SET IDENTITY_INSERT [dbo].[AttendanceStatus] ON 

GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (1, 34, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB8200FDB463 AS DateTime), NULL, 1, N'', 0, 0, 12, 0, 636, 636, 108, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (2, 35, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB8200FE1E7D AS DateTime), NULL, 1, N'', 0, 0, 5, 0, 725, 725, 190, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (3, 36, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB8200FE7552 AS DateTime), NULL, 1, N'', 1, 0, 240, 0, 510, 510, 210, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (4, 37, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB8200FECDCC AS DateTime), NULL, 1, N'', 0, 0, 5, 0, 570, 570, 35, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (5, 38, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB8200FEE088 AS DateTime), NULL, 1, N'', 0, 0, 10, 0, 551, 551, 21, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (7, 40, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB8200FFF4B8 AS DateTime), NULL, 1, N'', 0, 0, 19, 0, 541, 541, 20, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (8, 41, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB8201000239 AS DateTime), NULL, 1, N'', 0, 0, 19, 0, 541, 541, 20, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (9, 42, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB8201002A2E AS DateTime), NULL, 1, N'', 1, 0, 90, 0, 495, 495, 45, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (10, 43, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB8201005824 AS DateTime), NULL, 1, N'', 0, 0, 11, 0, 549, 549, 20, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (11, 44, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB8201006522 AS DateTime), NULL, 1, N'', 0, 0, 26, 0, 534, 534, 20, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (12, 45, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB82010081F9 AS DateTime), NULL, 1, N'', 1, 0, 235, 0, 430, 430, 125, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (13, 46, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820100B788 AS DateTime), NULL, 1, N'', 0, 0, 5, 0, 761, 761, 226, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (14, 47, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820100D1C4 AS DateTime), NULL, 1, N'', 1, 0, 80, 0, 670, 670, 210, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (15, 48, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820100D9E6 AS DateTime), NULL, 1, N'', 1, 0, 80, 0, 670, 670, 210, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (16, 49, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820100FE9B AS DateTime), NULL, 1, N'', 0, 0, 15, 0, 660, 660, 135, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (17, 50, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB8201011551 AS DateTime), NULL, 1, N'', 1, 0, 40, 0, 603, 603, 103, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (18, 51, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB8201013304 AS DateTime), NULL, 1, N'', 1, 0, 295, 0, 425, 425, 180, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (19, 52, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB8201015FBA AS DateTime), NULL, 1, N'', 0, 0, 13, 0, 675, 675, 148, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (20, 53, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820101769A AS DateTime), NULL, 1, N'', 0, 0, 17, 0, 656, 656, 133, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (21, 54, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB8201018E99 AS DateTime), NULL, 1, N'', 0, 0, 0, 0, 1380, 1380, 240, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (22, 55, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820101BB22 AS DateTime), NULL, 1, N'', 0, 0, 5, 0, 643, 643, 108, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (23, 56, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820101DA3E AS DateTime), NULL, 1, N'', 1, 0, 320, 0, 355, 355, 135, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (24, 57, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820101F907 AS DateTime), NULL, 1, N'', 0, 0, 0, 0, 620, 620, 80, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (25, 58, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB8201020AB4 AS DateTime), NULL, 1, N'', 1, 0, 50, 0, 570, 570, 80, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (26, 59, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB8201022831 AS DateTime), NULL, 1, N'', 0, 0, 15, 0, 720, 720, 195, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (27, 60, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB8201023BDD AS DateTime), NULL, 1, N'', 0, 0, 25, 0, 590, 590, 75, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (28, 61, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB8201024CD9 AS DateTime), NULL, 1, N'', 1, 0, 30, 0, 585, 585, 75, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (29, 62, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820102AF10 AS DateTime), NULL, 1, N'', 0, 0, 5, 0, 575, 575, 40, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (30, 63, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820102C432 AS DateTime), NULL, 1, N'', 1, 0, 35, 0, 527, 527, 22, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (31, 64, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820102D98E AS DateTime), NULL, 1, N'', 1, 0, 180, 0, 555, 555, 195, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (32, 65, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820103A8ED AS DateTime), NULL, 1, N'', 1, 1, 15, 0, 585, 585, 60, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (33, 66, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820103D96E AS DateTime), NULL, 1, N'', 1, 1, 20, 0, 580, 580, 60, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (34, 67, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820103F44D AS DateTime), NULL, 1, N'', 1, 1, 135, 0, 551, 551, 146, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (35, 68, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB8201041E0B AS DateTime), NULL, 1, N'', 1, 1, 18, 0, 654, 654, 132, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (36, 69, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820104A3BC AS DateTime), NULL, 1, N'', 1, 1, 115, 0, 580, 580, 155, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (37, 70, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820104E635 AS DateTime), NULL, 1, N'', 1, 1, 10, 0, 610, 610, 80, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (38, 71, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820104F9CC AS DateTime), NULL, 1, N'', 1, 1, 44, 0, 576, 576, 80, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (39, 72, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB8201050D33 AS DateTime), NULL, 1, N'', 1, 1, 0, 0, 1160, 1160, 80, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (40, 73, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB82010565BC AS DateTime), NULL, 1, N'', 1, 1, 20, 0, 550, 550, 30, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (41, 74, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB8201057CC1 AS DateTime), NULL, 1, N'', 1, 1, 45, 0, 525, 525, 30, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (42, 75, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820105984B AS DateTime), NULL, 1, N'', 1, 1, 130, 0, 440, 440, 30, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (43, 76, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820105C1BC AS DateTime), NULL, 1, N'', 1, 1, 26, 0, 624, 624, 110, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (44, 77, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820105CCED AS DateTime), NULL, 1, N'', 1, 1, 42, 0, 608, 608, 110, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (45, 78, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820105D7D2 AS DateTime), NULL, 1, N'', 1, 1, 50, 0, 600, 600, 110, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (46, 79, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820105F488 AS DateTime), NULL, 1, N'', 1, 1, 20, 0, 610, 610, 90, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (47, 80, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820106128E AS DateTime), NULL, 1, N'', 1, 1, 20, 0, 670, 670, 150, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (48, 81, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB82010621F7 AS DateTime), NULL, 1, N'', 1, 1, 15, 0, 675, 675, 150, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (49, 82, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB8201062F27 AS DateTime), NULL, 1, N'', 1, 1, 50, 0, 640, 640, 150, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (50, 83, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB8201065A2F AS DateTime), NULL, 1, N'', 1, 1, 15, 0, 735, 735, 210, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (51, 84, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820106670F AS DateTime), NULL, 1, N'', 1, 1, 30, 0, 720, 720, 210, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (52, 85, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB8201066EED AS DateTime), NULL, 1, N'', 1, 1, 55, 0, 695, 695, 210, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (53, 86, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820106AB45 AS DateTime), NULL, 1, N'', 1, 1, 34, 0, 636, 636, 130, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (54, 87, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820106B737 AS DateTime), NULL, 1, N'', 1, 1, 10, 0, 700, 700, 170, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (55, 88, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820106C81B AS DateTime), NULL, 1, N'', 1, 1, 320, 0, 390, 390, 170, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (56, 89, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820106F6B7 AS DateTime), NULL, 1, N'', 1, 1, 18, 0, 592, 592, 70, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (57, 90, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB8201070D1C AS DateTime), NULL, 1, N'', 1, 1, 100, 0, 1010, 510, 70, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (58, 91, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB8201073B1E AS DateTime), NULL, 1, N'', 0, 0, 15, 0, 605, 605, 80, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (59, 92, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB82010760ED AS DateTime), NULL, 1, N'', 0, 0, 5, 0, 535, 535, 0, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (60, 93, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB8201077D1A AS DateTime), NULL, 1, N'', 1, 0, 100, 0, 535, 535, 95, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (61, 94, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820107BA9F AS DateTime), NULL, 1, N'', 0, 0, 0, 0, 605, 605, 65, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (62, 95, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820107D1E8 AS DateTime), NULL, 1, N'', 0, 0, 12, 0, 568, 568, 40, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (63, 96, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820107E05D AS DateTime), NULL, 1, N'', 0, 0, 12, 0, 568, 568, 40, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (64, 97, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820108128F AS DateTime), NULL, 1, N'', 0, 0, 0, 0, 620, 620, 80, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (65, 98, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB82010824E4 AS DateTime), NULL, 1, N'', 0, 0, 15, 0, 667, 667, 142, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (66, 99, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB8201085EB1 AS DateTime), NULL, 1, N'', 0, 0, 100, 0, 540, 540, 100, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (67, 100, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820108E744 AS DateTime), NULL, 1, N'', 0, 0, 0, 0, 545, 545, 5, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (68, 101, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820108F3FC AS DateTime), NULL, 1, N'', 0, 0, 20, 0, 525, 525, 5, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (69, 102, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB8201094F4A AS DateTime), NULL, 1, N'', 0, 0, 0, 0, 696, 696, 156, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (70, 103, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB8201096270 AS DateTime), NULL, 1, N'', 0, 0, 100, 0, 596, 596, 156, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (71, 104, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB8201097148 AS DateTime), NULL, 1, N'', 0, 0, 210, 0, 486, 486, 156, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (72, 105, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB8201099292 AS DateTime), NULL, 1, N'', 0, 0, 0, 0, 585, 585, 45, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (73, 106, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820109DE5D AS DateTime), NULL, 1, N'', 0, 0, 0, 0, 586, 586, 46, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (74, 107, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB82010A08CF AS DateTime), NULL, 1, N'', 0, 0, 70, 0, 605, 605, 135, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (75, 108, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB82010A3A4C AS DateTime), NULL, 1, N'', 0, 0, 0, 0, 540, 540, 0, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (76, 109, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB82010A495E AS DateTime), NULL, 1, N'', 0, 0, 35, 0, 525, 525, 20, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (77, 110, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB82010A6A81 AS DateTime), NULL, 1, N'', 0, 0, 0, 0, 710, 710, 170, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (78, 111, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB82010A7E08 AS DateTime), NULL, 1, N'', 0, 0, 275, 0, 505, 505, 240, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (79, 112, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB82010AC80F AS DateTime), NULL, 1, N'', 0, 0, 10, 0, 545, 545, 15, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (80, 113, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB82010AD694 AS DateTime), NULL, 1, N'', 0, 0, 190, 0, 365, 365, 15, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (81, 114, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB82010AF492 AS DateTime), NULL, 1, N'', 0, 0, 45, 0, 1105, 570, 75, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (82, 115, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB82010B6DDD AS DateTime), NULL, 1, N'', 0, 0, 58, 0, 572, 572, 90, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (83, 116, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB82010B945C AS DateTime), NULL, 1, N'', 0, 0, 10, 0, 595, 595, 65, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (84, 117, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB82010E02DB AS DateTime), NULL, 1, N'', 0, 0, 15, 0, 605, 605, 80, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (85, 118, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB82010E1425 AS DateTime), NULL, 1, N'', 0, 0, 65, 0, 480, 480, 5, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (86, 119, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB82010E8496 AS DateTime), NULL, 1, N'', 0, 0, 15, 0, 550, 550, 25, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (87, 120, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB82010EB0BE AS DateTime), NULL, 1, N'', 0, 0, 55, 0, 582, 582, 97, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (88, 121, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB82010EC493 AS DateTime), NULL, 1, N'', 0, 0, 15, 0, 650, 650, 125, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (89, 122, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB82010F138B AS DateTime), NULL, 1, N'', 0, 0, 10, 0, 575, 575, 45, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (90, 123, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB8201106D4F AS DateTime), NULL, 1, N'', 0, 0, 5, 0, 575, 575, 40, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (91, 124, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB8201108362 AS DateTime), NULL, 1, N'', 0, 0, 15, 0, 565, 565, 40, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (92, 125, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB8201108C6C AS DateTime), NULL, 1, N'', 0, 0, 80, 0, 560, 560, 100, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (93, 126, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820110B9F0 AS DateTime), NULL, 1, N'', 0, 0, 0, 0, 590, 590, 50, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (94, 127, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820110C814 AS DateTime), NULL, 1, N'', 0, 0, 60, 0, 565, 565, 85, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (95, 128, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820110D418 AS DateTime), NULL, 1, N'', 0, 0, 0, 0, 613, 613, 73, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (96, 129, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820110E9E7 AS DateTime), NULL, 1, N'', 0, 0, 0, 0, 620, 620, 80, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (97, 130, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820110F91C AS DateTime), NULL, 1, N'', 0, 0, 15, 0, 615, 615, 90, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (98, 131, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820111222D AS DateTime), NULL, 1, N'', 0, 0, 10, 0, 585, 585, 55, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (99, 132, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB8201112EA3 AS DateTime), NULL, 1, N'', 0, 0, 10, 0, 590, 590, 60, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (100, 133, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB8201114170 AS DateTime), NULL, 1, N'', 0, 0, 83, 0, 547, 547, 90, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (101, 134, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB8201115CFC AS DateTime), NULL, 1, N'', 0, 0, 3, 0, 544, 544, 7, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (102, 135, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB8201116B62 AS DateTime), NULL, 1, N'', 0, 0, 7, 0, 556, 556, 23, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (103, 136, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB82011181C8 AS DateTime), NULL, 1, N'', 0, 0, 333, 0, 290, 290, 83, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (104, 137, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820111A297 AS DateTime), NULL, 1, N'', 0, 0, 5, 0, 610, 610, 75, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (105, 138, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820111CDF9 AS DateTime), NULL, 1, N'', 0, 0, 10, 0, 590, 590, 60, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (106, 139, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820111E09B AS DateTime), NULL, 1, N'', 0, 0, 149, 0, 551, 551, 160, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (107, 140, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB8201120508 AS DateTime), NULL, 1, N'', 0, 0, 0, 180, 375, 375, 0, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (108, 141, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB8201124DDB AS DateTime), NULL, 1, N'', 0, 0, 40, 0, 520, 520, 20, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (110, 142, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820113BC94 AS DateTime), NULL, 1, N'', 0, 0, 365, 0, 460, 460, 285, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (111, 143, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB8201140F6C AS DateTime), NULL, 1, N'', 0, 0, 0, 0, 570, 570, 20, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (112, 144, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB8201148EB9 AS DateTime), NULL, 1, N'', 0, 0, 5, 185, 350, 350, 0, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (113, 145, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820114B68A AS DateTime), NULL, 1, N'', 0, 0, 0, 0, 560, 560, 20, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (114, 146, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB82011508C3 AS DateTime), NULL, 1, N'', 0, 0, 25, 0, 535, 535, 20, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (115, 147, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820115307A AS DateTime), NULL, 1, N'', 0, 0, 10, 0, 1135, 640, 95, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (116, 148, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB8201154582 AS DateTime), NULL, 1, N'', 0, 0, 22, 0, 1126, 653, 135, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (117, 149, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB8201155C93 AS DateTime), NULL, 1, N'', 0, 0, 225, 0, 855, 525, 210, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (118, 150, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB82011642CA AS DateTime), NULL, 1, N'', 0, 0, 0, 0, 615, 615, 65, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (119, 151, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820116559B AS DateTime), NULL, 1, N'', 0, 0, 15, 0, 555, 555, 30, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (120, 152, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB82011693CB AS DateTime), NULL, 1, N'', 0, 0, 0, 0, 610, 610, 65, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (121, 153, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820116AC2C AS DateTime), NULL, 1, N'', 0, 0, 250, 0, 493, 493, 203, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (122, 154, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820116D5DB AS DateTime), NULL, 1, N'', 0, 0, 5, 0, 538, 538, 3, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (123, 155, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820116EDC0 AS DateTime), NULL, 1, N'', 0, 0, 0, 0, 543, 543, 3, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (124, 156, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB8201170ED1 AS DateTime), NULL, 1, N'', 0, 0, 0, 0, 571, 571, 31, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (125, 157, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB8201171A87 AS DateTime), NULL, 1, N'', 0, 0, 20, 0, 551, 551, 31, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (126, 158, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB82011771EB AS DateTime), NULL, 1, N'', 0, 0, 10, 0, 535, 535, 5, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (127, 159, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820117806D AS DateTime), NULL, 1, N'', 0, 0, 10, 0, 535, 535, 5, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (128, 160, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB8201179C63 AS DateTime), NULL, 1, N'', 0, 0, 181, 0, 519, 519, 160, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (129, 161, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820117C11D AS DateTime), NULL, 1, N'', 0, 0, 5, 0, 720, 720, 185, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (130, 162, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820117CF78 AS DateTime), NULL, 1, N'', 0, 0, 10, 0, 720, 720, 190, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (131, 163, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820117E175 AS DateTime), NULL, 1, N'', 0, 0, 110, 0, 680, 680, 250, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (132, 164, 1, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB8201180D6A AS DateTime), NULL, 1, N'', 0, 0, 50, 525, 397, 432, 0, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (133, 165, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB8201182712 AS DateTime), NULL, 1, N'', 0, 0, 325, 215, 230, 230, 15, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (134, 166, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB8201185B94 AS DateTime), NULL, 1, N'', 0, 0, 0, 0, 577, 577, 37, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (135, 167, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB8201186496 AS DateTime), NULL, 1, N'', 0, 0, 5, 0, 572, 572, 37, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (136, 168, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820118BDE2 AS DateTime), NULL, 1, N'', 0, 0, 5, 0, 572, 572, 37, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (137, 169, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820118C19A AS DateTime), NULL, 1, N'', 0, 0, 5, 0, 572, 572, 37, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (138, 170, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820118EA8D AS DateTime), NULL, 1, N'', 0, 0, 13, 0, 557, 557, 30, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (139, 171, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB820118F5C5 AS DateTime), NULL, 1, N'', 0, 0, 33, 0, 537, 537, 30, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (140, 172, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB82011906AC AS DateTime), NULL, 1, N'', 0, 0, 89, 0, 541, 541, 90, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (141, 173, 0, 0, 0, 0, 1, 1, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB8900BEF89C AS DateTime), NULL, 1, N'', 1, 0, 120, 0, 1140, 660, 210, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (142, 174, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB8900CB4341 AS DateTime), NULL, 1, N'', 0, 1, 0, 1140, 0, 0, 0, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (143, 175, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB89010A27CD AS DateTime), NULL, 1, N'', 1, 0, 60, 0, 480, 600, 120, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (144, 176, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB890123E3BB AS DateTime), NULL, 1, N'', 0, 1, 0, 120, 420, 420, 0, NULL)
GO
SET IDENTITY_INSERT [dbo].[AttendanceStatus] OFF
GO
SET IDENTITY_INSERT [dbo].[AttendanceType] ON 

GO
INSERT [dbo].[AttendanceType] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, N'Daily Attendance', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[AttendanceType] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, N'Lunch', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[AttendanceType] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (3, N'Tea', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[AttendanceType] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (4, N'Prayers', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[AttendanceType] OFF
GO
SET IDENTITY_INSERT [dbo].[AttendanceVariable] ON 

GO
INSERT [dbo].[AttendanceVariable] ([ID], [Name], [Description], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, N'Full Day', N'', 1, CAST(0x0000AB5D0174B079 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[AttendanceVariable] ([ID], [Name], [Description], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, N'Half Day', N'', 1, CAST(0x0000AB5D0174E472 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[AttendanceVariable] ([ID], [Name], [Description], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (4, N'Overtime', N'', 1, CAST(0x0000AB5D0174B079 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[AttendanceVariable] ([ID], [Name], [Description], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (5, N'Compensatory', N'', 1, CAST(0x0000AB5D0174E472 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[AttendanceVariable] ([ID], [Name], [Description], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (6, N'Standard', N'', 1, CAST(0x0000AB5D0174E472 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[AttendanceVariable] ([ID], [Name], [Description], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (7, N'Early', N'', 1, CAST(0x0000AB7A00F3A128 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[AttendanceVariable] ([ID], [Name], [Description], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (8, N'Late', N'', 1, CAST(0x0000AB7A00F3A128 AS DateTime), NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[AttendanceVariable] OFF
GO
SET IDENTITY_INSERT [dbo].[Branch] ON 

GO
INSERT [dbo].[Branch] ([ID], [Name], [AddressLine], [CityID], [StateID], [CountryID], [ZipCode], [PhoneNumber], [GUID], [CreatedDate], [UpdationDate], [UpdatedBy], [UserIP], [IsActive]) VALUES (1, N'21D-201', NULL, NULL, NULL, NULL, NULL, NULL, N'd02caa04afba49688db5d4218800873f', CAST(0x0000AB5D0160067F AS DateTime), NULL, NULL, N'::1', 1)
GO
SET IDENTITY_INSERT [dbo].[Branch] OFF
GO
SET IDENTITY_INSERT [dbo].[BranchDepartment] ON 

GO
INSERT [dbo].[BranchDepartment] ([ID], [BranchID], [DepartmentID], [CreatedDate], [UpdatedDate], [UpdatedBy], [UserIP], [IsActive]) VALUES (1, 1, 1, CAST(0x0000AB5D0168BF21 AS DateTime), NULL, NULL, NULL, 1)
GO
INSERT [dbo].[BranchDepartment] ([ID], [BranchID], [DepartmentID], [CreatedDate], [UpdatedDate], [UpdatedBy], [UserIP], [IsActive]) VALUES (2, 1, 2, CAST(0x0000AB5D016F0518 AS DateTime), NULL, 1, N'::1', 1)
GO
INSERT [dbo].[BranchDepartment] ([ID], [BranchID], [DepartmentID], [CreatedDate], [UpdatedDate], [UpdatedBy], [UserIP], [IsActive]) VALUES (3, 1, 3, CAST(0x0000AB5D016F24E3 AS DateTime), NULL, 1, N'::1', 1)
GO
INSERT [dbo].[BranchDepartment] ([ID], [BranchID], [DepartmentID], [CreatedDate], [UpdatedDate], [UpdatedBy], [UserIP], [IsActive]) VALUES (4, 1, 4, CAST(0x0000AB5D016F38E5 AS DateTime), NULL, 1, N'::1', 1)
GO
INSERT [dbo].[BranchDepartment] ([ID], [BranchID], [DepartmentID], [CreatedDate], [UpdatedDate], [UpdatedBy], [UserIP], [IsActive]) VALUES (5, 1, 5, CAST(0x0000AB5D016F499D AS DateTime), NULL, 1, N'::1', 1)
GO
INSERT [dbo].[BranchDepartment] ([ID], [BranchID], [DepartmentID], [CreatedDate], [UpdatedDate], [UpdatedBy], [UserIP], [IsActive]) VALUES (6, 1, 6, CAST(0x0000AB74010352A2 AS DateTime), NULL, 1, N'::1', 1)
GO
SET IDENTITY_INSERT [dbo].[BranchDepartment] OFF
GO
SET IDENTITY_INSERT [dbo].[BranchShift] ON 

GO
INSERT [dbo].[BranchShift] ([ID], [BranchID], [ShiftID], [CreatedDate], [UpdatedDate], [UpdatedBy], [UserIP], [IsActive]) VALUES (1, 1, 2, CAST(0x0000AB8200B5AE28 AS DateTime), CAST(0x0000AB8200B5AE28 AS DateTime), 1, N'::1', 1)
GO
INSERT [dbo].[BranchShift] ([ID], [BranchID], [ShiftID], [CreatedDate], [UpdatedDate], [UpdatedBy], [UserIP], [IsActive]) VALUES (2, 1, 3, CAST(0x0000AB8200B696F3 AS DateTime), CAST(0x0000AB8200B696F3 AS DateTime), 1, N'::1', 1)
GO
INSERT [dbo].[BranchShift] ([ID], [BranchID], [ShiftID], [CreatedDate], [UpdatedDate], [UpdatedBy], [UserIP], [IsActive]) VALUES (3, 1, 4, CAST(0x0000AB8200B6D931 AS DateTime), CAST(0x0000AB8200B6D931 AS DateTime), 1, N'::1', 1)
GO
INSERT [dbo].[BranchShift] ([ID], [BranchID], [ShiftID], [CreatedDate], [UpdatedDate], [UpdatedBy], [UserIP], [IsActive]) VALUES (4, 1, 5, CAST(0x0000AB8200B72949 AS DateTime), CAST(0x0000AB8200B72949 AS DateTime), 1, N'::1', 1)
GO
INSERT [dbo].[BranchShift] ([ID], [BranchID], [ShiftID], [CreatedDate], [UpdatedDate], [UpdatedBy], [UserIP], [IsActive]) VALUES (5, 1, 6, CAST(0x0000AB8200B7555F AS DateTime), CAST(0x0000AB8200B7555F AS DateTime), 1, N'::1', 1)
GO
INSERT [dbo].[BranchShift] ([ID], [BranchID], [ShiftID], [CreatedDate], [UpdatedDate], [UpdatedBy], [UserIP], [IsActive]) VALUES (6, 1, 7, CAST(0x0000AB8900BB854C AS DateTime), CAST(0x0000AB8900BB854C AS DateTime), 1, N'::1', 1)
GO
SET IDENTITY_INSERT [dbo].[BranchShift] OFF
GO
SET IDENTITY_INSERT [dbo].[Configuration] ON 

GO
INSERT [dbo].[Configuration] ([ID], [Name], [Type], [Value], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, N'SERVICE STATUS', N'BOOLEAN', N'1', 1, CAST(0x0000A5F5005515A4 AS DateTime), CAST(0x0000AB7501408A7F AS DateTime), 1, N'::1')
GO
INSERT [dbo].[Configuration] ([ID], [Name], [Type], [Value], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, N'PreAttendance', N'DATETIME', N'Dec 18 2016 11:00PM', 0, CAST(0x0000A62B0103B09A AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Configuration] ([ID], [Name], [Type], [Value], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (3, N'ALTERNATE ATTENDANCE', N'BOOLEAN', N'1', 1, CAST(0x0000A62D0116FBEC AS DateTime), CAST(0x0000AB7501408A91 AS DateTime), 1, N'::1')
GO
SET IDENTITY_INSERT [dbo].[Configuration] OFF
GO
SET IDENTITY_INSERT [dbo].[ContactType] ON 

GO
INSERT [dbo].[ContactType] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, N'Landline', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[ContactType] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, N'Mobile Number', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[ContactType] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (3, N'Alternate Mobile Number', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[ContactType] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (4, N'Emergency Mobile Number', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[ContactType] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (5, N'Email Address', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[ContactType] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (6, N'Alternate Email Address', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[ContactType] OFF
GO
SET IDENTITY_INSERT [dbo].[Country] ON 

GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, N'Afghanistan', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, N'Albania', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (3, N'Algeria', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (4, N'American Samoa', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (5, N'Andorra', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (6, N'Angola', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (7, N'Anguilla', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (8, N'Antarctica', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (9, N'Antigua and Barbuda', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (10, N'Argentina', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (11, N'Armenia', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (12, N'Aruba', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (13, N'Australia', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (14, N'Austria', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (15, N'Azerbaijan', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (16, N'Bahamas', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (17, N'Bahrain', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (18, N'Bangladesh', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (19, N'Barbados', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (20, N'Belarus', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (21, N'Belgium', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (22, N'Belize', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (23, N'Benin', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (24, N'Bermuda', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (25, N'Bhutan', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (26, N'Bolivia', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (27, N'Bosnia and Herzegovina', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (28, N'Botswana', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (29, N'Bouvet Island', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (30, N'Brazil', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (31, N'British Indian Ocean Territory', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (32, N'Brunei Darussalam', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (33, N'Bulgaria', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (34, N'Burkina Faso', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (35, N'Burundi', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (36, N'Cambodia', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (37, N'Cameroon', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (38, N'Canada', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (39, N'Cape Verde', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (40, N'Cayman Islands', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (41, N'Central African Republic', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (42, N'Chad', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (43, N'Chile', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (44, N'China', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (45, N'Christmas Island', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (46, N'Cocos (Keeling) Islands', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (47, N'Colombia', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (48, N'Comoros', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (49, N'Congo', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (50, N'Congo, the Democratic Republic of the', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (51, N'Cook Islands', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (52, N'Costa Rica', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (53, N'Cote D''Ivoire', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (54, N'Croatia', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (55, N'Cuba', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (56, N'Cyprus', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (57, N'Czech Republic', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (58, N'Denmark', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (59, N'Djibouti', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (60, N'Dominica', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (61, N'Dominican Republic', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (62, N'Ecuador', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (63, N'Egypt', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (64, N'El Salvador', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (65, N'Equatorial Guinea', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (66, N'Eritrea', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (67, N'Estonia', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (68, N'Ethiopia', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (69, N'Falkland Islands (Malvinas)', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (70, N'Faroe Islands', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (71, N'Fiji', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (72, N'Finland', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (73, N'France', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (74, N'French Guiana', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (75, N'French Polynesia', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (76, N'French Southern Territories', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (77, N'Gabon', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (78, N'Gambia', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (79, N'Georgia', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (80, N'Germany', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (81, N'Ghana', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (82, N'Gibraltar', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (83, N'Greece', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (84, N'Greenland', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (85, N'Grenada', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (86, N'Guadeloupe', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (87, N'Guam', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (88, N'Guatemala', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (89, N'Guinea', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (90, N'Guinea-Bissau', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (91, N'Guyana', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (92, N'Haiti', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (93, N'Heard Island and Mcdonald Islands', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (94, N'Holy See (Vatican City State)', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (95, N'Honduras', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (96, N'Hong Kong', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (97, N'Hungary', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (98, N'Iceland', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (99, N'India', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (100, N'Indonesia', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (101, N'Iran, Islamic Republic of', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (102, N'Iraq', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (103, N'Ireland', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (104, N'Israel', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (105, N'Italy', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (106, N'Jamaica', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (107, N'Japan', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (108, N'Jordan', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (109, N'Kazakhstan', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (110, N'Kenya', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (111, N'Kiribati', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (112, N'Korea, Democratic People''s Republic of', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (113, N'Korea, Republic of', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (114, N'Kuwait', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (115, N'Kyrgyzstan', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (116, N'Lao People''s Democratic Republic', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (117, N'Latvia', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (118, N'Lebanon', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (119, N'Lesotho', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (120, N'Liberia', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (121, N'Libyan Arab Jamahiriya', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (122, N'Liechtenstein', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (123, N'Lithuania', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (124, N'Luxembourg', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (125, N'Macao', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (126, N'Macedonia, the Former Yugoslav Republic of', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (127, N'Madagascar', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (128, N'Malawi', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (129, N'Malaysia', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (130, N'Maldives', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (131, N'Mali', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (132, N'Malta', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (133, N'Marshall Islands', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (134, N'Martinique', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (135, N'Mauritania', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (136, N'Mauritius', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (137, N'Mayotte', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (138, N'Mexico', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (139, N'Micronesia, Federated States of', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (140, N'Moldova, Republic of', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (141, N'Monaco', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (142, N'Mongolia', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (143, N'Montserrat', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (144, N'Morocco', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (145, N'Mozambique', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (146, N'Myanmar', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (147, N'Namibia', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (148, N'Nauru', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (149, N'Nepal', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (150, N'Netherlands', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (151, N'Netherlands Antilles', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (152, N'New Caledonia', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (153, N'New Zealand', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (154, N'Nicaragua', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (155, N'Niger', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (156, N'Nigeria', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (157, N'Niue', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (158, N'Norfolk Island', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (159, N'Northern Mariana Islands', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (160, N'Norway', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (161, N'Oman', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (162, N'Pakistan', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (163, N'Palau', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (164, N'Palestinian Territory, Occupied', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (165, N'Panama', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (166, N'Papua New Guinea', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (167, N'Paraguay', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (168, N'Peru', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (169, N'Philippines', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (170, N'Pitcairn', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (171, N'Poland', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (172, N'Portugal', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (173, N'Puerto Rico', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (174, N'Qatar', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (175, N'Reunion', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (176, N'Romania', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (177, N'Russian Federation', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (178, N'Rwanda', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (179, N'Saint Helena', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (180, N'Saint Kitts and Nevis', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (181, N'Saint Lucia', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (182, N'Saint Pierre and Miquelon', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (183, N'Saint Vincent and the Grenadines', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (184, N'Samoa', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (185, N'San Marino', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (186, N'Sao Tome and Principe', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (187, N'Saudi Arabia', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (188, N'Senegal', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (189, N'Serbia and Montenegro', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (190, N'Seychelles', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (191, N'Sierra Leone', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (192, N'Singapore', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (193, N'Slovakia', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (194, N'Slovenia', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (195, N'Solomon Islands', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (196, N'Somalia', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (197, N'South Africa', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (198, N'South Georgia and the South Sandwich Islands', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (199, N'Spain', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (200, N'Sri Lanka', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (201, N'Sudan', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (202, N'Suriname', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (203, N'Svalbard and Jan Mayen', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (204, N'Swaziland', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (205, N'Sweden', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (206, N'Switzerland', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (207, N'Syrian Arab Republic', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (208, N'Taiwan, Province of China', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (209, N'Tajikistan', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (210, N'Tanzania, United Republic of', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (211, N'Thailand', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (212, N'Timor-Leste', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (213, N'Togo', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (214, N'Tokelau', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (215, N'Tonga', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (216, N'Trinidad and Tobago', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (217, N'Tunisia', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (218, N'Turkey', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (219, N'Turkmenistan', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (220, N'Turks and Caicos Islands', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (221, N'Tuvalu', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (222, N'Uganda', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (223, N'Ukraine', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (224, N'United Arab Emirates', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (225, N'United Kingdom', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (226, N'United States', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (227, N'United States Minor Outlying Islands', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (228, N'Uruguay', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (229, N'Uzbekistan', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (230, N'Vanuatu', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (231, N'Venezuela', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (232, N'Viet Nam', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (233, N'Virgin Islands, British', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (234, N'Virgin Islands, U.s.', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (235, N'Wallis and Futuna', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (236, N'Western Sahara', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (237, N'Yemen', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (238, N'Zambia', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (239, N'Zimbabwe', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[Country] OFF
GO
SET IDENTITY_INSERT [dbo].[Department] ON 

GO
INSERT [dbo].[Department] ([ID], [Name], [Description], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, N'Head', NULL, 1, CAST(0x0000A648011CBAA0 AS DateTime), CAST(0x0000AB7500C000C3 AS DateTime), 1, N'::1')
GO
INSERT [dbo].[Department] ([ID], [Name], [Description], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, N'Software', N'Software developers, Website developers.', 1, CAST(0x0000AB5D016F0440 AS DateTime), CAST(0x0000AB7B00D1F299 AS DateTime), 1, N'::1')
GO
INSERT [dbo].[Department] ([ID], [Name], [Description], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (3, N'Graphics', N'Illustrator, designer, editor', 1, CAST(0x0000AB5D016F24D6 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[Department] ([ID], [Name], [Description], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (4, N'Content', N'Creative writers', 1, CAST(0x0000AB5D016F38D4 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[Department] ([ID], [Name], [Description], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (5, N'Sales', N'Sales Agent, Support Agents', 1, CAST(0x0000AB5D016F4991 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[Department] ([ID], [Name], [Description], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (6, N'new department', N'new department', 1, CAST(0x0000AB740103529A AS DateTime), NULL, 1, N'::1')
GO
SET IDENTITY_INSERT [dbo].[Department] OFF
GO
SET IDENTITY_INSERT [dbo].[Gender] ON 

GO
INSERT [dbo].[Gender] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, N'Male', 1, CAST(0x0000AB5D015F6F9F AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Gender] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, N'Female', 1, CAST(0x0000AB5D015F6F9F AS DateTime), NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[Gender] OFF
GO
SET IDENTITY_INSERT [dbo].[Holiday] ON 

GO
INSERT [dbo].[Holiday] ([ID], [Name], [Date], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, N'Kashmir Day', CAST(0x0000AB8400000000 AS DateTime), N'Kashmir Day', 1, CAST(0x0000AB8301099221 AS DateTime), NULL, 1, N'::1')
GO
SET IDENTITY_INSERT [dbo].[Holiday] OFF
GO
SET IDENTITY_INSERT [dbo].[LeaveType] ON 

GO
INSERT [dbo].[LeaveType] ([ID], [Name], [YearlyLeaves], [PriorDays], [MaxDays], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, N'Casual', 10, 5, 5, 1, CAST(0x0000A73500BCF9CA AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[LeaveType] ([ID], [Name], [YearlyLeaves], [PriorDays], [MaxDays], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, N'Sick', 10, 5, 5, 1, CAST(0x0000A73500BCF9CA AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[LeaveType] ([ID], [Name], [YearlyLeaves], [PriorDays], [MaxDays], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (3, N'Planned', 10, 5, 5, 1, CAST(0x0000A73500BCF9CF AS DateTime), NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[LeaveType] OFF
GO
SET IDENTITY_INSERT [dbo].[OG_LeaveType] ON 

GO
INSERT [dbo].[OG_LeaveType] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdatedBy], [UserIP]) VALUES (1, N'Casual', 1, CAST(0x0000A73500BBFD7F AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[OG_LeaveType] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdatedBy], [UserIP]) VALUES (2, N'Sick', 1, CAST(0x0000A73500BBFD7F AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[OG_LeaveType] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdatedBy], [UserIP]) VALUES (3, N'Planned', 1, CAST(0x0000A73500BBFD7F AS DateTime), NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[OG_LeaveType] OFF
GO
SET IDENTITY_INSERT [dbo].[Payroll] ON 

GO
INSERT [dbo].[Payroll] ([ID], [PayrollCycleID], [UserID], [Salary], [Addition], [Deduction], [NetSalary], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, 1, 2, CAST(25000.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(25000.00 AS Decimal(9, 2)), 0, CAST(0x0000AB7400E4722C AS DateTime), CAST(0x0000AB750109BA6B AS DateTime), NULL, NULL)
GO
INSERT [dbo].[Payroll] ([ID], [PayrollCycleID], [UserID], [Salary], [Addition], [Deduction], [NetSalary], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, 1, 3, CAST(10000.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(10000.00 AS Decimal(9, 2)), 0, CAST(0x0000AB7400E4723F AS DateTime), CAST(0x0000AB750109BA71 AS DateTime), NULL, NULL)
GO
INSERT [dbo].[Payroll] ([ID], [PayrollCycleID], [UserID], [Salary], [Addition], [Deduction], [NetSalary], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (3, 1, 4, CAST(35000.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(35000.00 AS Decimal(9, 2)), 0, CAST(0x0000AB7400E47240 AS DateTime), CAST(0x0000AB750109BA82 AS DateTime), NULL, NULL)
GO
INSERT [dbo].[Payroll] ([ID], [PayrollCycleID], [UserID], [Salary], [Addition], [Deduction], [NetSalary], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (4, 1, 5, CAST(40000.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(40000.00 AS Decimal(9, 2)), 0, CAST(0x0000AB7400E4724B AS DateTime), CAST(0x0000AB750109BA8A AS DateTime), NULL, NULL)
GO
INSERT [dbo].[Payroll] ([ID], [PayrollCycleID], [UserID], [Salary], [Addition], [Deduction], [NetSalary], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (5, 1, 6, CAST(32000.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(32000.00 AS Decimal(9, 2)), 0, CAST(0x0000AB7400E4724B AS DateTime), CAST(0x0000AB750109BA8B AS DateTime), NULL, NULL)
GO
INSERT [dbo].[Payroll] ([ID], [PayrollCycleID], [UserID], [Salary], [Addition], [Deduction], [NetSalary], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (6, 1, 7, CAST(10000.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(10000.00 AS Decimal(9, 2)), 0, CAST(0x0000AB7400E4724B AS DateTime), CAST(0x0000AB750109BA97 AS DateTime), NULL, NULL)
GO
INSERT [dbo].[Payroll] ([ID], [PayrollCycleID], [UserID], [Salary], [Addition], [Deduction], [NetSalary], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (7, 1, 2, CAST(25000.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(25000.00 AS Decimal(9, 2)), 1, CAST(0x0000AB7400E4951B AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Payroll] ([ID], [PayrollCycleID], [UserID], [Salary], [Addition], [Deduction], [NetSalary], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (8, 1, 3, CAST(10000.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(10000.00 AS Decimal(9, 2)), 1, CAST(0x0000AB7400E4951C AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Payroll] ([ID], [PayrollCycleID], [UserID], [Salary], [Addition], [Deduction], [NetSalary], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (9, 1, 4, CAST(35000.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(35000.00 AS Decimal(9, 2)), 1, CAST(0x0000AB7400E4951D AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Payroll] ([ID], [PayrollCycleID], [UserID], [Salary], [Addition], [Deduction], [NetSalary], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (10, 1, 5, CAST(40000.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(40000.00 AS Decimal(9, 2)), 1, CAST(0x0000AB7400E4951D AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Payroll] ([ID], [PayrollCycleID], [UserID], [Salary], [Addition], [Deduction], [NetSalary], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (11, 1, 6, CAST(32000.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(32000.00 AS Decimal(9, 2)), 1, CAST(0x0000AB7400E4951E AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Payroll] ([ID], [PayrollCycleID], [UserID], [Salary], [Addition], [Deduction], [NetSalary], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (12, 1, 7, CAST(10000.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(10000.00 AS Decimal(9, 2)), 1, CAST(0x0000AB7400E49525 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Payroll] ([ID], [PayrollCycleID], [UserID], [Salary], [Addition], [Deduction], [NetSalary], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (13, 1, 2, CAST(25000.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(25000.00 AS Decimal(9, 2)), 1, CAST(0x0000AB740124A4BC AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Payroll] ([ID], [PayrollCycleID], [UserID], [Salary], [Addition], [Deduction], [NetSalary], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (14, 1, 3, CAST(10000.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(10000.00 AS Decimal(9, 2)), 1, CAST(0x0000AB740124A4C0 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Payroll] ([ID], [PayrollCycleID], [UserID], [Salary], [Addition], [Deduction], [NetSalary], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (15, 1, 4, CAST(35000.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(35000.00 AS Decimal(9, 2)), 1, CAST(0x0000AB740124A4C5 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Payroll] ([ID], [PayrollCycleID], [UserID], [Salary], [Addition], [Deduction], [NetSalary], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (16, 1, 5, CAST(40000.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(40000.00 AS Decimal(9, 2)), 1, CAST(0x0000AB740124A4CB AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Payroll] ([ID], [PayrollCycleID], [UserID], [Salary], [Addition], [Deduction], [NetSalary], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (17, 1, 6, CAST(32000.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(32000.00 AS Decimal(9, 2)), 1, CAST(0x0000AB740124A4CC AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Payroll] ([ID], [PayrollCycleID], [UserID], [Salary], [Addition], [Deduction], [NetSalary], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (18, 1, 7, CAST(10000.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(10000.00 AS Decimal(9, 2)), 1, CAST(0x0000AB740124A4D2 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Payroll] ([ID], [PayrollCycleID], [UserID], [Salary], [Addition], [Deduction], [NetSalary], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (19, 1, 2, CAST(25000.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(25000.00 AS Decimal(9, 2)), 1, CAST(0x0000AB740124AC7A AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Payroll] ([ID], [PayrollCycleID], [UserID], [Salary], [Addition], [Deduction], [NetSalary], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (20, 1, 3, CAST(10000.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(10000.00 AS Decimal(9, 2)), 1, CAST(0x0000AB740124AC7B AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Payroll] ([ID], [PayrollCycleID], [UserID], [Salary], [Addition], [Deduction], [NetSalary], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (21, 1, 4, CAST(35000.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(35000.00 AS Decimal(9, 2)), 1, CAST(0x0000AB740124AC7B AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Payroll] ([ID], [PayrollCycleID], [UserID], [Salary], [Addition], [Deduction], [NetSalary], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (22, 1, 5, CAST(40000.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(40000.00 AS Decimal(9, 2)), 1, CAST(0x0000AB740124AC7C AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Payroll] ([ID], [PayrollCycleID], [UserID], [Salary], [Addition], [Deduction], [NetSalary], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (23, 1, 6, CAST(32000.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(32000.00 AS Decimal(9, 2)), 1, CAST(0x0000AB740124AC7C AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Payroll] ([ID], [PayrollCycleID], [UserID], [Salary], [Addition], [Deduction], [NetSalary], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (24, 1, 7, CAST(10000.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(10000.00 AS Decimal(9, 2)), 1, CAST(0x0000AB740124AC7D AS DateTime), NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[Payroll] OFF
GO
SET IDENTITY_INSERT [dbo].[PayrollCycle] ON 

GO
INSERT [dbo].[PayrollCycle] ([ID], [Name], [Month], [Year], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, N'Mar 2020 Payroll', 3, 2020, 1, CAST(0x0000AB7300E4C064 AS DateTime), NULL, 1, N'::1')
GO
SET IDENTITY_INSERT [dbo].[PayrollCycle] OFF
GO
SET IDENTITY_INSERT [dbo].[PayrollPolicy] ON 

GO
INSERT [dbo].[PayrollPolicy] ([ID], [PayrollVariableID], [IsUnit], [IsPercentage], [Value], [Description], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, 1, 1, 0, CAST(1000.00 AS Decimal(9, 2)), NULL, CAST(0x0000AB7400000000 AS DateTime), NULL, CAST(0x0000AB7401240706 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[PayrollPolicy] ([ID], [PayrollVariableID], [IsUnit], [IsPercentage], [Value], [Description], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, 2, 1, 0, CAST(1500.00 AS Decimal(9, 2)), NULL, CAST(0x0000AB7500000000 AS DateTime), NULL, CAST(0x0000AB7500B955BE AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[PayrollPolicy] ([ID], [PayrollVariableID], [IsUnit], [IsPercentage], [Value], [Description], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (3, 4, 0, 1, CAST(10.00 AS Decimal(9, 2)), NULL, CAST(0x0000AB7500000000 AS DateTime), NULL, CAST(0x0000AB750141200B AS DateTime), NULL, 1, N'::1')
GO
SET IDENTITY_INSERT [dbo].[PayrollPolicy] OFF
GO
SET IDENTITY_INSERT [dbo].[PayrollVariable] ON 

GO
INSERT [dbo].[PayrollVariable] ([ID], [Name], [IsDeduction], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, N'Quarter Day', 1, 1, CAST(0x0000AB5D0174B453 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[PayrollVariable] ([ID], [Name], [IsDeduction], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, N'Half Day', 1, 1, CAST(0x0000AB5D0174B453 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[PayrollVariable] ([ID], [Name], [IsDeduction], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (3, N'Full Day', 1, 1, CAST(0x0000AB5D0174B453 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[PayrollVariable] ([ID], [Name], [IsDeduction], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (4, N'Absent', 1, 1, CAST(0x0000AB5D0174B453 AS DateTime), NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[PayrollVariable] OFF
GO
SET IDENTITY_INSERT [dbo].[Religion] ON 

GO
INSERT [dbo].[Religion] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, N'Muslim', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Religion] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, N'Christian', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Religion] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (3, N'Hindu', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[Religion] OFF
GO
SET IDENTITY_INSERT [dbo].[SalaryType] ON 

GO
INSERT [dbo].[SalaryType] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, N'Monthly Salary', 1, CAST(0x0000A64801192A8B AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[SalaryType] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, N'Weekly Salary', 0, CAST(0x0000A64801192A8B AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[SalaryType] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (3, N'Daily Salary', 1, CAST(0x0000A64801192A8B AS DateTime), NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[SalaryType] OFF
GO
SET IDENTITY_INSERT [dbo].[Shift] ON 

GO
INSERT [dbo].[Shift] ([ID], [Name], [Description], [StartHour], [EndHour], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [BreakHour]) VALUES (2, N'Morning10', N'Morning 10am to 7pm 1hour lunch break, sat-sun off', N'10:00', N'19:00', 1, CAST(0x0000AB8200B5AE02 AS DateTime), NULL, 1, N'::1', CAST(0.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[Shift] ([ID], [Name], [Description], [StartHour], [EndHour], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [BreakHour]) VALUES (3, N'Morning9', N'9am to 6pm, Sat, Sun off, 1 hour break', N'9:00', N'18:00', 1, CAST(0x0000AB8200B696E6 AS DateTime), NULL, 1, N'::1', CAST(0.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[Shift] ([ID], [Name], [Description], [StartHour], [EndHour], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [BreakHour]) VALUES (4, N'Evening 2', N'2pm to 2am, sunday off, no break', N'14:00', N'2:00', 1, CAST(0x0000AB8200B6D926 AS DateTime), NULL, 1, N'::1', CAST(0.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[Shift] ([ID], [Name], [Description], [StartHour], [EndHour], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [BreakHour]) VALUES (5, N'Evening 5', N'5pm to 2am', N'17:00', N'2:00', 1, CAST(0x0000AB8200B72939 AS DateTime), NULL, 1, N'::1', CAST(0.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[Shift] ([ID], [Name], [Description], [StartHour], [EndHour], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [BreakHour]) VALUES (6, N'Night 9', N'9pm to 6am', N'21:00', N'6:00', 1, CAST(0x0000AB8200B7554B AS DateTime), NULL, 1, N'::1', CAST(0.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[Shift] ([ID], [Name], [Description], [StartHour], [EndHour], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [BreakHour]) VALUES (7, N'Morning''8', N'Morning''8', N'8:00', N'17:00', 1, CAST(0x0000AB8900BB852A AS DateTime), NULL, 1, N'::1', CAST(1.25 AS Decimal(18, 2)))
GO
SET IDENTITY_INSERT [dbo].[Shift] OFF
GO
SET IDENTITY_INSERT [dbo].[ShiftOffDay] ON 

GO
INSERT [dbo].[ShiftOffDay] ([ID], [ShiftID], [OffDayOfWeek], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, 2, 7, CAST(0x0000AB3500000000 AS DateTime), NULL, CAST(0x0000AB8200B5AE10 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[ShiftOffDay] ([ID], [ShiftID], [OffDayOfWeek], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, 2, 6, CAST(0x0000AB3500000000 AS DateTime), NULL, CAST(0x0000AB8200B5AE24 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[ShiftOffDay] ([ID], [ShiftID], [OffDayOfWeek], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (3, 3, 7, CAST(0x0000AB3500000000 AS DateTime), NULL, CAST(0x0000AB8200B696EB AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[ShiftOffDay] ([ID], [ShiftID], [OffDayOfWeek], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (4, 3, 6, CAST(0x0000AB3500000000 AS DateTime), NULL, CAST(0x0000AB8200B696EF AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[ShiftOffDay] ([ID], [ShiftID], [OffDayOfWeek], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (5, 4, 7, CAST(0x0000AB3500000000 AS DateTime), NULL, CAST(0x0000AB8200B6D92C AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[ShiftOffDay] ([ID], [ShiftID], [OffDayOfWeek], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (6, 5, 7, CAST(0x0000AB3500000000 AS DateTime), NULL, CAST(0x0000AB8200B72942 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[ShiftOffDay] ([ID], [ShiftID], [OffDayOfWeek], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (7, 5, 6, CAST(0x0000AB3500000000 AS DateTime), NULL, CAST(0x0000AB8200B72946 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[ShiftOffDay] ([ID], [ShiftID], [OffDayOfWeek], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (8, 6, 7, CAST(0x0000AB3500000000 AS DateTime), NULL, CAST(0x0000AB8200B75556 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[ShiftOffDay] ([ID], [ShiftID], [OffDayOfWeek], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (9, 6, 6, CAST(0x0000AB3500000000 AS DateTime), NULL, CAST(0x0000AB8200B7555B AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[ShiftOffDay] ([ID], [ShiftID], [OffDayOfWeek], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (10, 7, 7, CAST(0x0000AB8900000000 AS DateTime), NULL, CAST(0x0000AB8900BB8544 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[ShiftOffDay] ([ID], [ShiftID], [OffDayOfWeek], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (11, 7, 6, CAST(0x0000AB8900000000 AS DateTime), NULL, CAST(0x0000AB8900BB854B AS DateTime), NULL, 1, N'::1')
GO
SET IDENTITY_INSERT [dbo].[ShiftOffDay] OFF
GO
SET IDENTITY_INSERT [dbo].[User] ON 

GO
INSERT [dbo].[User] ([ID], [FirstName], [MiddleName], [LastName], [UserTypeID], [GenderID], [DateOfBirth], [NICNo], [ReligionID], [Address1], [Address2], [ZipCode], [CountryID], [CityID], [StateID], [AcadmicQualification], [Designation], [Salary], [LoginID], [Password], [ImagePath], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [SalaryTypeID], [FatherName], [AccountNumber]) VALUES (1, N'Syed Razi Wahid', NULL, NULL, 1, 1, CAST(0x0000A648011FA1C3 AS DateTime), NULL, NULL, NULL, NULL, NULL, 162, NULL, NULL, NULL, N'Asstt Acctt', NULL, N'razi', N'J0$h123', NULL, 1, CAST(0x0000A648011FA1C3 AS DateTime), NULL, NULL, NULL, NULL, N'Syed Wahid', N'36669-5')
GO
INSERT [dbo].[User] ([ID], [FirstName], [MiddleName], [LastName], [UserTypeID], [GenderID], [DateOfBirth], [NICNo], [ReligionID], [Address1], [Address2], [ZipCode], [CountryID], [CityID], [StateID], [AcadmicQualification], [Designation], [Salary], [LoginID], [Password], [ImagePath], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [SalaryTypeID], [FatherName], [AccountNumber]) VALUES (2, N'Arquam Belal', NULL, NULL, 2, 1, CAST(0x0000722300000000 AS DateTime), N'45455-5678468-4', 1, NULL, NULL, NULL, 162, NULL, NULL, N'BSCS', N'Website Developer', CAST(25000.00 AS Decimal(9, 2)), N'arquam', N'arquam', N'/Uploads/ProfileImages\2.png', 1, CAST(0x0000AB5D0170280C AS DateTime), CAST(0x0000AB7501091894 AS DateTime), 1, N'::1', 1, N'F/O Arquam Belal', N'12345646')
GO
INSERT [dbo].[User] ([ID], [FirstName], [MiddleName], [LastName], [UserTypeID], [GenderID], [DateOfBirth], [NICNo], [ReligionID], [Address1], [Address2], [ZipCode], [CountryID], [CityID], [StateID], [AcadmicQualification], [Designation], [Salary], [LoginID], [Password], [ImagePath], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [SalaryTypeID], [FatherName], [AccountNumber]) VALUES (3, N'Saeed Hussain Shah', NULL, NULL, 2, 1, CAST(0x0000722300000000 AS DateTime), N'45524-8755458-2', 1, NULL, NULL, NULL, 162, NULL, NULL, N'Certified', N'Website Developer Internee', CAST(10000.00 AS Decimal(9, 2)), N'saeed', N'saeed', NULL, 1, CAST(0x0000AB5D0170ACA8 AS DateTime), NULL, 1, N'::1', 1, N'FO Saeed Hussain Shah', NULL)
GO
INSERT [dbo].[User] ([ID], [FirstName], [MiddleName], [LastName], [UserTypeID], [GenderID], [DateOfBirth], [NICNo], [ReligionID], [Address1], [Address2], [ZipCode], [CountryID], [CityID], [StateID], [AcadmicQualification], [Designation], [Salary], [LoginID], [Password], [ImagePath], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [SalaryTypeID], [FatherName], [AccountNumber]) VALUES (4, N'Abdul Azeez', NULL, NULL, 2, 1, CAST(0x0000722300000000 AS DateTime), NULL, 1, NULL, NULL, NULL, 162, NULL, NULL, N'BSCS', N'Website Developer', CAST(35000.00 AS Decimal(9, 2)), N'azeez', N'azeez', NULL, 1, CAST(0x0000AB5D0170FA91 AS DateTime), NULL, 1, N'::1', 1, N'FO Abdul Azeez', NULL)
GO
INSERT [dbo].[User] ([ID], [FirstName], [MiddleName], [LastName], [UserTypeID], [GenderID], [DateOfBirth], [NICNo], [ReligionID], [Address1], [Address2], [ZipCode], [CountryID], [CityID], [StateID], [AcadmicQualification], [Designation], [Salary], [LoginID], [Password], [ImagePath], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [SalaryTypeID], [FatherName], [AccountNumber]) VALUES (5, N'Aqsa Shafiq', NULL, NULL, 2, 2, CAST(0x0000722300000000 AS DateTime), NULL, 1, NULL, NULL, NULL, 162, NULL, NULL, N'Masters', N'Graphics Designer', CAST(40000.00 AS Decimal(9, 2)), N'aqsa', N'aqsa', NULL, 1, CAST(0x0000AB5D017147C5 AS DateTime), NULL, 1, N'::1', 1, N'FO Aqsa Shafiq', NULL)
GO
INSERT [dbo].[User] ([ID], [FirstName], [MiddleName], [LastName], [UserTypeID], [GenderID], [DateOfBirth], [NICNo], [ReligionID], [Address1], [Address2], [ZipCode], [CountryID], [CityID], [StateID], [AcadmicQualification], [Designation], [Salary], [LoginID], [Password], [ImagePath], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [SalaryTypeID], [FatherName], [AccountNumber]) VALUES (6, N'Javeria Asif', NULL, NULL, 2, 2, CAST(0x0000722300000000 AS DateTime), NULL, 1, NULL, NULL, NULL, 162, NULL, NULL, N'Masters', N'Creative Writer', CAST(32000.00 AS Decimal(9, 2)), N'javeria', N'javeria', NULL, 1, CAST(0x0000AB5D0171903E AS DateTime), NULL, 1, N'::1', 1, N'FO Javeria Asif', NULL)
GO
INSERT [dbo].[User] ([ID], [FirstName], [MiddleName], [LastName], [UserTypeID], [GenderID], [DateOfBirth], [NICNo], [ReligionID], [Address1], [Address2], [ZipCode], [CountryID], [CityID], [StateID], [AcadmicQualification], [Designation], [Salary], [LoginID], [Password], [ImagePath], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [SalaryTypeID], [FatherName], [AccountNumber]) VALUES (7, N'Saboor', NULL, NULL, 2, 1, CAST(0x0000722C00000000 AS DateTime), N'54487-8547825-6', 1, NULL, NULL, NULL, 162, NULL, NULL, N'Masters', N'Website Developer Internee', CAST(10000.00 AS Decimal(9, 2)), N'saboor', N'Saboor123', NULL, 1, CAST(0x0000AB7300E1DC50 AS DateTime), CAST(0x0000AB7401198406 AS DateTime), 1, N'::1', 1, N'FO Saboor', N'123456789-1')
GO
INSERT [dbo].[User] ([ID], [FirstName], [MiddleName], [LastName], [UserTypeID], [GenderID], [DateOfBirth], [NICNo], [ReligionID], [Address1], [Address2], [ZipCode], [CountryID], [CityID], [StateID], [AcadmicQualification], [Designation], [Salary], [LoginID], [Password], [ImagePath], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [SalaryTypeID], [FatherName], [AccountNumber]) VALUES (8, N'Arqum', NULL, NULL, 2, 1, CAST(0x00008B5F00000000 AS DateTime), N'123456789', 1, N'21D', NULL, NULL, 162, NULL, NULL, N'qualification', N'IT', CAST(50.00 AS Decimal(9, 2)), N'arqum', N'arqum', NULL, 0, CAST(0x0000AB7400DDD5E3 AS DateTime), CAST(0x0000AB7400DED6E0 AS DateTime), 1, N'::1', 1, N'Shakil', N'11234567896')
GO
INSERT [dbo].[User] ([ID], [FirstName], [MiddleName], [LastName], [UserTypeID], [GenderID], [DateOfBirth], [NICNo], [ReligionID], [Address1], [Address2], [ZipCode], [CountryID], [CityID], [StateID], [AcadmicQualification], [Designation], [Salary], [LoginID], [Password], [ImagePath], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [SalaryTypeID], [FatherName], [AccountNumber]) VALUES (9, N'New User', NULL, NULL, 2, 1, CAST(0x0000722300000000 AS DateTime), N'1234569874563', 2, N'21D', NULL, NULL, 7, NULL, NULL, N'Inter', N'Saled Manager', CAST(500000.00 AS Decimal(9, 2)), N'abc', N'abc', NULL, 0, CAST(0x0000AB740100F284 AS DateTime), CAST(0x0000AB740116C280 AS DateTime), 1, N'::1', 3, N'New User''s Father', N'11234567896')
GO
INSERT [dbo].[User] ([ID], [FirstName], [MiddleName], [LastName], [UserTypeID], [GenderID], [DateOfBirth], [NICNo], [ReligionID], [Address1], [Address2], [ZipCode], [CountryID], [CityID], [StateID], [AcadmicQualification], [Designation], [Salary], [LoginID], [Password], [ImagePath], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [SalaryTypeID], [FatherName], [AccountNumber]) VALUES (10, N'Hamza Malick', NULL, NULL, 2, 1, CAST(0x0000722300000000 AS DateTime), N'1234567894561', 1, N'Phase-10', NULL, NULL, 162, NULL, NULL, N'Bachelors', N'Web Developer', CAST(25000.00 AS Decimal(9, 2)), N'HamzaMalick', N'123', NULL, 0, CAST(0x0000AB7E0110A768 AS DateTime), CAST(0x0000AB8101096374 AS DateTime), 1, N'::1', 1, N'Malick Bilal', N'1234567894561')
GO
INSERT [dbo].[User] ([ID], [FirstName], [MiddleName], [LastName], [UserTypeID], [GenderID], [DateOfBirth], [NICNo], [ReligionID], [Address1], [Address2], [ZipCode], [CountryID], [CityID], [StateID], [AcadmicQualification], [Designation], [Salary], [LoginID], [Password], [ImagePath], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [SalaryTypeID], [FatherName], [AccountNumber]) VALUES (11, N'Umair', NULL, NULL, NULL, 1, CAST(0x0000722300000000 AS DateTime), N'123456789', 1, NULL, NULL, NULL, 162, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, CAST(0x0000AB7E01153D07 AS DateTime), CAST(0x0000AB8101095C6C AS DateTime), 1, N'::1', NULL, N'Malick', NULL)
GO
INSERT [dbo].[User] ([ID], [FirstName], [MiddleName], [LastName], [UserTypeID], [GenderID], [DateOfBirth], [NICNo], [ReligionID], [Address1], [Address2], [ZipCode], [CountryID], [CityID], [StateID], [AcadmicQualification], [Designation], [Salary], [LoginID], [Password], [ImagePath], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [SalaryTypeID], [FatherName], [AccountNumber]) VALUES (12, N'Khalid Mehmood', NULL, NULL, 2, 1, CAST(0x0000722300000000 AS DateTime), N'1234567894561', 1, NULL, NULL, NULL, 162, NULL, NULL, NULL, NULL, NULL, N'Khalid', N'123', NULL, 0, CAST(0x0000AB8000AC351E AS DateTime), CAST(0x0000AB81010957BC AS DateTime), 1, N'::1', NULL, N'Khalid Memom', NULL)
GO
INSERT [dbo].[User] ([ID], [FirstName], [MiddleName], [LastName], [UserTypeID], [GenderID], [DateOfBirth], [NICNo], [ReligionID], [Address1], [Address2], [ZipCode], [CountryID], [CityID], [StateID], [AcadmicQualification], [Designation], [Salary], [LoginID], [Password], [ImagePath], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [SalaryTypeID], [FatherName], [AccountNumber]) VALUES (13, N'Maqbool Khan', NULL, NULL, 2, 1, CAST(0x0000722300000000 AS DateTime), N'1234567894561', 1, NULL, NULL, NULL, 162, NULL, NULL, NULL, NULL, NULL, N'maqbool', N'123', NULL, 0, CAST(0x0000AB8000BEE0B7 AS DateTime), CAST(0x0000AB81010951E0 AS DateTime), 1, N'::1', NULL, N'Maqbool Ijaz', NULL)
GO
INSERT [dbo].[User] ([ID], [FirstName], [MiddleName], [LastName], [UserTypeID], [GenderID], [DateOfBirth], [NICNo], [ReligionID], [Address1], [Address2], [ZipCode], [CountryID], [CityID], [StateID], [AcadmicQualification], [Designation], [Salary], [LoginID], [Password], [ImagePath], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [SalaryTypeID], [FatherName], [AccountNumber]) VALUES (14, N'Ahad Khan', NULL, NULL, 2, 1, CAST(0x0000722300000000 AS DateTime), N'1234567894561', 1, NULL, NULL, NULL, 162, NULL, NULL, NULL, NULL, NULL, N'Ahad', N'123', NULL, 0, CAST(0x0000AB8000C39666 AS DateTime), CAST(0x0000AB8101094C04 AS DateTime), 1, N'::1', NULL, N'Ahmed Ahad', NULL)
GO
INSERT [dbo].[User] ([ID], [FirstName], [MiddleName], [LastName], [UserTypeID], [GenderID], [DateOfBirth], [NICNo], [ReligionID], [Address1], [Address2], [ZipCode], [CountryID], [CityID], [StateID], [AcadmicQualification], [Designation], [Salary], [LoginID], [Password], [ImagePath], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [SalaryTypeID], [FatherName], [AccountNumber]) VALUES (15, N'Musa Khan', NULL, NULL, 2, 1, CAST(0x0000722300000000 AS DateTime), N'1234567894561', 1, NULL, NULL, NULL, 162, NULL, NULL, NULL, NULL, NULL, N'MUSA', N'123', NULL, 0, CAST(0x0000AB8000C738B8 AS DateTime), CAST(0x0000AB8101093B9C AS DateTime), 1, N'::1', NULL, N'Musa Khan', NULL)
GO
INSERT [dbo].[User] ([ID], [FirstName], [MiddleName], [LastName], [UserTypeID], [GenderID], [DateOfBirth], [NICNo], [ReligionID], [Address1], [Address2], [ZipCode], [CountryID], [CityID], [StateID], [AcadmicQualification], [Designation], [Salary], [LoginID], [Password], [ImagePath], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [SalaryTypeID], [FatherName], [AccountNumber]) VALUES (16, N'Samaria', NULL, NULL, 2, 2, CAST(0x0000722300000000 AS DateTime), N'1234567894561', 1, NULL, NULL, NULL, 162, NULL, NULL, NULL, NULL, NULL, N'samarria', N'123', NULL, 1, CAST(0x0000AB80013F8F96 AS DateTime), NULL, 1, N'::1', NULL, N'Samaria', NULL)
GO
INSERT [dbo].[User] ([ID], [FirstName], [MiddleName], [LastName], [UserTypeID], [GenderID], [DateOfBirth], [NICNo], [ReligionID], [Address1], [Address2], [ZipCode], [CountryID], [CityID], [StateID], [AcadmicQualification], [Designation], [Salary], [LoginID], [Password], [ImagePath], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [SalaryTypeID], [FatherName], [AccountNumber]) VALUES (17, N'Junaid Khan', NULL, NULL, 2, 1, CAST(0x0000722300000000 AS DateTime), N'1234567894561', 1, NULL, NULL, NULL, 162, NULL, NULL, NULL, NULL, NULL, N'JunaidKhan12', N'123', NULL, 1, CAST(0x0000AB8100B95B4D AS DateTime), NULL, 1, N'::1', NULL, N'Akbar Khan', NULL)
GO
INSERT [dbo].[User] ([ID], [FirstName], [MiddleName], [LastName], [UserTypeID], [GenderID], [DateOfBirth], [NICNo], [ReligionID], [Address1], [Address2], [ZipCode], [CountryID], [CityID], [StateID], [AcadmicQualification], [Designation], [Salary], [LoginID], [Password], [ImagePath], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [SalaryTypeID], [FatherName], [AccountNumber]) VALUES (18, N'Shahid Afridi', NULL, NULL, 2, 1, CAST(0x0000722300000000 AS DateTime), N'1234567894561', 1, NULL, NULL, NULL, 162, NULL, NULL, NULL, NULL, NULL, N'Afridi987', N'987', NULL, 0, CAST(0x0000AB8100DAE60B AS DateTime), CAST(0x0000AB81010944FC AS DateTime), 1, N'::1', NULL, N'Shahid Afridi', NULL)
GO
INSERT [dbo].[User] ([ID], [FirstName], [MiddleName], [LastName], [UserTypeID], [GenderID], [DateOfBirth], [NICNo], [ReligionID], [Address1], [Address2], [ZipCode], [CountryID], [CityID], [StateID], [AcadmicQualification], [Designation], [Salary], [LoginID], [Password], [ImagePath], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [SalaryTypeID], [FatherName], [AccountNumber]) VALUES (19, N'Humaima Khan', NULL, NULL, 2, 2, CAST(0x0000722300000000 AS DateTime), N'1234567894561', 1, NULL, NULL, NULL, 162, NULL, NULL, NULL, NULL, NULL, N'Humaima12', N'123', NULL, 1, CAST(0x0000AB8100F01A3C AS DateTime), NULL, 1, N'::1', NULL, N'Humaima Khan', NULL)
GO
INSERT [dbo].[User] ([ID], [FirstName], [MiddleName], [LastName], [UserTypeID], [GenderID], [DateOfBirth], [NICNo], [ReligionID], [Address1], [Address2], [ZipCode], [CountryID], [CityID], [StateID], [AcadmicQualification], [Designation], [Salary], [LoginID], [Password], [ImagePath], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [SalaryTypeID], [FatherName], [AccountNumber]) VALUES (20, N'Faisal ', NULL, NULL, 2, 1, CAST(0x0000722300000000 AS DateTime), N'1234567894561', 1, NULL, NULL, NULL, 162, NULL, NULL, NULL, NULL, NULL, N'FAISAL1234', N'1234', NULL, 1, CAST(0x0000AB810108F6DA AS DateTime), NULL, 1, N'::1', NULL, NULL, NULL)
GO
INSERT [dbo].[User] ([ID], [FirstName], [MiddleName], [LastName], [UserTypeID], [GenderID], [DateOfBirth], [NICNo], [ReligionID], [Address1], [Address2], [ZipCode], [CountryID], [CityID], [StateID], [AcadmicQualification], [Designation], [Salary], [LoginID], [Password], [ImagePath], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [SalaryTypeID], [FatherName], [AccountNumber]) VALUES (21, N'Ahmer Khan', NULL, NULL, 2, 1, CAST(0x0000722300000000 AS DateTime), N'1234567894561', 1, NULL, NULL, NULL, 162, NULL, NULL, NULL, NULL, NULL, N'ahmer123', N'123', NULL, 1, CAST(0x0000AB81011A5B21 AS DateTime), NULL, 1, N'::1', NULL, N'Ahmer Khan', NULL)
GO
INSERT [dbo].[User] ([ID], [FirstName], [MiddleName], [LastName], [UserTypeID], [GenderID], [DateOfBirth], [NICNo], [ReligionID], [Address1], [Address2], [ZipCode], [CountryID], [CityID], [StateID], [AcadmicQualification], [Designation], [Salary], [LoginID], [Password], [ImagePath], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [SalaryTypeID], [FatherName], [AccountNumber]) VALUES (22, N'Saud Bashir', NULL, NULL, 2, 1, CAST(0x0000722300000000 AS DateTime), N'1234567894561', 1, NULL, NULL, NULL, 162, NULL, NULL, NULL, NULL, NULL, N'saud123', N'saud', NULL, 1, CAST(0x0000AB81011AA716 AS DateTime), NULL, 1, N'::1', NULL, N'Saud Bashir', NULL)
GO
INSERT [dbo].[User] ([ID], [FirstName], [MiddleName], [LastName], [UserTypeID], [GenderID], [DateOfBirth], [NICNo], [ReligionID], [Address1], [Address2], [ZipCode], [CountryID], [CityID], [StateID], [AcadmicQualification], [Designation], [Salary], [LoginID], [Password], [ImagePath], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [SalaryTypeID], [FatherName], [AccountNumber]) VALUES (23, N'Sheraz Hashmi', NULL, NULL, 2, 1, CAST(0x0000722300000000 AS DateTime), N'1234567894561', 1, NULL, NULL, NULL, 162, NULL, NULL, NULL, NULL, NULL, N'Sheraz987', N'123', NULL, 1, CAST(0x0000AB81012DF7E1 AS DateTime), NULL, 1, N'::1', NULL, N'Sheraz Hashmi', NULL)
GO
INSERT [dbo].[User] ([ID], [FirstName], [MiddleName], [LastName], [UserTypeID], [GenderID], [DateOfBirth], [NICNo], [ReligionID], [Address1], [Address2], [ZipCode], [CountryID], [CityID], [StateID], [AcadmicQualification], [Designation], [Salary], [LoginID], [Password], [ImagePath], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [SalaryTypeID], [FatherName], [AccountNumber]) VALUES (24, N'Bruce Wayne', NULL, NULL, 2, 1, CAST(0x0000722300000000 AS DateTime), N'1234567894561', 1, NULL, NULL, NULL, 162, NULL, NULL, NULL, NULL, NULL, N'bruce12', N'123', NULL, 1, CAST(0x0000AB81013EE6B2 AS DateTime), NULL, 1, N'::1', NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[User] OFF
GO
SET IDENTITY_INSERT [dbo].[UserContact] ON 

GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, 1, 2, N'1234567890', 1, CAST(0x0000AB5D0167AD02 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, 1, 5, N'josh@rigelonic.com', 1, CAST(0x0000AB5D0167AD02 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (3, 2, 5, N'abc@arquam.com', 1, CAST(0x0000AB5D017028C4 AS DateTime), CAST(0x0000AB75010918A4 AS DateTime), 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (4, 2, 2, N'123123123321', 1, CAST(0x0000AB5D017028C4 AS DateTime), CAST(0x0000AB75010918A9 AS DateTime), 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (5, 2, 3, NULL, 1, CAST(0x0000AB5D017028C4 AS DateTime), CAST(0x0000AB75010918AF AS DateTime), 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (6, 3, 5, N'saeed@abc.com', 1, CAST(0x0000AB5D0170ACB8 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (7, 3, 2, N'4654654654654', 1, CAST(0x0000AB5D0170ACB9 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (8, 3, 3, NULL, 1, CAST(0x0000AB5D0170ACB9 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (9, 4, 5, N'azeez@email.com', 1, CAST(0x0000AB5D0170FA9E AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (10, 4, 2, N'1234654321321', 1, CAST(0x0000AB5D0170FA9E AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (11, 4, 3, NULL, 1, CAST(0x0000AB5D0170FA9E AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (12, 5, 5, N'aqsa@email.com', 1, CAST(0x0000AB5D017147D1 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (13, 5, 2, N'213213213213', 1, CAST(0x0000AB5D017147D2 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (14, 5, 3, NULL, 1, CAST(0x0000AB5D017147D2 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (15, 6, 5, N'javeria@email.com', 1, CAST(0x0000AB5D0171904A AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (16, 6, 2, N'23424234324', 1, CAST(0x0000AB5D0171904A AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (17, 6, 3, NULL, 1, CAST(0x0000AB5D0171904A AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (18, 7, 5, N'saboor@gmail.com', 1, CAST(0x0000AB7300E1DD5B AS DateTime), CAST(0x0000AB740119841B AS DateTime), 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (19, 7, 2, N'498465465465', 1, CAST(0x0000AB7300E1DD5B AS DateTime), CAST(0x0000AB7401198421 AS DateTime), 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (20, 7, 3, NULL, 1, CAST(0x0000AB7300E1DD5C AS DateTime), CAST(0x0000AB7401198427 AS DateTime), 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (21, 8, 5, N'arqum.123@gmail.com', 1, CAST(0x0000AB7400DDD61B AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (22, 8, 2, N'03412572079', 1, CAST(0x0000AB7400DDD61B AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (23, 8, 3, N'03412572079', 1, CAST(0x0000AB7400DDD61B AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (24, 9, 5, N'abc@abc.com', 1, CAST(0x0000AB740100F381 AS DateTime), CAST(0x0000AB740116AD8C AS DateTime), 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (25, 9, 2, N'03412572079', 1, CAST(0x0000AB740100F381 AS DateTime), CAST(0x0000AB740116AD94 AS DateTime), 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (26, 9, 3, N'03412572079', 1, CAST(0x0000AB740100F381 AS DateTime), CAST(0x0000AB740116AD9A AS DateTime), 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (27, 10, 5, N'hamza.12@gmail.com', 1, CAST(0x0000AB7E0110A8A8 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (28, 10, 2, N'03412572079', 1, CAST(0x0000AB7E0110A8A8 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (29, 10, 3, N'03412572079', 1, CAST(0x0000AB7E0110A8A8 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (30, 11, 5, NULL, 1, CAST(0x0000AB7E01153D2B AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (31, 11, 2, NULL, 1, CAST(0x0000AB7E01153D2B AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (32, 11, 3, NULL, 1, CAST(0x0000AB7E01153D2B AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (33, 12, 5, NULL, 1, CAST(0x0000AB8000AC353A AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (34, 12, 2, NULL, 1, CAST(0x0000AB8000AC353A AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (35, 12, 3, NULL, 1, CAST(0x0000AB8000AC353A AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (36, 13, 5, NULL, 1, CAST(0x0000AB8000BEE0C8 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (37, 13, 2, NULL, 1, CAST(0x0000AB8000BEE0C9 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (38, 13, 3, NULL, 1, CAST(0x0000AB8000BEE0C9 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (39, 14, 5, NULL, 1, CAST(0x0000AB8000C39676 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (40, 14, 2, NULL, 1, CAST(0x0000AB8000C39676 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (41, 14, 3, NULL, 1, CAST(0x0000AB8000C39676 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (42, 15, 5, NULL, 1, CAST(0x0000AB8000C738CE AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (43, 15, 2, NULL, 1, CAST(0x0000AB8000C738CF AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (44, 15, 3, NULL, 1, CAST(0x0000AB8000C738CF AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (45, 16, 5, NULL, 1, CAST(0x0000AB80013F8FB1 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (46, 16, 2, NULL, 1, CAST(0x0000AB80013F8FB1 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (47, 16, 3, NULL, 1, CAST(0x0000AB80013F8FB1 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (48, 17, 5, NULL, 1, CAST(0x0000AB8100B95B67 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (49, 17, 2, NULL, 1, CAST(0x0000AB8100B95B67 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (50, 17, 3, NULL, 1, CAST(0x0000AB8100B95B67 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (51, 18, 5, NULL, 1, CAST(0x0000AB8100DAE625 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (52, 18, 2, NULL, 1, CAST(0x0000AB8100DAE625 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (53, 18, 3, NULL, 1, CAST(0x0000AB8100DAE625 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (54, 19, 5, NULL, 1, CAST(0x0000AB8100F01A57 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (55, 19, 2, NULL, 1, CAST(0x0000AB8100F01A57 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (56, 19, 3, NULL, 1, CAST(0x0000AB8100F01A57 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (57, 20, 5, NULL, 1, CAST(0x0000AB810108F6F6 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (58, 20, 2, NULL, 1, CAST(0x0000AB810108F6F6 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (59, 20, 3, NULL, 1, CAST(0x0000AB810108F6F6 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (60, 21, 5, NULL, 1, CAST(0x0000AB81011A5B35 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (61, 21, 2, NULL, 1, CAST(0x0000AB81011A5B36 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (62, 21, 3, NULL, 1, CAST(0x0000AB81011A5B36 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (63, 22, 5, NULL, 1, CAST(0x0000AB81011AA721 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (64, 22, 2, NULL, 1, CAST(0x0000AB81011AA721 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (65, 22, 3, NULL, 1, CAST(0x0000AB81011AA721 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (66, 23, 5, NULL, 1, CAST(0x0000AB81012DF7F0 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (67, 23, 2, NULL, 1, CAST(0x0000AB81012DF7F0 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (68, 23, 3, NULL, 1, CAST(0x0000AB81012DF7F0 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (69, 24, 5, NULL, 1, CAST(0x0000AB81013EE6C7 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (70, 24, 2, NULL, 1, CAST(0x0000AB81013EE6C8 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (71, 24, 3, NULL, 1, CAST(0x0000AB81013EE6C8 AS DateTime), NULL, 1, N'::1')
GO
SET IDENTITY_INSERT [dbo].[UserContact] OFF
GO
SET IDENTITY_INSERT [dbo].[UserDepartment] ON 

GO
INSERT [dbo].[UserDepartment] ([ID], [UserID], [DepartmentID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, 1, 1, CAST(0x0000AB3500000000 AS DateTime), NULL, CAST(0x0000AB5D01681DC5 AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[UserDepartment] ([ID], [UserID], [DepartmentID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, 2, 2, CAST(0x0000AB5D00000000 AS DateTime), NULL, CAST(0x0000AB5D017028E7 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserDepartment] ([ID], [UserID], [DepartmentID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (3, 3, 2, CAST(0x0000AB5D00000000 AS DateTime), NULL, CAST(0x0000AB5D0170ACD2 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserDepartment] ([ID], [UserID], [DepartmentID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (4, 4, 2, CAST(0x0000AB5D00000000 AS DateTime), NULL, CAST(0x0000AB5D0170FAB5 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserDepartment] ([ID], [UserID], [DepartmentID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (5, 5, 3, CAST(0x0000AB5D00000000 AS DateTime), NULL, CAST(0x0000AB5D017147E8 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserDepartment] ([ID], [UserID], [DepartmentID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (6, 6, 4, CAST(0x0000AB5D00000000 AS DateTime), NULL, CAST(0x0000AB5D0171907A AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserDepartment] ([ID], [UserID], [DepartmentID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (7, 7, 2, CAST(0x0000AB7300000000 AS DateTime), NULL, CAST(0x0000AB7300E1DD7A AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserDepartment] ([ID], [UserID], [DepartmentID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (8, 8, 2, CAST(0x0000AB7400000000 AS DateTime), NULL, CAST(0x0000AB7400DDD630 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserDepartment] ([ID], [UserID], [DepartmentID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (9, 9, 5, CAST(0x0000AB7400000000 AS DateTime), NULL, CAST(0x0000AB740100F396 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserDepartment] ([ID], [UserID], [DepartmentID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (10, 10, 2, CAST(0x0000AB7E00000000 AS DateTime), NULL, CAST(0x0000AB7E0110A8D7 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserDepartment] ([ID], [UserID], [DepartmentID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (11, 11, 2, CAST(0x0000AB7E00000000 AS DateTime), NULL, CAST(0x0000AB7E01153D41 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserDepartment] ([ID], [UserID], [DepartmentID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (12, 12, 2, CAST(0x0000AB8000000000 AS DateTime), NULL, CAST(0x0000AB8000AC3561 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserDepartment] ([ID], [UserID], [DepartmentID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (13, 13, 2, CAST(0x0000AB8000000000 AS DateTime), NULL, CAST(0x0000AB8000BEE0D6 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserDepartment] ([ID], [UserID], [DepartmentID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (14, 14, 2, CAST(0x0000AB8000000000 AS DateTime), NULL, CAST(0x0000AB8000C39682 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserDepartment] ([ID], [UserID], [DepartmentID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (15, 15, 2, CAST(0x0000AB8000000000 AS DateTime), NULL, CAST(0x0000AB8000C738FB AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserDepartment] ([ID], [UserID], [DepartmentID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (16, 16, 3, CAST(0x0000AB8000000000 AS DateTime), NULL, CAST(0x0000AB80013F8FC4 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserDepartment] ([ID], [UserID], [DepartmentID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (17, 17, 5, CAST(0x0000AB8100000000 AS DateTime), NULL, CAST(0x0000AB8100B95B83 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserDepartment] ([ID], [UserID], [DepartmentID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (18, 18, 2, CAST(0x0000AB8100000000 AS DateTime), NULL, CAST(0x0000AB8100DAE637 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserDepartment] ([ID], [UserID], [DepartmentID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (19, 19, 4, CAST(0x0000AB8100000000 AS DateTime), NULL, CAST(0x0000AB8100F01A69 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserDepartment] ([ID], [UserID], [DepartmentID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (20, 20, 2, CAST(0x0000AB8100000000 AS DateTime), NULL, CAST(0x0000AB810108F706 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserDepartment] ([ID], [UserID], [DepartmentID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (21, 21, 5, CAST(0x0000AB8100000000 AS DateTime), NULL, CAST(0x0000AB81011A5B44 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserDepartment] ([ID], [UserID], [DepartmentID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (22, 22, 3, CAST(0x0000AB8100000000 AS DateTime), NULL, CAST(0x0000AB81011AA72B AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserDepartment] ([ID], [UserID], [DepartmentID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (23, 23, 4, CAST(0x0000AB8100000000 AS DateTime), NULL, CAST(0x0000AB81012DF800 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserDepartment] ([ID], [UserID], [DepartmentID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (24, 24, 5, CAST(0x0000AB8100000000 AS DateTime), NULL, CAST(0x0000AB81013EE6D5 AS DateTime), NULL, 1, N'::1')
GO
SET IDENTITY_INSERT [dbo].[UserDepartment] OFF
GO
SET IDENTITY_INSERT [dbo].[UserShift] ON 

GO
INSERT [dbo].[UserShift] ([ID], [UserID], [ShiftID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, 2, 2, CAST(0x0000AB3500000000 AS DateTime), CAST(0x0000AB43018B80D4 AS DateTime), CAST(0x0000AB8200B8E76E AS DateTime), CAST(0x0000AB8200B94BDC AS DateTime), 1, N'::1')
GO
INSERT [dbo].[UserShift] ([ID], [UserID], [ShiftID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, 3, 2, CAST(0x0000AB3500000000 AS DateTime), CAST(0x0000AB43018B80D4 AS DateTime), CAST(0x0000AB8200B8E776 AS DateTime), CAST(0x0000AB8200B94BEA AS DateTime), 1, N'::1')
GO
INSERT [dbo].[UserShift] ([ID], [UserID], [ShiftID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (3, 4, 2, CAST(0x0000AB3500000000 AS DateTime), CAST(0x0000AB43018B80D4 AS DateTime), CAST(0x0000AB8200B8E77A AS DateTime), CAST(0x0000AB8200B94BF3 AS DateTime), 1, N'::1')
GO
INSERT [dbo].[UserShift] ([ID], [UserID], [ShiftID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (4, 7, 2, CAST(0x0000AB3500000000 AS DateTime), CAST(0x0000AB43018B80D4 AS DateTime), CAST(0x0000AB8200B8E77E AS DateTime), CAST(0x0000AB8200B94BFC AS DateTime), 1, N'::1')
GO
INSERT [dbo].[UserShift] ([ID], [UserID], [ShiftID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (5, 2, 3, CAST(0x0000AB4400000000 AS DateTime), CAST(0x0000AB51018B80D4 AS DateTime), CAST(0x0000AB8200B94BE4 AS DateTime), CAST(0x0000AB8200BA3647 AS DateTime), 1, N'::1')
GO
INSERT [dbo].[UserShift] ([ID], [UserID], [ShiftID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (6, 3, 3, CAST(0x0000AB4400000000 AS DateTime), CAST(0x0000AB51018B80D4 AS DateTime), CAST(0x0000AB8200B94BEE AS DateTime), CAST(0x0000AB8200BA3652 AS DateTime), 1, N'::1')
GO
INSERT [dbo].[UserShift] ([ID], [UserID], [ShiftID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (7, 4, 3, CAST(0x0000AB4400000000 AS DateTime), CAST(0x0000AB51018B80D4 AS DateTime), CAST(0x0000AB8200B94BF8 AS DateTime), CAST(0x0000AB8200BA365B AS DateTime), 1, N'::1')
GO
INSERT [dbo].[UserShift] ([ID], [UserID], [ShiftID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (8, 7, 3, CAST(0x0000AB4400000000 AS DateTime), CAST(0x0000AB51018B80D4 AS DateTime), CAST(0x0000AB8200B94C01 AS DateTime), CAST(0x0000AB8200BA3664 AS DateTime), 1, N'::1')
GO
INSERT [dbo].[UserShift] ([ID], [UserID], [ShiftID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (10, 2, 2, CAST(0x0000AB5200000000 AS DateTime), CAST(0x0000AB88018B80D4 AS DateTime), CAST(0x0000AB8200BA364C AS DateTime), CAST(0x0000AB8900BC0BA2 AS DateTime), 1, N'::1')
GO
INSERT [dbo].[UserShift] ([ID], [UserID], [ShiftID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (11, 3, 2, CAST(0x0000AB5200000000 AS DateTime), NULL, CAST(0x0000AB8200BA3656 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserShift] ([ID], [UserID], [ShiftID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (12, 4, 2, CAST(0x0000AB5200000000 AS DateTime), CAST(0x0000AB5F018B80D4 AS DateTime), CAST(0x0000AB8200BA365F AS DateTime), CAST(0x0000AB8200BAF4E4 AS DateTime), 1, N'::1')
GO
INSERT [dbo].[UserShift] ([ID], [UserID], [ShiftID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (13, 7, 2, CAST(0x0000AB5200000000 AS DateTime), NULL, CAST(0x0000AB8200BA3668 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserShift] ([ID], [UserID], [ShiftID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (14, 4, 3, CAST(0x0000AB6000000000 AS DateTime), NULL, CAST(0x0000AB8200BAF4EA AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserShift] ([ID], [UserID], [ShiftID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (15, 5, 2, CAST(0x0000AB3500000000 AS DateTime), NULL, CAST(0x0000AB8200BB6125 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserShift] ([ID], [UserID], [ShiftID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (16, 6, 2, CAST(0x0000AB3500000000 AS DateTime), NULL, CAST(0x0000AB8200BB612A AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserShift] ([ID], [UserID], [ShiftID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (17, 1, 4, CAST(0x0000AB3500000000 AS DateTime), NULL, CAST(0x0000AB8200BB9709 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserShift] ([ID], [UserID], [ShiftID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (18, 16, 2, CAST(0x0000AB7900000000 AS DateTime), NULL, CAST(0x0000AB8200BC3840 AS DateTime), NULL, 1, N'::1')
GO
INSERT [dbo].[UserShift] ([ID], [UserID], [ShiftID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (19, 2, 7, CAST(0x0000AB8900000000 AS DateTime), NULL, CAST(0x0000AB8900BC0BB4 AS DateTime), NULL, 1, N'::1')
GO
SET IDENTITY_INSERT [dbo].[UserShift] OFF
GO
SET IDENTITY_INSERT [dbo].[UserType] ON 

GO
INSERT [dbo].[UserType] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, N'Admin', 1, CAST(0x0000A5E301538F4D AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[UserType] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, N'Employee', 1, CAST(0x0000A5E301538F4D AS DateTime), NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[UserType] OFF
GO
ALTER TABLE [dbo].[Attendance] ADD  CONSTRAINT [DF_Attendance_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Attendance] ADD  CONSTRAINT [DF_Attendance_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[AttendanceDetail] ADD  CONSTRAINT [DF_AttendanceDetail_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[AttendanceDetail] ADD  CONSTRAINT [DF_AttendanceDetail_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[AttendancePolicy] ADD  CONSTRAINT [DF_AttendancePolicy_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[AttendanceStatus] ADD  CONSTRAINT [DF_AttendanceStatus_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[AttendanceStatus] ADD  CONSTRAINT [DF_AttendanceStatus_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[AttendanceType] ADD  CONSTRAINT [DF_AttendanceType_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[AttendanceType] ADD  CONSTRAINT [DF_AttendanceType_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[AttendanceVariable] ADD  CONSTRAINT [DF_AttendanceVariable_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[AttendanceVariable] ADD  CONSTRAINT [DF_AttendanceVariable_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[Branch] ADD  CONSTRAINT [DF__Branch__CreatedD__75A278F5]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Branch] ADD  CONSTRAINT [DF__Branch__IsActive__76969D2E]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[BranchDepartment] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[BranchDepartment] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[BranchShift] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[BranchShift] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[City] ADD  CONSTRAINT [DF_City_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[City] ADD  CONSTRAINT [DF_City_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[Configuration] ADD  CONSTRAINT [DF_Configuration_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Configuration] ADD  CONSTRAINT [DF_Configuration_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[ContactType] ADD  CONSTRAINT [DF_ContactType_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[ContactType] ADD  CONSTRAINT [DF_ContactType_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[Country] ADD  CONSTRAINT [DF_Country_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Country] ADD  CONSTRAINT [DF_Country_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[Department] ADD  CONSTRAINT [DF_Department_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Department] ADD  CONSTRAINT [DF_Department_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[Device] ADD  CONSTRAINT [DF_Device_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Device] ADD  CONSTRAINT [DF_Device_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[DeviceModal] ADD  CONSTRAINT [DF_DeviceModal_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[DeviceModal] ADD  CONSTRAINT [DF_DeviceModal_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Gender] ADD  CONSTRAINT [DF_Gender_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Gender] ADD  CONSTRAINT [DF_Gender_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[Holiday] ADD  CONSTRAINT [DF_Holiday_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Holiday] ADD  CONSTRAINT [DF_Holiday_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[Leave] ADD  CONSTRAINT [DF_Leave_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Leave] ADD  CONSTRAINT [DF_Leave_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[LeaveType] ADD  CONSTRAINT [DF_LeaveType_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[LeaveType] ADD  CONSTRAINT [DF_LeaveType_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[OG_LeaveType] ADD  CONSTRAINT [DF_OG_LeaveType_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[OG_LeaveType] ADD  CONSTRAINT [DF_OG_LeaveType_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[Payroll] ADD  CONSTRAINT [DF_Payroll_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Payroll] ADD  CONSTRAINT [DF_Payroll_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[PayrollCycle] ADD  CONSTRAINT [DF_PayrollCycle_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[PayrollCycle] ADD  CONSTRAINT [DF_PayrollCycle_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[PayrollDetail] ADD  CONSTRAINT [DF_PayrollDetail_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[PayrollDetail] ADD  CONSTRAINT [DF_PayrollDetail_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[PayrollPolicy] ADD  CONSTRAINT [DF_PayrollPolicy_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[PayrollVariable] ADD  CONSTRAINT [DF_PayrollVariable_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[PayrollVariable] ADD  CONSTRAINT [DF_PayrollVariable_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[Religion] ADD  CONSTRAINT [DF_Religion_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Religion] ADD  CONSTRAINT [DF_Religion_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[SalaryType] ADD  CONSTRAINT [DF_SalaryType_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[SalaryType] ADD  CONSTRAINT [DF_SalaryType_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[Shift] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Shift] ADD  CONSTRAINT [DF_Shift_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[ShiftOffDay] ADD  CONSTRAINT [DF_ShiftOffDay_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[State] ADD  CONSTRAINT [DF_State_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[State] ADD  CONSTRAINT [DF_State_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[UserContact] ADD  CONSTRAINT [DF_UserContact_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[UserContact] ADD  CONSTRAINT [DF_UserContact_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[UserDepartment] ADD  CONSTRAINT [DF_UserDepartment_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[UserShift] ADD  CONSTRAINT [DF_UserShift_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[UserType] ADD  CONSTRAINT [DF_UserType_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[UserType] ADD  CONSTRAINT [DF_UserType_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[Attendance]  WITH CHECK ADD  CONSTRAINT [fk_Id_AttendanceUser] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([ID])
GO
ALTER TABLE [dbo].[Attendance] CHECK CONSTRAINT [fk_Id_AttendanceUser]
GO
ALTER TABLE [dbo].[AttendanceDetail]  WITH CHECK ADD  CONSTRAINT [fk_Id_AttendanceDetailAttendance] FOREIGN KEY([AttendanceID])
REFERENCES [dbo].[Attendance] ([ID])
GO
ALTER TABLE [dbo].[AttendanceDetail] CHECK CONSTRAINT [fk_Id_AttendanceDetailAttendance]
GO
ALTER TABLE [dbo].[AttendanceDetail]  WITH CHECK ADD  CONSTRAINT [fk_Id_AttendanceDetailAttendanceType] FOREIGN KEY([AttendanceTypeID])
REFERENCES [dbo].[AttendanceType] ([ID])
GO
ALTER TABLE [dbo].[AttendanceDetail] CHECK CONSTRAINT [fk_Id_AttendanceDetailAttendanceType]
GO
ALTER TABLE [dbo].[AttendancePolicy]  WITH CHECK ADD  CONSTRAINT [fk_Id_AttendancePolicyAttendanceVariable] FOREIGN KEY([AttendanceVariableID])
REFERENCES [dbo].[AttendanceVariable] ([ID])
GO
ALTER TABLE [dbo].[AttendancePolicy] CHECK CONSTRAINT [fk_Id_AttendancePolicyAttendanceVariable]
GO
ALTER TABLE [dbo].[AttendancePolicy]  WITH CHECK ADD  CONSTRAINT [fk_Id_AttendancePolicyShift] FOREIGN KEY([ShiftID])
REFERENCES [dbo].[Shift] ([ID])
GO
ALTER TABLE [dbo].[AttendancePolicy] CHECK CONSTRAINT [fk_Id_AttendancePolicyShift]
GO
ALTER TABLE [dbo].[AttendanceStatus]  WITH CHECK ADD  CONSTRAINT [fk_Id_AttendanceStatusAttendance] FOREIGN KEY([AttendanceID])
REFERENCES [dbo].[Attendance] ([ID])
GO
ALTER TABLE [dbo].[AttendanceStatus] CHECK CONSTRAINT [fk_Id_AttendanceStatusAttendance]
GO
ALTER TABLE [dbo].[BranchDepartment]  WITH CHECK ADD  CONSTRAINT [FK__BranchDep__Branc__04E4BC85] FOREIGN KEY([BranchID])
REFERENCES [dbo].[Branch] ([ID])
GO
ALTER TABLE [dbo].[BranchDepartment] CHECK CONSTRAINT [FK__BranchDep__Branc__04E4BC85]
GO
ALTER TABLE [dbo].[BranchDepartment]  WITH CHECK ADD FOREIGN KEY([DepartmentID])
REFERENCES [dbo].[Department] ([ID])
GO
ALTER TABLE [dbo].[BranchShift]  WITH CHECK ADD  CONSTRAINT [FK__BranchShi__Branc__02FC7413] FOREIGN KEY([BranchID])
REFERENCES [dbo].[Branch] ([ID])
GO
ALTER TABLE [dbo].[BranchShift] CHECK CONSTRAINT [FK__BranchShi__Branc__02FC7413]
GO
ALTER TABLE [dbo].[BranchShift]  WITH CHECK ADD FOREIGN KEY([ShiftID])
REFERENCES [dbo].[Shift] ([ID])
GO
ALTER TABLE [dbo].[City]  WITH CHECK ADD  CONSTRAINT [fk_Id_CityState] FOREIGN KEY([StateID])
REFERENCES [dbo].[State] ([ID])
GO
ALTER TABLE [dbo].[City] CHECK CONSTRAINT [fk_Id_CityState]
GO
ALTER TABLE [dbo].[Device]  WITH CHECK ADD  CONSTRAINT [FK_Device_Branch] FOREIGN KEY([BranchID])
REFERENCES [dbo].[Branch] ([ID])
GO
ALTER TABLE [dbo].[Device] CHECK CONSTRAINT [FK_Device_Branch]
GO
ALTER TABLE [dbo].[Device]  WITH CHECK ADD  CONSTRAINT [FK_Device_DeviceModal] FOREIGN KEY([DeviceModalID])
REFERENCES [dbo].[DeviceModal] ([ID])
GO
ALTER TABLE [dbo].[Device] CHECK CONSTRAINT [FK_Device_DeviceModal]
GO
ALTER TABLE [dbo].[Leave]  WITH CHECK ADD  CONSTRAINT [FK_Leave_OG_LeaveType] FOREIGN KEY([LeaveTypeID])
REFERENCES [dbo].[OG_LeaveType] ([ID])
GO
ALTER TABLE [dbo].[Leave] CHECK CONSTRAINT [FK_Leave_OG_LeaveType]
GO
ALTER TABLE [dbo].[Leave]  WITH CHECK ADD  CONSTRAINT [FK_Leave_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([ID])
GO
ALTER TABLE [dbo].[Leave] CHECK CONSTRAINT [FK_Leave_User]
GO
ALTER TABLE [dbo].[Payroll]  WITH CHECK ADD  CONSTRAINT [fk_Id_PayrollPayrollCycle] FOREIGN KEY([PayrollCycleID])
REFERENCES [dbo].[PayrollCycle] ([ID])
GO
ALTER TABLE [dbo].[Payroll] CHECK CONSTRAINT [fk_Id_PayrollPayrollCycle]
GO
ALTER TABLE [dbo].[PayrollDetail]  WITH CHECK ADD  CONSTRAINT [fk_Id_PayrollDetailPayroll] FOREIGN KEY([PayrollID])
REFERENCES [dbo].[Payroll] ([ID])
GO
ALTER TABLE [dbo].[PayrollDetail] CHECK CONSTRAINT [fk_Id_PayrollDetailPayroll]
GO
ALTER TABLE [dbo].[PayrollDetail]  WITH CHECK ADD  CONSTRAINT [fk_Id_PayrollDetailPayrollPolicy] FOREIGN KEY([PayrollPolicyID])
REFERENCES [dbo].[PayrollPolicy] ([ID])
GO
ALTER TABLE [dbo].[PayrollDetail] CHECK CONSTRAINT [fk_Id_PayrollDetailPayrollPolicy]
GO
ALTER TABLE [dbo].[PayrollPolicy]  WITH CHECK ADD  CONSTRAINT [fk_Id_PayrollPolicyPayrollVariable] FOREIGN KEY([PayrollVariableID])
REFERENCES [dbo].[PayrollVariable] ([ID])
GO
ALTER TABLE [dbo].[PayrollPolicy] CHECK CONSTRAINT [fk_Id_PayrollPolicyPayrollVariable]
GO
ALTER TABLE [dbo].[ShiftOffDay]  WITH CHECK ADD  CONSTRAINT [fk_Id_ShiftOffDayShift] FOREIGN KEY([ShiftID])
REFERENCES [dbo].[Shift] ([ID])
GO
ALTER TABLE [dbo].[ShiftOffDay] CHECK CONSTRAINT [fk_Id_ShiftOffDayShift]
GO
ALTER TABLE [dbo].[State]  WITH CHECK ADD  CONSTRAINT [fk_Id_StateCountry] FOREIGN KEY([CountryID])
REFERENCES [dbo].[Country] ([ID])
GO
ALTER TABLE [dbo].[State] CHECK CONSTRAINT [fk_Id_StateCountry]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [fk_Id_UserCity] FOREIGN KEY([CityID])
REFERENCES [dbo].[City] ([ID])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [fk_Id_UserCity]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [fk_Id_UserCountry] FOREIGN KEY([CountryID])
REFERENCES [dbo].[Country] ([ID])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [fk_Id_UserCountry]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [fk_Id_UserGender] FOREIGN KEY([GenderID])
REFERENCES [dbo].[Gender] ([ID])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [fk_Id_UserGender]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [fk_Id_UserReligion] FOREIGN KEY([ReligionID])
REFERENCES [dbo].[Religion] ([ID])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [fk_Id_UserReligion]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [fk_Id_UserSalaryType] FOREIGN KEY([SalaryTypeID])
REFERENCES [dbo].[SalaryType] ([ID])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [fk_Id_UserSalaryType]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [fk_Id_UserState] FOREIGN KEY([StateID])
REFERENCES [dbo].[State] ([ID])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [fk_Id_UserState]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [fk_Id_UserUserType] FOREIGN KEY([UserTypeID])
REFERENCES [dbo].[UserType] ([ID])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [fk_Id_UserUserType]
GO
ALTER TABLE [dbo].[UserContact]  WITH CHECK ADD  CONSTRAINT [fk_Id_UserContactContactType] FOREIGN KEY([ContactTypeID])
REFERENCES [dbo].[ContactType] ([ID])
GO
ALTER TABLE [dbo].[UserContact] CHECK CONSTRAINT [fk_Id_UserContactContactType]
GO
ALTER TABLE [dbo].[UserContact]  WITH CHECK ADD  CONSTRAINT [fk_Id_UserContactUser] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([ID])
GO
ALTER TABLE [dbo].[UserContact] CHECK CONSTRAINT [fk_Id_UserContactUser]
GO
ALTER TABLE [dbo].[UserDepartment]  WITH CHECK ADD  CONSTRAINT [fk_Id_UserDepartmentDepartment] FOREIGN KEY([DepartmentID])
REFERENCES [dbo].[Department] ([ID])
GO
ALTER TABLE [dbo].[UserDepartment] CHECK CONSTRAINT [fk_Id_UserDepartmentDepartment]
GO
ALTER TABLE [dbo].[UserDepartment]  WITH CHECK ADD  CONSTRAINT [fk_Id_UserDepartmentUser] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([ID])
GO
ALTER TABLE [dbo].[UserDepartment] CHECK CONSTRAINT [fk_Id_UserDepartmentUser]
GO
ALTER TABLE [dbo].[UserShift]  WITH CHECK ADD  CONSTRAINT [fk_Id_UserShiftUser] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([ID])
GO
ALTER TABLE [dbo].[UserShift] CHECK CONSTRAINT [fk_Id_UserShiftUser]
GO
