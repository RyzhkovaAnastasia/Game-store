using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineGameStore.DAL.Entities;
using System;
using System.Collections.Generic;

namespace OnlineGameStore.DAL.Context.EntitiesConfiguration
{
    internal class PlatformTypeConfiguration : IEntityTypeConfiguration<PlatformType>
    {
        public List<PlatformType> Platforms { get; private set; }

        public PlatformTypeConfiguration()
        {
            Platforms = new List<PlatformType>()
           {
                new PlatformType(){ Id = Guid.Parse("6e2274ee-c3bf-4b32-aca3-1fd9c985c673"), Type = "Mobile" },
                new PlatformType(){ Id = Guid.Parse("8072e5f8-1100-4282-8605-4520a1ae695a"), Type = "Browser" },
                new PlatformType(){ Id = Guid.Parse("ee772589-43fb-428c-a570-26de3350f2e3"), Type = "Desktop" },
                new PlatformType(){ Id = Guid.Parse("e96a7f0f-9512-475c-a191-03aa7e3ad013"), Type = "Console" },
            };
        }
        public void Configure(EntityTypeBuilder<PlatformType> builder)
        {
            builder.HasKey(platform => platform.Id);
            builder.HasIndex(platform => platform.Type).IsUnique();
            builder.Property(platform => platform.Type).IsRequired();

            builder.Property(platform => platform.Created).HasDefaultValue(DateTime.UtcNow);
            builder.Property(platform => platform.Updated).HasDefaultValue(DateTime.UtcNow);

            builder.Property(platform => platform.IsDeleted).IsRequired();
            builder.HasQueryFilter(platform => !EF.Property<bool>(platform, "IsDeleted"));

            builder.HasData(Platforms);
        }
    }
}
