using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMoq;
using Moq;
using NUnit.Framework;
using WikipediaMaze.Core;
using WikipediaMaze.Data;
using WikipediaMaze.Services.Implementations.BadgeAwarders;

namespace WikipediaMaze.Services.Tests.Badges
{
    [TestFixture]
    public class AwardEnigmatistBadgeTest
    {
        [Test]
        public void FailsForWrongActionType()
        {
            var moqer = new AutoMoqer();

            var awarder = new AwardEnigmatistBadge();
            awarder.Repository = moqer.GetMock<IRepository>().Object;

            var result = awarder.AwardBadge(new UserAction
            {
                Action = UserActionType.SolvedPuzzle
            });

            Assert.IsFalse(result);
        }

        [Test]
        public void AllowsMultiples()
        {
            var moqer = new AutoMoqer();
            var badgeInfo = new UserBadgeInfo
                                {
                                    Count = 1,
                                    Name = BadgeType.Dominator.ToString()
                                };
            var user = new User
                {
                    Id = 1,
                    Badges = new List<UserBadgeInfo> { badgeInfo }
                };

            moqer.GetMock<IRepository>().Setup(x => x.All<User>()).Returns(new List<User>{ user }.AsQueryable());

            var awarder = new AwardEnigmatistBadge();
            awarder.Repository = moqer.GetMock<IRepository>().Object;

            moqer.GetMock<IRepository>().Setup(x => x.All<Puzzle>()).Returns(new List<Puzzle>
                                                                                 {
                                                                                     new Puzzle
                                                                                         {
                                                                                             Id = 1,
                                                                                             CreatedById = 1,
                                                                                             VoteCount = 50
                                                                                         }
                                                                                 }.AsQueryable());

            var result = awarder.AwardBadge(new UserAction
            {
                Action = UserActionType.Voted,
                UserId = 1,
                PuzzleId = 1
            });

            Assert.True(result);
            Assert.AreEqual(2, badgeInfo.Count);
        }
    }
}
