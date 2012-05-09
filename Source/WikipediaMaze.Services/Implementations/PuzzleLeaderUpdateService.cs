using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using StructureMap;
using WikipediaMaze.Data;
using WikipediaMaze.Core;
using MongoDB.Driver.Linq;

namespace WikipediaMaze.Services
{
    public class PuzzleLeaderUpdateService : RecurringServiceBase
    {
        private readonly IRepository _repository;

        public PuzzleLeaderUpdateService(IRepository repository)
        {
            _repository = repository;
        }

        public override string ServiceName
        {
            get { return "PuzzleLeaderUpdateService"; }
        }

        public override int Interval
        {
            get
            {
                return Settings.PuzzleLeaderUpdateInterval;
            }
            set
            {
                base.Interval = value;
            }
        }

        protected override void DoWork()
        {
            foreach (var puzzle in _repository.All<Puzzle>().Where(x => x.SolutionCount >= 5))
            {
                puzzle.LeaderId = puzzle.Solutions.OrderBy(x => x.StepCount).ThenBy(x => x.DateCreated).Select(x => x.UserId).First();
                _repository.Save(puzzle);
            }
        }
    }
}
