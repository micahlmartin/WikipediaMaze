/*
This script was created by Visual Studio on 1/28/2011 at 6:32 PM.
Run this script on [MICAH-LAPTOP.wikipediamaze2] to make it the same as [MICAH-LAPTOP.wikipediamaze].
This script performs its actions in the following order:
1. Disable foreign-key constraints.
2. Perform DELETE commands. 
3. Perform UPDATE commands.
4. Perform INSERT commands.
5. Re-enable foreign-key constraints.
Please back up your target database before running this script.
*/
SET NUMERIC_ROUNDABORT OFF
GO
SET XACT_ABORT, ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
/*Pointer used for text / image updates. This might not be needed, but is declared here just in case*/
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
