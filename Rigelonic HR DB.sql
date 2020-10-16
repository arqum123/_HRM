--------------------------------------------------------------
--                      CREATE DB                           --
--------------------------------------------------------------

USE [master]
GO

/****** Object:  Database [RigelonicHR]    Script Date: 03/04/2020 11:47:57 ******/
CREATE DATABASE [RigelonicHR] ON  PRIMARY 
( NAME = N'RigelonicHR', FILENAME = N'D:\Work\G2\Company\Rigelionic\DB\RigelonicHR.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'RigelonicHR_log', FILENAME = N'D:\Work\G2\Company\Rigelionic\DB\RigelonicHR_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO

ALTER DATABASE [RigelonicHR] SET COMPATIBILITY_LEVEL = 100
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [RigelonicHR].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [RigelonicHR] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [RigelonicHR] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [RigelonicHR] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [RigelonicHR] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [RigelonicHR] SET ARITHABORT OFF 
GO

ALTER DATABASE [RigelonicHR] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [RigelonicHR] SET AUTO_CREATE_STATISTICS ON 
GO

ALTER DATABASE [RigelonicHR] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [RigelonicHR] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [RigelonicHR] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [RigelonicHR] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [RigelonicHR] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [RigelonicHR] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [RigelonicHR] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [RigelonicHR] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [RigelonicHR] SET  DISABLE_BROKER 
GO

ALTER DATABASE [RigelonicHR] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [RigelonicHR] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [RigelonicHR] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [RigelonicHR] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [RigelonicHR] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [RigelonicHR] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [RigelonicHR] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [RigelonicHR] SET  READ_WRITE 
GO

ALTER DATABASE [RigelonicHR] SET RECOVERY FULL 
GO

ALTER DATABASE [RigelonicHR] SET  MULTI_USER 
GO

ALTER DATABASE [RigelonicHR] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [RigelonicHR] SET DB_CHAINING OFF 
GO








--------------------------------------------------------------
--                     CREATE OBJ                           --
--------------------------------------------------------------



USE [RigelonicHR]
GO
/****** Object:  Table [dbo].[Department]    Script Date: 03/04/2020 11:47:39 ******/
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
SET IDENTITY_INSERT [dbo].[Department] ON
INSERT [dbo].[Department] ([ID], [Name], [Description], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, N'Head', NULL, 1, CAST(0x0000A648011CBAE3 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Department] ([ID], [Name], [Description], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, N'Software', N'Software developers, Website developers', 1, CAST(0x0000AB5D016F04DF AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[Department] ([ID], [Name], [Description], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (3, N'Graphics', N'Illustrator, designer, editor', 1, CAST(0x0000AB5D016F24D6 AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[Department] ([ID], [Name], [Description], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (4, N'Content', N'Creative writers', 1, CAST(0x0000AB5D016F38D4 AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[Department] ([ID], [Name], [Description], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (5, N'Sales', N'Sales Agent, Support Agents', 1, CAST(0x0000AB5D016F4991 AS DateTime), NULL, 1, N'::1')
SET IDENTITY_INSERT [dbo].[Department] OFF
/****** Object:  Table [dbo].[Country]    Script Date: 03/04/2020 11:47:39 ******/
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
/****** Object:  Table [dbo].[ContactType]    Script Date: 03/04/2020 11:47:39 ******/
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
/****** Object:  Table [dbo].[Configuration]    Script Date: 03/04/2020 11:47:39 ******/
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
SET IDENTITY_INSERT [dbo].[Configuration] ON
INSERT [dbo].[Configuration] ([ID], [Name], [Type], [Value], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, N'SERVICE STATUS', N'BOOLEAN', N'1', 1, CAST(0x0000A5F5005515A4 AS DateTime), CAST(0x0000A6CD00CAE210 AS DateTime), 5000, N'::1')
INSERT [dbo].[Configuration] ([ID], [Name], [Type], [Value], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, N'PreAttendance', N'DATETIME', N'Dec 18 2016 11:00PM', 0, CAST(0x0000A62B0103B09A AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Configuration] ([ID], [Name], [Type], [Value], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (3, N'ALTERNATE ATTENDANCE', N'BOOLEAN', N'1', 1, CAST(0x0000A62D0116FBEC AS DateTime), CAST(0x0000A6CD00CAE211 AS DateTime), 5000, N'::1')
SET IDENTITY_INSERT [dbo].[Configuration] OFF
/****** Object:  Table [dbo].[Holiday]    Script Date: 03/04/2020 11:47:39 ******/
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
SET IDENTITY_INSERT [dbo].[Holiday] ON
INSERT [dbo].[Holiday] ([ID], [Name], [Date], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, N'Independance day', CAST(0x0000A66200000000 AS DateTime), N'Pakistan Independance day', 1, CAST(0x0000A6480125DC7B AS DateTime), NULL, 5000, N'::1')
SET IDENTITY_INSERT [dbo].[Holiday] OFF
/****** Object:  UserDefinedFunction [dbo].[GetName]    Script Date: 03/04/2020 11:47:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[GetName] 
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
/****** Object:  Table [dbo].[Gender]    Script Date: 03/04/2020 11:47:39 ******/
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
INSERT [dbo].[Gender] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, N'Male', 1, CAST(0x0000AB5D015F6F9F AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[Gender] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, N'Female', 1, CAST(0x0000AB5D015F6F9F AS DateTime), NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Gender] OFF
/****** Object:  Table [dbo].[DeviceModal]    Script Date: 03/04/2020 11:47:39 ******/
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SalaryType]    Script Date: 03/04/2020 11:47:39 ******/
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
SET IDENTITY_INSERT [dbo].[SalaryType] ON
INSERT [dbo].[SalaryType] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, N'Monthly Salary', 1, CAST(0x0000A64801192A8B AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[SalaryType] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, N'Weekly Salary', 0, CAST(0x0000A64801192A8B AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[SalaryType] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (3, N'Daily Salary', 1, CAST(0x0000A64801192A8B AS DateTime), NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[SalaryType] OFF
/****** Object:  Table [dbo].[Religion]    Script Date: 03/04/2020 11:47:39 ******/
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
/****** Object:  Table [dbo].[PayrollVariable]    Script Date: 03/04/2020 11:47:39 ******/
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[PayrollVariable] ON
INSERT [dbo].[PayrollVariable] ([ID], [Name], [IsDeduction], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, N'Quarter Day', 1, 1, CAST(0x0000AB5D0174B453 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[PayrollVariable] ([ID], [Name], [IsDeduction], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, N'Half Day', 1, 1, CAST(0x0000AB5D0174B453 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[PayrollVariable] ([ID], [Name], [IsDeduction], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (3, N'Full Day', 1, 1, CAST(0x0000AB5D0174B453 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[PayrollVariable] ([ID], [Name], [IsDeduction], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (4, N'Absent', 1, 1, CAST(0x0000AB5D0174B453 AS DateTime), NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[PayrollVariable] OFF
/****** Object:  Table [dbo].[OG_LeaveType]    Script Date: 03/04/2020 11:47:39 ******/
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
SET IDENTITY_INSERT [dbo].[OG_LeaveType] ON
INSERT [dbo].[OG_LeaveType] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdatedBy], [UserIP]) VALUES (1, N'Casual', 1, CAST(0x0000A73500BBFD7F AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[OG_LeaveType] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdatedBy], [UserIP]) VALUES (2, N'Sick', 1, CAST(0x0000A73500BBFD7F AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[OG_LeaveType] ([ID], [Name], [IsActive], [CreationDate], [UpdateDate], [UpdatedBy], [UserIP]) VALUES (3, N'Planned', 1, CAST(0x0000A73500BBFD7F AS DateTime), NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[OG_LeaveType] OFF
/****** Object:  Table [dbo].[LeaveType]    Script Date: 03/04/2020 11:47:39 ******/
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
SET IDENTITY_INSERT [dbo].[LeaveType] ON
INSERT [dbo].[LeaveType] ([ID], [Name], [YearlyLeaves], [PriorDays], [MaxDays], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, N'Casual', 10, 5, 5, 1, CAST(0x0000A73500BCF9CA AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[LeaveType] ([ID], [Name], [YearlyLeaves], [PriorDays], [MaxDays], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, N'Sick', 10, 5, 5, 1, CAST(0x0000A73500BCF9CA AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[LeaveType] ([ID], [Name], [YearlyLeaves], [PriorDays], [MaxDays], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (3, N'Planned', 10, 5, 5, 1, CAST(0x0000A73500BCF9CF AS DateTime), NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[LeaveType] OFF
/****** Object:  Table [dbo].[PayrollCycle]    Script Date: 03/04/2020 11:47:39 ******/
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[PayrollCycle] ON
INSERT [dbo].[PayrollCycle] ([ID], [Name], [Month], [Year], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, N'Mar 2020 Payroll', 3, 2020, 1, CAST(0x0000AB7300E4C064 AS DateTime), NULL, 1, N'::1')
SET IDENTITY_INSERT [dbo].[PayrollCycle] OFF
/****** Object:  Table [dbo].[Branch]    Script Date: 03/04/2020 11:47:39 ******/
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[Branch] ON
INSERT [dbo].[Branch] ([ID], [Name], [AddressLine], [CityID], [StateID], [CountryID], [ZipCode], [PhoneNumber], [GUID], [CreatedDate], [UpdationDate], [UpdatedBy], [UserIP], [IsActive]) VALUES (1, N'21D-201', NULL, NULL, NULL, NULL, NULL, NULL, N'd02caa04afba49688db5d4218800873f', CAST(0x0000AB5D0160067F AS DateTime), NULL, NULL, N'::1', 1)
SET IDENTITY_INSERT [dbo].[Branch] OFF
/****** Object:  Table [dbo].[AttendanceVariable]    Script Date: 03/04/2020 11:47:39 ******/
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
INSERT [dbo].[AttendanceVariable] ([ID], [Name], [Description], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, N'Full Day', N'', 1, CAST(0x0000AB5D0174B079 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[AttendanceVariable] ([ID], [Name], [Description], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, N'Half Day', N'', 1, CAST(0x0000AB5D0174E472 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[AttendanceVariable] ([ID], [Name], [Description], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (4, N'Overtime', N'', 1, CAST(0x0000AB5D0174B079 AS DateTime), NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[AttendanceVariable] OFF
/****** Object:  Table [dbo].[AttendanceType]    Script Date: 03/04/2020 11:47:39 ******/
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
/****** Object:  Table [dbo].[Shift]    Script Date: 03/04/2020 11:47:39 ******/
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
 CONSTRAINT [PK_Shift] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[Shift] ON
INSERT [dbo].[Shift] ([ID], [Name], [Description], [StartHour], [EndHour], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (3, N'Morning 10', N'10 am to 7 pm', N'10:00', N'19:00', 1, CAST(0x0000AB5D0171E248 AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[Shift] ([ID], [Name], [Description], [StartHour], [EndHour], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (4, N'Evening 7', N'7 pm to 4 am', N'19:00', N'4:00', 1, CAST(0x0000AB7300E2CD34 AS DateTime), NULL, 1, N'::1')
SET IDENTITY_INSERT [dbo].[Shift] OFF
/****** Object:  Table [dbo].[UserType]    Script Date: 03/04/2020 11:47:39 ******/
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
/****** Object:  Table [dbo].[tmp]    Script Date: 03/04/2020 11:47:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING OFF
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
/****** Object:  Table [dbo].[State]    Script Date: 03/04/2020 11:47:39 ******/
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
/****** Object:  Table [dbo].[ShiftOffDay]    Script Date: 03/04/2020 11:47:39 ******/
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
SET IDENTITY_INSERT [dbo].[ShiftOffDay] ON
INSERT [dbo].[ShiftOffDay] ([ID], [ShiftID], [OffDayOfWeek], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, 3, 1, CAST(0x0000AB5D00000000 AS DateTime), NULL, CAST(0x0000AB5D0171E258 AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[ShiftOffDay] ([ID], [ShiftID], [OffDayOfWeek], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, 3, 7, CAST(0x0000AB5D00000000 AS DateTime), NULL, CAST(0x0000AB5D0171E265 AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[ShiftOffDay] ([ID], [ShiftID], [OffDayOfWeek], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (3, 4, 1, CAST(0x0000AB7300000000 AS DateTime), NULL, CAST(0x0000AB7300E2CD3D AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[ShiftOffDay] ([ID], [ShiftID], [OffDayOfWeek], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (4, 4, 3, CAST(0x0000AB7300000000 AS DateTime), NULL, CAST(0x0000AB7300E2CD4A AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[ShiftOffDay] ([ID], [ShiftID], [OffDayOfWeek], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (5, 4, 5, CAST(0x0000AB7300000000 AS DateTime), NULL, CAST(0x0000AB7300E2CD51 AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[ShiftOffDay] ([ID], [ShiftID], [OffDayOfWeek], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (6, 4, 7, CAST(0x0000AB7300000000 AS DateTime), NULL, CAST(0x0000AB7300E2CD58 AS DateTime), NULL, 1, N'::1')
SET IDENTITY_INSERT [dbo].[ShiftOffDay] OFF
/****** Object:  Table [dbo].[BranchShift]    Script Date: 03/04/2020 11:47:39 ******/
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[BranchShift] ON
INSERT [dbo].[BranchShift] ([ID], [BranchID], [ShiftID], [CreatedDate], [UpdatedDate], [UpdatedBy], [UserIP], [IsActive]) VALUES (1, 1, 3, CAST(0x0000AB5D0171E26D AS DateTime), CAST(0x0000AB5D0171E26D AS DateTime), 1, N'::1', 1)
INSERT [dbo].[BranchShift] ([ID], [BranchID], [ShiftID], [CreatedDate], [UpdatedDate], [UpdatedBy], [UserIP], [IsActive]) VALUES (2, 1, 4, CAST(0x0000AB7300E2CD60 AS DateTime), CAST(0x0000AB7300E2CD60 AS DateTime), 1, N'::1', 1)
SET IDENTITY_INSERT [dbo].[BranchShift] OFF
/****** Object:  Table [dbo].[BranchDepartment]    Script Date: 03/04/2020 11:47:39 ******/
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[BranchDepartment] ON
INSERT [dbo].[BranchDepartment] ([ID], [BranchID], [DepartmentID], [CreatedDate], [UpdatedDate], [UpdatedBy], [UserIP], [IsActive]) VALUES (1, 1, 1, CAST(0x0000AB5D0168BF21 AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[BranchDepartment] ([ID], [BranchID], [DepartmentID], [CreatedDate], [UpdatedDate], [UpdatedBy], [UserIP], [IsActive]) VALUES (2, 1, 2, CAST(0x0000AB5D016F0518 AS DateTime), NULL, 1, N'::1', 1)
INSERT [dbo].[BranchDepartment] ([ID], [BranchID], [DepartmentID], [CreatedDate], [UpdatedDate], [UpdatedBy], [UserIP], [IsActive]) VALUES (3, 1, 3, CAST(0x0000AB5D016F24E3 AS DateTime), NULL, 1, N'::1', 1)
INSERT [dbo].[BranchDepartment] ([ID], [BranchID], [DepartmentID], [CreatedDate], [UpdatedDate], [UpdatedBy], [UserIP], [IsActive]) VALUES (4, 1, 4, CAST(0x0000AB5D016F38E5 AS DateTime), NULL, 1, N'::1', 1)
INSERT [dbo].[BranchDepartment] ([ID], [BranchID], [DepartmentID], [CreatedDate], [UpdatedDate], [UpdatedBy], [UserIP], [IsActive]) VALUES (5, 1, 5, CAST(0x0000AB5D016F499D AS DateTime), NULL, 1, N'::1', 1)
SET IDENTITY_INSERT [dbo].[BranchDepartment] OFF
/****** Object:  Table [dbo].[AttendancePolicy]    Script Date: 03/04/2020 11:47:39 ******/
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
SET IDENTITY_INSERT [dbo].[AttendancePolicy] ON
INSERT [dbo].[AttendancePolicy] ([ID], [ShiftID], [AttendanceVariableID], [Hours], [Description], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, 3, 1, 4, NULL, CAST(0x0000AB5D00000000 AS DateTime), NULL, CAST(0x0000AB5D0174F6B9 AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[AttendancePolicy] ([ID], [ShiftID], [AttendanceVariableID], [Hours], [Description], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, 3, 2, 2, NULL, CAST(0x0000AB5D00000000 AS DateTime), NULL, CAST(0x0000AB5D0174F6D8 AS DateTime), NULL, 1, N'::1')
SET IDENTITY_INSERT [dbo].[AttendancePolicy] OFF
/****** Object:  Table [dbo].[Payroll]    Script Date: 03/04/2020 11:47:39 ******/
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PayrollPolicy]    Script Date: 03/04/2020 11:47:39 ******/
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Device]    Script Date: 03/04/2020 11:47:39 ******/
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[City]    Script Date: 03/04/2020 11:47:39 ******/
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
/****** Object:  Table [dbo].[PayrollDetail]    Script Date: 03/04/2020 11:47:39 ******/
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[User]    Script Date: 03/04/2020 11:47:39 ******/
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
	[SalaryTypeID] [int] NULL
) ON [PRIMARY]
SET ANSI_PADDING OFF
ALTER TABLE [dbo].[User] ADD [FatherName] [varchar](200) NULL
ALTER TABLE [dbo].[User] ADD [AccountNumber] [varchar](50) NULL
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[User] ON
INSERT [dbo].[User] ([ID], [FirstName], [MiddleName], [LastName], [UserTypeID], [GenderID], [DateOfBirth], [NICNo], [ReligionID], [Address1], [Address2], [ZipCode], [CountryID], [CityID], [StateID], [AcadmicQualification], [Designation], [Salary], [LoginID], [Password], [ImagePath], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [SalaryTypeID], [FatherName], [AccountNumber]) VALUES (1, N'Syed Razi Wahid', NULL, NULL, 1, 1, CAST(0x0000A648011FA1C3 AS DateTime), NULL, NULL, NULL, NULL, NULL, 162, NULL, NULL, NULL, N'Asstt Acctt', NULL, N'razi', N'J0$h123', NULL, 1, CAST(0x0000A648011FA1C3 AS DateTime), NULL, NULL, NULL, NULL, N'Syed Wahid', N'36669-5')
INSERT [dbo].[User] ([ID], [FirstName], [MiddleName], [LastName], [UserTypeID], [GenderID], [DateOfBirth], [NICNo], [ReligionID], [Address1], [Address2], [ZipCode], [CountryID], [CityID], [StateID], [AcadmicQualification], [Designation], [Salary], [LoginID], [Password], [ImagePath], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [SalaryTypeID], [FatherName], [AccountNumber]) VALUES (2, N'Arquam Belal', NULL, NULL, 2, 1, CAST(0x0000722300000000 AS DateTime), N'45455-5678468-4', 1, NULL, NULL, NULL, 162, NULL, NULL, N'BSCS', N'Website Developer', CAST(25000.00 AS Decimal(9, 2)), N'arquam', N'arquam', N'/Uploads/ProfileImages\2.png', 1, CAST(0x0000AB5D01702870 AS DateTime), NULL, 1, N'::1', 1, N'F/O Arquam Belal', N'12345646')
INSERT [dbo].[User] ([ID], [FirstName], [MiddleName], [LastName], [UserTypeID], [GenderID], [DateOfBirth], [NICNo], [ReligionID], [Address1], [Address2], [ZipCode], [CountryID], [CityID], [StateID], [AcadmicQualification], [Designation], [Salary], [LoginID], [Password], [ImagePath], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [SalaryTypeID], [FatherName], [AccountNumber]) VALUES (3, N'Saeed Hussain Shah', NULL, NULL, 2, 1, CAST(0x0000722300000000 AS DateTime), N'45524-8755458-2', 1, NULL, NULL, NULL, 162, NULL, NULL, N'Certified', N'Website Developer Internee', CAST(10000.00 AS Decimal(9, 2)), N'saeed', N'saeed', NULL, 1, CAST(0x0000AB5D0170ACA8 AS DateTime), NULL, 1, N'::1', 1, N'FO Saeed Hussain Shah', NULL)
INSERT [dbo].[User] ([ID], [FirstName], [MiddleName], [LastName], [UserTypeID], [GenderID], [DateOfBirth], [NICNo], [ReligionID], [Address1], [Address2], [ZipCode], [CountryID], [CityID], [StateID], [AcadmicQualification], [Designation], [Salary], [LoginID], [Password], [ImagePath], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [SalaryTypeID], [FatherName], [AccountNumber]) VALUES (4, N'Abdul Azeez', NULL, NULL, 2, 1, CAST(0x0000722300000000 AS DateTime), NULL, 1, NULL, NULL, NULL, 162, NULL, NULL, N'BSCS', N'Website Developer', CAST(35000.00 AS Decimal(9, 2)), N'azeez', N'azeez', NULL, 1, CAST(0x0000AB5D0170FA91 AS DateTime), NULL, 1, N'::1', 1, N'FO Abdul Azeez', NULL)
INSERT [dbo].[User] ([ID], [FirstName], [MiddleName], [LastName], [UserTypeID], [GenderID], [DateOfBirth], [NICNo], [ReligionID], [Address1], [Address2], [ZipCode], [CountryID], [CityID], [StateID], [AcadmicQualification], [Designation], [Salary], [LoginID], [Password], [ImagePath], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [SalaryTypeID], [FatherName], [AccountNumber]) VALUES (5, N'Aqsa Shafiq', NULL, NULL, 2, 2, CAST(0x0000722300000000 AS DateTime), NULL, 1, NULL, NULL, NULL, 162, NULL, NULL, N'Masters', N'Graphics Designer', CAST(40000.00 AS Decimal(9, 2)), N'aqsa', N'aqsa', NULL, 1, CAST(0x0000AB5D017147C5 AS DateTime), NULL, 1, N'::1', 1, N'FO Aqsa Shafiq', NULL)
INSERT [dbo].[User] ([ID], [FirstName], [MiddleName], [LastName], [UserTypeID], [GenderID], [DateOfBirth], [NICNo], [ReligionID], [Address1], [Address2], [ZipCode], [CountryID], [CityID], [StateID], [AcadmicQualification], [Designation], [Salary], [LoginID], [Password], [ImagePath], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [SalaryTypeID], [FatherName], [AccountNumber]) VALUES (6, N'Javeria Asif', NULL, NULL, 2, 2, CAST(0x0000722300000000 AS DateTime), NULL, 1, NULL, NULL, NULL, 162, NULL, NULL, N'Masters', N'Creative Writer', CAST(32000.00 AS Decimal(9, 2)), N'javeria', N'javeria', NULL, 1, CAST(0x0000AB5D0171903E AS DateTime), NULL, 1, N'::1', 1, N'FO Javeria Asif', NULL)
INSERT [dbo].[User] ([ID], [FirstName], [MiddleName], [LastName], [UserTypeID], [GenderID], [DateOfBirth], [NICNo], [ReligionID], [Address1], [Address2], [ZipCode], [CountryID], [CityID], [StateID], [AcadmicQualification], [Designation], [Salary], [LoginID], [Password], [ImagePath], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [SalaryTypeID], [FatherName], [AccountNumber]) VALUES (7, N'Saboor', NULL, NULL, 2, 1, CAST(0x0000722C00000000 AS DateTime), N'54487-8547825-6', 1, NULL, NULL, NULL, 162, NULL, NULL, N'Masters', N'Website Developer Internee', CAST(10000.00 AS Decimal(9, 2)), N'saboor', N'Saboor123', NULL, 1, CAST(0x0000AB7300E1DD39 AS DateTime), NULL, 1, N'::1', 1, N'FO Saboor', N'123456789-1')
SET IDENTITY_INSERT [dbo].[User] OFF
/****** Object:  Table [dbo].[UserShift]    Script Date: 03/04/2020 11:47:39 ******/
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
SET IDENTITY_INSERT [dbo].[UserShift] ON
INSERT [dbo].[UserShift] ([ID], [UserID], [ShiftID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, 1, 1, CAST(0x0000AB3500000000 AS DateTime), NULL, CAST(0x0000AB5D016754C9 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[UserShift] ([ID], [UserID], [ShiftID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, 2, 3, CAST(0x0000AB5D00000000 AS DateTime), NULL, CAST(0x0000AB5D01753C70 AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserShift] ([ID], [UserID], [ShiftID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (3, 3, 3, CAST(0x0000AB5D00000000 AS DateTime), NULL, CAST(0x0000AB5D01753C81 AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserShift] ([ID], [UserID], [ShiftID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (4, 4, 3, CAST(0x0000AB5D00000000 AS DateTime), NULL, CAST(0x0000AB5D01753C91 AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserShift] ([ID], [UserID], [ShiftID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (5, 5, 3, CAST(0x0000AB5D00000000 AS DateTime), NULL, CAST(0x0000AB5D01756834 AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserShift] ([ID], [UserID], [ShiftID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (6, 6, 3, CAST(0x0000AB5D00000000 AS DateTime), NULL, CAST(0x0000AB5D01756843 AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserShift] ([ID], [UserID], [ShiftID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (7, 7, 3, CAST(0x0000AB7500000000 AS DateTime), CAST(0x0000AB8F018B80D4 AS DateTime), CAST(0x0000AB7300E23A0E AS DateTime), CAST(0x0000AB7300E31AE4 AS DateTime), 1, N'::1')
INSERT [dbo].[UserShift] ([ID], [UserID], [ShiftID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (8, 7, 4, CAST(0x0000AB9000000000 AS DateTime), NULL, CAST(0x0000AB7300E31AEF AS DateTime), NULL, 1, N'::1')
SET IDENTITY_INSERT [dbo].[UserShift] OFF
/****** Object:  Table [dbo].[UserDepartment]    Script Date: 03/04/2020 11:47:39 ******/
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
SET IDENTITY_INSERT [dbo].[UserDepartment] ON
INSERT [dbo].[UserDepartment] ([ID], [UserID], [DepartmentID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, 1, 1, CAST(0x0000AB3500000000 AS DateTime), NULL, CAST(0x0000AB5D01681DC5 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[UserDepartment] ([ID], [UserID], [DepartmentID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, 2, 2, CAST(0x0000AB5D00000000 AS DateTime), NULL, CAST(0x0000AB5D017028E7 AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserDepartment] ([ID], [UserID], [DepartmentID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (3, 3, 2, CAST(0x0000AB5D00000000 AS DateTime), NULL, CAST(0x0000AB5D0170ACD2 AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserDepartment] ([ID], [UserID], [DepartmentID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (4, 4, 2, CAST(0x0000AB5D00000000 AS DateTime), NULL, CAST(0x0000AB5D0170FAB5 AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserDepartment] ([ID], [UserID], [DepartmentID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (5, 5, 3, CAST(0x0000AB5D00000000 AS DateTime), NULL, CAST(0x0000AB5D017147E8 AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserDepartment] ([ID], [UserID], [DepartmentID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (6, 6, 4, CAST(0x0000AB5D00000000 AS DateTime), NULL, CAST(0x0000AB5D0171907A AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserDepartment] ([ID], [UserID], [DepartmentID], [EffectiveDate], [RetiredDate], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (7, 7, 2, CAST(0x0000AB7300000000 AS DateTime), NULL, CAST(0x0000AB7300E1DD7A AS DateTime), NULL, 1, N'::1')
SET IDENTITY_INSERT [dbo].[UserDepartment] OFF
/****** Object:  Table [dbo].[UserContact]    Script Date: 03/04/2020 11:47:39 ******/
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
SET IDENTITY_INSERT [dbo].[UserContact] ON
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, 1, 2, N'1234567890', 1, CAST(0x0000AB5D0167AD02 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, 1, 5, N'josh@rigelonic.com', 1, CAST(0x0000AB5D0167AD02 AS DateTime), NULL, NULL, NULL)
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (3, 2, 5, N'abc@arquam.com', 1, CAST(0x0000AB5D017028C4 AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (4, 2, 2, N'123123123321', 1, CAST(0x0000AB5D017028C4 AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (5, 2, 3, NULL, 1, CAST(0x0000AB5D017028C4 AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (6, 3, 5, N'saeed@abc.com', 1, CAST(0x0000AB5D0170ACB8 AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (7, 3, 2, N'4654654654654', 1, CAST(0x0000AB5D0170ACB9 AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (8, 3, 3, NULL, 1, CAST(0x0000AB5D0170ACB9 AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (9, 4, 5, N'azeez@email.com', 1, CAST(0x0000AB5D0170FA9E AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (10, 4, 2, N'1234654321321', 1, CAST(0x0000AB5D0170FA9E AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (11, 4, 3, NULL, 1, CAST(0x0000AB5D0170FA9E AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (12, 5, 5, N'aqsa@email.com', 1, CAST(0x0000AB5D017147D1 AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (13, 5, 2, N'213213213213', 1, CAST(0x0000AB5D017147D2 AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (14, 5, 3, NULL, 1, CAST(0x0000AB5D017147D2 AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (15, 6, 5, N'javeria@email.com', 1, CAST(0x0000AB5D0171904A AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (16, 6, 2, N'23424234324', 1, CAST(0x0000AB5D0171904A AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (17, 6, 3, NULL, 1, CAST(0x0000AB5D0171904A AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (18, 7, 5, N'saboor@gmail.com', 1, CAST(0x0000AB7300E1DD5B AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (19, 7, 2, N'498465465465', 1, CAST(0x0000AB7300E1DD5B AS DateTime), NULL, 1, N'::1')
INSERT [dbo].[UserContact] ([ID], [UserID], [ContactTypeID], [Detail], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (20, 7, 3, NULL, 1, CAST(0x0000AB7300E1DD5C AS DateTime), NULL, 1, N'::1')
SET IDENTITY_INSERT [dbo].[UserContact] OFF
/****** Object:  Table [dbo].[Leave]    Script Date: 03/04/2020 11:47:39 ******/
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
/****** Object:  Table [dbo].[Attendance]    Script Date: 03/04/2020 11:47:39 ******/
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
SET IDENTITY_INSERT [dbo].[Attendance] ON
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, 2, CAST(0x0000AB5600000000 AS DateTime), 1, CAST(0x0000AB5D017BCF4E AS DateTime), NULL, NULL, N'::1')
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, 2, CAST(0x0000AB5700000000 AS DateTime), 1, CAST(0x0000AB5D017CC5C7 AS DateTime), NULL, NULL, N'::1')
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (3, 2, CAST(0x0000AB5800000000 AS DateTime), 1, CAST(0x0000AB5D017CC61B AS DateTime), NULL, NULL, N'::1')
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (4, 2, CAST(0x0000AB5900000000 AS DateTime), 1, CAST(0x0000AB5D017CC650 AS DateTime), NULL, NULL, N'::1')
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (5, 2, CAST(0x0000AB5A00000000 AS DateTime), 1, CAST(0x0000AB5D017CC67C AS DateTime), NULL, NULL, N'::1')
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (6, 2, CAST(0x0000AB5D00000000 AS DateTime), 1, CAST(0x0000AB5D017CC6CA AS DateTime), NULL, NULL, N'::1')
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (7, 3, CAST(0x0000AB5600000000 AS DateTime), 1, CAST(0x0000AB5D017CC757 AS DateTime), NULL, NULL, N'::1')
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (8, 3, CAST(0x0000AB5700000000 AS DateTime), 1, CAST(0x0000AB5D017CC7CB AS DateTime), NULL, NULL, N'::1')
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (9, 3, CAST(0x0000AB5800000000 AS DateTime), 1, CAST(0x0000AB5D017CC814 AS DateTime), NULL, NULL, N'::1')
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (10, 4, CAST(0x0000AB5600000000 AS DateTime), 1, CAST(0x0000AB5D017CC841 AS DateTime), NULL, NULL, N'::1')
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (11, 4, CAST(0x0000AB5800000000 AS DateTime), 1, CAST(0x0000AB5D017CC870 AS DateTime), NULL, NULL, N'::1')
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (12, 4, CAST(0x0000AB5A00000000 AS DateTime), 1, CAST(0x0000AB5D017CC89E AS DateTime), NULL, NULL, N'::1')
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (13, 4, CAST(0x0000AB5D00000000 AS DateTime), 1, CAST(0x0000AB5D017CC8CC AS DateTime), NULL, NULL, N'::1')
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (14, 5, CAST(0x0000AB5600000000 AS DateTime), 1, CAST(0x0000AB5D017CC923 AS DateTime), NULL, NULL, N'::1')
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (15, 5, CAST(0x0000AB5700000000 AS DateTime), 1, CAST(0x0000AB5D017CC97D AS DateTime), NULL, NULL, N'::1')
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (16, 5, CAST(0x0000AB5900000000 AS DateTime), 1, CAST(0x0000AB5D017CC9D4 AS DateTime), NULL, NULL, N'::1')
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (17, 5, CAST(0x0000AB5A00000000 AS DateTime), 1, CAST(0x0000AB5D017CCA42 AS DateTime), NULL, NULL, N'::1')
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (18, 5, CAST(0x0000AB5D00000000 AS DateTime), 1, CAST(0x0000AB5D017CCA8C AS DateTime), NULL, NULL, N'::1')
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (19, 6, CAST(0x0000AB5600000000 AS DateTime), 1, CAST(0x0000AB5D017CCAD6 AS DateTime), NULL, NULL, N'::1')
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (20, 6, CAST(0x0000AB5700000000 AS DateTime), 1, CAST(0x0000AB5D017CCB13 AS DateTime), NULL, NULL, N'::1')
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (21, 6, CAST(0x0000AB5900000000 AS DateTime), 1, CAST(0x0000AB5D017CCB3F AS DateTime), NULL, NULL, N'::1')
INSERT [dbo].[Attendance] ([ID], [UserID], [Date], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (22, 6, CAST(0x0000AB5D00000000 AS DateTime), 1, CAST(0x0000AB5D017CCB70 AS DateTime), NULL, NULL, N'::1')
SET IDENTITY_INSERT [dbo].[Attendance] OFF
/****** Object:  Table [dbo].[AttendanceDetail]    Script Date: 03/04/2020 11:47:39 ******/
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
SET IDENTITY_INSERT [dbo].[AttendanceDetail] ON
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (1, 1, 1, CAST(0x0000AB5600A8EA30 AS DateTime), CAST(0x0000AB5601601CA0 AS DateTime), 1, CAST(0x0000AB5D017BCF86 AS DateTime), NULL, NULL, N'::1')
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (2, 2, 1, CAST(0x0000AB5700AA49C0 AS DateTime), CAST(0x0000AB57013A7BD0 AS DateTime), 1, CAST(0x0000AB5D017CC5E3 AS DateTime), NULL, NULL, N'::1')
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (3, 3, 1, CAST(0x0000AB5800C042C0 AS DateTime), CAST(0x0000AB580163F500 AS DateTime), 1, CAST(0x0000AB5D017CC625 AS DateTime), NULL, NULL, N'::1')
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (4, 4, 1, CAST(0x0000AB5900A4CB80 AS DateTime), CAST(0x0000AB59013FFA10 AS DateTime), 1, CAST(0x0000AB5D017CC659 AS DateTime), NULL, NULL, N'::1')
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (5, 5, 1, CAST(0x0000AB5A00A4CB80 AS DateTime), CAST(0x0000AB5A0145BEA0 AS DateTime), 1, CAST(0x0000AB5D017CC684 AS DateTime), NULL, NULL, N'::1')
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (6, 6, 1, CAST(0x0000AB5D00A4CB80 AS DateTime), CAST(0x0000AB5D01391C40 AS DateTime), 1, CAST(0x0000AB5D017CC6E7 AS DateTime), NULL, NULL, N'::1')
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (7, 7, 1, CAST(0x0000AB5600A4CB80 AS DateTime), CAST(0x0000AB56014F1540 AS DateTime), 1, CAST(0x0000AB5D017CC776 AS DateTime), NULL, NULL, N'::1')
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (8, 8, 1, CAST(0x0000AB5700A4CB80 AS DateTime), CAST(0x0000AB57013A7BD0 AS DateTime), 1, CAST(0x0000AB5D017CC7DF AS DateTime), NULL, NULL, N'::1')
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (9, 9, 1, CAST(0x0000AB5800A4CB80 AS DateTime), CAST(0x0000AB58015A11C0 AS DateTime), 1, CAST(0x0000AB5D017CC81E AS DateTime), NULL, NULL, N'::1')
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (10, 10, 1, CAST(0x0000AB5600C042C0 AS DateTime), CAST(0x0000AB5601549380 AS DateTime), 1, CAST(0x0000AB5D017CC84A AS DateTime), NULL, NULL, N'::1')
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (11, 11, 1, CAST(0x0000AB5800DE7920 AS DateTime), CAST(0x0000AB58015A11C0 AS DateTime), 1, CAST(0x0000AB5D017CC879 AS DateTime), NULL, NULL, N'::1')
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (12, 12, 1, CAST(0x0000AB5A00B80560 AS DateTime), CAST(0x0000AB5A015E3070 AS DateTime), 1, CAST(0x0000AB5D017CC8A7 AS DateTime), NULL, NULL, N'::1')
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (13, 13, 1, CAST(0x0000AB5D00AE6870 AS DateTime), CAST(0x0000AB5D013E9A80 AS DateTime), 1, CAST(0x0000AB5D017CC8D6 AS DateTime), NULL, NULL, N'::1')
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (14, 14, 1, CAST(0x0000AB5600A4CB80 AS DateTime), CAST(0x0000AB5601391C40 AS DateTime), 1, CAST(0x0000AB5D017CC935 AS DateTime), NULL, NULL, N'::1')
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (15, 15, 1, CAST(0x0000AB5700A62B10 AS DateTime), CAST(0x0000AB57010BCAB0 AS DateTime), 1, CAST(0x0000AB5D017CC990 AS DateTime), NULL, NULL, N'::1')
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (16, 16, 1, CAST(0x0000AB5900AA49C0 AS DateTime), CAST(0x0000AB59010E89D0 AS DateTime), 1, CAST(0x0000AB5D017CC9E8 AS DateTime), NULL, NULL, N'::1')
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (17, 17, 1, CAST(0x0000AB5A00A78AA0 AS DateTime), CAST(0x0000AB5A0128A180 AS DateTime), 1, CAST(0x0000AB5D017CCA50 AS DateTime), NULL, NULL, N'::1')
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (18, 18, 1, CAST(0x0000AB5D00A8EA30 AS DateTime), CAST(0x0000AB5D0128A180 AS DateTime), 1, CAST(0x0000AB5D017CCA9F AS DateTime), NULL, NULL, N'::1')
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (19, 19, 1, CAST(0x0000AB5600A4CB80 AS DateTime), CAST(0x0000AB5601391C40 AS DateTime), 1, CAST(0x0000AB5D017CCAE0 AS DateTime), NULL, NULL, N'::1')
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (20, 20, 1, CAST(0x0000AB5700A62B10 AS DateTime), CAST(0x0000AB5701339E00 AS DateTime), 1, CAST(0x0000AB5D017CCB1D AS DateTime), NULL, NULL, N'::1')
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (21, 21, 1, CAST(0x0000AB5900AA49C0 AS DateTime), CAST(0x0000AB5901365D20 AS DateTime), 1, CAST(0x0000AB5D017CCB47 AS DateTime), NULL, NULL, N'::1')
INSERT [dbo].[AttendanceDetail] ([ID], [AttendanceID], [AttendanceTypeID], [StartDate], [EndDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP]) VALUES (22, 22, 1, CAST(0x0000AB5D00A8EA30 AS DateTime), CAST(0x0000AB5D0128A180 AS DateTime), 1, CAST(0x0000AB5D017CCB7A AS DateTime), NULL, NULL, N'::1')
SET IDENTITY_INSERT [dbo].[AttendanceDetail] OFF
/****** Object:  Table [dbo].[AttendanceStatus]    Script Date: 03/04/2020 11:47:39 ******/
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[AttendanceStatus] ON
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (1, 1, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB5D017CC59A AS DateTime), CAST(0x0000AB5D017D0281 AS DateTime), NULL, N'::1', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (2, 2, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB5D017CC60A AS DateTime), CAST(0x0000AB5D017D02A2 AS DateTime), NULL, N'::1', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (3, 3, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB5D017CC63F AS DateTime), CAST(0x0000AB5D017D02BC AS DateTime), NULL, N'::1', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (4, 4, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB5D017CC66E AS DateTime), CAST(0x0000AB5D017D02D5 AS DateTime), NULL, N'::1', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (5, 5, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB5D017CC6A1 AS DateTime), CAST(0x0000AB5D017D02EF AS DateTime), NULL, N'::1', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (6, 6, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB5D017CC720 AS DateTime), CAST(0x0000AB5D017D030D AS DateTime), NULL, N'::1', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (7, 7, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB5D017CC7AA AS DateTime), CAST(0x0000AB5D017D0326 AS DateTime), NULL, N'::1', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (8, 8, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB5D017CC803 AS DateTime), CAST(0x0000AB5D017D0340 AS DateTime), NULL, N'::1', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (9, 9, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB5D017CC833 AS DateTime), CAST(0x0000AB5D017D0359 AS DateTime), NULL, N'::1', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (10, 10, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB5D017CC861 AS DateTime), CAST(0x0000AB5D017D0373 AS DateTime), NULL, N'::1', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (11, 11, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB5D017CC88F AS DateTime), CAST(0x0000AB5D017D038F AS DateTime), NULL, N'::1', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (12, 12, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB5D017CC8BD AS DateTime), CAST(0x0000AB5D017D03A8 AS DateTime), NULL, N'::1', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (13, 13, 0, 0, 0, 0, 1, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB5D017CC901 AS DateTime), CAST(0x0000AB5D017D03C6 AS DateTime), NULL, N'::1', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (14, 14, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB5D017CC964 AS DateTime), CAST(0x0000AB5D017D03DE AS DateTime), NULL, N'::1', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (15, 15, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB5D017CC9B5 AS DateTime), CAST(0x0000AB5D017D03F8 AS DateTime), NULL, N'::1', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (16, 16, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB5D017CCA2A AS DateTime), CAST(0x0000AB5D017D0411 AS DateTime), NULL, N'::1', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (17, 17, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB5D017CCA68 AS DateTime), CAST(0x0000AB5D017D0431 AS DateTime), NULL, N'::1', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (18, 18, 0, 0, 0, 0, 1, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB5D017CCABC AS DateTime), CAST(0x0000AB5D017D0450 AS DateTime), NULL, N'::1', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (19, 19, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB5D017CCAFE AS DateTime), CAST(0x0000AB5D017D046A AS DateTime), NULL, N'::1', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (20, 20, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB5D017CCB31 AS DateTime), CAST(0x0000AB5D017D0486 AS DateTime), NULL, N'::1', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (21, 21, 0, 0, 0, 0, 0, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB5D017CCB60 AS DateTime), CAST(0x0000AB5D017D04A0 AS DateTime), NULL, N'::1', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[AttendanceStatus] ([ID], [AttendanceID], [IsShiftOffDay], [IsHoliday], [IsLeaveDay], [IsQuarterDay], [IsHalfDay], [IsFullDay], [Reason], [IsApproved], [Remarks], [ActionBy], [ActionDate], [IsActive], [CreationDate], [UpdateDate], [UpdateBy], [UserIP], [IsLate], [IsEarly], [LateMinutes], [EarlyMinutes], [WorkingMinutes], [TotalMinutes], [OverTimeMinutes], [BreakType]) VALUES (22, 22, 0, 0, 0, 0, 1, 0, N'', NULL, N'', NULL, NULL, 1, CAST(0x0000AB5D017CCB8E AS DateTime), CAST(0x0000AB5D017D04BF AS DateTime), NULL, N'::1', 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[AttendanceStatus] OFF
/****** Object:  StoredProcedure [dbo].[SELECT_MonthlyAttendanceSummary]    Script Date: 03/04/2020 11:47:37 ******/
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
/****** Object:  StoredProcedure [dbo].[SELECT_AttendanceSummary]    Script Date: 03/04/2020 11:47:37 ******/
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
/****** Object:  StoredProcedure [dbo].[SELECT_Attendance]    Script Date: 03/04/2020 11:47:37 ******/
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
/****** Object:  StoredProcedure [dbo].[InsertPreAttendance]    Script Date: 03/04/2020 11:47:37 ******/
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
/****** Object:  StoredProcedure [dbo].[insert_manualAttendance]    Script Date: 03/04/2020 11:47:37 ******/
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
/****** Object:  StoredProcedure [dbo].[GeneratePayroll]    Script Date: 03/04/2020 11:47:37 ******/
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
/****** Object:  Default [DF_Attendance_IsActive]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[Attendance] ADD  CONSTRAINT [DF_Attendance_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
/****** Object:  Default [DF_Attendance_CreationDate]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[Attendance] ADD  CONSTRAINT [DF_Attendance_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
/****** Object:  Default [DF_AttendanceDetail_IsActive]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[AttendanceDetail] ADD  CONSTRAINT [DF_AttendanceDetail_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
/****** Object:  Default [DF_AttendanceDetail_CreationDate]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[AttendanceDetail] ADD  CONSTRAINT [DF_AttendanceDetail_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
/****** Object:  Default [DF_AttendancePolicy_CreationDate]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[AttendancePolicy] ADD  CONSTRAINT [DF_AttendancePolicy_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
/****** Object:  Default [DF_AttendanceStatus_IsActive]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[AttendanceStatus] ADD  CONSTRAINT [DF_AttendanceStatus_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
/****** Object:  Default [DF_AttendanceStatus_CreationDate]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[AttendanceStatus] ADD  CONSTRAINT [DF_AttendanceStatus_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
/****** Object:  Default [DF_AttendanceType_IsActive]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[AttendanceType] ADD  CONSTRAINT [DF_AttendanceType_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
/****** Object:  Default [DF_AttendanceType_CreationDate]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[AttendanceType] ADD  CONSTRAINT [DF_AttendanceType_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
/****** Object:  Default [DF_AttendanceVariable_IsActive]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[AttendanceVariable] ADD  CONSTRAINT [DF_AttendanceVariable_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
/****** Object:  Default [DF_AttendanceVariable_CreationDate]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[AttendanceVariable] ADD  CONSTRAINT [DF_AttendanceVariable_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
/****** Object:  Default [DF__Branch__CreatedD__75A278F5]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[Branch] ADD  CONSTRAINT [DF__Branch__CreatedD__75A278F5]  DEFAULT (getdate()) FOR [CreatedDate]
GO
/****** Object:  Default [DF__Branch__IsActive__76969D2E]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[Branch] ADD  CONSTRAINT [DF__Branch__IsActive__76969D2E]  DEFAULT ((1)) FOR [IsActive]
GO
/****** Object:  Default [DF__BranchDep__Creat__74AE54BC]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[BranchDepartment] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
/****** Object:  Default [DF__BranchDep__IsAct__75A278F5]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[BranchDepartment] ADD  DEFAULT ((1)) FOR [IsActive]
GO
/****** Object:  Default [DF__BranchShi__Creat__72C60C4A]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[BranchShift] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
/****** Object:  Default [DF__BranchShi__IsAct__73BA3083]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[BranchShift] ADD  DEFAULT ((1)) FOR [IsActive]
GO
/****** Object:  Default [DF_City_IsActive]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[City] ADD  CONSTRAINT [DF_City_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
/****** Object:  Default [DF_City_CreationDate]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[City] ADD  CONSTRAINT [DF_City_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
/****** Object:  Default [DF_Configuration_IsActive]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[Configuration] ADD  CONSTRAINT [DF_Configuration_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
/****** Object:  Default [DF_Configuration_CreationDate]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[Configuration] ADD  CONSTRAINT [DF_Configuration_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
/****** Object:  Default [DF_ContactType_IsActive]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[ContactType] ADD  CONSTRAINT [DF_ContactType_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
/****** Object:  Default [DF_ContactType_CreationDate]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[ContactType] ADD  CONSTRAINT [DF_ContactType_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
/****** Object:  Default [DF_Country_IsActive]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[Country] ADD  CONSTRAINT [DF_Country_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
/****** Object:  Default [DF_Country_CreationDate]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[Country] ADD  CONSTRAINT [DF_Country_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
/****** Object:  Default [DF_Department_IsActive]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[Department] ADD  CONSTRAINT [DF_Department_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
/****** Object:  Default [DF_Department_CreationDate]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[Department] ADD  CONSTRAINT [DF_Department_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
/****** Object:  Default [DF_Device_IsActive]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[Device] ADD  CONSTRAINT [DF_Device_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
/****** Object:  Default [DF_Device_CreationDate]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[Device] ADD  CONSTRAINT [DF_Device_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
/****** Object:  Default [DF_DeviceModal_CreatedDate]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[DeviceModal] ADD  CONSTRAINT [DF_DeviceModal_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
/****** Object:  Default [DF_DeviceModal_IsActive]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[DeviceModal] ADD  CONSTRAINT [DF_DeviceModal_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
/****** Object:  Default [DF_Gender_IsActive]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[Gender] ADD  CONSTRAINT [DF_Gender_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
/****** Object:  Default [DF_Gender_CreationDate]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[Gender] ADD  CONSTRAINT [DF_Gender_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
/****** Object:  Default [DF_Holiday_IsActive]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[Holiday] ADD  CONSTRAINT [DF_Holiday_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
/****** Object:  Default [DF_Holiday_CreationDate]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[Holiday] ADD  CONSTRAINT [DF_Holiday_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
/****** Object:  Default [DF_Leave_IsActive]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[Leave] ADD  CONSTRAINT [DF_Leave_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
/****** Object:  Default [DF_Leave_CreationDate]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[Leave] ADD  CONSTRAINT [DF_Leave_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
/****** Object:  Default [DF_LeaveType_IsActive]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[LeaveType] ADD  CONSTRAINT [DF_LeaveType_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
/****** Object:  Default [DF_LeaveType_CreationDate]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[LeaveType] ADD  CONSTRAINT [DF_LeaveType_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
/****** Object:  Default [DF_OG_LeaveType_IsActive]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[OG_LeaveType] ADD  CONSTRAINT [DF_OG_LeaveType_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
/****** Object:  Default [DF_OG_LeaveType_CreationDate]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[OG_LeaveType] ADD  CONSTRAINT [DF_OG_LeaveType_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
/****** Object:  Default [DF_Payroll_IsActive]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[Payroll] ADD  CONSTRAINT [DF_Payroll_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
/****** Object:  Default [DF_Payroll_CreationDate]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[Payroll] ADD  CONSTRAINT [DF_Payroll_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
/****** Object:  Default [DF_PayrollCycle_IsActive]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[PayrollCycle] ADD  CONSTRAINT [DF_PayrollCycle_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
/****** Object:  Default [DF_PayrollCycle_CreationDate]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[PayrollCycle] ADD  CONSTRAINT [DF_PayrollCycle_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
/****** Object:  Default [DF_PayrollDetail_IsActive]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[PayrollDetail] ADD  CONSTRAINT [DF_PayrollDetail_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
/****** Object:  Default [DF_PayrollDetail_CreationDate]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[PayrollDetail] ADD  CONSTRAINT [DF_PayrollDetail_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
/****** Object:  Default [DF_PayrollPolicy_CreationDate]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[PayrollPolicy] ADD  CONSTRAINT [DF_PayrollPolicy_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
/****** Object:  Default [DF_PayrollVariable_IsActive]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[PayrollVariable] ADD  CONSTRAINT [DF_PayrollVariable_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
/****** Object:  Default [DF_PayrollVariable_CreationDate]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[PayrollVariable] ADD  CONSTRAINT [DF_PayrollVariable_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
/****** Object:  Default [DF_Religion_IsActive]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[Religion] ADD  CONSTRAINT [DF_Religion_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
/****** Object:  Default [DF_Religion_CreationDate]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[Religion] ADD  CONSTRAINT [DF_Religion_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
/****** Object:  Default [DF_SalaryType_IsActive]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[SalaryType] ADD  CONSTRAINT [DF_SalaryType_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
/****** Object:  Default [DF_SalaryType_CreationDate]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[SalaryType] ADD  CONSTRAINT [DF_SalaryType_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
/****** Object:  Default [DF__Shift__IsActive__6C190EBB]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[Shift] ADD  DEFAULT ((1)) FOR [IsActive]
GO
/****** Object:  Default [DF_Shift_CreationDate]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[Shift] ADD  CONSTRAINT [DF_Shift_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
/****** Object:  Default [DF_ShiftOffDay_CreationDate]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[ShiftOffDay] ADD  CONSTRAINT [DF_ShiftOffDay_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
/****** Object:  Default [DF_State_IsActive]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[State] ADD  CONSTRAINT [DF_State_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
/****** Object:  Default [DF_State_CreationDate]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[State] ADD  CONSTRAINT [DF_State_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
/****** Object:  Default [DF_User_IsActive]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
/****** Object:  Default [DF_User_CreationDate]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[User] ADD  CONSTRAINT [DF_User_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
/****** Object:  Default [DF_UserContact_IsActive]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[UserContact] ADD  CONSTRAINT [DF_UserContact_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
/****** Object:  Default [DF_UserContact_CreationDate]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[UserContact] ADD  CONSTRAINT [DF_UserContact_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
/****** Object:  Default [DF_UserDepartment_CreationDate]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[UserDepartment] ADD  CONSTRAINT [DF_UserDepartment_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
/****** Object:  Default [DF_UserShift_CreationDate]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[UserShift] ADD  CONSTRAINT [DF_UserShift_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
/****** Object:  Default [DF_UserType_IsActive]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[UserType] ADD  CONSTRAINT [DF_UserType_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
/****** Object:  Default [DF_UserType_CreationDate]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[UserType] ADD  CONSTRAINT [DF_UserType_CreationDate]  DEFAULT (getdate()) FOR [CreationDate]
GO
/****** Object:  ForeignKey [fk_Id_AttendanceUser]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[Attendance]  WITH CHECK ADD  CONSTRAINT [fk_Id_AttendanceUser] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([ID])
GO
ALTER TABLE [dbo].[Attendance] CHECK CONSTRAINT [fk_Id_AttendanceUser]
GO
/****** Object:  ForeignKey [fk_Id_AttendanceDetailAttendance]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[AttendanceDetail]  WITH CHECK ADD  CONSTRAINT [fk_Id_AttendanceDetailAttendance] FOREIGN KEY([AttendanceID])
REFERENCES [dbo].[Attendance] ([ID])
GO
ALTER TABLE [dbo].[AttendanceDetail] CHECK CONSTRAINT [fk_Id_AttendanceDetailAttendance]
GO
/****** Object:  ForeignKey [fk_Id_AttendanceDetailAttendanceType]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[AttendanceDetail]  WITH CHECK ADD  CONSTRAINT [fk_Id_AttendanceDetailAttendanceType] FOREIGN KEY([AttendanceTypeID])
REFERENCES [dbo].[AttendanceType] ([ID])
GO
ALTER TABLE [dbo].[AttendanceDetail] CHECK CONSTRAINT [fk_Id_AttendanceDetailAttendanceType]
GO
/****** Object:  ForeignKey [fk_Id_AttendancePolicyAttendanceVariable]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[AttendancePolicy]  WITH CHECK ADD  CONSTRAINT [fk_Id_AttendancePolicyAttendanceVariable] FOREIGN KEY([AttendanceVariableID])
REFERENCES [dbo].[AttendanceVariable] ([ID])
GO
ALTER TABLE [dbo].[AttendancePolicy] CHECK CONSTRAINT [fk_Id_AttendancePolicyAttendanceVariable]
GO
/****** Object:  ForeignKey [fk_Id_AttendancePolicyShift]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[AttendancePolicy]  WITH CHECK ADD  CONSTRAINT [fk_Id_AttendancePolicyShift] FOREIGN KEY([ShiftID])
REFERENCES [dbo].[Shift] ([ID])
GO
ALTER TABLE [dbo].[AttendancePolicy] CHECK CONSTRAINT [fk_Id_AttendancePolicyShift]
GO
/****** Object:  ForeignKey [fk_Id_AttendanceStatusAttendance]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[AttendanceStatus]  WITH CHECK ADD  CONSTRAINT [fk_Id_AttendanceStatusAttendance] FOREIGN KEY([AttendanceID])
REFERENCES [dbo].[Attendance] ([ID])
GO
ALTER TABLE [dbo].[AttendanceStatus] CHECK CONSTRAINT [fk_Id_AttendanceStatusAttendance]
GO
/****** Object:  ForeignKey [FK__BranchDep__Branc__04E4BC85]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[BranchDepartment]  WITH CHECK ADD  CONSTRAINT [FK__BranchDep__Branc__04E4BC85] FOREIGN KEY([BranchID])
REFERENCES [dbo].[Branch] ([ID])
GO
ALTER TABLE [dbo].[BranchDepartment] CHECK CONSTRAINT [FK__BranchDep__Branc__04E4BC85]
GO
/****** Object:  ForeignKey [FK__BranchDep__Depar__123EB7A3]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[BranchDepartment]  WITH CHECK ADD FOREIGN KEY([DepartmentID])
REFERENCES [dbo].[Department] ([ID])
GO
/****** Object:  ForeignKey [FK__BranchShi__Branc__02FC7413]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[BranchShift]  WITH CHECK ADD  CONSTRAINT [FK__BranchShi__Branc__02FC7413] FOREIGN KEY([BranchID])
REFERENCES [dbo].[Branch] ([ID])
GO
ALTER TABLE [dbo].[BranchShift] CHECK CONSTRAINT [FK__BranchShi__Branc__02FC7413]
GO
/****** Object:  ForeignKey [FK__BranchShi__Shift__10566F31]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[BranchShift]  WITH CHECK ADD FOREIGN KEY([ShiftID])
REFERENCES [dbo].[Shift] ([ID])
GO
/****** Object:  ForeignKey [fk_Id_CityState]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[City]  WITH CHECK ADD  CONSTRAINT [fk_Id_CityState] FOREIGN KEY([StateID])
REFERENCES [dbo].[State] ([ID])
GO
ALTER TABLE [dbo].[City] CHECK CONSTRAINT [fk_Id_CityState]
GO
/****** Object:  ForeignKey [FK_Device_Branch]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[Device]  WITH CHECK ADD  CONSTRAINT [FK_Device_Branch] FOREIGN KEY([BranchID])
REFERENCES [dbo].[Branch] ([ID])
GO
ALTER TABLE [dbo].[Device] CHECK CONSTRAINT [FK_Device_Branch]
GO
/****** Object:  ForeignKey [FK_Device_DeviceModal]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[Device]  WITH CHECK ADD  CONSTRAINT [FK_Device_DeviceModal] FOREIGN KEY([DeviceModalID])
REFERENCES [dbo].[DeviceModal] ([ID])
GO
ALTER TABLE [dbo].[Device] CHECK CONSTRAINT [FK_Device_DeviceModal]
GO
/****** Object:  ForeignKey [FK_Leave_OG_LeaveType]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[Leave]  WITH CHECK ADD  CONSTRAINT [FK_Leave_OG_LeaveType] FOREIGN KEY([LeaveTypeID])
REFERENCES [dbo].[OG_LeaveType] ([ID])
GO
ALTER TABLE [dbo].[Leave] CHECK CONSTRAINT [FK_Leave_OG_LeaveType]
GO
/****** Object:  ForeignKey [FK_Leave_User]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[Leave]  WITH CHECK ADD  CONSTRAINT [FK_Leave_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([ID])
GO
ALTER TABLE [dbo].[Leave] CHECK CONSTRAINT [FK_Leave_User]
GO
/****** Object:  ForeignKey [fk_Id_PayrollPayrollCycle]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[Payroll]  WITH CHECK ADD  CONSTRAINT [fk_Id_PayrollPayrollCycle] FOREIGN KEY([PayrollCycleID])
REFERENCES [dbo].[PayrollCycle] ([ID])
GO
ALTER TABLE [dbo].[Payroll] CHECK CONSTRAINT [fk_Id_PayrollPayrollCycle]
GO
/****** Object:  ForeignKey [fk_Id_PayrollDetailPayroll]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[PayrollDetail]  WITH CHECK ADD  CONSTRAINT [fk_Id_PayrollDetailPayroll] FOREIGN KEY([PayrollID])
REFERENCES [dbo].[Payroll] ([ID])
GO
ALTER TABLE [dbo].[PayrollDetail] CHECK CONSTRAINT [fk_Id_PayrollDetailPayroll]
GO
/****** Object:  ForeignKey [fk_Id_PayrollDetailPayrollPolicy]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[PayrollDetail]  WITH CHECK ADD  CONSTRAINT [fk_Id_PayrollDetailPayrollPolicy] FOREIGN KEY([PayrollPolicyID])
REFERENCES [dbo].[PayrollPolicy] ([ID])
GO
ALTER TABLE [dbo].[PayrollDetail] CHECK CONSTRAINT [fk_Id_PayrollDetailPayrollPolicy]
GO
/****** Object:  ForeignKey [fk_Id_PayrollPolicyPayrollVariable]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[PayrollPolicy]  WITH CHECK ADD  CONSTRAINT [fk_Id_PayrollPolicyPayrollVariable] FOREIGN KEY([PayrollVariableID])
REFERENCES [dbo].[PayrollVariable] ([ID])
GO
ALTER TABLE [dbo].[PayrollPolicy] CHECK CONSTRAINT [fk_Id_PayrollPolicyPayrollVariable]
GO
/****** Object:  ForeignKey [fk_Id_ShiftOffDayShift]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[ShiftOffDay]  WITH CHECK ADD  CONSTRAINT [fk_Id_ShiftOffDayShift] FOREIGN KEY([ShiftID])
REFERENCES [dbo].[Shift] ([ID])
GO
ALTER TABLE [dbo].[ShiftOffDay] CHECK CONSTRAINT [fk_Id_ShiftOffDayShift]
GO
/****** Object:  ForeignKey [fk_Id_StateCountry]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[State]  WITH CHECK ADD  CONSTRAINT [fk_Id_StateCountry] FOREIGN KEY([CountryID])
REFERENCES [dbo].[Country] ([ID])
GO
ALTER TABLE [dbo].[State] CHECK CONSTRAINT [fk_Id_StateCountry]
GO
/****** Object:  ForeignKey [fk_Id_UserCity]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [fk_Id_UserCity] FOREIGN KEY([CityID])
REFERENCES [dbo].[City] ([ID])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [fk_Id_UserCity]
GO
/****** Object:  ForeignKey [fk_Id_UserCountry]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [fk_Id_UserCountry] FOREIGN KEY([CountryID])
REFERENCES [dbo].[Country] ([ID])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [fk_Id_UserCountry]
GO
/****** Object:  ForeignKey [fk_Id_UserGender]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [fk_Id_UserGender] FOREIGN KEY([GenderID])
REFERENCES [dbo].[Gender] ([ID])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [fk_Id_UserGender]
GO
/****** Object:  ForeignKey [fk_Id_UserReligion]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [fk_Id_UserReligion] FOREIGN KEY([ReligionID])
REFERENCES [dbo].[Religion] ([ID])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [fk_Id_UserReligion]
GO
/****** Object:  ForeignKey [fk_Id_UserSalaryType]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [fk_Id_UserSalaryType] FOREIGN KEY([SalaryTypeID])
REFERENCES [dbo].[SalaryType] ([ID])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [fk_Id_UserSalaryType]
GO
/****** Object:  ForeignKey [fk_Id_UserState]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [fk_Id_UserState] FOREIGN KEY([StateID])
REFERENCES [dbo].[State] ([ID])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [fk_Id_UserState]
GO
/****** Object:  ForeignKey [fk_Id_UserUserType]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [fk_Id_UserUserType] FOREIGN KEY([UserTypeID])
REFERENCES [dbo].[UserType] ([ID])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [fk_Id_UserUserType]
GO
/****** Object:  ForeignKey [fk_Id_UserContactContactType]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[UserContact]  WITH CHECK ADD  CONSTRAINT [fk_Id_UserContactContactType] FOREIGN KEY([ContactTypeID])
REFERENCES [dbo].[ContactType] ([ID])
GO
ALTER TABLE [dbo].[UserContact] CHECK CONSTRAINT [fk_Id_UserContactContactType]
GO
/****** Object:  ForeignKey [fk_Id_UserContactUser]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[UserContact]  WITH CHECK ADD  CONSTRAINT [fk_Id_UserContactUser] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([ID])
GO
ALTER TABLE [dbo].[UserContact] CHECK CONSTRAINT [fk_Id_UserContactUser]
GO
/****** Object:  ForeignKey [fk_Id_UserDepartmentDepartment]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[UserDepartment]  WITH CHECK ADD  CONSTRAINT [fk_Id_UserDepartmentDepartment] FOREIGN KEY([DepartmentID])
REFERENCES [dbo].[Department] ([ID])
GO
ALTER TABLE [dbo].[UserDepartment] CHECK CONSTRAINT [fk_Id_UserDepartmentDepartment]
GO
/****** Object:  ForeignKey [fk_Id_UserDepartmentUser]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[UserDepartment]  WITH CHECK ADD  CONSTRAINT [fk_Id_UserDepartmentUser] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([ID])
GO
ALTER TABLE [dbo].[UserDepartment] CHECK CONSTRAINT [fk_Id_UserDepartmentUser]
GO
/****** Object:  ForeignKey [fk_Id_UserShiftUser]    Script Date: 03/04/2020 11:47:39 ******/
ALTER TABLE [dbo].[UserShift]  WITH CHECK ADD  CONSTRAINT [fk_Id_UserShiftUser] FOREIGN KEY([UserID])
REFERENCES [dbo].[User] ([ID])
GO
ALTER TABLE [dbo].[UserShift] CHECK CONSTRAINT [fk_Id_UserShiftUser]
GO
