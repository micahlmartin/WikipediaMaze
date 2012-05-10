using System;

namespace WikipediaMaze.Core
{
    public static class Badges
    {
        public static Badge GetBadgeByType(BadgeType type)
        {
            switch (type)
            {
                case BadgeType.Player:
                    return Player;
                case BadgeType.Critic:
                    return Critic;
                case BadgeType.Creator:
                    return Contributor;
                case BadgeType.Riddler:
                    return Riddler;
                case BadgeType.Supporter:
                    return Supporter;
                case BadgeType.Popular:
                    return PopularPuzzle;
                case BadgeType.Notable:
                    return NotablePuzzle;
                case BadgeType.Famous:
                    return FamousPuzzle;
                case BadgeType.Master:
                    return Master;
                case BadgeType.Dominator:
                    return Dominator;
                case BadgeType.Leader:
                    return Leader;
                case BadgeType.Beta:
                    return Beta;
                case BadgeType.Enigmatist:
                    return Enigmatist;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }
        }

        private static Badge _player;
        public static Badge Player
        {
            get
            {
                if (_player == null)
                    _player = new Badge
                                  {
                                      Name = "Player",
                                      Description = "Solved 1st puzzle",
                                      Id = (int)BadgeType.Player,
                                      Level = BadgeLevel.Bronze
                                  };

                return _player;
            }
        }

        private static Badge _critic;
        public static Badge Critic
        {
            get
            {
                if (_critic == null)
                    _critic = new Badge
                                  {
                                      Description = "First Down Vote.",
                                      Level = BadgeLevel.Bronze,
                                      Name = "Critic",
                                      Id = (int)BadgeType.Critic
                                  };

                return _critic;
            }
        }

        private static Badge _contributor;
        public static Badge Contributor
        {
            get
            {
                if (_contributor == null)
                    _contributor = new Badge
                                       {
                                           Description = "Created first puzzle.",
                                           Level = BadgeLevel.Bronze,
                                           Name = "Contributor",
                                           Id = (int)BadgeType.Creator
                                       };

                return _contributor;
            }
        }

        private static Badge _riddler;
        public static Badge Riddler
        {
            get
            {
                if (_riddler == null)
                    _riddler = new Badge
                                   {
                                       Name = "Riddler",
                                       Description = "Created a puzzle with 10+ up-votes.",
                                       Id = (int)BadgeType.Riddler,
                                       Level = BadgeLevel.Bronze
                                   };

                return _riddler;
            }
        }

        private static Badge _supporter;
        public static Badge Supporter
        {
            get
            {
                if (_supporter == null)
                    _supporter = new Badge
                                     {
                                         Description = "First up-vote",
                                         Level = BadgeLevel.Bronze,
                                         Name = "Supporter",
                                         Id = (int)BadgeType.Supporter
                                     };

                return _supporter;
            }
        }

        private static Badge _popularPuzzle;
        public static Badge PopularPuzzle
        {
            get
            {
                if (_popularPuzzle == null)
                    _popularPuzzle = new Badge
                                         {
                                             Name = "Popular Puzzle",
                                             Description = "Created a puzzle with 10+ solutions.",
                                             Id = (int)BadgeType.Popular,
                                             Level = BadgeLevel.Bronze
                                         };

                return _popularPuzzle;
            }   
        }

        private static Badge _notablePuzzle;
        public static Badge NotablePuzzle
        {
            get
            {
                if (_notablePuzzle == null)
                    _notablePuzzle = new Badge
                                         {
                                             Description = "Created a puzzle with 25+ solutions.",
                                             Level = BadgeLevel.Silver,
                                             Name = "Notable Puzzle",
                                             Id = (int)BadgeType.Notable
                                         };

                return _notablePuzzle;
            }
        }

        private static Badge _famousPuzzle;
        public static Badge FamousPuzzle
        {
            get
            {
                if (_famousPuzzle == null)
                    _famousPuzzle = new Badge
                                        {
                                            Description = "Created a puzzle with 100+ solutions.",
                                            Level = BadgeLevel.Gold,
                                            Name = "Famous Puzzle",
                                            Id = (int)BadgeType.Famous
                                        };

                return _famousPuzzle;
            }
        }

        private static Badge _master;
        public static Badge Master
        {
            get
            {
                if (_master == null)
                    _master = new Badge
                                  {
                                      Description = "Leader of 50+ puzzles.",
                                      Level = BadgeLevel.Gold,
                                      Name = "Master",
                                      Id = (int)BadgeType.Master
                                  };

                return _master;
            }
        }

        private static Badge _dominator;
        public static  Badge Dominator
        {
            get
            {
                if (_dominator == null)
                    _dominator = new Badge
                                     {
                                         Description = "Leader of 50+ puzzles.",
                                         Level = BadgeLevel.Silver,
                                         Name = "Dominator",
                                         Id = (int)BadgeType.Dominator
                                     };

                return _dominator;
            }
        }

        private static Badge _leader;
        public static Badge Leader
        {
            get
            {
                if (_leader == null)
                    _leader = new Badge
                                  {
                                      Description = "Leader of 5+ puzzles.",
                                      Level = BadgeLevel.Bronze,
                                      Name = "Leader",
                                      Id = (int)BadgeType.Leader
                                  };

                return _leader;
            }
        }

        private static Badge _beta;
        public static Badge Beta
        {
            get
            {
                if (_beta == null)
                    _beta = new Badge
                                {
                                    Description = "Participated in the Beta",
                                    Level = BadgeLevel.Silver,
                                    Name = "Beta",
                                    Id = (int)BadgeType.Beta
                                };

                return _beta;
            }
        }

        private static Badge _enigmatist;
        public static Badge Enigmatist
        {
            get
            {
                if (_enigmatist == null)
                    _enigmatist = new Badge
                                      {
                                          Description = "Created a puzzle with 25 votes",
                                          Level = BadgeLevel.Gold,
                                          Name = "Enigmatist",
                                          Id = (int)BadgeType.Beta
                                      };

                return _enigmatist;
            }
        }
    }
}