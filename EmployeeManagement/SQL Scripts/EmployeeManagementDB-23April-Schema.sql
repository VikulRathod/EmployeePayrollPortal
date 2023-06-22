go
create database EmployeeManagementDB
go
USE EmployeeManagementDB
GO
/****** Object:  UserDefinedFunction [dbo].[fn_Age]    Script Date: 4/23/2023 12:03:20 PM ******/
 
GO

GO
CREATE function [dbo].[fn_Age](@dob date)
returns varchar(200)
as
begin
	declare @currentDate date = getdate(),
@tempDate date, @year int, @month int, @days int

set @tempDate = @dob

select @year = DATEDIFF(year, @dob, @currentDate) - 
(
case when (month(@currentDate) < month(@dob) or 
(month(@currentDate) = month(@dob) and day(@currentDate) < day(@dob))) 
then 1 
else 0 end
)
set @tempDate = DATEADD(year, @year, @tempDate)

select @month = DATEDIFF(month, @tempDate, @currentDate) - 
(
case when day(@currentDate) < day(@dob) then 1 else 0 end 
)
set @tempDate = DATEADD(month, @month, @tempDate)

select @days = DATEDIFF(day, @tempDate, @currentDate) -- 11763 days

return cast(@year as varchar(5)) + '.' + 
cast(@month as varchar(5))

--return cast(@year as varchar(5)) + ' years ' + 
--cast(@month as varchar(5)) + ' months ' +
--cast(@days as varchar(5)) + ' days'
end
GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 4/23/2023 12:03:20 PM ******/
 
GO

GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](128) NOT NULL,
	[Email] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEndDateUtc] [datetime] NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Batch]    Script Date: 4/23/2023 12:03:21 PM ******/
 
GO

