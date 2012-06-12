/****** Object:  Table [dbo].[Bug]    Script Date: 11/22/2007 21:34:27 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Bug]') AND type in (N'U'))
DROP TABLE [dbo].[Bug]
GO
/****** Object:  Table [dbo].[Bug]    Script Date: 11/22/2007 21:34:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Bug]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Bug](
	[BugId] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](500) NOT NULL,
	[Description] [ntext] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[StatusId] [int] NOT NULL,
	[PriorityId] [int] NOT NULL,
	[TypeId] [int] NOT NULL,
	[ComponentId] [int] NOT NULL,
	[ProjectId] [int] NOT NULL,
	[ResolutionId] [int] NOT NULL,
	[VersionId] [int] NOT NULL,
	[LastUpdate] [datetime] NOT NULL,
	[ReporterUserId] [uniqueidentifier] NOT NULL,
	[AssignedToUserId] [uniqueidentifier] NULL,
	[LastUpdateUserId] [uniqueidentifier] NOT NULL,
	[DueDate] [datetime] NULL CONSTRAINT [DF_Bug_DueDate]  DEFAULT ('1/1/1900 12:00:00 AM'),
	[MilestoneId] [int] NOT NULL,
	[Visibility] [int] NOT NULL,
	[Estimation] [decimal](4, 2) NOT NULL CONSTRAINT [DF_Bug_Estimation]  DEFAULT ((0)),
 CONSTRAINT [PK__Bug__145C0A3F] PRIMARY KEY CLUSTERED 
(
	[BugId] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Bug_Component]') AND parent_object_id = OBJECT_ID(N'[dbo].[Bug]'))
ALTER TABLE [dbo].[Bug]  WITH CHECK ADD  CONSTRAINT [FK_Bug_Component] FOREIGN KEY([ComponentId])
REFERENCES [dbo].[Component] ([ComponentID])
ON DELETE CASCADE
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Bug_Priority]') AND parent_object_id = OBJECT_ID(N'[dbo].[Bug]'))
ALTER TABLE [dbo].[Bug]  WITH CHECK ADD  CONSTRAINT [FK_Bug_Priority] FOREIGN KEY([PriorityId])
REFERENCES [dbo].[Priority] ([PriorityID])
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Bug_Project]') AND parent_object_id = OBJECT_ID(N'[dbo].[Bug]'))
ALTER TABLE [dbo].[Bug]  WITH NOCHECK ADD  CONSTRAINT [FK_Bug_Project] FOREIGN KEY([ProjectId])
REFERENCES [dbo].[Project] ([ProjectID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Bug] CHECK CONSTRAINT [FK_Bug_Project]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Bug_Resolution]') AND parent_object_id = OBJECT_ID(N'[dbo].[Bug]'))
ALTER TABLE [dbo].[Bug]  WITH CHECK ADD  CONSTRAINT [FK_Bug_Resolution] FOREIGN KEY([ResolutionId])
REFERENCES [dbo].[Resolution] ([ResolutionID])
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Bug_Status]') AND parent_object_id = OBJECT_ID(N'[dbo].[Bug]'))
ALTER TABLE [dbo].[Bug]  WITH CHECK ADD  CONSTRAINT [FK_Bug_Status] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status] ([StatusID])
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Bug_Type]') AND parent_object_id = OBJECT_ID(N'[dbo].[Bug]'))
ALTER TABLE [dbo].[Bug]  WITH CHECK ADD  CONSTRAINT [FK_Bug_Type] FOREIGN KEY([TypeId])
REFERENCES [dbo].[Type] ([TypeID])
GO
