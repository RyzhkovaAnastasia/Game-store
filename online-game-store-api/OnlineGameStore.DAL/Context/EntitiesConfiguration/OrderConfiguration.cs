using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineGameStore.DAL.Entities;
using System;

namespace OnlineGameStore.DAL.Context.EntitiesConfiguration
{
    internal class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(order => order.Id);
            builder.Property(order => order.UserId).IsRequired();

            builder.Property(order => order.Created).HasDefaultValue(DateTime.UtcNow);
            builder.Property(order => order.Updated).HasDefaultValue(DateTime.UtcNow);

            builder.Property(order => order.IsDeleted).IsRequired();
        }
    }
}
