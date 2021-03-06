USE [master]
GO
/****** Object:  Database [HRManagementSystem]    Script Date: 04/09/2016 20:43:02 ******/
CREATE DATABASE [HRManagementSystem] ON  PRIMARY 
( NAME = N'HRManagementSystem', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL10_50.SQLEXPRESS\MSSQL\DATA\HRManagementSystem.mdf' , SIZE = 2048KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'HRManagementSystem_log', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL10_50.SQLEXPRESS\MSSQL\DATA\HRManagementSystem_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [HRManagementSystem] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [HRManagementSystem].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [HRManagementSystem] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [HRManagementSystem] SET ANSI_NULLS OFF
GO
ALTER DATABASE [HRManagementSystem] SET ANSI_PADDING OFF
GO
ALTER DATABASE [HRManagementSystem] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [HRManagementSystem] SET ARITHABORT OFF
GO
ALTER DATABASE [HRManagementSystem] SET AUTO_CLOSE OFF
GO
ALTER DATABASE [HRManagementSystem] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [HRManagementSystem] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [HRManagementSystem] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [HRManagementSystem] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [HRManagementSystem] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [HRManagementSystem] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [HRManagementSystem] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [HRManagementSystem] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [HRManagementSystem] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [HRManagementSystem] SET  DISABLE_BROKER
GO
ALTER DATABASE [HRManagementSystem] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [HRManagementSystem] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [HRManagementSystem] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [HRManagementSystem] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [HRManagementSystem] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [HRManagementSystem] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [HRManagementSystem] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [HRManagementSystem] SET  READ_WRITE
GO
ALTER DATABASE [HRManagementSystem] SET RECOVERY SIMPLE
GO
ALTER DATABASE [HRManagementSystem] SET  MULTI_USER
GO
ALTER DATABASE [HRManagementSystem] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [HRManagementSystem] SET DB_CHAINING OFF
GO
USE [HRManagementSystem]
GO
/****** Object:  Table [dbo].[UserType]    Script Date: 04/09/2016 20:43:02 ******/
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[UserType] ON
INSERT [dbo].[UserType] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, N'Admin', 1, CAST(0x0000A5E301538F4D AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[UserType] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, N'Employee', 1, CAST(0x0000A5E301538F4D AS DateTime), NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[UserType] OFF
/****** Object:  Table [dbo].[Shift]    Script Date: 04/09/2016 20:43:02 ******/
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
	IsActive [bit] Default(1),
	[CreationDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateBy] [int] NULL,
	[UserIP] [varchar](20) NULL,
 CONSTRAINT [PK_Shift] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Religion]    Script Date: 04/09/2016 20:43:02 ******/
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[Religion] ON
INSERT [dbo].[Religion] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, N'Muslim', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Religion] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, N'Christian', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Religion] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (3, N'Hindu', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Religion] OFF
/****** Object:  Table [dbo].[LeaveType]    Script Date: 04/09/2016 20:43:02 ******/
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Holiday]    Script Date: 04/09/2016 20:43:02 ******/
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Gender]    Script Date: 04/09/2016 20:43:02 ******/
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[Gender] ON
INSERT [dbo].[Gender] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, N'Male', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Gender] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, N'Female', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Gender] OFF
/****** Object:  Table [dbo].[Department]    Script Date: 04/09/2016 20:43:02 ******/
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Country]    Script Date: 04/09/2016 20:43:02 ******/
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[Country] ON
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, N'Afghanistan', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, N'Albania', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (3, N'Algeria', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (4, N'American Samoa', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (5, N'Andorra', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (6, N'Angola', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (7, N'Anguilla', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (8, N'Antarctica', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (9, N'Antigua and Barbuda', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (10, N'Argentina', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (11, N'Armenia', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (12, N'Aruba', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (13, N'Australia', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (14, N'Austria', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (15, N'Azerbaijan', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (16, N'Bahamas', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (17, N'Bahrain', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (18, N'Bangladesh', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (19, N'Barbados', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (20, N'Belarus', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (21, N'Belgium', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (22, N'Belize', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (23, N'Benin', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (24, N'Bermuda', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (25, N'Bhutan', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (26, N'Bolivia', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (27, N'Bosnia and Herzegovina', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (28, N'Botswana', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (29, N'Bouvet Island', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (30, N'Brazil', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (31, N'British Indian Ocean Territory', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (32, N'Brunei Darussalam', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (33, N'Bulgaria', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (34, N'Burkina Faso', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (35, N'Burundi', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (36, N'Cambodia', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (37, N'Cameroon', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (38, N'Canada', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (39, N'Cape Verde', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (40, N'Cayman Islands', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (41, N'Central African Republic', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (42, N'Chad', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (43, N'Chile', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (44, N'China', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (45, N'Christmas Island', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (46, N'Cocos (Keeling) Islands', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (47, N'Colombia', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (48, N'Comoros', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (49, N'Congo', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (50, N'Congo, the Democratic Republic of the', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (51, N'Cook Islands', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (52, N'Costa Rica', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (53, N'Cote D''Ivoire', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (54, N'Croatia', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (55, N'Cuba', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (56, N'Cyprus', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (57, N'Czech Republic', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (58, N'Denmark', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (59, N'Djibouti', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (60, N'Dominica', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (61, N'Dominican Republic', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (62, N'Ecuador', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (63, N'Egypt', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (64, N'El Salvador', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (65, N'Equatorial Guinea', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (66, N'Eritrea', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (67, N'Estonia', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (68, N'Ethiopia', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (69, N'Falkland Islands (Malvinas)', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (70, N'Faroe Islands', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (71, N'Fiji', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (72, N'Finland', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (73, N'France', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (74, N'French Guiana', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (75, N'French Polynesia', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (76, N'French Southern Territories', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (77, N'Gabon', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (78, N'Gambia', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (79, N'Georgia', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (80, N'Germany', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (81, N'Ghana', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (82, N'Gibraltar', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (83, N'Greece', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (84, N'Greenland', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (85, N'Grenada', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (86, N'Guadeloupe', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (87, N'Guam', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (88, N'Guatemala', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (89, N'Guinea', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (90, N'Guinea-Bissau', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (91, N'Guyana', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (92, N'Haiti', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (93, N'Heard Island and Mcdonald Islands', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (94, N'Holy See (Vatican City State)', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (95, N'Honduras', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (96, N'Hong Kong', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (97, N'Hungary', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (98, N'Iceland', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (99, N'India', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (100, N'Indonesia', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
print 'Processed 100 total records'
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (101, N'Iran, Islamic Republic of', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (102, N'Iraq', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (103, N'Ireland', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (104, N'Israel', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (105, N'Italy', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (106, N'Jamaica', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (107, N'Japan', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (108, N'Jordan', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (109, N'Kazakhstan', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (110, N'Kenya', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (111, N'Kiribati', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (112, N'Korea, Democratic People''s Republic of', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (113, N'Korea, Republic of', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (114, N'Kuwait', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (115, N'Kyrgyzstan', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (116, N'Lao People''s Democratic Republic', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (117, N'Latvia', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (118, N'Lebanon', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (119, N'Lesotho', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (120, N'Liberia', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (121, N'Libyan Arab Jamahiriya', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (122, N'Liechtenstein', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (123, N'Lithuania', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (124, N'Luxembourg', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (125, N'Macao', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (126, N'Macedonia, the Former Yugoslav Republic of', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (127, N'Madagascar', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (128, N'Malawi', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (129, N'Malaysia', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (130, N'Maldives', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (131, N'Mali', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (132, N'Malta', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (133, N'Marshall Islands', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (134, N'Martinique', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (135, N'Mauritania', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (136, N'Mauritius', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (137, N'Mayotte', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (138, N'Mexico', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (139, N'Micronesia, Federated States of', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (140, N'Moldova, Republic of', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (141, N'Monaco', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (142, N'Mongolia', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (143, N'Montserrat', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (144, N'Morocco', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (145, N'Mozambique', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (146, N'Myanmar', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (147, N'Namibia', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (148, N'Nauru', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (149, N'Nepal', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (150, N'Netherlands', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (151, N'Netherlands Antilles', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (152, N'New Caledonia', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (153, N'New Zealand', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (154, N'Nicaragua', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (155, N'Niger', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (156, N'Nigeria', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (157, N'Niue', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (158, N'Norfolk Island', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (159, N'Northern Mariana Islands', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (160, N'Norway', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (161, N'Oman', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (162, N'Pakistan', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (163, N'Palau', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (164, N'Palestinian Territory, Occupied', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (165, N'Panama', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (166, N'Papua New Guinea', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (167, N'Paraguay', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (168, N'Peru', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (169, N'Philippines', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (170, N'Pitcairn', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (171, N'Poland', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (172, N'Portugal', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (173, N'Puerto Rico', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (174, N'Qatar', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (175, N'Reunion', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (176, N'Romania', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (177, N'Russian Federation', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (178, N'Rwanda', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (179, N'Saint Helena', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (180, N'Saint Kitts and Nevis', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (181, N'Saint Lucia', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (182, N'Saint Pierre and Miquelon', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (183, N'Saint Vincent and the Grenadines', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (184, N'Samoa', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (185, N'San Marino', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (186, N'Sao Tome and Principe', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (187, N'Saudi Arabia', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (188, N'Senegal', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (189, N'Serbia and Montenegro', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (190, N'Seychelles', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (191, N'Sierra Leone', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (192, N'Singapore', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (193, N'Slovakia', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (194, N'Slovenia', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (195, N'Solomon Islands', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (196, N'Somalia', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (197, N'South Africa', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (198, N'South Georgia and the South Sandwich Islands', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (199, N'Spain', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (200, N'Sri Lanka', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (201, N'Sudan', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
GO
print 'Processed 200 total records'
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (202, N'Suriname', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (203, N'Svalbard and Jan Mayen', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (204, N'Swaziland', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (205, N'Sweden', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (206, N'Switzerland', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (207, N'Syrian Arab Republic', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (208, N'Taiwan, Province of China', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (209, N'Tajikistan', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (210, N'Tanzania, United Republic of', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (211, N'Thailand', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (212, N'Timor-Leste', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (213, N'Togo', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (214, N'Tokelau', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (215, N'Tonga', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (216, N'Trinidad and Tobago', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (217, N'Tunisia', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (218, N'Turkey', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (219, N'Turkmenistan', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (220, N'Turks and Caicos Islands', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (221, N'Tuvalu', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (222, N'Uganda', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (223, N'Ukraine', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (224, N'United Arab Emirates', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (225, N'United Kingdom', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (226, N'United States', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (227, N'United States Minor Outlying Islands', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (228, N'Uruguay', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (229, N'Uzbekistan', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (230, N'Vanuatu', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (231, N'Venezuela', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (232, N'Viet Nam', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (233, N'Virgin Islands, British', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (234, N'Virgin Islands, U.s.', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (235, N'Wallis and Futuna', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (236, N'Western Sahara', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (237, N'Yemen', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (238, N'Zambia', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Country] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (239, N'Zimbabwe', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Country] OFF
/****** Object:  Table [dbo].[ContactType]    Script Date: 04/09/2016 20:43:02 ******/
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[ContactType] ON
INSERT [dbo].[ContactType] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, N'Landline', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[ContactType] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, N'Mobile Number', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[ContactType] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (3, N'Alternate Mobile Number', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[ContactType] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (4, N'Emergency Mobile Number', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[ContactType] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (5, N'Email Address', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[ContactType] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (6, N'Alternate Email Address', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[ContactType] OFF
/****** Object:  Table [dbo].[AttendanceVariable]    Script Date: 04/09/2016 20:43:02 ******/
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[AttendanceVariable] ON
INSERT [dbo].[AttendanceVariable] ([ID], [Name], [Description], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, N'Quarter Day', N'Quarter Day', 1, CAST(0x0000A5E301538F4D AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[AttendanceVariable] ([ID], [Name], [Description], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, N'Half Day', N'Half Day', 1, CAST(0x0000A5E301538F4D AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[AttendanceVariable] ([ID], [Name], [Description], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (3, N'Full Day', N'Full Day', 1, CAST(0x0000A5E301538F4D AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[AttendanceVariable] ([ID], [Name], [Description], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (4, N'Over Time', N'Over Time', 1, CAST(0x0000A5E301538F4D AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[AttendanceVariable] ([ID], [Name], [Description], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (5, N'Compensatory Hours', N'Compensatory Hours', 1, CAST(0x0000A5E301538F4D AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[AttendanceVariable] ([ID], [Name], [Description], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (6, N'Standard Hours', N'Standard Hours', 1, CAST(0x0000A5E301538F4D AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[AttendanceVariable] ([ID], [Name], [Description], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (7, N'Early Hours', N'Early Hours', 1, CAST(0x0000A5E301538F4D AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[AttendanceVariable] ([ID], [Name], [Description], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (8, N'Late Hours', N'Late Hours', 1, CAST(0x0000A5E301538F4D AS DateTime), NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[AttendanceVariable] OFF
/****** Object:  Table [dbo].[AttendanceType]    Script Date: 04/09/2016 20:43:02 ******/
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[AttendanceType] ON
INSERT [dbo].[AttendanceType] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, N'Daily Attendance', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[AttendanceType] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, N'Lunch', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[AttendanceType] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (3, N'Tea', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[AttendanceType] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (4, N'Prayers', 1, CAST(0x0000A5E301538F49 AS DateTime), NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[AttendanceType] OFF
/****** Object:  Table [dbo].[AttendancePolicy]    Script Date: 04/09/2016 20:43:02 ******/
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
	[Hours] [int] NULL,
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[State]    Script Date: 04/09/2016 20:43:02 ******/
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ShiftOffDay]    Script Date: 04/09/2016 20:43:02 ******/
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[City]    Script Date: 04/09/2016 20:43:02 ******/
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[User]    Script Date: 04/09/2016 20:43:02 ******/
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
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserShift]    Script Date: 04/09/2016 20:43:03 ******/
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserDepartment]    Script Date: 04/09/2016 20:43:03 ******/
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserContact]    Script Date: 04/09/2016 20:43:03 ******/
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Attendance]    Script Date: 04/09/2016 20:43:03 ******/
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
 CONSTRAINT [PK_Attendance] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AttendanceStatus]    Script Date: 04/09/2016 20:43:03 ******/
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
 CONSTRAINT [PK_AttendanceStatus] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AttendanceDetail]    Script Date: 04/09/2016 20:43:03 ******/
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Default [DF_UserType_IsActive]    Script Date: 04/09/2016 20:43:02 ******/
ALTER TABLE [dbo].[UserType] ADD  CONSTRAINT [DF_UserType_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
/****** Object:  Default [DF_UserType_CreationDate]    Script Date: 04/09/2016 20:43:02 ******/
ALTER TABLE [dbo].[UserType] ADD  CONSTRAINT [DF_UserType_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
/****** Object:  Default [DF_Shift_CreationDate]    Script Date: 04/09/2016 20:43:02 ******/
ALTER TABLE [dbo].[Shift] ADD  CONSTRAINT [DF_Shift_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
/****** Object:  Default [DF_Religion_IsActive]    Script Date: 04/09/2016 20:43:02 ******/
ALTER TABLE [dbo].[Religion] ADD  CONSTRAINT [DF_Religion_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
/****** Object:  Default [DF_Religion_CreationDate]    Script Date: 04/09/2016 20:43:02 ******/
ALTER TABLE [dbo].[Religion] ADD  CONSTRAINT [DF_Religion_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
/****** Object:  Default [DF_LeaveType_IsActive]    Script Date: 04/09/2016 20:43:02 ******/
ALTER TABLE [dbo].[LeaveType] ADD  CONSTRAINT [DF_LeaveType_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
/****** Object:  Default [DF_LeaveType_CreationDate]    Script Date: 04/09/2016 20:43:02 ******/
ALTER TABLE [dbo].[LeaveType] ADD  CONSTRAINT [DF_LeaveType_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
/****** Object:  Default [DF_Holiday_IsActive]    Script Date: 04/09/2016 20:43:02 ******/
ALTER TABLE [dbo].[Holiday] ADD  CONSTRAINT [DF_Holiday_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
/****** Object:  Default [DF_Holiday_CreationDate]    Script Date: 04/09/2016 20:43:02 ******/
ALTER TABLE [dbo].[Holiday] ADD  CONSTRAINT [DF_Holiday_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
/****** Object:  Default [DF_Gender_IsActive]    Script Date: 04/09/2016 20:43:02 ******/
ALTER TABLE [dbo].[Gender] ADD  CONSTRAINT [DF_Gender_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
/****** Object:  Default [DF_Gender_CreationDate]    Script Date: 04/09/2016 20:43:02 ******/
ALTER TABLE [dbo].[Gender] ADD  CONSTRAINT [DF_Gender_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
/****** Object:  Default [DF_Department_IsActive]    Script Date: 04/09/2016 20:43:02 ******/
ALTER TABLE [dbo].[Department] ADD  CONSTRAINT [DF_Department_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
/****** Object:  Default [DF_Department_CreationDate]    Script Date: 04/09/2016 20:43:02 ******/
ALTER TABLE [dbo].[Department] ADD  CONSTRAINT [DF_Department_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
/****** Object:  Default [DF_Country_IsActive]    Script Date: 04/09/2016 20:43:02 ******/
ALTER TABLE [dbo].[Country] ADD  CONSTRAINT [DF_Country_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
/****** Object:  Default [DF_Country_CreationDate]    Script Date: 04/09/2016 20:43:02 ******/
ALTER TABLE [dbo].[Country] ADD  CONSTRAINT [DF_Country_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
/****** Object:  Default [DF_ContactType_IsActive]    Script Date: 04/09/2016 20:43:02 ******/
ALTER TABLE [dbo].[ContactType] ADD  CONSTRAINT [DF_ContactType_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
/****** Object:  Default [DF_ContactType_CreationDate]    Script Date: 04/09/2016 20:43:02 ******/
ALTER TABLE [dbo].[ContactType] ADD  CONSTRAINT [DF_ContactType_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
/****** Object:  Default [DF_AttendanceVariable_IsActive]    Script Date: 04/09/2016 20:43:02 ******/
ALTER TABLE [dbo].[AttendanceVariable] ADD  CONSTRAINT [DF_AttendanceVariable_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
/****** Object:  Default [DF_AttendanceVariable_CreationDate]    Script Date: 04/09/2016 20:43:02 ******/
ALTER TABLE [dbo].[AttendanceVariable] ADD  CONSTRAINT [DF_AttendanceVariable_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
/****** Object:  Default [DF_AttendanceType_IsActive]    Script Date: 04/09/2016 20:43:02 ******/
ALTER TABLE [dbo].[AttendanceType] ADD  CONSTRAINT [DF_AttendanceType_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
/****** Object:  Default [DF_AttendanceType_CreationDate]    Script Date: 04/09/2016 20:43:02 ******/
ALTER TABLE [dbo].[AttendanceType] ADD  CONSTRAINT [DF_AttendanceType_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
/****** Object:  Default [DF_AttendancePolicy_CreationDate]    Script Date: 04/09/2016 20:43:02 ******/
ALTER TABLE [dbo].[AttendancePolicy] ADD  CONSTRAINT [DF_AttendancePolicy_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
/****** Object:  Default [DF_State_IsActive]    Script Date: 04/09/2016 20:43:02 ******/
ALTER TABLE [dbo].[State] ADD  CONSTRAINT [DF_State_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
/****** Object:  Default [DF_State_CreationDate]    Script Date: 04/09/2016 20:43:02 ******/
ALTER TABLE [dbo].[State] ADD  CONSTRAINT [DF_State_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
/****** Object:  Default [DF_ShiftOffDay_CreationDate]    Script Date: 04/09/2016 20:43:02 ******/
ALTER TABLE [dbo].[ShiftOffDay] ADD  CONSTRAINT [DF_ShiftOffDay_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
/****** Object:  Default [DF_City_IsActive]    Script Date: 04/09/2016 20:43:02 ******/
ALTER TABLE [dbo].[City] ADD  CONSTRAINT [DF_City_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
/****** Object:  Default [DF_City_CreationDate]    Script Date: 04/09/2016 20:43:02 ******/
ALTER TABLE [dbo].[City] ADD  CONSTRAINT [DF_City_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
/****** Object:  Default [DF_User_IsActive]    Script Date: 04/09/2016 20:43:03 ******/
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
/****** Object:  Default [DF_User_CreationDate]    Script Date: 04/09/2016 20:43:03 ******/
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
/****** Object:  Default [DF_UserShift_CreationDate]    Script Date: 04/09/2016 20:43:03 ******/
ALTER TABLE [dbo].[UserShift] ADD  CONSTRAINT [DF_UserShift_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
/****** Object:  Default [DF_UserDepartment_CreationDate]    Script Date: 04/09/2016 20:43:03 ******/
ALTER TABLE [dbo].[UserDepartment] ADD  CONSTRAINT [DF_UserDepartment_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
/****** Object:  Default [DF_UserContact_IsActive]    Script Date: 04/09/2016 20:43:03 ******/
ALTER TABLE [dbo].[UserContact] ADD  CONSTRAINT [DF_UserContact_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
/****** Object:  Default [DF_UserContact_CreationDate]    Script Date: 04/09/2016 20:43:03 ******/
ALTER TABLE [dbo].[UserContact] ADD  CONSTRAINT [DF_UserContact_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
/****** Object:  Default [DF_Attendance_IsActive]    Script Date: 04/09/2016 20:43:03 ******/
ALTER TABLE [dbo].[Attendance] ADD  CONSTRAINT [DF_Attendance_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
/****** Object:  Default [DF_Attendance_CreationDate]    Script Date: 04/09/2016 20:43:03 ******/
ALTER TABLE [dbo].[Attendance] ADD  CONSTRAINT [DF_Attendance_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
/****** Object:  Default [DF_AttendanceStatus_IsActive]    Script Date: 04/09/2016 20:43:03 ******/
ALTER TABLE [dbo].[AttendanceStatus] ADD  CONSTRAINT [DF_AttendanceStatus_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
/****** Object:  Default [DF_AttendanceStatus_CreationDate]    Script Date: 04/09/2016 20:43:03 ******/
ALTER TABLE [dbo].[AttendanceStatus] ADD  CONSTRAINT [DF_AttendanceStatus_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
/****** Object:  Default [DF_AttendanceDetail_IsActive]    Script Date: 04/09/2016 20:43:03 ******/
ALTER TABLE [dbo].[AttendanceDetail] ADD  CONSTRAINT [DF_AttendanceDetail_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
/****** Object:  Default [DF_AttendanceDetail_CreationDate]    Script Date: 04/09/2016 20:43:03 ******/
ALTER TABLE [dbo].[AttendanceDetail] ADD  CONSTRAINT [DF_AttendanceDetail_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
/****** Object:  ForeignKey [fk_Id_AttendancePolicyAttendanceVariable]    Script Date: 04/09/2016 20:43:02 ******/
ALTER TABLE [dbo].[AttendancePolicy]  WITH CHECK ADD  CONSTRAINT [fk_Id_AttendancePolicyAttendanceVariable] FOREIGN KEY([AttendanceVariableID])
REFERENCES [dbo].[AttendanceVariable] ([ID])
GO
ALTER TABLE [dbo].[AttendancePolicy] CHECK CONSTRAINT [fk_Id_AttendancePolicyAttendanceVariable]
GO
/****** Object:  ForeignKey [fk_Id_AttendancePolicyShift]    Script Date: 04/09/2016 20:43:02 ******/
ALTER TABLE [dbo].[AttendancePolicy]  WITH CHECK ADD  CONSTRAINT [fk_Id_AttendancePolicyShift] FOREIGN KEY([ShiftID])
REFERENCES [dbo].[Shift] ([ID])
GO
ALTER TABLE [dbo].[AttendancePolicy] CHECK CONSTRAINT [fk_Id_AttendancePolicyShift]
GO
/****** Object:  ForeignKey [fk_Id_StateCountry]    Script Date: 04/09/2016 20:43:02 ******/
ALTER TABLE [dbo].[State]  WITH CHECK ADD  CONSTRAINT [fk_Id_StateCountry] FOREIGN KEY([CountryID])
REFERENCES [dbo].[Country] ([ID])
GO
ALTER TABLE [dbo].[State] CHECK CONSTRAINT [fk_Id_StateCountry]
GO
/****** Object:  ForeignKey [fk_Id_ShiftOffDayShift]    Script Date: 04/09/2016 20:43:02 ******/
ALTER TABLE [dbo].[ShiftOffDay]  WITH CHECK ADD  CONSTRAINT [fk_Id_ShiftOffDayShift] FOREIGN KEY([ShiftID])
REFERENCES [dbo].[Shift] ([ID])
GO
ALTER TABLE [dbo].[ShiftOffDay] CHECK CONSTRAINT [fk_Id_ShiftOffDayShift]
GO
/****** Object:  ForeignKey [fk_Id_CityState]    Script Date: 04/09/2016 20:43:02 ******/
ALTER TABLE [dbo].[City]  WITH CHECK ADD  CONSTRAINT [fk_Id_CityState] FOREIGN KEY([StateID])
REFERENCES [dbo].[State] ([ID])
GO
ALTER TABLE [dbo].[City] CHECK CONSTRAINT [fk_Id_CityState]
GO
/****** Object:  ForeignKey [fk_Id_UserCity]    Script Date: 04/09/2016 20:43:02 ******/
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [fk_Id_UserCity] FOREIGN KEY([CityID])
REFERENCES [dbo].[City] ([ID])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [fk_Id_UserCity]
GO
/****** Object:  ForeignKey [fk_Id_UserCountry]    Script Date: 04/09/2016 20:43:02 ******/
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [fk_Id_UserCountry] FOREIGN KEY([CountryID])
REFERENCES [dbo].[Country] ([ID])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [fk_Id_UserCountry]
GO
/****** Object:  ForeignKey [fk_Id_UserGender]    Script Date: 04/09/2016 20:43:02 ******/
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [fk_Id_UserGender] FOREIGN KEY([GenderID])
REFERENCES [dbo].[Gender] ([ID])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [fk_Id_UserGender]
GO
/****** Object:  ForeignKey [fk_Id_UserReligion]    Script Date: 04/09/2016 20:43:02 ******/
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [fk_Id_UserReligion] FOREIGN KEY([ReligionID])
REFERENCES [dbo].[Religion] ([ID])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [fk_Id_UserReligion]
GO
/****** Object:  ForeignKey [fk_Id_UserState]    Script Date: 04/09/2016 20:43:02 ******/
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [fk_Id_UserState] FOREIGN KEY([StateID])
REFERENCES [dbo].[State] ([ID])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [fk_Id_UserState]
GO
/****** Object:  ForeignKey [fk_Id_UserUserType]    Script Date: 04/09/2016 20:43:03 ******/
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [fk_Id_UserUserType] FOREIGN KEY([UserTypeID])
REFERENCES [dbo].[UserType] ([ID])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [fk_Id_UserUserType]
GO
/****** Object:  ForeignKey [fk_Id_UserShiftShift]    Script Date: 04/09/2016 20:43:03 ******/
ALTER TABLE [dbo].[UserShift]  WITH CHECK ADD  CONSTRAINT [fk_Id_UserShiftShift] FOREIGN KEY([ShiftID])
REFERENCES [dbo].[Shift] ([ID])
GO
ALTER TABLE [dbo].[UserShift] CHECK CONSTRAINT [fk_Id_UserShiftShift]
GO
/****** Object:  ForeignKey [fk_Id_UserShiftUser]    Script Date: 04/09/2016 20:43:03 ******/
ALTER TABLE [dbo].[UserShift]  WITH CHECK ADD  CONSTRAINT [fk_Id_UserShiftUser] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([ID])
GO
ALTER TABLE [dbo].[UserShift] CHECK CONSTRAINT [fk_Id_UserShiftUser]
GO
/****** Object:  ForeignKey [fk_Id_UserDepartmentDepartment]    Script Date: 04/09/2016 20:43:03 ******/
ALTER TABLE [dbo].[UserDepartment]  WITH CHECK ADD  CONSTRAINT [fk_Id_UserDepartmentDepartment] FOREIGN KEY([DepartmentID])
REFERENCES [dbo].[Department] ([ID])
GO
ALTER TABLE [dbo].[UserDepartment] CHECK CONSTRAINT [fk_Id_UserDepartmentDepartment]
GO
/****** Object:  ForeignKey [fk_Id_UserDepartmentUser]    Script Date: 04/09/2016 20:43:03 ******/
ALTER TABLE [dbo].[UserDepartment]  WITH CHECK ADD  CONSTRAINT [fk_Id_UserDepartmentUser] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([ID])
GO
ALTER TABLE [dbo].[UserDepartment] CHECK CONSTRAINT [fk_Id_UserDepartmentUser]
GO
/****** Object:  ForeignKey [fk_Id_UserContactContactType]    Script Date: 04/09/2016 20:43:03 ******/
ALTER TABLE [dbo].[UserContact]  WITH CHECK ADD  CONSTRAINT [fk_Id_UserContactContactType] FOREIGN KEY([ContactTypeID])
REFERENCES [dbo].[ContactType] ([ID])
GO
ALTER TABLE [dbo].[UserContact] CHECK CONSTRAINT [fk_Id_UserContactContactType]
GO
/****** Object:  ForeignKey [fk_Id_UserContactUser]    Script Date: 04/09/2016 20:43:03 ******/
ALTER TABLE [dbo].[UserContact]  WITH CHECK ADD  CONSTRAINT [fk_Id_UserContactUser] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([ID])
GO
ALTER TABLE [dbo].[UserContact] CHECK CONSTRAINT [fk_Id_UserContactUser]
GO
/****** Object:  ForeignKey [fk_Id_AttendanceUser]    Script Date: 04/09/2016 20:43:03 ******/
ALTER TABLE [dbo].[Attendance]  WITH CHECK ADD  CONSTRAINT [fk_Id_AttendanceUser] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([ID])
GO
ALTER TABLE [dbo].[Attendance] CHECK CONSTRAINT [fk_Id_AttendanceUser]
GO
/****** Object:  ForeignKey [fk_Id_AttendanceStatusAttendance]    Script Date: 04/09/2016 20:43:03 ******/
ALTER TABLE [dbo].[AttendanceStatus]  WITH CHECK ADD  CONSTRAINT [fk_Id_AttendanceStatusAttendance] FOREIGN KEY([AttendanceID])
REFERENCES [dbo].[Attendance] ([ID])
GO
ALTER TABLE [dbo].[AttendanceStatus] CHECK CONSTRAINT [fk_Id_AttendanceStatusAttendance]
GO
/****** Object:  ForeignKey [fk_Id_AttendanceDetailAttendance]    Script Date: 04/09/2016 20:43:03 ******/
ALTER TABLE [dbo].[AttendanceDetail]  WITH CHECK ADD  CONSTRAINT [fk_Id_AttendanceDetailAttendance] FOREIGN KEY([AttendanceID])
REFERENCES [dbo].[Attendance] ([ID])
GO
ALTER TABLE [dbo].[AttendanceDetail] CHECK CONSTRAINT [fk_Id_AttendanceDetailAttendance]
GO
/****** Object:  ForeignKey [fk_Id_AttendanceDetailAttendanceType]    Script Date: 04/09/2016 20:43:03 ******/
ALTER TABLE [dbo].[AttendanceDetail]  WITH CHECK ADD  CONSTRAINT [fk_Id_AttendanceDetailAttendanceType] FOREIGN KEY([AttendanceTypeID])
REFERENCES [dbo].[AttendanceType] ([ID])
GO
ALTER TABLE [dbo].[AttendanceDetail] CHECK CONSTRAINT [fk_Id_AttendanceDetailAttendanceType]
GO
