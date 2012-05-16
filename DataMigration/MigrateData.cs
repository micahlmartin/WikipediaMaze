using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver.Builders;
using StructureMap;
using WikipediaMaze.App;
using WikipediaMaze.Core;
using WikipediaMaze.Data.Mongo;
using WikipediaMaze.Data.NHibernate;
using WikipediaMaze.Services.Interfaces;
using log4net;

namespace DataMigration
{
    class MigrateData
    {
        private readonly NHibernateRepository _nhibernateRepository;
        private readonly MongoRepository _mongoRepository;
        private readonly ILog _logger = LogManager.GetLogger(typeof (MigrateData));

        public MigrateData(NHibernateRepository nhibernateRepository, MongoRepository mongoRepository)
        {
            _nhibernateRepository = nhibernateRepository;
            _mongoRepository = mongoRepository;
                
            MongoRepository.Database.Drop();

            var bootstrapper = new Bootstrapper();
            bootstrapper.BootstrapStructureMap();
        }

        public void Run()
        {
            using (_nhibernateRepository.OpenSession())
            {
                #region Puzzles

                _logger.Info("Migrating Puzzles");

                var puzzles = _nhibernateRepository.All<Puzzle>().ToList().Select(puzzle => new Puzzle
                                                                    {
                                                                        Id = puzzle.Id,
                                                                        DateCreated = puzzle.DateCreated.ToUniversalTime(),
                                                                        EndTopic = puzzle.EndTopic,
                                                                        IsVerified = puzzle.IsVerified,
                                                                        LeaderId = puzzle.LeaderId,
                                                                        Level = puzzle.Level,
                                                                        SolutionCount = puzzle.SolutionCount,
                                                                        StartTopic = puzzle.StartTopic,
                                                                        Themes = _nhibernateRepository.All<PuzzleTheme>().Where(x => x.Puzzle.Id == puzzle.Id).Select(x => x.Theme).ToList(),
                                                                        VoteCount = _nhibernateRepository.All<Vote>().Where(x => x.PuzzleId == puzzle.Id).ToList().Sum(x => (int)x.VoteType),
                                                                        CreatedById = puzzle.User.Id,
                                                                        Votes = _nhibernateRepository.All<Vote>().Where(x => x.PuzzleId == puzzle.Id).ToList().Select(vote => new Vote
                                                                                                                                                                 {
                                                                                                                                                                     DateVoted = vote.DateVoted.ToUniversalTime(),
                                                                                                                                                                     UserId = vote.UserId,
                                                                                                                                                                     VoteType = vote.VoteType
                                                                                                                                                                 }).ToList()
                                                                    }).ToList();


                _mongoRepository.InsertBatch(puzzles);

                var maxPuzzleId = puzzles.Max(x => x.Id);
                MongoRepository.Database.GetCollection(MongoRepository.SequenceCollectionName).FindAndModify(Query.EQ("_id", MongoRepository.GetCollectionNamingConvention(typeof(Puzzle))), SortBy.Null, new UpdateBuilder().Set("seq", maxPuzzleId), false, true);

                #endregion

                #region Users

                _logger.Info("Migrating users");

                var users = _nhibernateRepository.All<User>().ToList().Select(user => new User
                                                                                          {
                                                                                              Id = user.Id,
                                                                                              LastVisit = user.LastVisit,
                                                                                              Reputation = user.Reputation,
                                                                                              //Badges = GetUserBadges(user.Id),
                                                                                              RealName = user.RealName,
                                                                                              DateCreated = user.DateCreated.ToUniversalTime(),
                                                                                              BirthDate = user.BirthDate,
                                                                                              Location = user.Location,
                                                                                              Email = user.Email,
                                                                                              Photo = user.Photo,
                                                                                              DisplayName = user.DisplayName,
                                                                                              PreferredUserName = user.PreferredUserName,
                                                                                              LeadingPuzzleCount = user.LeadingPuzzleCount,
                                                                                              TwitterUserName = user.TwitterUserName,
                                                                                              OpenIdentifiers = GetOpenIdentifiers(user),
                                                                                              Notifications = _nhibernateRepository.All<Notification>().Where(x => x.UserId == user.Id).ToList().Select(notification => new Notification
                                                                                                                                                                                                                            {
                                                                                                                                                                                                                                Id = Guid.NewGuid(),
                                                                                                                                                                                                                                Message = notification.Message
                                                                                                                                                                                                                            }).ToList()
                                                                                          });

                _mongoRepository.InsertBatch(users);

                var maxUserId = users.Max(x => x.Id);
                MongoRepository.Database.GetCollection(MongoRepository.SequenceCollectionName).FindAndModify(Query.EQ("_id", MongoRepository.GetCollectionNamingConvention(typeof(User))), SortBy.Null, new UpdateBuilder().Set("seq", maxUserId), false, true);

                #endregion

                #region Themes

                _logger.Info("Migrating themes");

                var themes = _nhibernateRepository.All<Theme>().ToList();
                _mongoRepository.InsertBatch(themes);

                #endregion

                #region Solutions

                _logger.Info("Migrating solutions");

                var solutions = _nhibernateRepository.All<Solution>().ToList().Select(solution => new Solution
                                                                                                      {
                                                                                                          CurrentPuzzleLevel = solution.CurrentPuzzleLevel,
                                                                                                          CurrentSolutionCount = solution.CurrentSolutionCount,
                                                                                                          DateCreated = solution.DateCreated.ToUniversalTime(),
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

                #region Actions

                _logger.Info("Migrating actions");

                var actions = _nhibernateRepository.All<UserAction>().ToList();

                _mongoRepository.InsertBatch(actions.Select(action => new UserAction
                                                                          {
                                                                              Action = action.Action,
                                                                              Id = Guid.NewGuid(),
                                                                              DateCreated = action.DateCreated,
                                                                              PuzzleId = action.PuzzleId,
                                                                              SolutionId = action.SolutionId,
                                                                              UserId = action.UserId,
                                                                              VoteType = action.VoteType
                                                                          }).ToList());


                
                #endregion

                #region Badges

                var badgeAwarders = ObjectFactory.GetAllInstances<IAwardBadge>();

                foreach (var action in _mongoRepository.All<UserAction>())
                {
                    foreach (var awarder in badgeAwarders)
                        try
                        {
                            awarder.AwardBadge(action);
                        }
                        catch (Exception ex)
                        {
                            _logger.Error("An exception occurred awarding a badge", ex);
                        }
                }
                    

                #endregion
            }
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
