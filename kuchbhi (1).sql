USE [RigelonicHR]
GO
/****** Object:  Table [dbo].[Attendance]    Script Date: 3/24/2020 7:35:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  Table [dbo].[AttendanceDetail]    Script Date: 3/24/2020 7:35:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  Table [dbo].[AttendancePolicy]    Script Date: 3/24/2020 7:35:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  Table [dbo].[AttendanceStatus]    Script Date: 3/24/2020 7:35:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  Table [dbo].[AttendanceType]    Script Date: 3/24/2020 7:35:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  Table [dbo].[AttendanceVariable]    Script Date: 3/24/2020 7:35:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  Table [dbo].[Branch]    Script Date: 3/24/2020 7:35:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  Table [dbo].[BranchDepartment]    Script Date: 3/24/2020 7:35:45 PM ******/
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
/****** Object:  Table [dbo].[BranchShift]    Script Date: 3/24/2020 7:35:45 PM ******/
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
/****** Object:  Table [dbo].[City]    Script Date: 3/24/2020 7:35:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  Table [dbo].[Configuration]    Script Date: 3/24/2020 7:35:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  Table [dbo].[ContactType]    Script Date: 3/24/2020 7:35:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  Table [dbo].[Country]    Script Date: 3/24/2020 7:35:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  Table [dbo].[Department]    Script Date: 3/24/2020 7:35:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  Table [dbo].[Device]    Script Date: 3/24/2020 7:35:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  Table [dbo].[DeviceModal]    Script Date: 3/24/2020 7:35:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  Table [dbo].[Gender]    Script Date: 3/24/2020 7:35:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  Table [dbo].[Holiday]    Script Date: 3/24/2020 7:35:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  Table [dbo].[Leave]    Script Date: 3/24/2020 7:35:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  Table [dbo].[LeaveType]    Script Date: 3/24/2020 7:35:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  Table [dbo].[OG_LeaveType]    Script Date: 3/24/2020 7:35:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  Table [dbo].[Payroll]    Script Date: 3/24/2020 7:35:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  Table [dbo].[PayrollCycle]    Script Date: 3/24/2020 7:35:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  Table [dbo].[PayrollDetail]    Script Date: 3/24/2020 7:35:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  Table [dbo].[PayrollPolicy]    Script Date: 3/24/2020 7:35:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  Table [dbo].[PayrollVariable]    Script Date: 3/24/2020 7:35:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  Table [dbo].[Religion]    Script Date: 3/24/2020 7:35:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  Table [dbo].[SalaryType]    Script Date: 3/24/2020 7:35:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  Table [dbo].[Shift]    Script Date: 3/24/2020 7:35:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  Table [dbo].[ShiftOffDay]    Script Date: 3/24/2020 7:35:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  Table [dbo].[State]    Script Date: 3/24/2020 7:35:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  Table [dbo].[tmp]    Script Date: 3/24/2020 7:35:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  Table [dbo].[User]    Script Date: 3/24/2020 7:35:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  Table [dbo].[UserContact]    Script Date: 3/24/2020 7:35:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  Table [dbo].[UserDepartment]    Script Date: 3/24/2020 7:35:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  Table [dbo].[UserShift]    Script Date: 3/24/2020 7:35:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  Table [dbo].[UserType]    Script Date: 3/24/2020 7:35:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
SET IDENTITY_INSERT [dbo].[Attendance] ON 

INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (34, 3, CAST(N'2020-01-03T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:23:42.477' AS DateTime), NULL, 1, N'', CAST(N'2020-01-03T20:48:00.000' AS DateTime), CAST(N'2020-01-03T10:12:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (35, 2, CAST(N'2020-01-03T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:25:13.030' AS DateTime), NULL, 1, N'', CAST(N'2020-01-03T22:10:00.000' AS DateTime), CAST(N'2020-01-03T10:05:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (36, 4, CAST(N'2020-01-03T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:26:27.183' AS DateTime), NULL, 1, N'', CAST(N'2020-01-03T22:30:00.000' AS DateTime), CAST(N'2020-01-03T14:00:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (37, 2, CAST(N'2020-01-04T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:27:42.690' AS DateTime), NULL, 1, N'', CAST(N'2020-01-04T19:35:00.000' AS DateTime), CAST(N'2020-01-04T10:05:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (38, 3, CAST(N'2020-01-04T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:27:58.670' AS DateTime), NULL, 1, N'', CAST(N'2020-01-04T19:21:00.000' AS DateTime), CAST(N'2020-01-04T10:10:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (39, 4, CAST(N'2020-01-04T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:28:29.813' AS DateTime), NULL, 1, N'', CAST(N'2020-01-04T00:00:00.000' AS DateTime), CAST(N'2020-01-04T00:00:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (40, 2, CAST(N'2020-01-06T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:31:54.327' AS DateTime), NULL, 1, N'', CAST(N'2020-01-06T19:20:00.000' AS DateTime), CAST(N'2020-01-06T10:19:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (41, 3, CAST(N'2020-01-06T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:32:05.893' AS DateTime), NULL, 1, N'', CAST(N'2020-01-06T19:20:00.000' AS DateTime), CAST(N'2020-01-06T10:19:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (42, 4, CAST(N'2020-01-06T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:32:39.963' AS DateTime), NULL, 1, N'', CAST(N'2020-01-06T19:45:00.000' AS DateTime), CAST(N'2020-01-06T11:30:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (43, 3, CAST(N'2020-01-07T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:33:19.200' AS DateTime), NULL, 1, N'', CAST(N'2020-01-07T19:20:00.000' AS DateTime), CAST(N'2020-01-07T10:11:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (44, 2, CAST(N'2020-01-07T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:33:30.297' AS DateTime), NULL, 1, N'', CAST(N'2020-01-07T19:20:00.000' AS DateTime), CAST(N'2020-01-07T10:26:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (45, 4, CAST(N'2020-01-07T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:33:54.897' AS DateTime), NULL, 1, N'', CAST(N'2020-01-07T21:05:00.000' AS DateTime), CAST(N'2020-01-07T13:55:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (46, 2, CAST(N'2020-01-08T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:34:40.597' AS DateTime), NULL, 1, N'', CAST(N'2020-01-08T22:46:00.000' AS DateTime), CAST(N'2020-01-08T10:05:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (47, 4, CAST(N'2020-01-08T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:35:02.983' AS DateTime), NULL, 1, N'', CAST(N'2020-01-08T22:30:00.000' AS DateTime), CAST(N'2020-01-08T11:20:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (48, 3, CAST(N'2020-01-08T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:35:09.930' AS DateTime), NULL, 1, N'', CAST(N'2020-01-08T22:30:00.000' AS DateTime), CAST(N'2020-01-08T11:20:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (49, 3, CAST(N'2020-01-09T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:35:41.253' AS DateTime), NULL, 1, N'', CAST(N'2020-01-09T21:15:00.000' AS DateTime), CAST(N'2020-01-09T10:15:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (50, 2, CAST(N'2020-01-09T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:36:00.637' AS DateTime), NULL, 1, N'', CAST(N'2020-01-09T20:43:00.000' AS DateTime), CAST(N'2020-01-09T10:40:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (51, 4, CAST(N'2020-01-09T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:36:25.973' AS DateTime), NULL, 1, N'', CAST(N'2020-01-09T22:00:00.000' AS DateTime), CAST(N'2020-01-09T14:55:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (52, 2, CAST(N'2020-01-10T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:37:04.097' AS DateTime), NULL, 1, N'', CAST(N'2020-01-10T21:28:00.000' AS DateTime), CAST(N'2020-01-10T10:13:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (53, 3, CAST(N'2020-01-10T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:37:23.637' AS DateTime), NULL, 1, N'', CAST(N'2020-01-10T21:13:00.000' AS DateTime), CAST(N'2020-01-10T10:17:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (54, 4, CAST(N'2020-01-10T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:37:44.127' AS DateTime), NULL, 1, N'', CAST(N'2020-01-10T23:00:00.000' AS DateTime), CAST(N'2020-01-10T00:00:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (55, 2, CAST(N'2020-01-11T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:38:22.090' AS DateTime), NULL, 1, N'', CAST(N'2020-01-11T20:48:00.000' AS DateTime), CAST(N'2020-01-11T10:05:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (56, 4, CAST(N'2020-01-11T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:38:48.670' AS DateTime), NULL, 1, N'', CAST(N'2020-01-11T21:15:00.000' AS DateTime), CAST(N'2020-01-11T15:20:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (57, 3, CAST(N'2020-01-13T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:39:14.937' AS DateTime), NULL, 1, N'', CAST(N'2020-01-13T20:20:00.000' AS DateTime), CAST(N'2020-01-13T10:00:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (58, 2, CAST(N'2020-01-13T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:39:30.040' AS DateTime), NULL, 1, N'', CAST(N'2020-01-13T20:20:00.000' AS DateTime), CAST(N'2020-01-13T10:50:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (59, 3, CAST(N'2020-01-14T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:39:55.160' AS DateTime), NULL, 1, N'', CAST(N'2020-01-14T22:15:00.000' AS DateTime), CAST(N'2020-01-14T10:15:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (60, 2, CAST(N'2020-01-14T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:40:11.987' AS DateTime), NULL, 1, N'', CAST(N'2020-01-14T20:15:00.000' AS DateTime), CAST(N'2020-01-14T10:25:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (61, 4, CAST(N'2020-01-14T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:40:26.420' AS DateTime), NULL, 1, N'', CAST(N'2020-01-14T20:15:00.000' AS DateTime), CAST(N'2020-01-14T10:30:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (62, 2, CAST(N'2020-01-15T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:41:50.280' AS DateTime), NULL, 1, N'', CAST(N'2020-01-15T19:40:00.000' AS DateTime), CAST(N'2020-01-15T10:05:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (63, 3, CAST(N'2020-01-15T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:42:08.310' AS DateTime), NULL, 1, N'', CAST(N'2020-01-15T19:22:00.000' AS DateTime), CAST(N'2020-01-15T10:35:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (64, 4, CAST(N'2020-01-15T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:42:26.537' AS DateTime), NULL, 1, N'', CAST(N'2020-01-15T22:15:00.000' AS DateTime), CAST(N'2020-01-15T13:00:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (65, 2, CAST(N'2020-01-16T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:45:23.487' AS DateTime), NULL, 1, N'', CAST(N'2020-01-16T19:00:00.000' AS DateTime), CAST(N'2020-01-16T09:15:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (66, 3, CAST(N'2020-01-16T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:46:04.903' AS DateTime), NULL, 1, N'', CAST(N'2020-01-16T19:00:00.000' AS DateTime), CAST(N'2020-01-16T09:20:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (67, 4, CAST(N'2020-01-16T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:46:27.807' AS DateTime), NULL, 1, N'', CAST(N'2020-01-16T20:26:00.000' AS DateTime), CAST(N'2020-01-16T11:15:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (68, 3, CAST(N'2020-01-17T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:47:03.413' AS DateTime), NULL, 1, N'', CAST(N'2020-01-17T20:12:00.000' AS DateTime), CAST(N'2020-01-17T09:18:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (69, 4, CAST(N'2020-01-17T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:48:57.523' AS DateTime), NULL, 1, N'', CAST(N'2020-01-17T20:35:00.000' AS DateTime), CAST(N'2020-01-17T10:55:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (70, 3, CAST(N'2020-01-20T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:49:54.200' AS DateTime), NULL, 1, N'', CAST(N'2020-01-20T19:20:00.000' AS DateTime), CAST(N'2020-01-20T09:10:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (71, 2, CAST(N'2020-01-20T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:50:10.933' AS DateTime), NULL, 1, N'', CAST(N'2020-01-20T19:20:00.000' AS DateTime), CAST(N'2020-01-20T09:44:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (72, 4, CAST(N'2020-01-20T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:50:27.483' AS DateTime), NULL, 1, N'', CAST(N'2020-01-20T19:20:00.000' AS DateTime), CAST(N'2020-01-20T00:00:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (73, 3, CAST(N'2020-01-21T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:51:43.023' AS DateTime), NULL, 1, N'', CAST(N'2020-01-21T18:30:00.000' AS DateTime), CAST(N'2020-01-21T09:20:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (74, 2, CAST(N'2020-01-21T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:52:02.483' AS DateTime), NULL, 1, N'', CAST(N'2020-01-21T18:30:00.000' AS DateTime), CAST(N'2020-01-21T09:45:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (75, 4, CAST(N'2020-01-21T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:52:26.203' AS DateTime), NULL, 1, N'', CAST(N'2020-01-21T18:30:00.000' AS DateTime), CAST(N'2020-01-21T11:10:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (76, 3, CAST(N'2020-01-22T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:53:01.590' AS DateTime), NULL, 1, N'', CAST(N'2020-01-22T19:50:00.000' AS DateTime), CAST(N'2020-01-22T09:26:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (77, 2, CAST(N'2020-01-22T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:53:11.117' AS DateTime), NULL, 1, N'', CAST(N'2020-01-22T19:50:00.000' AS DateTime), CAST(N'2020-01-22T09:42:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (78, 4, CAST(N'2020-01-22T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:53:20.433' AS DateTime), NULL, 1, N'', CAST(N'2020-01-22T19:50:00.000' AS DateTime), CAST(N'2020-01-22T09:50:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (79, 3, CAST(N'2020-01-23T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:53:44.890' AS DateTime), NULL, 1, N'', CAST(N'2020-01-23T19:30:00.000' AS DateTime), CAST(N'2020-01-23T09:20:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (80, 2, CAST(N'2020-01-24T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:54:10.553' AS DateTime), NULL, 1, N'', CAST(N'2020-01-24T20:30:00.000' AS DateTime), CAST(N'2020-01-24T09:20:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (81, 3, CAST(N'2020-01-24T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:54:23.660' AS DateTime), NULL, 1, N'', CAST(N'2020-01-24T20:30:00.000' AS DateTime), CAST(N'2020-01-24T09:15:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (82, 4, CAST(N'2020-01-24T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:54:34.953' AS DateTime), NULL, 1, N'', CAST(N'2020-01-24T20:30:00.000' AS DateTime), CAST(N'2020-01-24T09:50:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (83, 2, CAST(N'2020-01-27T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:55:11.657' AS DateTime), NULL, 1, N'', CAST(N'2020-01-27T21:30:00.000' AS DateTime), CAST(N'2020-01-27T09:15:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (84, 3, CAST(N'2020-01-27T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:55:22.617' AS DateTime), NULL, 1, N'', CAST(N'2020-01-27T21:30:00.000' AS DateTime), CAST(N'2020-01-27T09:30:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (85, 4, CAST(N'2020-01-27T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:55:29.363' AS DateTime), NULL, 1, N'', CAST(N'2020-01-27T21:30:00.000' AS DateTime), CAST(N'2020-01-27T09:55:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (86, 2, CAST(N'2020-01-28T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:56:20.810' AS DateTime), NULL, 1, N'', CAST(N'2020-01-28T20:10:00.000' AS DateTime), CAST(N'2020-01-28T09:34:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (87, 3, CAST(N'2020-01-28T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:56:31.067' AS DateTime), NULL, 1, N'', CAST(N'2020-01-28T20:50:00.000' AS DateTime), CAST(N'2020-01-28T09:10:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (88, 4, CAST(N'2020-01-28T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:56:45.460' AS DateTime), NULL, 1, N'', CAST(N'2020-01-28T20:50:00.000' AS DateTime), CAST(N'2020-01-28T14:20:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (89, 3, CAST(N'2020-01-29T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:57:25.243' AS DateTime), NULL, 1, N'', CAST(N'2020-01-29T19:10:00.000' AS DateTime), CAST(N'2020-01-29T09:18:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (90, 4, CAST(N'2020-01-29T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:57:44.363' AS DateTime), NULL, 1, N'', CAST(N'2020-01-29T19:10:00.000' AS DateTime), CAST(N'2020-01-29T10:40:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (91, 2, CAST(N'2020-01-30T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:58:23.533' AS DateTime), NULL, 1, N'', CAST(N'2020-01-30T20:20:00.000' AS DateTime), CAST(N'2020-01-30T10:15:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (92, 3, CAST(N'2020-01-30T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:58:55.807' AS DateTime), NULL, 1, N'', CAST(N'2020-01-30T19:00:00.000' AS DateTime), CAST(N'2020-01-30T10:05:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (93, 4, CAST(N'2020-01-30T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:59:19.927' AS DateTime), NULL, 1, N'', CAST(N'2020-01-30T20:35:00.000' AS DateTime), CAST(N'2020-01-30T11:40:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (94, 2, CAST(N'2020-01-31T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:00:12.397' AS DateTime), NULL, 1, N'', CAST(N'2020-01-31T20:05:00.000' AS DateTime), CAST(N'2020-01-31T10:00:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (95, 3, CAST(N'2020-01-31T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:00:32.270' AS DateTime), NULL, 1, N'', CAST(N'2020-01-31T19:40:00.000' AS DateTime), CAST(N'2020-01-31T10:12:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (96, 4, CAST(N'2020-01-31T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:00:44.627' AS DateTime), NULL, 1, N'', CAST(N'2020-01-31T19:40:00.000' AS DateTime), CAST(N'2020-01-31T10:12:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (97, 3, CAST(N'2020-02-03T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:01:27.437' AS DateTime), NULL, 1, N'', CAST(N'2020-02-03T20:20:00.000' AS DateTime), CAST(N'2020-02-03T10:00:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (98, 2, CAST(N'2020-02-03T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:01:43.097' AS DateTime), NULL, 1, N'', CAST(N'2020-02-03T21:22:00.000' AS DateTime), CAST(N'2020-02-03T10:15:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (99, 4, CAST(N'2020-02-03T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:02:32.423' AS DateTime), NULL, 1, N'', CAST(N'2020-02-03T20:40:00.000' AS DateTime), CAST(N'2020-02-03T11:40:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (100, 3, CAST(N'2020-02-04T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:04:28.943' AS DateTime), NULL, 1, N'', CAST(N'2020-02-04T19:05:00.000' AS DateTime), CAST(N'2020-02-04T10:00:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (101, 2, CAST(N'2020-02-04T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:04:39.830' AS DateTime), NULL, 1, N'', CAST(N'2020-02-04T19:05:00.000' AS DateTime), CAST(N'2020-02-04T10:20:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (102, 3, CAST(N'2020-02-05T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:05:57.730' AS DateTime), NULL, 1, N'', CAST(N'2020-02-05T21:36:00.000' AS DateTime), CAST(N'2020-02-05T10:00:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (103, 2, CAST(N'2020-02-05T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:06:14.080' AS DateTime), NULL, 1, N'', CAST(N'2020-02-05T21:36:00.000' AS DateTime), CAST(N'2020-02-05T11:40:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (104, 4, CAST(N'2020-02-05T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:06:26.740' AS DateTime), NULL, 1, N'', CAST(N'2020-02-05T21:36:00.000' AS DateTime), CAST(N'2020-02-05T13:30:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (105, 2, CAST(N'2020-02-06T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:06:55.147' AS DateTime), NULL, 1, N'', CAST(N'2020-02-06T19:45:00.000' AS DateTime), CAST(N'2020-02-06T10:00:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (106, 2, CAST(N'2020-02-07T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:07:59.780' AS DateTime), NULL, 1, N'', CAST(N'2020-02-07T19:46:00.000' AS DateTime), CAST(N'2020-02-07T10:00:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (107, 4, CAST(N'2020-02-07T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:08:36.030' AS DateTime), NULL, 1, N'', CAST(N'2020-02-07T21:15:00.000' AS DateTime), CAST(N'2020-02-07T11:10:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (108, 2, CAST(N'2020-02-10T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:09:18.267' AS DateTime), NULL, 1, N'', CAST(N'2020-02-10T19:00:00.000' AS DateTime), CAST(N'2020-02-10T10:00:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (109, 4, CAST(N'2020-02-10T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:09:31.143' AS DateTime), NULL, 1, N'', CAST(N'2020-02-10T19:20:00.000' AS DateTime), CAST(N'2020-02-10T10:35:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (110, 2, CAST(N'2020-02-11T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:09:59.377' AS DateTime), NULL, 1, N'', CAST(N'2020-02-11T21:50:00.000' AS DateTime), CAST(N'2020-02-11T10:00:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (111, 4, CAST(N'2020-02-11T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:10:16.043' AS DateTime), NULL, 1, N'', CAST(N'2020-02-11T23:00:00.000' AS DateTime), CAST(N'2020-02-11T14:35:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (112, 2, CAST(N'2020-02-12T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:11:19.220' AS DateTime), NULL, 1, N'', CAST(N'2020-02-12T19:15:00.000' AS DateTime), CAST(N'2020-02-12T10:10:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (113, 4, CAST(N'2020-02-12T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:11:31.643' AS DateTime), NULL, 1, N'', CAST(N'2020-02-12T19:15:00.000' AS DateTime), CAST(N'2020-02-12T13:10:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (114, 4, CAST(N'2020-02-13T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:11:57.230' AS DateTime), NULL, 1, N'', CAST(N'2020-02-13T19:15:00.000' AS DateTime), CAST(N'2020-02-13T09:45:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (115, 4, CAST(N'2020-02-14T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:13:40.690' AS DateTime), NULL, 1, N'', CAST(N'2020-02-14T19:30:00.000' AS DateTime), CAST(N'2020-02-14T09:58:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (116, 2, CAST(N'2020-02-14T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:14:13.573' AS DateTime), NULL, 1, N'', CAST(N'2020-02-14T20:05:00.000' AS DateTime), CAST(N'2020-02-14T10:10:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (117, 2, CAST(N'2020-02-17T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:23:04.487' AS DateTime), NULL, 1, N'', CAST(N'2020-02-17T20:20:00.000' AS DateTime), CAST(N'2020-02-17T10:15:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (118, 4, CAST(N'2020-02-17T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:23:19.483' AS DateTime), NULL, 1, N'', CAST(N'2020-02-17T18:05:00.000' AS DateTime), CAST(N'2020-02-17T10:05:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (119, 2, CAST(N'2020-02-18T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:24:55.443' AS DateTime), NULL, 1, N'', CAST(N'2020-02-18T19:25:00.000' AS DateTime), CAST(N'2020-02-18T10:15:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (120, 4, CAST(N'2020-02-19T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:25:33.163' AS DateTime), NULL, 1, N'', CAST(N'2020-02-19T19:37:00.000' AS DateTime), CAST(N'2020-02-19T09:55:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (121, 2, CAST(N'2020-02-19T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:25:50.070' AS DateTime), NULL, 1, N'', CAST(N'2020-02-19T21:05:00.000' AS DateTime), CAST(N'2020-02-19T10:15:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (122, 3, CAST(N'2020-02-20T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:26:57.353' AS DateTime), NULL, 1, N'', CAST(N'2020-02-20T19:45:00.000' AS DateTime), CAST(N'2020-02-20T10:10:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (123, 3, CAST(N'2020-02-21T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:31:52.517' AS DateTime), NULL, 1, N'', CAST(N'2020-02-21T19:40:00.000' AS DateTime), CAST(N'2020-02-21T10:05:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (124, 2, CAST(N'2020-02-21T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:32:11.367' AS DateTime), NULL, 1, N'', CAST(N'2020-02-21T19:40:00.000' AS DateTime), CAST(N'2020-02-21T10:15:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (125, 4, CAST(N'2020-02-21T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:32:19.077' AS DateTime), NULL, 1, N'', CAST(N'2020-02-21T19:40:00.000' AS DateTime), CAST(N'2020-02-21T10:20:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (126, 3, CAST(N'2020-02-24T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:32:57.887' AS DateTime), NULL, 1, N'', CAST(N'2020-02-24T19:50:00.000' AS DateTime), CAST(N'2020-02-24T10:00:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (127, 4, CAST(N'2020-02-24T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:33:09.980' AS DateTime), NULL, 1, N'', CAST(N'2020-02-24T19:25:00.000' AS DateTime), CAST(N'2020-02-24T10:00:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (128, 2, CAST(N'2020-02-24T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:33:20.220' AS DateTime), NULL, 1, N'', CAST(N'2020-02-24T20:13:00.000' AS DateTime), CAST(N'2020-02-24T10:00:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (129, 3, CAST(N'2020-02-25T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:33:38.810' AS DateTime), NULL, 1, N'', CAST(N'2020-02-25T20:20:00.000' AS DateTime), CAST(N'2020-02-25T10:00:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (130, 2, CAST(N'2020-02-25T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:33:51.827' AS DateTime), NULL, 1, N'', CAST(N'2020-02-25T20:30:00.000' AS DateTime), CAST(N'2020-02-25T10:15:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (131, 3, CAST(N'2020-02-26T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:34:26.850' AS DateTime), NULL, 1, N'', CAST(N'2020-02-26T19:55:00.000' AS DateTime), CAST(N'2020-02-26T10:10:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (132, 2, CAST(N'2020-02-26T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:34:37.470' AS DateTime), NULL, 1, N'', CAST(N'2020-02-26T20:00:00.000' AS DateTime), CAST(N'2020-02-26T10:10:00.000' AS DateTime))
GO
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (133, 4, CAST(N'2020-02-26T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:34:53.490' AS DateTime), NULL, 1, N'', CAST(N'2020-02-26T19:30:00.000' AS DateTime), CAST(N'2020-02-26T10:23:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (134, 2, CAST(N'2020-02-27T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:35:17.033' AS DateTime), NULL, 1, N'', CAST(N'2020-02-27T19:07:00.000' AS DateTime), CAST(N'2020-02-27T10:03:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (135, 3, CAST(N'2020-02-27T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:35:29.337' AS DateTime), NULL, 1, N'', CAST(N'2020-02-27T19:23:00.000' AS DateTime), CAST(N'2020-02-27T10:07:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (136, 4, CAST(N'2020-02-27T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:35:48.453' AS DateTime), NULL, 1, N'', CAST(N'2020-02-27T19:23:00.000' AS DateTime), CAST(N'2020-02-27T14:33:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (137, 3, CAST(N'2020-02-28T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:36:16.403' AS DateTime), NULL, 1, N'', CAST(N'2020-02-28T20:15:00.000' AS DateTime), CAST(N'2020-02-28T10:05:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (138, 2, CAST(N'2020-02-28T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:36:53.447' AS DateTime), NULL, 1, N'', CAST(N'2020-02-28T20:00:00.000' AS DateTime), CAST(N'2020-02-28T10:10:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (139, 4, CAST(N'2020-02-28T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:37:09.357' AS DateTime), NULL, 1, N'', CAST(N'2020-02-28T20:40:00.000' AS DateTime), CAST(N'2020-02-28T11:29:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (140, 3, CAST(N'2020-02-29T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:37:40.460' AS DateTime), NULL, 1, N'', CAST(N'2020-02-29T16:00:00.000' AS DateTime), CAST(N'2020-02-29T09:45:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (141, 2, CAST(N'2020-02-29T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:38:42.577' AS DateTime), NULL, 1, N'', CAST(N'2020-02-29T19:20:00.000' AS DateTime), CAST(N'2020-02-29T10:40:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (142, 4, CAST(N'2020-02-29T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:39:27.047' AS DateTime), NULL, 1, N'', CAST(N'2020-02-29T22:45:00.000' AS DateTime), CAST(N'2020-02-29T15:05:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (143, 3, CAST(N'2020-03-02T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:45:06.190' AS DateTime), NULL, 1, N'', CAST(N'2020-03-02T19:20:00.000' AS DateTime), CAST(N'2020-03-02T09:50:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (144, 2, CAST(N'2020-03-02T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:46:54.827' AS DateTime), NULL, 1, N'', CAST(N'2020-03-02T15:55:00.000' AS DateTime), CAST(N'2020-03-02T10:05:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (145, 3, CAST(N'2020-03-03T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:47:28.810' AS DateTime), NULL, 1, N'', CAST(N'2020-03-03T19:20:00.000' AS DateTime), CAST(N'2020-03-03T10:00:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (146, 2, CAST(N'2020-03-03T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:48:38.993' AS DateTime), NULL, 1, N'', CAST(N'2020-03-03T19:20:00.000' AS DateTime), CAST(N'2020-03-03T10:25:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (147, 3, CAST(N'2020-03-04T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:49:12.873' AS DateTime), NULL, 1, N'', CAST(N'2020-03-04T20:35:00.000' AS DateTime), CAST(N'2020-03-04T09:55:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (148, 2, CAST(N'2020-03-04T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:49:30.803' AS DateTime), NULL, 1, N'', CAST(N'2020-03-04T21:15:00.000' AS DateTime), CAST(N'2020-03-04T10:22:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (149, 4, CAST(N'2020-03-04T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:49:50.507' AS DateTime), NULL, 1, N'', CAST(N'2020-03-04T21:30:00.000' AS DateTime), CAST(N'2020-03-04T12:45:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (150, 3, CAST(N'2020-03-06T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:53:06.943' AS DateTime), NULL, 1, N'', CAST(N'2020-03-06T20:05:00.000' AS DateTime), CAST(N'2020-03-06T09:50:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (151, 2, CAST(N'2020-03-06T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:53:23.000' AS DateTime), NULL, 1, N'', CAST(N'2020-03-06T19:30:00.000' AS DateTime), CAST(N'2020-03-06T10:15:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (152, 3, CAST(N'2020-03-09T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:54:16.073' AS DateTime), NULL, 1, N'', CAST(N'2020-03-09T20:05:00.000' AS DateTime), CAST(N'2020-03-09T09:55:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (153, 4, CAST(N'2020-03-09T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:54:36.880' AS DateTime), NULL, 1, N'', CAST(N'2020-03-09T21:23:00.000' AS DateTime), CAST(N'2020-03-09T13:10:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (154, 3, CAST(N'2020-03-10T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:55:12.457' AS DateTime), NULL, 1, N'', CAST(N'2020-03-10T19:03:00.000' AS DateTime), CAST(N'2020-03-10T10:05:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (155, 2, CAST(N'2020-03-10T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:55:32.823' AS DateTime), NULL, 1, N'', CAST(N'2020-03-10T19:03:00.000' AS DateTime), CAST(N'2020-03-10T10:00:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (156, 3, CAST(N'2020-03-11T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:56:01.063' AS DateTime), NULL, 1, N'', CAST(N'2020-03-11T19:31:00.000' AS DateTime), CAST(N'2020-03-11T10:00:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (157, 2, CAST(N'2020-03-11T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:56:11.063' AS DateTime), NULL, 1, N'', CAST(N'2020-03-11T19:31:00.000' AS DateTime), CAST(N'2020-03-11T10:20:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (158, 2, CAST(N'2020-03-12T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:57:25.600' AS DateTime), NULL, 1, N'', CAST(N'2020-03-12T19:05:00.000' AS DateTime), CAST(N'2020-03-12T10:10:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (159, 3, CAST(N'2020-03-12T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:57:37.880' AS DateTime), NULL, 1, N'', CAST(N'2020-03-12T19:05:00.000' AS DateTime), CAST(N'2020-03-12T10:10:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (160, 4, CAST(N'2020-03-12T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:58:01.847' AS DateTime), NULL, 1, N'', CAST(N'2020-03-12T20:40:00.000' AS DateTime), CAST(N'2020-03-12T12:01:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (161, 3, CAST(N'2020-03-13T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:58:33.190' AS DateTime), NULL, 1, N'', CAST(N'2020-03-13T22:05:00.000' AS DateTime), CAST(N'2020-03-13T10:05:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (162, 2, CAST(N'2020-03-13T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:58:45.430' AS DateTime), NULL, 1, N'', CAST(N'2020-03-13T22:10:00.000' AS DateTime), CAST(N'2020-03-13T10:10:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (163, 4, CAST(N'2020-03-13T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:59:00.817' AS DateTime), NULL, 1, N'', CAST(N'2020-03-13T22:10:00.000' AS DateTime), CAST(N'2020-03-13T10:50:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (164, 2, CAST(N'2020-03-14T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:59:38.320' AS DateTime), NULL, 1, N'', CAST(N'2020-03-14T17:27:00.000' AS DateTime), CAST(N'2020-03-14T10:15:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (165, 4, CAST(N'2020-03-14T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T17:00:00.207' AS DateTime), NULL, 1, N'', CAST(N'2020-03-14T18:15:00.000' AS DateTime), CAST(N'2020-03-14T14:25:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (166, 2, CAST(N'2020-03-15T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T17:00:45.017' AS DateTime), NULL, 1, N'', CAST(N'2020-03-15T19:37:00.000' AS DateTime), CAST(N'2020-03-15T10:00:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (167, 3, CAST(N'2020-03-15T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T17:00:52.707' AS DateTime), NULL, 1, N'', CAST(N'2020-03-15T19:37:00.000' AS DateTime), CAST(N'2020-03-15T10:05:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (168, 2, CAST(N'2020-03-16T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T17:02:08.903' AS DateTime), NULL, 1, N'', CAST(N'2020-03-16T19:37:00.000' AS DateTime), CAST(N'2020-03-16T10:05:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (169, 3, CAST(N'2020-03-16T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T17:02:12.080' AS DateTime), NULL, 1, N'', CAST(N'2020-03-16T19:37:00.000' AS DateTime), CAST(N'2020-03-16T10:05:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (170, 3, CAST(N'2020-03-17T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T17:02:47.027' AS DateTime), NULL, 1, N'', CAST(N'2020-03-17T19:30:00.000' AS DateTime), CAST(N'2020-03-17T10:13:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (171, 2, CAST(N'2020-03-17T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T17:02:56.590' AS DateTime), NULL, 1, N'', CAST(N'2020-03-17T19:30:00.000' AS DateTime), CAST(N'2020-03-17T10:33:00.000' AS DateTime))
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [DateTimeOut], [DateTimeIn]) VALUES (172, 4, CAST(N'2020-03-17T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T17:03:11.000' AS DateTime), NULL, 1, N'', CAST(N'2020-03-17T19:30:00.000' AS DateTime), CAST(N'2020-03-17T10:29:00.000' AS DateTime))
SET IDENTITY_INSERT [dbo].[Attendance] OFF
SET IDENTITY_INSERT [dbo].[AttendanceDetail] ON 

INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, 34, NULL, CAST(N'2020-01-03T10:12:00.000' AS DateTime), CAST(N'2020-01-03T20:48:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:23:42.547' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, 35, NULL, CAST(N'2020-01-03T10:05:00.000' AS DateTime), CAST(N'2020-01-03T22:10:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:25:13.083' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (3, 36, NULL, CAST(N'2020-01-03T14:00:00.000' AS DateTime), CAST(N'2020-01-03T22:30:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:26:27.213' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (4, 37, NULL, CAST(N'2020-01-04T10:05:00.000' AS DateTime), CAST(N'2020-01-04T19:35:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:27:42.720' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (5, 38, NULL, CAST(N'2020-01-04T10:10:00.000' AS DateTime), CAST(N'2020-01-04T19:21:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:27:58.693' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (6, 39, NULL, CAST(N'2020-01-04T00:00:00.000' AS DateTime), CAST(N'2020-01-04T00:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:28:29.870' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (7, 40, NULL, CAST(N'2020-01-06T10:19:00.000' AS DateTime), CAST(N'2020-01-06T19:20:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:31:54.347' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (8, 41, NULL, CAST(N'2020-01-06T10:19:00.000' AS DateTime), CAST(N'2020-01-06T19:20:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:32:05.920' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (9, 42, NULL, CAST(N'2020-01-06T11:30:00.000' AS DateTime), CAST(N'2020-01-06T19:45:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:32:39.993' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (10, 43, NULL, CAST(N'2020-01-07T10:11:00.000' AS DateTime), CAST(N'2020-01-07T19:20:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:33:19.217' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (11, 44, NULL, CAST(N'2020-01-07T10:26:00.000' AS DateTime), CAST(N'2020-01-07T19:20:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:33:30.320' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (12, 45, NULL, CAST(N'2020-01-07T13:55:00.000' AS DateTime), CAST(N'2020-01-07T21:05:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:33:54.917' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (13, 46, NULL, CAST(N'2020-01-08T10:05:00.000' AS DateTime), CAST(N'2020-01-08T22:46:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:34:40.613' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (14, 47, NULL, CAST(N'2020-01-08T11:20:00.000' AS DateTime), CAST(N'2020-01-08T22:30:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:35:03.007' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (15, 48, NULL, CAST(N'2020-01-08T11:20:00.000' AS DateTime), CAST(N'2020-01-08T22:30:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:35:09.953' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (16, 49, NULL, CAST(N'2020-01-09T10:15:00.000' AS DateTime), CAST(N'2020-01-09T21:15:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:35:41.283' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (17, 50, NULL, CAST(N'2020-01-09T10:40:00.000' AS DateTime), CAST(N'2020-01-09T20:43:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:36:00.657' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (18, 51, NULL, CAST(N'2020-01-09T14:55:00.000' AS DateTime), CAST(N'2020-01-09T22:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:36:25.997' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (19, 52, NULL, CAST(N'2020-01-10T10:13:00.000' AS DateTime), CAST(N'2020-01-10T21:28:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:37:04.120' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (20, 53, NULL, CAST(N'2020-01-10T10:17:00.000' AS DateTime), CAST(N'2020-01-10T21:13:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:37:23.663' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (21, 54, NULL, CAST(N'2020-01-10T00:00:00.000' AS DateTime), CAST(N'2020-01-10T23:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:37:44.147' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (22, 55, NULL, CAST(N'2020-01-11T10:05:00.000' AS DateTime), CAST(N'2020-01-11T20:48:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:38:22.137' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (23, 56, NULL, CAST(N'2020-01-11T15:20:00.000' AS DateTime), CAST(N'2020-01-11T21:15:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:38:48.693' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (24, 57, NULL, CAST(N'2020-01-13T10:00:00.000' AS DateTime), CAST(N'2020-01-13T20:20:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:39:14.963' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (25, 58, NULL, CAST(N'2020-01-13T10:50:00.000' AS DateTime), CAST(N'2020-01-13T20:20:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:39:30.057' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (26, 59, NULL, CAST(N'2020-01-14T10:15:00.000' AS DateTime), CAST(N'2020-01-14T22:15:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:39:55.187' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (27, 60, NULL, CAST(N'2020-01-14T10:25:00.000' AS DateTime), CAST(N'2020-01-14T20:15:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:40:12.010' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (28, 61, NULL, CAST(N'2020-01-14T10:30:00.000' AS DateTime), CAST(N'2020-01-14T20:15:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:40:26.483' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (29, 62, NULL, CAST(N'2020-01-15T10:05:00.000' AS DateTime), CAST(N'2020-01-15T19:40:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:41:50.303' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (30, 63, NULL, CAST(N'2020-01-15T10:35:00.000' AS DateTime), CAST(N'2020-01-15T19:22:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:42:08.340' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (31, 64, NULL, CAST(N'2020-01-15T13:00:00.000' AS DateTime), CAST(N'2020-01-15T22:15:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:42:26.553' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (32, 65, NULL, CAST(N'2020-01-16T09:15:00.000' AS DateTime), CAST(N'2020-01-16T19:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:45:23.513' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (33, 66, NULL, CAST(N'2020-01-16T09:20:00.000' AS DateTime), CAST(N'2020-01-16T19:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:46:04.917' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (34, 67, NULL, CAST(N'2020-01-16T11:15:00.000' AS DateTime), CAST(N'2020-01-16T20:26:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:46:27.827' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (35, 68, NULL, CAST(N'2020-01-17T09:18:00.000' AS DateTime), CAST(N'2020-01-17T20:12:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:47:03.433' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (36, 69, NULL, CAST(N'2020-01-17T10:55:00.000' AS DateTime), CAST(N'2020-01-17T20:35:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:48:57.553' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (37, 70, NULL, CAST(N'2020-01-20T09:10:00.000' AS DateTime), CAST(N'2020-01-20T19:20:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:49:54.220' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (38, 71, NULL, CAST(N'2020-01-20T09:44:00.000' AS DateTime), CAST(N'2020-01-20T19:20:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:50:10.977' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (39, 72, NULL, CAST(N'2020-01-20T00:00:00.000' AS DateTime), CAST(N'2020-01-20T19:20:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:50:27.537' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (40, 73, NULL, CAST(N'2020-01-21T09:20:00.000' AS DateTime), CAST(N'2020-01-21T18:30:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:51:43.047' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (41, 74, NULL, CAST(N'2020-01-21T09:45:00.000' AS DateTime), CAST(N'2020-01-21T18:30:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:52:02.523' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (42, 75, NULL, CAST(N'2020-01-21T11:10:00.000' AS DateTime), CAST(N'2020-01-21T18:30:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:52:26.240' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (43, 76, NULL, CAST(N'2020-01-22T09:26:00.000' AS DateTime), CAST(N'2020-01-22T19:50:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:53:01.603' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (44, 77, NULL, CAST(N'2020-01-22T09:42:00.000' AS DateTime), CAST(N'2020-01-22T19:50:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:53:11.137' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (45, 78, NULL, CAST(N'2020-01-22T09:50:00.000' AS DateTime), CAST(N'2020-01-22T19:50:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:53:20.450' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (46, 79, NULL, CAST(N'2020-01-23T09:20:00.000' AS DateTime), CAST(N'2020-01-23T19:30:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:53:44.903' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (47, 80, NULL, CAST(N'2020-01-24T09:20:00.000' AS DateTime), CAST(N'2020-01-24T20:30:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:54:10.573' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (48, 81, NULL, CAST(N'2020-01-24T09:15:00.000' AS DateTime), CAST(N'2020-01-24T20:30:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:54:23.677' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (49, 82, NULL, CAST(N'2020-01-24T09:50:00.000' AS DateTime), CAST(N'2020-01-24T20:30:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:54:34.977' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (50, 83, NULL, CAST(N'2020-01-27T09:15:00.000' AS DateTime), CAST(N'2020-01-27T21:30:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:55:11.690' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (51, 84, NULL, CAST(N'2020-01-27T09:30:00.000' AS DateTime), CAST(N'2020-01-27T21:30:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:55:22.647' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (52, 85, NULL, CAST(N'2020-01-27T09:55:00.000' AS DateTime), CAST(N'2020-01-27T21:30:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:55:29.387' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (53, 86, NULL, CAST(N'2020-01-28T09:34:00.000' AS DateTime), CAST(N'2020-01-28T20:10:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:56:20.877' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (54, 87, NULL, CAST(N'2020-01-28T09:10:00.000' AS DateTime), CAST(N'2020-01-28T20:50:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:56:31.083' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (55, 88, NULL, CAST(N'2020-01-28T14:20:00.000' AS DateTime), CAST(N'2020-01-28T20:50:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:56:45.480' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (56, 89, NULL, CAST(N'2020-01-29T09:18:00.000' AS DateTime), CAST(N'2020-01-29T19:10:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:57:25.270' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (57, 90, NULL, CAST(N'2020-01-29T10:40:00.000' AS DateTime), CAST(N'2020-01-29T19:10:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:57:44.377' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (58, 90, NULL, CAST(N'2020-01-29T10:50:00.000' AS DateTime), CAST(N'2020-01-29T19:10:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:57:50.483' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (59, 91, NULL, CAST(N'2020-01-30T10:15:00.000' AS DateTime), CAST(N'2020-01-30T20:20:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:58:23.607' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (60, 92, NULL, CAST(N'2020-01-30T10:05:00.000' AS DateTime), CAST(N'2020-01-30T19:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:58:55.870' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (61, 93, NULL, CAST(N'2020-01-30T11:40:00.000' AS DateTime), CAST(N'2020-01-30T20:35:00.000' AS DateTime), 1, CAST(N'2020-03-18T15:59:19.943' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (62, 94, NULL, CAST(N'2020-01-31T10:00:00.000' AS DateTime), CAST(N'2020-01-31T20:05:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:00:12.427' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (63, 95, NULL, CAST(N'2020-01-31T10:12:00.000' AS DateTime), CAST(N'2020-01-31T19:40:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:00:32.297' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (64, 96, NULL, CAST(N'2020-01-31T10:12:00.000' AS DateTime), CAST(N'2020-01-31T19:40:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:00:44.643' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (65, 97, NULL, CAST(N'2020-02-03T10:00:00.000' AS DateTime), CAST(N'2020-02-03T20:20:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:01:27.483' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (66, 98, NULL, CAST(N'2020-02-03T10:15:00.000' AS DateTime), CAST(N'2020-02-03T21:22:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:01:43.113' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (67, 99, NULL, CAST(N'2020-02-03T11:40:00.000' AS DateTime), CAST(N'2020-02-03T20:40:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:02:32.440' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (68, 100, NULL, CAST(N'2020-02-04T10:00:00.000' AS DateTime), CAST(N'2020-02-04T19:05:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:04:28.963' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (69, 101, NULL, CAST(N'2020-02-04T10:20:00.000' AS DateTime), CAST(N'2020-02-04T19:05:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:04:39.850' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (70, 102, NULL, CAST(N'2020-02-05T10:00:00.000' AS DateTime), CAST(N'2020-02-05T21:36:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:05:57.760' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (71, 103, NULL, CAST(N'2020-02-05T11:40:00.000' AS DateTime), CAST(N'2020-02-05T21:36:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:06:14.093' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (72, 104, NULL, CAST(N'2020-02-05T13:30:00.000' AS DateTime), CAST(N'2020-02-05T21:36:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:06:26.757' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (73, 105, NULL, CAST(N'2020-02-06T10:00:00.000' AS DateTime), CAST(N'2020-02-06T19:45:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:06:55.170' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (74, 106, NULL, CAST(N'2020-02-07T10:00:00.000' AS DateTime), CAST(N'2020-02-07T19:46:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:07:59.803' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (75, 107, NULL, CAST(N'2020-02-07T11:10:00.000' AS DateTime), CAST(N'2020-02-07T21:15:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:08:36.053' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (76, 108, NULL, CAST(N'2020-02-10T10:00:00.000' AS DateTime), CAST(N'2020-02-10T19:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:09:18.287' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (77, 109, NULL, CAST(N'2020-02-10T10:35:00.000' AS DateTime), CAST(N'2020-02-10T19:20:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:09:31.160' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (78, 110, NULL, CAST(N'2020-02-11T10:00:00.000' AS DateTime), CAST(N'2020-02-11T21:50:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:09:59.390' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (79, 111, NULL, CAST(N'2020-02-11T14:35:00.000' AS DateTime), CAST(N'2020-02-11T23:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:10:16.087' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (80, 112, NULL, CAST(N'2020-02-12T10:10:00.000' AS DateTime), CAST(N'2020-02-12T19:15:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:11:19.260' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (81, 113, NULL, CAST(N'2020-02-12T13:10:00.000' AS DateTime), CAST(N'2020-02-12T19:15:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:11:31.660' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (82, 114, NULL, CAST(N'2020-02-13T09:45:00.000' AS DateTime), CAST(N'2020-02-13T19:05:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:11:57.253' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (83, 114, NULL, CAST(N'2020-02-13T10:10:00.000' AS DateTime), CAST(N'2020-02-13T19:15:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:12:12.253' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (84, 115, NULL, CAST(N'2020-02-14T09:58:00.000' AS DateTime), CAST(N'2020-02-14T19:30:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:13:40.713' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (85, 116, NULL, CAST(N'2020-02-14T10:10:00.000' AS DateTime), CAST(N'2020-02-14T20:05:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:14:13.603' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (86, 117, NULL, CAST(N'2020-02-17T10:15:00.000' AS DateTime), CAST(N'2020-02-17T20:20:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:23:04.553' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (87, 118, NULL, CAST(N'2020-02-17T10:05:00.000' AS DateTime), CAST(N'2020-02-17T18:05:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:23:19.503' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (88, 119, NULL, CAST(N'2020-02-18T10:15:00.000' AS DateTime), CAST(N'2020-02-18T19:25:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:24:55.483' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (89, 120, NULL, CAST(N'2020-02-19T09:55:00.000' AS DateTime), CAST(N'2020-02-19T19:37:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:25:33.180' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (90, 121, NULL, CAST(N'2020-02-19T10:15:00.000' AS DateTime), CAST(N'2020-02-19T21:05:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:25:50.093' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (91, 122, NULL, CAST(N'2020-02-20T10:10:00.000' AS DateTime), CAST(N'2020-02-20T19:45:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:26:57.480' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (92, 123, NULL, CAST(N'2020-02-21T10:05:00.000' AS DateTime), CAST(N'2020-02-21T19:40:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:31:52.540' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (93, 124, NULL, CAST(N'2020-02-21T10:15:00.000' AS DateTime), CAST(N'2020-02-21T19:40:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:32:11.380' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (94, 125, NULL, CAST(N'2020-02-21T10:20:00.000' AS DateTime), CAST(N'2020-02-21T19:40:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:32:19.097' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (95, 126, NULL, CAST(N'2020-02-24T10:00:00.000' AS DateTime), CAST(N'2020-02-24T19:50:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:32:57.923' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (96, 127, NULL, CAST(N'2020-02-24T10:00:00.000' AS DateTime), CAST(N'2020-02-24T19:25:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:33:09.993' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (97, 128, NULL, CAST(N'2020-02-24T10:00:00.000' AS DateTime), CAST(N'2020-02-24T20:13:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:33:20.247' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (98, 129, NULL, CAST(N'2020-02-25T10:00:00.000' AS DateTime), CAST(N'2020-02-25T20:20:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:33:38.853' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (99, 130, NULL, CAST(N'2020-02-25T10:15:00.000' AS DateTime), CAST(N'2020-02-25T20:30:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:33:51.843' AS DateTime), NULL, 1, NULL)
GO
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (100, 131, NULL, CAST(N'2020-02-26T10:10:00.000' AS DateTime), CAST(N'2020-02-26T19:55:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:34:26.867' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (101, 132, NULL, CAST(N'2020-02-26T10:10:00.000' AS DateTime), CAST(N'2020-02-26T20:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:34:37.510' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (102, 133, NULL, CAST(N'2020-02-26T10:23:00.000' AS DateTime), CAST(N'2020-02-26T19:30:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:34:53.507' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (103, 134, NULL, CAST(N'2020-02-27T10:03:00.000' AS DateTime), CAST(N'2020-02-27T19:07:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:35:17.060' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (104, 135, NULL, CAST(N'2020-02-27T10:07:00.000' AS DateTime), CAST(N'2020-02-27T19:23:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:35:29.360' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (105, 136, NULL, CAST(N'2020-02-27T14:33:00.000' AS DateTime), CAST(N'2020-02-27T19:23:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:35:48.473' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (106, 137, NULL, CAST(N'2020-02-28T10:05:00.000' AS DateTime), CAST(N'2020-02-28T20:15:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:36:16.420' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (107, 138, NULL, CAST(N'2020-02-28T10:10:00.000' AS DateTime), CAST(N'2020-02-28T20:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:36:53.467' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (108, 139, NULL, CAST(N'2020-02-28T11:29:00.000' AS DateTime), CAST(N'2020-02-28T20:40:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:37:09.390' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (109, 140, NULL, CAST(N'2020-02-29T09:45:00.000' AS DateTime), CAST(N'2020-02-29T16:00:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:37:40.477' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (110, 141, NULL, CAST(N'2020-02-29T10:40:00.000' AS DateTime), CAST(N'2020-02-29T19:20:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:38:42.603' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (111, 142, NULL, CAST(N'2020-02-29T15:05:00.000' AS DateTime), CAST(N'2020-02-29T22:45:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:39:27.067' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (112, 142, NULL, CAST(N'2020-02-29T15:05:00.000' AS DateTime), CAST(N'2020-02-29T22:45:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:40:32.970' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (113, 142, NULL, CAST(N'2020-02-29T15:05:00.000' AS DateTime), CAST(N'2020-02-29T22:45:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:43:55.543' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (114, 143, NULL, CAST(N'2020-03-02T09:50:00.000' AS DateTime), CAST(N'2020-03-02T19:20:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:45:06.230' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (115, 144, NULL, CAST(N'2020-03-02T10:05:00.000' AS DateTime), CAST(N'2020-03-02T15:55:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:46:54.853' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (116, 145, NULL, CAST(N'2020-03-03T10:00:00.000' AS DateTime), CAST(N'2020-03-03T19:20:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:47:28.827' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (117, 146, NULL, CAST(N'2020-03-03T10:25:00.000' AS DateTime), CAST(N'2020-03-03T19:20:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:48:39.017' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (118, 147, NULL, CAST(N'2020-03-04T09:55:00.000' AS DateTime), CAST(N'2020-03-04T18:25:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:49:12.890' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (119, 148, NULL, CAST(N'2020-03-04T10:22:00.000' AS DateTime), CAST(N'2020-03-04T18:53:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:49:30.820' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (120, 149, NULL, CAST(N'2020-03-04T12:45:00.000' AS DateTime), CAST(N'2020-03-04T20:30:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:49:50.523' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (121, 147, NULL, CAST(N'2020-03-04T10:10:00.000' AS DateTime), CAST(N'2020-03-04T20:35:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:50:47.023' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (122, 148, NULL, CAST(N'2020-03-04T11:00:00.000' AS DateTime), CAST(N'2020-03-04T21:15:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:51:19.280' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (123, 149, NULL, CAST(N'2020-03-04T15:00:00.000' AS DateTime), CAST(N'2020-03-04T21:30:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:51:38.707' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (124, 150, NULL, CAST(N'2020-03-06T09:50:00.000' AS DateTime), CAST(N'2020-03-06T20:05:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:53:06.973' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (125, 151, NULL, CAST(N'2020-03-06T10:15:00.000' AS DateTime), CAST(N'2020-03-06T19:30:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:53:23.027' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (126, 152, NULL, CAST(N'2020-03-09T09:55:00.000' AS DateTime), CAST(N'2020-03-09T20:05:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:54:16.097' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (127, 153, NULL, CAST(N'2020-03-09T13:10:00.000' AS DateTime), CAST(N'2020-03-09T21:23:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:54:36.903' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (128, 154, NULL, CAST(N'2020-03-10T10:05:00.000' AS DateTime), CAST(N'2020-03-10T19:03:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:55:12.480' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (129, 155, NULL, CAST(N'2020-03-10T10:00:00.000' AS DateTime), CAST(N'2020-03-10T19:03:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:55:32.843' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (130, 156, NULL, CAST(N'2020-03-11T10:00:00.000' AS DateTime), CAST(N'2020-03-11T19:31:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:56:01.087' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (131, 157, NULL, CAST(N'2020-03-11T10:20:00.000' AS DateTime), CAST(N'2020-03-11T19:31:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:56:11.080' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (132, 158, NULL, CAST(N'2020-03-12T10:10:00.000' AS DateTime), CAST(N'2020-03-12T19:05:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:57:25.620' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (133, 159, NULL, CAST(N'2020-03-12T10:10:00.000' AS DateTime), CAST(N'2020-03-12T19:05:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:57:37.910' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (134, 160, NULL, CAST(N'2020-03-12T12:01:00.000' AS DateTime), CAST(N'2020-03-12T20:40:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:58:01.863' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (135, 161, NULL, CAST(N'2020-03-13T10:05:00.000' AS DateTime), CAST(N'2020-03-13T22:05:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:58:33.227' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (136, 162, NULL, CAST(N'2020-03-13T10:10:00.000' AS DateTime), CAST(N'2020-03-13T22:10:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:58:45.443' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (137, 163, NULL, CAST(N'2020-03-13T10:50:00.000' AS DateTime), CAST(N'2020-03-13T22:10:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:59:00.833' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (138, 164, NULL, CAST(N'2020-03-14T10:50:00.000' AS DateTime), CAST(N'2020-03-14T17:27:00.000' AS DateTime), 1, CAST(N'2020-03-18T16:59:38.347' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (139, 165, NULL, CAST(N'2020-03-14T14:25:00.000' AS DateTime), CAST(N'2020-03-14T18:15:00.000' AS DateTime), 1, CAST(N'2020-03-18T17:00:00.237' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (140, 166, NULL, CAST(N'2020-03-15T10:00:00.000' AS DateTime), CAST(N'2020-03-15T19:37:00.000' AS DateTime), 1, CAST(N'2020-03-18T17:00:45.030' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (141, 167, NULL, CAST(N'2020-03-15T10:05:00.000' AS DateTime), CAST(N'2020-03-15T19:37:00.000' AS DateTime), 1, CAST(N'2020-03-18T17:00:52.730' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (142, 168, NULL, CAST(N'2020-03-16T10:05:00.000' AS DateTime), CAST(N'2020-03-16T19:37:00.000' AS DateTime), 1, CAST(N'2020-03-18T17:02:08.920' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (143, 169, NULL, CAST(N'2020-03-16T10:05:00.000' AS DateTime), CAST(N'2020-03-16T19:37:00.000' AS DateTime), 1, CAST(N'2020-03-18T17:02:12.103' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (144, 170, NULL, CAST(N'2020-03-17T10:13:00.000' AS DateTime), CAST(N'2020-03-17T19:30:00.000' AS DateTime), 1, CAST(N'2020-03-18T17:02:47.043' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (145, 171, NULL, CAST(N'2020-03-17T10:33:00.000' AS DateTime), CAST(N'2020-03-17T19:30:00.000' AS DateTime), 1, CAST(N'2020-03-18T17:02:56.607' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (146, 172, NULL, CAST(N'2020-03-17T10:29:00.000' AS DateTime), CAST(N'2020-03-17T19:30:00.000' AS DateTime), 1, CAST(N'2020-03-18T17:03:11.030' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (147, 165, NULL, CAST(N'2020-03-14T14:25:00.000' AS DateTime), CAST(N'2020-03-14T14:25:00.000' AS DateTime), 1, CAST(N'2020-03-18T18:54:44.007' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (148, 164, NULL, CAST(N'2020-03-14T10:15:00.000' AS DateTime), CAST(N'2020-03-14T10:15:00.000' AS DateTime), 1, CAST(N'2020-03-18T18:55:34.837' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (149, 164, NULL, CAST(N'2020-03-14T10:15:00.000' AS DateTime), CAST(N'2020-03-14T10:15:00.000' AS DateTime), 1, CAST(N'2020-03-18T18:57:20.870' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (150, 164, NULL, CAST(N'2020-03-14T10:15:00.000' AS DateTime), CAST(N'2020-03-14T10:15:00.000' AS DateTime), 1, CAST(N'2020-03-18T19:02:41.170' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (151, 164, NULL, CAST(N'2020-03-14T10:15:00.000' AS DateTime), CAST(N'2020-03-14T10:15:00.000' AS DateTime), 1, CAST(N'2020-03-18T19:08:02.400' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (152, 164, NULL, CAST(N'2020-03-14T10:15:00.000' AS DateTime), CAST(N'2020-03-14T10:15:00.000' AS DateTime), 1, CAST(N'2020-03-18T19:09:58.627' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (153, 164, NULL, CAST(N'2020-03-14T10:15:00.000' AS DateTime), CAST(N'2020-03-14T10:15:00.000' AS DateTime), 1, CAST(N'2020-03-18T19:13:10.643' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (154, 164, NULL, CAST(N'2020-03-14T10:15:00.000' AS DateTime), CAST(N'2020-03-14T10:15:00.000' AS DateTime), 1, CAST(N'2020-03-18T19:13:39.840' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (155, 164, NULL, CAST(N'2020-03-14T10:15:00.000' AS DateTime), CAST(N'2020-03-14T10:15:00.000' AS DateTime), 1, CAST(N'2020-03-18T19:14:34.960' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (156, 164, NULL, CAST(N'2020-03-14T10:15:00.000' AS DateTime), CAST(N'2020-03-14T10:15:00.000' AS DateTime), 1, CAST(N'2020-03-18T19:15:03.940' AS DateTime), NULL, 1, NULL)
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (157, 164, NULL, CAST(N'2020-03-14T10:15:00.000' AS DateTime), CAST(N'2020-03-14T10:15:00.000' AS DateTime), 1, CAST(N'2020-03-18T19:16:52.083' AS DateTime), NULL, 1, NULL)
SET IDENTITY_INSERT [dbo].[AttendanceDetail] OFF
SET IDENTITY_INSERT [dbo].[AttendancePolicy] ON 

INSERT [dbo].[AttendancePolicy] ([ID], [ShiftID], [AttendanceVariableID], [Hours], [Description], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, 2, 7, CAST(0.50 AS Decimal(18, 2)), N'consider left early, if left half hour before shift end time', CAST(N'2020-02-01T00:00:00.000' AS DateTime), NULL, CAST(N'2020-03-18T11:09:50.333' AS DateTime), CAST(N'2020-03-18T12:52:50.000' AS DateTime), 1, N'::1')
INSERT [dbo].[AttendancePolicy] ([ID], [ShiftID], [AttendanceVariableID], [Hours], [Description], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, 2, 8, CAST(0.50 AS Decimal(18, 2)), N'consider came late, if came half hour after shift start time', CAST(N'2020-02-01T00:00:00.000' AS DateTime), NULL, CAST(N'2020-03-18T11:09:50.407' AS DateTime), CAST(N'2020-03-18T12:52:50.000' AS DateTime), 1, N'::1')
INSERT [dbo].[AttendancePolicy] ([ID], [ShiftID], [AttendanceVariableID], [Hours], [Description], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (3, 3, 7, CAST(0.50 AS Decimal(18, 2)), NULL, CAST(N'2020-02-01T00:00:00.000' AS DateTime), NULL, CAST(N'2020-03-18T11:10:08.023' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[AttendancePolicy] ([ID], [ShiftID], [AttendanceVariableID], [Hours], [Description], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (4, 3, 8, CAST(0.50 AS Decimal(18, 2)), NULL, CAST(N'2020-02-01T00:00:00.000' AS DateTime), NULL, CAST(N'2020-03-18T11:10:08.060' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[AttendancePolicy] ([ID], [ShiftID], [AttendanceVariableID], [Hours], [Description], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (5, 4, 1, CAST(4.00 AS Decimal(18, 2)), NULL, CAST(N'2020-02-01T00:00:00.000' AS DateTime), NULL, CAST(N'2020-03-18T11:10:33.647' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[AttendancePolicy] ([ID], [ShiftID], [AttendanceVariableID], [Hours], [Description], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (6, 4, 2, CAST(2.00 AS Decimal(18, 2)), NULL, CAST(N'2020-02-01T00:00:00.000' AS DateTime), NULL, CAST(N'2020-03-18T11:10:33.683' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[AttendancePolicy] ([ID], [ShiftID], [AttendanceVariableID], [Hours], [Description], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (7, 4, 7, CAST(1.00 AS Decimal(18, 2)), NULL, CAST(N'2020-02-01T00:00:00.000' AS DateTime), NULL, CAST(N'2020-03-18T11:10:33.740' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[AttendancePolicy] ([ID], [ShiftID], [AttendanceVariableID], [Hours], [Description], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (8, 4, 8, CAST(1.00 AS Decimal(18, 2)), NULL, CAST(N'2020-02-01T00:00:00.000' AS DateTime), NULL, CAST(N'2020-03-18T11:10:33.827' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[AttendancePolicy] ([ID], [ShiftID], [AttendanceVariableID], [Hours], [Description], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (9, 5, 7, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(N'2020-02-01T00:00:00.000' AS DateTime), NULL, CAST(N'2020-03-18T11:10:48.067' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[AttendancePolicy] ([ID], [ShiftID], [AttendanceVariableID], [Hours], [Description], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (10, 5, 8, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(N'2020-02-01T00:00:00.000' AS DateTime), NULL, CAST(N'2020-03-18T11:10:48.083' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[AttendancePolicy] ([ID], [ShiftID], [AttendanceVariableID], [Hours], [Description], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (11, 6, 7, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(N'2020-02-01T00:00:00.000' AS DateTime), NULL, CAST(N'2020-03-18T11:11:03.963' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[AttendancePolicy] ([ID], [ShiftID], [AttendanceVariableID], [Hours], [Description], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (12, 6, 8, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(N'2020-02-01T00:00:00.000' AS DateTime), NULL, CAST(N'2020-03-18T11:11:03.977' AS DateTime), NULL, 1, N'::1')
SET IDENTITY_INSERT [dbo].[AttendancePolicy] OFF
SET IDENTITY_INSERT [dbo].[AttendanceStatus] ON 

INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (1, 34, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:23:42.623' AS DateTime), NULL, 1, N'', 0, 0, 12, 0, 636, 636, 108, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (2, 35, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:25:13.163' AS DateTime), NULL, 1, N'', 0, 0, 5, 0, 725, 725, 190, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (3, 36, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:26:27.260' AS DateTime), NULL, 1, N'', 1, 0, 240, 0, 510, 510, 210, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (4, 37, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:27:42.760' AS DateTime), NULL, 1, N'', 0, 0, 5, 0, 570, 570, 35, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (5, 38, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:27:58.747' AS DateTime), NULL, 1, N'', 0, 0, 10, 0, 551, 551, 21, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (7, 40, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:31:54.427' AS DateTime), NULL, 1, N'', 0, 0, 19, 0, 541, 541, 20, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (8, 41, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:32:05.950' AS DateTime), NULL, 1, N'', 0, 0, 19, 0, 541, 541, 20, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (9, 42, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:32:40.047' AS DateTime), NULL, 1, N'', 1, 0, 90, 0, 495, 495, 45, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (10, 43, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:33:19.267' AS DateTime), NULL, 1, N'', 0, 0, 11, 0, 549, 549, 20, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (11, 44, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:33:30.353' AS DateTime), NULL, 1, N'', 0, 0, 26, 0, 534, 534, 20, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (12, 45, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:33:54.963' AS DateTime), NULL, 1, N'', 1, 0, 235, 0, 430, 430, 125, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (13, 46, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:34:40.667' AS DateTime), NULL, 1, N'', 0, 0, 5, 0, 761, 761, 226, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (14, 47, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:35:03.053' AS DateTime), NULL, 1, N'', 1, 0, 80, 0, 670, 670, 210, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (15, 48, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:35:09.993' AS DateTime), NULL, 1, N'', 1, 0, 80, 0, 670, 670, 210, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (16, 49, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:35:41.317' AS DateTime), NULL, 1, N'', 0, 0, 15, 0, 660, 660, 135, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (17, 50, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:36:00.697' AS DateTime), NULL, 1, N'', 1, 0, 40, 0, 603, 603, 103, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (18, 51, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:36:26.040' AS DateTime), NULL, 1, N'', 1, 0, 295, 0, 425, 425, 180, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (19, 52, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:37:04.193' AS DateTime), NULL, 1, N'', 0, 0, 13, 0, 675, 675, 148, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (20, 53, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:37:23.713' AS DateTime), NULL, 1, N'', 0, 0, 17, 0, 656, 656, 133, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (21, 54, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:37:44.190' AS DateTime), NULL, 1, N'', 0, 0, 0, 0, 1380, 1380, 240, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (22, 55, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:38:22.193' AS DateTime), NULL, 1, N'', 0, 0, 5, 0, 643, 643, 108, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (23, 56, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:38:48.740' AS DateTime), NULL, 1, N'', 1, 0, 320, 0, 355, 355, 135, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (24, 57, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:39:15.010' AS DateTime), NULL, 1, N'', 0, 0, 0, 0, 620, 620, 80, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (25, 58, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:39:30.093' AS DateTime), NULL, 1, N'', 1, 0, 50, 0, 570, 570, 80, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (26, 59, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:39:55.257' AS DateTime), NULL, 1, N'', 0, 0, 15, 0, 720, 720, 195, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (27, 60, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:40:12.043' AS DateTime), NULL, 1, N'', 0, 0, 25, 0, 590, 590, 75, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (28, 61, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:40:26.537' AS DateTime), NULL, 1, N'', 1, 0, 30, 0, 585, 585, 75, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (29, 62, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:41:50.347' AS DateTime), NULL, 1, N'', 0, 0, 5, 0, 575, 575, 40, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (30, 63, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:42:08.380' AS DateTime), NULL, 1, N'', 1, 0, 35, 0, 527, 527, 22, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (31, 64, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:42:26.607' AS DateTime), NULL, 1, N'', 1, 0, 180, 0, 555, 555, 195, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (32, 65, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:45:23.563' AS DateTime), NULL, 1, N'', 1, 1, 15, 0, 585, 585, 60, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (33, 66, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:46:04.953' AS DateTime), NULL, 1, N'', 1, 1, 20, 0, 580, 580, 60, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (34, 67, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:46:27.883' AS DateTime), NULL, 1, N'', 1, 1, 135, 0, 551, 551, 146, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (35, 68, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:47:03.503' AS DateTime), NULL, 1, N'', 1, 1, 18, 0, 654, 654, 132, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (36, 69, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:48:57.587' AS DateTime), NULL, 1, N'', 1, 1, 115, 0, 580, 580, 155, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (37, 70, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:49:54.310' AS DateTime), NULL, 1, N'', 1, 1, 10, 0, 610, 610, 80, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (38, 71, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:50:11.027' AS DateTime), NULL, 1, N'', 1, 1, 44, 0, 576, 576, 80, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (39, 72, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:50:27.583' AS DateTime), NULL, 1, N'', 1, 1, 0, 0, 1160, 1160, 80, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (40, 73, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:51:43.133' AS DateTime), NULL, 1, N'', 1, 1, 20, 0, 550, 550, 30, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (41, 74, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:52:02.777' AS DateTime), NULL, 1, N'', 1, 1, 45, 0, 525, 525, 30, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (42, 75, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:52:26.277' AS DateTime), NULL, 1, N'', 1, 1, 130, 0, 440, 440, 30, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (43, 76, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:53:01.640' AS DateTime), NULL, 1, N'', 1, 1, 26, 0, 624, 624, 110, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (44, 77, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:53:11.190' AS DateTime), NULL, 1, N'', 1, 1, 42, 0, 608, 608, 110, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (45, 78, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:53:20.487' AS DateTime), NULL, 1, N'', 1, 1, 50, 0, 600, 600, 110, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (46, 79, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:53:44.987' AS DateTime), NULL, 1, N'', 1, 1, 20, 0, 610, 610, 90, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (47, 80, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:54:10.607' AS DateTime), NULL, 1, N'', 1, 1, 20, 0, 670, 670, 150, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (48, 81, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:54:23.757' AS DateTime), NULL, 1, N'', 1, 1, 15, 0, 675, 675, 150, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (49, 82, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:54:35.010' AS DateTime), NULL, 1, N'', 1, 1, 50, 0, 640, 640, 150, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (50, 83, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:55:11.730' AS DateTime), NULL, 1, N'', 1, 1, 15, 0, 735, 735, 210, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (51, 84, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:55:22.717' AS DateTime), NULL, 1, N'', 1, 1, 30, 0, 720, 720, 210, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (52, 85, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:55:29.430' AS DateTime), NULL, 1, N'', 1, 1, 55, 0, 695, 695, 210, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (53, 86, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:56:20.923' AS DateTime), NULL, 1, N'', 1, 1, 34, 0, 636, 636, 130, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (54, 87, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:56:31.117' AS DateTime), NULL, 1, N'', 1, 1, 10, 0, 700, 700, 170, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (55, 88, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:56:45.530' AS DateTime), NULL, 1, N'', 1, 1, 320, 0, 390, 390, 170, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (56, 89, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:57:25.303' AS DateTime), NULL, 1, N'', 1, 1, 18, 0, 592, 592, 70, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (57, 90, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:57:44.413' AS DateTime), NULL, 1, N'', 1, 1, 100, 0, 1010, 510, 70, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (58, 91, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:58:23.673' AS DateTime), NULL, 1, N'', 0, 0, 15, 0, 605, 605, 80, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (59, 92, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:58:55.937' AS DateTime), NULL, 1, N'', 0, 0, 5, 0, 535, 535, 0, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (60, 93, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T15:59:19.980' AS DateTime), NULL, 1, N'', 1, 0, 100, 0, 535, 535, 95, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (61, 94, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:00:12.477' AS DateTime), NULL, 1, N'', 0, 0, 0, 0, 605, 605, 65, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (62, 95, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:00:32.347' AS DateTime), NULL, 1, N'', 0, 0, 12, 0, 568, 568, 40, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (63, 96, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:00:44.683' AS DateTime), NULL, 1, N'', 0, 0, 12, 0, 568, 568, 40, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (64, 97, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:01:27.517' AS DateTime), NULL, 1, N'', 0, 0, 0, 0, 620, 620, 80, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (65, 98, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:01:43.160' AS DateTime), NULL, 1, N'', 0, 0, 15, 0, 667, 667, 142, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (66, 99, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:02:32.483' AS DateTime), NULL, 1, N'', 0, 0, 100, 0, 540, 540, 100, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (67, 100, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:04:29.027' AS DateTime), NULL, 1, N'', 0, 0, 0, 0, 545, 545, 5, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (68, 101, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:04:39.880' AS DateTime), NULL, 1, N'', 0, 0, 20, 0, 525, 525, 5, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (69, 102, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:05:57.793' AS DateTime), NULL, 1, N'', 0, 0, 0, 0, 696, 696, 156, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (70, 103, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:06:14.133' AS DateTime), NULL, 1, N'', 0, 0, 100, 0, 596, 596, 156, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (71, 104, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:06:26.800' AS DateTime), NULL, 1, N'', 0, 0, 210, 0, 486, 486, 156, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (72, 105, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:06:55.207' AS DateTime), NULL, 1, N'', 0, 0, 0, 0, 585, 585, 45, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (73, 106, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:07:59.883' AS DateTime), NULL, 1, N'', 0, 0, 0, 0, 586, 586, 46, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (74, 107, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:08:36.103' AS DateTime), NULL, 1, N'', 0, 0, 70, 0, 605, 605, 135, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (75, 108, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:09:18.333' AS DateTime), NULL, 1, N'', 0, 0, 0, 0, 540, 540, 0, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (76, 109, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:09:31.193' AS DateTime), NULL, 1, N'', 0, 0, 35, 0, 525, 525, 20, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (77, 110, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:09:59.470' AS DateTime), NULL, 1, N'', 0, 0, 0, 0, 710, 710, 170, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (78, 111, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:10:16.133' AS DateTime), NULL, 1, N'', 0, 0, 275, 0, 505, 505, 240, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (79, 112, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:11:19.303' AS DateTime), NULL, 1, N'', 0, 0, 10, 0, 545, 545, 15, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (80, 113, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:11:31.693' AS DateTime), NULL, 1, N'', 0, 0, 190, 0, 365, 365, 15, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (81, 114, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:11:57.287' AS DateTime), NULL, 1, N'', 0, 0, 45, 0, 1105, 570, 75, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (82, 115, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:13:40.790' AS DateTime), NULL, 1, N'', 0, 0, 58, 0, 572, 572, 90, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (83, 116, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:14:13.640' AS DateTime), NULL, 1, N'', 0, 0, 10, 0, 595, 595, 65, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (84, 117, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:23:04.837' AS DateTime), NULL, 1, N'', 0, 0, 15, 0, 605, 605, 80, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (85, 118, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:23:19.590' AS DateTime), NULL, 1, N'', 0, 0, 65, 0, 480, 480, 5, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (86, 119, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:24:55.540' AS DateTime), NULL, 1, N'', 0, 0, 15, 0, 550, 550, 25, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (87, 120, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:25:33.220' AS DateTime), NULL, 1, N'', 0, 0, 55, 0, 582, 582, 97, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (88, 121, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:25:50.143' AS DateTime), NULL, 1, N'', 0, 0, 15, 0, 650, 650, 125, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (89, 122, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:26:57.530' AS DateTime), NULL, 1, N'', 0, 0, 10, 0, 575, 575, 45, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (90, 123, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:31:52.583' AS DateTime), NULL, 1, N'', 0, 0, 5, 0, 575, 575, 40, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (91, 124, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:32:11.420' AS DateTime), NULL, 1, N'', 0, 0, 15, 0, 565, 565, 40, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (92, 125, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:32:19.133' AS DateTime), NULL, 1, N'', 0, 0, 80, 0, 560, 560, 100, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (93, 126, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:32:57.973' AS DateTime), NULL, 1, N'', 0, 0, 0, 0, 590, 590, 50, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (94, 127, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:33:10.040' AS DateTime), NULL, 1, N'', 0, 0, 60, 0, 565, 565, 85, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (95, 128, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:33:20.293' AS DateTime), NULL, 1, N'', 0, 0, 0, 0, 613, 613, 73, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (96, 129, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:33:38.903' AS DateTime), NULL, 1, N'', 0, 0, 0, 0, 620, 620, 80, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (97, 130, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:33:51.880' AS DateTime), NULL, 1, N'', 0, 0, 15, 0, 615, 615, 90, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (98, 131, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:34:26.923' AS DateTime), NULL, 1, N'', 0, 0, 10, 0, 585, 585, 55, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (99, 132, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:34:37.557' AS DateTime), NULL, 1, N'', 0, 0, 10, 0, 590, 590, 60, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (100, 133, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:34:53.600' AS DateTime), NULL, 1, N'', 0, 0, 83, 0, 547, 547, 90, NULL)
GO
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (101, 134, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:35:17.107' AS DateTime), NULL, 1, N'', 0, 0, 3, 0, 544, 544, 7, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (102, 135, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:35:29.393' AS DateTime), NULL, 1, N'', 0, 0, 7, 0, 556, 556, 23, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (103, 136, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:35:48.507' AS DateTime), NULL, 1, N'', 0, 0, 333, 0, 290, 290, 83, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (104, 137, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:36:16.503' AS DateTime), NULL, 1, N'', 0, 0, 5, 0, 610, 610, 75, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (105, 138, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:36:53.523' AS DateTime), NULL, 1, N'', 0, 0, 10, 0, 590, 590, 60, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (106, 139, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:37:09.423' AS DateTime), NULL, 1, N'', 0, 0, 149, 0, 551, 551, 160, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (107, 140, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:37:40.507' AS DateTime), NULL, 1, N'', 0, 0, 0, 180, 375, 375, 0, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (108, 141, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:38:42.650' AS DateTime), NULL, 1, N'', 0, 0, 40, 0, 520, 520, 20, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (110, 142, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:43:55.587' AS DateTime), NULL, 1, N'', 0, 0, 365, 0, 460, 460, 285, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (111, 143, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:45:06.280' AS DateTime), NULL, 1, N'', 0, 0, 0, 0, 570, 570, 20, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (112, 144, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:46:54.910' AS DateTime), NULL, 1, N'', 0, 0, 5, 185, 350, 350, 0, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (113, 145, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:47:28.887' AS DateTime), NULL, 1, N'', 0, 0, 0, 0, 560, 560, 20, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (114, 146, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:48:39.050' AS DateTime), NULL, 1, N'', 0, 0, 25, 0, 535, 535, 20, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (115, 147, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:49:12.940' AS DateTime), NULL, 1, N'', 0, 0, 10, 0, 1135, 640, 95, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (116, 148, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:49:30.887' AS DateTime), NULL, 1, N'', 0, 0, 22, 0, 1126, 653, 135, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (117, 149, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:49:50.570' AS DateTime), NULL, 1, N'', 0, 0, 225, 0, 855, 525, 210, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (118, 150, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:53:07.020' AS DateTime), NULL, 1, N'', 0, 0, 0, 0, 615, 615, 65, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (119, 151, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:53:23.077' AS DateTime), NULL, 1, N'', 0, 0, 15, 0, 555, 555, 30, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (120, 152, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:54:16.143' AS DateTime), NULL, 1, N'', 0, 0, 0, 0, 610, 610, 65, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (121, 153, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:54:36.947' AS DateTime), NULL, 1, N'', 0, 0, 250, 0, 493, 493, 203, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (122, 154, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:55:12.517' AS DateTime), NULL, 1, N'', 0, 0, 5, 0, 538, 538, 3, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (123, 155, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:55:32.907' AS DateTime), NULL, 1, N'', 0, 0, 0, 0, 543, 543, 3, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (124, 156, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:56:01.123' AS DateTime), NULL, 1, N'', 0, 0, 0, 0, 571, 571, 31, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (125, 157, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:56:11.117' AS DateTime), NULL, 1, N'', 0, 0, 20, 0, 551, 551, 31, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (126, 158, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:57:25.690' AS DateTime), NULL, 1, N'', 0, 0, 10, 0, 535, 535, 5, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (127, 159, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:57:38.070' AS DateTime), NULL, 1, N'', 0, 0, 10, 0, 535, 535, 5, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (128, 160, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:58:01.930' AS DateTime), NULL, 1, N'', 0, 0, 181, 0, 519, 519, 160, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (129, 161, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:58:33.270' AS DateTime), NULL, 1, N'', 0, 0, 5, 0, 720, 720, 185, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (130, 162, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:58:45.520' AS DateTime), NULL, 1, N'', 0, 0, 10, 0, 720, 720, 190, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (131, 163, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:59:00.870' AS DateTime), NULL, 1, N'', 0, 0, 110, 0, 680, 680, 250, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (132, 164, 1, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T16:59:38.380' AS DateTime), NULL, 1, N'', 0, 0, 50, 525, 397, 432, 0, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (133, 165, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T17:00:00.273' AS DateTime), NULL, 1, N'', 0, 0, 325, 215, 230, 230, 15, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (134, 166, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T17:00:45.080' AS DateTime), NULL, 1, N'', 0, 0, 0, 0, 577, 577, 37, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (135, 167, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T17:00:52.767' AS DateTime), NULL, 1, N'', 0, 0, 5, 0, 572, 572, 37, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (136, 168, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T17:02:08.967' AS DateTime), NULL, 1, N'', 0, 0, 5, 0, 572, 572, 37, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (137, 169, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T17:02:12.140' AS DateTime), NULL, 1, N'', 0, 0, 5, 0, 572, 572, 37, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (138, 170, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T17:02:47.083' AS DateTime), NULL, 1, N'', 0, 0, 13, 0, 557, 557, 30, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (139, 171, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T17:02:56.657' AS DateTime), NULL, 1, N'', 0, 0, 33, 0, 537, 537, 30, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (140, 172, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(N'2020-03-18T17:03:11.080' AS DateTime), NULL, 1, N'', 0, 0, 89, 0, 541, 541, 90, NULL)
SET IDENTITY_INSERT [dbo].[AttendanceStatus] OFF
SET IDENTITY_INSERT [dbo].[AttendanceType] ON 

INSERT [dbo].[AttendanceType] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, N'Daily Attendance', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[AttendanceType] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, N'Lunch', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[AttendanceType] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (3, N'Tea', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[AttendanceType] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (4, N'Prayers', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[AttendanceType] OFF
SET IDENTITY_INSERT [dbo].[AttendanceVariable] ON 

INSERT [dbo].[AttendanceVariable] ([ID], [Name], [Description], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, N'Full Day', N'', 1, CAST(N'2020-02-10T22:36:55.230' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[AttendanceVariable] ([ID], [Name], [Description], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, N'Half Day', N'', 1, CAST(N'2020-02-10T22:37:39.580' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[AttendanceVariable] ([ID], [Name], [Description], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (4, N'Overtime', N'', 1, CAST(N'2020-02-10T22:36:55.230' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[AttendanceVariable] ([ID], [Name], [Description], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (5, N'Compensatory', N'', 1, CAST(N'2020-02-10T22:37:39.580' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[AttendanceVariable] ([ID], [Name], [Description], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (6, N'Standard', N'', 1, CAST(N'2020-02-10T22:37:39.580' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[AttendanceVariable] ([ID], [Name], [Description], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (7, N'Early', N'', 1, CAST(N'2020-03-10T14:47:01.680' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[AttendanceVariable] ([ID], [Name], [Description], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (8, N'Late', N'', 1, CAST(N'2020-03-10T14:47:01.680' AS DateTime), NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[AttendanceVariable] OFF
SET IDENTITY_INSERT [dbo].[Branch] ON 

INSERT [dbo].[Branch] ([ID], [Name], [AddressLine], [CityID], [StateID], [CountryID], [ZipCode], [PhoneNumber], [GUID], [CreatedDate], [UpdationDate], [UpdatedBy], [UserIP], [IsActive]) VALUES (1, N'21D-201', NULL, NULL, NULL, NULL, NULL, NULL, N'd02caa04afba49688db5d4218800873f', CAST(N'2020-02-10T21:21:41.117' AS DateTime), NULL, NULL, N'::1', 1)
SET IDENTITY_INSERT [dbo].[Branch] OFF
SET IDENTITY_INSERT [dbo].[BranchDepartment] ON 

INSERT [dbo].[BranchDepartment] ([ID], [BranchID], [DepartmentID], [CreatedDate], [UpdatedDate], [UpdatedBy], [UserIP], [IsActive]) VALUES (1, 1, 1, CAST(N'2020-02-10T21:53:26.297' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[BranchDepartment] ([ID], [BranchID], [DepartmentID], [CreatedDate], [UpdatedDate], [UpdatedBy], [UserIP], [IsActive]) VALUES (2, 1, 2, CAST(N'2020-02-10T22:16:16.720' AS DateTime), NULL, 1, N'::1', 1)
INSERT [dbo].[BranchDepartment] ([ID], [BranchID], [DepartmentID], [CreatedDate], [UpdatedDate], [UpdatedBy], [UserIP], [IsActive]) VALUES (3, 1, 3, CAST(N'2020-02-10T22:16:43.850' AS DateTime), NULL, 1, N'::1', 1)
INSERT [dbo].[BranchDepartment] ([ID], [BranchID], [DepartmentID], [CreatedDate], [UpdatedDate], [UpdatedBy], [UserIP], [IsActive]) VALUES (4, 1, 4, CAST(N'2020-02-10T22:17:00.923' AS DateTime), NULL, 1, N'::1', 1)
INSERT [dbo].[BranchDepartment] ([ID], [BranchID], [DepartmentID], [CreatedDate], [UpdatedDate], [UpdatedBy], [UserIP], [IsActive]) VALUES (5, 1, 5, CAST(N'2020-02-10T22:17:15.190' AS DateTime), NULL, 1, N'::1', 1)
INSERT [dbo].[BranchDepartment] ([ID], [BranchID], [DepartmentID], [CreatedDate], [UpdatedDate], [UpdatedBy], [UserIP], [IsActive]) VALUES (6, 1, 6, CAST(N'2020-03-04T15:44:09.927' AS DateTime), NULL, 1, N'::1', 1)
SET IDENTITY_INSERT [dbo].[BranchDepartment] OFF
SET IDENTITY_INSERT [dbo].[BranchShift] ON 

INSERT [dbo].[BranchShift] ([ID], [BranchID], [ShiftID], [CreatedDate], [UpdatedDate], [UpdatedBy], [UserIP], [IsActive]) VALUES (1, 1, 2, CAST(N'2020-03-18T11:01:28.667' AS DateTime), CAST(N'2020-03-18T11:01:28.667' AS DateTime), 1, N'::1', 1)
INSERT [dbo].[BranchShift] ([ID], [BranchID], [ShiftID], [CreatedDate], [UpdatedDate], [UpdatedBy], [UserIP], [IsActive]) VALUES (2, 1, 3, CAST(N'2020-03-18T11:04:47.317' AS DateTime), CAST(N'2020-03-18T11:04:47.317' AS DateTime), 1, N'::1', 1)
INSERT [dbo].[BranchShift] ([ID], [BranchID], [ShiftID], [CreatedDate], [UpdatedDate], [UpdatedBy], [UserIP], [IsActive]) VALUES (3, 1, 4, CAST(N'2020-03-18T11:05:43.843' AS DateTime), CAST(N'2020-03-18T11:05:43.843' AS DateTime), 1, N'::1', 1)
INSERT [dbo].[BranchShift] ([ID], [BranchID], [ShiftID], [CreatedDate], [UpdatedDate], [UpdatedBy], [UserIP], [IsActive]) VALUES (4, 1, 5, CAST(N'2020-03-18T11:06:52.190' AS DateTime), CAST(N'2020-03-18T11:06:52.190' AS DateTime), 1, N'::1', 1)
INSERT [dbo].[BranchShift] ([ID], [BranchID], [ShiftID], [CreatedDate], [UpdatedDate], [UpdatedBy], [UserIP], [IsActive]) VALUES (5, 1, 6, CAST(N'2020-03-18T11:07:29.810' AS DateTime), CAST(N'2020-03-18T11:07:29.810' AS DateTime), 1, N'::1', 1)
SET IDENTITY_INSERT [dbo].[BranchShift] OFF
SET IDENTITY_INSERT [dbo].[Configuration] ON 

INSERT [dbo].[Configuration] ([ID], [Name], [Type], [Value], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, N'SERVICE STATUS', N'BOOLEAN', N'1', 1, CAST(N'2016-04-27T05:09:47.000' AS DateTime), CAST(N'2020-03-05T19:27:03.250' AS DateTime), 1, N'::1')
INSERT [dbo].[Configuration] ([ID], [Name], [Type], [Value], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, N'PreAttendance', N'DATETIME', N'Dec 18 2016 11:00PM', 0, CAST(N'2016-06-20T15:45:30.113' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Configuration] ([ID], [Name], [Type], [Value], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (3, N'ALTERNATE ATTENDANCE', N'BOOLEAN', N'1', 1, CAST(N'2016-06-22T16:55:45.000' AS DateTime), CAST(N'2020-03-05T19:27:03.310' AS DateTime), 1, N'::1')
SET IDENTITY_INSERT [dbo].[Configuration] OFF
SET IDENTITY_INSERT [dbo].[ContactType] ON 

INSERT [dbo].[ContactType] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, N'Landline', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[ContactType] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, N'Mobile Number', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[ContactType] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (3, N'Alternate Mobile Number', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[ContactType] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (4, N'Emergency Mobile Number', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[ContactType] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (5, N'Email Address', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[ContactType] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (6, N'Alternate Email Address', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[ContactType] OFF
SET IDENTITY_INSERT [dbo].[Country] ON 

INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, N'Afghanistan', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, N'Albania', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (3, N'Algeria', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (4, N'American Samoa', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (5, N'Andorra', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (6, N'Angola', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (7, N'Anguilla', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (8, N'Antarctica', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (9, N'Antigua and Barbuda', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (10, N'Argentina', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (11, N'Armenia', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (12, N'Aruba', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (13, N'Australia', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (14, N'Austria', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (15, N'Azerbaijan', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (16, N'Bahamas', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (17, N'Bahrain', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (18, N'Bangladesh', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (19, N'Barbados', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (20, N'Belarus', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (21, N'Belgium', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (22, N'Belize', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (23, N'Benin', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (24, N'Bermuda', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (25, N'Bhutan', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (26, N'Bolivia', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (27, N'Bosnia and Herzegovina', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (28, N'Botswana', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (29, N'Bouvet Island', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (30, N'Brazil', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (31, N'British Indian Ocean Territory', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (32, N'Brunei Darussalam', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (33, N'Bulgaria', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (34, N'Burkina Faso', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (35, N'Burundi', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (36, N'Cambodia', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (37, N'Cameroon', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (38, N'Canada', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (39, N'Cape Verde', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (40, N'Cayman Islands', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (41, N'Central African Republic', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (42, N'Chad', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (43, N'Chile', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (44, N'China', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (45, N'Christmas Island', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (46, N'Cocos (Keeling) Islands', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (47, N'Colombia', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (48, N'Comoros', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (49, N'Congo', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (50, N'Congo, the Democratic Republic of the', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (51, N'Cook Islands', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (52, N'Costa Rica', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (53, N'Cote D''Ivoire', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (54, N'Croatia', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (55, N'Cuba', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (56, N'Cyprus', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (57, N'Czech Republic', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (58, N'Denmark', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (59, N'Djibouti', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (60, N'Dominica', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (61, N'Dominican Republic', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (62, N'Ecuador', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (63, N'Egypt', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (64, N'El Salvador', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (65, N'Equatorial Guinea', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (66, N'Eritrea', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (67, N'Estonia', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (68, N'Ethiopia', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (69, N'Falkland Islands (Malvinas)', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (70, N'Faroe Islands', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (71, N'Fiji', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (72, N'Finland', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (73, N'France', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (74, N'French Guiana', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (75, N'French Polynesia', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (76, N'French Southern Territories', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (77, N'Gabon', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (78, N'Gambia', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (79, N'Georgia', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (80, N'Germany', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (81, N'Ghana', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (82, N'Gibraltar', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (83, N'Greece', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (84, N'Greenland', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (85, N'Grenada', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (86, N'Guadeloupe', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (87, N'Guam', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (88, N'Guatemala', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (89, N'Guinea', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (90, N'Guinea-Bissau', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (91, N'Guyana', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (92, N'Haiti', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (93, N'Heard Island and Mcdonald Islands', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (94, N'Holy See (Vatican City State)', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (95, N'Honduras', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (96, N'Hong Kong', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (97, N'Hungary', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (98, N'Iceland', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (99, N'India', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (100, N'Indonesia', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (101, N'Iran, Islamic Republic of', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (102, N'Iraq', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (103, N'Ireland', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (104, N'Israel', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (105, N'Italy', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (106, N'Jamaica', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (107, N'Japan', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (108, N'Jordan', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (109, N'Kazakhstan', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (110, N'Kenya', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (111, N'Kiribati', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (112, N'Korea, Democratic People''s Republic of', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (113, N'Korea, Republic of', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (114, N'Kuwait', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (115, N'Kyrgyzstan', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (116, N'Lao People''s Democratic Republic', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (117, N'Latvia', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (118, N'Lebanon', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (119, N'Lesotho', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (120, N'Liberia', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (121, N'Libyan Arab Jamahiriya', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (122, N'Liechtenstein', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (123, N'Lithuania', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (124, N'Luxembourg', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (125, N'Macao', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (126, N'Macedonia, the Former Yugoslav Republic of', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (127, N'Madagascar', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (128, N'Malawi', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (129, N'Malaysia', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (130, N'Maldives', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (131, N'Mali', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (132, N'Malta', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (133, N'Marshall Islands', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (134, N'Martinique', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (135, N'Mauritania', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (136, N'Mauritius', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (137, N'Mayotte', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (138, N'Mexico', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (139, N'Micronesia, Federated States of', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (140, N'Moldova, Republic of', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (141, N'Monaco', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (142, N'Mongolia', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (143, N'Montserrat', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (144, N'Morocco', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (145, N'Mozambique', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (146, N'Myanmar', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (147, N'Namibia', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (148, N'Nauru', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (149, N'Nepal', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (150, N'Netherlands', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (151, N'Netherlands Antilles', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (152, N'New Caledonia', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (153, N'New Zealand', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (154, N'Nicaragua', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (155, N'Niger', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (156, N'Nigeria', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (157, N'Niue', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (158, N'Norfolk Island', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (159, N'Northern Mariana Islands', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (160, N'Norway', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (161, N'Oman', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (162, N'Pakistan', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (163, N'Palau', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (164, N'Palestinian Territory, Occupied', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (165, N'Panama', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (166, N'Papua New Guinea', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (167, N'Paraguay', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (168, N'Peru', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (169, N'Philippines', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (170, N'Pitcairn', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (171, N'Poland', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (172, N'Portugal', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (173, N'Puerto Rico', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (174, N'Qatar', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (175, N'Reunion', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (176, N'Romania', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (177, N'Russian Federation', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (178, N'Rwanda', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (179, N'Saint Helena', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (180, N'Saint Kitts and Nevis', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (181, N'Saint Lucia', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (182, N'Saint Pierre and Miquelon', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (183, N'Saint Vincent and the Grenadines', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (184, N'Samoa', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (185, N'San Marino', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (186, N'Sao Tome and Principe', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (187, N'Saudi Arabia', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (188, N'Senegal', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (189, N'Serbia and Montenegro', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (190, N'Seychelles', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (191, N'Sierra Leone', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (192, N'Singapore', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (193, N'Slovakia', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (194, N'Slovenia', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (195, N'Solomon Islands', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (196, N'Somalia', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (197, N'South Africa', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (198, N'South Georgia and the South Sandwich Islands', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (199, N'Spain', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (200, N'Sri Lanka', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (201, N'Sudan', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (202, N'Suriname', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (203, N'Svalbard and Jan Mayen', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (204, N'Swaziland', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (205, N'Sweden', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (206, N'Switzerland', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (207, N'Syrian Arab Republic', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (208, N'Taiwan, Province of China', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (209, N'Tajikistan', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (210, N'Tanzania, United Republic of', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (211, N'Thailand', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (212, N'Timor-Leste', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (213, N'Togo', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (214, N'Tokelau', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (215, N'Tonga', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (216, N'Trinidad and Tobago', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (217, N'Tunisia', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (218, N'Turkey', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (219, N'Turkmenistan', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (220, N'Turks and Caicos Islands', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (221, N'Tuvalu', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (222, N'Uganda', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (223, N'Ukraine', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (224, N'United Arab Emirates', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (225, N'United Kingdom', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (226, N'United States', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (227, N'United States Minor Outlying Islands', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (228, N'Uruguay', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (229, N'Uzbekistan', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (230, N'Vanuatu', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (231, N'Venezuela', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (232, N'Viet Nam', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (233, N'Virgin Islands, British', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (234, N'Virgin Islands, U.s.', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (235, N'Wallis and Futuna', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (236, N'Western Sahara', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (237, N'Yemen', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (238, N'Zambia', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (239, N'Zimbabwe', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Country] OFF
SET IDENTITY_INSERT [dbo].[Department] ON 

INSERT [dbo].[Department] ([ID], [Name], [Description], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, N'Head', NULL, 1, CAST(N'2016-07-19T17:16:40.000' AS DateTime), CAST(N'2020-03-05T11:39:03.690' AS DateTime), 1, N'::1')
INSERT [dbo].[Department] ([ID], [Name], [Description], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, N'Software', N'Software developers, Website developers.', 1, CAST(N'2020-02-10T22:16:16.000' AS DateTime), CAST(N'2020-03-11T12:44:23.763' AS DateTime), 1, N'::1')
INSERT [dbo].[Department] ([ID], [Name], [Description], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (3, N'Graphics', N'Illustrator, designer, editor', 1, CAST(N'2020-02-10T22:16:43.807' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[Department] ([ID], [Name], [Description], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (4, N'Content', N'Creative writers', 1, CAST(N'2020-02-10T22:17:00.867' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[Department] ([ID], [Name], [Description], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (5, N'Sales', N'Sales Agent, Support Agents', 1, CAST(N'2020-02-10T22:17:15.150' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[Department] ([ID], [Name], [Description], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (6, N'new department', N'new department', 1, CAST(N'2020-03-04T15:44:09.900' AS DateTime), NULL, 1, N'::1')
SET IDENTITY_INSERT [dbo].[Department] OFF
SET IDENTITY_INSERT [dbo].[Gender] ON 

INSERT [dbo].[Gender] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, N'Male', 1, CAST(N'2020-02-10T21:19:32.370' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Gender] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, N'Female', 1, CAST(N'2020-02-10T21:19:32.370' AS DateTime), NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Gender] OFF
SET IDENTITY_INSERT [dbo].[Holiday] ON 

INSERT [dbo].[Holiday] ([ID], [Name], [Date], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, N'Kashmir Day', CAST(N'2020-03-20T00:00:00.000' AS DateTime), N'Kashmir Day', 1, CAST(N'2020-03-19T16:06:54.830' AS DateTime), NULL, 1, N'::1')
SET IDENTITY_INSERT [dbo].[Holiday] OFF
SET IDENTITY_INSERT [dbo].[LeaveType] ON 

INSERT [dbo].[LeaveType] ([ID], [Name], [YearlyLeaves], [PriorDays], [MaxDays], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, N'Casual', 10, 5, 5, 1, CAST(N'2017-03-13T11:28:02.380' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[LeaveType] ([ID], [Name], [YearlyLeaves], [PriorDays], [MaxDays], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, N'Sick', 10, 5, 5, 1, CAST(N'2017-03-13T11:28:02.380' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[LeaveType] ([ID], [Name], [YearlyLeaves], [PriorDays], [MaxDays], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (3, N'Planned', 10, 5, 5, 1, CAST(N'2017-03-13T11:28:02.397' AS DateTime), NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[LeaveType] OFF
SET IDENTITY_INSERT [dbo].[OG_LeaveType] ON 

INSERT [dbo].[OG_LeaveType] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdatedBy], [UserIP]) VALUES (1, N'Casual', 1, CAST(N'2017-03-13T11:24:27.090' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[OG_LeaveType] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdatedBy], [UserIP]) VALUES (2, N'Sick', 1, CAST(N'2017-03-13T11:24:27.090' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[OG_LeaveType] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdatedBy], [UserIP]) VALUES (3, N'Planned', 1, CAST(N'2017-03-13T11:24:27.090' AS DateTime), NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[OG_LeaveType] OFF
SET IDENTITY_INSERT [dbo].[Payroll] ON 

INSERT [dbo].[Payroll] ([ID], [PayrollCycleID], [UserID], [Salary], [Addition], [Deduction], [NetSalary], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, 1, 2, CAST(25000.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(25000.00 AS Decimal(9, 2)), 0, CAST(N'2020-03-04T13:51:44.787' AS DateTime), CAST(N'2020-03-05T16:07:29.210' AS DateTime), NULL, NULL)
INSERT [dbo].[Payroll] ([ID], [PayrollCycleID], [UserID], [Salary], [Addition], [Deduction], [NetSalary], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, 1, 3, CAST(10000.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(10000.00 AS Decimal(9, 2)), 0, CAST(N'2020-03-04T13:51:44.850' AS DateTime), CAST(N'2020-03-05T16:07:29.230' AS DateTime), NULL, NULL)
INSERT [dbo].[Payroll] ([ID], [PayrollCycleID], [UserID], [Salary], [Addition], [Deduction], [NetSalary], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (3, 1, 4, CAST(35000.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(35000.00 AS Decimal(9, 2)), 0, CAST(N'2020-03-04T13:51:44.853' AS DateTime), CAST(N'2020-03-05T16:07:29.287' AS DateTime), NULL, NULL)
INSERT [dbo].[Payroll] ([ID], [PayrollCycleID], [UserID], [Salary], [Addition], [Deduction], [NetSalary], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (4, 1, 5, CAST(40000.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(40000.00 AS Decimal(9, 2)), 0, CAST(N'2020-03-04T13:51:44.890' AS DateTime), CAST(N'2020-03-05T16:07:29.313' AS DateTime), NULL, NULL)
INSERT [dbo].[Payroll] ([ID], [PayrollCycleID], [UserID], [Salary], [Addition], [Deduction], [NetSalary], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (5, 1, 6, CAST(32000.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(32000.00 AS Decimal(9, 2)), 0, CAST(N'2020-03-04T13:51:44.890' AS DateTime), CAST(N'2020-03-05T16:07:29.317' AS DateTime), NULL, NULL)
INSERT [dbo].[Payroll] ([ID], [PayrollCycleID], [UserID], [Salary], [Addition], [Deduction], [NetSalary], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (6, 1, 7, CAST(10000.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(10000.00 AS Decimal(9, 2)), 0, CAST(N'2020-03-04T13:51:44.890' AS DateTime), CAST(N'2020-03-05T16:07:29.357' AS DateTime), NULL, NULL)
INSERT [dbo].[Payroll] ([ID], [PayrollCycleID], [UserID], [Salary], [Addition], [Deduction], [NetSalary], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (7, 1, 2, CAST(25000.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(25000.00 AS Decimal(9, 2)), 1, CAST(N'2020-03-04T13:52:14.597' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Payroll] ([ID], [PayrollCycleID], [UserID], [Salary], [Addition], [Deduction], [NetSalary], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (8, 1, 3, CAST(10000.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(10000.00 AS Decimal(9, 2)), 1, CAST(N'2020-03-04T13:52:14.600' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Payroll] ([ID], [PayrollCycleID], [UserID], [Salary], [Addition], [Deduction], [NetSalary], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (9, 1, 4, CAST(35000.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(35000.00 AS Decimal(9, 2)), 1, CAST(N'2020-03-04T13:52:14.603' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Payroll] ([ID], [PayrollCycleID], [UserID], [Salary], [Addition], [Deduction], [NetSalary], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (10, 1, 5, CAST(40000.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(40000.00 AS Decimal(9, 2)), 1, CAST(N'2020-03-04T13:52:14.603' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Payroll] ([ID], [PayrollCycleID], [UserID], [Salary], [Addition], [Deduction], [NetSalary], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (11, 1, 6, CAST(32000.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(32000.00 AS Decimal(9, 2)), 1, CAST(N'2020-03-04T13:52:14.607' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Payroll] ([ID], [PayrollCycleID], [UserID], [Salary], [Addition], [Deduction], [NetSalary], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (12, 1, 7, CAST(10000.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(10000.00 AS Decimal(9, 2)), 1, CAST(N'2020-03-04T13:52:14.630' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Payroll] ([ID], [PayrollCycleID], [UserID], [Salary], [Addition], [Deduction], [NetSalary], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (13, 1, 2, CAST(25000.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(25000.00 AS Decimal(9, 2)), 1, CAST(N'2020-03-04T17:45:28.947' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Payroll] ([ID], [PayrollCycleID], [UserID], [Salary], [Addition], [Deduction], [NetSalary], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (14, 1, 3, CAST(10000.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(10000.00 AS Decimal(9, 2)), 1, CAST(N'2020-03-04T17:45:28.960' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Payroll] ([ID], [PayrollCycleID], [UserID], [Salary], [Addition], [Deduction], [NetSalary], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (15, 1, 4, CAST(35000.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(35000.00 AS Decimal(9, 2)), 1, CAST(N'2020-03-04T17:45:28.977' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Payroll] ([ID], [PayrollCycleID], [UserID], [Salary], [Addition], [Deduction], [NetSalary], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (16, 1, 5, CAST(40000.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(40000.00 AS Decimal(9, 2)), 1, CAST(N'2020-03-04T17:45:28.997' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Payroll] ([ID], [PayrollCycleID], [UserID], [Salary], [Addition], [Deduction], [NetSalary], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (17, 1, 6, CAST(32000.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(32000.00 AS Decimal(9, 2)), 1, CAST(N'2020-03-04T17:45:29.000' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Payroll] ([ID], [PayrollCycleID], [UserID], [Salary], [Addition], [Deduction], [NetSalary], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (18, 1, 7, CAST(10000.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(10000.00 AS Decimal(9, 2)), 1, CAST(N'2020-03-04T17:45:29.020' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Payroll] ([ID], [PayrollCycleID], [UserID], [Salary], [Addition], [Deduction], [NetSalary], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (19, 1, 2, CAST(25000.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(25000.00 AS Decimal(9, 2)), 1, CAST(N'2020-03-04T17:45:35.553' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Payroll] ([ID], [PayrollCycleID], [UserID], [Salary], [Addition], [Deduction], [NetSalary], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (20, 1, 3, CAST(10000.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(10000.00 AS Decimal(9, 2)), 1, CAST(N'2020-03-04T17:45:35.557' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Payroll] ([ID], [PayrollCycleID], [UserID], [Salary], [Addition], [Deduction], [NetSalary], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (21, 1, 4, CAST(35000.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(35000.00 AS Decimal(9, 2)), 1, CAST(N'2020-03-04T17:45:35.557' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Payroll] ([ID], [PayrollCycleID], [UserID], [Salary], [Addition], [Deduction], [NetSalary], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (22, 1, 5, CAST(40000.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(40000.00 AS Decimal(9, 2)), 1, CAST(N'2020-03-04T17:45:35.560' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Payroll] ([ID], [PayrollCycleID], [UserID], [Salary], [Addition], [Deduction], [NetSalary], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (23, 1, 6, CAST(32000.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(32000.00 AS Decimal(9, 2)), 1, CAST(N'2020-03-04T17:45:35.560' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Payroll] ([ID], [PayrollCycleID], [UserID], [Salary], [Addition], [Deduction], [NetSalary], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (24, 1, 7, CAST(10000.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(0.00 AS Decimal(9, 2)), CAST(10000.00 AS Decimal(9, 2)), 1, CAST(N'2020-03-04T17:45:35.563' AS DateTime), NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Payroll] OFF
SET IDENTITY_INSERT [dbo].[PayrollCycle] ON 

INSERT [dbo].[PayrollCycle] ([ID], [Name], [Month], [Year], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, N'Mar 2020 Payroll', 3, 2020, 1, CAST(N'2020-03-03T13:52:51.533' AS DateTime), NULL, 1, N'::1')
SET IDENTITY_INSERT [dbo].[PayrollCycle] OFF
SET IDENTITY_INSERT [dbo].[PayrollPolicy] ON 

INSERT [dbo].[PayrollPolicy] ([ID], [PayrollVariableID], [IsUnit], [IsPercentage], [Value], [Description], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, 1, 1, 0, CAST(1000.00 AS Decimal(9, 2)), NULL, CAST(N'2020-03-04T00:00:00.000' AS DateTime), NULL, CAST(N'2020-03-04T17:43:14.367' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[PayrollPolicy] ([ID], [PayrollVariableID], [IsUnit], [IsPercentage], [Value], [Description], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, 2, 1, 0, CAST(1500.00 AS Decimal(9, 2)), NULL, CAST(N'2020-03-05T00:00:00.000' AS DateTime), NULL, CAST(N'2020-03-05T11:14:47.033' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[PayrollPolicy] ([ID], [PayrollVariableID], [IsUnit], [IsPercentage], [Value], [Description], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (3, 4, 0, 1, CAST(10.00 AS Decimal(9, 2)), NULL, CAST(N'2020-03-05T00:00:00.000' AS DateTime), NULL, CAST(N'2020-03-05T19:29:10.863' AS DateTime), NULL, 1, N'::1')
SET IDENTITY_INSERT [dbo].[PayrollPolicy] OFF
SET IDENTITY_INSERT [dbo].[PayrollVariable] ON 

INSERT [dbo].[PayrollVariable] ([ID], [Name], [IsDeduction], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, N'Quarter Day', 1, 1, CAST(N'2020-02-10T22:36:58.517' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[PayrollVariable] ([ID], [Name], [IsDeduction], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, N'Half Day', 1, 1, CAST(N'2020-02-10T22:36:58.517' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[PayrollVariable] ([ID], [Name], [IsDeduction], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (3, N'Full Day', 1, 1, CAST(N'2020-02-10T22:36:58.517' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[PayrollVariable] ([ID], [Name], [IsDeduction], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (4, N'Absent', 1, 1, CAST(N'2020-02-10T22:36:58.517' AS DateTime), NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[PayrollVariable] OFF
SET IDENTITY_INSERT [dbo].[Religion] ON 

INSERT [dbo].[Religion] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, N'Muslim', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Religion] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, N'Christian', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Religion] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (3, N'Hindu', 1, CAST(N'2016-04-09T20:36:17.950' AS DateTime), NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Religion] OFF
SET IDENTITY_INSERT [dbo].[SalaryType] ON 

INSERT [dbo].[SalaryType] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, N'Monthly Salary', 1, CAST(N'2016-07-19T17:03:41.690' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[SalaryType] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, N'Weekly Salary', 0, CAST(N'2016-07-19T17:03:41.690' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[SalaryType] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (3, N'Daily Salary', 1, CAST(N'2016-07-19T17:03:41.690' AS DateTime), NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[SalaryType] OFF
SET IDENTITY_INSERT [dbo].[Shift] ON 

INSERT [dbo].[Shift] ([ID], [Name], [Description], [StartHour], [EndHour], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [BreakHour]) VALUES (2, N'Morning10', N'Morning 10am to 7pm 1hour lunch break, sat-sun off', N'10:00', N'19:00', 1, CAST(N'2020-03-18T11:01:28.540' AS DateTime), NULL, 1, N'::1', CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Shift] ([ID], [Name], [Description], [StartHour], [EndHour], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [BreakHour]) VALUES (3, N'Morning9', N'9am to 6pm, Sat, Sun off, 1 hour break', N'9:00', N'18:00', 1, CAST(N'2020-03-18T11:04:47.273' AS DateTime), NULL, 1, N'::1', CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Shift] ([ID], [Name], [Description], [StartHour], [EndHour], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [BreakHour]) VALUES (4, N'Evening 2', N'2pm to 2am, sunday off, no break', N'14:00', N'2:00', 1, CAST(N'2020-03-18T11:05:43.807' AS DateTime), NULL, 1, N'::1', CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Shift] ([ID], [Name], [Description], [StartHour], [EndHour], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [BreakHour]) VALUES (5, N'Evening 5', N'5pm to 2am', N'17:00', N'2:00', 1, CAST(N'2020-03-18T11:06:52.137' AS DateTime), NULL, 1, N'::1', CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Shift] ([ID], [Name], [Description], [StartHour], [EndHour], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [BreakHour]) VALUES (6, N'Night 9', N'9pm to 6am', N'21:00', N'6:00', 1, CAST(N'2020-03-18T11:07:29.743' AS DateTime), NULL, 1, N'::1', CAST(0.00 AS Decimal(18, 2)))
SET IDENTITY_INSERT [dbo].[Shift] OFF
SET IDENTITY_INSERT [dbo].[ShiftOffDay] ON 

INSERT [dbo].[ShiftOffDay] ([ID], [ShiftID], [OffDayOfWeek], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, 2, 7, CAST(N'2020-01-01T00:00:00.000' AS DateTime), NULL, CAST(N'2020-03-18T11:01:28.587' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[ShiftOffDay] ([ID], [ShiftID], [OffDayOfWeek], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, 2, 6, CAST(N'2020-01-01T00:00:00.000' AS DateTime), NULL, CAST(N'2020-03-18T11:01:28.653' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[ShiftOffDay] ([ID], [ShiftID], [OffDayOfWeek], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (3, 3, 7, CAST(N'2020-01-01T00:00:00.000' AS DateTime), NULL, CAST(N'2020-03-18T11:04:47.290' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[ShiftOffDay] ([ID], [ShiftID], [OffDayOfWeek], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (4, 3, 6, CAST(N'2020-01-01T00:00:00.000' AS DateTime), NULL, CAST(N'2020-03-18T11:04:47.303' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[ShiftOffDay] ([ID], [ShiftID], [OffDayOfWeek], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (5, 4, 7, CAST(N'2020-01-01T00:00:00.000' AS DateTime), NULL, CAST(N'2020-03-18T11:05:43.827' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[ShiftOffDay] ([ID], [ShiftID], [OffDayOfWeek], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (6, 5, 7, CAST(N'2020-01-01T00:00:00.000' AS DateTime), NULL, CAST(N'2020-03-18T11:06:52.167' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[ShiftOffDay] ([ID], [ShiftID], [OffDayOfWeek], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (7, 5, 6, CAST(N'2020-01-01T00:00:00.000' AS DateTime), NULL, CAST(N'2020-03-18T11:06:52.180' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[ShiftOffDay] ([ID], [ShiftID], [OffDayOfWeek], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (8, 6, 7, CAST(N'2020-01-01T00:00:00.000' AS DateTime), NULL, CAST(N'2020-03-18T11:07:29.780' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[ShiftOffDay] ([ID], [ShiftID], [OffDayOfWeek], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (9, 6, 6, CAST(N'2020-01-01T00:00:00.000' AS DateTime), NULL, CAST(N'2020-03-18T11:07:29.797' AS DateTime), NULL, 1, N'::1')
SET IDENTITY_INSERT [dbo].[ShiftOffDay] OFF
SET IDENTITY_INSERT [dbo].[User] ON 

INSERT [dbo].[User] ([ID], [FirstName], [MiddleName], [LastName], [UserTypeID], [GenderID], [DateOfBirth], [NICNo], [ReligionID], [Address1], [Address2], [ZipCode], [CountryID], [CityID], [StateID], [AcadmicQualification], [Designation], [Salary], [LoginID], [Password], [ImagePath], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [SalaryTypeID], [FatherName], [AccountNumber]) VALUES (1, N'Syed Razi Wahid', NULL, NULL, 1, 1, CAST(N'2016-07-19T17:27:14.143' AS DateTime), NULL, NULL, NULL, NULL, NULL, 162, NULL, NULL, NULL, N'Asstt Acctt', NULL, N'razi', N'J0$h123', NULL, 1, CAST(N'2016-07-19T17:27:14.143' AS DateTime), NULL, NULL, NULL, NULL, N'Syed Wahid', N'36669-5')
INSERT [dbo].[User] ([ID], [FirstName], [MiddleName], [LastName], [UserTypeID], [GenderID], [DateOfBirth], [NICNo], [ReligionID], [Address1], [Address2], [ZipCode], [CountryID], [CityID], [StateID], [AcadmicQualification], [Designation], [Salary], [LoginID], [Password], [ImagePath], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [SalaryTypeID], [FatherName], [AccountNumber]) VALUES (2, N'Arquam Belal', NULL, NULL, 2, 1, CAST(N'1980-01-01T00:00:00.000' AS DateTime), N'45455-5678468-4', 1, NULL, NULL, NULL, 162, NULL, NULL, N'BSCS', N'Website Developer', CAST(25000.00 AS Decimal(9, 2)), N'arquam', N'arquam', N'/Uploads/ProfileImages\2.png', 1, CAST(N'2020-02-10T22:20:25.000' AS DateTime), CAST(N'2020-03-05T16:05:11.107' AS DateTime), 1, N'::1', 1, N'F/O Arquam Belal', N'12345646')
INSERT [dbo].[User] ([ID], [FirstName], [MiddleName], [LastName], [UserTypeID], [GenderID], [DateOfBirth], [NICNo], [ReligionID], [Address1], [Address2], [ZipCode], [CountryID], [CityID], [StateID], [AcadmicQualification], [Designation], [Salary], [LoginID], [Password], [ImagePath], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [SalaryTypeID], [FatherName], [AccountNumber]) VALUES (3, N'Saeed Hussain Shah', NULL, NULL, 2, 1, CAST(N'1980-01-01T00:00:00.000' AS DateTime), N'45524-8755458-2', 1, NULL, NULL, NULL, 162, NULL, NULL, N'Certified', N'Website Developer Internee', CAST(10000.00 AS Decimal(9, 2)), N'saeed', N'saeed', NULL, 1, CAST(N'2020-02-10T22:22:18.160' AS DateTime), NULL, 1, N'::1', 1, N'FO Saeed Hussain Shah', NULL)
INSERT [dbo].[User] ([ID], [FirstName], [MiddleName], [LastName], [UserTypeID], [GenderID], [DateOfBirth], [NICNo], [ReligionID], [Address1], [Address2], [ZipCode], [CountryID], [CityID], [StateID], [AcadmicQualification], [Designation], [Salary], [LoginID], [Password], [ImagePath], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [SalaryTypeID], [FatherName], [AccountNumber]) VALUES (4, N'Abdul Azeez', NULL, NULL, 2, 1, CAST(N'1980-01-01T00:00:00.000' AS DateTime), NULL, 1, NULL, NULL, NULL, 162, NULL, NULL, N'BSCS', N'Website Developer', CAST(35000.00 AS Decimal(9, 2)), N'azeez', N'azeez', NULL, 1, CAST(N'2020-02-10T22:23:24.643' AS DateTime), NULL, 1, N'::1', 1, N'FO Abdul Azeez', NULL)
INSERT [dbo].[User] ([ID], [FirstName], [MiddleName], [LastName], [UserTypeID], [GenderID], [DateOfBirth], [NICNo], [ReligionID], [Address1], [Address2], [ZipCode], [CountryID], [CityID], [StateID], [AcadmicQualification], [Designation], [Salary], [LoginID], [Password], [ImagePath], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [SalaryTypeID], [FatherName], [AccountNumber]) VALUES (5, N'Aqsa Shafiq', NULL, NULL, 2, 2, CAST(N'1980-01-01T00:00:00.000' AS DateTime), NULL, 1, NULL, NULL, NULL, 162, NULL, NULL, N'Masters', N'Graphics Designer', CAST(40000.00 AS Decimal(9, 2)), N'aqsa', N'aqsa', NULL, 1, CAST(N'2020-02-10T22:24:30.523' AS DateTime), NULL, 1, N'::1', 1, N'FO Aqsa Shafiq', NULL)
INSERT [dbo].[User] ([ID], [FirstName], [MiddleName], [LastName], [UserTypeID], [GenderID], [DateOfBirth], [NICNo], [ReligionID], [Address1], [Address2], [ZipCode], [CountryID], [CityID], [StateID], [AcadmicQualification], [Designation], [Salary], [LoginID], [Password], [ImagePath], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [SalaryTypeID], [FatherName], [AccountNumber]) VALUES (6, N'Javeria Asif', NULL, NULL, 2, 2, CAST(N'1980-01-01T00:00:00.000' AS DateTime), NULL, 1, NULL, NULL, NULL, 162, NULL, NULL, N'Masters', N'Creative Writer', CAST(32000.00 AS Decimal(9, 2)), N'javeria', N'javeria', NULL, 1, CAST(N'2020-02-10T22:25:32.367' AS DateTime), NULL, 1, N'::1', 1, N'FO Javeria Asif', NULL)
INSERT [dbo].[User] ([ID], [FirstName], [MiddleName], [LastName], [UserTypeID], [GenderID], [DateOfBirth], [NICNo], [ReligionID], [Address1], [Address2], [ZipCode], [CountryID], [CityID], [StateID], [AcadmicQualification], [Designation], [Salary], [LoginID], [Password], [ImagePath], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [SalaryTypeID], [FatherName], [AccountNumber]) VALUES (7, N'Saboor', NULL, NULL, 2, 1, CAST(N'1980-01-10T00:00:00.000' AS DateTime), N'54487-8547825-6', 1, NULL, NULL, NULL, 162, NULL, NULL, N'Masters', N'Website Developer Internee', CAST(10000.00 AS Decimal(9, 2)), N'saboor', N'Saboor123', NULL, 1, CAST(N'2020-03-03T13:42:20.000' AS DateTime), CAST(N'2020-03-04T17:04:58.047' AS DateTime), 1, N'::1', 1, N'FO Saboor', N'123456789-1')
INSERT [dbo].[User] ([ID], [FirstName], [MiddleName], [LastName], [UserTypeID], [GenderID], [DateOfBirth], [NICNo], [ReligionID], [Address1], [Address2], [ZipCode], [CountryID], [CityID], [StateID], [AcadmicQualification], [Designation], [Salary], [LoginID], [Password], [ImagePath], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [SalaryTypeID], [FatherName], [AccountNumber]) VALUES (8, N'Arqum', NULL, NULL, 2, 1, CAST(N'1997-09-08T00:00:00.000' AS DateTime), N'123456789', 1, N'21D', NULL, NULL, 162, NULL, NULL, N'qualification', N'IT', CAST(50.00 AS Decimal(9, 2)), N'arqum', N'arqum', NULL, 0, CAST(N'2020-03-04T13:27:40.703' AS DateTime), CAST(N'2020-03-04T13:31:20.000' AS DateTime), 1, N'::1', 1, N'Shakil', N'11234567896')
INSERT [dbo].[User] ([ID], [FirstName], [MiddleName], [LastName], [UserTypeID], [GenderID], [DateOfBirth], [NICNo], [ReligionID], [Address1], [Address2], [ZipCode], [CountryID], [CityID], [StateID], [AcadmicQualification], [Designation], [Salary], [LoginID], [Password], [ImagePath], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [SalaryTypeID], [FatherName], [AccountNumber]) VALUES (9, N'New User', NULL, NULL, 2, 1, CAST(N'1980-01-01T00:00:00.000' AS DateTime), N'1234569874563', 2, N'21D', NULL, NULL, 7, NULL, NULL, N'Inter', N'Saled Manager', CAST(500000.00 AS Decimal(9, 2)), N'abc', N'abc', NULL, 0, CAST(N'2020-03-04T15:35:31.000' AS DateTime), CAST(N'2020-03-04T16:54:56.000' AS DateTime), 1, N'::1', 3, N'New User''s Father', N'11234567896')
INSERT [dbo].[User] ([ID], [FirstName], [MiddleName], [LastName], [UserTypeID], [GenderID], [DateOfBirth], [NICNo], [ReligionID], [Address1], [Address2], [ZipCode], [CountryID], [CityID], [StateID], [AcadmicQualification], [Designation], [Salary], [LoginID], [Password], [ImagePath], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [SalaryTypeID], [FatherName], [AccountNumber]) VALUES (10, N'Hamza Malick', NULL, NULL, 2, 1, CAST(N'1980-01-01T00:00:00.000' AS DateTime), N'1234567894561', 1, N'Phase-10', NULL, NULL, 162, NULL, NULL, N'Bachelors', N'Web Developer', CAST(25000.00 AS Decimal(9, 2)), N'HamzaMalick', N'123', NULL, 0, CAST(N'2020-03-14T16:32:42.160' AS DateTime), CAST(N'2020-03-17T16:06:15.000' AS DateTime), 1, N'::1', 1, N'Malick Bilal', N'1234567894561')
INSERT [dbo].[User] ([ID], [FirstName], [MiddleName], [LastName], [UserTypeID], [GenderID], [DateOfBirth], [NICNo], [ReligionID], [Address1], [Address2], [ZipCode], [CountryID], [CityID], [StateID], [AcadmicQualification], [Designation], [Salary], [LoginID], [Password], [ImagePath], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [SalaryTypeID], [FatherName], [AccountNumber]) VALUES (11, N'Umair', NULL, NULL, NULL, 1, CAST(N'1980-01-01T00:00:00.000' AS DateTime), N'123456789', 1, NULL, NULL, NULL, 162, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, CAST(N'2020-03-14T16:49:23.650' AS DateTime), CAST(N'2020-03-17T16:06:09.000' AS DateTime), 1, N'::1', NULL, N'Malick', NULL)
INSERT [dbo].[User] ([ID], [FirstName], [MiddleName], [LastName], [UserTypeID], [GenderID], [DateOfBirth], [NICNo], [ReligionID], [Address1], [Address2], [ZipCode], [CountryID], [CityID], [StateID], [AcadmicQualification], [Designation], [Salary], [LoginID], [Password], [ImagePath], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [SalaryTypeID], [FatherName], [AccountNumber]) VALUES (12, N'Khalid Mehmood', NULL, NULL, 2, 1, CAST(N'1980-01-01T00:00:00.000' AS DateTime), N'1234567894561', 1, NULL, NULL, NULL, 162, NULL, NULL, NULL, NULL, NULL, N'Khalid', N'123', NULL, 0, CAST(N'2020-03-16T10:26:59.300' AS DateTime), CAST(N'2020-03-17T16:06:05.000' AS DateTime), 1, N'::1', NULL, N'Khalid Memom', NULL)
INSERT [dbo].[User] ([ID], [FirstName], [MiddleName], [LastName], [UserTypeID], [GenderID], [DateOfBirth], [NICNo], [ReligionID], [Address1], [Address2], [ZipCode], [CountryID], [CityID], [StateID], [AcadmicQualification], [Designation], [Salary], [LoginID], [Password], [ImagePath], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [SalaryTypeID], [FatherName], [AccountNumber]) VALUES (13, N'Maqbool Khan', NULL, NULL, 2, 1, CAST(N'1980-01-01T00:00:00.000' AS DateTime), N'1234567894561', 1, NULL, NULL, NULL, 162, NULL, NULL, NULL, NULL, NULL, N'maqbool', N'123', NULL, 0, CAST(N'2020-03-16T11:34:57.890' AS DateTime), CAST(N'2020-03-17T16:06:00.000' AS DateTime), 1, N'::1', NULL, N'Maqbool Ijaz', NULL)
INSERT [dbo].[User] ([ID], [FirstName], [MiddleName], [LastName], [UserTypeID], [GenderID], [DateOfBirth], [NICNo], [ReligionID], [Address1], [Address2], [ZipCode], [CountryID], [CityID], [StateID], [AcadmicQualification], [Designation], [Salary], [LoginID], [Password], [ImagePath], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [SalaryTypeID], [FatherName], [AccountNumber]) VALUES (14, N'Ahad Khan', NULL, NULL, 2, 1, CAST(N'1980-01-01T00:00:00.000' AS DateTime), N'1234567894561', 1, NULL, NULL, NULL, 162, NULL, NULL, NULL, NULL, NULL, N'Ahad', N'123', NULL, 0, CAST(N'2020-03-16T11:52:06.740' AS DateTime), CAST(N'2020-03-17T16:05:55.000' AS DateTime), 1, N'::1', NULL, N'Ahmed Ahad', NULL)
INSERT [dbo].[User] ([ID], [FirstName], [MiddleName], [LastName], [UserTypeID], [GenderID], [DateOfBirth], [NICNo], [ReligionID], [Address1], [Address2], [ZipCode], [CountryID], [CityID], [StateID], [AcadmicQualification], [Designation], [Salary], [LoginID], [Password], [ImagePath], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [SalaryTypeID], [FatherName], [AccountNumber]) VALUES (15, N'Musa Khan', NULL, NULL, 2, 1, CAST(N'1980-01-01T00:00:00.000' AS DateTime), N'1234567894561', 1, NULL, NULL, NULL, 162, NULL, NULL, NULL, NULL, NULL, N'MUSA', N'123', NULL, 0, CAST(N'2020-03-16T12:05:20.613' AS DateTime), CAST(N'2020-03-17T16:05:41.000' AS DateTime), 1, N'::1', NULL, N'Musa Khan', NULL)
INSERT [dbo].[User] ([ID], [FirstName], [MiddleName], [LastName], [UserTypeID], [GenderID], [DateOfBirth], [NICNo], [ReligionID], [Address1], [Address2], [ZipCode], [CountryID], [CityID], [StateID], [AcadmicQualification], [Designation], [Salary], [LoginID], [Password], [ImagePath], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [SalaryTypeID], [FatherName], [AccountNumber]) VALUES (16, N'Samaria', NULL, NULL, 2, 2, CAST(N'1980-01-01T00:00:00.000' AS DateTime), N'1234567894561', 1, NULL, NULL, NULL, 162, NULL, NULL, NULL, NULL, NULL, N'samarria', N'123', NULL, 1, CAST(N'2020-03-16T19:23:29.140' AS DateTime), NULL, 1, N'::1', NULL, N'Samaria', NULL)
INSERT [dbo].[User] ([ID], [FirstName], [MiddleName], [LastName], [UserTypeID], [GenderID], [DateOfBirth], [NICNo], [ReligionID], [Address1], [Address2], [ZipCode], [CountryID], [CityID], [StateID], [AcadmicQualification], [Designation], [Salary], [LoginID], [Password], [ImagePath], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [SalaryTypeID], [FatherName], [AccountNumber]) VALUES (17, N'Junaid Khan', NULL, NULL, 2, 1, CAST(N'1980-01-01T00:00:00.000' AS DateTime), N'1234567894561', 1, NULL, NULL, NULL, 162, NULL, NULL, NULL, NULL, NULL, N'JunaidKhan12', N'123', NULL, 1, CAST(N'2020-03-17T11:14:51.777' AS DateTime), NULL, 1, N'::1', NULL, N'Akbar Khan', NULL)
INSERT [dbo].[User] ([ID], [FirstName], [MiddleName], [LastName], [UserTypeID], [GenderID], [DateOfBirth], [NICNo], [ReligionID], [Address1], [Address2], [ZipCode], [CountryID], [CityID], [StateID], [AcadmicQualification], [Designation], [Salary], [LoginID], [Password], [ImagePath], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [SalaryTypeID], [FatherName], [AccountNumber]) VALUES (18, N'Shahid Afridi', NULL, NULL, 2, 1, CAST(N'1980-01-01T00:00:00.000' AS DateTime), N'1234567894561', 1, NULL, NULL, NULL, 162, NULL, NULL, NULL, NULL, NULL, N'Afridi987', N'987', NULL, 0, CAST(N'2020-03-17T13:16:59.130' AS DateTime), CAST(N'2020-03-17T16:05:49.000' AS DateTime), 1, N'::1', NULL, N'Shahid Afridi', NULL)
INSERT [dbo].[User] ([ID], [FirstName], [MiddleName], [LastName], [UserTypeID], [GenderID], [DateOfBirth], [NICNo], [ReligionID], [Address1], [Address2], [ZipCode], [CountryID], [CityID], [StateID], [AcadmicQualification], [Designation], [Salary], [LoginID], [Password], [ImagePath], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [SalaryTypeID], [FatherName], [AccountNumber]) VALUES (19, N'Humaima Khan', NULL, NULL, 2, 2, CAST(N'1980-01-01T00:00:00.000' AS DateTime), N'1234567894561', 1, NULL, NULL, NULL, 162, NULL, NULL, NULL, NULL, NULL, N'Humaima12', N'123', NULL, 1, CAST(N'2020-03-17T14:34:11.187' AS DateTime), NULL, 1, N'::1', NULL, N'Humaima Khan', NULL)
INSERT [dbo].[User] ([ID], [FirstName], [MiddleName], [LastName], [UserTypeID], [GenderID], [DateOfBirth], [NICNo], [ReligionID], [Address1], [Address2], [ZipCode], [CountryID], [CityID], [StateID], [AcadmicQualification], [Designation], [Salary], [LoginID], [Password], [ImagePath], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [SalaryTypeID], [FatherName], [AccountNumber]) VALUES (20, N'Faisal ', NULL, NULL, 2, 1, CAST(N'1980-01-01T00:00:00.000' AS DateTime), N'1234567894561', 1, NULL, NULL, NULL, 162, NULL, NULL, NULL, NULL, NULL, N'FAISAL1234', N'1234', NULL, 1, CAST(N'2020-03-17T16:04:42.327' AS DateTime), NULL, 1, N'::1', NULL, NULL, NULL)
INSERT [dbo].[User] ([ID], [FirstName], [MiddleName], [LastName], [UserTypeID], [GenderID], [DateOfBirth], [NICNo], [ReligionID], [Address1], [Address2], [ZipCode], [CountryID], [CityID], [StateID], [AcadmicQualification], [Designation], [Salary], [LoginID], [Password], [ImagePath], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [SalaryTypeID], [FatherName], [AccountNumber]) VALUES (21, N'Ahmer Khan', NULL, NULL, 2, 1, CAST(N'1980-01-01T00:00:00.000' AS DateTime), N'1234567894561', 1, NULL, NULL, NULL, 162, NULL, NULL, NULL, NULL, NULL, N'ahmer123', N'123', NULL, 1, CAST(N'2020-03-17T17:08:01.603' AS DateTime), NULL, 1, N'::1', NULL, N'Ahmer Khan', NULL)
INSERT [dbo].[User] ([ID], [FirstName], [MiddleName], [LastName], [UserTypeID], [GenderID], [DateOfBirth], [NICNo], [ReligionID], [Address1], [Address2], [ZipCode], [CountryID], [CityID], [StateID], [AcadmicQualification], [Designation], [Salary], [LoginID], [Password], [ImagePath], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [SalaryTypeID], [FatherName], [AccountNumber]) VALUES (22, N'Saud Bashir', NULL, NULL, 2, 1, CAST(N'1980-01-01T00:00:00.000' AS DateTime), N'1234567894561', 1, NULL, NULL, NULL, 162, NULL, NULL, NULL, NULL, NULL, N'saud123', N'saud', NULL, 1, CAST(N'2020-03-17T17:09:06.420' AS DateTime), NULL, 1, N'::1', NULL, N'Saud Bashir', NULL)
INSERT [dbo].[User] ([ID], [FirstName], [MiddleName], [LastName], [UserTypeID], [GenderID], [DateOfBirth], [NICNo], [ReligionID], [Address1], [Address2], [ZipCode], [CountryID], [CityID], [StateID], [AcadmicQualification], [Designation], [Salary], [LoginID], [Password], [ImagePath], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [SalaryTypeID], [FatherName], [AccountNumber]) VALUES (23, N'Sheraz Hashmi', NULL, NULL, 2, 1, CAST(N'1980-01-01T00:00:00.000' AS DateTime), N'1234567894561', 1, NULL, NULL, NULL, 162, NULL, NULL, NULL, NULL, NULL, N'Sheraz987', N'123', NULL, 1, CAST(N'2020-03-17T18:19:25.977' AS DateTime), NULL, 1, N'::1', NULL, N'Sheraz Hashmi', NULL)
INSERT [dbo].[User] ([ID], [FirstName], [MiddleName], [LastName], [UserTypeID], [GenderID], [DateOfBirth], [NICNo], [ReligionID], [Address1], [Address2], [ZipCode], [CountryID], [CityID], [StateID], [AcadmicQualification], [Designation], [Salary], [LoginID], [Password], [ImagePath], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [SalaryTypeID], [FatherName], [AccountNumber]) VALUES (24, N'Bruce Wayne', NULL, NULL, 2, 1, CAST(N'1980-01-01T00:00:00.000' AS DateTime), N'1234567894561', 1, NULL, NULL, NULL, 162, NULL, NULL, NULL, NULL, NULL, N'bruce12', N'123', NULL, 1, CAST(N'2020-03-17T19:21:05.020' AS DateTime), NULL, 1, N'::1', NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[User] OFF
SET IDENTITY_INSERT [dbo].[UserContact] ON 

INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, 1, 2, N'1234567890', 1, CAST(N'2020-02-10T21:49:32.380' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, 1, 5, N'josh@rigelonic.com', 1, CAST(N'2020-02-10T21:49:32.380' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (3, 2, 5, N'abc@arquam.com', 1, CAST(N'2020-02-10T22:20:25.613' AS DateTime), CAST(N'2020-03-05T16:05:11.160' AS DateTime), 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (4, 2, 2, N'123123123321', 1, CAST(N'2020-02-10T22:20:25.613' AS DateTime), CAST(N'2020-03-05T16:05:11.177' AS DateTime), 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (5, 2, 3, NULL, 1, CAST(N'2020-02-10T22:20:25.613' AS DateTime), CAST(N'2020-03-05T16:05:11.197' AS DateTime), 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (6, 3, 5, N'saeed@abc.com', 1, CAST(N'2020-02-10T22:22:18.213' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (7, 3, 2, N'4654654654654', 1, CAST(N'2020-02-10T22:22:18.217' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (8, 3, 3, NULL, 1, CAST(N'2020-02-10T22:22:18.217' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (9, 4, 5, N'azeez@email.com', 1, CAST(N'2020-02-10T22:23:24.687' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (10, 4, 2, N'1234654321321', 1, CAST(N'2020-02-10T22:23:24.687' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (11, 4, 3, NULL, 1, CAST(N'2020-02-10T22:23:24.687' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (12, 5, 5, N'aqsa@email.com', 1, CAST(N'2020-02-10T22:24:30.563' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (13, 5, 2, N'213213213213', 1, CAST(N'2020-02-10T22:24:30.567' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (14, 5, 3, NULL, 1, CAST(N'2020-02-10T22:24:30.567' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (15, 6, 5, N'javeria@email.com', 1, CAST(N'2020-02-10T22:25:32.407' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (16, 6, 2, N'23424234324', 1, CAST(N'2020-02-10T22:25:32.407' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (17, 6, 3, NULL, 1, CAST(N'2020-02-10T22:25:32.407' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (18, 7, 5, N'saboor@gmail.com', 1, CAST(N'2020-03-03T13:42:20.890' AS DateTime), CAST(N'2020-03-04T17:04:58.117' AS DateTime), 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (19, 7, 2, N'498465465465', 1, CAST(N'2020-03-03T13:42:20.890' AS DateTime), CAST(N'2020-03-04T17:04:58.137' AS DateTime), 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (20, 7, 3, NULL, 1, CAST(N'2020-03-03T13:42:20.893' AS DateTime), CAST(N'2020-03-04T17:04:58.157' AS DateTime), 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (21, 8, 5, N'arqum.123@gmail.com', 1, CAST(N'2020-03-04T13:27:40.890' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (22, 8, 2, N'03412572079', 1, CAST(N'2020-03-04T13:27:40.890' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (23, 8, 3, N'03412572079', 1, CAST(N'2020-03-04T13:27:40.890' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (24, 9, 5, N'abc@abc.com', 1, CAST(N'2020-03-04T15:35:31.843' AS DateTime), CAST(N'2020-03-04T16:54:38.120' AS DateTime), 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (25, 9, 2, N'03412572079', 1, CAST(N'2020-03-04T15:35:31.843' AS DateTime), CAST(N'2020-03-04T16:54:38.147' AS DateTime), 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (26, 9, 3, N'03412572079', 1, CAST(N'2020-03-04T15:35:31.843' AS DateTime), CAST(N'2020-03-04T16:54:38.167' AS DateTime), 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (27, 10, 5, N'hamza.12@gmail.com', 1, CAST(N'2020-03-14T16:32:43.227' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (28, 10, 2, N'03412572079', 1, CAST(N'2020-03-14T16:32:43.227' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (29, 10, 3, N'03412572079', 1, CAST(N'2020-03-14T16:32:43.227' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (30, 11, 5, NULL, 1, CAST(N'2020-03-14T16:49:23.770' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (31, 11, 2, NULL, 1, CAST(N'2020-03-14T16:49:23.770' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (32, 11, 3, NULL, 1, CAST(N'2020-03-14T16:49:23.770' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (33, 12, 5, NULL, 1, CAST(N'2020-03-16T10:26:59.393' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (34, 12, 2, NULL, 1, CAST(N'2020-03-16T10:26:59.393' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (35, 12, 3, NULL, 1, CAST(N'2020-03-16T10:26:59.393' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (36, 13, 5, NULL, 1, CAST(N'2020-03-16T11:34:57.947' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (37, 13, 2, NULL, 1, CAST(N'2020-03-16T11:34:57.950' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (38, 13, 3, NULL, 1, CAST(N'2020-03-16T11:34:57.950' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (39, 14, 5, NULL, 1, CAST(N'2020-03-16T11:52:06.793' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (40, 14, 2, NULL, 1, CAST(N'2020-03-16T11:52:06.793' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (41, 14, 3, NULL, 1, CAST(N'2020-03-16T11:52:06.793' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (42, 15, 5, NULL, 1, CAST(N'2020-03-16T12:05:20.687' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (43, 15, 2, NULL, 1, CAST(N'2020-03-16T12:05:20.690' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (44, 15, 3, NULL, 1, CAST(N'2020-03-16T12:05:20.690' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (45, 16, 5, NULL, 1, CAST(N'2020-03-16T19:23:29.230' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (46, 16, 2, NULL, 1, CAST(N'2020-03-16T19:23:29.230' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (47, 16, 3, NULL, 1, CAST(N'2020-03-16T19:23:29.230' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (48, 17, 5, NULL, 1, CAST(N'2020-03-17T11:14:51.863' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (49, 17, 2, NULL, 1, CAST(N'2020-03-17T11:14:51.863' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (50, 17, 3, NULL, 1, CAST(N'2020-03-17T11:14:51.863' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (51, 18, 5, NULL, 1, CAST(N'2020-03-17T13:16:59.217' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (52, 18, 2, NULL, 1, CAST(N'2020-03-17T13:16:59.217' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (53, 18, 3, NULL, 1, CAST(N'2020-03-17T13:16:59.217' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (54, 19, 5, NULL, 1, CAST(N'2020-03-17T14:34:11.277' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (55, 19, 2, NULL, 1, CAST(N'2020-03-17T14:34:11.277' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (56, 19, 3, NULL, 1, CAST(N'2020-03-17T14:34:11.277' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (57, 20, 5, NULL, 1, CAST(N'2020-03-17T16:04:42.420' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (58, 20, 2, NULL, 1, CAST(N'2020-03-17T16:04:42.420' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (59, 20, 3, NULL, 1, CAST(N'2020-03-17T16:04:42.420' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (60, 21, 5, NULL, 1, CAST(N'2020-03-17T17:08:01.670' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (61, 21, 2, NULL, 1, CAST(N'2020-03-17T17:08:01.673' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (62, 21, 3, NULL, 1, CAST(N'2020-03-17T17:08:01.673' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (63, 22, 5, NULL, 1, CAST(N'2020-03-17T17:09:06.457' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (64, 22, 2, NULL, 1, CAST(N'2020-03-17T17:09:06.457' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (65, 22, 3, NULL, 1, CAST(N'2020-03-17T17:09:06.457' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (66, 23, 5, NULL, 1, CAST(N'2020-03-17T18:19:26.027' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (67, 23, 2, NULL, 1, CAST(N'2020-03-17T18:19:26.027' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (68, 23, 3, NULL, 1, CAST(N'2020-03-17T18:19:26.027' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (69, 24, 5, NULL, 1, CAST(N'2020-03-17T19:21:05.090' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (70, 24, 2, NULL, 1, CAST(N'2020-03-17T19:21:05.093' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (71, 24, 3, NULL, 1, CAST(N'2020-03-17T19:21:05.093' AS DateTime), NULL, 1, N'::1')
SET IDENTITY_INSERT [dbo].[UserContact] OFF
SET IDENTITY_INSERT [dbo].[UserDepartment] ON 

INSERT [dbo].[UserDepartment] ([ID], [UserID], [DepartmentID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, 1, 1, CAST(N'2020-01-01T00:00:00.000' AS DateTime), NULL, CAST(N'2020-02-10T21:51:08.603' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[UserDepartment] ([ID], [UserID], [DepartmentID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, 2, 2, CAST(N'2020-02-10T00:00:00.000' AS DateTime), NULL, CAST(N'2020-02-10T22:20:25.730' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserDepartment] ([ID], [UserID], [DepartmentID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (3, 3, 2, CAST(N'2020-02-10T00:00:00.000' AS DateTime), NULL, CAST(N'2020-02-10T22:22:18.300' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserDepartment] ([ID], [UserID], [DepartmentID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (4, 4, 2, CAST(N'2020-02-10T00:00:00.000' AS DateTime), NULL, CAST(N'2020-02-10T22:23:24.763' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserDepartment] ([ID], [UserID], [DepartmentID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (5, 5, 3, CAST(N'2020-02-10T00:00:00.000' AS DateTime), NULL, CAST(N'2020-02-10T22:24:30.640' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserDepartment] ([ID], [UserID], [DepartmentID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (6, 6, 4, CAST(N'2020-02-10T00:00:00.000' AS DateTime), NULL, CAST(N'2020-02-10T22:25:32.567' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserDepartment] ([ID], [UserID], [DepartmentID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (7, 7, 2, CAST(N'2020-03-03T00:00:00.000' AS DateTime), NULL, CAST(N'2020-03-03T13:42:20.993' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserDepartment] ([ID], [UserID], [DepartmentID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (8, 8, 2, CAST(N'2020-03-04T00:00:00.000' AS DateTime), NULL, CAST(N'2020-03-04T13:27:40.960' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserDepartment] ([ID], [UserID], [DepartmentID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (9, 9, 5, CAST(N'2020-03-04T00:00:00.000' AS DateTime), NULL, CAST(N'2020-03-04T15:35:31.913' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserDepartment] ([ID], [UserID], [DepartmentID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (10, 10, 2, CAST(N'2020-03-14T00:00:00.000' AS DateTime), NULL, CAST(N'2020-03-14T16:32:43.383' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserDepartment] ([ID], [UserID], [DepartmentID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (11, 11, 2, CAST(N'2020-03-14T00:00:00.000' AS DateTime), NULL, CAST(N'2020-03-14T16:49:23.843' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserDepartment] ([ID], [UserID], [DepartmentID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (12, 12, 2, CAST(N'2020-03-16T00:00:00.000' AS DateTime), NULL, CAST(N'2020-03-16T10:26:59.523' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserDepartment] ([ID], [UserID], [DepartmentID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (13, 13, 2, CAST(N'2020-03-16T00:00:00.000' AS DateTime), NULL, CAST(N'2020-03-16T11:34:57.993' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserDepartment] ([ID], [UserID], [DepartmentID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (14, 14, 2, CAST(N'2020-03-16T00:00:00.000' AS DateTime), NULL, CAST(N'2020-03-16T11:52:06.833' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserDepartment] ([ID], [UserID], [DepartmentID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (15, 15, 2, CAST(N'2020-03-16T00:00:00.000' AS DateTime), NULL, CAST(N'2020-03-16T12:05:20.837' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserDepartment] ([ID], [UserID], [DepartmentID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (16, 16, 3, CAST(N'2020-03-16T00:00:00.000' AS DateTime), NULL, CAST(N'2020-03-16T19:23:29.293' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserDepartment] ([ID], [UserID], [DepartmentID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (17, 17, 5, CAST(N'2020-03-17T00:00:00.000' AS DateTime), NULL, CAST(N'2020-03-17T11:14:51.957' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserDepartment] ([ID], [UserID], [DepartmentID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (18, 18, 2, CAST(N'2020-03-17T00:00:00.000' AS DateTime), NULL, CAST(N'2020-03-17T13:16:59.277' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserDepartment] ([ID], [UserID], [DepartmentID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (19, 19, 4, CAST(N'2020-03-17T00:00:00.000' AS DateTime), NULL, CAST(N'2020-03-17T14:34:11.337' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserDepartment] ([ID], [UserID], [DepartmentID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (20, 20, 2, CAST(N'2020-03-17T00:00:00.000' AS DateTime), NULL, CAST(N'2020-03-17T16:04:42.473' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserDepartment] ([ID], [UserID], [DepartmentID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (21, 21, 5, CAST(N'2020-03-17T00:00:00.000' AS DateTime), NULL, CAST(N'2020-03-17T17:08:01.720' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserDepartment] ([ID], [UserID], [DepartmentID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (22, 22, 3, CAST(N'2020-03-17T00:00:00.000' AS DateTime), NULL, CAST(N'2020-03-17T17:09:06.490' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserDepartment] ([ID], [UserID], [DepartmentID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (23, 23, 4, CAST(N'2020-03-17T00:00:00.000' AS DateTime), NULL, CAST(N'2020-03-17T18:19:26.080' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserDepartment] ([ID], [UserID], [DepartmentID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (24, 24, 5, CAST(N'2020-03-17T00:00:00.000' AS DateTime), NULL, CAST(N'2020-03-17T19:21:05.137' AS DateTime), NULL, 1, N'::1')
SET IDENTITY_INSERT [dbo].[UserDepartment] OFF
SET IDENTITY_INSERT [dbo].[UserShift] ON 

INSERT [dbo].[UserShift] ([ID], [UserID], [ShiftID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, 2, 2, CAST(N'2020-01-01T00:00:00.000' AS DateTime), CAST(N'2020-01-15T23:59:59.000' AS DateTime), CAST(N'2020-03-18T11:13:12.900' AS DateTime), CAST(N'2020-03-18T11:14:38.600' AS DateTime), 1, N'::1')
INSERT [dbo].[UserShift] ([ID], [UserID], [ShiftID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, 3, 2, CAST(N'2020-01-01T00:00:00.000' AS DateTime), CAST(N'2020-01-15T23:59:59.000' AS DateTime), CAST(N'2020-03-18T11:13:12.927' AS DateTime), CAST(N'2020-03-18T11:14:38.647' AS DateTime), 1, N'::1')
INSERT [dbo].[UserShift] ([ID], [UserID], [ShiftID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (3, 4, 2, CAST(N'2020-01-01T00:00:00.000' AS DateTime), CAST(N'2020-01-15T23:59:59.000' AS DateTime), CAST(N'2020-03-18T11:13:12.940' AS DateTime), CAST(N'2020-03-18T11:14:38.677' AS DateTime), 1, N'::1')
INSERT [dbo].[UserShift] ([ID], [UserID], [ShiftID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (4, 7, 2, CAST(N'2020-01-01T00:00:00.000' AS DateTime), CAST(N'2020-01-15T23:59:59.000' AS DateTime), CAST(N'2020-03-18T11:13:12.953' AS DateTime), CAST(N'2020-03-18T11:14:38.707' AS DateTime), 1, N'::1')
INSERT [dbo].[UserShift] ([ID], [UserID], [ShiftID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (5, 2, 3, CAST(N'2020-01-16T00:00:00.000' AS DateTime), CAST(N'2020-01-29T23:59:59.000' AS DateTime), CAST(N'2020-03-18T11:14:38.627' AS DateTime), CAST(N'2020-03-18T11:17:58.637' AS DateTime), 1, N'::1')
INSERT [dbo].[UserShift] ([ID], [UserID], [ShiftID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (6, 3, 3, CAST(N'2020-01-16T00:00:00.000' AS DateTime), CAST(N'2020-01-29T23:59:59.000' AS DateTime), CAST(N'2020-03-18T11:14:38.660' AS DateTime), CAST(N'2020-03-18T11:17:58.673' AS DateTime), 1, N'::1')
INSERT [dbo].[UserShift] ([ID], [UserID], [ShiftID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (7, 4, 3, CAST(N'2020-01-16T00:00:00.000' AS DateTime), CAST(N'2020-01-29T23:59:59.000' AS DateTime), CAST(N'2020-03-18T11:14:38.693' AS DateTime), CAST(N'2020-03-18T11:17:58.703' AS DateTime), 1, N'::1')
INSERT [dbo].[UserShift] ([ID], [UserID], [ShiftID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (8, 7, 3, CAST(N'2020-01-16T00:00:00.000' AS DateTime), CAST(N'2020-01-29T23:59:59.000' AS DateTime), CAST(N'2020-03-18T11:14:38.723' AS DateTime), CAST(N'2020-03-18T11:17:58.733' AS DateTime), 1, N'::1')
INSERT [dbo].[UserShift] ([ID], [UserID], [ShiftID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (10, 2, 2, CAST(N'2020-01-30T00:00:00.000' AS DateTime), NULL, CAST(N'2020-03-18T11:17:58.653' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserShift] ([ID], [UserID], [ShiftID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (11, 3, 2, CAST(N'2020-01-30T00:00:00.000' AS DateTime), NULL, CAST(N'2020-03-18T11:17:58.687' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserShift] ([ID], [UserID], [ShiftID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (12, 4, 2, CAST(N'2020-01-30T00:00:00.000' AS DateTime), CAST(N'2020-02-12T23:59:59.000' AS DateTime), CAST(N'2020-03-18T11:17:58.717' AS DateTime), CAST(N'2020-03-18T11:20:41.293' AS DateTime), 1, N'::1')
INSERT [dbo].[UserShift] ([ID], [UserID], [ShiftID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (13, 7, 2, CAST(N'2020-01-30T00:00:00.000' AS DateTime), NULL, CAST(N'2020-03-18T11:17:58.747' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserShift] ([ID], [UserID], [ShiftID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (14, 4, 3, CAST(N'2020-02-13T00:00:00.000' AS DateTime), NULL, CAST(N'2020-03-18T11:20:41.313' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserShift] ([ID], [UserID], [ShiftID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (15, 5, 2, CAST(N'2020-01-01T00:00:00.000' AS DateTime), NULL, CAST(N'2020-03-18T11:22:13.670' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserShift] ([ID], [UserID], [ShiftID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (16, 6, 2, CAST(N'2020-01-01T00:00:00.000' AS DateTime), NULL, CAST(N'2020-03-18T11:22:13.687' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserShift] ([ID], [UserID], [ShiftID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (17, 1, 4, CAST(N'2020-01-01T00:00:00.000' AS DateTime), NULL, CAST(N'2020-03-18T11:22:59.657' AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserShift] ([ID], [UserID], [ShiftID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (18, 16, 2, CAST(N'2020-03-09T00:00:00.000' AS DateTime), NULL, CAST(N'2020-03-18T11:25:17.227' AS DateTime), NULL, 1, N'::1')
SET IDENTITY_INSERT [dbo].[UserShift] OFF
SET IDENTITY_INSERT [dbo].[UserType] ON 

INSERT [dbo].[UserType] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, N'Admin', 1, CAST(N'2016-04-09T20:36:17.963' AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[UserType] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, N'Employee', 1, CAST(N'2016-04-09T20:36:17.963' AS DateTime), NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[UserType] OFF
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
/****** Object:  StoredProcedure [dbo].[GeneratePayroll]    Script Date: 3/24/2020 7:35:46 PM ******/
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
/****** Object:  StoredProcedure [dbo].[insert_manualAttendance]    Script Date: 3/24/2020 7:35:46 PM ******/
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
/****** Object:  StoredProcedure [dbo].[InsertPreAttendance]    Script Date: 3/24/2020 7:35:46 PM ******/
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
/****** Object:  StoredProcedure [dbo].[SELECT_Attendance]    Script Date: 3/24/2020 7:35:46 PM ******/
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
/****** Object:  StoredProcedure [dbo].[SELECT_AttendanceSummary]    Script Date: 3/24/2020 7:35:46 PM ******/
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
/****** Object:  StoredProcedure [dbo].[SELECT_MonthlyAttendanceSummary]    Script Date: 3/24/2020 7:35:46 PM ******/
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
