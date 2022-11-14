using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineGameStore.DAL.Entities;
using System;
using System.Collections.Generic;

namespace OnlineGameStore.DAL.Contex.EntitiesConfiguration
{
    internal class PublisherConfiguration : IEntityTypeConfiguration<Publisher>
    {
        public List<Publisher> Publishers { get; private set; }

        public PublisherConfiguration()
        {
            Publishers = new List<Publisher>();

            for (int i = 0; i < 10; i++)
            {
                Publishers.Add(
                    new Publisher()
                    {
                        Id = Guid.NewGuid(),
                        CompanyName = Faker.Company.Name(),
                        Description = Faker.Company.CatchPhrase(),
                        HomePage = Faker.Company.Name() + ".com"
                    }
            );
            }
        }
        public void Configure(EntityTypeBuilder<Publisher> builder)
        {
            builder.HasKey(publisher => publisher.Id);
            builder.Property(publisher => publisher.CompanyName).HasMaxLength(40);
            builder.HasIndex(publisher => publisher.CompanyName).IsUnique();
            builder.Property(publisher => publisher.HomePage).HasColumnType("Ntext");
            builder.Property(publisher => publisher.Description).HasColumnType("Ntext");

            builder.Property(publisher => publisher.Created).HasDefaultValue(DateTime.UtcNow);
            builder.Property(publisher => publisher.Updated).HasDefaultValue(DateTime.UtcNow);

            builder.Property(publisher => publisher.IsDeleted).IsRequired();
            builder.HasQueryFilter(publisher => !EF.Property<bool>(publisher, "IsDeleted"));
        }
    }
}