GO
CREATE TABLE [dbo].[Batch](
	[BatchId] [int] IDENTITY(1,1) NOT NULL,
	[BatchName] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[BatchId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Company]    Script Date: 4/23/2023 12:03:21 PM ******/
 
GO

GO
CREATE TABLE [dbo].[Company](
	[CompanyId] [int] IDENTITY(1,1) NOT NULL,
	[CompanyName] [varchar](100) NULL,
	[CreatedBy] [nvarchar](128) NULL,
	[CreatedDate] [datetime] NULL,
	[ManagerId] [varchar](50) NULL,
	[ManagerName] [varchar](100) NULL,
	[ManagerMobile] [varchar](10) NULL,
	[ManagerOfficialEmail] [varchar](500) NULL,
	[HRId] [varchar](50) NULL,
	[HRName] [varchar](100) NULL,
	[HRMobile] [varchar](10) NULL,
	[HROfficialEmail] [varchar](500) NULL,
	[LeadId] [varchar](50) NULL,
	[LeadName] [varchar](100) NULL,
	[LeadMobile] [varchar](10) NULL,
	[LeadOfficialEmail] [varchar](500) NULL,
	[CompanyLogo] [varbinary](max) NULL,
	[CompanyAddress] [varchar](500) NULL,
	[CompanyOfficialEmail] [varchar](500) NULL,
	[CompanyOfficialContact] [varchar](50) NULL,
	[CompanyWebsite] [varchar](500) NULL,
	[CompanyFounder] [varchar](500) NULL,
	[CompanyFoundedMonthYear] [varchar](100) NULL,
	[CompanyEmployeeStrength] [varchar](500) NULL,
 CONSTRAINT [PK__Company__2D971CACDD8287B0] PRIMARY KEY CLUSTERED 
(
	[CompanyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Employee]    Script Date: 4/23/2023 12:03:21 PM ******/
 
GO

GO
CREATE TABLE [dbo].[Employee](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](128) NULL,
	[FullName] [varchar](200) NULL,
	[Photo] [varbinary](max) NULL,
	[PermanentAddress] [varchar](500) NULL,
	[CurrentAddress] [varchar](500) NULL,
	[PAN] [varchar](100) NULL,
	[Mobile] [varchar](100) NULL,
	[AlternateMobile] [varchar](100) NULL,
	[EmailId] [varchar](100) NULL,
	[AlternateEmailId] [varchar](100) NULL,
	[BloodGroup] [varchar](10) NULL,
	[HighestQualification] [varchar](500) NULL,
	[HighestQualificationPassoutMonthYear] [varchar](500) NULL,
	[Technology] [varchar](2000) NULL,
	[OfferDate] [date] NULL,
	[JoiningDate] [date] NULL,
	[OfferDesignation] [varchar](500) NULL,
	[CurrentDesignation] [varchar](500) NULL,
	[OfferSalary] [numeric](18, 2) NULL,
	[CurrentSalary] [numeric](18, 2) NULL,
	[LastHikeMonthYear] [varchar](500) NULL,
	[ResignationDate] [date] NULL,
	[RelivingDate] [date] NULL,
	[OfficialEmployeeId] [varchar](100) NULL,
	[OfficialEmailId] [varchar](100) NULL,
	[OfficialEmailIdPassword] [varchar](100) NULL,
	[AddedDate] [datetime] NULL,
	[CompanyId] [int] NULL,
	[Gender] [varchar](10) NULL,
	[DateOfBirth] [date] NULL,
	[Adhaar] [varchar](20) NULL,
	[UAN] [varchar](20) NULL,
 CONSTRAINT [PK__Employee__3214EC07C3D75858] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ__Employee__6FAE07827E70FAF8] UNIQUE NONCLUSTERED 
(
	[Mobile] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ__Employee__7ED91ACE36777FD4] UNIQUE NONCLUSTERED 
(
	[EmailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ__Employee__C577943D1D751E58] UNIQUE NONCLUSTERED 
(
	[PAN] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmployeeITExperience]    Script Date: 4/23/2023 12:03:21 PM ******/
 
GO

GO
CREATE TABLE [dbo].[EmployeeITExperience](
	[ExperienceId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](128) NULL,
	[BatchId] [int] NULL,
	[TeamId] [int] NULL,
	[ProjectName] [varchar](500) NULL,
	[ProjectTechnologies] [varchar](1000) NULL,
	[ProjectDescription] [varchar](8000) NULL,
	[JoiningDate] [datetime2](7) NULL,
	[JoiningSalary] [numeric](16, 2) NULL,
	[JoiningDesignation] [varchar](200) NULL,
	[CurrentSalary] [numeric](16, 2) NULL,
	[CurrentDesignation] [varchar](200) NULL,
	[CreatedDate] [datetime2](7) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[DeletedDate] [datetime2](7) NULL,
	[BankAccountId] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ExperienceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmployeeResignation]    Script Date: 4/23/2023 12:03:21 PM ******/
 
GO

GO
CREATE TABLE [dbo].[EmployeeResignation](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](128) NULL,
	[CompanyOffers] [varchar](2000) NULL,
	[ResignationDate] [date] NULL,
	[LastWorkingDay] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Team]    Script Date: 4/23/2023 12:03:21 PM ******/
 
GO

GO
CREATE TABLE [dbo].[Team](
	[TeamId] [int] IDENTITY(1,1) NOT NULL,
	[TeamName] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[TeamId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[EmployeeITProfile]    Script Date: 4/23/2023 12:03:21 PM ******/
 
GO

GO
CREATE view [dbo].[EmployeeITProfile]
as
select 
u.Id,
UPPER(e.FullName) as [Full Name],c.CompanyId, c.CompanyLogo, c.CompanyName as [Company Name], 
e.OfficialEmployeeId as [Employee Id],
e.OfficialEmailId as [Official Email],
e.OfficialEmailIdPassword as [Official Email Password],
c.CompanyAddress, c.CompanyEmployeeStrength,
c.CompanyFoundedMonthYear, c.CompanyFounder,
c.CompanyOfficialContact, c.CompanyOfficialEmail,
c.CompanyWebsite, c.HRId, c.HRName, c.HRMobile, c.HROfficialEmail,
c.LeadId, c.LeadName, c.LeadMobile, c.LeadOfficialEmail, 
c.ManagerId, c.ManagerName, c.ManagerMobile, c.ManagerOfficialEmail,
e.Mobile, e.EmailId as [Email Id],
b.BatchName as [Batch], t.TeamName as [Team],
e.PermanentAddress as [Permanent Address], 
e.CurrentAddress as [Current Address],
e.PAN, e.Adhaar, e.UAN, CAST(it.JoiningDate as date) [Joining Date], 
it.JoiningSalary as [Joining Salary], it.JoiningDesignation as [Joining Designation], 
it.CurrentSalary as [Current Salary], 
it.CurrentDesignation as [Current Designation],
CAST(er.ResignationDate as date) as [Resignation Date], 
CAST(er.LastWorkingDay as date) as [Last Working Day],
it.ProjectName, it.ProjectTechnologies, it.ProjectDescription
from [dbo].[AspNetUsers] u join [dbo].[Employee] e 
on u.Id = e.UserId
join [dbo].[EmployeeITExperience] it 
on u.Id = it.UserId
join Batch b on it.BatchId = b.BatchId
join Team t on it.TeamId = t.TeamId
join Company c on c.CompanyId = e.CompanyId
left join EmployeeResignation er on u.Id = er.UserId
GO
/****** Object:  View [dbo].[EmployeeBgvView]    Script Date: 4/23/2023 12:03:21 PM ******/
 
GO

GO

create view [dbo].[EmployeeBgvView]
as
select 
u.Id,
UPPER(e.FullName) as [Full Name],c.CompanyName as [Company Name], 
e.OfficialEmployeeId as [Employee Id], e.Mobile, e.EmailId as [Email Id],
b.BatchName as [Batch], t.TeamName as [Team],
e.PermanentAddress as [Permanent Address], 
e.CurrentAddress as [Current Address],
e.PAN, e.Adhaar, e.UAN, CAST(it.JoiningDate as date) [Joining Date], 
it.JoiningSalary as [Joining Salary], it.JoiningDesignation as [Joining Designation], 
it.CurrentSalary as [Current Salary], 
it.CurrentDesignation as [Current Designation],
CAST(er.ResignationDate as date) as [Resignation Date], 
CAST(er.LastWorkingDay as date) as [Last Working Day]
from [dbo].[AspNetUsers] u join [dbo].[Employee] e 
on u.Id = e.UserId
join [dbo].[EmployeeITExperience] it 
on u.Id = it.UserId
join Batch b on it.BatchId = b.BatchId
join Team t on it.TeamId = t.TeamId
join Company c on c.CompanyId = e.CompanyId
left join EmployeeResignation er on u.Id = er.UserId
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 4/23/2023 12:03:21 PM ******/
 
GO

GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 4/23/2023 12:03:21 PM ******/
 
GO

GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 4/23/2023 12:03:21 PM ******/
 
GO

GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 4/23/2023 12:03:21 PM ******/
 
GO

GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](128) NOT NULL,
	[RoleId] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CompanyDocumentConfigurableValues]    Script Date: 4/23/2023 12:03:21 PM ******/
 
GO

GO
CREATE TABLE [dbo].[CompanyDocumentConfigurableValues](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CompanyId] [int] NULL,
	[FieldName] [varchar](100) NULL,
	[ValueType] [varchar](50) NULL,
	[FieldValue] [varchar](5000) NULL,
	[AddedDate] [datetime] NULL,
	[CompanyDocumentId] [int] NULL,
	[FieldFormat] [varchar](2000) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CompanyDocuments]    Script Date: 4/23/2023 12:03:21 PM ******/
 
GO

GO
CREATE TABLE [dbo].[CompanyDocuments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DocumentName] [varchar](200) NULL,
	[DocumentPath] [varbinary](max) NULL,
	[CreatedBy] [nvarchar](128) NULL,
	[CreatedDate] [datetime] NULL,
	[CompanyId] [int] NULL,
	[DocumentType] [varchar](10) NULL,
	[CompanyDocumentsTypeId] [int] NULL,
 CONSTRAINT [PK__CompanyD__3214EC07B6632A45] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CompanyDocumentsType]    Script Date: 4/23/2023 12:03:21 PM ******/
 
