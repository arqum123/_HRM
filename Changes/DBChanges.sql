--16Apr16
Alter table attendancestatus add IsLate bit


--26Apr16
USE [HRManagementSystem]
GO

/****** Object:  Table [dbo].[Configuration]    Script Date: 04/27/2016 05:05:19 ******/
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Configuration] ADD  CONSTRAINT [DF_Configuration_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO

ALTER TABLE [dbo].[Configuration] ADD  CONSTRAINT [DF_Configuration_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO



INsert into Configuration( Name,Type,Value) VALUES ('SERVICE STATUS','BOOLEAN',0)


--29Apr16

USE [HRManagementSystem]
GO

/****** Object:  Table [dbo].[Device]    Script Date: 04/29/2016 20:48:09 ******/
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
	[IsActive] [bit] NULL,
	[CreationDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateBy] [int] NULL,
	[UserIP] [varchar](20) NULL,
 CONSTRAINT [PK_Device] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Device] ADD  CONSTRAINT [DF_Device_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO

ALTER TABLE [dbo].[Device] ADD  CONSTRAINT [DF_Device_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO



insert into Device(MachineID,DeviceID,ConnectionTypeID,IPAddress,PortNumber,Password,ComPort,Baudrate,Status,StatusDescription,IsActive,CreationDate)
select 2,2,2,'127.0.0.1',5005,'1111','',0,'','',1,GETDATE()





/*================================================
	Change Date : 02 May, 2016
=================================================*/

Create table dbo.Branch
(
ID int identity(1, 1) not null Primary Key
,Name nvarchar(500)
,AddressLine nvarchar(1000)
,CityID int
,StateID int
,CountryID int
,ZipCode nvarchar(20)
,PhoneNumber nvarchar(50)
,CreatedDate datetime default getdate()
,UpdationDate datetime
,UpdatedBy int 
,UserIP nvarchar(50)
,IsActive bit default 1
)

GO

create table dbo.BranchShift
(
ID int identity(1, 1) not null Primary Key
,BranchID int
,ShiftID int
,CreatedDate datetime Default getdate()
,UpdatedDate datetime
,UpdatedBy int
,UserIP nvarchar(50)
,IsActive bit default 1
)

GO

create table dbo.BranchDepartment
(
ID int identity(1, 1) not null Primary Key
,BranchID int
,DepartmentID int
,CreatedDate datetime Default getdate()
,UpdatedDate datetime
,UpdatedBy int
,UserIP nvarchar(50)
,IsActive bit default 1
)

GO


ALTER TABLE dbo.BranchShift
ADD FOREIGN KEY (BranchID)
REFERENCES Branch(ID)

GO

ALTER TABLE dbo.BranchShift
ADD FOREIGN KEY (ShiftID)
REFERENCES Shift(ID)

GO

ALTER TABLE dbo.BranchDepartment
ADD FOREIGN KEY (BranchID)
REFERENCES Branch(ID)

GO

ALTER TABLE dbo.BranchDepartment
ADD FOREIGN KEY (DepartmentID)
REFERENCES Department(ID)

GO



/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Branch SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Device
	DROP CONSTRAINT DF_Device_IsActive
GO
ALTER TABLE dbo.Device
	DROP CONSTRAINT DF_Device_CreationDate
GO
CREATE TABLE dbo.Tmp_Device
	(
	ID int NOT NULL IDENTITY (1, 1),
	MachineID int NULL,
	DeviceID int NULL,
	ConnectionTypeID int NULL,
	IPAddress varchar(50) NULL,
	PortNumber int NULL,
	Password varchar(50) NULL,
	ComPort int NULL,
	Baudrate bigint NULL,
	Status varchar(50) NULL,
	StatusDescription varchar(1000) NULL,
	BranchID int NULL,
	IsActive bit NULL,
	CreationDate datetime NULL,
	UpdateDate datetime NULL,
	UpdateBy int NULL,
	UserIP varchar(20) NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_Device SET (LOCK_ESCALATION = TABLE)
GO
ALTER TABLE dbo.Tmp_Device ADD CONSTRAINT
	DF_Device_IsActive DEFAULT ((1)) FOR IsActive
GO
ALTER TABLE dbo.Tmp_Device ADD CONSTRAINT
	DF_Device_CreationDate DEFAULT (getdate()) FOR CreationDate
GO
SET IDENTITY_INSERT dbo.Tmp_Device ON



ALTER TABLE dbo.Branch
ADD GUID VARCHAR(500)




USE [HRManagementSystem]
GO

/****** Object:  StoredProcedure [dbo].[InsertPreAttendance]    Script Date: 05/05/2016 20:25:48 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InsertPreAttendance]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[InsertPreAttendance]
GO

USE [HRManagementSystem]
GO

/****** Object:  StoredProcedure [dbo].[InsertPreAttendance]    Script Date: 05/05/2016 20:25:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*
EXEC InsertPreAttendance
EXEC InsertPreAttendance '04/29/2016 14:00:00'

*/

CREATE PROC [dbo].[InsertPreAttendance]
@AttendanceDate AS DATETIME = null
AS
BEGIN
	IF @AttendanceDate IS NULL SET @AttendanceDate=GETDATE()
	-- START Attendance Table Data Generation
	DECLARE @tblAttendance AS TABLE(ID INT IDENTITY,UserID INT, ShiftID INT, StartHour varchar(10), EndHour varchar(10),Diff INT)
	INSERT INTO @tblAttendance (UserID, ShiftID,StartHour,EndHour,Diff)
	SELECT	u.ID,us.ShiftID,s.StartHour,s.EndHour,
			DATEDIFF(hour,CAST(CONVERT(varchar,@AttendanceDate,101) + ' ' +s.StartHour AS DATETIME),@AttendanceDate)
	FROM	[User] u 
			INNER JOIN UserShift us ON u.ID=us.UserID
			INNER JOIN Shift s ON us.ShiftID=s.ID
	WHERE	@AttendanceDate BETWEEN us.EffectiveDate AND ISNULL(us.RetiredDate,@AttendanceDate)
	AND		DATEDIFF(hour,CAST(CONVERT(varchar,@AttendanceDate,101) + ' ' +s.StartHour AS DATETIME),@AttendanceDate)=-1
	AND		u.IsActive=1 AND s.IsActive=1
	AND		NOT EXISTS (SELECT TOP 1 1 FROM Attendance a WHERE a.UserID=u.ID AND a.IsActive=1 AND a.Date=CAST(@AttendanceDate AS DATE))
	-- END Attendance Table Data Generation

	-- START Shift Off day Data Generation
	DECLARE @tblShiftOffDay TABLE(ID INT IDENTITY, ShiftID INT, OffDayOfWeek INT)
	INSERT INTO @tblShiftOffDay (ShiftID,OffDayOfWeek )
	SELECT	sod.ShiftID,sod.OffDayOfWeek
	FROM	Shift s INNER JOIN ShiftOffDay sod ON s.ID=sod.ShiftID
	WHERE	s.IsActive=1
	AND		@AttendanceDate BETWEEN sod.EffectiveDate AND ISNULL(sod.RetiredDate,@AttendanceDate)
	-- END Shif Off day Data Generation

	-- START Generated Data Selection FOR Testing
	SELECT * from @tblAttendance
	SELECT * from @tblShiftOffDay
	-- STOP Generated Data Selection FOR Testing
	RETURN
	DECLARE @index INT=0, @count INT=0, @UserID INT=0,@ShiftID INT=0, @AttendanceID INT=0
	SELECT @count=COUNT(1) FROM @tblAttendance
	
	WHILE(@index<@count)
	BEGIN
		SELECT @UserID=UserID,@ShiftID=ShiftID FROM @tblAttendance WHERE ID=@index
		
		IF @UserID !=0
		BEGIN
			INSERT INTO Attendance (UserID,Date,IsActive,CreationDate,UserIP)
			VALUES	(@UserID,CAST(@AttendanceDate AS Date),1,GETDATE(),'JOB')
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
			END
		END
		SET @index=@index+1
	END
END


GO


/*10 May 2016*/

INSERT INTO Configuration(Name,Type,Value,IsActive)
SELECT 'PreAttendance','DATETIME','2016-05-10 12:00:00.000',0


GO

ALTER PROC [dbo].[InsertPreAttendance]
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
			END
		END
		SET @index=@index+1
	END
	UPDATE Configuration SET Value=CAST(@AttendanceDate AS VARCHAR(100)) WHERE [Name]='PreAttendance'
END




/*Changes 06 Jun 2016*/

/****** Object:  Table [dbo].[DeviceModal]    Script Date: 06/06/2016 19:04:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[DeviceModal](
	[ID] [int] NOT NULL  IDENTITY (1, 1),
	[DeviceModal] [varchar](100) NULL,
	[CreatedDate] [datetime] NULL,
	[UpdationDate] [datetime] NULL,
	[UpdatedBy] [int] NULL,
	[UserIP] [nvarchar](50) NULL,
	[IsActive] [bit] NULL,
 CONSTRAINT [PK_DeviceModal] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[DeviceModal] ADD  CONSTRAINT [DF_DeviceModal_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO

ALTER TABLE [dbo].[DeviceModal] ADD  CONSTRAINT [DF_DeviceModal_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO



/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
ALTER TABLE Device
ADD DeviceModalID INT

ALTER TABLE dbo.Device ADD CONSTRAINT
	FK_Device_DeviceModal FOREIGN KEY
	(
	DeviceModalID
	) REFERENCES dbo.DeviceModal
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO


/*8 Jun 2016*/

ALTER TABLE AttendanceStatus 
ADD IsEarly BIT


/*10 Jun 2016*/
USE [HRManagementSystem]
GO

/****** Object:  Table [dbo].[PayrollVariable]    Script Date: 04/09/2016 18:30:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[PayrollVariable](
[ID] int IDENTITY(1,1) NOT NULL,
[Name] varchar(100) NULL,
[IsDeduction] bit NULL,
[IsActive] bit NULL,
[CreationDate] datetime NULL,
[UpdateDate] datetime NULL,
[UpdateBy] int NULL,
[UserIP] varchar(20) NULL,
 CONSTRAINT [PK_PayrollVariable] PRIMARY KEY CLUSTERED 
(
 [ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[PayrollVariable] ADD  CONSTRAINT [DF_PayrollVariable_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO

ALTER TABLE [dbo].[PayrollVariable] ADD  CONSTRAINT [DF_PayrollVariable_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO



USE [HRManagementSystem]
GO

/****** Object:  Table [dbo].[PayrollPolicy]    Script Date: 04/09/2016 18:30:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[PayrollPolicy](
[ID] int IDENTITY(1,1) NOT NULL,
[PayrollVariableID] int NULL,
[IsUnit] bit NULL,
[IsPercentage] bit NULL,
[Value] decimal(9,2) NULL,
[Description] varchar(1000) NULL,
[EffectiveDate] datetime NULL,
[RetiredDate] datetime NULL,
[CreationDate] datetime NULL,
[UpdateDate] datetime NULL,
[UpdateBy] int NULL,
[UserIP] varchar(20) NULL,
 CONSTRAINT [PK_PayrollPolicy] PRIMARY KEY CLUSTERED 
(
 [ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[PayrollPolicy] ADD  CONSTRAINT [DF_PayrollPolicy_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO

ALTER TABLE [dbo].[PayrollPolicy]  WITH CHECK ADD  CONSTRAINT [fk_Id_PayrollPolicyPayrollVariable] FOREIGN KEY([PayrollVariableID]) REFERENCES [dbo].[PayrollVariable] ([ID])
GO


USE [HRManagementSystem]
GO

/****** Object:  Table [dbo].[PayrollCycle]    Script Date: 04/09/2016 18:30:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[PayrollCycle](
[ID] int IDENTITY(1,1) NOT NULL,
[Name] varchar(100) NULL,
[Month] int NULL,
[Year] int NULL,
[IsActive] bit NULL,
[CreationDate] datetime NULL,
[UpdateDate] datetime NULL,
[UpdateBy] int NULL,
[UserIP] varchar(20) NULL,
 CONSTRAINT [PK_PayrollCycle] PRIMARY KEY CLUSTERED 
(
 [ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[PayrollCycle] ADD  CONSTRAINT [DF_PayrollCycle_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO

ALTER TABLE [dbo].[PayrollCycle] ADD  CONSTRAINT [DF_PayrollCycle_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO



USE [HRManagementSystem]
GO
/****** Object:  Table [dbo].[Payroll]    Script Date: 04/09/2016 18:30:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Payroll](
[ID] int IDENTITY(1,1) NOT NULL,
[PayrollCycleID] int NULL,
[UserID] int NULL,
[Salary] decimal(9,2) NULL,
[Addition] decimal(9,2) NULL,
[Deduction] decimal(9,2) NULL,
[NetSalary] decimal(9,2) NULL,
[IsActive] bit NULL,
[CreationDate] datetime NULL,
[UpdateDate] datetime NULL,
[UpdateBy] int NULL,
[UserIP] varchar(20) NULL,
 CONSTRAINT [PK_Payroll] PRIMARY KEY CLUSTERED 
(
 [ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[Payroll] ADD  CONSTRAINT [DF_Payroll_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Payroll] ADD  CONSTRAINT [DF_Payroll_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[Payroll]  WITH CHECK ADD  CONSTRAINT [fk_Id_PayrollPayrollCycle] FOREIGN KEY([PayrollCycleID]) REFERENCES [dbo].[PayrollCycle] ([ID])
GO

USE [HRManagementSystem]
GO
/****** Object:  Table [dbo].[PayrollDetail]    Script Date: 04/09/2016 18:30:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PayrollDetail](
[ID] int IDENTITY(1,1) NOT NULL,
[PayrollID] int NULL,
[PayrollPolicyID] int NULL,
[Amount] decimal(9,2) NULL,
[IsActive] bit NULL,
[CreationDate] datetime NULL,
[UpdateDate] datetime NULL,
[UpdateBy] int NULL,
[UserIP] varchar(20) NULL,
 CONSTRAINT [PK_PayrollDetail] PRIMARY KEY CLUSTERED 
(
 [ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[PayrollDetail] ADD  CONSTRAINT [DF_PayrollDetail_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[PayrollDetail] ADD  CONSTRAINT [DF_PayrollDetail_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
ALTER TABLE [dbo].[PayrollDetail]  WITH CHECK ADD  CONSTRAINT [fk_Id_PayrollDetailPayroll] FOREIGN KEY([PayrollID]) REFERENCES [dbo].[Payroll] ([ID])
GO
ALTER TABLE [dbo].[PayrollDetail]  WITH CHECK ADD  CONSTRAINT [fk_Id_PayrollDetailPayrollPolicy] FOREIGN KEY([PayrollPolicyID]) REFERENCES [dbo].[PayrollPolicy] ([ID])
GO

/* 17 Jun 2016 */

alter PROC [dbo].[InsertPreAttendance]  
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


/*18 JUne*/

Alter table AttendanceStatus
Add LateMinutes int,
EarlyMinutes int,
WorkingMinutes int,
TotalMinutes int,
OverTimeMinutes int
GO

/* 19 June 2016 */

insert into configuration(name,type,value)
select 'ALTERNATE ATTENDANCE','BOOLEAN',0

go

alter PROC [dbo].[InsertPreAttendance]  
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
	SET s.IsQuarterDay = 0,s.IsHalfDay = 0,s.IsFullDay = 1,s.IsEarly = 1,s.EarlyMinutes = 0,s.WorkingMinutes = 0,s.TotalMinutes = 0
	FROM AttendanceStatus s
	INNER JOIN Attendance a on a.ID = s.AttendanceID
	INNER JOIN AttendanceDetail ad on ad.AttendanceID = a.ID
	WHERE a.UserID = @UserID
	AND a.IsActive = 1 AND ad.IsActive = 1 AND s.IsActive = 1
	AND ad.AttendanceTypeID = 1
	AND ad.StartDate IS NOT NULL AND ad.EndDate IS NULL
	
	UPDATE ad
	SET ad.EndDate = ad.StartDate
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


/*20 Jun*/

alter PROC [dbo].[InsertPreAttendance]  
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

/*15 July 2016*/
USE [HRManagementSystem]
GO

/****** Object:  Table [dbo].[SalaryType]    Script Date: 07/10/2016 20:11:31 ******/
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[SalaryType] ADD  CONSTRAINT [DF_SalaryType_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO

ALTER TABLE [dbo].[SalaryType] ADD  CONSTRAINT [DF_SalaryType_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO


ALTER TABLE [user]
ADD SalaryTypeID INT

ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [fk_Id_UserSalaryType] FOREIGN KEY([SalaryTypeID])
REFERENCES [dbo].[SalaryType] ([ID])
GO

ALTER TABLE [dbo].[User] CHECK CONSTRAINT [fk_Id_UserSalaryType]
GO


insert into salarytype(name)
select 'Monthly Salary' union all
select 'Weekly Salary' union all
select 'Daily Salary'
go

alter table [user]
add FatherName varchar(200)
go
alter table [user]
add AccountNumber varchar(50)
go


insert into PayrollVariable(Name,IsDeduction,IsActive)
select 'Quarter Day',1,0 union all
select 'Half Day',1,0 union all
select 'Full Day',1,0 union all
select 'Absent',1,1 union all
select 'Income Tax',1,0 union all
select 'Compensatory Leave',0,0 union all
select 'Bonus',0,0 union all
select 'Over Time',0,1
go


--GeneratePayroll 1,1
create proc dbo.GeneratePayroll
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
select ID, FirstName, SalaryTypeID,Salary from [User] where IsActive = 1 and ID = ISNULL(@SingleUserID,ID)

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

/*19 July 2016*/
--GeneratePayroll 1,1,3
ALTER proc dbo.GeneratePayroll
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

--3-Sep-2016--

USE [HRManagementSystem]
GO

/****** Object:  Table [dbo].[Leave]    Script Date: 09/03/2016 14:43:58 ******/
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Leave]  WITH CHECK ADD  CONSTRAINT [FK_Leave_OG_LeaveType] FOREIGN KEY([LeaveTypeID])
REFERENCES [dbo].[OG_LeaveType] ([ID])
GO

ALTER TABLE [dbo].[Leave] CHECK CONSTRAINT [FK_Leave_OG_LeaveType]
GO

ALTER TABLE [dbo].[Leave] ADD  CONSTRAINT [DF_Leave_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO

ALTER TABLE [dbo].[Leave] ADD  CONSTRAINT [DF_Leave_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO



USE [HRManagementSystem]
GO

/****** Object:  Table [dbo].[OG_LeaveType]    Script Date: 09/03/2016 14:47:56 ******/
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[OG_LeaveType] ADD  CONSTRAINT [DF_OG_LeaveType_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO

ALTER TABLE [dbo].[OG_LeaveType] ADD  CONSTRAINT [DF_OG_LeaveType_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO

--6-sep--2016

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.[User] SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.Leave ADD CONSTRAINT
	FK_Leave_User FOREIGN KEY
	(
	UserID
	) REFERENCES dbo.[User]
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.Leave SET (LOCK_ESCALATION = TABLE)
GO
COMMIT











/*
16 Nov 2016
*/
/*
EXEC SELECT_Attendance @StartDate='11/01/2016', @EndDate='11/15/2016',@UserId=17
*/
ALTER PROC dbo.SELECT_Attendance
@StartDate DATETIME,
@EndDate DATETIME,
@UserId INT=NULL,
@BranchID INT=NULL
AS
BEGIN

SELECT a.Id AS AttendanceId, a.[Date], u.ID AS UserId, u.FirstName, u.UserTypeID,u.Designation, u.AccountNumber,
d.ID AS DepartmentId,ud.EffectiveDate AS DEffectiveDate, ud.RetiredDate AS DRetiredDate,d.Name AS DepartmentName,
s.ID AS ShiftId, us.ShiftId,us.EffectiveDate AS SEffectiveDate, us.RetiredDate AS SRetiredDate, s.Name AS ShiftName,
ats.Id AS AttendanceStatusId, ats.IsShiftOffDay, ats.IsHoliday, ats.IsLeaveDay, ats.IsQuarterDay, ats.IsHalfDay, ats.IsFullDay, ats.IsLate,
ats.IsEarly,ats.LateMinutes,ats.EarlyMinutes, ats.WorkingMinutes, ats.TotalMinutes, ats.OverTimeMinutes, ats.Reason,ats.IsApproved,ats.Remarks,
XADTI.Id AS XADTIAttendanceDetailId,XADTI.AttendanceId AS XADTIAttendanceId, XADTI.StartDate AS XADTIStartDate, XADTI.EndDate AS XADTIEndDate,
XADTO.Id AS XADTOAttendanceDetailId,XADTO.AttendanceId AS XADTOAttendanceId, XADTO.StartDate AS XADTOStartDate, XADTO.EndDate AS XADTOEndDate
FROM Attendance a
LEFT JOIN [User] u ON a.UserId=u.Id
LEFT JOIN UserDepartment ud ON u.ID=ud.UserId AND a.[Date]>=ud.EffectiveDate AND a.[Date]<=ISNULL(ud.RetiredDate,a.[Date])
LEFT JOIN BranchDepartment bd ON ud.DepartmentID=bd.DepartmentID AND bd.BranchID=@BranchID
LEFT JOIN Department d ON ud.DepartmentId=d.Id
LEFT JOIN UserShift us ON us.UserId=u.ID AND a.[Date]>=us.EffectiveDate AND a.[Date]<=ISNULL(us.RetiredDate,a.[Date])
LEFT JOIN Shift s ON us.ShiftId=s.Id
LEFT JOIN AttendanceStatus ats ON a.Id=ats.AttendanceId AND ats.IsActive=1
OUTER APPLY ( SELECT TOP 1 adTI.* FROM AttendanceDetail adTI WHERE a.Id=adTI.AttendanceId AND adTI.AttendanceTypeId=1 AND adTI.IsActive=1 ORDER BY adTI.StartDate) AS XADTI
OUTER APPLY ( SELECT TOP 1 adTO.* FROM AttendanceDetail adTO WHERE a.Id=adTO.AttendanceId AND adTO.AttendanceTypeId=1 AND adTO.IsActive=1 ORDER BY adTO.EndDate DESC) AS XADTO
WHERE a.[Date] BETWEEN @StartDate AND @EndDate AND a.IsActive=1
AND (@UserId IS NULL OR a.UserId=@UserId)
AND (@BranchID IS NULL OR bd.BranchID IS NOT NULL)
ORDER BY a.[Date]
END




/*3 December 2016*/
/*
EXEC SELECT_AttendanceSummary @StartDate='11/01/2016', @EndDate='11/01/2016'
EXEC SELECT_AttendanceSummary @StartDate='11/04/2016', @EndDate='11/04/2016'
*/
CREATE PROC dbo.SELECT_AttendanceSummary
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
EXEC SELECT_Attendance @StartDate='11/01/2016', @EndDate='11/15/2016',@UserId=17
*/
ALTER PROC dbo.SELECT_Attendance
@StartDate DATETIME,
@EndDate DATETIME,
@UserId INT=NULL,
@BranchID INT=NULL
AS
BEGIN

SELECT a.Id AS AttendanceId, a.[Date], u.ID AS UserId, u.FirstName, u.UserTypeID,u.Designation, u.AccountNumber, u.SalaryTypeID,st.Name AS SalaryTypeName,
d.ID AS DepartmentId,ud.EffectiveDate AS DEffectiveDate, ud.RetiredDate AS DRetiredDate,d.Name AS DepartmentName,bd.BranchID, b.Name AS BranchName,
s.ID AS ShiftId, us.ShiftId,us.EffectiveDate AS SEffectiveDate, us.RetiredDate AS SRetiredDate, s.Name AS ShiftName,
ats.Id AS AttendanceStatusId, ats.IsShiftOffDay, ats.IsHoliday, ats.IsLeaveDay, ats.IsQuarterDay, ats.IsHalfDay, ats.IsFullDay, ats.IsLate,
ats.IsEarly,ats.LateMinutes,ats.EarlyMinutes, ats.WorkingMinutes, ats.TotalMinutes, ats.OverTimeMinutes, ats.Reason,ats.IsApproved,ats.Remarks,
XADTI.Id AS XADTIAttendanceDetailId,XADTI.AttendanceId AS XADTIAttendanceId, XADTI.StartDate AS XADTIStartDate, XADTI.EndDate AS XADTIEndDate,
XADTO.Id AS XADTOAttendanceDetailId,XADTO.AttendanceId AS XADTOAttendanceId, XADTO.StartDate AS XADTOStartDate, XADTO.EndDate AS XADTOEndDate
FROM Attendance a
LEFT JOIN [User] u ON a.UserId=u.Id
LEFT JOIN SalaryType st ON u.SalaryTypeID=st.ID
LEFT JOIN UserDepartment ud ON u.ID=ud.UserId AND a.[Date]>=ud.EffectiveDate AND a.[Date]<=ISNULL(ud.RetiredDate,a.[Date])
LEFT JOIN BranchDepartment bd ON ud.DepartmentID=bd.DepartmentID AND bd.BranchID=@BranchID
LEFT JOIN Branch b ON bd.BranchID=b.Id
LEFT JOIN Department d ON ud.DepartmentId=d.Id
LEFT JOIN UserShift us ON us.UserId=u.ID AND a.[Date]>=us.EffectiveDate AND a.[Date]<=ISNULL(us.RetiredDate,a.[Date])
LEFT JOIN Shift s ON us.ShiftId=s.Id
LEFT JOIN AttendanceStatus ats ON a.Id=ats.AttendanceId AND ats.IsActive=1
OUTER APPLY ( SELECT TOP 1 adTI.* FROM AttendanceDetail adTI WHERE a.Id=adTI.AttendanceId AND adTI.AttendanceTypeId=1 AND adTI.IsActive=1 ORDER BY adTI.StartDate) AS XADTI
OUTER APPLY ( SELECT TOP 1 adTO.* FROM AttendanceDetail adTO WHERE a.Id=adTO.AttendanceId AND adTO.AttendanceTypeId=1 AND adTO.IsActive=1 ORDER BY adTO.EndDate DESC) AS XADTO
WHERE a.[Date] BETWEEN @StartDate AND @EndDate AND a.IsActive=1
AND (@UserId IS NULL OR a.UserId=@UserId)
AND (@BranchID IS NULL OR bd.BranchID IS NOT NULL)
ORDER BY a.[Date]
END



/*
EXEC SELECT_Attendance @StartDate='11/01/2016', @EndDate='11/01/2016',@UserId=17
*/
ALTER PROC dbo.SELECT_Attendance
@StartDate DATETIME,
@EndDate DATETIME,
@UserId INT=NULL,
@BranchID INT=NULL
AS
BEGIN

SELECT a.Id AS AttendanceId, a.[Date], u.ID AS UserId, u.FirstName, u.UserTypeID,u.Designation, u.AccountNumber, u.SalaryTypeID,st.Name AS SalaryTypeName,
d.ID AS DepartmentId,ud.EffectiveDate AS DEffectiveDate, ud.RetiredDate AS DRetiredDate,d.Name AS DepartmentName,bd.BranchID, b.Name AS BranchName,
s.ID AS ShiftId, us.ShiftId,us.EffectiveDate AS SEffectiveDate, us.RetiredDate AS SRetiredDate, s.Name AS ShiftName,
ats.Id AS AttendanceStatusId, ats.IsShiftOffDay, ats.IsHoliday, ats.IsLeaveDay, ats.IsQuarterDay, ats.IsHalfDay, ats.IsFullDay, ats.IsLate,
ats.IsEarly,ats.LateMinutes,ats.EarlyMinutes, ats.WorkingMinutes, ats.TotalMinutes, ats.OverTimeMinutes, ats.Reason,ats.IsApproved,ats.Remarks,
XADTI.Id AS XADTIAttendanceDetailId,XADTI.AttendanceId AS XADTIAttendanceId, XADTI.StartDate AS XADTIStartDate, XADTI.EndDate AS XADTIEndDate,
XADTO.Id AS XADTOAttendanceDetailId,XADTO.AttendanceId AS XADTOAttendanceId, XADTO.StartDate AS XADTOStartDate, XADTO.EndDate AS XADTOEndDate
FROM Department d  
INNER JOIN UserDepartment ud ON ud.DepartmentId=d.Id 
INNER JOIN [User] u ON ud.UserId=u.Id  
INNER JOIN SalaryType st ON u.SalaryTypeID=st.ID    
INNER JOIN BranchDepartment bd ON d.ID=bd.DepartmentID   
INNER JOIN Branch b ON bd.BranchID=b.ID   
INNER JOIN UserShift us ON us.UserId=u.ID   
INNER JOIN Shift s ON us.ShiftId=s.Id   
LEFT JOIN Attendance a ON a.UserID=u.ID AND a.[Date] BETWEEN @StartDate AND @EndDate AND a.IsActive=1  
LEFT JOIN AttendanceStatus ats ON a.Id=ats.AttendanceId AND ats.IsActive=1  
OUTER APPLY ( SELECT TOP 1 adTI.* FROM AttendanceDetail adTI WHERE a.Id=adTI.AttendanceId AND adTI.AttendanceTypeId=1 AND adTI.IsActive=1 ORDER BY adTI.StartDate) AS XADTI  
OUTER APPLY ( SELECT TOP 1 adTO.* FROM AttendanceDetail adTO WHERE a.Id=adTO.AttendanceId AND adTO.AttendanceTypeId=1 AND adTO.IsActive=1 ORDER BY adTO.EndDate DESC) AS XADTO  
WHERE us.RetiredDate IS NULL  AND ud.RetiredDate IS NULL  
AND d.IsActive=1 AND b.IsActive=1
ORDER BY a.[Date]
END



--7Dec16
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
/*
EXEC SELECT_Attendance @StartDate='12/07/2016', @EndDate='12/07/2016',@UserId=null,@BranchId=1
*/
ALTER PROC dbo.SELECT_Attendance
@StartDate DATETIME,
@EndDate DATETIME,
@UserId INT=NULL,
@BranchID INT=NULL
AS
BEGIN

SELECT a.Id AS AttendanceId, a.[Date], u.ID AS UserId, u.FirstName, u.UserTypeID,u.Designation, u.AccountNumber, u.SalaryTypeID,st.Name AS SalaryTypeName,
d.ID AS DepartmentId,ud.EffectiveDate AS DEffectiveDate, ud.RetiredDate AS DRetiredDate,d.Name AS DepartmentName,bd.BranchID, b.Name AS BranchName,
s.ID AS ShiftId, us.ShiftId,us.EffectiveDate AS SEffectiveDate, us.RetiredDate AS SRetiredDate, s.Name AS ShiftName,
ats.Id AS AttendanceStatusId, ats.IsShiftOffDay, ats.IsHoliday, ats.IsLeaveDay, ats.IsQuarterDay, ats.IsHalfDay, ats.IsFullDay, ats.IsLate,
ats.IsEarly,ats.LateMinutes,ats.EarlyMinutes, ats.WorkingMinutes, ats.TotalMinutes, ats.OverTimeMinutes, ats.Reason,ats.IsApproved,ats.Remarks,
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
WHERE us.RetiredDate IS NULL  AND ud.RetiredDate IS NULL  
AND d.IsActive=1 AND b.IsActive=1
AND (@UserId is null or u.id=@Userid)
AND (@BranchID is null or b.id=@BranchID)

ORDER BY a.[Date]
END




-- 12Dec16

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION dbo.GetName 
(
	@FN Varchar(300)=null,
	@MN Varchar(300)=null,
	@LN Varchar(300)=null
)
RETURNS varchar(900)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Name varchar(900)=''

	IF(@FN IS NOT NULL AND LEN(LTRIM(RTRIM(@FN)))>0)
		SET @Name=@FN
	IF(@MN IS NOT NULL AND LEN(LTRIM(RTRIM(@MN)))>0)
		SET @Name=@Name +' '+ @MN
	IF(@LN IS NOT NULL AND LEN(LTRIM(RTRIM(@LN)))>0)
		SET @Name=@Name +' '+ @LN
	RETURN @Name
END
GO




---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

/*
EXEC SELECT_Attendance @StartDate='12/07/2016', @EndDate='12/07/2016',@UserId=null,@BranchId=1
*/
ALTER PROC dbo.SELECT_Attendance
@StartDate DATETIME,
@EndDate DATETIME,
@UserId INT=NULL,
@BranchID INT=NULL
AS
BEGIN

SELECT a.Id AS AttendanceId, a.[Date], u.ID AS UserId, u.FirstName, u.UserTypeID,u.Designation,u.ImagePath, u.AccountNumber, u.SalaryTypeID,st.Name AS SalaryTypeName,
d.ID AS DepartmentId,ud.EffectiveDate AS DEffectiveDate, ud.RetiredDate AS DRetiredDate,d.Name AS DepartmentName,bd.BranchID, b.Name AS BranchName,
s.ID AS ShiftId, us.ShiftId,us.EffectiveDate AS SEffectiveDate, us.RetiredDate AS SRetiredDate, s.Name AS ShiftName,
ats.Id AS AttendanceStatusId, ats.IsShiftOffDay, ats.IsHoliday, ats.IsLeaveDay, ats.IsQuarterDay, ats.IsHalfDay, ats.IsFullDay, ats.IsLate,
ats.IsEarly,ats.LateMinutes,ats.EarlyMinutes, ats.WorkingMinutes, ats.TotalMinutes, ats.OverTimeMinutes, ats.Reason,ats.IsApproved,ats.Remarks,
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

/*
SELECT_MonthlyAttendanceSummary '11/01/2016','11/30/2016'

*/
CREATE PROC dbo.SELECT_MonthlyAttendanceSummary
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
