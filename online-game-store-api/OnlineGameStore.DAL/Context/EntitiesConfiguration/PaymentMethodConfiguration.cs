using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineGameStore.DAL.Entities;
using System;

namespace OnlineGameStore.DAL.Context.EntitiesConfiguration
{
    internal class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
    {
        public void Configure(EntityTypeBuilder<PaymentMethod> builder)
        {
            builder.HasKey(payment => payment.Id);
            builder.Property(payment => payment.Title).IsRequired();

            builder.Property(payment => payment.Created).HasDefaultValue(DateTime.UtcNow);
            builder.Property(payment => payment.Updated).HasDefaultValue(DateTime.UtcNow);

            DataSeedGenres(builder);
        }

        private void DataSeedGenres(EntityTypeBuilder<PaymentMethod> builder)
        {
            PaymentMethod[] paymentMethods = new[]
            {
                new PaymentMethod()
                {
                    Id = Guid.Parse("628b16d3-c912-4fea-825a-4d057f5b4102"),
                    Title = "Bank",
                    Description = "Generate invoice file",
                    ImageFileName = "bank.png"
                },
                new PaymentMethod()
                {
                    Id = Guid.Parse("dcb677de-e6dd-475b-bbab-b1405ea5dfb0"),
                    Title = "IBox",
                    Description = "IBox account payment",
                    ImageFileName = "ibox.png"
                },
                new PaymentMethod()
                {
                    Id = Guid.Parse("da9afe30-e3cf-46bc-a633-e68022f6a3d9"),
                    Title = "Visa",
                    Description = "Pay by Visa credit card",
                    ImageFileName = "visa.png"
                }
            };

            builder.HasData(paymentMethods);
        }
    }
}