GO

GO
CREATE TABLE [dbo].[CompanyDocumentsType](
	[DocumentTypeId] [int] IDENTITY(1,1) NOT NULL,
	[DocumentTypeName] [varchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[DocumentTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmployeeBankAccount]    Script Date: 4/23/2023 12:03:21 PM ******/
 
GO

GO
CREATE TABLE [dbo].[EmployeeBankAccount](
	[AccountId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](128) NULL,
	[BankName] [varchar](500) NOT NULL,
	[BranchName] [varchar](200) NULL,
	[AccountNumber] [varchar](50) NOT NULL,
	[IFSCCode] [varchar](20) NOT NULL,
	[CreatedDate] [datetime2](7) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[DeletedDate] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[AccountId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmployeeEducation]    Script Date: 4/23/2023 12:03:21 PM ******/
 
GO

GO
CREATE TABLE [dbo].[EmployeeEducation](
	[EducationId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](128) NULL,
	[EducationTypeId] [int] NULL,
	[CollegeName] [varchar](500) NULL,
	[Percentage] [numeric](18, 2) NULL,
	[PassoutYear] [int] NULL,
	[Specialization] [varchar](200) NULL,
 CONSTRAINT [PK__Employee__4BBE38055FE36C82] PRIMARY KEY CLUSTERED 
(
	[EducationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmployeeEducationType]    Script Date: 4/23/2023 12:03:21 PM ******/
 
GO

GO
CREATE TABLE [dbo].[EmployeeEducationType](
	[EducationTypeId] [int] IDENTITY(1,1) NOT NULL,
	[EducationName] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[EducationTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmployeeExperiece]    Script Date: 4/23/2023 12:03:21 PM ******/
 
GO

GO
CREATE TABLE [dbo].[EmployeeExperiece](
	[ExperienceId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](128) NULL,
	[CompanyName] [varchar](500) NOT NULL,
	[JoiningDate] [datetime2](7) NULL,
	[CurrentCTC] [numeric](18, 2) NULL,
	[RelivingDate] [datetime2](7) NULL,
	[IsPFOpted] [bit] NULL,
	[IsAllDocumentsAvailable] [bit] NULL,
	[AccountId] [int] NULL,
	[CreatedDate] [datetime2](7) NULL,
	[ModifiedDate] [datetime2](7) NULL,
	[DeletedDate] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[ExperienceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmployeeIncrement]    Script Date: 4/23/2023 12:03:21 PM ******/
 
GO

GO
CREATE TABLE [dbo].[EmployeeIncrement](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](128) NULL,
	[HikeMonthYear] [varchar](100) NULL,
	[HikeInPercentage] [decimal](18, 2) NULL,
	[HikeAmount] [numeric](18, 2) NULL,
	[SalaryAfterHike] [numeric](18, 2) NULL,
	[SalaryEffectiveFrom] [date] NULL,
	[ModifiedDate] [datetime] NULL,
 CONSTRAINT [PK__Employee__3214EC071D71C395] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmployeeInterviews]    Script Date: 4/23/2023 12:03:21 PM ******/
 
GO

GO
CREATE TABLE [dbo].[EmployeeInterviews](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](128) NULL,
	[CompanyName] [varchar](2000) NULL,
	[InterviewDate] [date] NULL,
	[InterviewerName] [varchar](200) NULL,
	[InterviewRound] [varchar](200) NULL,
	[InterviewQuestions] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Employee] ADD  CONSTRAINT [DF__Employee__OfferD__4BAC3F29]  DEFAULT ('Trainee Engineer') FOR [OfferDesignation]
GO
ALTER TABLE [dbo].[Employee] ADD  CONSTRAINT [DF__Employee__Curren__4CA06362]  DEFAULT ('Software Engineer') FOR [CurrentDesignation]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[Company]  WITH CHECK ADD  CONSTRAINT [FK__Company__Created__66603565] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Company] CHECK CONSTRAINT [FK__Company__Created__66603565]
GO
ALTER TABLE [dbo].[CompanyDocumentConfigurableValues]  WITH CHECK ADD  CONSTRAINT [FK__CompanyDo__Compa__1AD3FDA4] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([CompanyId])
GO
ALTER TABLE [dbo].[CompanyDocumentConfigurableValues] CHECK CONSTRAINT [FK__CompanyDo__Compa__1AD3FDA4]
GO
ALTER TABLE [dbo].[CompanyDocumentConfigurableValues]  WITH CHECK ADD  CONSTRAINT [FK__CompanyDo__Compa__4D5F7D71] FOREIGN KEY([CompanyDocumentId])
REFERENCES [dbo].[CompanyDocuments] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CompanyDocumentConfigurableValues] CHECK CONSTRAINT [FK__CompanyDo__Compa__4D5F7D71]
GO
ALTER TABLE [dbo].[CompanyDocuments]  WITH CHECK ADD FOREIGN KEY([CompanyDocumentsTypeId])
REFERENCES [dbo].[CompanyDocumentsType] ([DocumentTypeId])
GO
ALTER TABLE [dbo].[CompanyDocuments]  WITH CHECK ADD  CONSTRAINT [FK__CompanyDo__Compa__6C190EBB] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([CompanyId])
GO
ALTER TABLE [dbo].[CompanyDocuments] CHECK CONSTRAINT [FK__CompanyDo__Compa__6C190EBB]
GO
ALTER TABLE [dbo].[CompanyDocuments]  WITH CHECK ADD  CONSTRAINT [FK__CompanyDo__Creat__6B24EA82] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[CompanyDocuments] CHECK CONSTRAINT [FK__CompanyDo__Creat__6B24EA82]
GO
ALTER TABLE [dbo].[Employee]  WITH CHECK ADD  CONSTRAINT [FK__Employee__Compan__6754599E] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Company] ([CompanyId])
GO
ALTER TABLE [dbo].[Employee] CHECK CONSTRAINT [FK__Employee__Compan__6754599E]
GO
ALTER TABLE [dbo].[Employee]  WITH CHECK ADD  CONSTRAINT [FK__Employee__UserId__4AB81AF0] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Employee] CHECK CONSTRAINT [FK__Employee__UserId__4AB81AF0]
GO
ALTER TABLE [dbo].[EmployeeEducation]  WITH CHECK ADD  CONSTRAINT [FK__EmployeeE__Educa__5BE2A6F2] FOREIGN KEY([EducationTypeId])
REFERENCES [dbo].[EmployeeEducationType] ([EducationTypeId])
GO
ALTER TABLE [dbo].[EmployeeEducation] CHECK CONSTRAINT [FK__EmployeeE__Educa__5BE2A6F2]
GO
ALTER TABLE [dbo].[EmployeeEducation]  WITH CHECK ADD  CONSTRAINT [FK__EmployeeE__UserI__5CD6CB2B] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EmployeeEducation] CHECK CONSTRAINT [FK__EmployeeE__UserI__5CD6CB2B]
GO
ALTER TABLE [dbo].[EmployeeExperiece]  WITH CHECK ADD  CONSTRAINT [FK__EmployeeE__Accou__5DCAEF64] FOREIGN KEY([AccountId])
REFERENCES [dbo].[EmployeeBankAccount] ([AccountId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EmployeeExperiece] CHECK CONSTRAINT [FK__EmployeeE__Accou__5DCAEF64]
GO
ALTER TABLE [dbo].[EmployeeExperiece]  WITH CHECK ADD  CONSTRAINT [FK__EmployeeE__UserI__5EBF139D] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EmployeeExperiece] CHECK CONSTRAINT [FK__EmployeeE__UserI__5EBF139D]
GO
ALTER TABLE [dbo].[EmployeeIncrement]  WITH CHECK ADD  CONSTRAINT [FK__EmployeeI__UserI__4F7CD00D] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EmployeeIncrement] CHECK CONSTRAINT [FK__EmployeeI__UserI__4F7CD00D]
GO
ALTER TABLE [dbo].[EmployeeInterviews]  WITH CHECK ADD  CONSTRAINT [FK__EmployeeI__UserI__60A75C0F] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EmployeeInterviews] CHECK CONSTRAINT [FK__EmployeeI__UserI__60A75C0F]
GO
ALTER TABLE [dbo].[EmployeeITExperience]  WITH CHECK ADD  CONSTRAINT [FK__EmployeeI__BankA__6E01572D] FOREIGN KEY([BankAccountId])
REFERENCES [dbo].[EmployeeBankAccount] ([AccountId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EmployeeITExperience] CHECK CONSTRAINT [FK__EmployeeI__BankA__6E01572D]
GO
ALTER TABLE [dbo].[EmployeeITExperience]  WITH CHECK ADD FOREIGN KEY([BatchId])
REFERENCES [dbo].[Batch] ([BatchId])
GO
ALTER TABLE [dbo].[EmployeeITExperience]  WITH CHECK ADD FOREIGN KEY([TeamId])
REFERENCES [dbo].[Team] ([TeamId])
GO
ALTER TABLE [dbo].[EmployeeITExperience]  WITH CHECK ADD  CONSTRAINT [FK__EmployeeI__UserI__6383C8BA] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EmployeeITExperience] CHECK CONSTRAINT [FK__EmployeeI__UserI__6383C8BA]
GO
ALTER TABLE [dbo].[EmployeeResignation]  WITH CHECK ADD  CONSTRAINT [FK__EmployeeR__UserI__6477ECF3] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EmployeeResignation] CHECK CONSTRAINT [FK__EmployeeR__UserI__6477ECF3]
GO
/****** Object:  StoredProcedure [dbo].[usp_GetEmployeeTableColumns]    Script Date: 4/23/2023 12:03:21 PM ******/
 
