using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineGameStore.DAL.Entities;
using System;
using System.Collections.Generic;

namespace OnlineGameStore.DAL.Context.EntitiesConfiguration
{
    internal class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(comment => comment.Id);
            builder.Property(comment => comment.Name).HasMaxLength(100).IsRequired();
            builder.Property(comment => comment.Body).HasMaxLength(1000).IsRequired();

            builder.Property(comment => comment.IsQuoted).HasDefaultValue(false);

            builder.Property(comment => comment.IsDeleted).IsRequired();

            builder.Property(comment => comment.Created).HasDefaultValue(DateTime.UtcNow);
            builder.Property(comment => comment.Updated).HasDefaultValue(DateTime.UtcNow);

            builder.HasOne(comment => comment.Game).WithMany(game => game.Comments).IsRequired();

            builder.HasOne(c => c.ParentComment)
                .WithMany(c => c.ChildComments)
                .HasForeignKey(c => c.ParentCommentId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(c => c.ChildComments)
                .WithOne(c => c.ParentComment)
                .HasForeignKey(c => c.ParentCommentId)
                .OnDelete(DeleteBehavior.NoAction);
        }
        public IEnumerable<Comment> GenerateComments(Guid gameId)
        {
            List<Comment> comments = new List<Comment>();
            for (int j = 0; j < Faker.RandomNumber.Next(0, 100); j++)
            {
                comments.Add(
                    new Comment()
                    {
                        Id = Guid.NewGuid(),
                        GameId = gameId,
                        Body = Faker.Lorem.Paragraph(1),
                        Name = Faker.Name.First(),
                        IsDeleted = Faker.RandomNumber.Next(0, 1) != 0
                    });
            }
            return comments;
        }
    }
}
