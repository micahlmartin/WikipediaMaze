USE [master]
GO
/****** Object:  Database [wikipediamaze]    Script Date: 01/28/2011 18:27:08 ******/
CREATE DATABASE [wikipediamaze] ON  PRIMARY 
( NAME = N'wikipediamaze', FILENAME = N'D:\Databases\wikipediamaze.mdf' , SIZE = 7424KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'wikipediamaze_log', FILENAME = N'D:\Databases\wikipediamaze_log.LDF' , SIZE = 8384KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [wikipediamaze] SET COMPATIBILITY_LEVEL = 90
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [wikipediamaze].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [wikipediamaze] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [wikipediamaze] SET ANSI_NULLS ON
GO
ALTER DATABASE [wikipediamaze] SET ANSI_PADDING ON
GO
ALTER DATABASE [wikipediamaze] SET ANSI_WARNINGS ON
GO
ALTER DATABASE [wikipediamaze] SET ARITHABORT ON
GO
ALTER DATABASE [wikipediamaze] SET AUTO_CLOSE OFF
GO
ALTER DATABASE [wikipediamaze] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [wikipediamaze] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [wikipediamaze] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [wikipediamaze] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [wikipediamaze] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [wikipediamaze] SET CONCAT_NULL_YIELDS_NULL ON
GO
ALTER DATABASE [wikipediamaze] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [wikipediamaze] SET QUOTED_IDENTIFIER ON
GO
ALTER DATABASE [wikipediamaze] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [wikipediamaze] SET  DISABLE_BROKER
GO
ALTER DATABASE [wikipediamaze] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [wikipediamaze] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [wikipediamaze] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [wikipediamaze] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [wikipediamaze] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [wikipediamaze] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [wikipediamaze] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [wikipediamaze] SET  READ_WRITE
GO
ALTER DATABASE [wikipediamaze] SET RECOVERY FULL
GO
ALTER DATABASE [wikipediamaze] SET  MULTI_USER
GO
ALTER DATABASE [wikipediamaze] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [wikipediamaze] SET DB_CHAINING OFF
GO
EXEC sys.sp_db_vardecimal_storage_format N'wikipediamaze', N'ON'
GO
USE [wikipediamaze]
GO
/****** Object:  User [mmartin]    Script Date: 01/28/2011 18:27:08 ******/
CREATE USER [mmartin] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[mmartin]
GO
/****** Object:  Table [dbo].[REVISIONS]    Script Date: 01/28/2011 18:27:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[REVISIONS](
	[ID] [int] NOT NULL,
	[VCURRENT] [int] NOT NULL,
	[V1] [int] NOT NULL,
	[V2] [int] NOT NULL,
	[V3] [int] NOT NULL,
	[V4] [int] NOT NULL,
	[V5] [int] NOT NULL,
	[V6] [int] NOT NULL,
	[V7] [int] NOT NULL,
	[V8] [int] NOT NULL,
	[V9] [int] NOT NULL,
	[V10] [int] NOT NULL,
	[V11] [int] NOT NULL,
	[V12] [int] NOT NULL,
	[V13] [int] NOT NULL,
	[V14] [int] NOT NULL,
	[V15] [int] NOT NULL,
	[V16] [int] NOT NULL,
	[V17] [int] NOT NULL,
	[V18] [int] NOT NULL,
	[V19] [int] NOT NULL,
	[V20] [int] NOT NULL,
	[V21] [int] NOT NULL,
	[V22] [int] NOT NULL,
	[V23] [int] NOT NULL,
	[V24] [int] NOT NULL,
	[V25] [int] NOT NULL,
	[V26] [int] NOT NULL,
	[V27] [int] NOT NULL,
	[V28] [int] NOT NULL,
	[V29] [int] NOT NULL,
	[V30] [int] NOT NULL,
	[V31] [int] NOT NULL,
	[V32] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ELMAH_Error]    Script Date: 01/28/2011 18:27:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ELMAH_Error](
	[ErrorId] [uniqueidentifier] NOT NULL,
	[Application] [nvarchar](60) NOT NULL,
	[Host] [nvarchar](50) NOT NULL,
	[Type] [nvarchar](100) NOT NULL,
	[Source] [nvarchar](60) NOT NULL,
	[Message] [nvarchar](500) NOT NULL,
	[User] [nvarchar](50) NOT NULL,
	[StatusCode] [int] NOT NULL,
	[TimeUtc] [datetime] NOT NULL,
	[Sequence] [int] IDENTITY(1,1) NOT NULL,
	[AllXml] [ntext] NOT NULL,
 CONSTRAINT [PK_ELMAH_Error] PRIMARY KEY NONCLUSTERED 
(
	[ErrorId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_ELMAH_Error_App_Time_Seq] ON [dbo].[ELMAH_Error] 
(
	[Application] ASC,
	[TimeUtc] DESC,
	[Sequence] DESC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Badges]    Script Date: 01/28/2011 18:27:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Badges](
	[Id] [int] NOT NULL,
	[Description] [varchar](50) NOT NULL,
	[Name] [varchar](300) NOT NULL,
	[BadgeLevel] [int] NOT NULL,
 CONSTRAINT [PK_Badges] PRIMARY KEY CLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Table [dbo].[Puzzles]    Script Date: 01/28/2011 18:27:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Puzzles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[StartTopic] [varchar](max) NOT NULL,
	[EndTopic] [varchar](max) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[CreatedById] [int] NOT NULL,
	[Level] [int] NOT NULL,
	[VoteCount] [int] NOT NULL,
	[SolutionCount] [int] NOT NULL,
	[IsVerified] [bit] NOT NULL,
	[LeaderId] [int] NULL,
 CONSTRAINT [PK_Puzzles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Table [dbo].[Notifications]    Script Date: 01/28/2011 18:27:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Notifications](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[Message] [varchar](500) NOT NULL,
 CONSTRAINT [PK_Notifications] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Table [dbo].[Log]    Script Date: 01/28/2011 18:27:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Log](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NOT NULL,
	[Thread] [varchar](255) NOT NULL,
	[Level] [varchar](50) NOT NULL,
	[Logger] [varchar](255) NOT NULL,
	[Message] [varchar](4000) NOT NULL,
	[Exception] [varchar](2000) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  StoredProcedure [dbo].[Reputation_CalculateReputationForSolution]    Script Date: 01/28/2011 18:27:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Reputation_CalculateReputationForSolution]
(
	@PuzzleCreatorId int, 
	@SolutionUserId int,
	@StepCount int, 
	@PuzzleLevel int,
	@CalculatedReputation int OUTPUT
)

AS

DECLARE @AverageSolutionReputationValue int = 25
DECLARE @MinimumSolutionReputationValue int = 10

/*
We don't award points for solving your own puzzle.
It's a requirement when you create the puzzle to solve it.
*/
IF @PuzzleCreatorId = @SolutionUserId
BEGIN
	SET @CalculatedReputation = 0
	RETURN
END

/* 
 * Points are awarded based on the number of steps it took to solve
 * the puzzle in relation to the average.
 * If it took fewer steps than normal than they get more points
 * If it took longer than they get fewer points.
 * 
 * First calculatue the difference between the number of steps and the average.
 * This number will be negative if it took longer.
 * Get the percentage of that number in relation to the average.
 * Multiply that percentage by the AverageSolutionReputationValue. 
 * This gives us the initial amount to award the user. 
 * If the user completed the puzzle in fewer steps than average than
 * we double that number to make sure we are awarding a healthy amount of points.
 * If the user completed them in more steps than average, this number is negative.
 * Add that result to the AverageSolutionReputationValue and that gives
 * you your final points to award.
*/
DECLARE @Difference int = @PuzzleLevel - @StepCount
IF @Difference = 0
BEGIN
	SET @CalculatedReputation = @AverageSolutionReputationValue
	RETURN
END

DECLARE @Percentage float = Round(CONVERT(float, @Difference) / CONVERT(float, @PuzzleLevel), 2)
DECLARE @BasePoints float = 0

--The amount of points awarded before the average is applied    
IF(@PuzzleLevel < @StepCount)
	SET @BasePoints = @AverageSolutionReputationValue * @Percentage
ELSE
	SET @BasePoints = @AverageSolutionReputationValue * @Percentage * 2	
	
DECLARE @ReputationToAward int = CONVERT(int, Round(@BasePoints + @AverageSolutionReputationValue, 2))

IF @ReputationToAward > @AverageSolutionReputationValue
	SET @CalculatedReputation = @ReputationToAward
ELSE
BEGIN
	IF @ReputationToAward > @MinimumSolutionReputationValue
		SET @CalculatedReputation = @ReputationToAward
	ELSE
		SET @CalculatedReputation = @MinimumSolutionReputationValue
END
GO
/****** Object:  Table [dbo].[Actions]    Script Date: 01/28/2011 18:27:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Actions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ActionType] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[DateCreated] [datetime] NULL,
	[PuzzleId] [int] NULL,
	[VoteType] [int] NULL,
	[SolutionId] [int] NULL,
	[AffectedUserId] [int] NULL,
	[HasBeenChecked] [bit] NOT NULL,
 CONSTRAINT [PK_Actions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 01/28/2011 18:27:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LastVisit] [datetime] NOT NULL,
	[Reputation] [int] NOT NULL,
	[RealName] [varchar](150) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[BirthDate] [datetime] NULL,
	[Location] [varchar](300) NOT NULL,
	[Email] [varchar](900) NOT NULL,
	[Photo] [varchar](2000) NOT NULL,
	[DisplayName] [varchar](150) NOT NULL,
	[PreferredUserName] [varchar](150) NOT NULL,
	[TwitterUserName] [varchar](15) NULL,
	[LeadingPuzzleCount] [int] NOT NULL,
	[LastActionDate] [datetime] NULL,
 CONSTRAINT [PK_Users_Temp] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Table [dbo].[UserBadges]    Script Date: 01/28/2011 18:27:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserBadges](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BadgeId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK_UserBadges] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[spUpdateRevision]    Script Date: 01/28/2011 18:27:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spUpdateRevision]
 (
    @ID         int OUTPUT,
    @VCURRENT   int,
    @V1         int,    @V2         int,    @V3         int,    @V4         int,    @V5         int,
    @V6         int,    @V7         int,    @V8         int,    @V9         int,    @V10         int,
    @V11         int,    @V12         int,    @V13         int,    @V14         int,    @V15         int,
    @V16         int,    @V17         int,    @V18         int,    @V19         int,    @V20         int,
    @V21         int,    @V22         int,    @V23         int,    @V24         int,    @V25         int,
    @V26         int,    @V27         int,    @V28         int,    @V29         int,    @V30         int,
    @V31         int,    @V32         int
 )
 AS
 UPDATE
 Revisions
 SET
    VCURRENT = @VCURRENT,
    V1 = @V1, V2 = @V2, V3 = @V3, V4 = @V4, V5 = @V5, V6 = @V6, V7 = @V7, V8 = @V8, V9 = @V9, V10 = @V10,
    V11 = @V11, V12 = @V12, V13 = @V13, V14 = @V14, V15 = @V15, V16 = @V16, V17 = @V17, V18 = @V18, V19 = @V19, V20 = @V20,
    V21 = @V21, V22 = @V22, V23 = @V23, V24 = @V24, V25 = @V25, V26 = @V26, V27 = @V27, V28 = @V28, V29 = @V29, V30 = @V30,
    V31 = @V31, V32 = @V32
 WHERE
 ID = @ID
GO
/****** Object:  StoredProcedure [dbo].[spLoadRecord_Revision]    Script Date: 01/28/2011 18:27:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  PROCEDURE [dbo].[spLoadRecord_Revision]

(
   @PKID int
)
 AS
 
 SELECT  *
 FROM Revisions
 WHERE ID = @PKID
GO
/****** Object:  Table [dbo].[Solutions]    Script Date: 01/28/2011 18:27:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Solutions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[PuzzleId] [int] NOT NULL,
	[PointsAwarded] [int] NOT NULL,
	[StepCount] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[CurrentPuzzleLevel] [int] NULL,
	[CurrentSolutionCount] [int] NULL,
 CONSTRAINT [PK_Solutions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Visits]    Script Date: 01/28/2011 18:27:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Visits](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[OriginalUrl] [varchar](max) NOT NULL,
	[IPAddress] [varchar](20) NULL,
	[Source] [varchar](50) NULL,
	[Campaign] [varchar](50) NULL,
	[UserAgent] [varchar](500) NULL,
	[Keyword] [varchar](100) NULL,
	[UrlReferrer] [varchar](max) NULL,
 CONSTRAINT [PK_Visits] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Table [dbo].[OpenIdentifiers]    Script Date: 01/28/2011 18:27:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[OpenIdentifiers](
	[Identifier] [varchar](900) NOT NULL,
	[UserId] [int] NOT NULL,
	[IsPrimary] [bit] NOT NULL,
 CONSTRAINT [PK_UserProfiles_Temp] PRIMARY KEY CLUSTERED 
(
	[Identifier] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Table [dbo].[Votes]    Script Date: 01/28/2011 18:27:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Votes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[PuzzleId] [int] NOT NULL,
	[VoteType] [int] NOT NULL,
	[DateVoted] [datetime] NOT NULL,
 CONSTRAINT [PK_Votes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[ELMAH_LogError]    Script Date: 01/28/2011 18:27:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ELMAH_LogError]
@ErrorId UNIQUEIDENTIFIER, @Application NVARCHAR (60), @Host NVARCHAR (30), @Type NVARCHAR (100), @Source NVARCHAR (60), @Message NVARCHAR (500), @User NVARCHAR (50), @AllXml NTEXT, @StatusCode INT, @TimeUtc DATETIME
AS
SET NOCOUNT ON

    INSERT
    INTO
        [ELMAH_Error]
        (
            [ErrorId],
            [Application],
            [Host],
            [Type],
            [Source],
            [Message],
            [User],
            [AllXml],
            [StatusCode],
            [TimeUtc]
        )
    VALUES
        (
            @ErrorId,
            @Application,
            @Host,
            @Type,
            @Source,
            @Message,
            @User,
            @AllXml,
            @StatusCode,
            @TimeUtc
        )
GO
/****** Object:  StoredProcedure [dbo].[ELMAH_GetErrorXml]    Script Date: 01/28/2011 18:27:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ELMAH_GetErrorXml]
(
    @Application NVARCHAR(60),
    @ErrorId UNIQUEIDENTIFIER
)
AS

    SET NOCOUNT ON

    SELECT 
        [AllXml]
    FROM 
        [ELMAH_Error]
    WHERE
        [ErrorId] = @ErrorId
    AND
        [Application] = @Application
GO
/****** Object:  StoredProcedure [dbo].[ELMAH_GetErrorsXml]    Script Date: 01/28/2011 18:27:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ELMAH_GetErrorsXml]
(
    @Application NVARCHAR(60),
    @PageIndex INT = 0,
    @PageSize INT = 15,
    @TotalCount INT OUTPUT
)
AS 

    SET NOCOUNT ON

    DECLARE @FirstTimeUTC DATETIME
    DECLARE @FirstSequence INT
    DECLARE @StartRow INT
    DECLARE @StartRowIndex INT

    SELECT 
        @TotalCount = COUNT(1) 
    FROM 
        [ELMAH_Error]
    WHERE 
        [Application] = @Application

    -- Get the ID of the first error for the requested page

    SET @StartRowIndex = @PageIndex * @PageSize + 1

    IF @StartRowIndex <= @TotalCount
    BEGIN

        SET ROWCOUNT @StartRowIndex

        SELECT  
            @FirstTimeUTC = [TimeUtc],
            @FirstSequence = [Sequence]
        FROM 
            [ELMAH_Error]
        WHERE   
            [Application] = @Application
        ORDER BY 
            [TimeUtc] DESC, 
            [Sequence] DESC

    END
    ELSE
    BEGIN

        SET @PageSize = 0

    END

    -- Now set the row count to the requested page size and get
    -- all records below it for the pertaining application.

    SET ROWCOUNT @PageSize

    SELECT 
        errorId     = [ErrorId], 
        application = [Application],
        host        = [Host], 
        type        = [Type],
        source      = [Source],
        message     = [Message],
        [user]      = [User],
        statusCode  = [StatusCode], 
        time        = CONVERT(VARCHAR(50), [TimeUtc], 126) + 'Z'
    FROM 
        [ELMAH_Error] error
    WHERE
        [Application] = @Application
    AND
        [TimeUtc] <= @FirstTimeUTC
    AND 
        [Sequence] <= @FirstSequence
    ORDER BY
        [TimeUtc] DESC, 
        [Sequence] DESC
    FOR
        XML AUTO
GO
/****** Object:  StoredProcedure [dbo].[UpdatePlayerLastActionDate]    Script Date: 01/28/2011 18:27:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdatePlayerLastActionDate]

    AS

    --Reset the last action date
    UPDATE       Users
    SET          LastActionDate = NULL

    DECLARE @UserId			int
    DECLARE @LastActionDate datetime = null
    DECLARE @NumberRecords	int
    DECLARE @RowCount		int

    CREATE TABLE #PlayerList_ActionDate
    (
    RowID	int IDENTITY(1, 1),
    UserId	int
    )

    INSERT INTO #PlayerList_ActionDate (UserId) SELECT Id From Users

    -- Get the number of records in the temporary table
    SET @NumberRecords = @@ROWCOUNT
    SET @RowCount = 1

    WHILE @RowCount <= @NumberRecords
    BEGIN
    SELECT @UserId = UserId FROM #PlayerList_ActionDate WHERE RowID = @RowCount

    UPDATE Users
    SET LastActionDate = (SELECT TOP(1) DateCreated FROM Actions WHERE UserId = @UserId ORDER BY DateCreated desc)
    WHERE Id = @UserId

    SET @RowCount = @RowCount + 1
    END

    DROP TABLE #PlayerList_ActionDate
GO
/****** Object:  Table [dbo].[Themes]    Script Date: 01/28/2011 18:27:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Themes](
	[Name] [varchar](30) NOT NULL,
	[UserId] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[Count] [int] NOT NULL,
 CONSTRAINT [PK_Themes] PRIMARY KEY CLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Table [dbo].[Steps]    Script Date: 01/28/2011 18:27:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Steps](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Topic] [nvarchar](max) NOT NULL,
	[SolutionId] [int] NOT NULL,
	[StepNumber] [int] NOT NULL,
 CONSTRAINT [PK_Step] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[UpdateLevel]    Script Date: 01/28/2011 18:27:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateLevel]

    AS

    UPDATE Puzzles SET SolutionCount = 0

    DECLARE @PuzzleId int
    DECLARE @NumberRecords	int
    DECLARE @RowCount		int

    CREATE TABLE #PuzzleList
    (
    RowID		int IDENTITY(1, 1),
    PuzzleId	int
    )
    INSERT INTO #PuzzleList (PuzzleId) SELECT Id FROM Puzzles
    -- Get the number of records in the temporary table
    SET @NumberRecords = @@ROWCOUNT
    SET @RowCount = 1

    CREATE TABLE #PuzzleSolutions
    (
    PuzzleId	int,
    UserId		int
    )

    WHILE @RowCount <= @NumberRecords
    BEGIN
    SELECT @PuzzleId = PuzzleId FROM #PuzzleList Where RowID = @RowCount

    UPDATE Puzzles SET Level = COALESCE((SELECT AVG(StepCount) as Average FROM Solutions WHERE PuzzleId = @PuzzleID),0) WHERE Id = @PuzzleID

    SET @RowCount = @RowCount + 1
    END

    DROP TABLE #PuzzleList
    DROP TABLE #PuzzleSolutions
GO
/****** Object:  View [dbo].[VoteReputationTotalsView]    Script Date: 01/28/2011 18:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[VoteReputationTotalsView]
AS
SELECT        TOP (100) PERCENT UserId, SUM(PointsAwarded) AS TotalPointsAwarded
FROM            (SELECT        dbo.Users.Id AS UserId, dbo.Votes.VoteType, CASE VoteType WHEN - 1 THEN - 5 ELSE 10 END AS PointsAwarded
                          FROM            dbo.Votes INNER JOIN
                                                    dbo.Puzzles ON dbo.Votes.PuzzleId = dbo.Puzzles.Id INNER JOIN
                                                    dbo.Users ON dbo.Puzzles.CreatedById = dbo.Users.Id) AS PuzzleVotes
GROUP BY UserId
ORDER BY UserId
GO
/****** Object:  StoredProcedure [dbo].[UpdateSolutionCount]    Script Date: 01/28/2011 18:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateSolutionCount]

    AS

    UPDATE Puzzles SET SolutionCount = 0

    DECLARE @PuzzleId int
    DECLARE @NumberRecords	int
    DECLARE @RowCount		int

    CREATE TABLE #PuzzleList
    (
    RowID		int IDENTITY(1, 1),
    PuzzleId	int
    )
    INSERT INTO #PuzzleList (PuzzleId) SELECT Id FROM Puzzles
    -- Get the number of records in the temporary table
    SET @NumberRecords = @@ROWCOUNT
    SET @RowCount = 1

    CREATE TABLE #PuzzleSolutions
    (
    PuzzleId	int,
    UserId		int
    )

    WHILE @RowCount <= @NumberRecords
    BEGIN
    SELECT @PuzzleId = PuzzleId FROM #PuzzleList Where RowID = @RowCount

    DELETE FROM #PuzzleSolutions
    INSERT INTO #PuzzleSolutions (PuzzleId, UserId)
    SELECT PuzzleId, UserId
    FROM   Solutions
    GROUP BY PuzzleId, UserId
    HAVING (PuzzleId = @PuzzleId)

    DECLARE @TimesSolved int
    SET @TimesSolved = (SELECT COUNT(*) FROM #PuzzleSolutions)

    UPDATE Puzzles SET SolutionCount = @TimesSolved WHERE Id = @PuzzleId

    SET @RowCount = @RowCount + 1
    END

    DROP TABLE #PuzzleList
    DROP TABLE #PuzzleSolutions
GO
/****** Object:  StoredProcedure [dbo].[UpdatePuzzleStats]    Script Date: 01/28/2011 18:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdatePuzzleStats]
(
	@PuzzleID	int
)

AS
DECLARE @PuzzleCreatorId int
DECLARE @OldPuzzleLeaderId int

SELECT @OldPuzzleLeaderId = LeaderId, @PuzzleCreatorId = CreatedById FROM Puzzles WHERE Id = @PuzzleID

DECLARE @NewPuzzleLeaderId int = (SELECT TOP (1) UserId FROM dbo.Solutions WHERE (PuzzleId = @PuzzleId) ORDER BY StepCount, DateCreated)
DECLARE @AverageSteps int = (SELECT AVG(StepCount) as Average FROM Solutions WHERE PuzzleId = @PuzzleID)
DECLARE @SolutionCount int = (SELECT COUNT(*) FROM (SELECT UserId FROM Solutions WHERE (PuzzleId = @PuzzleID) GROUP BY UserId) AS SolutionCountList)

Update Puzzles SET Level = @AverageSteps, SolutionCount = @SolutionCount, LeaderId = @NewPuzzleLeaderId WHERE Id = @PuzzleId    

IF @OldPuzzleLeaderId <> @NewPuzzleLeaderId AND @SolutionCount >= 5
BEGIN
	UPDATE Users SET LeadingPuzzleCount = LeadingPuzzleCount -1 WHERE Id = @OldPuzzleLeaderId
	UPDATE Users SET LeadingPuzzleCount = LeadingPuzzleCount +1 WHERE Id = @NewPuzzleLeaderId
END
GO
/****** Object:  View [dbo].[SolutionReputationTotalsView]    Script Date: 01/28/2011 18:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[SolutionReputationTotalsView]
AS
SELECT        UserId, SUM(PointsAwarded) AS PointsAwarded
FROM            (SELECT        TOP (100) PERCENT dbo.Users.Id AS UserId, dbo.Solutions.PuzzleId AS PuzzledId, MAX(dbo.Solutions.PointsAwarded) AS PointsAwarded
                          FROM            dbo.Puzzles INNER JOIN
                                                    dbo.Solutions ON dbo.Puzzles.Id = dbo.Solutions.PuzzleId INNER JOIN
                                                    dbo.Users ON dbo.Solutions.UserId = dbo.Users.Id
                          GROUP BY dbo.Users.Id, dbo.Solutions.PuzzleId
                          ORDER BY UserId) AS TopSulutions
GROUP BY UserId
GO
/****** Object:  View [dbo].[SolutionProfileView]    Script Date: 01/28/2011 18:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[SolutionProfileView]
AS
SELECT        dbo.Solutions.Id, dbo.Solutions.UserId, dbo.Solutions.PuzzleId, dbo.Solutions.PointsAwarded, dbo.Solutions.StepCount, dbo.Solutions.DateCreated, 
                         dbo.Puzzles.StartTopic, dbo.Puzzles.EndTopic
FROM            dbo.Solutions INNER JOIN
                         dbo.Puzzles ON dbo.Solutions.PuzzleId = dbo.Puzzles.Id;
GO
/****** Object:  StoredProcedure [dbo].[Badge_AwardAddictBadge]    Script Date: 01/28/2011 18:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Badge_AwardAddictBadge]
(
	@UserId int
)

AS

DECLARE @AddictBadgeId int = 11

IF EXISTS(SELECT * FROM UserBadges WHERE UserId = @UserId AND BadgeId = @AddictBadgeId)
	RETURN

DECLARE @ThirtyDaysAgo date = DATEADD(day, -30, getdate())

CREATE TABLE #UserActions
(
	DateCreated date
)

INSERT INTO #UserActions(DateCreated)
SELECT DateCreated FROM Actions WHERE UserId = @UserId AND DateCreated <= @ThirtyDaysAgo AND ActionType = 4

DECLARE @DateToCheck date = @ThirtyDaysAgo
DECLARE @AwardBadge bit = 1
DECLARE @Today date = getdate()

WHILE @DateToCheck < getdate() AND @AwardBadge = 1
BEGIN
	IF NOT EXISTS(SELECT * FROM #UserActions WHERE DateCreated = @DateToCheck)
	BEGIN
		SET @AwardBadge = 0
	END
	
	SET @DateToCheck = DATEADD(day, 1, @DateToCheck)
END

IF @AwardBadge = 1
BEGIN
	INSERT INTO UserBadges(BadgeId, UserId)
	VALUES(@AddictBadgeId, @UserId)
	
	INSERT INTO Notifications([Message], UserId)
	VALUES('You have earned the "Addict" Badge', @UserId)
END

DROP TABLE #UserActions
GO
/****** Object:  Table [dbo].[PuzzleThemes]    Script Date: 01/28/2011 18:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PuzzleThemes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Theme] [varchar](30) NOT NULL,
	[PuzzleId] [int] NOT NULL,
 CONSTRAINT [PK_PuzzleThemes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  View [dbo].[LeaderBoard]    Script Date: 01/28/2011 18:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[LeaderBoard]
AS

SELECT 	case 
			WHEN PreferredUserName IS NOT NULL THEN PreferredUserName
			WHEN DisplayName IS NOT NULL THEN DisplayName
			ELSE RealName
		end as UserName,
		Reputation,
		Email,
		LeadingPuzzleCount,
		(
			SELECT COUNT(*) 
			FROM UserBadges ub
				 inner join Badges b on ub.BadgeId = b.Id
			WHERE b.BadgeLevel = 0 and UserId = u.Id
		) as BronzeBadgeCount,
	  	(
	  		SELECT COUNT(*) 
	  		FROM UserBadges ub
				 inner join Badges b on ub.BadgeId = b.Id
			WHERE b.BadgeLevel = 1 and UserId = u.Id
		) as SilverBadgeCount,
	  	(
	  		SELECT COUNT(*) 
	  		FROM UserBadges ub
				 inner join Badges b on ub.BadgeId = b.Id
			WHERE b.BadgeLevel = 2 and UserId = u.Id
		) as GoldBadgeCount
		
FROM Users u
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Users"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 264
               Right = 314
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'LeaderBoard'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'LeaderBoard'
GO
/****** Object:  StoredProcedure [dbo].[GetPuzzleLeader]    Script Date: 01/28/2011 18:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetPuzzleLeader]
    (
    @PuzzleId int,
    @LeadingUserId int OUTPUT
    )

    AS
	
    SET @LeadingUserId = 0

    SELECT TOP (1) @LeadingUserId = UserId
    FROM dbo.Solutions
    WHERE (PuzzleId = @PuzzleId)
    ORDER BY StepCount, DateCreated
GO
/****** Object:  StoredProcedure [dbo].[MergeUsers]    Script Date: 01/28/2011 18:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[MergeUsers]
(
	@OldUserId int,
	@newUserId int
)

AS

Begin

UPDATE    dbo.Votes
SET              UserId = @newUserId
WHERE     (UserId = @OldUserId)

UPDATE    dbo.Solutions
SET              UserId = @newUserId
WHERE     (UserId = @OldUserId)

UPDATE    dbo.Puzzles
SET              CreatedById = @newUserId
WHERE     (CreatedById = @OldUserId)

UPDATE	   dbo.UserProfiles
SET				UserId = @newUserId
WHERE      (UserId = @OldUserId)

DELETE FROM dbo.Users
WHERE		(Id = @OldUserId)

End
GO
/****** Object:  View [dbo].[PuzzleReputationTotalsView]    Script Date: 01/28/2011 18:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[PuzzleReputationTotalsView]
AS
SELECT        TOP (100) PERCENT dbo.Users.Id AS CreatedById, COUNT(dbo.Solutions.UserId) * 5 AS PointsAwarded
FROM            dbo.Puzzles INNER JOIN
                         dbo.Solutions ON dbo.Puzzles.Id = dbo.Solutions.PuzzleId AND dbo.Puzzles.CreatedById <> dbo.Solutions.UserId INNER JOIN
                         dbo.Users ON dbo.Puzzles.CreatedById = dbo.Users.Id
GROUP BY dbo.Users.Id
ORDER BY CreatedById
GO
/****** Object:  StoredProcedure [dbo].[Badge_AwardYearlingBadge]    Script Date: 01/28/2011 18:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Badge_AwardYearlingBadge]
(
	@UserId int
)

AS

DECLARE @YearlingBadgeId int = 13

IF EXISTS(SELECT * FROM UserBadges WHERE BadgeId = @YearlingBadgeId AND UserId = @UserId)
	RETURN
	
DECLARE @CreatedDate date
SELECT @CreatedDate = DateCreated FROM Users WHERE Id = @UserId

DECLARE @OneYearLater date = DATEADD(year, 1, @CreatedDate)
	
DECLARE @LastActionDate date
SELECT TOP(1) @LastActionDate = DateCreated FROM Actions WHERE UserId = @UserId AND (ActionType = 1 OR ActionType = 3 OR ActionType = 4) ORDER BY DateCreated Desc

IF @LastActionDate >= @OneYearLater
BEGIN
	INSERT INTO UserBadges(BadgeId, UserId)
	VALUES(@YearlingBadgeId, @UserId)
	
	INSERT INTO Notifications([Message], UserId)
	VALUES('You have earned the "Yearline" Badge', @UserId)
END
GO
/****** Object:  StoredProcedure [dbo].[Badge_AwardSupporterBadge]    Script Date: 01/28/2011 18:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Badge_AwardSupporterBadge]
(
	@UserId int
)	

AS

DECLARE @SupporterBadgeId int = 3

IF EXISTS(SELECT * FROM UserBadges WHERE BadgeId = @SupporterBadgeId AND UserId = @UserId)
	RETURN

IF EXISTS(SELECT * FROM Votes WHERE VoteType = 1 AND UserId = @UserId)
BEGIN
	INSERT INTO UserBadges(BadgeId, UserId)
	VALUES(@SupporterBadgeId, @UserId)
END
GO
/****** Object:  StoredProcedure [dbo].[Badge_AwardRiddlerBadge]    Script Date: 01/28/2011 18:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Badge_AwardRiddlerBadge]
(
	@UserId int
)

AS

DECLARE @RiddlerBadgeId int = 5
DECLARE @QualifyingPuzzleCount int = 0

SELECT @QualifyingPuzzleCount = COUNT(*) FROM Puzzles WHERE CreatedById = @UserId AND VoteCount >= 5

DECLARE @CurrentBadgeCount int = 0
SELECT @CurrentBadgeCount = COUNT(*) FROM UserBadges WHERE BadgeId = @RiddlerBadgeId AND UserId = @UserId

DECLARE @BadgesToAward int = @QualifyingPuzzleCount - @CurrentBadgeCount
IF @BadgesToAward > 0
BEGIN
	INSERT INTO Notifications([Message], UserId)
	VALUES('You have earned the "Riddler" Badge', @UserId)
	
	WHILE @BadgesToAward > 0
	BEGIN
		INSERT INTO UserBadges(BadgeId, UserId)
		VALUES(@RiddlerBadgeId,@UserId)
		
		SET @BadgesToAward = @BadgesToAward - 1
	END
END
GO
/****** Object:  StoredProcedure [dbo].[Badge_AwardPopularBadge]    Script Date: 01/28/2011 18:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Badge_AwardPopularBadge]
(
	@UserId int
)

AS

DECLARE @PopularBadgeId int = 6

IF EXISTS(SELECT * FROM UserBadges WHERE BadgeId = @PopularBadgeId And UserId = @UserId)
	RETURN

DECLARE @QualifiedPuzzlesCount int = 0
SELECT @QualifiedPuzzlesCount = COUNT(*) FROM Puzzles WHERE SolutionCount >= 10 AND CreatedById = @UserId

DECLARE @BadgeCount int = 0
SELECT @BadgeCount = COUNT(*) FROM UserBadges WHERE BadgeId = @PopularBadgeId AND UserId = @UserId

DECLARE @BadgesToAward int = @QualifiedPuzzlesCount - @BadgeCount
IF @BadgesToAward > 0
BEGIN

	INSERT INTO Notifications([Message], UserId)
	VALUES('You have earned the "Popular" Badge', @UserId)
	
	WHILE @BadgesToAward > 0
	BEGIN
		INSERT INTO UserBadges(BadgeId, UserId)
		VALUES(@PopularBadgeId, @UserId)
		
		SET @BadgesToAward = @BadgesToAward - 1
	END
	
END
GO
/****** Object:  StoredProcedure [dbo].[Badge_AwardPlayerBadge]    Script Date: 01/28/2011 18:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Badge_AwardPlayerBadge]
(
	@UserId int
)

AS

DECLARE @PlayerBadgeId int = 1

IF EXISTS(SELECT * FROM UserBadges WHERE BadgeId = @PlayerBadgeId And UserId = @UserId)
BEGIN
	RETURN
END

DECLARE @SolutionsCount int = 0

SELECT  @SolutionsCount = COUNT(*)
FROM	Solutions INNER JOIN Puzzles ON Solutions.PuzzleId = Puzzles.Id
WHERE   (Solutions.UserId = @UserId) AND (Puzzles.CreatedById <> @UserId)

If @SolutionsCount > 0
BEGIN
	INSERT INTO UserBadges(BadgeId, UserId)
	VALUES(@PlayerBadgeId, @UserId)
	
	INSERT INTO Notifications([Message], UserId)
	VALUES('You have earned the "Player" Badge', @UserId)
END
GO
/****** Object:  StoredProcedure [dbo].[Badge_AwardNotableBadge]    Script Date: 01/28/2011 18:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Badge_AwardNotableBadge]
(
	@UserId int
)

AS

DECLARE @NotableBadgeId int = 9

IF EXISTS(SELECT * FROM UserBadges WHERE BadgeId = @NotableBadgeId And UserId = @UserId)
	RETURN

DECLARE @QualifiedPuzzlesCount int = 0
SELECT @QualifiedPuzzlesCount = COUNT(*) FROM Puzzles WHERE SolutionCount >= 25 AND CreatedById = @UserId

DECLARE @BadgeCount int = 0
SELECT @BadgeCount = COUNT(*) FROM UserBadges WHERE BadgeId = @NotableBadgeId AND UserId = @UserId

DECLARE @BadgesToAward int = @QualifiedPuzzlesCount - @BadgeCount
IF @BadgesToAward > 0
BEGIN

	INSERT INTO Notifications([Message], UserId)
	VALUES('You have earned the "Notable" Badge', @UserId)
	
	WHILE @BadgesToAward > 0
	BEGIN
		INSERT INTO UserBadges(BadgeId, UserId)
		VALUES(@NotableBadgeId, @UserId)
		
		SET @BadgesToAward = @BadgesToAward - 1
	END
	
END
GO
/****** Object:  StoredProcedure [dbo].[Badge_AwardMysterioBadge]    Script Date: 01/28/2011 18:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Badge_AwardMysterioBadge]
(
	@UserId int
)

AS

DECLARE @MysterioBadgeId int = 12
DECLARE @QualifyingPuzzleCount int = 0

SELECT @QualifyingPuzzleCount = COUNT(*) FROM Puzzles WHERE CreatedById = @UserId AND VoteCount >= 25

DECLARE @CurrentBadgeCount int = 0
SELECT @CurrentBadgeCount = COUNT(*) FROM UserBadges WHERE BadgeId = @MysterioBadgeId AND UserId = @UserId

DECLARE @BadgesToAward int = @QualifyingPuzzleCount - @CurrentBadgeCount
IF @BadgesToAward > 0
BEGIN

	INSERT INTO Notifications([Message], UserId)
	VALUES('You have earned the "Mysterio" Badge', @UserId)
		
	WHILE @BadgesToAward > 0
	BEGIN
		INSERT INTO UserBadges(BadgeId, UserId)
		VALUES(@MysterioBadgeId,@UserId)
		
		SET @BadgesToAward = @BadgesToAward - 1
	END
END
GO
/****** Object:  StoredProcedure [dbo].[Badge_AwardMasterBadge]    Script Date: 01/28/2011 18:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Badge_AwardMasterBadge]
(
	@UserId int
)

AS

DECLARE @MasterBadgeId int
SET @MasterBadgeId = 17

	
IF NOT EXISTS(SELECT * FROM UserBadges WHERE UserId = @UserId AND BadgeId = @MasterBadgeId)
BEGIN

	DECLARE @PuzzleLeadCount int

	SELECT @PuzzleLeadCount = LeadingPuzzleCount FROM Users WHERE Id = @UserId
	
	IF @PuzzleLeadCount >= 50
	BEGIN
		INSERT INTO UserBadges(BadgeId, UserId)
		VALUES (@MasterBadgeId, @UserId)
		
		INSERT INTO Notifications([Message], UserId)
		VALUES('You have earned the "Leader" Badge', @UserId)
	END

END
GO
/****** Object:  StoredProcedure [dbo].[Badge_AwardLeaderBadge]    Script Date: 01/28/2011 18:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Badge_AwardLeaderBadge]
(
	@UserId int
)

AS

DECLARE @LeaderBadgeId int
SET @LeaderBadgeId = 7

	
IF NOT EXISTS(SELECT * FROM UserBadges WHERE UserId = @UserId AND BadgeId = @LeaderBadgeId)
BEGIN

	DECLARE @PuzzleLeadCount int

	SELECT @PuzzleLeadCount = LeadingPuzzleCount FROM Users WHERE Id = @UserId
	
	IF @PuzzleLeadCount >= 5
	BEGIN
		INSERT INTO UserBadges(BadgeId, UserId)
		VALUES(@LeaderBadgeId, @UserId)
		
		INSERT INTO Notifications([Message], UserId)
		VALUES('You have earned the "Leader" Badge', @UserId)
	
	END

END
GO
/****** Object:  StoredProcedure [dbo].[Badge_AwardFamousBadge]    Script Date: 01/28/2011 18:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Badge_AwardFamousBadge]
(
	@UserId int
)

AS

DECLARE @FamousBadgeId int = 16

IF EXISTS(SELECT * FROM UserBadges WHERE BadgeId = @FamousBadgeId And UserId = @UserId)
	RETURN

DECLARE @QualifiedPuzzlesCount int = 0
SELECT @QualifiedPuzzlesCount = COUNT(*) FROM Puzzles WHERE SolutionCount >= 50 AND CreatedById = @UserId

DECLARE @BadgeCount int = 0
SELECT @BadgeCount = COUNT(*) FROM UserBadges WHERE BadgeId = @FamousBadgeId AND UserId = @UserId

DECLARE @BadgesToAward int = @QualifiedPuzzlesCount - @BadgeCount
IF @BadgesToAward > 0
BEGIN

	INSERT INTO Notifications([Message], UserId)
	VALUES('You have earned the "Famous" Badge', @UserId)
		
	WHILE @BadgesToAward > 0
	BEGIN
		INSERT INTO UserBadges(BadgeId, UserId)
		VALUES(@FamousBadgeId, @UserId)
		
		SET @BadgesToAward = @BadgesToAward - 1
	END
	
END
GO
/****** Object:  StoredProcedure [dbo].[Badge_AwardEnigmatistBadge]    Script Date: 01/28/2011 18:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Badge_AwardEnigmatistBadge]
(
	@UserId int
)

AS

DECLARE @EnigmatistBadgeId int = 15
DECLARE @QualifyingPuzzleCount int = 0

SELECT @QualifyingPuzzleCount = COUNT(*) FROM Puzzles WHERE CreatedById = @UserId AND VoteCount >= 25

DECLARE @CurrentBadgeCount int = 0
SELECT @CurrentBadgeCount = COUNT(*) FROM UserBadges WHERE BadgeId = @EnigmatistBadgeId AND UserId = @UserId

DECLARE @BadgesToAward int = @QualifyingPuzzleCount - @CurrentBadgeCount
IF @BadgesToAward > 0
BEGIN
	WHILE @BadgesToAward > 0
	BEGIN
		INSERT INTO UserBadges(BadgeId, UserId)
		VALUES(@EnigmatistBadgeId,@UserId)
		
		SET @BadgesToAward = @BadgesToAward - 1
		
		INSERT INTO Notifications([Message], UserId)
		VALUES('You have earned the "Enigmatist" Badge', @UserId)
	END
END
GO
/****** Object:  StoredProcedure [dbo].[Badge_AwardDominatorBadge]    Script Date: 01/28/2011 18:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Badge_AwardDominatorBadge]
(
	@UserId int
)

AS

DECLARE @DominatorBadge int
SET @DominatorBadge = 10

	
IF NOT EXISTS(SELECT * FROM UserBadges WHERE UserId = @UserId AND BadgeId = @DominatorBadge)
BEGIN

	DECLARE @PuzzleLeadCount int

	SELECT @PuzzleLeadCount = LeadingPuzzleCount FROM Users WHERE Id = @UserId
	
	IF @PuzzleLeadCount >= 25
	BEGIN
		INSERT INTO UserBadges(BadgeId, UserId)
		VALUES(@DominatorBadge, @UserId)
		
		INSERT INTO Notifications([Message], UserId)
		VALUES('You have earned the "Dominator" Badge', @UserId)
	END

END
GO
/****** Object:  StoredProcedure [dbo].[Badge_AwardCriticBadge]    Script Date: 01/28/2011 18:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Badge_AwardCriticBadge]
(
	@UserId int
)	

AS

DECLARE @CriticBadgeId int = 2

IF EXISTS(SELECT * FROM UserBadges WHERE BadgeId = @CriticBadgeId AND UserId = @UserId)
	RETURN

IF EXISTS(SELECT * FROM Votes WHERE VoteType = -1 AND UserId = @UserId)
BEGIN
	INSERT INTO UserBadges(BadgeId, UserId)
	VALUES(@CriticBadgeId, @UserId)
					
	INSERT INTO Notifications([Message], UserId)
	VALUES('You have earned the "Critic" Badge', @UserId)
END
GO
/****** Object:  StoredProcedure [dbo].[Badge_AwardCreatorBadge]    Script Date: 01/28/2011 18:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Badge_AwardCreatorBadge]
(
	@UserId int
)

AS

DECLARE @CreatorBadgeId int = 4

IF NOT EXISTS(SELECT * FROM UserBadges WHERE BadgeId = @CreatorBadgeId And UserId = @UserId)
BEGIN
	IF EXISTS(SELECT * FROM Puzzles Where CreatedById = @UserId)
	BEGIN
		INSERT INTO UserBadges(BadgeId,UserId)
		VALUES(@CreatorBadgeId, @UserId)
					
		INSERT INTO Notifications([Message], UserId)
		VALUES('You have earned the "Creator" Badge', @UserId)
	END 
END
GO
/****** Object:  StoredProcedure [dbo].[Badge_AwardCrazedBadge]    Script Date: 01/28/2011 18:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Badge_AwardCrazedBadge]
(
	@UserId int
)

AS

DECLARE @CrazedBadgeId int = 14

IF EXISTS(SELECT * FROM UserBadges WHERE UserId = @UserId AND BadgeId = @CrazedBadgeId)
	RETURN

DECLARE @SixtyDaysAgo date = DATEADD(day, -60, getdate())

CREATE TABLE #UserActions
(
	DateCreated date
)

INSERT INTO #UserActions(DateCreated)
SELECT DateCreated FROM Actions WHERE UserId = @UserId AND DateCreated <= @SixtyDaysAgo AND ActionType = 4

DECLARE @DateToCheck date = @SixtyDaysAgo
DECLARE @AwardBadge bit = 1
DECLARE @Today date = getdate()

WHILE @DateToCheck < getdate() AND @AwardBadge = 1
BEGIN
	IF NOT EXISTS(SELECT * FROM #UserActions WHERE DateCreated = @DateToCheck)
	BEGIN
		SET @AwardBadge = 0
	END
	
	SET @DateToCheck = DATEADD(day, 1, @DateToCheck)
END

IF @AwardBadge = 1
BEGIN
	INSERT INTO UserBadges(BadgeId, UserId)
	VALUES(@CrazedBadgeId, @UserId)
	
	INSERT INTO Notifications([Message], UserId)
	VALUES('You have earned the "Crazed" Badge', @UserId)
END

DROP TABLE #UserActions
GO
/****** Object:  StoredProcedure [dbo].[Badge_AwardBetaBadge]    Script Date: 01/28/2011 18:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Badge_AwardBetaBadge]
(
	@UserId int
)

AS

DECLARE @CutoffDate Date = CONVERT(date,'03/01/2010',101)

DECLARE @BetaBadgeId int = 8

IF EXISTS(SELECT * FROM UserBadges WHERE BadgeId = @BetaBadgeId AND UserId = @UserId)
	RETURN

DECLARE @CreatedDate date
SELECT @CreatedDate = DateCreated FROM Users WHERE Id = @UserId

IF @CreatedDate < @CutoffDate
BEGIN
	INSERT INTO UserBadges(BadgeId, UserId)
	VALUES(@BetaBadgeId, @UserId)
	
		
	INSERT INTO Notifications([Message], UserId)
	VALUES('You have earned the "Beta" Badge', @UserId)
END
GO
/****** Object:  StoredProcedure [dbo].[Badge_AwardBadges]    Script Date: 01/28/2011 18:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Badge_AwardBadges]
(
	@UserId	int
)
AS

    EXEC Badge_AwardDominatorBadge	@UserId
    EXEC Badge_AwardLeaderBadge		@UserId
    EXEC Badge_AwardMasterBadge		@UserId
    EXEC Badge_AwardPlayerBadge		@UserId
    EXEC Badge_AwardCriticBadge		@UserId
    EXEC Badge_AwardSupporterBadge	@UserId
    EXEC Badge_AwardCreatorBadge	@UserId
    EXEC Badge_AwardRiddlerBadge	@UserId
    EXEC Badge_AwardMysterioBadge	@UserId
    EXEC Badge_AwardEnigmatistBadge	@UserId
    EXEC Badge_AwardPopularBadge	@UserId
    EXEC Badge_AwardNotableBadge	@UserId
    EXEC Badge_AwardFamousBadge		@UserId
    EXEC Badge_AwardAddictBadge		@UserId
    EXEC Badge_AwardCrazedBadge		@UserId
    EXEC Badge_AwardBetaBadge		@UserId
    EXEC Badge_AwardYearlingBadge	@UserId
    EXEC Badge_AwardMasterBadge		@UserId
GO
/****** Object:  StoredProcedure [dbo].[UpdateThemeCount]    Script Date: 01/28/2011 18:27:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateThemeCount]

AS

--Reset the number of puzzles for each theme
UPDATE       Themes
SET          [Count] = 0	

DECLARE @ThemeName varchar(30)
DECLARE @ThemeCount int
DECLARE @NumberRecords	int
DECLARE @RowCount		int

CREATE TABLE #ThemeCount
(
	RowID int IDENTITY(1, 1),
	TimesUsed int,
	ThemeName varchar(30)
)

INSERT INTO #ThemeCount (TimesUsed, ThemeName)
SELECT   COUNT(*) AS TimesUsed, Themes.Name
FROM     PuzzleThemes INNER JOIN
         Themes ON PuzzleThemes.Theme = Themes.Name INNER JOIN
         Puzzles ON PuzzleThemes.PuzzleId = Puzzles.Id
WHERE    (Puzzles.IsVerified = 1)
GROUP BY Themes.Name

-- Get the number of records in the temporary table
SET @NumberRecords = @@ROWCOUNT
SET @RowCount = 1

WHILE @RowCount <= @NumberRecords
BEGIN

	SELECT @ThemeCount = TimesUsed, @ThemeName = ThemeName FROM #ThemeCount WHERE RowID = @RowCount
	UPDATE Themes SET [Count] = @ThemeCount WHERE [Name] = @ThemeName

 SET @RowCount = @RowCount + 1
END

DROP TABLE #ThemeCount
GO
/****** Object:  UserDefinedFunction [dbo].[FN_GetThemeList]    Script Date: 01/28/2011 18:27:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		GetThemeList
-- Create date: 1/27/2011
-- =============================================
CREATE FUNCTION [dbo].[FN_GetThemeList]
(
	-- Add the parameters for the function here
	@PuzzleId int
)
RETURNS varchar(MAX)

AS

BEGIN

	-- Declare the return variable here
	DECLARE @Result VARCHAR(MAX) = ''

	DECLARE @ThemeList as Table(RowIndex int IDENTITY(1,1), Theme varchar(30))
	
	INSERT INTO @ThemeList (Theme)
	SELECT Theme from PuzzleThemes where PuzzleId = @PuzzleId
	
	DECLARE @Total int = @@ROWCOUNT
	DECLARE @Index int = 1
	
	WHILE @Index <= @Total
    BEGIN
		
		DECLARE @CurrentTheme varchar(30)
		
		SELECT @CurrentTheme = Theme From @ThemeList Where  RowIndex = @Index
		
		SET @Result = @Result + @CurrentTheme
		
		IF(@Index < @Total)
			SET @Result = @Result + ','
    
		SET @Index = @Index + 1
    
    END
    
    	

	-- Return the result of the function
	RETURN @Result

END
GO
/****** Object:  StoredProcedure [dbo].[UpdatePuzzleLeaderId]    Script Date: 01/28/2011 18:27:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdatePuzzleLeaderId]
(
	@PuzzleId	int
)

AS

DECLARE @LeaderId int
DECLARE @SolutionCount int

SELECT @SolutionCount = SolutionCount FROM Puzzles WHERE Id = @PuzzleId

IF @SolutionCount < 5
	SET @LeaderId = null
ELSE
	EXEC GetPuzzleLeader @PuzzleId, @LeaderId OUTPUT

UPDATE Puzzles SET LeaderId = @LeaderId Where Id = @PuzzleId
GO
/****** Object:  StoredProcedure [dbo].[UpdatePuzzleLeadCount]    Script Date: 01/28/2011 18:27:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdatePuzzleLeadCount]

AS

--Reset the number of puzzles being led for each user
UPDATE       Users
SET          LeadingPuzzleCount = 0

DECLARE @PuzzleId int
DECLARE @NumberRecords	int
DECLARE @RowCount		int
DECLARE @SolutionCount	int

CREATE TABLE #PuzzleList_LeadCount
(
	RowID		int IDENTITY(1, 1),
	PuzzleId	int
)

INSERT INTO #PuzzleList_LeadCount (PuzzleId) SELECT Id FROM Puzzles

-- Get the number of records in the temporary table
SET @NumberRecords = @@ROWCOUNT
SET @RowCount = 1

WHILE @RowCount <= @NumberRecords
BEGIN
	SET @PuzzleId = (SELECT PuzzleId FROM #PuzzleList_LeadCount WHERE RowID = @RowCount)

	SELECT @SolutionCount = SolutionCount FROM Puzzles WHERE Id = @PuzzleId

	IF @SolutionCount >= 5
	BEGIN
		DECLARE @LeadingUserId int
		EXECUTE GetPuzzleLeader @PuzzleId, @LeadingUserId OUTPUT
		UPDATE  Users SET LeadingPuzzleCount = LeadingPuzzleCount + 1 WHERE	Id = @LeadingUserId
	END
	
	SET @RowCount = @RowCount + 1
END

DROP TABLE #PuzzleList_LeadCount
GO
/****** Object:  StoredProcedure [dbo].[Reputation_GetReputationFromVotes]    Script Date: 01/28/2011 18:27:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Reputation_GetReputationFromVotes]
(
	@UserId int,
	@CalculatedReputation int OUTPUT
)
AS
	
SELECT @CalculatedReputation = TotalPointsAwarded 
FROM VoteReputationTotalsView 
WHERE UserId = @UserId
GO
/****** Object:  StoredProcedure [dbo].[Reputation_GetReputationFromPuzzles]    Script Date: 01/28/2011 18:27:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Reputation_GetReputationFromPuzzles]
(
	@UserId int,
	@CalculatedReputation int OUTPUT
)

AS

SELECT @CalculatedReputation = PointsAwarded FROM PuzzleReputationTotalsView Where CreatedById = @UserId
GO
/****** Object:  StoredProcedure [dbo].[Reputation_GetReputationForSolutions]    Script Date: 01/28/2011 18:27:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Reputation_GetReputationForSolutions]
(
	@UserId int,
	@CalculatedReputation int OUTPUT
)

AS
	
SELECT @CalculatedReputation = PointsAwarded 
FROM SolutionReputationTotalsView 
WHERE UserId = @UserId
GO
/****** Object:  StoredProcedure [dbo].[UpdateAllPuzzleLeaderIds]    Script Date: 01/28/2011 18:27:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateAllPuzzleLeaderIds]

AS

DECLARE @NumberRecords	int
DECLARE @RowCount		int
DECLARE @PuzzleId		int

CREATE TABLE #PuzzleList_PuzzleLeaders
(
	RowID		int IDENTITY(1, 1),
	PuzzleId	int
)
INSERT INTO #PuzzleList_PuzzleLeaders (PuzzleId) SELECT Id FROM Puzzles
-- Get the number of records in the temporary table
SET @NumberRecords = @@ROWCOUNT
SET @RowCount = 1


WHILE @RowCount <= @NumberRecords
BEGIN
	SELECT @PuzzleId = PuzzleId FROM #PuzzleList_PuzzleLeaders WHERE RowID = @RowCount
	EXEC UpdatePuzzleLeaderId @PuzzleId

 SET @RowCount = @RowCount + 1
END

DROP TABLE #PuzzleList_PuzzleLeaders
GO
/****** Object:  StoredProcedure [dbo].[Reputation_RecalculateReputation]    Script Date: 01/28/2011 18:27:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Reputation_RecalculateReputation]
(
	@ApplyChanges bit = 0
)

AS


CREATE TABLE #UserReputationTable
(
	UserId int,
	SolutionReputation int,
	VoteReputation int,
	PuzzleReputation int,
	OldReputation int,
	NewReputation int,
	Difference int
)

DECLARE @UserId int 
DECLARE @CurrentReputationScore int
DECLARE @SolutionReputation int
DECLARE @VoteReputation int
DECLARE @PuzzleReputation int
DECLARE @NewReputationScore int

DECLARE RecalculateReputationCursor Cursor FOR 
SELECT Id, Reputation FROM Users

OPEN RecalculateReputationCursor
FETCH NEXT FROM RecalculateReputationCursor INTO @UserId, @CurrentReputationScore

WHILE @@FETCH_STATUS <> -1
BEGIN
	IF @@FETCH_STATUS <> -2
	BEGIN

	SET @SolutionReputation = 0
	SET @VoteReputation = 0
	SET @PuzzleReputation = 0

    EXECUTE	Reputation_GetReputationForSolutions @UserId, @SolutionReputation OUTPUT
	EXECUTE Reputation_GetReputationFromVotes @UserId, @VoteReputation OUTPUT
	EXECUTE Reputation_GetReputationFromPuzzles @UserId, @PuzzleReputation OUTPUT
	
	SET @NewReputationScore = @SolutionReputation + @VoteReputation + @PuzzleReputation
	
	IF @ApplyChanges = 1
	BEGIN
		Update Users SET Reputation = @NewReputationScore WHERE Id = @UserId
	END

	INSERT INTO #UserReputationTable(UserId, SolutionReputation, VoteReputation, PuzzleReputation, OldReputation, NewReputation, Difference)
	VALUES(@UserId, @SolutionReputation, @VoteReputation, @PuzzleReputation, @CurrentReputationScore, @NewReputationScore, @NewReputationScore - @CurrentReputationScore)

	END
FETCH NEXT FROM RecalculateReputationCursor INTO @UserId, @CurrentReputationScore
END

CLOSE RecalculateReputationCursor
DEALLOCATE RecalculateReputationCursor

SELECT * FROM #UserReputationTable ORDER BY Difference
DROP TABLE #UserReputationTable
GO
/****** Object:  StoredProcedure [dbo].[AwardBadges]    Script Date: 01/28/2011 18:27:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AwardBadges]

    AS

    DECLARE @NumberRecords	int
    DECLARE @RowCount		int
    DECLARE @QueueId		int
    DECLARE @UserId			int
	DECLARE @AffectedUserId	int
    
    CREATE TABLE #BadgeQueueList
    (
		RowID   int IDENTITY(1, 1),
		QueueId int,
		UserId  int,
		AffectedUserId int
    )

    INSERT INTO #BadgeQueueList(QueueId, UserId, AffectedUserId)
    SELECT Id, UserId, AffectedUserId FROM Actions WHERE HasBeenChecked = 0

    -- Get the number of records in the temporary table
    SET @NumberRecords = @@ROWCOUNT
    SET @RowCount = 1

    WHILE @RowCount <= @NumberRecords
    BEGIN

    SELECT @QueueId = QueueId, @UserId = UserId, @AffectedUserId = AffectedUserId FROM #BadgeQueueList WHERE RowID = @RowCount

	EXEC Badge_AwardBadges @UserId
  
	IF @AffectedUserId <> NULL
	  EXEC Badge_AwardBadges @AffectedUserId

    UPDATE Actions SET HasBeenChecked = 1 WHERE Id = @QueueId

    SET @RowCount = @RowCount + 1
    END

    DROP TABLE #BadgeQueueList
GO
/****** Object:  StoredProcedure [dbo].[AwardAllBadges]    Script Date: 01/28/2011 18:27:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AwardAllBadges]

AS

DELETE FROM UserBadges

DECLARE @NumberRecords	int
DECLARE @RowCount		int
DECLARE @PlayerId		int

CREATE TABLE #AwardAllBadges_Players
(
	RowID		int IDENTITY(1, 1),
	PlayerId	int
)

INSERT INTO #AwardAllBadges_Players (PlayerId) SELECT Id FROM Users
-- Get the number of records in the temporary table
SET @NumberRecords = @@ROWCOUNT
SET @RowCount = 1


WHILE @RowCount <= @NumberRecords
BEGIN
	SELECT @PlayerId = PlayerId FROM #AwardAllBadges_Players WHERE RowID = @RowCount
	
	EXEC Badge_AwardBadges @PlayerId

 SET @RowCount = @RowCount + 1
END

DROP TABLE #AwardAllBadges_Players
    
DELETE FROM Notifications
GO
/****** Object:  View [dbo].[PuzzleDetailView]    Script Date: 01/28/2011 18:27:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE View [dbo].[PuzzleDetailView]

AS

SELECT	p.Id as PuzzleId, 
		StartTopic, 
		EndTopic, 
		p.DateCreated, 
		CreatedById, 
		[Level], 
		VoteCount, 
		SolutionCount, 
		IsVerified, 
		LeaderId,
		Reputation, 
		case 
			WHEN PreferredUserName IS NOT NULL THEN PreferredUserName
			WHEN DisplayName IS NOT NULL THEN DisplayName
			ELSE RealName
		end as UserName,
		Photo,
		Email,
		LeadingPuzzleCount,
		(
			SELECT COUNT(*) 
			FROM UserBadges ub
				 inner join Badges b on ub.BadgeId = b.Id
			WHERE b.BadgeLevel = 0 and UserId = u.Id
		) as BronzeBadgeCount,
	  	(
	  		SELECT COUNT(*) 
	  		FROM UserBadges ub
				 inner join Badges b on ub.BadgeId = b.Id
			WHERE b.BadgeLevel = 1 and UserId = u.Id
		) as SilverBadgeCount,
	  	(
	  		SELECT COUNT(*) 
	  		FROM UserBadges ub
				 inner join Badges b on ub.BadgeId = b.Id
			WHERE b.BadgeLevel = 2 and UserId = u.Id
		) as GoldBadgeCount,
		(
			SELECT TOP(1) UserId 
			FROM Solutions 
			WHERE PuzzleId = p.Id order by StepCount, DateCreated
		) as PuzzleLeader,
		dbo.FN_GetThemeList(p.Id) as Themes
		
FROM   dbo.Puzzles p
	   inner JOIN Users u on u.Id = p.CreatedById
WHERE IsVerified = 1
GO
/****** Object:  Default [DF_REVISIONS_ID]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[REVISIONS] ADD  CONSTRAINT [DF_REVISIONS_ID]  DEFAULT ((1)) FOR [ID]
GO
/****** Object:  Default [DF_REVISIONS_VCURRENT]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[REVISIONS] ADD  CONSTRAINT [DF_REVISIONS_VCURRENT]  DEFAULT ((1)) FOR [VCURRENT]
GO
/****** Object:  Default [DF_REVISIONS_V1]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[REVISIONS] ADD  CONSTRAINT [DF_REVISIONS_V1]  DEFAULT ((0)) FOR [V1]
GO
/****** Object:  Default [DF_REVISIONS_V2]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[REVISIONS] ADD  CONSTRAINT [DF_REVISIONS_V2]  DEFAULT ((0)) FOR [V2]
GO
/****** Object:  Default [DF_REVISIONS_V3]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[REVISIONS] ADD  CONSTRAINT [DF_REVISIONS_V3]  DEFAULT ((0)) FOR [V3]
GO
/****** Object:  Default [DF_REVISIONS_V4]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[REVISIONS] ADD  CONSTRAINT [DF_REVISIONS_V4]  DEFAULT ((0)) FOR [V4]
GO
/****** Object:  Default [DF_REVISIONS_V5]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[REVISIONS] ADD  CONSTRAINT [DF_REVISIONS_V5]  DEFAULT ((0)) FOR [V5]
GO
/****** Object:  Default [DF_REVISIONS_V6]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[REVISIONS] ADD  CONSTRAINT [DF_REVISIONS_V6]  DEFAULT ((0)) FOR [V6]
GO
/****** Object:  Default [DF_REVISIONS_V7]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[REVISIONS] ADD  CONSTRAINT [DF_REVISIONS_V7]  DEFAULT ((0)) FOR [V7]
GO
/****** Object:  Default [DF_REVISIONS_V8]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[REVISIONS] ADD  CONSTRAINT [DF_REVISIONS_V8]  DEFAULT ((0)) FOR [V8]
GO
/****** Object:  Default [DF_REVISIONS_V9]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[REVISIONS] ADD  CONSTRAINT [DF_REVISIONS_V9]  DEFAULT ((0)) FOR [V9]
GO
/****** Object:  Default [DF_REVISIONS_V10]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[REVISIONS] ADD  CONSTRAINT [DF_REVISIONS_V10]  DEFAULT ((0)) FOR [V10]
GO
/****** Object:  Default [DF_REVISIONS_V11]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[REVISIONS] ADD  CONSTRAINT [DF_REVISIONS_V11]  DEFAULT ((0)) FOR [V11]
GO
/****** Object:  Default [DF_REVISIONS_V12]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[REVISIONS] ADD  CONSTRAINT [DF_REVISIONS_V12]  DEFAULT ((0)) FOR [V12]
GO
/****** Object:  Default [DF_REVISIONS_V13]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[REVISIONS] ADD  CONSTRAINT [DF_REVISIONS_V13]  DEFAULT ((0)) FOR [V13]
GO
/****** Object:  Default [DF_REVISIONS_V14]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[REVISIONS] ADD  CONSTRAINT [DF_REVISIONS_V14]  DEFAULT ((0)) FOR [V14]
GO
/****** Object:  Default [DF_REVISIONS_V15]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[REVISIONS] ADD  CONSTRAINT [DF_REVISIONS_V15]  DEFAULT ((0)) FOR [V15]
GO
/****** Object:  Default [DF_REVISIONS_V16]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[REVISIONS] ADD  CONSTRAINT [DF_REVISIONS_V16]  DEFAULT ((0)) FOR [V16]
GO
/****** Object:  Default [DF_REVISIONS_V17]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[REVISIONS] ADD  CONSTRAINT [DF_REVISIONS_V17]  DEFAULT ((0)) FOR [V17]
GO
/****** Object:  Default [DF_REVISIONS_V18]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[REVISIONS] ADD  CONSTRAINT [DF_REVISIONS_V18]  DEFAULT ((0)) FOR [V18]
GO
/****** Object:  Default [DF_REVISIONS_V19]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[REVISIONS] ADD  CONSTRAINT [DF_REVISIONS_V19]  DEFAULT ((0)) FOR [V19]
GO
/****** Object:  Default [DF_REVISIONS_V20]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[REVISIONS] ADD  CONSTRAINT [DF_REVISIONS_V20]  DEFAULT ((0)) FOR [V20]
GO
/****** Object:  Default [DF_REVISIONS_V21]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[REVISIONS] ADD  CONSTRAINT [DF_REVISIONS_V21]  DEFAULT ((0)) FOR [V21]
GO
/****** Object:  Default [DF_REVISIONS_V22]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[REVISIONS] ADD  CONSTRAINT [DF_REVISIONS_V22]  DEFAULT ((0)) FOR [V22]
GO
/****** Object:  Default [DF_REVISIONS_V23]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[REVISIONS] ADD  CONSTRAINT [DF_REVISIONS_V23]  DEFAULT ((0)) FOR [V23]
GO
/****** Object:  Default [DF_REVISIONS_V24]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[REVISIONS] ADD  CONSTRAINT [DF_REVISIONS_V24]  DEFAULT ((0)) FOR [V24]
GO
/****** Object:  Default [DF_REVISIONS_V25]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[REVISIONS] ADD  CONSTRAINT [DF_REVISIONS_V25]  DEFAULT ((0)) FOR [V25]
GO
/****** Object:  Default [DF_REVISIONS_V26]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[REVISIONS] ADD  CONSTRAINT [DF_REVISIONS_V26]  DEFAULT ((0)) FOR [V26]
GO
/****** Object:  Default [DF_REVISIONS_V27]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[REVISIONS] ADD  CONSTRAINT [DF_REVISIONS_V27]  DEFAULT ((0)) FOR [V27]
GO
/****** Object:  Default [DF_REVISIONS_V28]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[REVISIONS] ADD  CONSTRAINT [DF_REVISIONS_V28]  DEFAULT ((0)) FOR [V28]
GO
/****** Object:  Default [DF_REVISIONS_V29]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[REVISIONS] ADD  CONSTRAINT [DF_REVISIONS_V29]  DEFAULT ((0)) FOR [V29]
GO
/****** Object:  Default [DF_REVISIONS_V30]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[REVISIONS] ADD  CONSTRAINT [DF_REVISIONS_V30]  DEFAULT ((0)) FOR [V30]
GO
/****** Object:  Default [DF_REVISIONS_V31]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[REVISIONS] ADD  CONSTRAINT [DF_REVISIONS_V31]  DEFAULT ((0)) FOR [V31]
GO
/****** Object:  Default [DF_REVISIONS_V32]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[REVISIONS] ADD  CONSTRAINT [DF_REVISIONS_V32]  DEFAULT ((0)) FOR [V32]
GO
/****** Object:  Default [DF_ELMAH_Error_ErrorId]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[ELMAH_Error] ADD  CONSTRAINT [DF_ELMAH_Error_ErrorId]  DEFAULT (newid()) FOR [ErrorId]
GO
/****** Object:  Default [DF_Badges_Name]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[Badges] ADD  CONSTRAINT [DF_Badges_Name]  DEFAULT ('') FOR [Description]
GO
/****** Object:  Default [DF_Puzzles_Level]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[Puzzles] ADD  CONSTRAINT [DF_Puzzles_Level]  DEFAULT ((0)) FOR [Level]
GO
/****** Object:  Default [DF_Puzzles_VoteCount]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[Puzzles] ADD  CONSTRAINT [DF_Puzzles_VoteCount]  DEFAULT ((0)) FOR [VoteCount]
GO
/****** Object:  Default [DF_Puzzles_SolutionCount]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[Puzzles] ADD  CONSTRAINT [DF_Puzzles_SolutionCount]  DEFAULT ((0)) FOR [SolutionCount]
GO
/****** Object:  Default [DF_Puzzles_IsVerified]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[Puzzles] ADD  CONSTRAINT [DF_Puzzles_IsVerified]  DEFAULT ((0)) FOR [IsVerified]
GO
/****** Object:  Default [DF_Actions_HasBeenChecked]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[Actions] ADD  CONSTRAINT [DF_Actions_HasBeenChecked]  DEFAULT ((0)) FOR [HasBeenChecked]
GO
/****** Object:  Default [DF__Users__LastVisit__276EDEB3]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF__Users__LastVisit__276EDEB3]  DEFAULT (getdate()) FOR [LastVisit]
GO
/****** Object:  Default [DF__Users__Reputatio__286302EC]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF__Users__Reputatio__286302EC]  DEFAULT ((0)) FOR [Reputation]
GO
/****** Object:  Default [DF__Users__Name__2A4B4B5E]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF__Users__Name__2A4B4B5E]  DEFAULT ('') FOR [RealName]
GO
/****** Object:  Default [DF__Users__DateCreat__2B3F6F97]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF__Users__DateCreat__2B3F6F97]  DEFAULT (getdate()) FOR [DateCreated]
GO
/****** Object:  Default [DF_Users_Location]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_Location]  DEFAULT ('') FOR [Location]
GO
/****** Object:  Default [DF_Users_Email]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_Email]  DEFAULT ('') FOR [Email]
GO
/****** Object:  Default [DF_Users_Photo]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_Photo]  DEFAULT ('') FOR [Photo]
GO
/****** Object:  Default [DF_Users_DisplayName]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_DisplayName]  DEFAULT ('') FOR [DisplayName]
GO
/****** Object:  Default [DF_Users_PreferredUserName]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_PreferredUserName]  DEFAULT ('') FOR [PreferredUserName]
GO
/****** Object:  Default [DF_Users_TwitterUserName]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_TwitterUserName]  DEFAULT ('') FOR [TwitterUserName]
GO
/****** Object:  Default [DF__Users__LeadingPu__1E6F845E]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((0)) FOR [LeadingPuzzleCount]
GO
/****** Object:  Default [DF_Solutions_PuzzleId]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[Solutions] ADD  CONSTRAINT [DF_Solutions_PuzzleId]  DEFAULT ((0)) FOR [PuzzleId]
GO
/****** Object:  Default [DF_Solutions_PointsAwarded]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[Solutions] ADD  CONSTRAINT [DF_Solutions_PointsAwarded]  DEFAULT ((0)) FOR [PointsAwarded]
GO
/****** Object:  Default [DF_Solutions_StepCount]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[Solutions] ADD  CONSTRAINT [DF_Solutions_StepCount]  DEFAULT ((0)) FOR [StepCount]
GO
/****** Object:  Default [DF_Solutions_CurrentPuzzleLevel]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[Solutions] ADD  CONSTRAINT [DF_Solutions_CurrentPuzzleLevel]  DEFAULT ((0)) FOR [CurrentPuzzleLevel]
GO
/****** Object:  Default [DF_Solutions_CurrentSolutionCount]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[Solutions] ADD  CONSTRAINT [DF_Solutions_CurrentSolutionCount]  DEFAULT ((0)) FOR [CurrentSolutionCount]
GO
/****** Object:  Default [DF_OpenIdentifiers_IsPrimary]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[OpenIdentifiers] ADD  CONSTRAINT [DF_OpenIdentifiers_IsPrimary]  DEFAULT ((1)) FOR [IsPrimary]
GO
/****** Object:  Default [DF_Themes_Count]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[Themes] ADD  CONSTRAINT [DF_Themes_Count]  DEFAULT ((0)) FOR [Count]
GO
/****** Object:  ForeignKey [FK_UserBadges_Users]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[UserBadges]  WITH CHECK ADD  CONSTRAINT [FK_UserBadges_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[UserBadges] CHECK CONSTRAINT [FK_UserBadges_Users]
GO
/****** Object:  ForeignKey [FK_Solutions_Puzzles]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[Solutions]  WITH CHECK ADD  CONSTRAINT [FK_Solutions_Puzzles] FOREIGN KEY([PuzzleId])
REFERENCES [dbo].[Puzzles] ([Id])
GO
ALTER TABLE [dbo].[Solutions] CHECK CONSTRAINT [FK_Solutions_Puzzles]
GO
/****** Object:  ForeignKey [FK_Solutions_Users]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[Solutions]  WITH CHECK ADD  CONSTRAINT [FK_Solutions_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Solutions] CHECK CONSTRAINT [FK_Solutions_Users]
GO
/****** Object:  ForeignKey [FK_Visits_Users]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[Visits]  WITH CHECK ADD  CONSTRAINT [FK_Visits_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Visits] CHECK CONSTRAINT [FK_Visits_Users]
GO
/****** Object:  ForeignKey [FK_OpenIdentifiers_Users1]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[OpenIdentifiers]  WITH CHECK ADD  CONSTRAINT [FK_OpenIdentifiers_Users1] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[OpenIdentifiers] CHECK CONSTRAINT [FK_OpenIdentifiers_Users1]
GO
/****** Object:  ForeignKey [FK_Votes_Puzzles]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[Votes]  WITH CHECK ADD  CONSTRAINT [FK_Votes_Puzzles] FOREIGN KEY([PuzzleId])
REFERENCES [dbo].[Puzzles] ([Id])
GO
ALTER TABLE [dbo].[Votes] CHECK CONSTRAINT [FK_Votes_Puzzles]
GO
/****** Object:  ForeignKey [FK_Votes_Users]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[Votes]  WITH CHECK ADD  CONSTRAINT [FK_Votes_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Votes] CHECK CONSTRAINT [FK_Votes_Users]
GO
/****** Object:  ForeignKey [FK_Themes_Users]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[Themes]  WITH CHECK ADD  CONSTRAINT [FK_Themes_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO
ALTER TABLE [dbo].[Themes] CHECK CONSTRAINT [FK_Themes_Users]
GO
/****** Object:  ForeignKey [FK_Step_Solutions]    Script Date: 01/28/2011 18:27:09 ******/
ALTER TABLE [dbo].[Steps]  WITH CHECK ADD  CONSTRAINT [FK_Step_Solutions] FOREIGN KEY([SolutionId])
REFERENCES [dbo].[Solutions] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Steps] CHECK CONSTRAINT [FK_Step_Solutions]
GO
/****** Object:  ForeignKey [FK_PuzzleThemes_Puzzles]    Script Date: 01/28/2011 18:27:10 ******/
ALTER TABLE [dbo].[PuzzleThemes]  WITH CHECK ADD  CONSTRAINT [FK_PuzzleThemes_Puzzles] FOREIGN KEY([PuzzleId])
REFERENCES [dbo].[Puzzles] ([Id])
GO
ALTER TABLE [dbo].[PuzzleThemes] CHECK CONSTRAINT [FK_PuzzleThemes_Puzzles]
GO
/****** Object:  ForeignKey [FK_PuzzleThemes_Themes]    Script Date: 01/28/2011 18:27:10 ******/
ALTER TABLE [dbo].[PuzzleThemes]  WITH CHECK ADD  CONSTRAINT [FK_PuzzleThemes_Themes] FOREIGN KEY([Theme])
REFERENCES [dbo].[Themes] ([Name])
GO
ALTER TABLE [dbo].[PuzzleThemes] CHECK CONSTRAINT [FK_PuzzleThemes_Themes]
GO

