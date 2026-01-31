using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.HasKey(i => i.Id);

            builder.Property(i => i.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(i => i.Description)
                .HasMaxLength(1000);

            builder.Property(i => i.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(i => i.Stock)
                .IsRequired();

            builder.Property(i => i.Category)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(i => i.ImageUrl)
                .IsRequired();

            builder.Property(i => i.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            builder.HasQueryFilter(i => !i.IsDeleted);

            builder.Property(i => i.UpdatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");
            
            builder.HasMany(i => i.Reviews)
                .WithOne(r => r.Item)
                .HasForeignKey(r => r.ItemId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
