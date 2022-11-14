using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineGameStore.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineGameStore.DAL.Context.EntitiesConfiguration
{
    internal class GameConfiguration : IEntityTypeConfiguration<Game>
    {
        public List<Game> Games { get; private set; }

        public GameConfiguration(List<Publisher> publishers = null)
        {
            Games = new List<Game>();
            DataSeedGames(publishers);
        }

        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder.HasKey(game => game.Id);
            builder.HasIndex(game => game.Key).IsUnique();
            builder.Property(game => game.Key).IsRequired();

            builder.Property(game => game.Name).HasMaxLength(100).IsRequired();
            builder.Property(game => game.Price).HasColumnType("Money").IsRequired();
            builder.Property(game => game.Description).HasMaxLength(1000).IsRequired();
            builder.Property(game => game.Discontinued).IsRequired();
            builder.Property(game => game.UnitsInStock).IsRequired();
            builder.Property(game => game.IsDeleted).IsRequired();
            builder.Property(game => game.Discount).HasDefaultValue(0);
            builder.Property(game => game.ViewsNumber).HasDefaultValue(0);
            builder.Property(game => game.Created).HasDefaultValue(DateTime.UtcNow);
            builder.Property(game => game.Updated).HasDefaultValue(DateTime.UtcNow);
        }

        public void DataSeedGames(List<Publisher> publishers)
        {
            for (int i = 0; i < 500; i++)
            {
                Guid id = Guid.NewGuid();
                string name = string.Join(' ', Faker.Lorem.Words(Faker.RandomNumber.Next(1, 5)));

                Games.Add(new Game()
                {
                    Id = id,
                    Key = name + '_' + id,
                    Name = name,
                    Discount = (short)Faker.RandomNumber.Next(0, 100),
                    Description = string.Join(' ', Faker.Lorem.Sentences(Faker.RandomNumber.Next(5, 15))),
                    Price = Math.Round((decimal)Faker.RandomNumber.Next(0, 500), 2),
                    UnitsInStock = (short)Faker.RandomNumber.Next(0, short.MaxValue),
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow,
                    DownloadPath = "Resources/Games/" + name + id.ToString(),
                    Published = Faker.Identification.DateOfBirth().ToUniversalTime(),
                    PublisherId = publishers != null && publishers.Any() ?
                        publishers[Faker.RandomNumber.Next(0, publishers.Count - 1)].Id :
                        (Guid?)null
                });
            }
        }
    }
}