GO

GO
CREATE proc [dbo].[usp_GetEmployeeTableColumns]
as
begin 
	SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.Columns WHERE TABLE_NAME = 'Employee'
	and COLUMN_NAME not in ('Id', 'UserId')
	union all
	SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.Columns WHERE TABLE_NAME = 'EmployeeIncrement'
	and COLUMN_NAME not in ('Id', 'UserId', 'ModifiedDate')
end 
GO


ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId]
GO

ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId]
GO

ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId]
GO

ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId]
GO

ALTER TABLE [dbo].[Company] CHECK CONSTRAINT [FK__Company__Created__66603565]
GO

ALTER TABLE [dbo].[CompanyDocumentConfigurableValues] CHECK CONSTRAINT [FK__CompanyDo__Compa__1AD3FDA4]
GO

ALTER TABLE [dbo].[CompanyDocumentConfigurableValues] CHECK CONSTRAINT [FK__CompanyDo__Compa__4D5F7D71]
GO
ALTER TABLE [dbo].[CompanyDocuments]  WITH CHECK ADD FOREIGN KEY([CompanyDocumentsTypeId])
REFERENCES [dbo].[CompanyDocumentsType] ([DocumentTypeId])
GO

ALTER TABLE [dbo].[CompanyDocuments] CHECK CONSTRAINT [FK__CompanyDo__Compa__6C190EBB]
GO

