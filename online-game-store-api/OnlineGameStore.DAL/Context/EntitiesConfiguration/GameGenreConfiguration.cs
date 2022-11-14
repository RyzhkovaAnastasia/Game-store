using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineGameStore.DAL.Entities;
using System;
using System.Collections.Generic;

namespace OnlineGameStore.DAL.Context.EntitiesConfiguration
{
    internal class GameGenreConfiguration : IEntityTypeConfiguration<GameGenre>
    {
        public void Configure(EntityTypeBuilder<GameGenre> builder)
        {
            builder.HasKey(gg => new { gg.GenreId, gg.GameId });

            builder.HasOne(gg => gg.Genre)
                .WithMany(g => g.Games)
                .HasForeignKey(gg => gg.GenreId);

            builder.HasOne(gg => gg.Game)
                .WithMany(game => game.Genres)
                .HasForeignKey(gg => gg.GameId);
        }

        public List<GameGenre> GenerateGenres(Guid gameId)
        {
            List<Genre> genres = new GenreConfiguration().Genres;
            List<GameGenre> result = new List<GameGenre>();

            for (int i = 0; i < Faker.RandomNumber.Next(0, 5); i++)
            {
                result.Add(
                    new GameGenre()
                    {
                        GameId = gameId,
                        GenreId = genres[i].Id
                    });
            }
            return result;
        }
    }
}
