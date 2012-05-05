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
        IPagination<Puzzle> GetPuzzles(PuzzleSortType sort, int page, int pageSize, IEnumerable<string> themes);
        CreatePuzzleResult CreatePuzzle(string startTopic, string endTopic, IEnumerable<string> themes);
        VoteResult VoteOnPuzzle(int puzzleId, VoteType voteType);
        IEnumerable<int> GetPuzzlesLedByUser(IEnumerable<Puzzle> puzzles, int userId);
        IEnumerable<int> GetPuzzlesLedByUser(int userId);

        IPagination<Puzzle> GetPuzzlesByUserId(int userId, PuzzleSortType sort, int page, int pageSize);
        IEnumerable<Step> GetSteps(int solutionId);

        IEnumerable<Solution> GetSolutions(int puzzleId);

        IPagination<Solution> GetSolutionsByUserId(int userId, SolutionSortType sortType, int page, int pageSize);

        IEnumerable<Theme> GetAllThemes();
        void RetagPuzzle(int puzzleId, int userId, IEnumerable<string> themes);
        IPagination<Theme> GetThemes(int pageSize, int page, ThemeSortType sortType);   

        void UpdateThemeCount();

        void DeletePuzzle(int id);
    }
}
