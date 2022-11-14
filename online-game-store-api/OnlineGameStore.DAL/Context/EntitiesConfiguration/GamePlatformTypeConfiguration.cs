using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineGameStore.DAL.Entities;
using System;
using System.Collections.Generic;

namespace OnlineGameStore.DAL.Context.EntitiesConfiguration
{
    internal class GamePlatformTypeConfiguration : IEntityTypeConfiguration<GamePlatformType>
    {
        public void Configure(EntityTypeBuilder<GamePlatformType> builder)
        {
            builder.HasKey(gpt => new { gpt.PlatformTypeId, gpt.GameId });

            builder.HasOne(gpt => gpt.Game)
                .WithMany(g => g.PlatformTypes)
                .HasForeignKey(gpt => gpt.GameId);

            builder.HasOne(gpt => gpt.PlatformType)
                .WithMany(pt => pt.Games)
                .HasForeignKey(gpt => gpt.PlatformTypeId);
        }

        public List<GamePlatformType> GeneratePlatforms(Guid gameId)
        {
            List<PlatformType> platforms = new PlatformTypeConfiguration().Platforms;
            List<GamePlatformType> result = new List<GamePlatformType>();

            for (int i = 0; i < Faker.RandomNumber.Next(0, platforms.Count - 1); i++)
            {
                result.Add(
                    new GamePlatformType()
                    {
                        GameId = gameId,
                        PlatformTypeId = platforms[i].Id
                    });
            }
            return result;
        }
    }
}