ALTER TABLE [dbo].[CompanyDocuments] CHECK CONSTRAINT [FK__CompanyDo__Creat__6B24EA82]
GO

ALTER TABLE [dbo].[Employee] CHECK CONSTRAINT [FK__Employee__Compan__6754599E]
GO

ALTER TABLE [dbo].[Employee] CHECK CONSTRAINT [FK__Employee__UserId__4AB81AF0]
GO

ALTER TABLE [dbo].[EmployeeEducation] CHECK CONSTRAINT [FK__EmployeeE__Educa__5BE2A6F2]
GO

ALTER TABLE [dbo].[EmployeeEducation] CHECK CONSTRAINT [FK__EmployeeE__UserI__5CD6CB2B]
GO

ALTER TABLE [dbo].[EmployeeExperiece] CHECK CONSTRAINT [FK__EmployeeE__Accou__5DCAEF64]
GO

ALTER TABLE [dbo].[EmployeeExperiece] CHECK CONSTRAINT [FK__EmployeeE__UserI__5EBF139D]
GO

ALTER TABLE [dbo].[EmployeeIncrement] CHECK CONSTRAINT [FK__EmployeeI__UserI__4F7CD00D]
GO

ALTER TABLE [dbo].[EmployeeInterviews] CHECK CONSTRAINT [FK__EmployeeI__UserI__60A75C0F]
GO

