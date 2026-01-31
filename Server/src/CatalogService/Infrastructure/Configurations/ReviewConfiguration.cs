using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public sealed class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("Reviews");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.UserId).IsRequired();
        builder.Property(r => r.ItemId).IsRequired();

        builder.Property(r => r.Rating).IsRequired();
        builder.Property(r => r.Description).HasMaxLength(2000);
        builder.Property(r => r.ImageUrl).HasMaxLength(2048);
        
        builder.HasOne<Item>()           
            .WithMany()                 
            .HasForeignKey(r => r.ItemId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex(r => new { r.ItemId, r.UserId }).IsUnique(); 
        builder.HasIndex(r => r.ItemId);
        
        builder.HasQueryFilter(i => !i.IsDeleted);
        
    }
}