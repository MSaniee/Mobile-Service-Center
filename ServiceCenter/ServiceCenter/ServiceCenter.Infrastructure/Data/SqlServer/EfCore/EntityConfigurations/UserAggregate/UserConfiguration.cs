using ServiceCenter.Domain.Entities.UserAggregate;

namespace ServiceCenter.Infrastructure.Data.SqlServer.EfCore.EntityConfigurations.UserAggregate;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {       
        builder.Property(p => p.FullName).HasMaxLength(400);
        builder.Property(p => p.NationalCode).IsUnicode(false).HasMaxLength(50);

        builder.HasMany(p => p.UserRefreshTokens).WithOne(p => p.User).HasForeignKey(p => p.UserId);
    }
}
