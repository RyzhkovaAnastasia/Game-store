using OnlineGameStore.DAL.Entities;
using OnlineGameStore.DAL.Interfaces;
using System;
using System.Collections.Generic;

namespace OnlineGameStore.DAL.UnitTests.SqlRepositoriesTests
{
    internal class TestData
    {
        public List<Game> Games { get; set; }
        public List<Genre> Genres { get; set; }
        public List<PlatformType> PlatformTypes { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Publisher> Publishers { get; set; }
        public List<Order> Orders { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }

        private List<GameGenre> _gameGenres { get; set; }
        private List<GamePlatformType> _gamePlatformType { get; set; }


        internal TestData(IGameStoreDBContext context)
        {
            Genres = SeedGenreData();
            context.Genres.AddRange(Genres);

            PlatformTypes = SeedPlatformTypeData();
            context.PlatformTypes.AddRange(PlatformTypes);

            Games = SeedGameData();
            context.Games.AddRange(Games);

            _gameGenres = SeedGameGenreData();
            context.GameGenres.AddRange(_gameGenres);

            _gamePlatformType = SeedGamePlatformTypeData();
            context.GamePlatformTypes.AddRange(_gamePlatformType);

            Comments = SeedCommentData();
            context.Comments.AddRange(Comments);

            Publishers = SeedPublisherData();
            context.Publishers.AddRange(Publishers);

            Orders = SeedOrderData();
            context.Orders.AddRange(Orders);

            OrderDetails = SeedOrderDetailData();
            context.OrderDetails.AddRange(OrderDetails);

            context.SaveChanges();
        }

        internal TestData()
        {
            Genres = SeedGenreData();

            PlatformTypes = SeedPlatformTypeData();

            Games = SeedGameData();

            _gameGenres = SeedGameGenreData();

            _gamePlatformType = SeedGamePlatformTypeData();

            Comments = SeedCommentData();

            Publishers = SeedPublisherData();

            Orders = SeedOrderData();

            OrderDetails = SeedOrderDetailData();

        }


        private List<Game> SeedGameData()
        {
            return new List<Game>()
            {
                new Game()
                {
                    Id = Guid.NewGuid(),
                    Name = "Sims 3",
                    Key = "sims3",
                    Description = "The Sims 3 is a life simulation video game developed by the Redwood Shores studio of Maxis and published by Electronic Arts.",
                    IsDeleted = false,
                    Price = 120,
                    Published = DateTime.UtcNow,
                    ViewsNumber = 1,
                     MongoGenreId = Guid.NewGuid(),
                      ObjectId = MongoDB.Bson.ObjectId.GenerateNewId(),
                      PublisherId = Guid.NewGuid(),
                      UnitsInStock = 1,
                      UnitsOnOrder = 1,
                      ReorderLevel = 1,
                      Discontinued= false
                },
                new Game()
                {
                    Id = Guid.NewGuid(),
                    Name = "Dark souls",
                    Key = "darksouls",
                    Description = "Dark Souls is a 2011 action role-playing game developed by FromSoftware and published by Namco Bandai Games.",
                    IsDeleted = false,
                    Price = 130,
                    Published = DateTime.UtcNow,
                    ViewsNumber = 1,
                     MongoGenreId = Guid.NewGuid(),
                      ObjectId = MongoDB.Bson.ObjectId.GenerateNewId(),
                      PublisherId = Guid.NewGuid(),
                      UnitsInStock = 1,
                      UnitsOnOrder = 1,
                      ReorderLevel = 1,
                      Discontinued= false
                }
            };
        }

        private List<Publisher> SeedPublisherData()
        {
            return new List<Publisher>()
            {
                new Publisher()
                {
                    Id = Guid.NewGuid(),
                    CompanyName = "EA Games",
                    Description = "Text about EA Games",
                    HomePage = "Home Page EA Games"
                },
                new Publisher()
                {
                    Id = Guid.NewGuid(),
                    CompanyName = "Bandai Namco Games",
                    Description = "Text about Bandai Namco Games",
                    HomePage = "Home Page Bandai Namco Games"
                }
            };
        }

        private List<Genre> SeedGenreData()
        {
            List<Genre> genres = new List<Genre>()
            {
                new Genre(){ Id = Guid.NewGuid(), Name = "Strategy" },
                new Genre(){ Id = Guid.NewGuid(), Name = "RPG" },
                new Genre(){ Id = Guid.NewGuid(), Name = "Sports" },
                new Genre(){ Id = Guid.NewGuid(), Name = "Races" },
                new Genre(){ Id = Guid.NewGuid(), Name = "Action" },
                new Genre(){ Id = Guid.NewGuid(), Name = "Adventure" },
                new Genre(){ Id = Guid.NewGuid(), Name = "Puzzle & Skill" },
                new Genre(){ Id = Guid.NewGuid(), Name = "Misc." }
            };

            List<Genre> subgenres = new List<Genre>()
            {
                new Genre() { Id = Guid.NewGuid(), Name = "RTS", ParentGenreId = genres[0].Id },
                new Genre() { Id = Guid.NewGuid(), Name = "TBS", ParentGenreId = genres[0].Id },
                new Genre() { Id = Guid.NewGuid(), Name = "Rally", ParentGenreId = genres[3].Id },
                new Genre() { Id = Guid.NewGuid(), Name = "Arcade", ParentGenreId = genres[3].Id },
                new Genre() { Id = Guid.NewGuid(), Name = "Formula", ParentGenreId = genres[3].Id },
                new Genre() { Id = Guid.NewGuid(), Name = "Off-road", ParentGenreId = genres[3].Id },
                new Genre() { Id = Guid.NewGuid(), Name = "FPS", ParentGenreId = genres[4].Id },
                new Genre() { Id = Guid.NewGuid(), Name = "TPS", ParentGenreId = genres[4].Id },
                new Genre() { Id = Guid.NewGuid(), Name = "Action Misc.", ParentGenreId = genres[4].Id
                }
            };

            List<Genre> genresToDB = new List<Genre>();
            genresToDB.AddRange(genres);
            genresToDB.AddRange(subgenres);

            return genresToDB;
        }
        private List<PlatformType> SeedPlatformTypeData()
        {
            return new List<PlatformType>()
            {
                new PlatformType(){ Id = Guid.NewGuid(), Type = "Mobile" },
                new PlatformType(){ Id = Guid.NewGuid(), Type = "Browser" },
                new PlatformType(){ Id = Guid.NewGuid(), Type = "Desktop" },
                new PlatformType(){ Id = Guid.NewGuid(), Type = "Console" },
            };
        }

        private List<Comment> SeedCommentData()
        {
            List<Comment> commentsToDb = new List<Comment>();

            List<Comment> comments = new List<Comment>()
            {
                new Comment()
                {
                    Id = Guid.NewGuid(),
                    Name = "Anastasiia",
                    Body = "Wow, cool game!",
                    GameId = Games[0].Id
                },
                new Comment()
                {
                    Id = Guid.NewGuid(),
                    Name = "Romeo",
                    Body = "The best game ever",
                    GameId = Games[1].Id
                }
            };

            commentsToDb.AddRange(comments);


            List<Comment> secondLevelComments = new List<Comment>()
                {
                    new Comment()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Maksym",
                        Body = "Did you play Sims 4? It's better",
                        ParentCommentId = commentsToDb[0].Id,
                        GameId = Games[0].Id
                    }
                };

            commentsToDb.AddRange(secondLevelComments);

            List<Comment> thirdLevelComments = new List<Comment>()
                {
                    new Comment()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Anna",
                        Body = "I think Sims 2 is the best in the collection",
                        ParentCommentId = commentsToDb[1].Id,
                        GameId = Games[0].Id
                    }
                };

