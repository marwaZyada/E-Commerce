using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregation;

namespace Talabat.Repository.Data.Configurations
{
    public class OrderConfigration : IEntityTypeConfiguration<Order>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(o => o.ShippingAddress, sh => sh.WithOwner());
            builder.Property(o => o.Status)
                .HasConversion(
                os => os.ToString(),
                os => (OrderStatus)Enum.Parse(typeof(OrderStatus), os)) ;
            builder.Property(o => o.SubTotal).HasColumnType("decimal(18,2)");
            builder.HasOne(o=>o.DeliveryMethod).WithMany().OnDelete(DeleteBehavior.SetNull) ;
            builder.HasMany(o => o.OrderItems)
                .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
