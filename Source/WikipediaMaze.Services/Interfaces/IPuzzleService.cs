using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcContrib.Pagination;
using WikipediaMaze.Core;

namespace WikipediaMaze.Services
{
    public interface IPuzzleService
    {
        void UpdatePuzzleStats(int puzzleId);
        Puzzle GetPuzzleById(int id);
        IPagination<Puzzle> GetPuzzles(PuzzleSortType sort, int page, int pageSize);
        IPagination<PuzzleDetailView> GetPuzzleDetailView(PuzzleSortType sort, int page, int pageSize);
        IPagination<Puzzle> GetPuzzles(PuzzleSortType sort, int page, int pageSize, IEnumerable<string> themes);
        IPagination<PuzzleDetailView> GetPuzzleDetailView(PuzzleSortType sort, int page, int pageSize, IEnumerable<string> themes);
        CreatePuzzleResult CreatePuzzle(string startTopic, string endTopic);
        VoteResult VoteOnPuzzle(int puzzleId, VoteType voteType);
        int UpdatePuzzleVoteCount(int puzzleId);
        IEnumerable<Vote> GetVotes(IEnumerable<int> puzzleIds, int userId);
        IEnumerable<Vote> GetVotes(IEnumerable<Puzzle> puzzles, int userId);
        IEnumerable<Vote> GetVotes(IEnumerable<PuzzleDetailView> puzzles, int userId);
        IEnumerable<int> GetPuzzlesLedByUser(IEnumerable<Puzzle> puzzles, int userId);
        IEnumerable<int> GetPuzzlesLedByUser(int userId);

        IPagination<Puzzle> GetPuzzlesByUserId(int userId, PuzzleSortType sort, int page, int pageSize);
        IEnumerable<Step> GetSteps(int solutionId);

        IEnumerable<Solution> GetSolutions(int puzzleId);
        IEnumerable<Vote> GetVotes(int puzzleId);


        IPagination<SolutionProfile> GetSolutionsByUserId(int userId, SolutionSortType sortType, int page, int pageSize);

        IEnumerable<Theme> GetAllThemes();
        void AddThemesToPuzzle(int puzzleId, int userId, IEnumerable<string> themes);
        IPagination<Theme> GetThemes(int pageSize, int page, ThemeSortType sortType);   

        void UpdateThemeCount();

        void DeletePuzzle(int id);
    }
}