            commentsToDb.AddRange(thirdLevelComments);

            return commentsToDb;

        }
        private List<GameGenre> SeedGameGenreData()
        {
            return new List<GameGenre>()
            {
                new GameGenre()
                {
                     GameId = Games[0].Id,
                     GenreId = Genres[7].Id
                },
                new GameGenre()
                {
                     GameId = Games[0].Id,
                     GenreId = Genres[2].Id
                },
                new GameGenre()
                {
                     GameId = Games[1].Id,
                     GenreId = Genres[2].Id
                },
                new GameGenre()
                {
                     GameId = Games[1].Id,
                     GenreId = Genres[14].Id
                }
            };
        }

        private List<GamePlatformType> SeedGamePlatformTypeData()
        {
            return new List<GamePlatformType>()
            {
                new GamePlatformType()
                {
                     GameId= Games[0].Id,
                     PlatformTypeId = PlatformTypes[0].Id,
                },
                 new GamePlatformType()
                {
                     GameId= Games[0].Id,
                     PlatformTypeId = PlatformTypes[3].Id,
                },
                  new GamePlatformType()
                {
                     GameId= Games[1].Id,
                     PlatformTypeId = PlatformTypes[3].Id,
                },
            };
        }

        private List<Order> SeedOrderData()
        {
            return new List<Order>()
           {
                new Order()
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.NewGuid(),

                },
                new Order()
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.NewGuid()
                }
           };
        }

        private List<OrderDetail> SeedOrderDetailData()
        {
            return new List<OrderDetail>()
            {
                new OrderDetail()
                {
                    Id = Guid.NewGuid(),
                    GameId = Guid.NewGuid(),
                    Discount = 0,
                    Price = 100,
                    Quantity = 1,
                    OrderId = Orders[0].Id

                },
                new OrderDetail()
                {
                    Id = Guid.NewGuid(),
                    GameId = Guid.NewGuid(),
                    Discount = 10,
                    Price = 100,
                    Quantity = 2,
                    OrderId = Orders[1].Id
                }
            };
        }
    }
}
