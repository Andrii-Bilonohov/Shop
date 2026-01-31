using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.FirstName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(u => u.LastName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(u => u.Email)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.HasIndex(u => u.Email).IsUnique();

            builder.Property(u => u.Provider)
                   .HasConversion<string>()
                   .IsRequired();

            builder.Property(u => u.ProviderId)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.HasIndex(u => u.ProviderId).IsUnique();

            builder.Property(u => u.Role)
                   .HasConversion<string>()
                   .IsRequired();

            builder.Property(u => u.AvatarUrl)
                   .HasMaxLength(500);

            builder.Property(u => u.IsDeleted)
                   .HasDefaultValue(false);

            builder.HasQueryFilter(u => !u.IsDeleted);
        }
    }
}
