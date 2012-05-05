using System;

namespace WikipediaMaze.Core
{
    public enum FeedFormat
    {
        Rss,
        Atom,
        Json
    }
    public enum PuzzleSortType
    {
        Newest = 0,
        Solutions = 1,
        Level = 2,
        Votes = 3
    }
    public enum SolutionSortType
    {
        Newest = 0,
        Steps = 1,
        PointsAwarded = 2
    }
    public enum VoteType
    {
        Down = -1,
        None = 0,
        Up = 1
    }
    public enum PlayerSortType
    {
        Reputation = 0,
        Newest = 1,
        Oldest = 2,
        Name = 3
    }
    public enum ThemeSortType
    {
        Popular = 0,
        Alphabetical = 1
    }
    public enum UserActionType
    {
        None = 0,
        Voted = 1,
        ReceivedVote = 2,
        CreatedPuzzle = 3,
        SolvedPuzzle = 4,
        PuzzlePlayed = 5,
        LoggedIn = 6,
        Registered = 7,
        ReTaggedPuzzle = 8,
    }
    public enum BadgeLevel
    {
        Bronze = 0,
        Silver = 1,
        Gold = 2
    }
    public enum BadgeType
    {
        Player = 1,
        Critic = 2,
        Supporter = 3,
        Creator = 4,
        Riddler = 5,
        Popular = 6,
        Leader = 7,
        Beta = 8,
        Notable = 9,
        Dominator = 10,
        Addict = 11,
        Mysterio = 12,
        Yearling = 13,
        Crazed = 14,
        Enigmatist = 15,
        Famous = 16,
        Master = 17
    }
}
