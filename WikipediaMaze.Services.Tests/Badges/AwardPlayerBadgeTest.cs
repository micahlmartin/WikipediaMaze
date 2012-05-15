﻿using System;
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
    public class AwardPlayerBadgeTest
    {
        [Test]
        public void FailsForWrongActionType()
        {
            var moqer = new AutoMoqer();

            var awarder = new AwardPlayerBadge();
            awarder.Repository = moqer.GetMock<IRepository>().Object;

            var result = awarder.AwardBadge(new UserAction
            {
                Action = UserActionType.CreatedPuzzle
            });

            Assert.IsFalse(result);
        }

        [Test]
        public void DoesNotAllowMultiples()
        {
            var moqer = new AutoMoqer();
            moqer.GetMock<IRepository>().Setup(x => x.All<User>()).Returns(new List<User>{new User
                                                                                       {
                                                                                           Id = 1,
                                                                                           Badges = new List<UserBadgeInfo>
                                                                                                        {
                                                                                                            new UserBadgeInfo
                                                                                                                {
                                                                                                                    Count = 1,
                                                                                                                    Name = BadgeType.Player.ToString()
                                                                                                                }
                                                                                                        }
                                                                                       }}.AsQueryable());

            var awarder = new AwardPlayerBadge();
            awarder.Repository = moqer.GetMock<IRepository>().Object;

            var result = awarder.AwardBadge(new UserAction
            {
                Action = UserActionType.SolvedPuzzle,
                UserId = 1
            });

            Assert.IsFalse(result);
        }

        //[Test]
        //public void AssignsBadge()
        //{
        //    var moqer = new AutoMoqer();
        //    moqer.GetMock<IRepository>().Setup(x => x.All<User>()).Returns(new List<User>{new User
        //                                                                               {
        //                                                                                   Id = 1,
        //                                                                                   Badges = new List<UserBadgeInfo>(),
               
        //                                                                               }}.AsQueryable());

        //    var awarder = new AwardPlayerBadge();
        //    awarder.Repository = moqer.GetMock<IRepository>().Object;


        //    var result = awarder.AwardBadge(new UserAction
        //    {
        //        Action = UserActionType.SolvedPuzzle,
        //        UserId = 1
        //    });

        //    Assert.IsTrue(result);
        //    moqer.Verify<IRepository>(x => x.Save(It.Is<User>(user =>
        //                                                              user.Badges.Any(
        //                                                                  y =>
        //                                                                  y.Count == 1 &&
        //                                                                  y.Name == BadgeType.Player.ToString())
        //                                                          )), Times.Once());
        //}
    }
}