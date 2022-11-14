using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineGameStore.DAL.Entities;
using System;
using System.Collections.Generic;

namespace OnlineGameStore.DAL.Context.EntitiesConfiguration
{
    internal class GenreConfiguration : IEntityTypeConfiguration<Genre>
    {
        public List<Genre> Genres { get; private set; }

        public GenreConfiguration()
        {
            Genres = new List<Genre>()
            {
                new Genre(){ Id = Guid.Parse("0a6e3b76-0593-41e9-9cae-b5c83a8875fd"), Name = "Strategy" },
                new Genre(){ Id = Guid.Parse("d2dfac8b-401c-47a5-b14d-7661aa9998fd"), Name = "RPG" },
                new Genre(){ Id = Guid.Parse("ed9f6bb2-a3e2-4a9f-859b-232f73bb5a58"), Name = "Sports" },
                new Genre(){ Id = Guid.Parse("b626f167-b5b7-40e0-bfb6-ec384900692f"), Name = "Races" },
                new Genre(){ Id = Guid.Parse("96414ac9-7a52-426a-ae38-a7c5cba3b9b5"), Name = "Action" },
                new Genre(){ Id = Guid.Parse("bf7f9936-e273-4b00-8175-15cdf5ecc91d"), Name = "Adventure" },
                new Genre(){ Id = Guid.Parse("f7d47345-68ab-46bb-822f-dc2458bdc383"), Name = "Puzzle & Skill" },
                new Genre(){ Id = Guid.Parse("a9747b67-f696-4557-bffa-2ffba702bda8"), Name = "Misc." }
            };

            Genres.AddRange(
                new List<Genre>()
                {
                    new Genre() { Id = Guid.Parse("2e412965-bb18-4921-87b8-2a36795c7292"), Name = "RTS", ParentGenreId = Genres[0].Id },
                    new Genre() { Id = Guid.Parse("df8cd2c6-9011-4982-9222-588a9c5aeb7d"), Name = "TBS", ParentGenreId = Genres[0].Id },
                    new Genre() { Id = Guid.Parse("e4fc1c2f-1d85-47ae-94d0-1c2f0d628d43"), Name = "Rally", ParentGenreId = Genres[3].Id },
                    new Genre() { Id = Guid.Parse("4b19b49c-2be3-4beb-85aa-bb1cc4c24242"), Name = "Arcade", ParentGenreId = Genres[3].Id },
                    new Genre() { Id = Guid.Parse("5e4ab5e4-8670-4f67-8aa3-ab0c88b8635f"), Name = "Formula", ParentGenreId = Genres[3].Id },
                    new Genre() { Id = Guid.Parse("21be8e70-d1de-4a83-ae94-0283007514f5"), Name = "Off-road", ParentGenreId = Genres[3].Id },
                    new Genre() { Id = Guid.Parse("c28317b2-0db9-4e67-a52f-aadd30909ddb"), Name = "FPS", ParentGenreId = Genres[4].Id },
                    new Genre() { Id = Guid.Parse("46ab3872-2603-4a7a-9c05-061e4f7356f1"), Name = "TPS", ParentGenreId = Genres[4].Id },
                    new Genre() { Id = Guid.Parse("41e4a02a-5585-463b-89eb-6d9c4f3c2617"), Name = "Action Misc.", ParentGenreId = Genres[4].Id }
                });
        }

        public void Configure(EntityTypeBuilder<Genre> builder)
        {
            builder.HasKey(genre => genre.Id);
            builder.HasIndex(genre => genre.Name).IsUnique();
            builder.Property(genre => genre.Name).HasMaxLength(20).IsRequired();

            builder.Property(genre => genre.IsDeleted).IsRequired();
            builder.Property(genre => genre.Created).HasDefaultValue(DateTime.UtcNow);
            builder.Property(genre => genre.Updated).HasDefaultValue(DateTime.UtcNow);


            builder.HasQueryFilter(genre => !EF.Property<bool>(genre, "IsDeleted"));

            builder
                .HasOne(genre => genre.ParentGenre)
                .WithMany(genre => genre.ChildGenres)
                .HasForeignKey(genre => genre.ParentGenreId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasData(Genres);
        }
    }
}