ALTER TABLE [dbo].[EmployeeITExperience] CHECK CONSTRAINT [FK__EmployeeI__BankA__6E01572D]
GO
ALTER TABLE [dbo].[EmployeeITExperience]  WITH CHECK ADD FOREIGN KEY([BatchId])
REFERENCES [dbo].[Batch] ([BatchId])
GO
ALTER TABLE [dbo].[EmployeeITExperience]  WITH CHECK ADD FOREIGN KEY([TeamId])
REFERENCES [dbo].[Team] ([TeamId])
GO

ALTER TABLE [dbo].[EmployeeITExperience] CHECK CONSTRAINT [FK__EmployeeI__UserI__6383C8BA]
GO

ALTER TABLE [dbo].[EmployeeResignation] CHECK CONSTRAINT [FK__EmployeeR__UserI__6477ECF3]
GO

go

INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'1', N'Super Admin')
INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'2', N'HR')
INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'0', N'Student')
INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'3', N'Admin')
INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'4', N'Manager')
INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'5', N'Lead')
INSERT [dbo].[AspNetRoles] ([Id], [Name]) VALUES (N'6', N'Employee')
go
SET IDENTITY_INSERT [dbo].[Batch] ON 

INSERT [dbo].[Batch] ([BatchId], [BatchName]) VALUES (1, N'DN#20')
INSERT [dbo].[Batch] ([BatchId], [BatchName]) VALUES (2, N'DN#21')
INSERT [dbo].[Batch] ([BatchId], [BatchName]) VALUES (3, N'TESTING')
INSERT [dbo].[Batch] ([BatchId], [BatchName]) VALUES (4, N'JAVA')
INSERT [dbo].[Batch] ([BatchId], [BatchName]) VALUES (5, N'OLD BATCH')
SET IDENTITY_INSERT [dbo].[Batch] OFF
GO
SET IDENTITY_INSERT [dbo].[CompanyDocumentsType] ON 

