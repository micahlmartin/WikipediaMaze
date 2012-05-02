using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WikipediaMaze.Core;
using WikipediaMaze.Data;
using WikipediaMaze.Data.Mongo;
using WikipediaMaze.Data.NHibernate;

namespace WikipediaMaze.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly NHibernateRepository _nhibernateRepository;
        private readonly MongoRepository _mongoRepository;

        public AdminController(NHibernateRepository nhibernateRepository, MongoRepository mongoRepository)
        {

            _nhibernateRepository = nhibernateRepository;
            _mongoRepository = mongoRepository;
        }

        public ActionResult Index()
        {
            using (_nhibernateRepository.OpenSession())
            {
                #region Puzzles
                var puzzles = _nhibernateRepository.All<Puzzle>().ToList().Select(puzzle => new Puzzle
                                                                    {
                                                                        Id = puzzle.Id,
                                                                        DateCreated = puzzle.DateCreated,
                                                                        EndTopic = puzzle.EndTopic,
                                                                        IsVerified = puzzle.IsVerified,
                                                                        LeaderId = puzzle.LeaderId,
                                                                        Level = puzzle.Level,
                                                                        SolutionCount = puzzle.SolutionCount,
                                                                        StartTopic = puzzle.StartTopic,
                                                                        Themes = _nhibernateRepository.All<PuzzleTheme>().Where(x => x.Puzzle.Id == puzzle.Id).Select(x => x.Theme).ToList(),
                                                                        VoteCount = puzzle.VoteCount,
                                                                        CreatedById = puzzle.User.Id
                                                                    }).ToList();


                _mongoRepository.InsertBatch(puzzles);

                #endregion

                #region Users

                var users = _nhibernateRepository.All<User>().ToList().Select(user => new User
                                                                                          {
                                                                                              Id = user.Id,
                                                                                              LastVisit = user.LastVisit,
                                                                                              Reputation = user.Reputation,
                                                                                              Badges = GetUserBadges(user.Id),
                                                                                              RealName = user.RealName,
                                                                                              DateCreated = user.DateCreated,
                                                                                              BirthDate = user.BirthDate,
                                                                                              Location = user.Location,
                                                                                              Email = user.Email,
                                                                                              Photo = user.Photo,
                                                                                              DisplayName = user.DisplayName,
                                                                                              PreferredUserName = user.PreferredUserName,
                                                                                              LeadingPuzzleCount = user.LeadingPuzzleCount,
                                                                                              TwitterUserName = user.TwitterUserName,
                                                                                              OpenIdentifiers = GetOpenIdentifiers(user),
                                                                                          });

                _mongoRepository.InsertBatch(users);

                #endregion

                #region Solutions

                var solutions = _nhibernateRepository.All<Solution>().ToList().Select(solution => new Solution
                                                                                                      {
                                                                                                          CurrentPuzzleLevel = solution.CurrentPuzzleLevel,
                                                                                                          CurrentSolutionCount = solution.CurrentSolutionCount,
                                                                                                          DateCreated = solution.DateCreated,
                                                                                                          PuzzleId = solution.PuzzleId,
                                                                                                          Id = Guid.NewGuid(),
                                                                                                          PointsAwarded = solution.PointsAwarded,
                                                                                                          StepCount = solution.StepCount,
                                                                                                          Steps = solution.Steps.Select(step => new Step
                                                                                                                                                    {
                                                                                                                                                        StepNumber = step.StepNumber,
                                                                                                                                                        Topic = step.Topic
                                                                                                                                                    }).ToList(),
                                                                                                          UserId = solution.UserId,
                                                                                                      }).ToList();

                _mongoRepository.InsertBatch(solutions);

                #endregion
            }

            return Content("Success");
        }

        private IList<OpenIdentifier> GetOpenIdentifiers(User user)
        {
            var s = _nhibernateRepository.All<OpenIdentifier>().Where(x => x.UserId == user.Id).Select(x => new OpenIdentifier
                                                                                                               {
                                                                                                                   Identifier = x.Identifier,
                                                                                                                   IsPrimary = x.IsPrimary
                                                                                                               }).ToList();
            return s;
        }

        private IList<UserBadgeInfo> GetUserBadges(int userId)
        {
            var badges = _nhibernateRepository.All<UserBadge>().Where(x => x.UserId == userId).ToList();
            var groupedBadges = badges.GroupBy(x => x.BadgeId);

            return (from badgeGroup in groupedBadges
                    let @group = badgeGroup
                    let badge = _nhibernateRepository.All<Badge>().First(x => x.Id == @group.Key)
                    select new UserBadgeInfo
                               {
                                   Count = badgeGroup.Count(),
                                   Description = badge.Description, 
                                   Level = badge.Level, 
                                   Name = badge.Name
                               }).ToList();
        }
    }
}
