using Microsoft.EntityFrameworkCore;
using Order.Domain.Entities;
using OrderEntity = Order.Domain.Entities.Order;

namespace Order.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }

        public required DbSet<OrderEntity> Orders { get; set; }
        public required DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderEntity>(order =>
            {
                order.ToTable("Orders");

                order.HasKey(o => o.Id);

                order.Property(o => o.Id)
                     .ValueGeneratedNever();

                order.Property(o => o.CreatedAt)
                     .IsRequired();

                order.Property(o => o.Status)
                     .IsRequired()
                     .HasConversion<string>();

                order.OwnsMany(o => o.Items, item =>
                {
                    item.ToTable("OrderItems");

                    item.WithOwner()
                        .HasForeignKey("OrderId");

                    item.Property<Guid>("Id");
                    item.HasKey("Id");

                    item.Property(i => i.ProductId).IsRequired();
                    item.Property(i => i.Quantity).IsRequired();
                });

                order.Navigation(o => o.Items)
                     .UsePropertyAccessMode(PropertyAccessMode.Field);
            });
        }
    }
}