INSERT [dbo].[CompanyDocumentsType] ([DocumentTypeId], [DocumentTypeName]) VALUES (1, N'Offer Letter')
INSERT [dbo].[CompanyDocumentsType] ([DocumentTypeId], [DocumentTypeName]) VALUES (2, N'Increment Letter')
INSERT [dbo].[CompanyDocumentsType] ([DocumentTypeId], [DocumentTypeName]) VALUES (3, N'Payslips')
INSERT [dbo].[CompanyDocumentsType] ([DocumentTypeId], [DocumentTypeName]) VALUES (4, N'Reliving Letter')
INSERT [dbo].[CompanyDocumentsType] ([DocumentTypeId], [DocumentTypeName]) VALUES (5, N'Experience Letter')
INSERT [dbo].[CompanyDocumentsType] ([DocumentTypeId], [DocumentTypeName]) VALUES (6, N'Promotion Letter')
INSERT [dbo].[CompanyDocumentsType] ([DocumentTypeId], [DocumentTypeName]) VALUES (7, N'Confirmation Letter')
INSERT [dbo].[CompanyDocumentsType] ([DocumentTypeId], [DocumentTypeName]) VALUES (8, N'Internship Joining Letter')
INSERT [dbo].[CompanyDocumentsType] ([DocumentTypeId], [DocumentTypeName]) VALUES (9, N'Internship Completion Letter')
INSERT [dbo].[CompanyDocumentsType] ([DocumentTypeId], [DocumentTypeName]) VALUES (10, N'Training Certificate')
SET IDENTITY_INSERT [dbo].[CompanyDocumentsType] OFF
go
SET IDENTITY_INSERT [dbo].[EmployeeEducationType] ON 

