using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WikipediaMaze.Data;
using System.Data.SqlClient;
using WikipediaMaze.Core;

namespace WikipediaMaze.Services
{
    public class UpdatePuzzleSolutionCountService : RecurringServiceBase
    {
        private readonly IRepository _repository;

        public UpdatePuzzleSolutionCountService(IRepository repository)
        {
            _repository = repository;
        }

        public override string ServiceName
        {
            get { return "UpdatePuzzleSolutionCountService"; }
        }

        public override int Interval
        {
            get
            {
                return Settings.UpdatePuzzleSolutionCountInterval;
            }
            set
            {
                base.Interval = value;
            }
        }

        protected override void DoWork()
        {
            foreach (var puzzle in _repository.All<Puzzle>())
            {
                var currentPuzzle = puzzle;
                currentPuzzle.SolutionCount = _repository.All<Solution>().Where(x => x.PuzzleId == currentPuzzle.Id).Select(x => x.UserId).Distinct().Count();
                _repository.Save(currentPuzzle);
            }
        }
    }
}
