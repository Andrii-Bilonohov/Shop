using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.ToTable("Items");
            builder.HasKey(i => i.Id);

            builder.Property(i => i.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(i => i.Description)
                .IsRequired(false)
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
                .IsRequired(false);

            builder.Property(x => x.CreatedAt)
                .HasDefaultValueSql("NOW()");

            builder.Property(x => x.UpdatedAt)
                .HasDefaultValueSql("NOW()");

            builder.HasQueryFilter(i => !i.IsDeleted);
            
            builder.HasMany(i => i.Reviews)
                .WithOne(r => r.Item)
                .HasForeignKey(r => r.ItemId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