INSERT [dbo].[EmployeeEducationType] ([EducationTypeId], [EducationName]) VALUES (1, N'SSC')
INSERT [dbo].[EmployeeEducationType] ([EducationTypeId], [EducationName]) VALUES (2, N'HSC')
INSERT [dbo].[EmployeeEducationType] ([EducationTypeId], [EducationName]) VALUES (3, N'DIPLOMA')
INSERT [dbo].[EmployeeEducationType] ([EducationTypeId], [EducationName]) VALUES (4, N'BE')
INSERT [dbo].[EmployeeEducationType] ([EducationTypeId], [EducationName]) VALUES (5, N'ME')
INSERT [dbo].[EmployeeEducationType] ([EducationTypeId], [EducationName]) VALUES (6, N'BTech')
INSERT [dbo].[EmployeeEducationType] ([EducationTypeId], [EducationName]) VALUES (7, N'MTech')
INSERT [dbo].[EmployeeEducationType] ([EducationTypeId], [EducationName]) VALUES (8, N'BCA')
INSERT [dbo].[EmployeeEducationType] ([EducationTypeId], [EducationName]) VALUES (9, N'MCA')
INSERT [dbo].[EmployeeEducationType] ([EducationTypeId], [EducationName]) VALUES (10, N'BCS')
INSERT [dbo].[EmployeeEducationType] ([EducationTypeId], [EducationName]) VALUES (11, N'MCS')
INSERT [dbo].[EmployeeEducationType] ([EducationTypeId], [EducationName]) VALUES (12, N'BSc')
INSERT [dbo].[EmployeeEducationType] ([EducationTypeId], [EducationName]) VALUES (13, N'MSc')
INSERT [dbo].[EmployeeEducationType] ([EducationTypeId], [EducationName]) VALUES (14, N'B Com')
INSERT [dbo].[EmployeeEducationType] ([EducationTypeId], [EducationName]) VALUES (15, N'M Com')
INSERT [dbo].[EmployeeEducationType] ([EducationTypeId], [EducationName]) VALUES (16, N'BA')
INSERT [dbo].[EmployeeEducationType] ([EducationTypeId], [EducationName]) VALUES (17, N'MA')
INSERT [dbo].[EmployeeEducationType] ([EducationTypeId], [EducationName]) VALUES (18, N'BBA')
INSERT [dbo].[EmployeeEducationType] ([EducationTypeId], [EducationName]) VALUES (19, N'MBA')
INSERT [dbo].[EmployeeEducationType] ([EducationTypeId], [EducationName]) VALUES (20, N'NOT LISTED')
SET IDENTITY_INSERT [dbo].[EmployeeEducationType] OFF
GO
SET IDENTITY_INSERT [dbo].[Team] ON 

INSERT [dbo].[Team] ([TeamId], [TeamName]) VALUES (1, N'Team 1')
INSERT [dbo].[Team] ([TeamId], [TeamName]) VALUES (2, N'Team 2')
INSERT [dbo].[Team] ([TeamId], [TeamName]) VALUES (3, N'Team 3')
INSERT [dbo].[Team] ([TeamId], [TeamName]) VALUES (4, N'Team 4')
INSERT [dbo].[Team] ([TeamId], [TeamName]) VALUES (5, N'Team 5')
INSERT [dbo].[Team] ([TeamId], [TeamName]) VALUES (6, N'Team 6')
INSERT [dbo].[Team] ([TeamId], [TeamName]) VALUES (7, N'Team 7')
INSERT [dbo].[Team] ([TeamId], [TeamName]) VALUES (8, N'Team 8')
INSERT [dbo].[Team] ([TeamId], [TeamName]) VALUES (9, N'Team 9')
INSERT [dbo].[Team] ([TeamId], [TeamName]) VALUES (10, N'Team 10')
INSERT [dbo].[Team] ([TeamId], [TeamName]) VALUES (11, N'Team 11')
INSERT [dbo].[Team] ([TeamId], [TeamName]) VALUES (12, N'Team 12')
INSERT [dbo].[Team] ([TeamId], [TeamName]) VALUES (13, N'Team 13')
INSERT [dbo].[Team] ([TeamId], [TeamName]) VALUES (14, N'Team 14')
INSERT [dbo].[Team] ([TeamId], [TeamName]) VALUES (15, N'Team 15')
INSERT [dbo].[Team] ([TeamId], [TeamName]) VALUES (16, N'Not Applicable')
SET IDENTITY_INSERT [dbo].[Team] OFF
go

