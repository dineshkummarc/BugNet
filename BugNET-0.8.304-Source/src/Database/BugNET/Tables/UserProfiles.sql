/****** Object:  Table [dbo].[UserProfiles]    Script Date: 09/30/2007 17:43:21 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserProfiles]') AND type in (N'U'))
DROP TABLE [dbo].[UserProfiles]
GO
/****** Object:  Table [dbo].[UserProfiles]    Script Date: 09/30/2007 17:43:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserProfiles]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[UserProfiles](
	[UserName] [nvarchar](50) NOT NULL,
	[FirstName] [nvarchar](100) NULL,
	[LastName] [nvarchar](100) NULL,
	[FullName] [nvarchar](100) NULL,
	[ShowAssignedToMe] [bit] NULL,
	[ShowReportedByMe] [bit] NULL,
	[ShowMonitoredByMe] [bit] NULL,
	[ShowInProgressByMe] [bit] NULL,
	[ShowResolvedByMe] [bit] NULL,
	[ShowClosedByMe] [bit] NULL,
	[IssuesPageSize] [int] NULL,
	[MyIssuesPageSize] [int] NULL,
	[LastUpdate] [datetime] NOT NULL,
 CONSTRAINT [PK_UserProfiles] PRIMARY KEY CLUSTERED 
(
	[UserName] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
