using ServiceCenter.Domain.Entities.ReceiptAggregate;

namespace ServiceCenter.Infrastructure.Data.SqlServer.EfCore.EntityConfigurations.ReceiptAggregate;

public class ReceiptConfiguration : IEntityTypeConfiguration<Receipt>
{
    public void Configure(EntityTypeBuilder<Receipt> builder)
    {
        builder.Property(p => p.Imei).IsUnicode(false).HasMaxLength(15);

        builder.HasOne(p => p.User).WithMany(p => p.UserReceipts).HasForeignKey(p => p.UserId);
        builder.HasOne(p => p.Technician).WithMany(p => p.TechnicianReceipts).HasForeignKey(p => p.TechnicianId);
    }
}