SET NUMERIC_ROUNDABORT OFF
GO
SET XACT_ABORT, ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
DECLARE @pv binary(16)
BEGIN TRANSACTION
ALTER TABLE [dbo].[Visits] DROP CONSTRAINT [FK_Visits_Users]
INSERT INTO [dbo].[Badges] ([Name], [Id], [Description], [BadgeLevel]) VALUES (N'Addict', 11, N'Plays at least 1 puzzle everyday for 30 days', 1)
INSERT INTO [dbo].[Badges] ([Name], [Id], [Description], [BadgeLevel]) VALUES (N'Beta', 8, N'Participated in the Beta release', 1)
INSERT INTO [dbo].[Badges] ([Name], [Id], [Description], [BadgeLevel]) VALUES (N'Crazed', 14, N'Plays at least 1 puzzle everyday for 60 days', 2)
INSERT INTO [dbo].[Badges] ([Name], [Id], [Description], [BadgeLevel]) VALUES (N'Creator', 4, N'Created first puzzle', 0)
INSERT INTO [dbo].[Badges] ([Name], [Id], [Description], [BadgeLevel]) VALUES (N'Critic', 2, N'First down-vote', 0)
INSERT INTO [dbo].[Badges] ([Name], [Id], [Description], [BadgeLevel]) VALUES (N'Dominator', 10, N'Leader of 25+ puzzles (Minimum of 5 solutions)', 1)
INSERT INTO [dbo].[Badges] ([Name], [Id], [Description], [BadgeLevel]) VALUES (N'Enigmatist', 15, N'Creates a puzzle with 50+ votes', 2)
INSERT INTO [dbo].[Badges] ([Name], [Id], [Description], [BadgeLevel]) VALUES (N'Famous', 16, N'Created a puzzle that has been solved 50+ times', 2)
INSERT INTO [dbo].[Badges] ([Name], [Id], [Description], [BadgeLevel]) VALUES (N'Leader', 7, N'Leader of 5+ puzzles (Minimum of 5 solutions)', 0)
INSERT INTO [dbo].[Badges] ([Name], [Id], [Description], [BadgeLevel]) VALUES (N'Master', 17, N'Leader of 50+ puzzles (Minimum of 5 solutions)', 2)
INSERT INTO [dbo].[Badges] ([Name], [Id], [Description], [BadgeLevel]) VALUES (N'Mysterio', 12, N'Created a puzzle with 25+ up-votes', 1)
INSERT INTO [dbo].[Badges] ([Name], [Id], [Description], [BadgeLevel]) VALUES (N'Notable', 9, N'Created a puzzle that has been solved 25+ times', 1)
INSERT INTO [dbo].[Badges] ([Name], [Id], [Description], [BadgeLevel]) VALUES (N'Player', 1, N'Solved first puzzle', 0)
INSERT INTO [dbo].[Badges] ([Name], [Id], [Description], [BadgeLevel]) VALUES (N'Popular', 6, N'Created a puzzle that has been solved 10+ times', 0)
INSERT INTO [dbo].[Badges] ([Name], [Id], [Description], [BadgeLevel]) VALUES (N'Riddler', 5, N'Created a puzzle with 5+ up-votes', 0)
INSERT INTO [dbo].[Badges] ([Name], [Id], [Description], [BadgeLevel]) VALUES (N'Supporter', 3, N'First up-vote', 0)
INSERT INTO [dbo].[Badges] ([Name], [Id], [Description], [BadgeLevel]) VALUES (N'Yearling', 13, N'Active member for 1 year', 1)
ALTER TABLE [dbo].[Visits] ADD CONSTRAINT [FK_Visits_Users] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
COMMIT TRANSACTION
