using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.TokenHash)
               .IsRequired()
               .HasMaxLength(128);

        builder.Property(x => x.UserIdCode)
               .IsRequired()
               .HasMaxLength(15);

        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.ExpiresAt).IsRequired();

        builder.Property(x => x.RevokedReason).HasMaxLength(50);
        builder.Property(x => x.CreatedByIp).HasMaxLength(45);
        builder.Property(x => x.UserAgent).HasMaxLength(500);

        builder.HasIndex(x => x.TokenHash).IsUnique()
               .HasDatabaseName("IX_RefreshToken_TokenHash");

        builder.HasIndex(x => x.UserIdCode)
               .HasDatabaseName("IX_RefreshToken_UserIdCode");

        builder.HasIndex(x => x.ExpiresAt)
               .HasDatabaseName("IX_RefreshToken_ExpiresAt");
    }
}
