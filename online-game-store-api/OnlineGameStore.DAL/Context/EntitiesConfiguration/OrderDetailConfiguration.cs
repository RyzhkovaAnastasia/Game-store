using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineGameStore.DAL.Entities;

namespace OnlineGameStore.DAL.Context.EntitiesConfiguration
{
    internal class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.HasKey(item => item.Id);
            builder.Property(item => item.GameId).IsRequired();
            builder.Property(item => item.Price).HasColumnType("Money").IsRequired();
            //builder
            //    .HasOne(orderDetail => orderDetail.Game)
            //    .WithMany(game => game.OrderDetails)
            //    .HasForeignKey(order => order.GameId)
            //    .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
